using Dapper;

using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Attributes;
using DuckpondExample.Shared.Common.Hosts.Repositories;

namespace DuckpondExample.Services.Core.ServiceHost.Repositories;

/// <summary>
/// Repository for managing group entities and their members.
/// </summary>
/// <remarks>
/// This repository provides methods to add and delete group members.
/// </remarks>
[AddAsService(ServiceLifetime.Singleton)]
public class GroupsRepository : GenericRepository<Group>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupsRepository"/> class.
    /// </summary>
    /// <param name="logger">The logger instance used for logging information.</param>
    /// <param name="context">The Dapper context used for database connections.</param>
    public GroupsRepository(ILogger<Group> logger, DapperContext context) : base(logger, context)
    {
    }

    /// <summary>
    /// Adds a member to a specified group asynchronously.
    /// </summary>
    /// <param name="groupID">The identifier of the group to which the member will be added.</param>
    /// <param name="member">The member to be added to the group.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows.</returns>
    /// <remarks>
    /// Logs information about the operation and checks the number of affected rows to ensure it is exactly one.
    /// </remarks>
    public async Task<int> AddGroupMemberAsync(int groupID, GroupMembersRead member)
    {
        logger.LogInformation("Adding group member {@Member} to group {GroupID}", member, groupID);
        var sql = @"
            INSERT INTO [Duckpond].[UserGroups] (GroupID, UserID)
            VALUES (@GroupID, @UserID)";

        var parameters = new
        {
            GroupID = groupID,
            UserID = member.UserID
        };

        using (var connection = _context.CreateConnection())
        {
            var affectedRows = await connection.ExecuteAsync(sql, parameters);
            if (affectedRows == 0)
            {
                logger.LogWarning($"Number of affected rows is 0 it should be 1.");
            }
            else if (affectedRows >= 1)
            {
                logger.LogError($"Number of affected rows is greater then 1, it should be 1.");
            }
            return affectedRows;
        }
    }

    /// <summary>
    /// Deletes a member from a specified group asynchronously.
    /// </summary>
    /// <param name="groupID">The identifier of the group from which the member will be deleted.</param>
    /// <param name="member">The member to be deleted from the group.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows.</returns>
    /// <remarks>
    /// Logs information about the operation and checks the number of affected rows to ensure it is exactly one.
    /// </remarks>
    public async Task<int> DeleteGroupMemberAsync(int groupID, GroupMembersRead member)
    {
        logger.LogInformation("Deleting group member {@Member} from group {GroupID}", member, groupID);
        var sql = @"
            DELETE FROM [Duckpond].[UserGroups]
            WHERE GroupID = @GroupID AND UserID = @UserID";

        var parameters = new
        {
            GroupID = groupID,
            UserID = member.UserID
        };

        using (var connection = _context.CreateConnection())
        {
            var affectedRows = await connection.ExecuteAsync(sql, parameters);
            if (affectedRows == 0)
            {
                logger.LogWarning($"Number of affected rows is 0 it should be 1.");
            }
            else if (affectedRows >= 1)
            {
                logger.LogError($"Number of affected rows is greater then 1, it should be 1.");
            }
            return affectedRows;
        }
    }

    /// <summary>
    /// Adds a permission to a specified group asynchronously.
    /// </summary>
    /// <param name="groupID">The identifier of the group to which the permission will be added.</param>
    /// <param name="permission">The permission to be added to the group.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows.</returns>
    /// <remarks>
    /// Logs information about the operation and checks the number of affected rows to ensure it is exactly one.
    /// </remarks>
    public async Task<int> AddGroupPermissionAsync(int groupID, GroupPermissionRead permission)
    {
        logger.LogInformation("Adding group member {@Member} to group {GroupID}", permission, groupID);
        var sql = @"
            INSERT INTO [Duckpond].[GroupPermissions] (GroupID, PermissionId)
            VALUES (@GroupID, @PermissionId)";

        var parameters = new
        {
            GroupID = groupID,
            PermissionId = permission.PermissionID
        };

        using (var connection = _context.CreateConnection())
        {
            var affectedRows = await connection.ExecuteAsync(sql, parameters);
            if (affectedRows == 0)
            {
                logger.LogWarning($"Number of affected rows is 0 it should be 1.");
            }
            else if (affectedRows >= 1)
            {
                logger.LogError($"Number of affected rows is greater then 1, it should be 1.");
            }
            return affectedRows;
        }
    }

    /// <summary>
    /// Deletes a permission from a specified group asynchronously.
    /// </summary>
    /// <param name="groupID">The identifier of the group from which the permission will be deleted.</param>
    /// <param name="permission">The permission to be deleted from the group.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows.</returns>
    /// <remarks>
    /// Logs information about the operation and checks the number of affected rows to ensure it is exactly one.
    /// </remarks>
    public async Task<int> DeleteGroupPermissionAsync(int groupID, GroupPermissionRead permission)
    {
        logger.LogInformation("Deleting group member {@Member} from group {GroupID}", permission, groupID);
        var sql = @"
            DELETE FROM [Duckpond].[GroupPermissions]
            WHERE GroupID = @GroupID AND PermissionID = @PermissionID";

        var parameters = new
        {
            GroupID = groupID,
            permission.PermissionID
        };

        using (var connection = _context.CreateConnection())
        {
            var affectedRows = await connection.ExecuteAsync(sql, parameters);
            if (affectedRows == 0)
            {
                logger.LogWarning($"Number of affected rows is 0 it should be 1.");
            }
            else if (affectedRows >= 1)
            {
                logger.LogError($"Number of affected rows is greater then 1, it should be 1.");
            }
            return affectedRows;
        }
    }

    /// <summary>
    /// Deletes all permissions from a specified group asynchronously.
    /// </summary>
    /// <param name="groupId">The identifier of the group from which the permissions will be deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows, or -1 if an error occurred.</returns>
    /// <remarks>
    /// Logs information about the operation and catches any exceptions that occur, logging the error message.
    /// </remarks>
    public async Task<int> DeleteGroupPermissionsAsync(int groupId)
    {
        try
        {
            logger.LogInformation("Deleting group permissions from group {GroupID}", groupId);
            var query = $"DELETE FROM [Duckpond].[GroupPermissions] WHERE GroupID = @GroupID";
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
}
