USE [MIS_DB]
GO

/****** Object:  Table [dbo].[FOX_TBL_REFERRAL_REGION_COUNTY]    Script Date: 7/21/2022 9:31:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FOX_TBL_REFERRAL_REGION_COUNTY](
	[REFERRAL_REGION_COUNTY_ID] [bigint] NOT NULL,
	[PRACTICE_CODE] [bigint] NULL,
	[REFERRAL_REGION_ID] [bigint] NULL,
	[REGION_ZIPCODE_DATA_ID] [bigint] NULL,
	[CREATED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_REFERRAL_REGION_COUNTY_CREATED_BY]  DEFAULT ('FOX TEAM'),
	[CREATED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_REFERRAL_REGION_COUNTY_CREATED_DATE]  DEFAULT (getdate()),
	[MODIFIED_BY] [varchar](70) NOT NULL CONSTRAINT [DF_FOX_TBL_REFERRAL_REGION_COUNTY_MODIFIED_BY]  DEFAULT ('FOX TEAM'),
	[MODIFIED_DATE] [datetime] NOT NULL CONSTRAINT [DF_FOX_TBL_REFERRAL_REGION_COUNTY_MODIFIED_DATE]  DEFAULT (getdate()),
	[DELETED] [bit] NOT NULL CONSTRAINT [DF_FOX_TBL_REFERRAL_REGION_COUNTY_DELETED]  DEFAULT ((0)),
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [MSmerge_df_rowguid_20D6EE8CFD484C60BE34450092876E26]  DEFAULT (newsequentialid()),
	[ZIP_STATE_COUNTY_ID] [bigint] NULL,
 CONSTRAINT [PK_FOX_TBL_REFERRAL_REGION_COUNTY] PRIMARY KEY CLUSTERED 
(
	[REFERRAL_REGION_COUNTY_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


