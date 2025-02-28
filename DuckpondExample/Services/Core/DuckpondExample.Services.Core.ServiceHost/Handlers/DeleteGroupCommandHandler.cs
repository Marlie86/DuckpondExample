using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Handles the DeleteGroupCommand to delete a group.
/// </summary>
public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, DeleteGroupResult>
{
    private readonly ILogger<DeleteGroupCommandHandler> logger;
    private readonly GroupsRepository groupRepository;
    private readonly UserGroupsRepository userGroupsRepository;

    /// <summary>
    /// Handles the DeleteGroupCommand to delete a group.
    /// </summary>
    /// <param name="logger">The logger instance used for logging information.</param>
    /// <param name="groupRepository">The repository for managing group entities.</param>
    /// <param name="userGroupsRepository">The repository for managing user group entities.</param>
    public DeleteGroupCommandHandler(
        ILogger<DeleteGroupCommandHandler> logger,
        GroupsRepository groupRepository,
        UserGroupsRepository userGroupsRepository)
    {
        this.logger = logger;
        this.groupRepository = groupRepository;
        this.userGroupsRepository = userGroupsRepository;
    }

    /// <summary>
    /// Handles the DeleteGroupCommand to delete a group.
    /// </summary>
    /// <param name="request">The command request containing necessary parameters.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the DeleteGroupResult.</returns>
    public async Task<DeleteGroupResult> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to delete group with ID {request.GroupId}.");

        // Delete all group members
        var membersDeleted = await userGroupsRepository.DeleteGroupMembersAsync(request.GroupId);

        // Delete all permissions for this group
        var permissionsDeleted = await groupRepository.DeleteGroupPermissionsAsync(request.GroupId);

        var result = await groupRepository.DeleteAsync(request.GroupId);
        if (result)
        {
            logger.LogInformation($"Group with ID {request.GroupId} deleted successfully.");
            return new DeleteGroupResult { IsSuccess = true, IsDeleted = true };
        }
        logger.LogWarning($"Error deleting group with ID {request.GroupId}.");
        return new DeleteGroupResult { IsSuccess = false, IsDeleted = false, ErrorMessages = new List<string> { "Error deleting group." } };
    }
}
