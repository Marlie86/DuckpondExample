using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Shared.Common.BaseClasses;

namespace DuckpondExample.Services.Core.Shared.ResultModels;
/// <summary>
/// Represents the result of a create user operation.
/// </summary>
public class CreateUserResult : BaseResult
{
    /// <summary>
    /// Gets or sets the created user.
    /// </summary>
    public UserModel User { get; set; } = new UserModel();
}
