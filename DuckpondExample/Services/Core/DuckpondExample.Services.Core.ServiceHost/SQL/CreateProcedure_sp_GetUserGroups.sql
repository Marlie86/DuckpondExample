CREATE PROCEDURE [Duckpond].[sp_GetUserGroups]
    @UserIDToSearch INT
AS
BEGIN
   SELECT 
    g.GroupID, 
    g.Name AS GroupName,
    CASE WHEN EXISTS (
        SELECT bd.UserID FROM [Duckpond].[vw_UserGroups] AS bd WHERE bd.UserID = @UserIDToSearch AND bd.GroupID = g.GroupID
    ) THEN 1 ELSE 0 END AS IsSelected
    FROM [Duckpond].[Groups] AS g 
END
GO