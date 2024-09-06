-- AUTHOR:  <Abdul Sattar>                                                                                                          
-- CREATE DATE: <CREATE DATE, 05/11/2023>                                                                                                          
-- DESCRIPTION: <GET LIST OF Referral Reports>                
-- =============================================                         
-- FOX_PROC_REPORT_REFERRAL_LIST 1012714, NULL, NULL, NULL, NULL, '', 'Trashed', '', '', '', 1, 50, '', 'WORK_ID', 'ASC','1'                   
CREATE PROCEDURE [dbo].[FOX_PROC_REPORT_REFERRAL_LIST]               
 (                
@PRACTICE_CODE BIGINT                
,@INDEXED_DATE_FROM DATETIME                
,@INDEXED_DATE_TO DATETIME                
,@RECEIVED_DATE_FROM DATETIME                
,@RECEIVED_DATE_TO DATETIME                
,@INDEXED_STATUS VARCHAR(100)                
,@STATUS VARCHAR(100)                
,@DOCUMENT_TYPE VARCHAR(100)                
,@SOURCE_TYPE VARCHAR(100)                
,@ASSIGNED_PERSON_NAME VARCHAR(100)                
,@CURRENT_PAGE INT                
,@RECORD_PER_PAGE INT                
,@SEARCH_TEXT VARCHAR(100)                
,@SORT_BY VARCHAR(20)                
,@SORT_ORDER VARCHAR(5)                
,@PRIORITY varchar(20)                
 )                
AS                
--declare                
--@PRACTICE_CODE BIGINT=1012714                 
--,@INDEXED_DATE_FROM DATETIME= null                
--,@INDEXED_DATE_TO DATETIME=null                
--,@RECEIVED_DATE_FROM DATETIME='3/6/2022 12:00:00 AM'                
--,@RECEIVED_DATE_TO DATETIME = '3/7/2023 12:47:46 PM'                
--,@INDEXED_STATUS VARCHAR(100)=''                
--,@STATUS VARCHAR(100)=''                 
--,@DOCUMENT_TYPE VARCHAR(100) =''                 
--,@SOURCE_TYPE VARCHAR(100)=''                 
--,@ASSIGNED_PERSON_NAME VARCHAR(100) =''                
--,@CURRENT_PAGE INT = 1                
--,@RECORD_PER_PAGE INT=500                
--,@SEARCH_TEXT VARCHAR(100)= ''                
--,@SORT_BY VARCHAR(20)= 'WORK_ID'                
--,@SORT_ORDER VARCHAR(5)= 'ASC'                
--,@PRIORITY varchar(20)='0'                
BEGIN                
if(@PRIORITY = '')                
begin                
set @PRIORITY = null                
end                
 IF OBJECT_ID('TEMPDB..#TEMP_TABLE_REFERRAL_REPORT') IS NOT NULL DROP TABLE #TEMP_TABLE_REFERRAL_REPORT                
                
 SELECT TEMP.*                
INTO #TEMP_TABLE_REFERRAL_REPORT                
 FROM (                
 SELECT WQ.WORK_ID AS WORK_ID,                
 case                 
 when IS_EMERGENCY_ORDER =1 then'Emergency'                 
 when isnull(IS_EMERGENCY_ORDER,0)=0 then 'Normal'                 
 end as PRIORITY,                 
 WQ.UNIQUE_ID AS UNIQUE_ID,                 
 WQ.FAX_ID,                
 WQ.IS_EMERGENCY_ORDER,                
 ISNULL(dt.NAME, '') AS DOCUMENT_TYPE,                 
 dt.DOCUMENT_TYPE_ID,                
--CASE                
-- WHEN ISNULL(WQ.DOCUMENT_TYPE, 0) = 1 THEN 'POC'                
-- WHEN ISNULL(WQ.DOCUMENT_TYPE, 0) = 2 THEN 'Referral Order'                
-- WHEN ISNULL(WQ.DOCUMENT_TYPE, 0) = 3 THEN 'Other'                
-- ELSE ''                
--END AS DOCUMENT_TYPE,                
CASE                
 WHEN WQ.INDEXED_BY IS NOT NULL AND (WQ.WORK_STATUS = 'INDEXED' OR WQ.WORK_STATUS = 'COMPLETED') THEN 'Indexed'                
 WHEN WQ.INDEXED_BY IS NULL AND WQ.WORK_STATUS = 'INDEX PENDING' THEN 'Index Pending'                
 WHEN WQ.INDEXED_BY IS NULL AND WQ.WORK_STATUS != 'INDEX PENDING' AND WQ.IS_TRASH_REFERRAL != 1 THEN 'In Progress'    
 WHEN WQ.IS_TRASH_REFERRAL = 1 THEN 'Index Pending'     
 ELSE ''                
END AS INDEXING_STATUS,                
(IB.LAST_NAME + ', ' + IB.FIRST_NAME) AS INDEXED_BY,                
WQ.INDEXED_DATE AS INDEXED_DATE,                
((convert(varchar, WQ.INDEXED_DATE, 101)) + ' ' + (convert(varchar, cast(WQ.INDEXED_DATE as time), 0))) AS Indexed_Date_Str,           
WQ.SORCE_TYPE AS SOURCE_TYPE,                
WQ.RECEIVE_DATE,                
((convert(varchar, WQ.RECEIVE_DATE, 101)) + ' ' + (convert(varchar, cast(WQ.RECEIVE_DATE as time), 0))) AS Received_Date_Str,                
(AT.FIRST_NAME + ', ' + AT.LAST_NAME) AS ASSIGNED_TO,           
(ORS.FIRST_NAME + ','+ ORS.LAST_NAME + ' | ' + ORS.NPI) AS ORDERING_REFERRAL_SOURCE,          
TR.ROLE_NAME,                 
(CB.LAST_NAME + ', ' + CB.FIRST_NAME) AS COMPLETED_BY,           
CASE                
 WHEN WQ.INDEXED_BY IS NULL AND (WORK_STATUS = 'CREATED' OR UPPER(ISNULL(WORK_STATUS,'INDEX PENDING')) = 'INDEX PENDING') THEN 'Pending'                
 WHEN WQ.INDEXED_BY IS NOT NULL AND WQ.WORK_STATUS = 'COMPLETED' THEN 'Completed'       
  WHEN WQ.IS_TRASH_REFERRAL = 1 THEN 'Trashed'      
 ELSE ''              
END AS WORK_STATUS,                
 WQ.COMPLETED_DATE AS COMPLETED_DATE,                 
 ((convert(varchar, WQ.COMPLETED_DATE, 101)) + ' ' + (convert(varchar, cast(WQ.COMPLETED_DATE as time), 0))) AS Completed_Date_Str          
 FROM FOX_TBL_WORK_QUEUE WQ with(nolock, nowait)          
LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE ORS with(nolock, nowait) on ORS.SOURCE_ID = WQ.SENDER_ID and ISNULL(ORS.DELETED, 0) = 0           
left join fox_tbl_document_type dt with(nolock, nowait) on dt.DOCUMENT_TYPE_ID=wq.DOCUMENT_TYPE and ISNULL(dt.DELETED, 0) = 0                
LEFT JOIN FOX_TBL_APPLICATION_USER AT with(nolock, nowait) ON WQ.ASSIGNED_TO = AT.USER_NAME and ISNULL(at.DELETED, 0) = 0                
LEFT JOIN FOX_TBL_APPLICATION_USER CB with(nolock, nowait) ON WQ.COMPLETED_BY = CB.USER_NAME and ISNULL(cb.DELETED, 0) = 0                
LEFT JOIN FOX_TBL_APPLICATION_USER IB with(nolock, nowait) ON WQ.ASSIGNED_BY = IB.USER_NAME and ISNULL(ib.DELETED, 0) = 0                
LEFT JOIN FOX_TBL_ROLE TR with(nolock, nowait) ON AT.ROLE_ID = TR.ROLE_ID and ISNULL(tr.DELETED, 0) = 0                
 WHERE ISNULL(wq.DELETED, 0) = 0 AND wq.PRACTICE_CODE = @PRACTICE_CODE                 
AND(@PRIORITY IS NULL OR ISNULL(WQ.IS_EMERGENCY_ORDER,0) = @PRIORITY )                
AND(@INDEXED_DATE_FROM IS NULL OR @INDEXED_DATE_TO IS NULL OR (INDEXED_DATE BETWEEN @INDEXED_DATE_FROM AND @INDEXED_DATE_TO))                
AND(@RECEIVED_DATE_FROM IS NULL OR @RECEIVED_DATE_TO IS NULL OR (RECEIVE_DATE BETWEEN @RECEIVED_DATE_FROM AND @RECEIVED_DATE_TO))                 
) TEMP                
                
 SET @CURRENT_PAGE = @CURRENT_PAGE - 1                
 DECLARE @START_FROM INT = @CURRENT_PAGE * @RECORD_PER_PAGE                
 DECLARE @TOATL_PAGESUDM FLOAT             
 --Select --------          
 SELECT @TOATL_PAGESUDM = COUNT(*)                
 FROM #TEMP_TABLE_REFERRAL_REPORT                
 WHERE (                
 WORK_ID LIKE @SEARCH_TEXT + '%'                
 OR DOCUMENT_TYPE LIKE @SEARCH_TEXT + '%'                
 OR INDEXED_BY LIKE @SEARCH_TEXT + '%'                
 OR convert(VARCHAR, INDEXED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                
 OR SOURCE_TYPE LIKE @SEARCH_TEXT + '%'                
 OR convert(VARCHAR, RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                
 OR ROLE_NAME LIKE @SEARCH_TEXT + '%'                
 OR ASSIGNED_TO LIKE @SEARCH_TEXT + '%'          
 OR ORDERING_REFERRAL_SOURCE LIKE @SEARCH_TEXT + '%'          
 OR WORK_STATUS LIKE @SEARCH_TEXT + '%'                
 OR COMPLETED_BY LIKE @SEARCH_TEXT + '%'                
 OR convert(VARCHAR, COMPLETED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                
)                
AND DOCUMENT_TYPE LIKE '%' + @DOCUMENT_TYPE + '%'                
AND INDEXING_STATUS LIKE '%' + @INDEXED_STATUS + '%'                
AND SOURCE_TYPE LIKE '%' + @SOURCE_TYPE + '%'                
--AND ( @INDEXED_DATE_FROM IS NULL OR @INDEXED_DATE_TO IS NULL OR INDEXED_DATE BETWEEN @INDEXED_DATE_FROM AND @INDEXED_DATE_TO )                
--AND ( @RECEIVED_DATE_FROM IS NULL OR @RECEIVED_DATE_TO IS NULL OR RECEIVE_DATE BETWEEN @RECEIVED_DATE_FROM AND @RECEIVED_DATE_TO )                
AND WORK_STATUS LIKE '%' + @STATUS + '%'                
AND ASSIGNED_TO LIKE @ASSIGNED_PERSON_NAME + '%'                
                
 IF (@RECORD_PER_PAGE = 0)                
 BEGIN                
SET @RECORD_PER_PAGE = @TOATL_PAGESUDM                
 END                
 ELSE                
 BEGIN                
SET @RECORD_PER_PAGE = @RECORD_PER_PAGE                
 END                
                
 DECLARE @TOTAL_RECORDS INT = @TOATL_PAGESUDM                
                
 SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE)                
                
 SELECT *, @TOATL_PAGESUDM AS TOTAL_ROCORD_PAGES, @TOTAL_RECORDS TOTAL_RECORDS                
 FROM                
(                
SELECT                 
WORK_ID ,                
PRIORITY,                
UNIQUE_ID,                 
FAX_ID,                
IS_EMERGENCY_ORDER,                 
DOCUMENT_TYPE,                 
INDEXING_STATUS,                 
INDEXED_BY ,                
INDEXED_DATE,                 
Indexed_Date_Str,                 
SOURCE_TYPE,                 
RECEIVE_DATE,                
Received_Date_Str,                 
ASSIGNED_TO,                 
ROLE_NAME,                
COMPLETED_BY,                 
WORK_STATUS,                 
COMPLETED_DATE,                 
Completed_Date_Str,          
ORDERING_REFERRAL_SOURCE          
FROM #TEMP_TABLE_REFERRAL_REPORT                
WHERE (                
 WORK_ID LIKE @SEARCH_TEXT + '%'                
OR DOCUMENT_TYPE LIKE @SEARCH_TEXT + '%'                
OR INDEXED_BY LIKE @SEARCH_TEXT + '%'                
OR convert(VARCHAR, INDEXED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                
OR SOURCE_TYPE LIKE @SEARCH_TEXT + '%'                
OR convert(VARCHAR, RECEIVE_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                
OR ROLE_NAME LIKE @SEARCH_TEXT + '%'                
OR ASSIGNED_TO LIKE @SEARCH_TEXT + '%'          
OR ORDERING_REFERRAL_SOURCE LIKE @SEARCH_TEXT + '%'          
OR WORK_STATUS LIKE @SEARCH_TEXT + '%'                
OR COMPLETED_BY LIKE @SEARCH_TEXT + '%'                
OR convert(VARCHAR, COMPLETED_DATE, 101) LIKE '%' + @SEARCH_TEXT + '%'                
)                
AND DOCUMENT_TYPE LIKE '%' + @DOCUMENT_TYPE + '%'                
AND INDEXING_STATUS LIKE '%' + @INDEXED_STATUS + '%'                
AND SOURCE_TYPE LIKE '%' + @SOURCE_TYPE + '%'                
--AND ( @INDEXED_DATE_FROM IS NULL OR @INDEXED_DATE_TO IS NULL OR INDEXED_DATE BETWEEN @INDEXED_DATE_FROM AND @INDEXED_DATE_TO )                
--AND ( @RECEIVED_DATE_FROM IS NULL OR @RECEIVED_DATE_TO IS NULL OR RECEIVE_DATE BETWEEN @RECEIVED_DATE_FROM AND @RECEIVED_DATE_TO )                
AND WORK_STATUS LIKE '%' + @STATUS + '%'                
AND ASSIGNED_TO LIKE @ASSIGNED_PERSON_NAME + '%'                
) AS REPORT_REFERRAL_LIST                 
                
 ORDER BY                 
case when @SORT_BY = 'WORK_ID' and @SORT_ORDER = 'ASC' then WORK_ID end ASC,                
case when @SORT_BY = 'WORK_ID'and @SORT_ORDER = 'DESC' then WORK_ID end DESC,                
case when @SORT_BY = 'UNIQUE_ID' and @SORT_ORDER = 'ASC' then UNIQUE_ID end ASC,                
case when @SORT_BY = 'UNIQUE_ID'and @SORT_ORDER = 'DESC' then UNIQUE_ID end DESC,                 
case when @SORT_BY = 'DOCUMENT_TYPE' and @SORT_ORDER = 'ASC' then DOCUMENT_TYPE end ASC,                
case when @SORT_BY = 'DOCUMENT_TYPE'and @SORT_ORDER = 'DESC' then DOCUMENT_TYPE end DESC,                
case when @SORT_BY = 'INDEXED_BY' and @SORT_ORDER = 'ASC' then INDEXED_BY end ASC,                
case when @SORT_BY = 'INDEXED_BY'and @SORT_ORDER = 'DESC' then INDEXED_BY end DESC,                
case when @SORT_BY = 'INDEXED_DATE' and @SORT_ORDER = 'ASC' then INDEXED_DATE end ASC,                
case when @SORT_BY = 'INDEXED_DATE'and @SORT_ORDER = 'DESC' then INDEXED_DATE end DESC,                
case when @SORT_BY = 'SOURCE_TYPE' and @SORT_ORDER = 'ASC' then SOURCE_TYPE end ASC,                
case when @SORT_BY = 'SOURCE_TYPE'and @SORT_ORDER = 'DESC' then SOURCE_TYPE end DESC,                
case when @SORT_BY = 'ROLE_NAME' and @SORT_ORDER = 'ASC' then ROLE_NAME end ASC,                
case when @SORT_BY = 'ROLE_NAME'and @SORT_ORDER = 'DESC' then ROLE_NAME end DESC,                
case when @SORT_BY = 'ASSIGNED_TO' and @SORT_ORDER = 'ASC' then ASSIGNED_TO end ASC,                
case when @SORT_BY = 'ASSIGNED_TO'and @SORT_ORDER = 'DESC' then ASSIGNED_TO end DESC,          
case when @SORT_BY = 'ORDERING_REFERRAL_SOURCE'and @SORT_ORDER = 'DESC' then ORDERING_REFERRAL_SOURCE end DESC,          
case when @SORT_BY = 'RECEIVE_DATE' and @SORT_ORDER = 'ASC' then RECEIVE_DATE end ASC,                
case when @SORT_BY = 'RECEIVE_DATE'and @SORT_ORDER = 'DESC' then RECEIVE_DATE end DESC,                
case when @SORT_BY = 'WORK_STATUS' and @SORT_ORDER = 'ASC' then WORK_STATUS end ASC,                
case when @SORT_BY = 'WORK_STATUS'and @SORT_ORDER = 'DESC' then WORK_STATUS end DESC,                
case when @SORT_BY = 'COMPLETED_BY' and @SORT_ORDER = 'ASC' then COMPLETED_BY end ASC,                
case when @SORT_BY = 'COMPLETED_BY'and @SORT_ORDER = 'DESC' then COMPLETED_BY end DESC,                
case when @SORT_BY = 'COMPLETED_DATE' and @SORT_ORDER = 'ASC' then COMPLETED_DATE end ASC,                
case when @SORT_BY = 'COMPLETED_DATE'and @SORT_ORDER = 'DESC' then COMPLETED_DATE end DESC,                
case when @SORT_BY = 'PRIORITY' and @SORT_ORDER = 'ASC' then PRIORITY end ASC,                
case when @SORT_BY = 'PRIORITY'and @SORT_ORDER = 'DESC' then PRIORITY end DESC                
                 
 OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY                
END 