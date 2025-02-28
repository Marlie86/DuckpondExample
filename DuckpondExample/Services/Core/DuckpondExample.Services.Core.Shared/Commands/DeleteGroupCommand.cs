using DuckpondExample.Services.Core.Shared.ResultModels;
using MediatR;

namespace DuckpondExample.Services.Core.Shared.Commands;

/// <summary>
/// Represents a command to delete a group.
/// </summary>
public class DeleteGroupCommand : IRequest<DeleteGroupResult>
{
    /// <summary>
    /// Gets or sets the ID of the group to be deleted.
    /// </summary>
    public int GroupId { get; set; }
}
