using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Shared.Common.Hosts.BaseObjects;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DuckpondExample.Services.Core.ServiceHost.Controllers;
/// <summary>
/// Controller for managing groups.
/// </summary>
/// <remarks>
/// This controller provides endpoints to manage groups, including retrieving all groups,
/// getting group members, getting group permissions, updating groups with members and permissions,
/// and creating groups with members and permissions.
/// </remarks>
[Route("api/[controller]")]
[ApiController]
public class GroupsController : DuckpondApiControllerBase
{
    public GroupsController(ILogger logger, IMediator mediator) : base(logger, mediator)
    {
    }

    /// <summary>
    /// Gets all groups.
    /// </summary>
    /// <returns>A list of groups.</returns>
    [HttpGet("getall")]
    public async Task<IActionResult> GetGroups()
    {
        return await ExecuteCommandAsync(new GetGroupsCommand());
    }

    /// <summary>
    /// Gets the members of a specific group.
    /// </summary>
    /// <param name="getCommand">The command containing the group ID.</param>
    /// <returns>A list of group members.</returns>
    [HttpPost("getgroupmembers")]
    public async Task<IActionResult> GetGroupMembers([FromBody] GetGroupMembersCommand getCommand)
    {
        return await ExecuteCommandAsync(getCommand);
    }

    /// <summary>
    /// Gets the permissions of a specific group.
    /// </summary>
    /// <param name="getCommand">The command containing the group ID.</param>
    /// <returns>A list of group permissions.</returns>
    [HttpPost("getpermissions")]
    public async Task<IActionResult> GetPermissions([FromBody] GetGroupPermissionsCommand getCommand)
    {
        return await ExecuteCommandAsync(getCommand);
    }

    /// <summary>
    /// Updates a group with its members and permissions.
    /// </summary>
    /// <param name="updateCommand">The command containing the group, changed members, and changed permissions.</param>
    /// <returns>The result of the update operation.</returns>
    [HttpPost("updategroupwithmembersandpermissions")]
    public async Task<IActionResult> UpdateGroupWithMembersAndPermissions([FromBody] UpdateGroupWithMembersAndPermissionsCommand updateCommand)
    {
        return await ExecuteCommandAsync(updateCommand);
    }

    /// <summary>
    /// Creates a group with its members and permissions.
    /// </summary>
    /// <param name="createCommand">The command containing the group, members, and permissions.</param>
    /// <returns>The result of the create operation.</returns>
    [HttpPost("creategroupwithmembersandpermissions")]
    public async Task<IActionResult> CreateGroupWithMembersAndPermissions([FromBody] CreateGroupWithMembersAndPermissionsCommand createCommand)
    {
        return await ExecuteCommandAsync(createCommand);
    }

    /// <summary>
    /// Deletes a specific group.
    /// </summary>
    /// <param name="deleteCommand">The command containing the group ID.</param>
    /// <returns>The result of the delete operation.</returns>
    [HttpPost("deletegroup")]
    public async Task<IActionResult> DeleteGroup([FromBody] DeleteGroupCommand deleteCommand)
    {
        return await ExecuteCommandAsync(deleteCommand);
    }

}
