CREATE TABLE [Duckpond].[UserGroups]
(
    UserGroupID  INT IDENTITY(1,1) PRIMARY KEY,
    GroupID INT NOT NULL FOREIGN KEY REFERENCES [Duckpond].[Groups],
    UserID INT NOT NULL FOREIGN KEY REFERENCES [Duckpond].[Users]    
)
GO