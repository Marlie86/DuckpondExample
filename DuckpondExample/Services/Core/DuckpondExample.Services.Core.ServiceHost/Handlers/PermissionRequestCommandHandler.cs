using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;
using DuckpondExample.Shared.Common.Hosts.Utilities;

using MediatR;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Handles the permission request command by processing the request and returning the result.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="IdentityRepository">The repository to access identity data.</param>
/// <param name="jwtUtility">The utility for generating JWT tokens.</param>
/// <param name="configuration">The configuration settings.</param>
public class PermissionRequestCommandHandler(
        ILogger<LogonCommandHandler> logger,
        PermissionReadRepository permissionReadRepository,
        UserRepository usersRepository
     ) : IRequestHandler<PermissionRequestCommand, PermissionRequestResult>
{
    /// <summary>
    /// Handles the permission request command.
    /// </summary>
    /// <param name="request">The permission request command containing the permission name and user ID.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the permission request result.</returns>
    public async Task<PermissionRequestResult> Handle(PermissionRequestCommand request, CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByIdAsync(request.UserID);
        if (user == null)
        {
            logger.LogWarning($"User not found: {request.UserID}");
            return new PermissionRequestResult { IsSuccess = false, ErrorMessages = ["User not found."] };
        }

        if (user.IsAdmin)
        {
            logger.LogInformation($"User is admin: {request.UserID}");
            return new PermissionRequestResult { IsSuccess = true };
        }

        var hasPermission = await permissionReadRepository.HasPermission(request);
        if (hasPermission)
        {
            logger.LogInformation($"User has permission: {request.Permission}");
            return new PermissionRequestResult { IsSuccess = true };
        }
        logger.LogWarning($"User does not have permission: {request.Permission}");
        return new PermissionRequestResult { IsSuccess = false, ErrorMessages = ["User does not have permission."] };
    }
}
