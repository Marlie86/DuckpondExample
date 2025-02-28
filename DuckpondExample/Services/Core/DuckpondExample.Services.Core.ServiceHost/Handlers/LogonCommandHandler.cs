using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.Models;
using DuckpondExample.Services.Core.Shared.ResultModels;
using DuckpondExample.Shared.Common.Hosts.Utilities;

using MediatR;

using System.Security.Claims;

namespace DuckpondExample.Services.Core.ServiceHost.Handlers;

/// <summary>
/// Handles the logon command by verifying the user's credentials and generating JWT tokens.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="identityRepository">The repository to access identity data.</param>
/// <param name="jwtUtility">The utility for generating JWT tokens.</param>
/// <param name="configuration">The configuration settings.</param>
public class LogonCommandHandler(
        ILogger<LogonCommandHandler> logger,
        IdentityRepository identityRepository,
        JwtUtility jwtUtility,
        IConfiguration configuration
    )
    : IRequestHandler<LogonCommand, LogonResult>
{
    /// <summary>
    /// Handles the logon command.
    /// </summary>
    /// <param name="request">The logon command request containing logon name and password.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the logon result.</returns>
    public async Task<LogonResult> Handle(LogonCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Logging in user: {request.Username}");
        var foundUser = await identityRepository.GetIdentityByUsernameAsync(request.Username);
        if (foundUser == null)
        {
            logger.LogWarning($"User not found: {request.Username}");
            return new LogonResult { IsSuccess = false, ErrorMessages = ["User not found or password incorrect."] };
        }

        var isSame = HashUtility.VerifyPassword(request.Password, foundUser.HashedPassword);
        if (!isSame)
        {
            logger.LogWarning($"Password incorrect: {request.Username}");
            return new LogonResult { IsSuccess = false, ErrorMessages = ["User not found or password incorrect."] };
        }

        var claims = new List<Claim>
            {
                new Claim("UserID", foundUser.UserID.ToString()),
                new Claim("Username", foundUser.Username),
                new Claim("IsAdmin", foundUser.IsAdmin.ToString())
            };
        var token = jwtUtility.GenerateAccessToken(claims);
        var refreshToken = jwtUtility.GenerateRefreshToken();
        var data = new TokenData
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.Now.AddMinutes(configuration.GetValue<int>("Jwt:ExpireMinutes"))
        };

        logger.LogInformation($"Logged in user: {request.Username}");
        return new LogonResult { IsSuccess = true, TokenData = data };
    }
}
