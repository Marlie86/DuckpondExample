using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.BaseClasses;

namespace DuckpondExample.Services.Core.Shared.ResultModels;
/// <summary>
/// Represents the result of a request to get group members.
/// </summary>
public class GetGroupMembersResult : BaseResult
{
    /// <summary>
    /// Gets or sets the collection of group members.
    /// </summary>
    public IEnumerable<GroupMembersRead> Members { get; set; } = [];
}
