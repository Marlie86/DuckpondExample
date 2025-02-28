using Blazored.LocalStorage;

using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Web.ApiClients;
using DuckpondExample.Web.Models;
using DuckpondExample.Web.UtilityServices;
using DuckpondExample.Web.Validators;

using FluentValidation;

using MudBlazor;

namespace DuckpondExample.Web.Domains.User;

/// <summary>
/// ViewModel for editing user information.
/// </summary>
[AddAsService(ServiceLifetime.Scoped)]
public class UserEditViewModel : BaseViewModel
{
    private readonly ILogger<UserEditViewModel> logger;
    private readonly ILocalStorageService localStorage;
    private readonly UserApiClient userApiClient;

    /// <summary>
    /// Gets or sets the user model.
    /// </summary>
    public UserModel User { get; set; } = new UserModel();

    /// <summary>
    /// Gets the validator for the user model.
    /// </summary>
    public UserValidator Validator { get; }

    /// <summary>
    /// Gets the snackbar service for displaying messages.
    /// </summary>
    public ISnackbar Snackbar { get; }

    /// <summary>
    /// Gets or sets the form for validation.
    /// </summary>
    public MudForm Form { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserEditViewModel"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="localStorage">The local storage service.</param>
    /// <param name="userApiClient">The API client for user operations.</param>
    /// <param name="validator">The validator for the user model.</param>
    /// <param name="snackbar">The snackbar service for displaying messages.</param>
    public UserEditViewModel(ILogger<UserEditViewModel> logger, StorageService localStorage, UserApiClient userApiClient, UserValidator validator, ISnackbar snackbar)
    {
        this.logger = logger;
        this.localStorage = localStorage;
        this.userApiClient = userApiClient;
        Validator = validator;
        Snackbar = snackbar;
    }

    /// <summary>
    /// Initializes the ViewModel asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InitializeAsync()
    {
        var userClaims = await localStorage.GetItemAsync<Dictionary<string, string>>("UserClaims");
        var userId = userClaims?["UserID"] ?? string.Empty;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogError("UserEditViewModel.InitializeAsync: UserId is missing from UserClaims");
            return;
        }

        var result = await userApiClient.GetUserByIdAsync(new GetUserByIdCommand() { UserID = Convert.ToInt32(userId) });
        if (result.IsSuccess && result.User != null)
        {
            User = result.User;
            RaisePropertyChanged(nameof(User));
        }
        else
        {
            logger.LogError("UserEditViewModel.InitializeAsync: Error getting user data");
        }
    }

    /// <summary>
    /// Tries to save the user model asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task TrySaveAsync()
    {
        var validationResult = await Validator.ValidateAsync(User);

        if (validationResult.IsValid)
        {
            var result = await userApiClient.UpdateUser(new UpdateUserCommand() { User = User });
            if (result.IsSuccess)
            {
                Snackbar.Add("User data saved", MudBlazor.Severity.Success);
            }
            else
            {
                Snackbar.Add("Error saving user data", MudBlazor.Severity.Error);
            }
        }
        else
        {
            await Form.Validate();
        }
    }
}
