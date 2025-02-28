using AutoMapper;

using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Handles the GetGroupsCommand to retrieve a list of groups.
/// </summary>
/// <param name="logger">The logger instance for logging information and warnings.</param>
/// <param name="groupsRepository">The repository instance for accessing group data.</param>
/// <param name="mapper">The mapper instance for mapping data objects.</param>
public class GetGroupsCommandHandler(
        ILogger<GetUserCommand> logger,
        GroupsRepository groupsRepository
    ) : IRequestHandler<GetGroupsCommand, GetGroupsResult>
{
    /// <summary>
    /// Handles the GetGroupsCommand to retrieve a list of groups.
    /// </summary>
    /// <param name="request">The command request to get groups.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the GetGroupsResult.</returns>
    public async Task<GetGroupsResult> Handle(GetGroupsCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting groups");
        var groups = await groupsRepository.GetAllAsync();
        if (groups == null)
        {
            logger.LogWarning("Error fetching groups.");
            return new GetGroupsResult() { IsSuccess = false, ErrorMessages = ["Error fetching groups."] };
        }
        return new GetGroupsResult() { IsSuccess = true, Groups = groups };
    }
}
