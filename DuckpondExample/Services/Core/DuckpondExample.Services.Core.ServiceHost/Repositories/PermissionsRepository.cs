using Dapper;

using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Shared.Common.Hosts.Repositories;

namespace Duckpond.Aspire.Identity.Repositories;

/// <summary>
/// Repository for managing permissions in the application.
/// </summary>
[AddAsService(ServiceLifetime.Singleton)]
public class PermissionsRepository : GenericRepository<Permission>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionsRepository"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="context">The Dapper context.</param>
    public PermissionsRepository(ILogger<Permission> logger, DapperContext context) : base(logger, context)
    {
    }

    /// <summary>
    /// Checks if a permission with the specified name exists.
    /// </summary>
    /// <param name="name">The name of the permission.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the permission exists; otherwise, false.</returns>
    public async Task<bool> PermissionExists(string name)
    {
        var query = $"SELECT TOP(1) * FROM [Duckpond].[Permissions] WHERE PermissionName = @PermissionName";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Permission>(query, new { PermissionName = name }) != null;
        }
    }
}
