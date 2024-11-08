USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[user]    Script Date: 4/11/2024 5:16:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[user](
	[username] [nvarchar](500) NOT NULL,
	[email] [nvarchar](max) NOT NULL,
	[role] [nvarchar](50) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


