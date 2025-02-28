using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Shared.Common.Hosts.BaseObjects;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DuckpondExample.Services.Core.ServiceHost.Controllers;
/// <summary>
/// API Controller for handling users-related operations.
/// </summary>
[Route("api/[controller]/")]
[ApiController]
public class UsersController : DuckpondApiControllerBase
{

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="mediator">The mediator instance.</param>
    public UsersController(ILogger logger, IMediator mediator) : base(logger, mediator)
    {
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing the list of users.</returns>
    [HttpGet("getall")]
    public async Task<IActionResult> GetUsers()
    {
        return await ExecuteCommandAsync(new GetUserCommand());
    }

    /// <summary>
    /// Gets a user by their ID.
    /// </summary>
    /// <param name="getCommand">The command containing the user ID.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    [HttpPost("getbyid")]
    public async Task<IActionResult> GetById([FromBody] GetUserByIdCommand getCommand)
    {
        return await ExecuteCommandAsync(getCommand);
    }

    /// <summary>
    /// Updates a user's details.
    /// </summary>
    /// <param name="updateCommand">The command containing the user details to update.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    [HttpPost("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand updateCommand)
    {
        return await ExecuteCommandAsync(updateCommand);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="createCommand">The command containing the user details to create.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand createCommand)
    {
        return await ExecuteCommandAsync(createCommand);
    }

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="deleteCommand">The command containing the user ID.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    [HttpPost("delete")]
    public async Task<IActionResult> DeleteUser([FromBody] DeleteUserCommand deleteCommand)
    {
        return await ExecuteCommandAsync(deleteCommand);
    }

}
