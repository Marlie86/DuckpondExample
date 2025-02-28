using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

namespace DuckpondExample.Services.Core.Shared.Commands;
// <summary>
// Represents a command to log on a user.
// </summary>
public class LogonCommand : IRequest<LogonResult>
{
    /// <summary>
    /// Gets or sets the logon name of the user.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
