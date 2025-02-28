using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DuckpondExample.Shared.Common.Hosts.Utilities;
public class JwtUtility
{
    private readonly ILogger<JwtUtility> logger;
    private readonly IConfiguration configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtUtility"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="configuration">The configuration instance.</param>
    public JwtUtility(ILogger<JwtUtility> logger, IConfiguration configuration)
    {
        this.logger = logger;
        this.configuration = configuration;
    }

    /// <summary>
    /// Generates an access token with the specified claims.
    /// </summary>
    /// <param name="claims">The claims to include in the token.</param>
    /// <returns>The generated access token as a string.</returns>
    public string GenerateAccessToken(List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key") ?? "123"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("Jwt:Issuer"),
            audience: configuration.GetValue<string>("Jwt:Audience"),
            claims: claims,
            expires: DateTime.Now.AddMinutes(configuration.GetValue<int>("Jwt:ExpireMinutes")),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Generates a refresh token.
    /// </summary>
    /// <returns>The generated refresh token as a string.</returns>
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// Generates a new access token from the specified refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to use for generating a new access token.</param>
    /// <returns>The generated access token as a string.</returns>
    public string GenerateAccessTokenFromRefreshToken(string refreshToken)
    {
        // Implement logic to generate a new access token from the refresh token
        // Verify the refresh token and extract necessary information (e.g., user ID)
        // Then generate a new access token

        // For demonstration purposes, return a new token with an extended expiry
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key") ?? "123"));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = configuration.GetValue<string>("Jwt:Issuer"),
            Audience = configuration.GetValue<string>("Jwt:Audience"),
            Expires = DateTime.Now.AddMinutes(configuration.GetValue<int>("Jwt:ExpireMinutes")), // Extend expiration time
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
