USE [DONOR_CONNECT]
GO

/****** Object:  Table [dbo].[temp_expiry_reminder]    Script Date: 4/11/2024 5:15:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[temp_expiry_reminder](
	[content] [varchar](800) NOT NULL,
	[orgId] [varchar](500) NOT NULL,
	[created_on] [datetime] NOT NULL,
	[read] [bit] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[temp_expiry_reminder] ADD  DEFAULT ((0)) FOR [read]
GO


