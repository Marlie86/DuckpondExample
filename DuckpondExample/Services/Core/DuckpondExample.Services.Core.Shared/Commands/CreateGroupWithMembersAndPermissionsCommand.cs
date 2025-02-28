using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckpondExample.Services.Core.Shared.Commands;
public class CreateGroupWithMembersAndPermissionsCommand : IRequest<CreateGroupWithMembersAndPermissionsResult>
{
    /// <summary>
    /// Gets or sets the group to be updated.
    /// </summary>
    public Group Group { get; set; } = new Group();

    /// <summary>
    /// Gets or sets the members of the group that have changed.
    /// </summary>
    public IEnumerable<GroupMembersRead> ChangedMembers { get; set; } = [];

    /// <summary>
    /// Gets or sets the permissions of the group that have changed.
    /// </summary>
    public IEnumerable<GroupPermissionRead> ChangedPermissions { get; set; } = [];
}
