using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Web.Models;
using DuckpondExample.Web.UtilityServices;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DuckpondExample.Web.Domains.Identity;

/// <summary>
/// ViewModel for handling user logout functionality.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="localStorageService">The service for managing local storage.</param>
/// <param name="navigationManager">The navigation manager for handling redirections.</param>
/// <param name="authenticationStateProvider">The authentication state provider for managing authentication state.</param>
[AddAsService(ServiceLifetime.Scoped)]
public class LogoutViewModel(ILogger<LogoutViewModel> logger, StorageService localStorageService, NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider) : BaseViewModel
{
    /// <summary>
    /// Logs out the user by removing authentication tokens and user claims from local storage,
    /// updating the authentication state, and navigating to the home page.
    /// </summary>
    public async Task Logout()
    {
        await localStorageService.RemoveItemsAsync(new[] { "Token", "UserClaims" });
        await authenticationStateProvider.GetAuthenticationStateAsync();
        navigationManager.NavigateTo("/", true);
    }
}
