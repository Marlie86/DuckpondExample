using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Web.ApiClients;
using DuckpondExample.Web.Enums;
using DuckpondExample.Web.Models;
using DuckpondExample.Web.Validators;

using FluentValidation;

using MudBlazor;

using Severity = MudBlazor.Severity;

namespace DuckpondExample.Web.Domains.Administrator.User;

/// <summary>
/// ViewModel for handling the logic of the User Edit Dialog.
/// </summary>
[AddAsService(ServiceLifetime.Transient)]
public class UserEditDialogViewModel : BaseViewModel
{
    private readonly ILogger<UserEditDialogViewModel> logger;
    private readonly UserApiClient userApiClient;
    private readonly UsersGroupsApiClient usersGroupsApiClient;
    private readonly UserValidator validationRules;
    private readonly ISnackbar snackbar;
    private IEnumerable<UserGroupRead> groups;

    public IMudDialogInstance MudDialog { get; set; }

    public UserModel User { get; set; }
    public IEnumerable<UserGroupRead> Groups { get => groups; set { groups = value; RaisePropertyChanged(); } }
    public DialogModeEnum Mode { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="UserEditDialogViewModel"/> class.
    /// </summary>
    /// <param name="logger">The logger instance to use for logging.</param>
    public UserEditDialogViewModel(ILogger<UserEditDialogViewModel> logger, UserApiClient userApiClient, UsersGroupsApiClient usersGroupsApiClient, UserValidator validationRules, ISnackbar snackbar)
    {
        this.logger = logger;
        this.userApiClient = userApiClient;
        this.usersGroupsApiClient = usersGroupsApiClient;
        this.validationRules = validationRules;
        this.snackbar = snackbar;
    }

    /// <summary>
    /// Initializes the ViewModel by fetching the users groups for the specified user.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public override async Task InitializeAsync()
    {
        var getUsersGroupsResult = await usersGroupsApiClient.GetUserGroups(User.UserID);
        if (!getUsersGroupsResult.IsSuccess)
        {
            logger.LogWarning("Error fetching users groups.");
        }
        Groups = getUsersGroupsResult.UserGroups;
    }



    /// <summary>
    /// Creates a new user and updates their groups.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task CreateUser()
    {
        var result = await userApiClient.CreateUser(new CreateUserCommand() { User = User, Groups = Groups });
        if (!result.IsSuccess)
        {
            logger.LogWarning("Error creating user.");
            snackbar.Add(string.Join(", ", result.ErrorMessages), Severity.Error);
            //MudDialog.Close(DialogResult.Cancel());
            return;
        }

        snackbar.Add($"User '{User.Username}' created.", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    /// <summary>
    /// Updates the user's details and their associated groups.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateUser()
    {
        var result = await userApiClient.UpdateUser(new UpdateUserCommand() { User = User, Groups = Groups });
        if (!result.IsSuccess)
        {
            logger.LogWarning("Error updating user.");
            snackbar.Add(string.Join(", ", result.ErrorMessages), Severity.Error);
            //MudDialog.Close(DialogResult.Cancel());
            return;
        }

        var groupsResult = await usersGroupsApiClient.UpdateUserGroups(User, Groups);
        if (!groupsResult.IsSuccess)
        {
            logger.LogWarning("Error updating user.");
            snackbar.Add(string.Join(", ", groupsResult.ErrorMessages), Severity.Error);
            return;
        }

        snackbar.Add($"User '{User.Username}' updated.", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }
    /// <summary>
    /// Submits the dialog and closes it with a positive result.
    /// </summary>
    public async Task Submit()
    {
        var validationResult = await validationRules.ValidateAsync(User);
        if (!validationResult.IsValid)
        {
            snackbar.Add(string.Join(", ", validationResult.Errors.Select(err => err.ErrorMessage)), Severity.Error);
            return;
        }

        if (Mode == DialogModeEnum.Create)
        {
            await CreateUser();
        }
        else
        {
            await UpdateUser();
        }
    }
    /// <summary>
    /// Cancels the dialog and closes it with a cancel result.
    /// </summary>
    public async Task Cancel()
    {
        MudDialog.Close(DialogResult.Cancel());
    }
}
