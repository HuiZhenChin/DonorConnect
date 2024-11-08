USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[rider_location]    Script Date: 4/11/2024 5:15:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[rider_location](
	[deliveryId] [varchar](500) NOT NULL,
	[latitude] [varchar](max) NULL,
	[longitude] [varchar](max) NULL,
	[lastUpdated] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


