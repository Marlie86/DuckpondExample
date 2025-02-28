using AutoMapper;

using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Services.Core.Shared.ResultModels;
using DuckpondExample.Shared.Common.Hosts.Utilities;

using MediatR;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Handles the creation of a new user.
/// </summary>
/// <param name="request">The command containing the user details to be created.</param>
/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
/// <returns>A task that represents the asynchronous operation. The task result contains the result of the user creation.</returns>
public class CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, IMapper mapper, IMediator mediator, UserRepository usersRepository) : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    /// <summary>
    /// Handles the creation of a new user.
    /// </summary>
    /// <param name="request">The command containing the user details to be created.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the user creation.</returns>
    public async Task<CreateUserResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userToCreate = mapper.Map<User>(request.User);

            if (userToCreate == null)
            {
                logger.LogWarning("Error mapping user to create.");
                return new CreateUserResult() { IsSuccess = false, ErrorMessages = ["Error mapping user to create."] };
            }

            userToCreate.CreatedWhen = DateTime.Now;
            userToCreate.CreatedBy = 1; // Todo : Get the current user ID
            userToCreate.LastEditedWhen = DateTime.Now;
            userToCreate.LastEditedBy = 1; // Todo : Get the current user ID

            if (!string.IsNullOrWhiteSpace(request.User.Password))
            {
                userToCreate.HashedPassword = HashUtility.HashPassword(request.User.Password);
            }
            var newId = await usersRepository.InsertAsync(userToCreate);
            if (newId == -1)
            {
                logger.LogWarning($"Error creating user '{request.User.Username}'.");
                return new CreateUserResult() { IsSuccess = false, ErrorMessages = [$"Error creating user '{request.User.Username}'."] };
            }
            var createdUser = await usersRepository.GetByIdAsync(newId);
            if (createdUser == null)
            {
                logger.LogWarning($"Error fetching user '{request.User.Username}'.");
                return new CreateUserResult() { IsSuccess = false, ErrorMessages = [$"Error fetching user '{request.User.Username}'."] };
            }
            var groupUpdateResult = await mediator.Send(new UpdateUserGroupsCommand() { UserID = createdUser.UserID, Groups = request.Groups });
            if (!groupUpdateResult.IsSuccess)
            {
                logger.LogWarning($"Error updating groups for user '{request.User.Username}'.");
                return new CreateUserResult() { IsSuccess = false, ErrorMessages = [$"Error updating groups for user '{request.User.Username}'."] };
            }
            return new CreateUserResult() { IsSuccess = true, User = mapper.Map<UserModel>(createdUser) };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error creating user '{request.User.Username}'.");
            return new CreateUserResult() { IsSuccess = false, ErrorMessages = [$"Error creating user '{request.User.Username}'."] };
        }
    }
}
