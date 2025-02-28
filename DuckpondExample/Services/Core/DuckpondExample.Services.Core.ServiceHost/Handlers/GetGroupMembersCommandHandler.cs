using AutoMapper;

using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Handles the command to get the members of a specific group.
/// </summary>
/// <param name="logger">The logger instance to log information.</param>
/// <param name="groupMembersReadRepository">The repository to read group members from.</param>
/// <param name="mapper">The mapper instance to map data.</param>
public class GetGroupMembersCommandHandler(
        ILogger<GetUserCommand> logger,
        GroupMembersReadRepository groupMembersReadRepository
    ) : IRequestHandler<GetGroupMembersCommand, GetGroupMembersResult>
{
    /// <summary>
    /// Handles the request to get the members of a specific group.
    /// </summary>
    /// <param name="request">The command containing the group ID.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the group members result.</returns>
    public async Task<GetGroupMembersResult> Handle(GetGroupMembersCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to get all all group members for GroupID '{request.GroupID}'.");
        var result = await groupMembersReadRepository.GetGroupMembersForGroup(request.GroupID);
        if (result != null)
        {
            logger.LogInformation($"Returning {result.Count()} group members.");
            return new GetGroupMembersResult { IsSuccess = true, Members = result };
        }
        logger.LogInformation($"Error fetching group members.");
        return new GetGroupMembersResult { IsSuccess = false, ErrorMessages = ["Error fetching group members."] };
    }
}
