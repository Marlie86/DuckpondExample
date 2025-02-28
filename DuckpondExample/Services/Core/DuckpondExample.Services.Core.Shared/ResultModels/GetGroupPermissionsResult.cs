using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.BaseClasses;

namespace DuckpondExample.Services.Core.Shared.ResultModels;
/// <summary>
/// Represents the result of a request to get group permissions.
/// </summary>
public class GetGroupPermissionsResult : BaseResult
{
    /// <summary>
    /// Gets or sets the collection of group permissions.
    /// </summary>
    public IEnumerable<GroupPermissionRead> GroupPermissions { get; set; } = [];
}
