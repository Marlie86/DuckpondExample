using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;
using DuckpondExample.Web.UtilityServices;

using System.Net.Http.Json;

namespace DuckpondExample.Web.ApiClients;

/// <summary>
/// Client for interacting with the User API.
/// </summary>
public class UserApiClient : ApiClientBase
{
    /// <summary>
    /// Gets the HTTP client used for making requests.
    /// </summary>
    public HttpClient HttpClient { get => httpClient; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client instance.</param>
    /// <param name="storageService">The storage service instance.</param>
    public UserApiClient(HttpClient httpClient, StorageService storageService) : base(httpClient, storageService)
    {
    }

    /// <summary>
    /// Gets a user by their ID.
    /// </summary>
    /// <param name="command">The command containing the ID of the user to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user result.</returns>
    public async Task<UserResult> GetUserByIdAsync(GetUserByIdCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await httpClient.PostAsJsonAsync("Users/getbyid", command, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserResult>(cancellationToken: cancellationToken) ?? new UserResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(GetUserByIdAsync)}'."] };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new UserResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(GetUserByIdAsync)}'."] };
        }
    }

    /// <summary>
    /// Updates a user's details.
    /// </summary>
    /// <param name="command">The command containing the user's details to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the update user result.</returns>
    public async Task<UpdateUserResult> UpdateUser(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await httpClient.PostAsJsonAsync("Users/update", command, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UpdateUserResult>(cancellationToken: cancellationToken) ?? new UpdateUserResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(UpdateUser)}'."] };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new UpdateUserResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(UpdateUser)}'."] };
        }
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the get users result.</returns>
    public async Task<GetUserResult> GetUsers(CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await httpClient.GetFromJsonAsync<GetUserResult>("Users/getall", cancellationToken: cancellationToken);
            return response ?? throw new NullReferenceException("Error fetching all users.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new GetUserResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(GetUsers)}'."] };
        }
    }
    /// <summary>
    /// Create a user's details.
    /// </summary>
    /// <param name="command">The command containing the user's details to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the update user result.</returns>
    public async Task<CreateUserResult> CreateUser(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await httpClient.PostAsJsonAsync("Users/create", command, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CreateUserResult>(cancellationToken: cancellationToken) ?? new CreateUserResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(UpdateUser)}'."] };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new CreateUserResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(CreateUser)}'."] };
        }
    }
    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="command">The command containing the ID of the user to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the delete user result.</returns>
    public async Task<DeleteUserResult> DeleteUser(DeleteUserCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await httpClient.PostAsJsonAsync("Users/delete", command, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<DeleteUserResult>(cancellationToken: cancellationToken) ?? new DeleteUserResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(DeleteUser)}'."] };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new DeleteUserResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(DeleteUser)}'."] };
        }
    }
    
}
