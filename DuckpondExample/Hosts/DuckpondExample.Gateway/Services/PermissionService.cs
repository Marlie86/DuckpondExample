using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Services.Core.Shared.Enums;
using DuckpondExample.Services.Core.Shared.ResultModels;
using DuckpondExample.Gateway.ApiClients;
using DuckpondExample.Gateway.Models;
using DuckpondExample.Shared.Common.Extensions;
using DuckpondExample.Web;

namespace DuckpondExample.Gateway.Services;

/// <summary>
/// Service to manage permissions by interacting with the API gateway and permission API client.
/// </summary>
public class PermissionService
{
    private readonly ILogger<PermissionService> logger;
    private readonly ApiGatewaySettings apiGateway;
    private readonly PermissionApiClient permissionApiClients;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="apiGateway">The API gateway settings.</param>
    /// <param name="permissionApiClients">The permission API client.</param>
    public PermissionService(ILogger<PermissionService> logger, ApiGatewaySettings apiGateway, PermissionApiClient permissionApiClients)
    {
        this.logger = logger;
        this.apiGateway = apiGateway;
        this.permissionApiClients = permissionApiClients;
    }

    /// <summary>
    /// Updates the known permissions by fetching them from the API gateway settings and adding them using the permission API client.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateKnownPermissions()
    {
        var permissions = apiGateway.ApiServices
            .SelectMany(s => s.Value.ExposedEndpoints)
            .Where(ep => !string.IsNullOrWhiteSpace(ep.Permission))
            .Select(ep => new Permission()
            {
                PermissionName = ep.Permission,
                Description = $"Permission to use permissions named '{ep.Permission}'",
                LastEditedBy = 1,
                Target = PermissionTargetEnum.Server
            });
        AddPermissionsResult result;
        do
        {
            result = await permissionApiClients.AddNewPermissions(new AddPermissionsCommand()
            {
                Target = PermissionTargetEnum.Server,
                Permissions = permissions
            });

            if (!result.IsSuccess)
            {
                logger.LogError("Error updating known permissions");
                result.ErrorMessages.ForEach(em => logger.LogError(em));
            }
        } while (!result.IsSuccess);

        // Load Permissions from Clint API
        var permissionsType = typeof(Permissions);
        var clientPermissions = new List<Permission>();
        var permissionList = permissionsType.GetProperties().Select(p => p).ForEach(property =>
        {
            var permissionName = property.GetValue(null).ToString();
            var permission = new Permission()
            {
                PermissionName = permissionName,
                Description = $"Permission named '{permissionName}'",
                LastEditedBy = 1,
                Target = PermissionTargetEnum.Client
            };
            clientPermissions.Add(permission);
        }, (ex, msg) => logger.LogError(ex, msg));

        result = await permissionApiClients.AddNewPermissions(new AddPermissionsCommand()
        {
            Target = PermissionTargetEnum.Client,
            Permissions = clientPermissions
        });

        if (!result.IsSuccess)
        {
            logger.LogError("Error updating known permissions");
            result.ErrorMessages.ForEach(em => logger.LogError(em));
        }
        //var permissionsClass = assembly.GetType("Duckpond.Aspire.Web.Permissions");
    }
}
