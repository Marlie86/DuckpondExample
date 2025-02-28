using System.ComponentModel.DataAnnotations;

namespace DuckpondExample.Services.Core.Shared.Models;
/// <summary>
/// Represents a user with various useral details.
/// </summary>
public class UserModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [Key]
    public int UserID { get; set; } = -1;

    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string EMailAddress { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the user is a system user.
    /// </summary>
    public bool IsSystemUser { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the user is an admin.
    /// </summary>
    public bool IsAdmin { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the user is permitted to log on.
    /// </summary>
    public bool IsPermittedToLogon { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the user uses an external logon provider.
    /// </summary>
    public bool IsExternalLogonProvider { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the user is deactivated.
    /// </summary>
    public bool IsDeactivated { get; set; } = true;




}
