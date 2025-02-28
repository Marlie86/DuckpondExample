using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

namespace DuckpondExample.Services.Core.Shared.Commands;
// <summary>
// Represents a command to update a user.
// </summary>
public class UpdateUserCommand : IRequest<UpdateUserResult>
{
    /// <summary>
    /// Command to update a user's details.
    /// </summary>
    public UserModel User { get; set; } = new UserModel();

    /// <summary>
    /// Gets or sets the groups associated with the user.
    /// </summary>
    public IEnumerable<UserGroupRead>? Groups { get; set; }
}
