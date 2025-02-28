using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Web.ApiClients;
using DuckpondExample.Web.Models;
using DuckpondExample.Web.UtilityServices;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DuckpondExample.Web.Domains.Identity;

/// <summary>
/// ViewModel for handling user logon functionality.
/// </summary>
/// <remarks>
/// This ViewModel interacts with the Identity API to authenticate users and manage authentication state.
/// </remarks>
[AddAsService(ServiceLifetime.Scoped)]
public class LogonViewModel : BaseViewModel
{
    private readonly ILogger<LogonViewModel> logger;
    private readonly IdentityApiClient identityApiClient;
    private readonly NavigationManager navigationManager;
    private readonly AuthenticationStateProvider authStateProvider;
    private readonly StorageService localStorage;

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; } = true;
    public string ReturnUrl { get; set; } = string.Empty;

    public string[] ErrorMessages { get; set; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="LogonViewModel"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="identityApiClient">The identity API client instance.</param>
    /// <param name="navigationManager">The navigation manager instance.</param>
    /// <param name="authStateProvider">The authentication state provider instance.</param>
    /// <param name="localStorage">The local storage service instance.</param>
    public LogonViewModel(ILogger<LogonViewModel> logger, IdentityApiClient identityApiClient, NavigationManager navigationManager, AuthenticationStateProvider authStateProvider, StorageService localStorage)
    {
        this.logger = logger;
        this.identityApiClient = identityApiClient;
        this.navigationManager = navigationManager;
        this.authStateProvider = authStateProvider;
        this.localStorage = localStorage;
    }

    /// <summary>
    /// Logs on the user asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous logon operation.</returns>
    public async Task LogonAsync()
    {
        var command = new LogonCommand
        {
            Username = Username,
            Password = Password
        };
        var result = await identityApiClient.LogonAsync(command);

        await localStorage.SetUseLocalStorage(RememberMe);

        if (result.IsSuccess)
        {
            await localStorage.SetItemAsync("Token", result.TokenData);
            await authStateProvider.GetAuthenticationStateAsync();
            navigationManager.NavigateTo(ReturnUrl);
            return;
        }
        ErrorMessages = result.ErrorMessages.ToArray();
    }

    public async Task AutoLogonAsync()
    {
        Username = "admin";
        Password = "123qwe";
        RememberMe = true;
        await LogonAsync();
    }
}
