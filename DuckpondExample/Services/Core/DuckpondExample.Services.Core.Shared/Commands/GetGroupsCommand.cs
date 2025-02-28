using DuckpondExample.Services.Core.Shared.ResultModels;

using MediatR;

namespace DuckpondExample.Services.Core.Shared.Commands;
/// <summary>
/// Represents a command to retrieve a list of groups.
/// </summary>
public class GetGroupsCommand : IRequest<GetGroupsResult>
{
}
