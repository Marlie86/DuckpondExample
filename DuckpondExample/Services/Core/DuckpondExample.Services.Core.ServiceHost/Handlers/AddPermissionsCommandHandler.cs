using Duckpond.Aspire.Identity.Repositories;

using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;
using DuckpondExample.Shared.Common.Extensions;

using MediatR;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;


/// <summary>
/// Handles the add permissions command.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="permissionsRepository">The repository to access permissions data.</param>
public class AddPermissionsCommandHandler(
        ILogger<LogonCommandHandler> logger,
        PermissionsRepository permissionsRepository
    )
    : IRequestHandler<AddPermissionsCommand, AddPermissionsResult>
{
    /// <summary>
    /// Handles the add permissions command.
    /// </summary>
    /// <param name="request">The add permissions command request containing permissions to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of adding permissions.</returns>
    public async Task<AddPermissionsResult> Handle(AddPermissionsCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Adding permissions: {string.Join(", ", request.Permissions.Select(p => p.PermissionName))}");
        var errors = new List<string>();
        await request.Permissions.ForEachAsync(async permission =>
        {
            if (await permissionsRepository.PermissionExists(permission.PermissionName))
            {
                errors = new List<string> { $"Permission already exists, '{permission.PermissionName}'" };
                logger.LogWarning($"Permission already exists, '{permission.PermissionName}'");
                return;
            }

            var result = await permissionsRepository.InsertAsync(permission);
            if (result != 1)
            {
                logger.LogWarning($"Error adding permission, '{permission.PermissionName}'");
                return;
            }
            logger.LogInformation($"Successfully added '{permission.PermissionName}'");
        });

        logger.LogInformation("Sucess adding permissions");
        return new AddPermissionsResult { IsSuccess = errors.Count == 0, ErrorMessages = errors };
    }
}
