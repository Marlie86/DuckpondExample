IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Duckpond].[GroupPermissions]') AND type in (N'U'))
    PRINT 'Drop Table [Duckpond].[GroupPermissions]'
    DROP TABLE [Duckpond].[GroupPermissions]
GO