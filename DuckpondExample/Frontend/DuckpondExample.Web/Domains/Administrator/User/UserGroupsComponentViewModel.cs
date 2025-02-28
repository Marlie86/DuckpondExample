using Duckpond.Aspire.Web.Models.StateModels;

using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Shared.Common.Extensions;
using DuckpondExample.Web.ApiClients;
using DuckpondExample.Web.Models;

namespace DuckpondExample.Web.Domains.Administrator.User;

/// <summary>
/// ViewModel for managing users groups in the administrator section.
/// </summary>
/// <remarks>
/// This ViewModel handles the selection and updating of users groups for a specific user.
/// It interacts with the UsersGroupsApiClient to fetch and update the groups, and maintains
/// the state of selected groups.
/// </remarks>
[AddAsService(ServiceLifetime.Transient)]
public class UserGroupsComponentViewModel : BaseViewModel
{
    private readonly ILogger<UserGroupsComponentViewModel> logger;
    private readonly UsersGroupsApiClient userGroupsApiClient;
    private readonly UserStateModel userStateModel;
    private HashSet<UserGroupRead> selectedUsersGroups = new HashSet<UserGroupRead>();

    /// <summary>
    /// Initializes a new instance of the <see cref="UserGroupsComponentViewModel"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging information.</param>
    /// <param name="usersGroupsApiClient">The API client for managing users groups.</param>
    /// <param name="userStateModel">The state model for the user.</param>
    public UserGroupsComponentViewModel(ILogger<UserGroupsComponentViewModel> logger, UsersGroupsApiClient usersGroupsApiClient, UserStateModel userStateModel)
    {
        this.logger = logger;
        this.userGroupsApiClient = usersGroupsApiClient;
        this.userStateModel = userStateModel;
    }

    /// <summary>
    /// Gets or sets the user model.
    /// </summary>
    public UserModel User { get; set; }

    /// <summary>
    /// Gets or sets the collection of users groups.
    /// </summary>
    public IEnumerable<UserGroupRead> UserGroups { get; set; }

    /// <summary>
    /// Gets or sets the selected users groups.
    /// </summary>
    public HashSet<UserGroupRead> SelectedUserGroups
    {
        get => selectedUsersGroups;
        set
        {
            selectedUsersGroups = value;
            RaisePropertyChanged();
        }
    }

    /// <summary>
    /// Initializes the ViewModel asynchronously.
    /// </summary>
    /// <remarks>
    /// This method fetches the users groups and updates the selected groups based on their selection status.
    /// </remarks>
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        UserGroups.ForEach(pg =>
        {
            if (pg.IsSelected)
            {
                SelectedUserGroups.Add(pg);
                RaisePropertyChanged(nameof(SelectedUserGroups));
            }
        });
    }
}