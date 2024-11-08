USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[donor]    Script Date: 4/11/2024 5:13:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[donor](
	[donorId] [nvarchar](50) NOT NULL,
	[donorName] [nvarchar](max) NOT NULL,
	[donorUsername] [nvarchar](max) NOT NULL,
	[donorEmail] [nvarchar](max) NOT NULL,
	[donorContactNumber] [nvarchar](50) NOT NULL,
	[donorHashPassword] [nvarchar](max) NOT NULL,
	[donorNewHashPassword] [nvarchar](max) NULL,
	[donorAddress1] [nvarchar](max) NULL,
	[donorProfilePicBase64] [nvarchar](max) NULL,
	[createdOn] [datetime] NOT NULL,
	[status] [nvarchar](200) NOT NULL,
	[terminateReason] [varchar](200) NULL,
	[warning] [varchar](50) NULL,
	[warningReason] [varchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[donor] ADD  DEFAULT ('Active') FOR [status]
GO


