USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[donation_item_expiry_date]    Script Date: 4/11/2024 5:12:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[donation_item_expiry_date](
	[donationId] [varchar](100) NULL,
	[itemCategory] [varchar](100) NULL,
	[item2] [varchar](200) NULL,
	[totalQuantity] [varchar](100) NULL,
	[quantityWithSameExpiryDate] [varchar](100) NULL,
	[expiryDate] [varchar](100) NULL,
	[donorId] [varchar](100) NULL,
	[orgId] [varchar](100) NULL,
	[donationPublishId] [varchar](100) NULL,
	[created_on] [datetime] NULL
) ON [PRIMARY]
GO


