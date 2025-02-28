using Dapper;

using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Shared.Common.Hosts.Repositories;

namespace Duckpond.Aspire.Identity.Repositories;

/// <summary>
/// Repository for reading users groups data.
/// </summary>
/// <remarks>
/// This repository provides methods to retrieve users groups for a specific user and group members for a specific group.
/// </remarks>
[AddAsService(ServiceLifetime.Singleton)]
public class UserGroupsReadRepository : GenericReaderRepository<UserGroupRead>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserGroupsReadRepository"/> class.
    /// </summary>
    /// <param name="logger">The logger instance to use for logging.</param>
    /// <param name="context">The Dapper context for database connections.</param>
    /// <param name="tableName">The name of the table. If not provided, the table name is derived from the entity type.</param>
    public UserGroupsReadRepository(ILogger<UserGroupRead> logger, DapperContext context, string tableName = "") : base(logger, context, tableName)
    {
    }

    /// <summary>
    /// Retrieves all users groups associated with a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="UserGroupRead"/> entities.</returns>
    /// <remarks>
    /// This method executes a stored procedure to fetch the users groups for the specified user.
    /// </remarks>
    public async Task<IEnumerable<UserGroupRead>> GetUsersGroupsForUserAsync(int userId)
    {
        try
        {
            logger.LogInformation($"Trying to get all users groups for UserID {userId}.");
            var sql = $"EXEC [Duckpond].[sp_GetUserGroups] @UserIDToSearch = @UserID";
            return await ExecuteQuery(sql, new { UserID = userId });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new List<UserGroupRead>();
        }
    }
}
