using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Web.ApiClients;
using DuckpondExample.Web.Models;

using MudBlazor;

namespace DuckpondExample.Web.Domains.Administrator.Groups;

/// <summary>
/// ViewModel for managing group members in the administrator domain.
/// </summary>
/// <remarks>
/// This ViewModel handles the loading, selection, and searching of group members.
/// It interacts with the GroupsApiClient to fetch group members and uses MudBlazor's ISnackbar for notifications.
/// </remarks>
[AddAsService(ServiceLifetime.Transient)]
public class GroupMembersViewModel : BaseViewModel
{
    private readonly ILogger<GroupsViewModel> logger;
    private readonly GroupsApiClient groupsApiClient;
    private readonly ISnackbar snackbar;
    private Group group = new Group();

    public string SearchString { get; set; } = string.Empty;
    public Group Group { get => group; set { if (group == value) { return; } group = value; RaisePropertyChanged(); } }

    public bool IsLoading { get; set; } = false;

    /// <summary>
    /// Gets or sets the collection of group members.
    /// </summary>
    public IEnumerable<GroupMembersRead>? Members { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of changed group members.
    /// </summary>
    public List<GroupMembersRead>? ChangedMembers { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of selected group members.
    /// </summary>
    public HashSet<GroupMembersRead> SelectedMembers { get; set; } = [];


    /// <summary>
    /// Initializes a new instance of the <see cref="GroupMembersViewModel"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="groupsApiClient">The API client for fetching group members.</param>
    /// <param name="snackbar">The snackbar service for displaying notifications.</param>
    public GroupMembersViewModel(ILogger<GroupsViewModel> logger, GroupsApiClient groupsApiClient, ISnackbar snackbar)
    {
        this.logger = logger;
        this.groupsApiClient = groupsApiClient;
        this.snackbar = snackbar;
    }

    /// <summary>
    /// Initializes the ViewModel asynchronously by loading the group members.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await LoadGroupMembers();
    }

    private async Task LoadGroupMembers()
    {
        logger.LogInformation($"Fetching members for group {Group?.Name}.");
        IsLoading = true;
        if (Group == null)
        {
            logger.LogWarning("Group is null.");
            snackbar.Add("Group is null.", Severity.Warning);
            return;
        }

        var apiCallResult = await groupsApiClient.GetGroupMembersAsync(new GetGroupMembersCommand() { GroupID = Group.GroupID });
        if (!apiCallResult.IsSuccess)
        {
            logger.LogWarning("Error fetching group members.");
            snackbar.Add("Error fetching group members.", Severity.Error);
            return;
        }
        Members = apiCallResult.Members;
        SelectedMembers = Members.Where(m => m.IsSelected).ToHashSet();
        IsLoading = false;
        RaisePropertyChanged();
    }

    /// <summary>
    /// Handles the change in selection state of a group member.
    /// </summary>
    /// <param name="member">The group member whose selection state has changed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask SelectedItemChangedHandler(GroupMembersRead member)
    {
        member.IsSelected = !member.IsSelected;
    }

    /// <summary>
    /// Handles the change in selection state of a group member.
    /// </summary>
    /// <param name="value">The new selection state of the group member.</param>
    /// <param name="member">The group member whose selection state has changed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask ItemIsSelectedChangedHandler(bool value, GroupMembersRead member)
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
    /// Searches for a group member based on the search string.
    /// </summary>
    /// <param name="model">The group member to search for.</param>
    /// <returns>True if the group member matches the search string; otherwise, false.</returns>
    public bool Search(GroupMembersRead model)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
        {
            return true;
        }

        if ($"{model.Username}".Contains(SearchString, StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }
        return false;
    }
}
