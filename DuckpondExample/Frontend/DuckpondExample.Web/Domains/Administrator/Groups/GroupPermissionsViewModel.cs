using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Web.ApiClients;
using DuckpondExample.Web.Models;

using MudBlazor;

namespace DuckpondExample.Web.Domains.Administrator.Groups;

/// <summary>
/// ViewModel for managing group permissions in the administrator domain.
/// </summary>
/// <remarks>
/// This ViewModel handles the loading, searching, and selection of group permissions.
/// </remarks>
[AddAsService(ServiceLifetime.Transient)]
public class GroupPermissionsViewModel : BaseViewModel
{
    private readonly ILogger<GroupsViewModel> logger;
    private readonly GroupsApiClient groupsApiClient;
    private readonly ISnackbar snackbar;
    private Group group = new Group();

    /// <summary>
    /// Gets or sets the search string for filtering group permissions.
    /// </summary>
    public string SearchString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the group for which permissions are being managed.
    /// </summary>
    public Group Group { get => group; set { if (group == value) { return; } group = value; RaisePropertyChanged(); } }

    /// <summary>
    /// Gets or sets a value indicating whether the permissions are currently being loaded.
    /// </summary>
    public bool IsLoading { get; set; } = false;

    /// <summary>
    /// Gets or sets the collection of group permissions.
    /// </summary>
    public IEnumerable<GroupPermissionRead>? Permissions { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of changed group permissions.
    /// </summary>
    public List<GroupPermissionRead>? ChangedMembers { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of selected group permissions.
    /// </summary>
    public HashSet<GroupPermissionRead> SelectedMembers { get; set; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupPermissionsViewModel"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="groupsApiClient">The API client for fetching group permissions.</param>
    /// <param name="snackbar">The snackbar service for displaying notifications.</param>
    public GroupPermissionsViewModel(ILogger<GroupsViewModel> logger, GroupsApiClient groupsApiClient, ISnackbar snackbar)
    {
        this.logger = logger;
        this.groupsApiClient = groupsApiClient;
        this.snackbar = snackbar;
    }

    /// <summary>
    /// Initializes the ViewModel asynchronously by loading the group permissions.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await LoadGroupPermissions();
    }

    /// <summary>
    /// Loads the group permissions asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task LoadGroupPermissions()
    {
        logger.LogInformation($"Fetching permissions for group {Group?.Name}.");
        IsLoading = true;
        if (Group == null)
        {
            logger.LogWarning("Group is null.");
            snackbar.Add("Group is null.", Severity.Warning);
            return;
        }

        var apiCallResult = await groupsApiClient.GetGroupPermissionsAsync(new GetGroupPermissionsCommand() { GroupID = Group.GroupID });
        if (!apiCallResult.IsSuccess)
        {
            logger.LogWarning("Error fetching group members.");
            snackbar.Add("Error fetching group members.", Severity.Error);
            return;
        }
        Permissions = apiCallResult.GroupPermissions;
        SelectedMembers = Permissions.Where(m => m.IsSelected).ToHashSet();
        IsLoading = false;
        RaisePropertyChanged();
    }

    /// <summary>
    /// Handles the change of selection state for a group permission.
    /// </summary>
    /// <param name="member">The group permission that changed selection state.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask SelectedItemChangedHandler(GroupPermissionRead member)
    {
        member.IsSelected = !member.IsSelected;
    }

    /// <summary>
    /// Handles the change of selection state for a group permission.
    /// </summary>
    /// <param name="value">The new selection state.</param>
    /// <param name="member">The group permission that changed selection state.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask ItemIsSelectedChangedHandler(bool value, GroupPermissionRead member)
    {
        member.IsSelected = value;
        if (member.IsSelected && !SelectedMembers.Contains(member))
        {
            SelectedMembers.Add(member);
        }
        else
        {
            SelectedMembers.Remove(member);
        }

        if (ChangedMembers.Contains(member))
        {
            ChangedMembers.Remove(member);
        }
        else
        {
            ChangedMembers.Add(member);
        }
    }

    /// <summary>
    /// Searches for group permissions based on the search string.
    /// </summary>
    /// <param name="model">The group permission to search for.</param>
    /// <returns>True if the group permission matches the search string, otherwise false.</returns>
    public bool Search(GroupPermissionRead model)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
        {
            return true;
        }

        if ($"{model.PermissionName}".Contains(SearchString, StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }
        return false;
    }
}
