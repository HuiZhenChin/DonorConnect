USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[donation_item_request]    Script Date: 4/11/2024 5:12:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[donation_item_request](
	[donationId] [varchar](100) NULL,
	[itemImage] [varchar](max) NULL,
	[requestStatus] [varchar](50) NULL,
	[itemCategory] [varchar](100) NULL,
	[quantityDonated] [varchar](100) NULL,
	[reasonOfRejection] [varchar](500) NULL,
	[description] [varchar](max) NULL,
	[donorId] [varchar](100) NULL,
	[orgId] [varchar](100) NULL,
	[donationPublishId] [varchar](100) NULL,
	[created_on] [datetime] NULL,
	[item] [varchar](max) NULL,
	[donorFullName] [varchar](200) NULL,
	[donorEmail] [varchar](300) NULL,
	[donorPhone] [varchar](100) NULL,
	[pickUpAddress] [varchar](500) NULL,
	[state] [varchar](100) NULL,
	[totalDistance] [varchar](500) NULL,
	[paymentStatus] [varchar](100) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


