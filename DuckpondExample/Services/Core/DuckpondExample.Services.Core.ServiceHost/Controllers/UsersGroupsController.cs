using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Shared.Common.Hosts.BaseObjects;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DuckpondExample.Services.Core.ServiceHost.Controllers;
/// <summary>
/// Controller for managing users groups.
/// </summary>
/// <remarks>
/// This controller provides endpoints to get and update users groups for a user.
/// </remarks>
[Route("api/[controller]")]
[ApiController]
public class UsersGroupsController : DuckpondApiControllerBase
{
    public UsersGroupsController(ILogger logger, IMediator mediator) : base(logger, mediator)
    {
    }

    /// <summary>
    /// Gets the users groups for a specific user.
    /// </summary>
    /// <param name="command">The command containing the user ID.</param>
    /// <returns>An <see cref="IActionResult"/> representing the result of the command execution.</returns>
    [HttpPost("getusersgroups")]
    public async Task<IActionResult> GetUserGroup(GetUserGroupReadCommand command)
    {
        return await ExecuteCommandAsync(command);
    }

    /// <summary>
    /// Updates the groups for a specific user.
    /// </summary>
    /// <param name="command">The command containing the user ID and the group IDs to update.</param>
    /// <returns>An <see cref="IActionResult"/> representing the result of the command execution.</returns>
    [HttpPost("updateusersgroups")]
    public async Task<IActionResult> UpdateUserGroups(UpdateUserGroupsCommand command)
    {
        return await ExecuteCommandAsync(command);
    }
}
