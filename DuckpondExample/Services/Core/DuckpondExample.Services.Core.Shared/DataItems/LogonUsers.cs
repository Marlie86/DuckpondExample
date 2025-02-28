using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuckpondExample.Services.Core.Shared.DataItems;

/// <summary>
/// Represents an identity in the application.
/// </summary>
[Table("[Duckpond].[LogonUsers]")]
public class LogonUsers
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    [Key]
    public int UserID { get; set; }

    /// <summary>
    /// Gets or sets the hashed password.
    /// </summary>
    public byte[] HashedPassword { get; set; } = [];

    /// <summary>
    /// Gets or sets the logon name.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the logon name.
    /// </summary>
    public bool IsAdmin { get; set; }
}
