using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuckpondExample.Services.Core.Shared.DataItems;
/// <summary>
/// Represents a group of user with specific permissions.
/// </summary>
[Table("[Duckpond].[UserGroups]")]
public class UserGroup
{
    /// <summary>
    /// Gets or sets the permission identifier for the user group.
    /// </summary>
    [Key]
    public int UserGroupID { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the group.
    /// </summary>
    public int GroupID { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public int? UserID { get; set; }
} // class UsersGroups
