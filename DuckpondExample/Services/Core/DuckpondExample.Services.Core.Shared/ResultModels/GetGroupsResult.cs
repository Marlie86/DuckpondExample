using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.BaseClasses;

namespace DuckpondExample.Services.Core.Shared.ResultModels;
/// <summary>
/// Represents the result of a request to get groups.
/// </summary>
public class GetGroupsResult : BaseResult
{
    /// <summary>
    /// Gets or sets the collection of groups.
    /// </summary>
    public IEnumerable<Group> Groups { get; set; } = [];
}
