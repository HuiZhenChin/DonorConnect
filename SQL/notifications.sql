USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[notifications]    Script Date: 4/11/2024 5:14:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[notifications](
	[notificationId] [nvarchar](500) NOT NULL,
	[userId] [nvarchar](500) NOT NULL,
	[link] [varchar](max) NULL,
	[content] [nvarchar](max) NOT NULL,
	[created_on] [datetime] NOT NULL,
	[supportingId] [varchar](500) NULL,
	[isRead] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[notifications] ADD  DEFAULT ((0)) FOR [isRead]
GO


