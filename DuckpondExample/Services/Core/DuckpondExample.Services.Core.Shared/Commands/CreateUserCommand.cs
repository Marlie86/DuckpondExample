using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckpondExample.Services.Core.Shared.Commands;
/// <summary>
/// Command to create a new user.
/// </summary>
public class CreateUserCommand : IRequest<CreateUserResult>
{
    /// <summary>
    /// Gets or sets the user to be created.
    /// </summary>
    public UserModel User { get; set; } = new UserModel();

    /// <summary>
    /// Gets or sets the groups associated with the user.
    /// </summary>
    public IEnumerable<UserGroupRead> Groups { get; set; } = [];
}
