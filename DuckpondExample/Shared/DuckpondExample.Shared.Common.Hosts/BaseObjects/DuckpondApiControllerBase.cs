using DuckpondExample.Shared.Common.BaseClasses;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DuckpondExample.Shared.Common.Hosts.BaseObjects;
/// <summary>
/// Base controller class for Duckpond API controllers.
/// </summary>
public class DuckpondApiControllerBase : ControllerBase
{
    protected readonly ILogger logger;
    protected readonly IMediator mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DuckpondApiControllerBase"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="mediator">The mediator instance.</param>
    public DuckpondApiControllerBase(ILogger logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    /// <summary>
    /// Executes a command asynchronously.
    /// </summary>
    /// <typeparam name="TParam">The type of the command parameter.</typeparam>
    /// <param name="command">The command to execute.</param>
    /// <returns>An <see cref="IActionResult"/> representing the result of the command execution.</returns>
    public async Task<IActionResult> ExecuteCommandAsync<TParam>(TParam command) where TParam : class
    {
        try
        {
            var result = await mediator.Send(command);
            var baseResult = result as BaseResult;
            if (baseResult != null && baseResult.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error logging on");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
