USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[admin]    Script Date: 4/11/2024 5:08:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[admin](
	[adminId] [nvarchar](50) NOT NULL,
	[adminUsername] [nvarchar](50) NOT NULL,
	[adminEmail] [nvarchar](max) NULL,
	[adminHashPassword] [nvarchar](max) NOT NULL,
	[status] [nvarchar](50) NULL,
	[isMain] [nvarchar](50) NULL,
	[created_on] [datetime] NULL,
	[terminateReason] [varchar](200) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
