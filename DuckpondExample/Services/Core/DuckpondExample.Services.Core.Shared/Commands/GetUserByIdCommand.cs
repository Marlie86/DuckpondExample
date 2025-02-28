using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

namespace DuckpondExample.Services.Core.Shared.Commands;
/// <summary>
/// Command to get a user by their ID.
/// </summary>
public class GetUserByIdCommand : IRequest<UserResult>
{
    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    public int UserID { get; set; }
}
