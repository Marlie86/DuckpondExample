using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Shared.Common.Hosts.Repositories;

namespace DuckpondExample.Services.Core.ServiceHost.Repositories;

/// <summary>
/// Repository for reading group permissions from the database.
/// </summary>
/// <remarks>
/// This repository provides methods to retrieve group permissions using Dapper.
/// </remarks>
[AddAsService(ServiceLifetime.Singleton)]
public class GroupPermissionReadRepository : GenericReaderRepository<GroupPermissionRead>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupPermissionReadRepository"/> class.
    /// </summary>
    /// <param name="logger">The logger instance to use for logging.</param>
    /// <param name="context">The Dapper context for database connections.</param>
    /// <param name="tableName">The name of the table. If not provided, the table name is derived from the entity type.</param>
    public GroupPermissionReadRepository(ILogger<GroupPermissionRead> logger, DapperContext context, string tableName = "") : base(logger, context, tableName)
    {
    }

    /// <summary>
    /// Retrieves all permissions for a specified group.
    /// </summary>
    /// <param name="groupID">The ID of the group to retrieve permissions for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="GroupPermissionRead"/> objects.</returns>
    /// <remarks>
    /// This method executes a stored procedure to get the permissions for the specified group.
    /// </remarks>
    public async Task<IEnumerable<GroupPermissionRead>> GetGroupPermissionsForGroup(int groupID)
    {
        try
        {
            logger.LogInformation($"Trying to get all permissions for GroupID {groupID}.");
            var sql = $"EXEC [Duckpond].[sp_GetGroupPermissions] @GroupIDToSearch = @GroupID";
            return await ExecuteQuery(sql, new { GroupID = groupID });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new List<GroupPermissionRead>();
        }
    }
}
