using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;
using DuckpondExample.Web.UtilityServices;

using System.Net.Http.Json;

namespace DuckpondExample.Web.ApiClients;

/// <summary>
/// Client to interact with the Identity API.
/// </summary>
/// <param name="httpClient">The HTTP client instance.</param>
/// <param name="storageService">The storage service instance.</param>
public class IdentityApiClient : ApiClientBase
{
    public IdentityApiClient(HttpClient httpClient, StorageService storageService) : base(httpClient, storageService)
    {
    }

    /// <summary>
    /// Logs on a user asynchronously.
    /// </summary>
    /// <param name="command">The logon command containing user credentials.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the logon result.</returns>
    public async Task<LogonResult> LogonAsync(LogonCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await HttpClient.PostAsJsonAsync("Identity/logon", command, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<LogonResult>() ?? new LogonResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(LogonAsync)}'."] };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new LogonResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(LogonAsync)}'."] };
        }
    }

    /// <summary>
    /// Checks if a user has a specific permission asynchronously.
    /// </summary>
    /// <param name="hasPermissionCommand">The command containing the permission request details.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the permission request result.</returns>
    public async Task<PermissionRequestResult> HasPermission(PermissionRequestCommand hasPermissionCommand, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await HttpClient.PostAsJsonAsync("Identity/haspermission", hasPermissionCommand, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PermissionRequestResult>() ?? new PermissionRequestResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(HasPermission)}'."] };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new PermissionRequestResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(HasPermission)}'."] };
        }
    }
}
