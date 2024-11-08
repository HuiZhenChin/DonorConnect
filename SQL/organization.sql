USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[organization]    Script Date: 4/11/2024 5:14:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[organization](
	[orgId] [nvarchar](50) NOT NULL,
	[orgName] [nvarchar](max) NOT NULL,
	[orgEmail] [nvarchar](max) NOT NULL,
	[orgContactNumber] [nvarchar](50) NOT NULL,
	[orgHashPassword] [nvarchar](max) NOT NULL,
	[orgNewHashPassword] [nvarchar](max) NULL,
	[orgAddress] [nvarchar](max) NOT NULL,
	[businessLicenseImageBase64] [nvarchar](max) NOT NULL,
	[picName] [nvarchar](max) NOT NULL,
	[picEmail] [nvarchar](max) NOT NULL,
	[picContactNumber] [nvarchar](50) NOT NULL,
	[orgProfilePicBase64] [nvarchar](max) NULL,
	[orgDescription] [nvarchar](max) NULL,
	[mostNeededItemCategory] [nvarchar](max) NULL,
	[orgRegion] [nvarchar](50) NOT NULL,
	[orgStatus] [nvarchar](50) NOT NULL,
	[adminId] [nvarchar](50) NULL,
	[createdOn] [datetime] NOT NULL,
	[rejectedReason] [varchar](500) NULL,
	[resubmitApplication] [varchar](50) NULL,
	[signature] [varchar](max) NULL,
	[signaturePIC] [varchar](500) NULL,
	[expiryDateReminder] [int] NULL,
	[terminateReason] [varchar](200) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


