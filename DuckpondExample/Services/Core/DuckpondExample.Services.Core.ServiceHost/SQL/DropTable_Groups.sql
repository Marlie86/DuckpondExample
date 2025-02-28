IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Duckpond].[Groups]') AND type in (N'U'))
    PRINT 'Drop Table [Duckpond].[Groups]'
    DROP TABLE [Duckpond].[Groups]
GO




