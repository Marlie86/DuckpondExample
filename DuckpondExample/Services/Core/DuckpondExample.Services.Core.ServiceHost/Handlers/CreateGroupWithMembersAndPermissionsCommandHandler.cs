using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Handles the creation of a group along with its members and permissions.
/// </summary>
/// <param name="logger">The logger instance for logging information and errors.</param>
/// <param name="groupRepository">The repository for managing group entities and their members.</param>
/// <returns>A task that represents the asynchronous operation. The task result contains the result of the creation operation.</returns>
/// <exception cref="Exception">Thrown when an error occurs during the creation process.</exception>
public class CreateGroupWithMembersAndPermissionsCommandHandler(
        ILogger<CreateGroupWithMembersAndPermissionsCommandHandler> logger,
        GroupsRepository groupRepository
    ) : IRequestHandler<CreateGroupWithMembersAndPermissionsCommand, CreateGroupWithMembersAndPermissionsResult>
{
    /// <summary>
    /// <summary>
    /// Handles the creation of a group along with its members and permissions.
    /// </summary>
    /// <param name="logger">The logger instance for logging information and errors.</param>
    /// <param name="groupRepository">The repository for managing group entities and their members.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the creation operation.</returns>
    /// <exception cref="Exception">Thrown when an error occurs during the creation process.</exception>
    public async Task<CreateGroupWithMembersAndPermissionsResult> Handle(CreateGroupWithMembersAndPermissionsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("UpdateGroupMembersAndPermissionsCommandHandler.Handle called");
            // Update group
            var newId = await groupRepository.InsertAsync(request.Group);

            // Edit members
            foreach (var member in request.ChangedMembers)
            {
                await groupRepository.AddGroupMemberAsync(newId, member);
            }

            // Edit permissions
            foreach (var permission in request.ChangedPermissions)
            {
                await groupRepository.AddGroupPermissionAsync(newId, permission);
            }

            return new CreateGroupWithMembersAndPermissionsResult() { IsSuccess = true, ErrorMessages = [] };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating group, members and permissions for group '{GroupName}'", request.Group.Name);
            return new CreateGroupWithMembersAndPermissionsResult() { IsSuccess = false, ErrorMessages = [$"Could not update group, members and permissions, for group '{request.Group.Name}'.", ex.Message] };
        }
    }
}
