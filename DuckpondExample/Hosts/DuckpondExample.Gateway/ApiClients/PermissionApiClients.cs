using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.ResultModels;

namespace DuckpondExample.Gateway.ApiClients;

/// <summary>
/// Client to interact with the Permission API.
/// </summary>
public class PermissionApiClient
{
    private readonly HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client to be used for making requests.</param>
    public PermissionApiClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /// <summary>
    /// Adds new permissions asynchronously.
    /// </summary>
    /// <param name="command">The command containing the permissions to be added.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of adding the permissions.</returns>
    public async Task<AddPermissionsResult> AddNewPermissions(AddPermissionsCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("Permission/addnew", command, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AddPermissionsResult>() ?? new AddPermissionsResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(AddNewPermissions)}'."] };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new AddPermissionsResult() { IsSuccess = false, ErrorMessages = [$"Error calling '{nameof(AddNewPermissions)}'."] };
        }
    }

    /// <summary>
    /// Checks if a permission exists asynchronously.
    /// </summary>
    /// <param name="hasPermissionCommand">The command containing the permission request details.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the permission request.</returns>
    public async Task<PermissionRequestResult> HasPermission(PermissionRequestCommand hasPermissionCommand, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("Identity/haspermission", hasPermissionCommand, cancellationToken);
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
