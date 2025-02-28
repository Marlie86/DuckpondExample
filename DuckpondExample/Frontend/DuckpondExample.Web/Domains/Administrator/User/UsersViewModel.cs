using Duckpond.Aspire.Web.Models.StateModels;

using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Web.ApiClients;
using DuckpondExample.Web.Domains.User;
using DuckpondExample.Web.Enums;
using DuckpondExample.Web.Models;
using DuckpondExample.Web.UtilityServices;

using MudBlazor;

namespace DuckpondExample.Web.Domains.Administrator.User;

/// <summary>
/// ViewModel for managing users in the administrator domain.
/// </summary>
/// <remarks>
/// This ViewModel interacts with the Users API client to fetch and manage user data.
/// </remarks>
[AddAsService(ServiceLifetime.Scoped)]
public class UsersViewModel : BaseViewModel
{
    private readonly ILogger<UserEditViewModel> logger;
    private readonly StorageService localStorage;
    private readonly UserApiClient userApiClient;
    private readonly UsersGroupsApiClient usersGroupsApiClient;
    private readonly IDialogService dialogService;
    private readonly UserStateModel userStateModel;
    private readonly ISnackbar snackbar;

    /// <summary>
    /// Gets or sets the collection of users.
    /// </summary>
    public IEnumerable<UserModel> Users { get; set; } = new List<UserModel>();

    /// <summary>
    /// Gets the ID of the user.
    /// </summary>
    public int UserID { get; private set; }

    /// <summary>
    /// Gets or sets the search string.
    /// </summary>
    public string SearchString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    public string Height { get; set; } = "100";

    /// <summary>
    /// Gets or sets a value indicating whether the ViewModel is loading.
    /// </summary>
    public bool Loading { get; set; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersViewModel"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="localStorage">The local storage service instance.</param>
    /// <param name="userApiClient">The API client for users operations.</param>
    /// <param name="usersGroupsApiClient">The API client for users groups operations.</param>
    /// <param name="dialogService">The dialog service instance.</param>
    /// <param name="userStateModel">The user state model instance.</param>
    public UsersViewModel(ILogger<UserEditViewModel> logger, StorageService localStorage, UserApiClient userApiClient, UsersGroupsApiClient usersGroupsApiClient, IDialogService dialogService, UserStateModel userStateModel, ISnackbar snackbar)
    {
        this.logger = logger;
        this.localStorage = localStorage;
        this.userApiClient = userApiClient;
        this.usersGroupsApiClient = usersGroupsApiClient;
        this.dialogService = dialogService;
        this.userStateModel = userStateModel;
        this.snackbar = snackbar;
    }

    /// <summary>
    /// Initializes the ViewModel asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public override async Task InitializeAsync()
    {
        Loading = true;
        logger.LogInformation("Initializing UsersViewModel...");
        var claims = await localStorage.GetItemAsync<Dictionary<string, string>>("UserClaims");
        if (claims != null && claims.TryGetValue("UserID", out var userId))
        {
            if (!string.IsNullOrEmpty(userId))
            {
                UserID = Convert.ToInt32(userId);
            }
        }

        var usersResult = await userApiClient.GetUsers();
        Users = usersResult.Users;
        Loading = false;
        RaisePropertyChanged();
        await base.InitializeAsync();
    }

    /// <summary>
    /// Searches for a user based on the search string.
    /// </summary>
    /// <param name="model">The user model to search.</param>
    /// <returns>True if the user matches the search string; otherwise, false.</returns>
    public bool Search(UserModel model)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
        {
            return true;
        }

        if ($"{model.Username}{model.EMailAddress}".Contains(SearchString))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Opens the edit user dialog for the specified user.
    /// </summary>
    /// <param name="user">The user to edit.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task OpenEditUserDialog(UserModel user)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large };
        var dialogParameters = new DialogParameters()
        {
            { "User", user },
            { "Mode", DialogModeEnum.Edit }
        };

        var dialog = await dialogService.ShowAsync<UserEditDialog>("Anwender bearbeiten", dialogParameters, options);
        var dialogResult = await dialog.Result;
        if (dialogResult == null || dialogResult.Canceled)
        {
            await InitializeAsync();
            return;
        }

        var resultData = dialogResult.Data as bool?;
        if (resultData == true)
        {
            await InitializeAsync();
            logger.LogInformation("User updated successfully.");
            return;
        }
        await InitializeAsync();
        logger.LogInformation("Error updating user.");
    }

    /// <summary>
    /// Opens the create user dialog.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task OpenCreateUserDialog()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large };
        var dialogParameters = new DialogParameters()
        {
            { "User", new UserModel() },
            { "Mode", DialogModeEnum.Create }
        };

        var dialog = await dialogService.ShowAsync<UserEditDialog>("Anwender hinzufügen", dialogParameters, options);
        var dialogResult = await dialog.Result;
        if (dialogResult == null || dialogResult.Canceled)
        {
            return;
        }

        var resultData = dialogResult.Data as bool?;
        if (resultData == true)
        {
            logger.LogInformation("User created successfully.");
            await InitializeAsync();
            return;
        }
        await InitializeAsync();
        logger.LogInformation("Error creation user.");
    }

    public async Task RemoveUser(UserModel user)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small };
        var dialogParameters = new DialogParameters()
        {
            { "User", user }
        };
        var dialogResult = await dialogService.ShowMessageBox("Anwender löschen", $"Möchten Sie den Anwender '{user.Username}' wirklich löschen?", yesText: "Ja", noText: "Nein");
        if (!dialogResult.GetValueOrDefault(false))
        {
            await InitializeAsync();
            return;
        }
        else 
        {
            var result = await userApiClient.DeleteUser(new DeleteUserCommand() { UserId = user.UserID });
            if (result.IsSuccess && result.IsDeleted)
            {
                snackbar.Add($"Anwender '{user.Username}' erfolgreich gelöscht.", Severity.Success);
                await InitializeAsync();
                return;
            }
            snackbar.Add($"Fehler beim Löschen des Anwenders '{user.Username}'.", Severity.Error);
            await InitializeAsync();
            return;
        }
    }
}
