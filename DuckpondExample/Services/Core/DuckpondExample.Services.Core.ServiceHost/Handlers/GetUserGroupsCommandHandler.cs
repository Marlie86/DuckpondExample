using AutoMapper;

using Duckpond.Aspire.Identity.Repositories;

using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Handles the command to get users groups for a specific user.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="usersGroupsReadRepository">The repository to read users groups data.</param>
/// <param name="mapper">The mapper instance to map data items.</param>
/// <returns>A handler for the GetUsersGroupsCommand.</returns>
public class GetUserGroupsCommandHandler(
        ILogger<GetUserCommand> logger,
        UserGroupsReadRepository usersGroupsReadRepository
    )
    : IRequestHandler<GetUserGroupReadCommand, GetUserGroupReadResult>
{
    /// <summary>
    /// <summary>
    /// Handles the command to get users groups for a specific user.
    /// </summary>
    /// <param name="logger">The logger instance for logging information.</param>
    /// <param name="usersGroupsReadRepository">The repository to read users groups data.</param>
    /// <param name="mapper">The mapper instance to map data items.</param>
    /// <returns>A handler for the GetUsersGroupsCommand.</returns>
    public async Task<GetUserGroupReadResult> Handle(GetUserGroupReadCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to get all user groups for UserID {request.UserID}.");
        var usersGroups = await usersGroupsReadRepository.GetUsersGroupsForUserAsync(request.UserID);
        if (usersGroups != null && usersGroups.Count() >= 0)
        {
            logger.LogInformation($"Returning {usersGroups.Count()} user groups.");
            return new GetUserGroupReadResult { IsSuccess = true, UserGroups = usersGroups };
        }
        logger.LogInformation($"Error fetching user groups.");
        return new GetUserGroupReadResult { IsSuccess = false, ErrorMessages = ["Error fetching user groups."] };
    }
}
