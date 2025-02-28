using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DuckpondExample.Services.Core.Shared.DataItems;

/// <summary>
/// Represents a user in the application.
/// </summary>
[Table("[Duckpond].[Users]")]
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [Key]
    public int UserID { get; set; }

    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string EMailAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hashed password of the user.
    /// </summary>
    public byte[] HashedPassword { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether the user is a system user.
    /// </summary>
    public bool IsSystemUser { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is an admin.
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is permitted to log on.
    /// </summary>
    public bool IsPermittedToLogon { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user uses an external logon provider.
    /// </summary>
    public bool IsExternalLogonProvider { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is deactivated.
    /// </summary>
    public bool IsDeactivated { get; set; }

    /// <summary>
    /// Gets or sets the refresh token of the user.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the expiration date and time of the token.
    /// </summary>
    public DateTime? TokenExpires { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created this record.
    /// </summary>
    public int CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when this record was created.
    /// </summary>
    public DateTime CreatedWhen { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the identifier of the user who last edited this record.
    /// </summary>
    public int LastEditedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when this record was last edited.
    /// </summary>
    public DateTime LastEditedWhen { get; set; } = DateTime.Now;
}
