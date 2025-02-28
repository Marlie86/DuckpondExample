using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Web.UtilityServices;

namespace DuckpondExample.Web.ApiClients;

/// <summary>
/// Base class for API clients providing common functionality such as setting the authorization header.
/// </summary>
public class ApiClientBase
{
    protected HttpClient httpClient;
    private readonly StorageService storageService;

    /// <summary>
    /// Gets the HTTP client instance.
    /// </summary>
    public HttpClient HttpClient { get => httpClient; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClientBase"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client instance.</param>
    /// <param name="storageService">The storage service instance.</param>
    public ApiClientBase(HttpClient httpClient, StorageService storageService)
    {
        this.httpClient = httpClient;
        this.storageService = storageService;
        this.httpClient.Timeout = TimeSpan.FromMinutes(1);
    }

    /// <summary>
    /// Sets the authorization header for the HTTP client if it is not already set.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task SetAuthorizationHeader()
    {
        if (httpClient.DefaultRequestHeaders.Authorization == null)
        {
            var token = await storageService.GetItemAsync<TokenData>("Token");
            lock (httpClient.DefaultRequestHeaders)
            {
                if (token != null && !string.IsNullOrEmpty(token.Token))
                {
                    if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
                    {
                        httpClient.DefaultRequestHeaders.Remove("Authorization");
                    }
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Token}");
                }
            }
        }
    }
}
