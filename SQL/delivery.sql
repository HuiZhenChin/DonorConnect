USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[delivery]    Script Date: 4/11/2024 5:11:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[delivery](
	[deliveryId] [nvarchar](500) NOT NULL,
	[pickupDate] [nvarchar](500) NOT NULL,
	[pickupTime] [nvarchar](500) NOT NULL,
	[deliveryStatus] [nvarchar](100) NOT NULL,
	[vehicleType] [nvarchar](50) NOT NULL,
	[pickupAddress] [nvarchar](max) NOT NULL,
	[destinationAddress] [nvarchar](max) NOT NULL,
	[donorId] [nvarchar](500) NOT NULL,
	[orgId] [nvarchar](500) NOT NULL,
	[riderId] [nvarchar](500) NULL,
	[donationId] [nvarchar](500) NOT NULL,
	[noteRider] [nvarchar](500) NULL,
	[noteOrg] [nvarchar](500) NULL,
	[created_on] [datetime] NULL,
	[paymentAmount] [varchar](100) NULL,
	[qrCode] [nvarchar](max) NULL,
	[pageToken] [varchar](max) NULL,
	[acceptTimeByRider] [datetime] NULL,
	[pickupTimeByRider] [datetime] NULL,
	[reachTimeByRider] [datetime] NULL,
	[usedQrCode] [bit] NULL,
	[pickupImg] [varchar](max) NULL,
	[reachImg] [varchar](max) NULL,
	[refundReason] [varchar](200) NULL,
	[itemApproved] [bit] NULL,
	[acknowledgmentReceiptPath] [varchar](max) NULL,
	[approvalDeadline] [datetime] NULL,
	[autoApproved] [bit] NULL,
	[walletUpdated] [bit] NULL,
	[earningReceiptPath] [varchar](max) NULL,
	[liveLink] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[delivery] ADD  DEFAULT ((0)) FOR [usedQrCode]
GO

ALTER TABLE [dbo].[delivery] ADD  DEFAULT ((0)) FOR [itemApproved]
GO

ALTER TABLE [dbo].[delivery] ADD  DEFAULT ((0)) FOR [autoApproved]
GO

ALTER TABLE [dbo].[delivery] ADD  DEFAULT ((0)) FOR [walletUpdated]
GO


