CREATE VIEW [Duckpond].[vw_UserPermissions]
AS
    SELECT 
        per.PermissionID,
        per.PermissionName,
        g.Name AS GroupName,
        ids.UserID,
        ids.Username
    FROM [Duckpond].[Permissions] AS per
    LEFT JOIN [Duckpond].[GroupPermissions] AS gp ON gp.PermissionID  = per.PermissionID
    LEFT JOIN [Duckpond].[Groups] AS g ON g.GroupID = gp.GroupID
    LEFT JOIN [Duckpond].[UserGroups] AS pg ON pg.GroupID = gp.GroupID
    LEFT JOIN [Duckpond].[Users] AS ids ON ids.UserID = pg.UserID
GO