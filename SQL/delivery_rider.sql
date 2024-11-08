USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[delivery_rider]    Script Date: 4/11/2024 5:11:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[delivery_rider](
	[riderId] [nvarchar](50) NOT NULL,
	[riderUsername] [nvarchar](max) NOT NULL,
	[riderFullName] [nvarchar](max) NOT NULL,
	[riderEmail] [nvarchar](max) NOT NULL,
	[riderContactNumber] [nvarchar](50) NOT NULL,
	[riderHashPassword] [nvarchar](max) NOT NULL,
	[riderNewHashPassword] [nvarchar](max) NULL,
	[drivingLicenseImageBase64] [nvarchar](max) NOT NULL,
	[vehicleType] [nvarchar](50) NOT NULL,
	[riderFacePicBase64] [nvarchar](max) NOT NULL,
	[noOfDeliveryMade] [nvarchar](50) NULL,
	[walletAmount] [nvarchar](50) NULL,
	[vehiclePlateNumber] [nvarchar](50) NOT NULL,
	[registerDate] [datetime] NOT NULL,
	[riderStatus] [nvarchar](50) NOT NULL,
	[adminId] [nvarchar](50) NULL,
	[rejectedReason] [varchar](500) NULL,
	[resubmitApplication] [varchar](50) NULL,
	[riderRegion] [varchar](100) NULL,
	[terminateReason] [varchar](200) NULL,
	[warning] [varchar](50) NULL,
	[warningReason] [varchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


