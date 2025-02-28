using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Handles the command to get permissions for a specific group.
/// </summary>
/// <param name="logger">The logger instance to log information and warnings.</param>
/// <param name="groupPermissionReadRepository">The repository to read group permissions from the database.</param>
public class GetGroupPermissionsCommandHandler(
    ILogger<GetGroupPermissionsCommandHandler> logger,
    GroupPermissionReadRepository groupPermissionReadRepository
    ) : IRequestHandler<GetGroupPermissionsCommand, GetGroupPermissionsResult>
{
    /// <summary>
    /// Handles the request to get group permissions.
    /// </summary>
    /// <param name="request">The command containing the group ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the group permissions result.</returns>
    public async Task<GetGroupPermissionsResult> Handle(GetGroupPermissionsCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to get all permissions for GroupID '{request.GroupID}'.");
        var permissions = await groupPermissionReadRepository.GetGroupPermissionsForGroup(request.GroupID);
        if (permissions == null)
        {
            logger.LogWarning($"Error fetching permissions '{request.GroupID}'.");
            return new GetGroupPermissionsResult() { IsSuccess = false, ErrorMessages = [$"Error fetching permissions '{request.GroupID}'."] };
        }
        return new GetGroupPermissionsResult() { IsSuccess = true, GroupPermissions = permissions };
    }
}
