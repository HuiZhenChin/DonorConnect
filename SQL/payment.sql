USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[payment]    Script Date: 4/11/2024 5:15:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[payment](
	[paymentId] [nvarchar](500) NOT NULL,
	[paymentDateTime] [datetime] NOT NULL,
	[paymentAmount] [nvarchar](100) NOT NULL,
	[paymentStatus] [nvarchar](100) NOT NULL,
	[donorId] [nvarchar](500) NOT NULL,
	[donationId] [varchar](500) NULL
) ON [PRIMARY]
GO


