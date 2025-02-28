IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Duckpond].[UserGroups]') AND type in (N'U'))
    PRINT 'Drop Table [Duckpond].[UserGroups]'
    DROP TABLE [Duckpond].[UserGroups]
GO