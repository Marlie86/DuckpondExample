using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.BaseClasses;

namespace DuckpondExample.Services.Core.Shared.ResultModels;
/// <summary>
/// Represents the result of a read operation for user groups.
/// </summary>
public class GetUserGroupReadResult : BaseResult
{
    /// <summary>
    /// Gets or sets the collection of user groups.
    /// </summary>
    public IEnumerable<UserGroupRead> UserGroups { get; set; } = [];
}
