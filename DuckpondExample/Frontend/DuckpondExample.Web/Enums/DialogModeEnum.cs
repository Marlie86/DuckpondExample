using System.Runtime.InteropServices;

namespace DuckpondExample.Web.Enums;

/// <summary>
/// Specifies the mode of the dialog.
/// </summary>
public enum DialogModeEnum
{
    /// <summary>
    /// The dialog is in create mode.
    /// </summary>
    Create,

    /// <summary>
    /// The dialog is in edit mode.
    /// </summary>
    Edit,

    /// <summary>
    /// The dialog is in view mode.
    /// </summary>
    View
}
