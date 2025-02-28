using AutoMapper;

using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Handles the command to get a user by their ID.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="usersRepository">The repository to access users data.</param>
/// <param name="mapper">The mapper instance for object mapping.</param>
/// <returns>A handler for the GetUserByIdCommand that returns a UserResult.</returns>
public class GetUserByIdCommandHandler(
        ILogger<LogonCommandHandler> logger,
        UserRepository usersRepository,
        IMapper mapper
    )
    : IRequestHandler<GetUserByIdCommand, UserResult>
{
    /// <summary>
    /// Handles the GetUserByIdCommand.
    /// </summary>
    /// <param name="request">The command request containing the user ID.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the UserResult.</returns>
    public async Task<UserResult> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting user by ID: {request.UserID}");
        var foundUser = await usersRepository.GetByIdAsync(request.UserID);
        if (foundUser == null)
        {
            logger.LogWarning($"User not found: {request.UserID}");
            return new UserResult { IsSuccess = false, ErrorMessages = ["User not found."] };
        }
        foundUser.HashedPassword = []; 
        logger.LogInformation($"User found: {foundUser.UserID}");
        return new UserResult { IsSuccess = true, User = mapper.Map<UserModel>(foundUser) };
    }
}
