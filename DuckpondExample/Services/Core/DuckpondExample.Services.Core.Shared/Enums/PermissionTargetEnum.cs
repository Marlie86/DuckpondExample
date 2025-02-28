using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckpondExample.Services.Core.Shared.Enums;
/// <summary>
/// Specifies the target for permissions.
/// </summary>
public enum PermissionTargetEnum
{
    /// <summary>
    /// Permission target is the server.
    /// </summary>
    Server = 0,

    /// <summary>
    /// Permission target is the client.
    /// </summary>
    Client = 1
}
