using AutoMapper.Execution;

using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Handles the command to update a group with its members and permissions.
/// </summary>
/// <param name="logger">The logger instance for logging information and errors.</param>
/// <param name="groupRepository">The repository for managing group entities and their members.</param>
public class UpdateGroupWithMembersAndPermissionsCommandHandler(
        ILogger<UpdateGroupWithMembersAndPermissionsResult> logger,
        GroupsRepository groupRepository
    ) : IRequestHandler<UpdateGroupWithMembersAndPermissionsCommand, UpdateGroupWithMembersAndPermissionsResult>
{
    /// <summary>
    /// Handles the command to update a group with its members and permissions.
    /// </summary>
    /// <param name="request">The command request containing the group, changed members, and changed permissions.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the update operation.</returns>
    /// <remarks>
    /// This method updates the group information, adds or deletes members based on their selection status, and adds or deletes permissions based on their selection status.
    /// Logs information and errors during the process.
    /// </remarks>
    public async Task<UpdateGroupWithMembersAndPermissionsResult> Handle(UpdateGroupWithMembersAndPermissionsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("UpdateGroupMembersAndPermissionsCommandHandler.Handle called");
            // Update group
            var updateResult = await groupRepository.UpdateAsync(request.Group);

            // Edit members
            foreach (var member in request.ChangedMembers)
            {
                if (member.IsSelected)
                {
                    await groupRepository.AddGroupMemberAsync(request.Group.GroupID, member);
                }
                else
                {
                    await groupRepository.DeleteGroupMemberAsync(request.Group.GroupID, member);
                }
            }

            // Edit permissions
            foreach (var permission in request.ChangedPermissions)
            {
                if (permission.IsSelected)
                {
                    await groupRepository.AddGroupPermissionAsync(request.Group.GroupID, permission);
                }
                else
                {
                    await groupRepository.DeleteGroupPermissionAsync(request.Group.GroupID, permission);
                }
            }

            return new UpdateGroupWithMembersAndPermissionsResult() { IsSuccess = true, ErrorMessages = [] };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating group, members and permissions for group '{GroupName}'", request.Group.Name);
            return new UpdateGroupWithMembersAndPermissionsResult() { IsSuccess = false, ErrorMessages = [$"Could not update group, members and permissions, for group '{request.Group.Name}'.", ex.Message] };


        }
    }
}
