using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckpondExample.Services.Core.Shared.DataItems;

/// <summary>
/// Represents a read-only view of group members.
/// </summary>
public class GroupMembersRead
{
    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    public string UserID { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the logon name of the user.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the user is selected.
    /// </summary>
    public bool IsSelected { get; set; }
}
