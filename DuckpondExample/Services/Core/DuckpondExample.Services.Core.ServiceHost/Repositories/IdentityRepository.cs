using Dapper;

using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Shared.Common.Hosts.Repositories;

namespace DuckpondExample.Services.Core.ServiceHost.Repositories;

/// <summary>
/// Repository for managing identity data.
/// </summary>
[AddAsService(ServiceLifetime.Singleton)]
public class IdentityRepository : GenericRepository<Shared.DataItems.LogonUsers>
{
    private readonly ILogger<Shared.DataItems.LogonUsers> logger;
    private readonly DapperContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityRepository"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="context">The Dapper context instance.</param>
    public IdentityRepository(ILogger<Shared.DataItems.LogonUsers> logger, DapperContext context) : base(logger, context)
    {
        this.logger = logger;
        this.context = context;
    }

    /// <summary>
    /// Gets an identity by the logon name.
    /// </summary>
    /// <param name="username">The logon name of the identity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the identity if found; otherwise, null.</returns>
    public async Task<Shared.DataItems.LogonUsers?> GetIdentityByUsernameAsync(string username)
    {
        var query = $"SELECT * FROM [Duckpond].[vw_LogonUsers] WHERE Username = @Username";
        using (var connection = context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Shared.DataItems.LogonUsers>(query, new { Username = username });
        }
    }
}
