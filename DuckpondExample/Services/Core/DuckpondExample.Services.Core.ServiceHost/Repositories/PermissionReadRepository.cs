using Dapper;

using DuckpondExample.Services.Core.Shared.Commands;
using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Shared.Common.Hosts.Repositories;

namespace DuckpondExample.Services.Core.ServiceHost.Repositories;

/// <summary>
/// Repository for reading permissions from the database.
/// </summary>
[AddAsService(ServiceLifetime.Singleton)]
public class PermissionReadRepository : GenericReaderRepository<Permission>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionReadRepository"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="context">The Dapper context.</param>
    /// <param name="tableName">The name of the table. If not provided, the table name is derived from the entity type.</param>
    public PermissionReadRepository(ILogger<Permission> logger, DapperContext context, string tableName = "") : base(logger, context, tableName)
    {
    }

    /// <summary>
    /// Checks if a user has a specific permission.
    /// </summary>
    /// <param name="permissionRequestCommand">The permission request command containing the permission name and user ID.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the user has the specified permission.</returns>
    public async Task<bool> HasPermission(PermissionRequestCommand permissionRequestCommand)
    {
        var query = $"SELECT TOP(1) * FROM [Duckpond].[vw_UserPermissions] WHERE PermissionName = @PermissionName AND UserId = @UserID";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Permission>(query, new { PermissionName = permissionRequestCommand.Permission, UserID = permissionRequestCommand.UserID }) != null;
        }
    }
}
