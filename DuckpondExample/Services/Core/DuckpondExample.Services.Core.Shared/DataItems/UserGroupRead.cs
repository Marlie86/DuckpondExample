using System.ComponentModel.DataAnnotations.Schema;

namespace DuckpondExample.Services.Core.Shared.DataItems;
/// <summary>
/// Represents a read-only view of a user group.
/// </summary>
[Table("[Duckpond].[vw_UserGroups]")]
public class UserGroupRead
{
    /// <summary>
    /// Gets or sets the unique identifier for the group.
    /// </summary>
    public int GroupID { get; set; }

    /// <summary>
    /// Gets or sets the name of the group.
    /// </summary>
    public string GroupName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the group is selected.
    /// </summary>
    public bool IsSelected { get; set; }
}
