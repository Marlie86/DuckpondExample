using AutoMapper;

using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Initializes a new instance of the <see cref="GetUserCommandHandler"/> class.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="usersRepository">The repository instance for accessing users data.</param>
/// <param name="mapper">The mapper instance for mapping entities.</param>
public class GetUserCommandHandler(
        ILogger<GetUserCommand> logger,
        UserRepository usersRepository,
        IMapper mapper
    )
    : IRequestHandler<GetUserCommand, GetUserResult>
{
    /// <summary>
    /// Handles the GetUsersCommand to retrieve a list of users.
    /// </summary>
    /// <param name="request">The command request containing necessary parameters.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the GetUsersResult.</returns>
    public async Task<GetUserResult> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Trying to get all users.");
        var users = await usersRepository.GetAllAsync();
        if (users != null)
        {
            logger.LogInformation($"Returning {users.Count()} users.");
            return new GetUserResult { IsSuccess = true, Users = mapper.Map<IEnumerable<UserModel>>(users) };
        }
        logger.LogWarning($"Error fetching users.");
        return new GetUserResult { IsSuccess = false, ErrorMessages = ["Error fetching users."] };
    }
}
