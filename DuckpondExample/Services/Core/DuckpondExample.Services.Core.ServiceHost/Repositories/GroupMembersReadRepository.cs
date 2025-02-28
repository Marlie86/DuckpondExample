using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Shared.Common.Hosts.Repositories;

namespace DuckpondExample.Services.Core.ServiceHost.Repositories;

/// <summary>
/// Repository for reading group members data.
/// </summary>
/// <remarks>
/// This repository provides methods to read group members from the database.
/// </remarks>
[AddAsService(ServiceLifetime.Singleton)]
public class GroupMembersReadRepository : GenericReaderRepository<GroupMembersRead>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupMembersReadRepository"/> class.
    /// </summary>
    /// <param name="logger">The logger instance to use for logging.</param>
    /// <param name="context">The Dapper context to use for database connections.</param>
    /// <param name="tableName">The name of the table. If not provided, the table name is derived from the entity type.</param>
    public GroupMembersReadRepository(ILogger<GroupMembersRead> logger, DapperContext context, string tableName = "") : base(logger, context, tableName)
    {
    }

    /// <summary>
    /// Gets the group members for a specified group.
    /// </summary>
    /// <param name="groupID">The ID of the group to get members for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="GroupMembersRead"/>.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving the group members.</exception>
    public async Task<IEnumerable<GroupMembersRead>> GetGroupMembersForGroup(int groupID)
    {
        try
        {
            logger.LogInformation($"Trying to get all users groups for GroupID {groupID}.");
            var sql = $"EXEC [Duckpond].[sp_GetGroupMembers] @GroupIDToSearch = @GroupID";
            return await ExecuteQuery(sql, new { GroupID = groupID });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new List<GroupMembersRead>();
        }
    }
}
