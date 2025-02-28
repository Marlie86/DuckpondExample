
CREATE VIEW [Duckpond].[vw_UserGroups]
AS
    SELECT p.UserID, p.Username, g.GroupID, g.Name AS GroupName
        FROM [Duckpond].[Groups] AS g 
        LEFT JOIN [Duckpond].[UserGroups] AS pg ON pg.GroupID = g.GroupID
        LEFT JOIN [Duckpond].[Users] AS p ON p.UserID = pg.UserID
GO