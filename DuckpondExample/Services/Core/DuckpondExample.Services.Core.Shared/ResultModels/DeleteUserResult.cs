using DuckpondExample.Shared.Common.BaseClasses;

namespace DuckpondExample.Services.Core.Shared.ResultModels;

/// <summary>
/// Represents the result of a request to delete a user.
/// </summary>
public class DeleteUserResult : BaseResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the user was successfully deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
}
