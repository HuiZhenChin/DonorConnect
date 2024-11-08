USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[itemCategory]    Script Date: 4/11/2024 5:14:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[itemCategory](
	[categoryName] [varchar](300) NOT NULL,
	[specificItems] [varchar](max) NULL,
	[hasExpiryDate] [varchar](50) NULL,
	[categoryIcon] [varchar](100) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


