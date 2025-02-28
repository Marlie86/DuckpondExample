using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Web.ApiClients;
using DuckpondExample.Web.Enums;
using DuckpondExample.Web.Models;

using MudBlazor;

namespace DuckpondExample.Web.Domains.Administrator.Groups;

[AddAsService(ServiceLifetime.Scoped)]
public class GroupEditDialogViewModel : BaseViewModel
{
    private readonly ILogger<GroupEditDialogViewModel> logger;
    private readonly GroupsApiClient groupsApiClient;
    private readonly ISnackbar snackbar;

    /// <summary>
    /// Gets or sets the collection of groups.
    /// </summary>
    public Group Group { get; set; }
    public List<GroupMembersRead> ChangedGroupMembers { get; set; } = [];
    public List<GroupPermissionRead> ChangedGroupPermissions { get; set; } = [];

    public IMudDialogInstance MudDialog { get; set; }
    public DialogModeEnum Mode { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEditDialogViewModel"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging information.</param>
    /// <param name="groupsApiClient">The API client for managing groups and their members.</param>
    /// <param name="snackbar">The snackbar service for displaying notifications.</param>
    public GroupEditDialogViewModel(ILogger<GroupEditDialogViewModel> logger, GroupsApiClient groupsApiClient, ISnackbar snackbar)
    {
        this.logger = logger;
        this.groupsApiClient = groupsApiClient;
        this.snackbar = snackbar;
    }

    /// <summary>
    /// Submits the dialog and closes it with a positive result.
    /// </summary>
    public async Task Submit()
    {
        if (Mode == DialogModeEnum.Create)
        {
            await CreateGroup();
        }
        else
        {
            await UpdateGroup();
        }
    }
    private async Task CreateGroup()
    {
        logger.LogInformation($"Creating group {Group.Name}.");
        var command = new CreateGroupWithMembersAndPermissionsCommand()
        {
            Group = Group,
            ChangedMembers = ChangedGroupMembers,
            ChangedPermissions = ChangedGroupPermissions
        };

        var requestResult = await groupsApiClient.CreateGroupWithMembersAndPermissions(command);
        if (requestResult.IsSuccess)
        {
            logger.LogInformation($"Group {Group.Name} created.");
            ChangedGroupMembers.Clear();
            ChangedGroupPermissions.Clear();
            snackbar.Add($"Group {Group.Name} created.", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
            return;
        }

        logger.LogWarning($"Error creating group. {string.Join("; ", requestResult.ErrorMessages)}");
        snackbar.Add($"Error creating group. {string.Join("; ", requestResult.ErrorMessages)}", Severity.Warning);
        return;
    }

    private async Task UpdateGroup()
    {
        logger.LogInformation($"Updating group {Group.Name}.");
        var command = new UpdateGroupWithMembersAndPermissionsCommand()
        {
            Group = Group,
            ChangedMembers = ChangedGroupMembers,
            ChangedPermissions = ChangedGroupPermissions
        };

        var requestResult = await groupsApiClient.UpdateGroupPermissionsAsync(command);
        if (requestResult.IsSuccess)
        {
            logger.LogInformation($"Group {Group.Name} updated.");
            ChangedGroupMembers.Clear();
            ChangedGroupPermissions.Clear();
            snackbar.Add($"Group {Group.Name} updated.", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
            return;
        }

        logger.LogWarning($"Error updating group. {string.Join("; ", requestResult.ErrorMessages)}");
        snackbar.Add($"Error updating group. {string.Join("; ", requestResult.ErrorMessages)}", Severity.Warning);
        return;
    }

    /// <summary>
    /// Cancels the dialog and closes it with a cancel result.
    /// </summary>
    public async Task Cancel()
    {
        MudDialog.Close(DialogResult.Cancel());
    }
}
