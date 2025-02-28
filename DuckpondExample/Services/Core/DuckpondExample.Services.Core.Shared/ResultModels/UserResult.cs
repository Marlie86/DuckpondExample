using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Shared.Common.BaseClasses;

namespace DuckpondExample.Services.Core.Shared.ResultModels;
/// <summary>
/// Represents the result of an operation involving a user.
/// </summary>
public class UserResult : BaseResult
{
    /// <summary>
    /// Gets or sets the user involved in the operation.
    /// </summary>
    public UserModel? User { get; set; } = null;
}
