USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[contact_us]    Script Date: 4/11/2024 5:07:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[contact_us](
	[sentBy] [nvarchar](50) NOT NULL,
	[dateSent] [datetime] NOT NULL,
	[messageSent] [varchar](300) NOT NULL,
	[fileAttchSent] [varchar](max) NULL,
	[senderEmail] [nvarchar](100) NOT NULL,
	[senderPhoneNumber] [nvarchar](50) NOT NULL,
	[senderFullName] [nvarchar](100) NULL,
	[senderOrgName] [nvarchar](100) NULL,
	[messageId] [varchar](300) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


