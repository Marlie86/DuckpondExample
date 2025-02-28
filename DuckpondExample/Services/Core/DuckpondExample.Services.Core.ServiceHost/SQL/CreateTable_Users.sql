CREATE TABLE [Duckpond].[Users]
(
	UserID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Username NVARCHAR(50) NOT NULL UNIQUE,
	EMailAddress NVARCHAR(255) NOT NULL UNIQUE,
	HashedPassword varbinary(max) NULL,
	IsSystemUser bit NOT NULL,
	IsAdmin bit NOT NULL,
	IsPermittedToLogon bit NOT NULL,
	IsExternalLogonProvider bit NOT NULL,
	IsDeactivated bit NOT NULL,
	RefreshToken NVARCHAR(255) NULL,
	TokenExpires DATETIME NULL,
	
    CreatedBy INT NOT NULL DEFAULT 0,
	CreatedWhen DATETIME NOT NULL DEFAULT GETDATE(),
    LastEditedBy INT NOT NULL DEFAULT 0,
	LastEditedWhen DATETIME NOT NULL DEFAULT GETDATE()
)
GO