using DuckpondExample.Services.Core.Shared.ResultModels;
using MediatR;

namespace DuckpondExample.Services.Core.Shared.Commands;

/// <summary>
/// Represents a command to delete a user.
/// </summary>
public class DeleteUserCommand : IRequest<DeleteUserResult>
{
    /// <summary>
    /// Gets or sets the ID of the user to be deleted.
    /// </summary>
    public int UserId { get; set; }
}
