using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Shared.Common.Hosts.BaseObjects;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DuckpondExample.Services.Core.ServiceHost.Controllers;
/// <summary>
/// API Controller for handling identity-related operations.
/// </summary>
/// <remarks>
/// This controller provides endpoints for user logon and permission requests.
/// </remarks>
[Route("api/[controller]/")]
[ApiController]
public class IdentityController : DuckpondApiControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="mediator">The mediator instance.</param>
    public IdentityController(ILogger<IdentityController> logger, IMediator mediator) : base(logger, mediator)
    {

    }

    /// <summary>
    /// Handles the logon operation.
    /// </summary>
    /// <param name="logonCommand">The logon command containing logon details.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the logon operation.</returns>
    [HttpPost("logon")]
    public async Task<IActionResult> Logon([FromBody] LogonCommand logonCommand)
    {
        return await ExecuteCommandAsync(logonCommand);
    }

    /// <summary>
    /// Handles the logon operation.
    /// </summary>
    /// <param name="permissionRequestCommand">The logon command containing logon details.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the logon operation.</returns>
    [HttpPost("haspermission")]
    public async Task<IActionResult> HasPermisson([FromBody] PermissionRequestCommand permissionRequestCommand)
    {
        return await ExecuteCommandAsync(permissionRequestCommand);
    }
}
