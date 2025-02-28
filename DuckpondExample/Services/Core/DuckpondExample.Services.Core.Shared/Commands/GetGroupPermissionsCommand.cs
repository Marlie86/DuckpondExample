using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckpondExample.Services.Core.Shared.Commands;
/// <summary>
/// Command to get the permissions of a specific group.
/// </summary>
public class GetGroupPermissionsCommand : IRequest<GetGroupPermissionsResult>
{
    /// <summary>
    /// Gets or sets the ID of the group.
    /// </summary>
    public int GroupID { get; set; }
}
