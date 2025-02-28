CREATE PROCEDURE [Duckpond].[sp_GetGroupMembers]
    @GroupIDToSearch INT
AS
BEGIN
   SELECT 
    p.UserID, 
    p.Username,
    CASE WHEN EXISTS (
        SELECT bd.UserID FROM [Duckpond].[vw_UserGroups] AS bd WHERE bd.GroupID = @GroupIDToSearch AND bd.UserID = p.UserID
    ) THEN 1 ELSE 0 END AS IsSelected
    FROM [Duckpond].[Users] AS p
    WHERE IsPermittedToLogon = 1
END
GO