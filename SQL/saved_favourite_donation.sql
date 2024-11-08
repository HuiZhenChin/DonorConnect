USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[saved_favourite_donation]    Script Date: 4/11/2024 5:15:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[saved_favourite_donation](
	[savedDonationId] [nvarchar](50) NOT NULL,
	[username] [nvarchar](max) NOT NULL,
	[donationPublishId] [nvarchar](500) NOT NULL,
	[savedOn] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


