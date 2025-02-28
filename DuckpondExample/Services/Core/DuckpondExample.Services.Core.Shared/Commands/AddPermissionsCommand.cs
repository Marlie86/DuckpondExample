using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Services.Core.Shared.Enums;
using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

namespace DuckpondExample.Services.Core.Shared.Commands;
/// <summary>
/// Command to add permissions to a specified target.
/// </summary>
public class AddPermissionsCommand : IRequest<AddPermissionsResult>
{
    /// <summary>
    /// Gets or sets the target for the permissions.
    /// </summary>
    public PermissionTargetEnum Target { get; set; }

    /// <summary>
    /// Gets or sets the collection of permissions to be added.
    /// </summary>
    public IEnumerable<Permission> Permissions { get; set; } = [];
}
