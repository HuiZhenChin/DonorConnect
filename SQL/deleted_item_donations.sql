USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[deleted_item_donations]    Script Date: 4/11/2024 5:09:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[deleted_item_donations](
	[donationId] [nvarchar](500) NOT NULL,
	[content] [nvarchar](800) NOT NULL
) ON [PRIMARY]
GO


