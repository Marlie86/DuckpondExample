using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckpondExample.Services.Core.Shared.Commands;
/// <summary>
/// Command to get the read model of users groups for a specific user.
/// </summary>
public class GetUserGroupReadCommand : IRequest<GetUserGroupReadResult>
{
    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    public int UserID { get; set; } = -1;
}
