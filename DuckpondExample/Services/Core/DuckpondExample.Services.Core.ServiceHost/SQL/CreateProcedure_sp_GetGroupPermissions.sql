CREATE PROCEDURE [Duckpond].[sp_GetGroupPermissions]
    @GroupIDToSearch INT
AS
BEGIN
   SELECT 
     p.PermissionID,
     p.PermissionName,
      CASE WHEN EXISTS (
        SELECT gp.PermissionID FROM [Duckpond].[GroupPermissions] AS gp WHERE gp.GroupID = @GroupIDToSearch AND gp.PermissionID = p.PermissionID
    ) THEN 1 ELSE 0 END AS IsSelected
    FROM [Duckpond].[Permissions] AS p
    
END
GO