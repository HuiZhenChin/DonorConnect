USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[inventory]    Script Date: 4/11/2024 5:13:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inventory](
	[inventoryId] [varchar](500) NOT NULL,
	[itemCategory] [varchar](500) NOT NULL,
	[item] [varchar](500) NOT NULL,
	[quantity] [varchar](500) NOT NULL,
	[expiryDate] [varchar](500) NULL,
	[orgId] [varchar](500) NOT NULL,
	[itemUsage] [varchar](500) NOT NULL,
	[donationId] [varchar](500) NULL,
	[created_on] [datetime] NULL,
	[imageItem] [varchar](max) NULL,
	[threshold] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


