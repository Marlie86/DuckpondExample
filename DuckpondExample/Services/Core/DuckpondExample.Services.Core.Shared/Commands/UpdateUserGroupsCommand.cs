using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckpondExample.Services.Core.Shared.Commands;
/// <summary>
/// Command to update the groups associated with a user.
/// </summary>
public class UpdateUserGroupsCommand : IRequest<UpdateUserGroupsResult>
{
    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    public int UserID { get; set; }

    /// <summary>
    /// Gets or sets the collection of groups associated with the user.
    /// </summary>
    public IEnumerable<UserGroupRead> Groups { get; set; } = [];
}
