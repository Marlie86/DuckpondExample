using DuckpondExample.Shared.Common.BaseClasses;

namespace DuckpondExample.Services.Core.Shared.ResultModels;

/// <summary>
/// Represents the result of a request to delete a group.
/// </summary>
public class DeleteGroupResult : BaseResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the group was successfully deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
}
