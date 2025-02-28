using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;
using DuckpondExample.Shared.Common.Hosts.Extensions;
using DuckpondExample.Shared.Common.Hosts.Utilities;

using MediatR;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Handles the update user command by updating the user's information in the repository.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="usersRepository">The repository to access users data.</param>
/// <returns>A task that represents the asynchronous operation. The task result contains the update user result.</returns>
public class UpdateUserCommandHandler(
        ILogger<LogonCommandHandler> logger,
        UserRepository usersRepository) : IRequestHandler<UpdateUserCommand, UpdateUserResult>
{
    /// <summary>
    /// Handles the update user command.
    /// </summary>
    /// <param name="request">The update user command containing the user's details to be updated.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the update user result.</returns>
    public async Task<UpdateUserResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Updating user: {request.User.UserID}");
        var foundUser = await usersRepository.GetByIdAsync(request.User.UserID);
        if (foundUser == null)
        {
            logger.LogWarning($"User not found: {request.User.UserID}");
            return new UpdateUserResult() { IsSuccess = false, ErrorMessages = ["User not found."] };
        }

        foundUser = request.User.CopyProperties(foundUser);

        if (foundUser != null && !string.IsNullOrWhiteSpace(request.User.Password))
        {
            foundUser.HashedPassword = HashUtility.HashPassword(request.User.Password);
        }

        var affectedRows = usersRepository.Update(foundUser);
        if (affectedRows == 1)
        {
            logger.LogWarning($"Error updating user with id '{request.User.UserID}'");
            return new UpdateUserResult() { IsSuccess = true };
        }
        logger.LogInformation($"Successfully updated user with id '{request.User.UserID}'");
        return new UpdateUserResult() { IsSuccess = false, ErrorMessages = [$"Error updating user with id '{request.User.UserID}'."] };
    }
}
