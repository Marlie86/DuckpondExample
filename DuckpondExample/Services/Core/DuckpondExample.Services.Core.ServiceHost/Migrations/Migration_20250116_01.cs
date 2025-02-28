namespace Duckpond.Aspire.Identity.Migrations;

using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Hosts.Repositories;

using FluentMigrator;

/// <summary>
/// Represents a database migration to add the IsAdmin column to the Users table and create the vw_Identities view.
/// </summary>
/// <param name="logger">The logger instance for logging migration details.</param>
/// <param name="context">The Dapper context for database connections.</param>
/// <param name="usersRepository">The repository for accessing Users entities.</param>
[Migration(202501162251)]
public class Migration_20250116_01(ILogger logger, DapperContext context, IGenericRepository<User> usersRepository) : Migration
{
    public override void Up()
    {
        logger.LogInformation("Creating Application schema.");
        Execute.Script("./SQL/CreateSchema_Duckpond.sql");

        logger.LogInformation("Creating Users table.");
        Execute.Script("./SQL/CreateTable_Users.sql");

        logger.LogInformation("Creating Groups table.");
        Execute.Script("./SQL/CreateTable_Groups.sql");

        logger.LogInformation("Creating Permissions table.");
        Execute.Script("./SQL/CreateTable_Permissions.sql");

        logger.LogInformation("Creating UserGroups table.");
        Execute.Script("./SQL/CreateTable_UserGroups.sql");

        logger.LogInformation("Creating GroupPermission table.");
        Execute.Script("./SQL/CreateTable_GroupPermissions.sql");

        logger.LogInformation("Creating vw_LogonUsers view.");
        Execute.Script("./SQL/CreateView_vw_LogonUsers.sql");

        logger.LogInformation("Creating vw_UserPermissions view.");
        Execute.Script("./SQL/CreateView_vw_UserPermissions.sql");

        logger.LogInformation("Creating vw_UserGroups view.");
        Execute.Script("./SQL/CreateView_vw_UserGroups.sql");

        logger.LogInformation("Creating sp_GetGroupMembers procedure.");
        Execute.Script("./SQL/CreateProcedure_sp_GetGroupMembers.sql");

        logger.LogInformation("Creating sp_GetGroupPermissions procedure.");
        Execute.Script("./SQL/CreateProcedure_sp_GetGroupPermissions.sql");

        logger.LogInformation("Creating sp_GetUserGroups procedure.");
        Execute.Script("./SQL/CreateProcedure_sp_GetUserGroups.sql");
    }

    public override void Down()
    {

        logger.LogInformation("Dropping Procedure sp_GetUserGroups");
        Execute.Script("./SQL/DropProcedure_sp_GetUserGroups.sql");

        logger.LogInformation("Dropping Procedure sp_GetGroupPermissions");
        Execute.Script("./SQL/DropProcedure_sp_GetGroupPermissions.sql");

        logger.LogInformation("Dropping Procedure sp_GetGroupMembers");
        Execute.Script("./SQL/DropProcedure_sp_GetGroupMembers.sql");

        logger.LogInformation("Dropping View vw_UserGroups");
        Execute.Script("./SQL/DropView_vw_UserGroups.sql");

        logger.LogInformation("Dropping View vw_UserPermissions");
        Execute.Script("./SQL/DropView_vw_UserPermissions");

        logger.LogInformation("Dropping View vw_LogonUsers");
        Execute.Script("./SQL/DropView_vw_LogonUsers.sql");

        logger.LogInformation("Dropping Table GroupPermission");
        Execute.Script("./SQL/DropTable_GroupPermission.sql");

        logger.LogInformation("Dropping Table UserGroups");
        Execute.Script("./SQL/DropTable_UserGroups.sql");

        logger.LogInformation("Dropping Table Permissions");
        Execute.Script("./SQL/DropTable_Permissions.sql");

        logger.LogInformation("Dropping Table Groups");
        Execute.Script("./SQL/DropTable_Groups.sql");

        logger.LogInformation("Dropping Table Users");
        Execute.Script("./SQL/DropTable_Users.sql");

        logger.LogInformation("Dropping Schema Application");
        Execute.Script("./SQL/CreateScheme_Duckpond.sql");
    }
}
