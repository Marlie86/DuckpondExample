using Dapper;

using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Shared.Common.Hosts.Repositories;

namespace DuckpondExample.Services.Core.ServiceHost.Repositories;

[AddAsService(ServiceLifetime.Singleton)]
public class UserGroupsRepository : GenericRepository<UserGroup>
{

    /// <summary>
    /// Initializes a new instance of the <see cref="UserGroupsRepository"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="context">The Dapper context.</param>
    public UserGroupsRepository(ILogger<UserGroup> logger, DapperContext context) : base(logger, context)
    {
    }

    /// <summary>
    /// Checks if a user is already in a specific group.
    /// </summary>
    /// <param name="groupid">The identifier of the group.</param>
    /// <param name="userId">The identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the user is in the group; otherwise, false.</returns>
    public async Task<bool> IsExisting(int groupid, int userId)
    {
        var query = $"SELECT TOP(1) * FROM {_tableName} WHERE GroupID = @GroupID AND UserID = @UserID";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<UserGroup>(query, new { GroupID = groupid, UserID = userId }) != null;
        }
    }

    /// <summary>
    /// Updates the user groups asynchronously.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="groups">The collection of groups to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows.</returns>
    public async Task<bool> UpdateUsersGroupsAsync(int userId, IEnumerable<UserGroupRead> groups)
    {
        try
        {

            foreach (var group in groups)
            {
                var sql = string.Empty;
                if (!group.IsSelected)
                {
                    await DeleteAsync(new UserGroup() { GroupID = group.GroupID, UserID = userId });
                }
                else if (group.IsSelected)
                {
                    if (await IsExisting(group.GroupID, userId))
                    {
                        continue;
                    }
                    await InsertAsync(new UserGroup() { GroupID = group.GroupID, UserID = userId });
                }
            }
            return true;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Deletes an entity by its identifier asynchronously.
    /// </summary>
    /// <param name="usersGroup">The users group entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the entity was deleted; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(UserGroup usersGroup)
    {
        var query = $"DELETE FROM {_tableName} WHERE GroupID = @GroupID AND UserID = @UserID";
        using (var connection = _context.CreateConnection())
        {
            var result = await connection.ExecuteAsync(query, usersGroup);
            return result > 0;
        }
    }

    /// <summary>
    /// Deletes all members of a specific group asynchronously.
    /// </summary>
    /// <param name="groupId">The identifier of the group.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows, or -1 if an error occurred.</returns>
    public async Task<int> DeleteGroupMembersAsync(int groupId)
    {
        try
        {
            logger.LogInformation("Deleting group members from group {GroupID}", groupId);
            var query = $"DELETE FROM {_tableName} WHERE GroupID = @GroupID";
            logger.LogInformation($"Executing query: {query}");
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { GroupID = groupId });
                return result;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return -1;
        }
    }

    /// <summary>
    /// Deletes all user groups associated with a specific user asynchronously.
    /// </summary>
    /// <param name="userid">The identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows, or -1 if an error occurred.</returns>
    public async Task<int> DeleteAllUserGroupsByUserIdAsnyc(int userid)
    {
        try
        {
            logger.LogInformation("Deleting entries with {UserID}", userid);
            var query = $"DELETE FROM {_tableName} WHERE UserID = @UserID";
            logger.LogInformation($"Executing query: {query}");
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { UserID = userid });
                return result;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return -1;
        }
    }
}
