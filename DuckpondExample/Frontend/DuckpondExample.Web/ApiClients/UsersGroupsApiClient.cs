using Duckpond.Aspire.Web.UtilityServices;

using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Services.Core.Shared.ResultModels;
using DuckpondExample.Web.UtilityServices;

using System.Net.Http.Json;

namespace DuckpondExample.Web.ApiClients;

/// <summary>
/// API client for managing user groups.
/// </summary>
public class UsersGroupsApiClient : ApiClientBase
{
    private UserClaimsService userClaimsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersGroupsApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client instance.</param>
    /// <param name="storageService">The storage service instance.</param>
    public UsersGroupsApiClient(HttpClient httpClient, StorageService storageService, UserClaimsService userClaimsService) : base(httpClient, storageService)
    {
        this.userClaimsService = userClaimsService;
    }


    /// <summary>
    /// Gets the user groups.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="GetUserResult"/>.</returns>
    public async Task<GetUserGroupReadResult> GetUserGroups(int userId, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await httpClient.PostAsJsonAsync("UsersGroups/getusersgroups", new GetUserGroupReadCommand() { UserID = userId }, cancellationToken: cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetUserGroupReadResult>(cancellationToken: cancellationToken) ?? new GetUserGroupReadResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(GetUserGroupReadResult)}'."] };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new GetUserGroupReadResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(GetUserGroupReadResult)}'."] };
        }
    }

    /// <summary>
    /// Updates the user groups.
    /// </summary>
    /// <param name="userModel">The user model containing the user's details.</param>
    /// <param name="groups">The groups to update for the user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="UpdateUserGroupsResult"/>.</returns>
    public async Task<UpdateUserGroupsResult> UpdateUserGroups(UserModel userModel, IEnumerable<UserGroupRead> groups, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();

            var response = await httpClient.PostAsJsonAsync("UsersGroups/updateusersgroups", new UpdateUserGroupsCommand() { UserID = userModel.UserID, Groups = groups }, cancellationToken: cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UpdateUserGroupsResult>(cancellationToken: cancellationToken) ?? new UpdateUserGroupsResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(UpdateUserGroupsResult)}'."] };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new UpdateUserGroupsResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(UpdateUserGroupsResult)}'."] };
        }
    }
}
