using Blazored.LocalStorage;

using DuckpondExample.Shared.Common.Extensions;
using DuckpondExample.Web.ApiClients;
using DuckpondExample.Web.Authorize;
using DuckpondExample.Web.Domains;
using DuckpondExample.Web.UtilityServices;
using DuckpondExample.Web.Validators;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MudBlazor;
using MudBlazor.Services;

namespace DuckpondExample.Web;

/// <summary>
/// The main entry point for the application.
/// </summary>
/// <param name="args">An array of command-line arguments.</param>
/// <returns>A task that represents the asynchronous operation.</returns>
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = true;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 5000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
        });
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        builder.Services.AddHttpClient<IdentityApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7091/gateway/api/");
        });
        builder.Services.AddHttpClient<UserApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7091/gateway/api/");
        });
        builder.Services.AddHttpClient<UsersGroupsApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7091/gateway/api/");
        });
        builder.Services.AddHttpClient<GroupsApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7091/gateway/api/");
        });

        builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
        builder.Services.AddAuthorizationCore();

        builder.Services.AddScoped<UserValidator>();

        builder.Services.AddSingleton<TemporaryStorageService>();
        builder.Services.AddScoped(c =>
        {
            var localStorage = c.GetRequiredService<ILocalStorageService>();
            var temporaryStorage = c.GetRequiredService<TemporaryStorageService>();
            var logger = c.GetRequiredService<ILogger<StorageService>>();
            var service = new StorageService(logger, localStorage, temporaryStorage);
            return service;
        });

        builder.AddAttributedServices();

        builder.Services.AddBlazoredLocalStorage(config =>
        {
            config.JsonSerializerOptions.WriteIndented = true;

        });

        await builder.Build().RunAsync();
    }
}
