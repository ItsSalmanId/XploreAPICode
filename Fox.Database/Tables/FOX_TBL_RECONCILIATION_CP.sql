USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_RECONCILIATION_CP]    Script Date: 7/21/2022 9:57:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_RECONCILIATION_CP](
	[RECONCILIATION_CP_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NOT NULL,
	[DEPOSIT_DATE] [datetime] NULL,
	[DEPOSIT_TYPE_ID] [bigint] NULL,
	[CATEGORY_ID] [bigint] NULL,
	[CHECK_NO] [varchar](100) NULL,
	[AMOUNT] [money] NULL,
	[AMOUNT_POSTED] [money] NULL,
	[AMOUNT_NOT_POSTED] [money] NULL,
	[RECONCILIATION_STATUS_ID] [int] NULL,
	[ASSIGNED_TO] [varchar](70) NULL,
	[ASSIGNED_DATE] [datetime] NULL,
	[COMPLETED_DATE] [datetime] NULL,
	[COMPLETED_BY] [varchar](70) NULL,
	[LEDGER_NAME] [varchar](200) NULL,
	[LEDGER_PATH] [varchar](500) NULL,
	[TOTAL_LEDGER_PAGES] [int] NULL,
	[CREATED_BY] [varchar](70) NOT NULL,
	[CREATED_DATE] [datetime] NOT NULL,
	[MODIFIED_BY] [varchar](70) NOT NULL,
	[MODIFIED_DATE] [datetime] NOT NULL,
	[DELETED] [bit] NOT NULL,
	[IS_RECONCILIED] [bit] NULL,
	[REASON] [bigint] NULL,
	[DATE_POSTED] [datetime] NULL,
	[REMARKS] [varchar](300) NULL,
	[BATCH_NO] [varchar](300) NULL,
	[ASSIGNED_GROUP] [varchar](70) NULL,
	[ASSIGNED_GROUP_DATE] [date] NULL,
	[FOX_TBL_INSURANCE_NAME] [varchar](100) NULL,
	[STATE] [varchar](100) NULL,
 CONSTRAINT [PK_FOX_TBL_RECONCILIATION_CP] PRIMARY KEY CLUSTERED 
(
	[RECONCILIATION_CP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


