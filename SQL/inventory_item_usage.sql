USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[inventory_item_usage]    Script Date: 4/11/2024 5:13:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inventory_item_usage](
	[itemCategory] [varchar](500) NOT NULL,
	[item] [varchar](500) NOT NULL,
	[quantityIn] [varchar](500) NULL,
	[quantityOut] [varchar](500) NULL,
	[created_on] [datetime] NOT NULL,
	[inventoryId] [varchar](500) NOT NULL,
	[orgId] [varchar](500) NULL
) ON [PRIMARY]
GO


