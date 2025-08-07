SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF OBJECT_ID(N'slipfree.ErrorLogTbl', N'U') IS NULL
CREATE TABLE slipfree.ErrorLogTbl(
   [LogId] [bigint] IDENTITY(1,1) NOT NULL,
   [ExceptionMsg] [varchar](100) NULL,
   [ExceptionType] [varchar](100) NULL,
   [ExceptionSource] [nvarchar](max) NULL,
   [ExceptionURL] [varchar](100) NULL,
   [LogDate] [datetime] NULL,
   CONSTRAINT [PK_Tbl_ExceptionLoggingToDataBase] PRIMARY KEY CLUSTERED
       (
        [LogId] ASC
           )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO 