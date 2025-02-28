IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Duckpond].[Permissions]') AND type in (N'U'))
    PRINT 'Drop Table [Duckpond].[Permissions]'
    DROP TABLE [Duckpond].[Permissions]
GO




