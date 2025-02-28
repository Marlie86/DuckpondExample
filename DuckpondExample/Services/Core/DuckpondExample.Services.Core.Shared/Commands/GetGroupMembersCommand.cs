using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckpondExample.Services.Core.Shared.Commands;
/// <summary>
/// Command to get the members of a specific group.
/// </summary>
public class GetGroupMembersCommand : IRequest<GetGroupMembersResult>
{
    /// <summary>
    /// Gets or sets the ID of the group.
    /// </summary>
    public int GroupID { get; set; } = -1;
}
