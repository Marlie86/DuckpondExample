using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Shared.Common.Hosts.BaseObjects;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DuckpondExample.Services.Core.ServiceHost.Controllers;
/// <summary>
/// Controller for managing permissions.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PermissionController : DuckpondApiControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="mediator">The mediator instance.</param>
    public PermissionController(ILogger<PermissionController> logger, IMediator mediator) : base(logger, mediator)
    {
    }

    /// <summary>
    /// Adds new permissions.
    /// </summary>
    /// <param name="addPermissionsCommand">The command containing the permissions to add.</param>
    /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
    [HttpPost("addnew")]
    public async Task<IActionResult> AddPermission([FromBody] AddPermissionsCommand addPermissionsCommand)
    {
        return await ExecuteCommandAsync(addPermissionsCommand);
    }
}
