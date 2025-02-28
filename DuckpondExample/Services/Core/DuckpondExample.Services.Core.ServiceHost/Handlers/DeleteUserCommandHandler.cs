using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Handles the DeleteUserCommand to delete a user.
/// </summary>
public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResult>
{
    private readonly ILogger<DeleteUserCommandHandler> logger;
    private readonly UserRepository userRepository;
    private readonly UserGroupsRepository userGroupsRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteUserCommandHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging information.</param>
    /// <param name="userRepository">The repository instance for accessing user data.</param>
    public DeleteUserCommandHandler(
        ILogger<DeleteUserCommandHandler> logger,
        UserRepository userRepository,
        UserGroupsRepository userGroupsRepository)
    {
        this.logger = logger;
        this.userRepository = userRepository;
        this.userGroupsRepository = userGroupsRepository;
    }

    /// <summary>
    /// Handles the DeleteUserCommand to delete a user.
    /// </summary>
    /// <param name="request">The command request containing necessary parameters.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the DeleteUserResult.</returns>
    public async Task<DeleteUserResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var deletedCount = userGroupsRepository.DeleteAllUserGroupsByUserIdAsnyc(request.UserId);

            logger.LogInformation($"Trying to delete user with ID {request.UserId}.");
            var result = await userRepository.DeleteAsync(request.UserId);
            if (result)
            {
                logger.LogInformation($"User with ID {request.UserId} deleted successfully.");
                return new DeleteUserResult { IsSuccess = true, IsDeleted = true };
            }
            logger.LogWarning($"Error deleting user with ID {request.UserId}.");
            return new DeleteUserResult { IsSuccess = false, IsDeleted = false, ErrorMessages = new List<string> { "Error deleting user." } };

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while deleting user with ID {request.UserId}.");
            return new DeleteUserResult { IsSuccess = false, IsDeleted = false, ErrorMessages = new List<string> { "Error deleting user.", ex.Message } };
        }
    }
}
