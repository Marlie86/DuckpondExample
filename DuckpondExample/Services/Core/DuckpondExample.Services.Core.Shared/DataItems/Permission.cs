using DuckpondExample.Services.Core.Shared.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuckpondExample.Services.Core.Shared.DataItems;

/// <summary>
/// Represents a permission entity in the application.
/// </summary>
[Table("[Duckpond].[Permissions]")]
public class Permission
{
    /// <summary>
    /// Gets or sets the permission ID.
    /// </summary>
    [Key]
    public int PermissionID { get; set; }

    /// <summary>
    /// Gets or sets the name of the permission.
    /// </summary>
    public string PermissionName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the permission.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target of the permission.
    /// </summary>
    public PermissionTargetEnum Target { get; set; } = PermissionTargetEnum.Server;

    /// <summary>
    /// Gets or sets the identifier of the last user who edited this record.
    /// </summary>
    public int LastEditedBy { get; set; }
}
