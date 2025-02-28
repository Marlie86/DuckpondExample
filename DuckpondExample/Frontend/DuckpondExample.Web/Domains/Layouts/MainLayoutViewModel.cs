using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Web.Models;
using DuckpondExample.Web.UtilityServices;

using MudBlazor;

namespace DuckpondExample.Web.Domains.Layouts;

/// <summary>
/// ViewModel for the main layout of the application.
/// Manages the state of the drawer, user menu, logon name, breadcrumbs, and current theme.
/// </summary>
/// <param name="logger">Logger instance for logging.</param>
/// <param name="localStorage">Service for accessing local storage.</param>
/// <param name="configuration">Configuration settings for the application.</param>
[AddAsService(ServiceLifetime.Scoped)]
public class MainLayoutViewModel(ILogger<MainLayoutViewModel> logger, StorageService localStorage, IConfiguration configuration) : BaseViewModel
{
    /// <summary>
    /// Gets or sets a value indicating whether the drawer is open.
    /// </summary>
    public bool DrawerOpen { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the user menu is open.
    /// </summary>
    public bool UserMenuOpen { get; set; } = false;

    /// <summary>
    /// Gets or sets the logon name of the user.
    /// </summary>
    public string Username { get; set; } = "-- unset --";

    /// <summary>
    /// Gets or sets the breadcrumbs for the current navigation path.
    /// </summary>
    public IReadOnlyList<BreadcrumbItem> Breadcrumbs { get; set; } = new List<BreadcrumbItem>();

    /// <summary>
    /// Gets or sets the current theme of the application.
    /// </summary>
    public MudTheme CurrentTheme { get; set; } = new DefaultColorTheme();

    /// <summary>
    /// Initializes the ViewModel by loading user claims from local storage.
    /// </summary>
    public async Task InitalizeAsync()
    {
        var claims = await localStorage.GetItemAsync<Dictionary<string, string>>("UserClaims");
        if (claims != null && claims.TryGetValue("Username", out var Username))
        {
            if (!string.IsNullOrEmpty(Username))
            {
                this.Username = Username;
            }
        }
    }

    /// <summary>
    /// Toggles the state of the drawer.
    /// </summary>
    public void DrawerToggle()
    {
        DrawerOpen = !DrawerOpen;
    }

    /// <summary>
    /// Toggles the state of the user menu.
    /// </summary>
    public void UserMenuToggle()
    {
        UserMenuOpen = !UserMenuOpen;
    }
}
