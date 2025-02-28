CREATE VIEW [Duckpond].[vw_LogonUsers]
AS
    SELECT TOP (1000) 
        [UserID]     
        ,[Username]     
        ,[HashedPassword]
        ,[IsAdmin]
    FROM [Duckpond].[Users]
    WHERE IsPermittedToLogon = 1 AND IsDeactivated = 0;
GO
