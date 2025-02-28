using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Shared.Common.Attributes;

namespace Duckpond.Aspire.Web.Models.StateModels;

/// <summary>
/// Represents the state model for a user, including the user details and associated groups.
/// </summary>
[AddAsService(ServiceLifetime.Singleton)]
public class UserStateModel
{
    /// <summary>
    /// Gets or sets the user details.
    /// </summary>
    public UserModel User { get; set; } = new UserModel();

    /// <summary>
    /// Gets or sets the collection of groups associated with the user.
    /// </summary>
    public List<UserGroupRead> Groups { get; set; } = new List<UserGroupRead>();
}
