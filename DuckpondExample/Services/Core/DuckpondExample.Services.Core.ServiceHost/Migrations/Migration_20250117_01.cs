using DuckpondExample.Services.Core.ServiceHost.Repositories;
using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Extensions;
using DuckpondExample.Shared.Common.Hosts.Repositories;
using DuckpondExample.Shared.Common.Hosts.Utilities;

using FluentMigrator;
using FluentMigrator.SqlServer;

namespace DuckpondExample.Services.Core.ServiceHost.Migrations;

[Migration(202501171622)]
public class Migration_20250117_01(
    ILogger logger, 
    UserRepository userRepository, 
    GroupsRepository groupsRepository,
    UserGroupsRepository userGroupsRepository) : Migration
{
    public override void Up()
    {
        logger.LogInformation("Creating default users.");
        new List<User>()
        {
            new User() {
                UserID = 1,
                Username = "roboduck",
                HashedPassword = HashUtility.HashPassword("123qwe"),
                EMailAddress = "roboduck@duckpond.local",
                IsSystemUser = true,
                IsAdmin = false,
                IsPermittedToLogon = false,
                IsExternalLogonProvider = false,
                IsDeactivated = false,
                LastEditedBy = 1,
                LastEditedWhen = DateTime.Now,
                CreatedBy = 1,
                CreatedWhen = DateTime.Now,
                TokenExpires = DateTime.Now,
                RefreshToken = ""
            },
            new User() {
                UserID = 2,
                Username = "admin",
                HashedPassword = HashUtility.HashPassword("123qwe"),
                EMailAddress = "admin@duckpond.local",
                IsSystemUser = false,
                IsAdmin = true,
                IsPermittedToLogon = true,
                IsExternalLogonProvider = false,
                IsDeactivated = false,
                LastEditedBy = 1,
                LastEditedWhen = DateTime.Now,
                CreatedBy = 1,
                CreatedWhen = DateTime.Now,
                TokenExpires = DateTime.Now,
                RefreshToken = ""
            },
        }.ForEach(user => { 
            if (user.UserID == 0 || userRepository.GetById(user.UserID) != null) return;
            Insert.IntoTable("Users").InSchema("Duckpond").Row(user).WithIdentityInsert();
        });

       
        
        logger.LogInformation("Creating default groups.");
        new List<Group>()
        {
            new Group() { GroupID = 1, Name = "System", IsDefault = false },
            new Group() { GroupID = 2, Name = "Admins", IsDefault = false },
            new Group() { GroupID = 3, Name = "Moderators", IsDefault = false },
            new Group() { GroupID = 4, Name = "Users", IsDefault = true },
        }.ForEach(group =>
        {
            if (groupsRepository.GetById(group.GroupID) != null) return;
            Insert.IntoTable("Groups").InSchema("Duckpond").Row(group).WithIdentityInsert();
        });

        logger.LogInformation("Assigning users to groups.");
        new List<UserGroup>() { 
            new UserGroup() { UserGroupID = 1, GroupID = 1, UserID = 1 },
            new UserGroup() { UserGroupID = 2, GroupID = 4, UserID = 1 },
            new UserGroup() { UserGroupID = 3, GroupID = 2, UserID = 2 },
            new UserGroup() { UserGroupID = 4, GroupID = 4, UserID = 2 },
        }.ForEach(userToGroup =>
        {
            if (userGroupsRepository.GetById(userToGroup.UserGroupID) != null) return;
            Insert.IntoTable("UserGroups").InSchema("Duckpond").Row(userToGroup).WithIdentityInsert();
        });
    }


    public override void Down()
    {
        logger.LogInformation("Deleting default users.");
        Delete.FromTable("UserGroups").InSchema("Duckpond").AllRows(); 

        logger.LogInformation("Deleting default groups.");
        Delete.FromTable("Groups").InSchema("Duckpond").AllRows();

        logger.LogInformation("Deleting default users.");
        Delete.FromTable("Users").InSchema("Duckpond").AllRows();
    }

}
