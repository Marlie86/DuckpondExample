using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckpondExample.Services.Core.Shared.Commands;
/// <summary>
/// Represents a command to request permission for a specific user.
/// </summary>
public class PermissionRequestCommand : IRequest<PermissionRequestResult>
{
    /// <summary>
    /// Gets or sets the permission to be requested.
    /// </summary>
    public string Permission { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the user requesting the permission.
    /// </summary>
    public int UserID { get; set; } = 0;
}
