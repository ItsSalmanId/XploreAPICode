USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD]    Script Date: 7/21/2022 9:37:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD](
	[TASK_SUB_TYPE_INTEL_TASK_FIELD_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[TASK_SUB_TYPE_ID] [int] NULL,
	[INTEL_TASK_FIELD_ID] [int] NULL,
	[IS_REQUIRED] [bit] NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD_DELETED]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_571463B3C4D54F329671F24627061C3F]  DEFAULT (newsequentialid()),
 CONSTRAINT [PK_FOX_TBL_TASK_SUB_TYPE_INTEL_TASK_FIELD] PRIMARY KEY CLUSTERED 
(
	[TASK_SUB_TYPE_INTEL_TASK_FIELD_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


