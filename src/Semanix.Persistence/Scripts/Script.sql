USE [master]
GO
/****** Object:  Database [SemanixDb]    Script Date: 8/7/2025 1:39:49 PM ******/
CREATE DATABASE [SemanixDb]
GO
ALTER DATABASE [SemanixDb] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SemanixDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
USE [SemanixDb]
GO
/****** Object:  Table [dbo].[TicketEvents]    Script Date: 8/7/2025 1:39:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TicketEvents](
	[Id] [uniqueidentifier] NOT NULL,
	[TenantId] [nvarchar](max) NOT NULL,
	[EntityId] [nvarchar](max) NULL,
	[TicketId] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[TimestampUtc] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tickets]    Script Date: 8/7/2025 1:39:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tickets](
	[Id] [uniqueidentifier] NOT NULL,
	[TenantId] [nvarchar](max) NOT NULL,
	[EntityId] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Priority] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedUtc] [datetime2](7) NOT NULL,
	[SlaDeadlineUtc] [datetime2](7) NOT NULL,
	[LastStatusChangeUtc] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Tickets] ADD  DEFAULT ((0)) FOR [Status]
GO
USE [master]
GO
ALTER DATABASE [SemanixDb] SET  READ_WRITE 
GO
