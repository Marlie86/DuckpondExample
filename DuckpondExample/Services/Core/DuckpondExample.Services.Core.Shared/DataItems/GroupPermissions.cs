using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuckpondExample.Services.Core.Shared.DataItems;
/// <summary>
/// Represents the permissions associated with a group.
/// </summary>
[Table("[Duckpond].[GroupPermissions]")]
public class GroupPermission
{
    /// <summary>
    /// Gets or sets the unique identifier for the group.
    /// </summary>
    [Key]
    public int GroupID { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user group permission.
    /// </summary>
    public int GroupPermissionID { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the permission.
    /// </summary>
    public int PermissionID { get; set; }
} // class GroupPermissions
