USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[inventory_history]    Script Date: 4/11/2024 5:13:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[inventory_history](
	[content] [varchar](800) NULL,
	[orgId] [varchar](500) NOT NULL,
	[created_on] [datetime] NOT NULL
) ON [PRIMARY]
GO


