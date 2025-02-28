CREATE TABLE [Duckpond].[GroupPermissions]
(
    GroupPermissionID  INT IDENTITY(1,1) PRIMARY KEY,
    GroupID INT NOT NULL FOREIGN KEY REFERENCES [Duckpond].[Groups],
    PermissionID INT NOT NULL FOREIGN KEY REFERENCES [Duckpond].[Permissions]    
)
GO