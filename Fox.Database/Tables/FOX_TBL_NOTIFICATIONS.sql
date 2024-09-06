USE [MIS_DB]
GO

/****** Object:  Table [dbo].[Fox_Tbl_Notifications]    Script Date: 7/21/2022 9:54:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Fox_Tbl_Notifications](
	[FOX_NOTIFICATION_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[NOTIFICATION_TEXT] [varchar](400) NULL,
	[CASE_ID] [bigint] NULL,
	[CASE_STATUS_ID] [int] NULL,
	[PATIENT_ACCOUNT] [bigint] NULL,
	[NOTIFICATION_TYPE] [varchar](50) NULL,
	[SENT_ON] [datetime] NULL,
	[SENT_BY_USER_ID] [bigint] NULL,
	[SENT_TO_USER] [bigint] NULL,
	[IS_READ] [bit] NULL,
	[APPLICATION] [varchar](50) NULL,
	[DELETED] [bit] NULL,
	[CRATED_BY] [varchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[TASK_ID] [varchar](100) NULL,
 CONSTRAINT [PK__Fox_Tbl___19E57EDF2A70B5C3] PRIMARY KEY CLUSTERED 
(
	[FOX_NOTIFICATION_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


