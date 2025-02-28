using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;
using DuckpondExample.Web.UtilityServices;

using System.Net.Http.Json;

namespace DuckpondExample.Web.ApiClients;

/// <summary>
/// API client for managing groups and their members.
/// </summary>
public class GroupsApiClient : ApiClientBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupsApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client instance.</param>
    /// <param name="storageService">The storage service instance.</param>
    public GroupsApiClient(HttpClient httpClient, StorageService storageService) : base(httpClient, storageService)
    {
    }

    /// <summary>
    /// Asynchronously retrieves all groups.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the groups retrieval result.</returns>
    public async Task<GetGroupsResult> GetGroupsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var result = await httpClient.GetFromJsonAsync<GetGroupsResult>("Groups/getall");
            return result ?? new GetGroupsResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(GetGroupsCommand)}'."] };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new GetGroupsResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(GetGroupsCommand)}'."] };
        }
    }

    /// <summary>
    /// Asynchronously retrieves members of a specific group.
    /// </summary>
    /// <param name="command">The command containing the group ID.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the group members retrieval result.</returns>
    public async Task<GetGroupMembersResult> GetGroupMembersAsync(GetGroupMembersCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await httpClient.PostAsJsonAsync("Groups/getgroupmembers", command, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetGroupMembersResult>(cancellationToken: cancellationToken) ?? new GetGroupMembersResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(GetGroupMembersAsync)}'."] };

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new GetGroupMembersResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(GetGroupMembersCommand)}'."] };
        }
    }

    /// <summary>
    /// Asynchronously retrieves the permissions of a specific group.
    /// </summary>
    /// <param name="command">The command containing the group ID.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the group permissions retrieval result.</returns>
    public async Task<GetGroupPermissionsResult> GetGroupPermissionsAsync(GetGroupPermissionsCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await httpClient.PostAsJsonAsync("Groups/getpermissions", command, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetGroupPermissionsResult>(cancellationToken: cancellationToken) ?? new GetGroupPermissionsResult() { IsSuccess = false, ErrorMessages = [$"Error reading '{nameof(GetGroupPermissionsResult)}'."] };

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new GetGroupPermissionsResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(GetGroupPermissionsAsync)}'."] };
        }
    }

    /// <summary>
    /// Asynchronously updates the group, its members and permissions.
    /// </summary>
    /// <param name="command">The command containing the group details and the changes to be made.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the update result.</returns>
    public async Task<UpdateGroupWithMembersAndPermissionsResult> UpdateGroupPermissionsAsync(UpdateGroupWithMembersAndPermissionsCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await httpClient.PostAsJsonAsync("Groups/updategroupwithmembersandpermissions", command, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UpdateGroupWithMembersAndPermissionsResult>(cancellationToken: cancellationToken) ?? new UpdateGroupWithMembersAndPermissionsResult() { IsSuccess = false, ErrorMessages = [$"Error reading '{nameof(UpdateGroupWithMembersAndPermissionsResult)}'."] };

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new UpdateGroupWithMembersAndPermissionsResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(UpdateGroupWithMembersAndPermissionsResult)}'."] };
        }
    }

    /// <summary>
    /// Asynchronously creates a new group with members and permissions.
    /// </summary>
    /// <param name="command">The command containing the group details and the members and permissions to be added.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the creation result.</returns>
    public async Task<CreateGroupWithMembersAndPermissionsResult> CreateGroupWithMembersAndPermissions(CreateGroupWithMembersAndPermissionsCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await httpClient.PostAsJsonAsync("Groups/creategroupwithmembersandpermissions", command, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CreateGroupWithMembersAndPermissionsResult>(cancellationToken: cancellationToken) ?? new CreateGroupWithMembersAndPermissionsResult() { IsSuccess = false, ErrorMessages = [$"Error reading '{nameof(CreateGroupWithMembersAndPermissionsResult)}'."] };

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new CreateGroupWithMembersAndPermissionsResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(CreateGroupWithMembersAndPermissionsResult)}'."] };
        }
    }

    /// <summary>
    /// Asynchronously deletes a specific group.
    /// </summary>
    /// <param name="command">The command containing the group ID.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the delete result.</returns>
    public async Task<DeleteGroupResult> DeleteGroupAsync(DeleteGroupCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await httpClient.PostAsJsonAsync("Groups/deletegroup", command, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<DeleteGroupResult>(cancellationToken: cancellationToken) ?? new DeleteGroupResult() { IsSuccess = false, ErrorMessages = [$"Error reading '{nameof(DeleteGroupResult)}'."] };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new DeleteGroupResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(DeleteGroupAsync)}'."] };
        }
    }

    
}
