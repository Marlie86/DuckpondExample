using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Shared.Common.BaseClasses;

namespace DuckpondExample.Services.Core.Shared.ResultModels;
/// <summary>
/// Represents the result of a request to get user.
/// </summary>
public class GetUserResult : BaseResult
{
    /// <summary>
    /// Gets or sets the collection of user.
    /// </summary>
    public IEnumerable<UserModel> Users { get; set; } = [];
}
