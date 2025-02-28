using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Web.ApiClients;
using DuckpondExample.Web.UtilityServices;

using Microsoft.AspNetCore.Components.Authorization;

using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace DuckpondExample.Web.Authorize;

/// <summary>
/// Provides authentication state for the application.
/// </summary>
/// <param name="logger">The logger instance for logging.</param>
/// <param name="localStorage">The local storage service for storing and retrieving data.</param>
/// <param name="identityApiClient">The identity API client for interacting with the identity API.</param>
public class AuthStateProvider(ILogger<AuthStateProvider> logger, StorageService localStorage, IdentityApiClient identityApiClient) : AuthenticationStateProvider
{
    /// <summary>
    /// Gets the current authentication state asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the authentication state.</returns>
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsync<TokenData>("Token");

        var identity = new ClaimsIdentity();
        identityApiClient.HttpClient.DefaultRequestHeaders.Authorization = null;

        if (token != null && !string.IsNullOrEmpty(token?.Token))
        {
            identity = new ClaimsIdentity(ParseClaimsFromJwt(token.Token), "jwt");
            identityApiClient.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Token.Replace("\"", ""));
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);
        var claims = identity.Claims
          .GroupBy(claim => claim.Type) // Desired Key
          .SelectMany(group => group
             .Select((item, index) => group.Count() <= 1
                ? Tuple.Create(group.Key, item.Value) // One claim in group
                : Tuple.Create($"{group.Key}_{index + 1}", item.Value) // Many claims
              ))
          .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
        await localStorage.SetItemAsync("UserClaims", claims);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    /// <summary>
    /// Parses claims from a JWT token.
    /// </summary>
    /// <param name="jwt">The JWT token.</param>
    /// <returns>An enumerable of claims extracted from the JWT token.</returns>
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    /// <summary>
    /// Parses a base64 string without padding.
    /// </summary>
    /// <param name="base64">The base64 string.</param>
    /// <returns>A byte array representation of the base64 string.</returns>
    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
