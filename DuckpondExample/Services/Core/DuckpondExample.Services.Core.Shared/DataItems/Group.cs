using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuckpondExample.Services.Core.Shared.DataItems;
/// <summary>
/// Represents a group entity with properties for default status, group ID, and name.
/// </summary>
[Table("[Duckpond].[Groups]")]
public class Group
{
    /// <summary>
    /// Gets or sets a value indicating whether the group is the default group.
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the group.
    /// </summary>
    [Key]
    public int GroupID { get; set; }

    /// <summary>
    /// Gets or sets the name of the group.
    /// </summary>
    public string Name { get; set; } = string.Empty;
} // class Groups
