using AutoMapper;

using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Handles the command to update the groups associated with a users.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="usersGroupsRepository">The repository to access and update users groups data.</param>
/// <param name="mapper">The mapper instance for object-object mapping.</param>
/// <returns>A task that represents the asynchronous operation. The task result contains the result of the update operation.</returns>
public class UpdateUserGroupsCommandHandler(
        ILogger<UpdateUserGroupsCommandHandler> logger,
        UserGroupsRepository usersGroupsRepository
    )
    : IRequestHandler<UpdateUserGroupsCommand, UpdateUserGroupsResult>
{
    /// <summary>
    /// Handles the command to update the groups associated with a users.
    /// </summary>
    /// <param name="request">The command request containing the users ID and the groups to update.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the update operation.</returns>
    public async Task<UpdateUserGroupsResult> Handle(UpdateUserGroupsCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Updating groups for UserID '{request.UserID}'.");
        try
        {
            var result = await usersGroupsRepository.UpdateUsersGroupsAsync(request.UserID, request.Groups);
            var errors = result ? [] : new List<string>() { $"Error updating the groups for '{request.UserID}'." };
            return new UpdateUserGroupsResult() { IsSuccess = result, ErrorMessages = errors };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error updating groups for UserID '{request.UserID}'.");
            return new UpdateUserGroupsResult() { IsSuccess = false, ErrorMessages = [$"Error updating groups for UserID '{request.UserID}'."] };
        }
    }
}
