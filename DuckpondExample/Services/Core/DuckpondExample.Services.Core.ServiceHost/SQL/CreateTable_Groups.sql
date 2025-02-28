CREATE TABLE [Duckpond].[Groups]
(
    GroupID INT IDENTITY(1,1) PRIMARY KEY,   
    Name NVARCHAR(24) NOT NULL UNIQUE,     
    Description NVARCHAR(255) NULL,  
    IsDefault BIT NOT NULL DEFAULT 0
)
GO
