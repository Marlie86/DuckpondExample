using System.ComponentModel.DataAnnotations;

namespace DuckpondExample.Services.Core.Shared.DataItems;
/// <summary>
/// Represents a group permission with its ID, name, and selection status.
/// </summary>
public class GroupPermissionRead
{
    /// <summary>
    /// Gets or sets the unique identifier for the permission.
    /// </summary>
    [Key]
    public int PermissionID { get; set; }

    /// <summary>
    /// Gets or sets the name of the permission.
    /// </summary>
    public string PermissionName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the permission is selected.
    /// </summary>
    public bool IsSelected { get; set; }
}
