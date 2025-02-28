using Duckpond.Aspire.Web.UtilityServices;

using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Web.ApiClients;

namespace DuckpondExample.Web.Authorize;

/// <summary>
/// Service to check if a user has a specific permission.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="identityApiClient">The identity API client instance.</param>
/// <param name="userClaimsService">The user claims service instance.</param>
[AddAsService(ServiceLifetime.Scoped)]
public class PermissionService(ILogger<PermissionService> logger, IdentityApiClient identityApiClient, UserClaimsService userClaimsService)
{

    /// <summary>
    /// Checks if the user has the specified permission.
    /// </summary>
    /// <param name="permission">The permission to check.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the user has the permission.</returns>
    public async Task<bool> HasPermissionAsync(string permission)
    {
        var userId = Convert.ToInt32(await userClaimsService.GetClaim<string>("UserID"));

        var response = await identityApiClient.HasPermission(new PermissionRequestCommand() { Permission = permission, UserID = userId });
        return response.IsSuccess;
    }
}
