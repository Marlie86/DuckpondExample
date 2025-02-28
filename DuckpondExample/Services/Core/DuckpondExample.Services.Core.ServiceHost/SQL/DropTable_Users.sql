IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Duckpond].[Users]') AND type in (N'U'))
    PRINT 'Drop Table [Duckpond].[Users]'
    DROP TABLE [Duckpond].[Users]
GO