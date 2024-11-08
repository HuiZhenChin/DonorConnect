USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[reset_password]    Script Date: 4/11/2024 5:15:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[reset_password](
	[email] [nvarchar](max) NOT NULL,
	[username] [nvarchar](500) NOT NULL,
	[password_token] [nvarchar](max) NOT NULL,
	[userRole] [nvarchar](50) NOT NULL,
	[created_on] [datetime] NOT NULL,
	[used] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[reset_password] ADD  DEFAULT ((0)) FOR [used]
GO


