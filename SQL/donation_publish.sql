USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[donation_publish]    Script Date: 4/11/2024 5:12:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[donation_publish](
	[donationPublishId] [nvarchar](50) NOT NULL,
	[title] [nvarchar](max) NOT NULL,
	[peopleNeeded] [nvarchar](50) NOT NULL,
	[address] [nvarchar](max) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[itemCategory] [varchar](max) NULL,
	[specificItemsForCategory] [nvarchar](max) NULL,
	[specificQtyForCategory] [nvarchar](50) NULL,
	[timeRange] [varchar](50) NULL,
	[urgentStatus] [nvarchar](50) NOT NULL,
	[status] [nvarchar](50) NOT NULL,
	[donationImage] [nvarchar](max) NULL,
	[donationAttch] [nvarchar](max) NULL,
	[orgId] [nvarchar](50) NOT NULL,
	[adminId] [nvarchar](50) NULL,
	[created_on] [datetime] NOT NULL,
	[restriction] [varchar](500) NULL,
	[donationState] [varchar](500) NULL,
	[closureReason] [varchar](500) NULL,
	[resubmit] [varchar](50) NULL,
	[rejectedReason] [varchar](500) NULL,
	[recipientPhoneNumber] [varchar](100) NULL,
	[recipientName] [varchar](100) NULL,
	[approved_on] [datetime] NULL,
	[countdownEnded] [datetime] NULL,
	[newCountdownStart] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


