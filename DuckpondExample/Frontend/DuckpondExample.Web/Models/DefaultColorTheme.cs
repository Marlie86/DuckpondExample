using MudBlazor;

namespace DuckpondExample.Web.Models;

/// <summary>
/// Represents the default color theme for the application using MudBlazor.
/// </summary>
public class DefaultColorTheme : MudTheme
{
    public DefaultColorTheme()
    {
        PaletteLight.Primary = "#29A39F";
        PaletteLight.Secondary = "#E396DF";
        PaletteLight.Tertiary = "#92140C";
        PaletteLight.PrimaryContrastText = "#333333";
        PaletteLight.SecondaryContrastText = "#333333";
        PaletteLight.TertiaryContrastText = "#333333";
        PaletteLight.AppbarBackground = "#082120";
        PaletteLight.Black = "#000000";
        PaletteLight.White = "#FFFFFF";
        PaletteLight.Info = "#2196f3";
        PaletteLight.Success = "#4caf50";
        PaletteLight.Warning = "#ff9800";
        PaletteLight.Error = "#f44336";

        PaletteDark.Primary = "#29A39F";
        PaletteDark.Secondary = "#E396DF";
        PaletteDark.Tertiary = "#92140C";
        PaletteDark.PrimaryContrastText = "#333333";
        PaletteDark.SecondaryContrastText = "#333333";
        PaletteDark.TertiaryContrastText = "#333333";
        PaletteDark.AppbarBackground = "#082120";
        PaletteDark.Black = "#000000";
        PaletteDark.White = "#FFFFFF";
        PaletteDark.Info = "#2196f3";
        PaletteDark.Success = "#4caf50";
        PaletteDark.Warning = "#ff9800";
        PaletteDark.Error = "#f44336";
    }
}
