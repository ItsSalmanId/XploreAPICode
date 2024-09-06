-- AUTHOR:  <DEVELOPER, MUHAMMAD SALMAN>    
-- CREATE DATE: <CREATE DATE, 10/29/2023>       
-- MODIFY DATE: <CREATE DATE, 10/29/2023>       
-- DESCRIPTION: <FOX_GET_ANYLYSIS_REPORT>                                                                                          
-- --FOX_GET_ANYLYSIS_REPORT  '','','MONTHTODATE','','','','',1,1,100           
ALTER PROCEDURE FOX_GET_ANYLYSIS_REPORT                      
           
           
-- DECLARE          
--@DATEFROM VARCHAR(15) ='',          
--@DATETO VARCHAR(15) ='',          
--@TIMEFRAME VARCHAR(50) ='MONTHTODATE',          
--@BUSINESS_HOURS9A VARCHAR(50) ='' ,          
--@BUSINESS_HOURS5A VARCHAR(50) ='',          
--@SATURDAYSA VARCHAR(500) = '',          
--@SUNDAYSA VARCHAR(500) ='',          
--@EXCLUDEWEEKEND BIT =1,          
--@PAGEINDEX BIGINT =1,          
--@PAGESIZE BIGINT =100          
(                              
@DATEFROM VARCHAR(15) ,                          
@DATETO VARCHAR(15) ,                          
@TIMEFRAME VARCHAR(50) ,                           
@BUSINESS_HOURS9A VARCHAR(50)  ,                          
@BUSINESS_HOURS5A VARCHAR(50)  ,                          
@SATURDAYSA VARCHAR(500),                             
@SUNDAYSA VARCHAR(500) ,                          
@EXCLUDEWEEKEND BIT ,                           
@PAGEINDEX BIGINT,                                
@PAGESIZE BIGINT            
     )                      
                                           
  AS                          
BEGIN               
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED            
DECLARE @BUSINESS_HOURS9 TIME                         
DECLARE @BUSINESS_HOURS5 TIME                         
DECLARE @SATURDAYS VARCHAR(500)                         
DECLARE @SUNDAYS VARCHAR(500)                         
                        
IF(@SATURDAYSA = '') BEGIN                        
SET @SATURDAYS = NULL                        
END                        
ELSE BEGIN                        
SET @SATURDAYS=@SATURDAYSA                        
END                        
                        
IF(@SUNDAYSA = '')                        
BEGIN                        
SET @SUNDAYSA = NULL                        
END                        
ELSE BEGIN                        
SET @SUNDAYS=@SUNDAYSA                        
END                        
                        
IF(@BUSINESS_HOURS9A = '')                        
BEGIN                        
SET @BUSINESS_HOURS9 = NULL                        
END                        
ELSE BEGIN                        
SET @BUSINESS_HOURS9=@BUSINESS_HOURS9A                        
END                        
                        
IF(@BUSINESS_HOURS5A = '')                        
BEGIN                        
SET @BUSINESS_HOURS5 = NULL                        
END                        
ELSE BEGIN                        
SET @BUSINESS_HOURS5=@BUSINESS_HOURS5A                        
END                        
                        
if(@TIMEFRAME = 'MONTHTODATE')                        
begin                        
                        
IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUE_DATA', 'U') IS NOT NULL DROP TABLE #WORKQUEUE_DATA                        
IF OBJECT_ID('TEMPDB.DBO.#LOGQUEUE_DATA', 'U') IS NOT NULL DROP TABLE #LOGQUEUE_DATA                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP2DATA', 'U') IS NOT NULL DROP TABLE #TEMP2DATA                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP3DATA', 'U') IS NOT NULL DROP TABLE #TEMP3DATA                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP4DATA', 'U') IS NOT NULL DROP TABLE #TEMP4DATA                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA', 'U') IS NOT NULL DROP TABLE #TEMP5DATAA                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAB', 'U') IS NOT NULL DROP TABLE #TEMP5DATAB                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATA_FINAL', 'U') IS NOT NULL DROP TABLE #TEMP5DATA_FINAL                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP6DATA', 'U') IS NOT NULL DROP TABLE #TEMP6DATA                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP7DATA', 'U') IS NOT NULL DROP TABLE #TEMP7DATA                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP8DATA', 'U') IS NOT NULL DROP TABLE #TEMP8DATA                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIMEDATA', 'U') IS NOT NULL DROP TABLE #INDEX_TIMEDATA                    
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME1DATA', 'U') IS NOT NULL DROP TABLE #INDEX_TIME1DATA                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME2DATA', 'U') IS NOT NULL DROP TABLE #INDEX_TIME2DATA                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIMEDATA_FINAL', 'U') IS NOT NULL DROP TABLE #INDEX_TIMEDATA_FINAL                        
IF OBJECT_ID('TEMPDB.DBO.#FINALRECORD', 'U') IS NOT NULL DROP TABLE  #FINALRECORD                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA_Set', 'U') IS NOT NULL DROP TABLE  #TEMP5DATAA_Set                        
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION_TBL', 'U') IS NOT NULL DROP TABLE  #PAGINATION_TBL                    
IF OBJECT_ID('TEMPDB.DBO.#TEMP6DATA_TOTAL', 'U') IS NOT NULL DROP TABLE  #TEMP6DATA_TOTAL       
IF OBJECT_ID('TEMPDB.DBO.#REFASSIGNDETAILS', 'U') IS NOT NULL DROP TABLE #REFASSIGNDETAILS                     
                        
                        
                        
IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUEFILTEREDRECORDS', 'U') IS NOT NULL DROP TABLE #WORKQUEUEFILTEREDRECORDS                           
 SELECT * INTO #WORKQUEUEFILTEREDRECORDS                           
FROM FOX_TBL_WORK_QUEUE where                          
CONVERT(DATE,RECEIVE_DATE)>=CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0))   AND                           
CONVERT(DATE,RECEIVE_DATE)< DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 1)                          
AND CONVERT(TIME,RECEIVE_DATE) >=CONVERT(TIME,ISNULL(@BUSINESS_HOURS9,RECEIVE_DATE))                          
AND CONVERT(TIME,RECEIVE_DATE) <=CONVERT(TIME,ISNULL(@BUSINESS_HOURS5,RECEIVE_DATE))                          
AND (DATENAME(dw,RECEIVE_DATE) = ISNULL(@SATURDAYS,DATENAME(dw, RECEIVE_DATE)) OR DATENAME(dw,RECEIVE_DATE) = ISNULL(@SUNDAYS,DATENAME(dw,RECEIVE_DATE)))                      
                        
          
------------------------------------QURIES-------------------------                        
-----#TIME_ELAPSED_COMPLETION_TIME                        
                        
IF OBJECT_ID('TEMPDB.DBO.#TOTALTIMEINSECONDS', 'U') IS NOT NULL DROP TABLE #TOTALTIMEINSECONDS                         
 SELECT (CONVERT(BIGINT,                          
DATEDIFF(MINUTE, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE))))                         
TOTALTIMEINSECONDS,RECEIVE_DATE,COMPLETED_DATE INTO #TOTALTIMEINSECONDS                         
FROM #WORKQUEUEFILTEREDRECORDS WHERE COMPLETED_DATE IS NOT NULL                          
AND ISNULL(DELETED,0)=0                         
AND UPPER(WORK_STATUS)='COMPLETED'                        
          
          
                        
SELECT DISTINCT  CONVERT(DATE,W.RECEIVE_DATE) RECEIVE_DATE,E.ROLE_NAME,W.WORK_ID,                        
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,W.ASSIGNED_DATE), CONVERT(DATETIME,W.COMPLETED_DATE )))) as COMPLETEFILESCOUNT ,                        
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE)))) TOTALTIMEINSECONDS_TOCOMPLETE                        
 INTO #WORKQUEUE_DATA           
 FROM FOX_TBL_WORK_QUEUE W              WITH(INDEX (IDX_FTWQ_05262022),FORCESEEK)          
JOIN  FOX_TBL_APPLICATION_USER R ON R.USER_NAME = W.ASSIGNED_TO                         
LEFT JOIN FOX_TBL_ROLE E ON E.ROLE_ID =R.ROLE_ID                        
WHERE COMPLETED_DATE IS NOT NULL                          
AND ISNULL(W.DELETED,0)=0                         
AND UPPER(W.WORK_STATUS)='COMPLETED'                        
     
SELECT * INTO #REFASSIGNDETAILS FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS    
WHERE WORK_ID IN (SELECT WORK_ID FROM #WORKQUEUEFILTEREDRECORDS)      
ORDER BY WORK_ID,CREATED_DATE ASC         
                        
SELECT ASSIGNED_BY_DESIGNATION,WORK_ID,                        
(CONVERT(BIGINT, DATEDIFF(SS,CONVERT(DATETIME,(SELECT TOP 1 ASSIGNED_TIME FROM           
#REFASSIGNDETAILS WHERE WORK_ID=MAIN.WORK_ID          
AND ASSIGNED_TIME < MAIN.ASSIGNED_TIME ORDER BY ASSIGNED_TIME DESC)), CONVERT(DATETIME,ASSIGNED_TIME)))) TOTALTIMEINSECONDS            
INTO #LOGQUEUE_DATA                        
FROM #REFASSIGNDETAILS  AS MAIN                        
--ORDER BY WORK_ID,CREATED_DATE ASC                        
          
       
                        
                        
SELECT  Q.RECEIVE_DATE, A.* INTO #TEMP3DATA FROM #LOGQUEUE_DATA A                        
JOIN FOX_TBL_WORK_QUEUE Q  WITH(INDEX (IDX_FTWQ_05262022)) ON A.WORK_ID = Q.WORK_ID AND UPPER(WORK_STATUS)='COMPLETED' AND ISNULL(DELETED,0)=0                        
                        
SELECT RECEIVE_DATE, ROLE_NAME, SUM(COMPLETEFILESCOUNT) AS TOTAL_TIME,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE           
INTO #TEMP2DATA FROM ( SELECT * FROM #WORKQUEUE_DATA UNION ALL SELECT *,0 TOTALTIMEINSECONDS_TOCOMPLETE FROM #TEMP3DATA)A                        
GROUP BY A.ROLE_NAME,RECEIVE_DATE                        
                     
                
                        
 ----------------INDXER ASSIGN TIME---------------------------------                        
 SELECT D.WORK_ID,W.RECEIVE_DATE,D.CREATED_DATE INTO   #INDEX_TIMEDATA FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS D                         
 JOIN  FOX_TBL_WORK_QUEUE W ON D.WORK_ID = W.WORK_ID                        
 WHERE UPPER(W.WORK_STATUS)='COMPLETED'  AND W.COMPLETED_DATE IS NOT NULL AND ISNULL(W.DELETED,0) =0                          
 AND UPPER(D.ASSIGNED_TO_DESIGNATION) = 'INDEXER'                        
                        
 SELECT WORK_ID,RECEIVE_DATE,MIN(CREATED_DATE) CREATED_DATE INTO   #INDEX_TIME1DATA FROM #INDEX_TIMEDATA                        
 GROUP BY WORK_ID,RECEIVE_DATE                        
          
                        
 SELECT WORK_ID,CONVERT(DATE,RECEIVE_DATE)  RECEIVE_DATE,                        
 (CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,CREATED_DATE)))) INDEXER_ASSIGNMENT_TIME                           
INTO   #INDEX_TIMEDATA_FINAL FROM #INDEX_TIME1DATA                        
                        
                        
SELECT RECEIVE_DATE,SUM(INDEXER_ASSIGNMENT_TIME) INDEXER_ASSIGNMENT_TIME  INTO #FINALRECORD FROM #INDEX_TIMEDATA_FINAL                        
GROUP BY RECEIVE_DATE                        
          
          
                         
                        
SELECT DISTINCT  CONVERT(DATE,A.RECEIVE_DATE) RECEIVE_DATE,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('INDEXER') THEN A.TOTAL_TIME ELSE 0 END) AS INDEXER_TOTAL_TIME,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('AGENT') THEN A.TOTAL_TIME ELSE 0 END) AS AGENT_TOTAL_TIME,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('SUPERVISOR') THEN A.TOTAL_TIME ELSE 0 END) AS SUPERVISOR_TOTAL_TIME                        
INTO  #TEMP5DATAA_SET                         
FROM #TEMP2DATA A                        
--LEFT JOIN #TEMP2DATA B ON CONVERT(DATE,A.RECEIVE_DATE)=CONVERT(DATE,B.RECEIVE_DATE)                         
                        
          
                      
          
                    
                    
select                         
  CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,                        
SUM (isnull(a.INDEXER_TOTAL_TIME,0)) INDEXER_TOTAL_TIME,                        
SUM (isnull(a.AGENT_TOTAL_TIME,0)) AGENT_TOTAL_TIME,                        
SUM (isnull(a.SUPERVISOR_TOTAL_TIME,0)) SUPERVISOR_TOTAL_TIME                        
 INTO  #TEMP5DATAA                        
 FROM #TEMP5DATAA_Set A                        
GROUP BY CONVERT(DATE,a.RECEIVE_DATE)                        
ORDER BY 1                        
          
          
---------------------------------------------                        
SELECT  CONVERT(DATE,A.RECEIVE_DATE) RECEIVE_DATE,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATAB FROM #WORKQUEUE_DATA A                        
GROUP BY CONVERT(DATE,A.RECEIVE_DATE)                        
ORDER BY 1                         
             
SELECT A.*,B.TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATA_FINAL FROM           
#TEMP5DATAA A                        
LEFT JOIN #TEMP5DATAB B ON A.RECEIVE_DATE=B.RECEIVE_DATE                        
ORDER BY 1                        
                        
IF OBJECT_ID('TEMPDB.DBO.#TIME_ELAPSED_COMPLETION_TIME', 'U') IS NOT NULL DROP TABLE #TIME_ELAPSED_COMPLETION_TIME                         
SELECT                         
CONVERT(DATE,RECEIVE_DATE)  AS DATE,                        
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 0 AND 15 THEN 1 ELSE 0 END) '0-15',                         
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 16 AND 30 THEN 1 ELSE 0 END) '16-30' ,                        
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 31 AND 45 THEN 1 ELSE 0 END) '31-45' ,                         
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 46 AND 60 THEN 1 ELSE 0 END) '46-60' ,                          
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 61 AND 120 THEN 1 ELSE 0 END) '1-2 HOUR',                        
SUM(CASE WHEN  TOTALTIMEINSECONDS > 120 THEN 1 ELSE 0 END) '> 2 HOUR',                        
COUNT(*) AS GRANDTOTAL   INTO #TIME_ELAPSED_COMPLETION_TIME                        
FROM #TOTALTIMEINSECONDS                         
GROUP BY CONVERT(DATE,RECEIVE_DATE)                         
ORDER BY CONVERT(DATE,RECEIVE_DATE) ASC                         
                        
SELECT E.RECEIVE_DATE,CONVERT(INT,E.INDEXER_TOTAL_TIME/(T.GRANDTOTAL)) INDEXER_TOTAL_TIME ,                      
CONVERT(INT,E.AGENT_TOTAL_TIME/(T.GRANDTOTAL)) AGENT_TOTAL_TIME ,                      
CONVERT(INT,E.SUPERVISOR_TOTAL_TIME/(T.GRANDTOTAL)) SUPERVISOR_TOTAL_TIME,                      
CONVERT(INT,E.TOTALTIMEINSECONDS_TOCOMPLETE/(T.GRANDTOTAL)) TOTALTIMEINSECONDS_TOCOMPLETE,                      
CONVERT(INT,D.INDEXER_ASSIGNMENT_TIME/(T.GRANDTOTAL)) INDEXER_ASSIGNMENT_TIME,                     
T.[0-15],T.[16-30],T.[31-45],T.[46-60],T.[1-2 HOUR],T.[> 2 HOUR],T.GRANDTOTAL            ,CASE WHEN  D.INDEXER_ASSIGNMENT_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_ASSIGNMENT_GRANDTOTAL                    
,CASE WHEN E.INDEXER_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_GRANDTOTAL                    
,CASE WHEN E.AGENT_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS AGENT_GRANDTOTAL                    
,CASE WHEN E.SUPERVISOR_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS SUPERVISOR_GRANDTOTAL                    
,CASE WHEN E.TOTALTIMEINSECONDS_TOCOMPLETE <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS TOTALTIME_TOCOMPLETE_GRANDTOTAL                    
INTO  #TEMP6DATA                        
FROM #TEMP5DATA_FINAL E                        
 JOIN   #TIME_ELAPSED_COMPLETION_TIME T                         
ON E.RECEIVE_DATE = T.DATE                         
LEFT JOIN  #FINALRECORD D  ON E.RECEIVE_DATE = D.RECEIVE_DATE                         
                    
                    
SELECT E.*, D.INDEXER_ASSIGNMENT_TIME,                     
T.[0-15],T.[16-30],T.[31-45],T.[46-60],T.[1-2 HOUR],T.[> 2 HOUR],T.GRANDTOTAL                    
,CASE WHEN D.INDEXER_ASSIGNMENT_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_ASSIGNMENT_GRANDTOTAL                    
,CASE WHEN E.INDEXER_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_GRANDTOTAL                    
,CASE WHEN E.AGENT_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS AGENT_GRANDTOTAL                    
,CASE WHEN E.SUPERVISOR_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS SUPERVISOR_GRANDTOTAL                    
,CASE WHEN E.TOTALTIMEINSECONDS_TOCOMPLETE <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS TOTALTIME_TOCOMPLETE_GRANDTOTAL                    
INTO  #TEMP6DATA_TOTAL                       
FROM #TEMP5DATA_FINAL E                        
 JOIN   #TIME_ELAPSED_COMPLETION_TIME T                         
ON E.RECEIVE_DATE = T.DATE                         
LEFT JOIN  #FINALRECORD D  ON E.RECEIVE_DATE = D.RECEIVE_DATE                        
                    
                   
                  
--SELECT * FROM #TEMP6DATA          
                  
                    
 SELECT CONVERT(VARCHAR, RECEIVE_DATE ,101) AS RECEIVE_DATE ,INDEXER_ASSIGNMENT_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME                        
 ,[0-15],[16-30],[31-45],[46-60],[1-2 HOUR],[> 2 HOUR],GRANDTOTAL,TOTALTIMEINSECONDS_TOCOMPLETE INTO    #TEMP8DATA FROM #TEMP6DATA                        
                       
 UNION ALL                        
                      
            
 SELECT  '1900/01/02'  DATE,                    
 CONVERT(INT, case when SUM(INDEXER_ASSIGNMENT_GRANDTOTAL) <> 0 then   SUM(INDEXER_ASSIGNMENT_TIME)/SUM(INDEXER_ASSIGNMENT_GRANDTOTAL)else 0 end )                    
 AS INDEXER_ASSIGNMENT_TIME,                    
                    
  CONVERT(INT, case when SUM(INDEXER_GRANDTOTAL) <> 0 then                    
 SUM(INDEXER_TOTAL_TIME)/SUM(INDEXER_GRANDTOTAL)else 0 end )                    
 AS INDEXER_TOTAL_TIME,                    
                    
  CONVERT(INT, case when SUM(AGENT_GRANDTOTAL) <> 0 then                    
 SUM(AGENT_TOTAL_TIME)/SUM(AGENT_GRANDTOTAL)else 0 end )                    
 AS AGENT_TOTAL_TIME,                    
                    
  CONVERT(INT, case when SUM(SUPERVISOR_GRANDTOTAL) <> 0 then                    
 SUM(SUPERVISOR_TOTAL_TIME)/SUM(SUPERVISOR_GRANDTOTAL)else 0 end )                    
 AS SUPERVISOR_TOTAL_TIME,                    
                   
                    
 SUM([0-15])  AS  [0-15] ,SUM([16-30])  AS  [16-30]  ,SUM([31-45])  AS  [31-45]  ,SUM([46-60])  AS  [46-60]                       
 ,SUM([1-2 HOUR])  AS [1-2 HOUR] ,SUM([> 2 HOUR])  AS [> 2 HOUR] ,SUM(GRANDTOTAL) AS GRANDTOTAL                     
                    
    , CONVERT(INT, case when SUM(TOTALTIME_TOCOMPLETE_GRANDTOTAL) <> 0 then                    
 SUM(TOTALTIMEINSECONDS_TOCOMPLETE)/SUM(TOTALTIME_TOCOMPLETE_GRANDTOTAL)else 0 end )                    
 AS TOTALTIMEINSECONDS_TOCOMPLETE                    
                  
 FROM #TEMP6DATA_TOTAL                    
                    
                  
                  
                    
 UNION ALL                        
                    
 SELECT  '1900/01/01'  DATE,CONVERT(INT, SUM(INDEXER_ASSIGNMENT_TIME)/COUNT(*)) AS INDEXER_ASSIGNMENT_TIME,CONVERT(INT,                         
 SUM(INDEXER_TOTAL_TIME)/COUNT(*)) AS INDEXER_TOTAL_TIME, CONVERT(INT,SUM(AGENT_TOTAL_TIME)/COUNT(*)) AS AGENT_TOTAL_TIME, CONVERT(INT,                         
 SUM(SUPERVISOR_TOTAL_TIME)/COUNT(*)) AS SUPERVISOR_TOTAL_TIME,                    
 Convert(INT,convert(decimal, convert(decimal ,SUM([0-15])) /convert(decimal ,COUNT(*))))  AS  [0-15],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([16-30])) /convert(decimal ,COUNT(*))))  AS  [16-30],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([31-45])) /convert(decimal ,COUNT(*))))  AS  [31-45],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([46-60])) /convert(decimal ,COUNT(*))))  AS  [46-60],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([1-2 HOUR])) /convert(decimal ,COUNT(*))))  AS [1-2 HOUR],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([> 2 HOUR])) /convert(decimal ,COUNT(*))))  AS [> 2 HOUR],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM(GRANDTOTAL)) /convert(decimal ,COUNT(*))))  AS GRANDTOTAL,                        
 CONVERT(INT, SUM(TOTALTIMEINSECONDS_TOCOMPLETE)/COUNT(*)) AS TOTALTIMEINSECONDS_TOCOMPLETE                           
 FROM #TEMP6DATA                         
                   
--SELECT * FROM #PAGINATION_TBL             
--SELECT DATENAME(dw,CASE RECEIVE_DATE WHEN 'Grand Total' THEN '' ELSE RECEIVE_DATE END) FROM #PAGINATION_TBL             
--SELECT * FROM #TEMP6DATA            
            
            
                   
 ---------------------------------------                        
 --DROP TABLE #PAGINATION_TBL          
 SELECT ROW_NUMBER() OVER(ORDER BY t.RECEIVE_DATE desc) AS ROW,              
  --DATENAME(dw,CONVERT(VARCHAR,RECEIVE_DATE))AS RECEIVE_DAY,                    
 * into   #PAGINATION_TBL                        
  FROM (                        
 SELECT                         
CASE Y.RECEIVE_DATE WHEN '1900/01/01' THEN 'GRAND AVG' WHEN '1900/01/02' THEN 'GRAND TOTAL'  ELSE  Y.RECEIVE_DATE END  RECEIVE_DATE ,[0-15] as ZEROTO_FIFTEEN,[16-30] as                         
SIXTEENTO_THIRTY,[31-45] as THIRTYONETO_FOURTFIVE,[46-60] as FOURTYSIXTO_SIXTY,[1-2 HOUR] as GREATERTHAN_HOUR,                        
[> 2 HOUR] as GREATERTHAN_TWOHOUR,GRANDTOTAL ,                        
CONVERT(VARCHAR, (y.INDEXER_ASSIGNMENT_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60) END 'INDEXER_ASSIGNMENT_TOTAL_TIME',                         
                        
CONVERT(VARCHAR, (y.INDEXER_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN             
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60) END 'INDEXER_TOTAL_TIME',                         
                        
CONVERT(VARCHAR, (y.AGENT_TOTAL_TIME)/3600 ) +':'+           
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, AGENT_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60) END 'AGENT_TOTAL_TIME',                        
                        
CONVERT(VARCHAR, (y.SUPERVISOR_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60) END 'SUPERVISOR_TOTAL_TIME',                        
                        
CONVERT(VARCHAR, (y.TOTALTIMEINSECONDS_TOCOMPLETE)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60) END 'TOTALTIME_INSECONDS_TOCOMPLETE'                        
FROM #TEMP8DATA  Y                          
LEFT JOIN #FINALRECORD U ON Y.RECEIVE_DATE = U.RECEIVE_DATE                        
                        
)t                        
                        
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION') IS NOT NULL DROP TABLE  #PAGINATION              
SELECT *,RECEIVE_DATE AS REC_DATE INTO   #PAGINATION FROM #PAGINATION_TBL          
          
UPDATE #PAGINATION          
SET REC_DATE = ''          
WHERE REC_DATE LIKE '%[A^Z]%'          
          
          
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION1') IS NOT NULL DROP TABLE  #PAGINATION1               
SELECT DATENAME(dw,CONVERT(DATE,REC_DATE))AS RECEIVE_DAY,* INTO #PAGINATION1          
FROM #PAGINATION          
                  
declare @Total_Pages BIGINT                        
declare @Total_Records BIGINT                              
                        
IF(@EXCLUDEWEEKEND=1)                        
BEGIN                         
                        
SET @Total_Pages  = (Select TOP 1 CAST(CEILING(COUNT(*) OVER()/(CONVERT(INT, @PAGESIZE )*1.0)) AS BIGINT) AS TOTAL_PAGE from #PAGINATION1                        
WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'             
)                             
SET @Total_Records  = (select Count(*) from #PAGINATION1                         
WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'   )                        
                        
 select ROW,RECEIVE_DATE,ZEROTO_FIFTEEN,SIXTEENTO_THIRTY,THIRTYONETO_FOURTFIVE,FOURTYSIXTO_SIXTY,GREATERTHAN_HOUR,GREATERTHAN_TWOHOUR,GRANDTOTAL,          
 INDEXER_ASSIGNMENT_TOTAL_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME,TOTALTIME_INSECONDS_TOCOMPLETE,@Total_Pages as TOTAL_PAGE,@Total_Records as TOTAL_RECORDS                          
 from #PAGINATION1           
 WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'             
 AND ROW BETWEEN( @PAGEINDEX -1) *  @PAGESIZE  + 1 AND(((  @PAGEINDEX  -1) *   @PAGESIZE  + 1) +   @PAGESIZE ) - 1                         
 OR ROW=1                        
  ORDER BY ROW                         
END                        
ELSE                        
BEGIN                        
SET @Total_Pages = (Select TOP 1 CAST(CEILING(COUNT(*) OVER()/(CONVERT(INT, @PAGESIZE )*1.0)) AS BIGINT) AS TOTAL_PAGE from #PAGINATION1)                             
 SET @Total_Records  = (select Count(*) from #PAGINATION1)                        
 select ROW,RECEIVE_DATE,ZEROTO_FIFTEEN,SIXTEENTO_THIRTY,THIRTYONETO_FOURTFIVE,FOURTYSIXTO_SIXTY,GREATERTHAN_HOUR,GREATERTHAN_TWOHOUR,GRANDTOTAL,          
 INDEXER_ASSIGNMENT_TOTAL_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME,TOTALTIME_INSECONDS_TOCOMPLETE,@Total_Pages as TOTAL_PAGE,@Total_Records as TOTAL_RECORDS                         
                        
 from #PAGINATION1 where ROW BETWEEN( @PAGEINDEX -1) *  @PAGESIZE  + 1 AND(((  @PAGEINDEX  -1) *   @PAGESIZE  + 1) +   @PAGESIZE ) - 1                         
 OR ROW=1                        
 ORDER BY ROW              
           
           
--SELECT * FROM #PAGINATION1        WHERE 1<>1                
-------------------Pagination query-----------------------------------------------------------------                        
end                        
          
end              
          
          
--SELECT * FROM #PAGINATION_TBL          
          
                    
if(@TIMEFRAME = 'YEARTODATE')                        
begin                        
                        
IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUE_DATA1', 'U') IS NOT NULL DROP TABLE #WORKQUEUE_DATA1                        
IF OBJECT_ID('TEMPDB.DBO.#LOGQUEUE_DATA1', 'U') IS NOT NULL DROP TABLE #LOGQUEUE_DATA1                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP2DATA1', 'U') IS NOT NULL DROP TABLE #TEMP2DATA1                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP3DATA1', 'U') IS NOT NULL DROP TABLE #TEMP3DATA1                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP4DATA1', 'U') IS NOT NULL DROP TABLE #TEMP4DATA1                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA1', 'U') IS NOT NULL DROP TABLE #TEMP5DATAA1                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAB1', 'U') IS NOT NULL DROP TABLE #TEMP5DATAB1                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATA_FINAL1', 'U') IS NOT NULL DROP TABLE #TEMP5DATA_FINAL1                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP6DATA1', 'U') IS NOT NULL DROP TABLE #TEMP6DATA1                
IF OBJECT_ID('TEMPDB.DBO.#TEMP8DATA1', 'U') IS NOT NULL DROP TABLE #TEMP8DATA1                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIMEDATA1', 'U') IS NOT NULL DROP TABLE #INDEX_TIMEDATA1                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME1DATA1', 'U') IS NOT NULL DROP TABLE #INDEX_TIME1DATA1                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME2DATA1', 'U') IS NOT NULL DROP TABLE #INDEX_TIME2DATA1                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIMEDATA1_FINAL1', 'U') IS NOT NULL DROP TABLE #INDEX_TIMEDATA1_FINAL1                        
IF OBJECT_ID('TEMPDB.DBO.#FINALRECORD1', 'U') IS NOT NULL DROP TABLE  #FINALRECORD1                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA_Set1', 'U') IS NOT NULL DROP TABLE  #TEMP5DATAA_Set1                        
IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUEFILTEREDRECORDS1', 'U') IS NOT NULL DROP TABLE #WORKQUEUEFILTEREDRECORDS1                         
IF OBJECT_ID('TEMPDB.DBO.#TEMP6DATA_TOTAL1', 'U') IS NOT NULL DROP TABLE #TEMP6DATA_TOTAL1     
IF OBJECT_ID('TEMPDB.DBO.#REFASSIGNDETAILS1', 'U') IS NOT NULL DROP TABLE #REFASSIGNDETAILS1                        
                         
SELECT * INTO #WORKQUEUEFILTEREDRECORDS1                        
FROM FOX_TBL_WORK_QUEUE where CONVERT(DATE,RECEIVE_DATE) BETWEEN   DATEADD(yy, DATEDIFF(yy,0,GETDATE()), 0) AND GETDATE()                        
AND CONVERT(TIME,RECEIVE_DATE) >=CONVERT(TIME,ISNULL(@BUSINESS_HOURS9,RECEIVE_DATE))                        
AND CONVERT(TIME,RECEIVE_DATE) <=CONVERT(TIME,ISNULL(@BUSINESS_HOURS5,RECEIVE_DATE))                        
AND (DATENAME(dw,RECEIVE_DATE)= ISNULL(@SATURDAYS,DATENAME(dw, RECEIVE_DATE)) OR DATENAME(dw,RECEIVE_DATE) = ISNULL(@SUNDAYS,DATENAME(dw,RECEIVE_DATE)))                        
------------------------------------QURIES-----------------------                      
                        
-----#TIME_ELAPSED_COMPLETION_TIME                        
IF OBJECT_ID('TEMPDB.DBO.#TOTALTIMEINSECONDS1', 'U') IS NOT NULL DROP TABLE #TOTALTIMEINSECONDS1                         
 SELECT (CONVERT(BIGINT,                          
DATEDIFF(MINUTE, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE))))                         
TOTALTIMEINSECONDS,RECEIVE_DATE,COMPLETED_DATE INTO #TOTALTIMEINSECONDS1                         
FROM #WORKQUEUEFILTEREDRECORDS1  WHERE COMPLETED_DATE IS NOT NULL                          
AND ISNULL(DELETED,0)=0                         
AND WORK_STATUS=UPPER('COMPLETED')                        
                        
SELECT DISTINCT  CONVERT(DATE,W.RECEIVE_DATE) RECEIVE_DATE,E.ROLE_NAME,W.WORK_ID,                        
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,W.ASSIGNED_DATE), CONVERT(DATETIME,W.COMPLETED_DATE )))) as COMPLETEFILESCOUNT ,                        
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE)))) TOTALTIMEINSECONDS_TOCOMPLETE                        
 INTO #WORKQUEUE_DATA1 FROM FOX_TBL_WORK_QUEUE W                        
JOIN  FOX_TBL_APPLICATION_USER R ON R.USER_NAME = W.ASSIGNED_TO                         
LEFT JOIN FOX_TBL_ROLE E ON E.ROLE_ID =R.ROLE_ID                    
WHERE COMPLETED_DATE IS NOT NULL                          
AND ISNULL(W.DELETED,0)=0                         
AND UPPER(W.WORK_STATUS)='COMPLETED'                        
        
SELECT * INTO #REFASSIGNDETAILS1 FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS    
WHERE WORK_ID IN (SELECT WORK_ID FROM #WORKQUEUEFILTEREDRECORDS1)      
ORDER BY WORK_ID,CREATED_DATE ASC                    
                        
SELECT ASSIGNED_BY_DESIGNATION,WORK_ID,                        
(CONVERT(BIGINT, DATEDIFF(SS,CONVERT(DATETIME,(SELECT TOP 1 ASSIGNED_TIME FROM #REFASSIGNDETAILS1 WHERE WORK_ID=MAIN.WORK_ID AND ASSIGNED_TIME < MAIN.ASSIGNED_TIME                        
ORDER BY ASSIGNED_TIME DESC)), CONVERT(DATETIME,ASSIGNED_TIME)))) TOTALTIMEINSECONDS  INTO #LOGQUEUE_DATA1                        
FROM #REFASSIGNDETAILS1  AS MAIN                        
ORDER BY WORK_ID,CREATED_DATE ASC                        
                        
SELECT  Q.RECEIVE_DATE, A.* INTO #TEMP3DATA1 FROM #LOGQUEUE_DATA1 A                        
JOIN FOX_TBL_WORK_QUEUE Q WITH(INDEX (IDX_FTWQ_05262022))          
ON A.WORK_ID = Q.WORK_ID AND UPPER(WORK_STATUS)='COMPLETED' AND ISNULL(DELETED,0)=0                        
                        
                        
SELECT RECEIVE_DATE, ROLE_NAME, SUM(COMPLETEFILESCOUNT) AS TOTAL_TIME,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP2DATA1 FROM (                        
 SELECT * FROM #WORKQUEUE_DATA1 UNION ALL SELECT *,0 TOTALTIMEINSECONDS_TOCOMPLETE FROM #TEMP3DATA1                        
)A                        
GROUP BY A.ROLE_NAME,RECEIVE_DATE                        
                        
 ----------------INDXER ASSIGN TIME---------------------------------                        
 SELECT D.WORK_ID,W.RECEIVE_DATE,D.CREATED_DATE INTO   #INDEX_TIMEDATA1 FROM #REFASSIGNDETAILS1 D JOIN                 
 FOX_TBL_WORK_QUEUE W ON D.WORK_ID = W.WORK_ID                        
 WHERE UPPER(W.WORK_STATUS)='COMPLETED'  AND W.COMPLETED_DATE IS NOT NULL AND ISNULL(W.DELETED,0) =0  AND UPPER(D.ASSIGNED_TO_DESIGNATION) = 'INDEXER'                        
 SELECT WORK_ID,RECEIVE_DATE,MIN(CREATED_DATE) CREATED_DATE INTO   #INDEX_TIME1DATA1 FROM #INDEX_TIMEDATA1                        
 GROUP BY WORK_ID,RECEIVE_DATE                        
                        
 SELECT WORK_ID,CONVERT(DATE,RECEIVE_DATE)  RECEIVE_DATE,(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,CREATED_DATE)))) INDEXER_ASSIGNMENT_TIME   INTO   #INDEX_TIMEDATA1_FINAL1 FROM #INDEX_TIME1DATA1                      
  
                        
 SELECT RECEIVE_DATE,SUM(INDEXER_ASSIGNMENT_TIME) INDEXER_ASSIGNMENT_TIME  INTO #FINALRECORD1 FROM #INDEX_TIMEDATA1_FINAL1                        
 GROUP BY RECEIVE_DATE                        
                        
                        
SELECT DISTINCT  CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('INDEXER') THEN A.TOTAL_TIME ELSE 0 END) AS INDEXER_TOTAL_TIME,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('AGENT') THEN A.TOTAL_TIME ELSE 0 END) AS AGENT_TOTAL_TIME,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('SUPERVISOR') THEN A.TOTAL_TIME ELSE 0 END) AS SUPERVISOR_TOTAL_TIME                        
 INTO  #TEMP5DATAA_Set1                        
 FROM #TEMP2DATA1 A                        
--LEFT JOIN #TEMP2DATA1 B ON CONVERT(DATE,A.RECEIVE_DATE)=CONVERT(DATE,B.RECEIVE_DATE)                         
                        
                        
select                         
  CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,                        
SUM (isnull(a.INDEXER_TOTAL_TIME,0)) INDEXER_TOTAL_TIME,                        
SUM (isnull(a.AGENT_TOTAL_TIME,0)) AGENT_TOTAL_TIME,                        
SUM (isnull(a.SUPERVISOR_TOTAL_TIME,0)) SUPERVISOR_TOTAL_TIME                        
 INTO  #TEMP5DATAA1                        
 FROM #TEMP5DATAA_Set1 A                        
GROUP BY CONVERT(DATE,a.RECEIVE_DATE)                 
ORDER BY 1                        
---------------------------------------------                        
                        
                        
SELECT  CONVERT(DATE,A.RECEIVE_DATE) RECEIVE_DATE,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATAB1 FROM #WORKQUEUE_DATA1 A                        
GROUP BY CONVERT(DATE,A.RECEIVE_DATE)                        
ORDER BY 1                         
                        
SELECT A.*,B.TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATA_FINAL1 FROM #TEMP5DATAA1 A                        
LEFT JOIN #TEMP5DATAB1 B ON A.RECEIVE_DATE=B.RECEIVE_DATE                        
ORDER BY 1                        
                        
IF OBJECT_ID('TEMPDB.DBO.#TIME_ELAPSED_COMPLETION_TIME1', 'U') IS NOT NULL DROP TABLE #TIME_ELAPSED_COMPLETION_TIME1                         
SELECT                         
CONVERT(DATE,RECEIVE_DATE)  AS DATE,                        
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 0 AND 15 THEN 1 ELSE 0 END) '0-15',                         
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 16 AND 30 THEN 1 ELSE 0 END) '16-30' ,                        
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 31 AND 45 THEN 1 ELSE 0 END) '31-45' ,                         
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 46 AND 60 THEN 1 ELSE 0 END) '46-60' ,                          
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 61 AND 120 THEN 1 ELSE 0 END) '1-2 HOUR',                        
SUM(CASE WHEN  TOTALTIMEINSECONDS > 120 THEN 1 ELSE 0 END) '> 2 HOUR',                        
COUNT(*) AS GRANDTOTAL   INTO #TIME_ELAPSED_COMPLETION_TIME1                        
FROM #TOTALTIMEINSECONDS1                         
GROUP BY CONVERT(DATE,RECEIVE_DATE)                         
ORDER BY CONVERT(DATE,RECEIVE_DATE) ASC                         
                        
                     
SELECT E.RECEIVE_DATE,CONVERT(INT,E.INDEXER_TOTAL_TIME/(T.GRANDTOTAL)) INDEXER_TOTAL_TIME ,                      
CONVERT(INT,E.AGENT_TOTAL_TIME/(T.GRANDTOTAL)) AGENT_TOTAL_TIME ,                      
CONVERT(INT,E.SUPERVISOR_TOTAL_TIME/(T.GRANDTOTAL)) SUPERVISOR_TOTAL_TIME,                      
CONVERT(INT,E.TOTALTIMEINSECONDS_TOCOMPLETE/(T.GRANDTOTAL)) TOTALTIMEINSECONDS_TOCOMPLETE,                      
CONVERT(INT,D.INDEXER_ASSIGNMENT_TIME/(T.GRANDTOTAL)) INDEXER_ASSIGNMENT_TIME,                     
T.[0-15],T.[16-30],T.[31-45],T.[46-60],T.[1-2 HOUR],T.[> 2 HOUR],T.GRANDTOTAL                    
,CASE WHEN  D.INDEXER_ASSIGNMENT_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_ASSIGNMENT_GRANDTOTAL                    
,CASE WHEN E.INDEXER_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_GRANDTOTAL                    
,CASE WHEN E.AGENT_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS AGENT_GRANDTOTAL                    
,CASE WHEN E.SUPERVISOR_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS SUPERVISOR_GRANDTOTAL                    
,CASE WHEN E.TOTALTIMEINSECONDS_TOCOMPLETE <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS TOTALTIME_TOCOMPLETE_GRANDTOTAL                    
  INTO    #TEMP6DATA1  FROM #TEMP5DATA_FINAL1 E                        
 JOIN   #TIME_ELAPSED_COMPLETION_TIME1 T                         
ON E.RECEIVE_DATE = T.DATE                         
LEFT JOIN  #FINALRECORD1 D  ON E.RECEIVE_DATE = D.RECEIVE_DATE                      
                    
                    
                    
                    
SELECT E.*, D.INDEXER_ASSIGNMENT_TIME,                     
T.[0-15],T.[16-30],T.[31-45],T.[46-60],T.[1-2 HOUR],T.[> 2 HOUR],T.GRANDTOTAL                    
,CASE WHEN  D.INDEXER_ASSIGNMENT_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_ASSIGNMENT_GRANDTOTAL                    
,CASE WHEN E.INDEXER_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_GRANDTOTAL                    
,CASE WHEN E.AGENT_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS AGENT_GRANDTOTAL                    
,CASE WHEN E.SUPERVISOR_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS SUPERVISOR_GRANDTOTAL              
,CASE WHEN E.TOTALTIMEINSECONDS_TOCOMPLETE <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS TOTALTIME_TOCOMPLETE_GRANDTOTAL                    
INTO  #TEMP6DATA_TOTAL1                       
FROM #TEMP5DATA_FINAL1 E                        
 JOIN   #TIME_ELAPSED_COMPLETION_TIME1 T                         
ON E.RECEIVE_DATE = T.DATE                         
LEFT JOIN  #FINALRECORD1 D  ON E.RECEIVE_DATE = D.RECEIVE_DATE                        
                    
                    
                        
 SELECT CONVERT(VARCHAR, RECEIVE_DATE ,101) AS RECEIVE_DATE ,INDEXER_ASSIGNMENT_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME,[0-15],[16-30],[31-45],                        
 [46-60],[1-2 HOUR],[> 2 HOUR],GRANDTOTAL,TOTALTIMEINSECONDS_TOCOMPLETE INTO     #TEMP8DATA1 FROM #TEMP6DATA1                        
                      
 UNION ALL                        
                      
                        
 SELECT  '1900/01/02'  DATE,                    
 CONVERT(INT, case when SUM(INDEXER_ASSIGNMENT_GRANDTOTAL) <> 0 then                    
SUM(INDEXER_ASSIGNMENT_TIME)/SUM(INDEXER_ASSIGNMENT_GRANDTOTAL)else 0 end )                    
 AS INDEXER_ASSIGNMENT_TIME,                    
                    
  CONVERT(INT, case when SUM(INDEXER_GRANDTOTAL) <> 0 then                    
 SUM(INDEXER_TOTAL_TIME)/SUM(INDEXER_GRANDTOTAL)else 0 end )                    
 AS INDEXER_TOTAL_TIME,                    
                    
  CONVERT(INT, case when SUM(AGENT_GRANDTOTAL) <> 0 then                    
 SUM(AGENT_TOTAL_TIME)/SUM(AGENT_GRANDTOTAL)else 0 end )                    
 AS AGENT_TOTAL_TIME,                    
                    
  CONVERT(INT, case when SUM(SUPERVISOR_GRANDTOTAL) <> 0 then                    
 SUM(SUPERVISOR_TOTAL_TIME)/SUM(SUPERVISOR_GRANDTOTAL)else 0 end )                    
 AS SUPERVISOR_TOTAL_TIME,                    
                  
 SUM([0-15])  AS  [0-15] ,SUM([16-30])  AS  [16-30]  ,SUM([31-45])  AS  [31-45]  ,SUM([46-60])  AS  [46-60]                       
 ,SUM([1-2 HOUR])  AS [1-2 HOUR] ,SUM([> 2 HOUR])  AS [> 2 HOUR] ,SUM(GRANDTOTAL) AS GRANDTOTAL                     
                    
 ,  CONVERT(INT, case when SUM(TOTALTIME_TOCOMPLETE_GRANDTOTAL) <> 0 then                    
 SUM(TOTALTIMEINSECONDS_TOCOMPLETE)/SUM(TOTALTIME_TOCOMPLETE_GRANDTOTAL)else 0 end )                    
 AS TOTALTIMEINSECONDS_TOCOMPLETE                  
                       
 FROM #TEMP6DATA_TOTAL1                       
                      
 UNION ALL                        
                      
 SELECT  '1900/01/01'  DATE,CONVERT(INT, SUM(INDEXER_ASSIGNMENT_TIME)/COUNT(*)) AS INDEXER_ASSIGNMENT_TIME,CONVERT(INT,                         
 SUM(INDEXER_TOTAL_TIME)/COUNT(*)) AS INDEXER_TOTAL_TIME, CONVERT(INT,SUM(AGENT_TOTAL_TIME)/COUNT(*)) AS AGENT_TOTAL_TIME, CONVERT(INT,                         
 SUM(SUPERVISOR_TOTAL_TIME)/COUNT(*)) AS SUPERVISOR_TOTAL_TIME,                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([0-15])) /convert(decimal ,COUNT(*))))  AS  [0-15],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([16-30])) /convert(decimal ,COUNT(*))))  AS  [16-30],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([31-45])) /convert(decimal ,COUNT(*))))  AS  [31-45],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([46-60])) /convert(decimal ,COUNT(*))))  AS  [46-60],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([1-2 HOUR])) /convert(decimal ,COUNT(*))))  AS [1-2 HOUR],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([> 2 HOUR])) /convert(decimal ,COUNT(*))))  AS [> 2 HOUR],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM(GRANDTOTAL)) /convert(decimal ,COUNT(*))))  AS GRANDTOTAL,                        
 CONVERT(INT, SUM(TOTALTIMEINSECONDS_TOCOMPLETE)/COUNT(*)) AS TOTALTIMEINSECONDS_TOCOMPLETE                           
 FROM #TEMP6DATA1               
                      
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION_TBL1', 'U') IS NOT NULL DROP TABLE #PAGINATION_TBL1          
 SELECT ROW_NUMBER() OVER(ORDER BY t.RECEIVE_DATE desc) AS ROW,                        
 * into #PAGINATION_TBL1 FROM (                        
 SELECT                         
CASE Y.RECEIVE_DATE WHEN '1900/01/01' THEN 'GRAND AVG' WHEN '1900/01/02' THEN 'GRAND TOTAL'  ELSE  Y.RECEIVE_DATE END  RECEIVE_DATE ,[0-15] as ZEROTO_FIFTEEN,                        
[16-30] as SIXTEENTO_THIRTY,[31-45] as THIRTYONETO_FOURTFIVE,[46-60] as FOURTYSIXTO_SIXTY,[1-2 HOUR] as GREATERTHAN_HOUR,[> 2 HOUR] as GREATERTHAN_TWOHOUR,GRANDTOTAL                        
                        
,                        
CONVERT(VARCHAR, (y.INDEXER_ASSIGNMENT_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60) END 'INDEXER_ASSIGNMENT_TOTAL_TIME',                         
                        
CONVERT(VARCHAR, (y.INDEXER_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60) END 'INDEXER_TOTAL_TIME',                         
                        
CONVERT(VARCHAR, (y.AGENT_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, AGENT_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60) END 'AGENT_TOTAL_TIME',                        
                        
CONVERT(VARCHAR, (y.SUPERVISOR_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN        
CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60) END 'SUPERVISOR_TOTAL_TIME',                        
                        
CONVERT(VARCHAR, (y.TOTALTIMEINSECONDS_TOCOMPLETE)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) ELSE             
CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60) END 'TOTALTIME_INSECONDS_TOCOMPLETE'                        
                        
FROM #TEMP8DATA1  Y           
LEFT JOIN #FINALRECORD1 U ON Y.RECEIVE_DATE = U.RECEIVE_DATE                        
                        
)t                        
          
          
                 
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION2') IS NOT NULL DROP TABLE  #PAGINATION2                    
SELECT *,RECEIVE_DATE AS REC_DATE INTO   #PAGINATION2 FROM #PAGINATION_TBL1          
          
UPDATE #PAGINATION2          
SET REC_DATE = ''          
WHERE REC_DATE LIKE '%[A^Z]%'          
          
          
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION3') IS NOT NULL DROP TABLE  #PAGINATION3              
SELECT DATENAME(dw,CONVERT(DATE,REC_DATE))AS RECEIVE_DAY,* INTO #PAGINATION3          
FROM #PAGINATION2          
          
          
declare @Total_Pages1 BIGINT                        
declare @Total_Records1 BIGINT          
          
                     
IF(@EXCLUDEWEEKEND=1)                        
BEGIN                         
                        
SET @Total_Pages1  = (Select TOP 1 CAST(CEILING(COUNT(*) OVER()/(CONVERT(INT, @PAGESIZE )*1.0)) AS BIGINT) AS TOTAL_PAGE from #PAGINATION3                        
WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'                          
)                             
SET @Total_Records1  = (select Count(*) from #PAGINATION3 WHERE                         
RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'   )                        
                        
 select ROW,RECEIVE_DATE,ZEROTO_FIFTEEN,SIXTEENTO_THIRTY,THIRTYONETO_FOURTFIVE,FOURTYSIXTO_SIXTY,GREATERTHAN_HOUR,GREATERTHAN_TWOHOUR,GRANDTOTAL,          
 INDEXER_ASSIGNMENT_TOTAL_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME,TOTALTIME_INSECONDS_TOCOMPLETE,@Total_Pages1 as TOTAL_PAGE,@Total_Records1 as TOTAL_RECORDS                          
 from #PAGINATION3 where                         
 RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'                          
 AND ROW BETWEEN( @PAGEINDEX -1) *  @PAGESIZE  + 1 AND(((  @PAGEINDEX  -1) *   @PAGESIZE  + 1) +   @PAGESIZE ) - 1                         
 OR ROW=1                        
  ORDER BY ROW                         
END                        
ELSE                        
BEGIN                        
SET @Total_Pages1  = (Select TOP 1 CAST(CEILING(COUNT(*) OVER()/(CONVERT(INT, @PAGESIZE )*1.0)) AS BIGINT) AS TOTAL_PAGE from #PAGINATION_TBL1)                             
 SET @Total_Records1  = (select Count(*) from #PAGINATION_TBL1)                        
 select ROW,RECEIVE_DATE,ZEROTO_FIFTEEN,SIXTEENTO_THIRTY,THIRTYONETO_FOURTFIVE,FOURTYSIXTO_SIXTY,GREATERTHAN_HOUR,GREATERTHAN_TWOHOUR,GRANDTOTAL,          
 INDEXER_ASSIGNMENT_TOTAL_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME,TOTALTIME_INSECONDS_TOCOMPLETE,@Total_Pages1 as TOTAL_PAGE,@Total_Records1 as TOTAL_RECORDS                         
                        
 from #PAGINATION3 where ROW BETWEEN( @PAGEINDEX -1) *  @PAGESIZE  + 1 AND(((  @PAGEINDEX  -1) *   @PAGESIZE  + 1) +   @PAGESIZE ) - 1                         
 OR ROW=1                        
  ORDER BY ROW                         
-------------------Pagination query-----------------------------------------------------------------                        
                        
END                        
END                
          
          
          
          
if(@TIMEFRAME = 'LASTMONTH')                        
                        
begin                        
                        
IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUE_DATA2', 'U') IS NOT NULL DROP TABLE #WORKQUEUE_DATA2                        
IF OBJECT_ID('TEMPDB.DBO.#LOGQUEUE_DATA2', 'U') IS NOT NULL DROP TABLE #LOGQUEUE_DATA2                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP2DATA2', 'U') IS NOT NULL DROP TABLE #TEMP2DATA2                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP3DATA2', 'U') IS NOT NULL DROP TABLE #TEMP3DATA2                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP4DATA2', 'U') IS NOT NULL DROP TABLE #TEMP4DATA2                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA2', 'U') IS NOT NULL DROP TABLE #TEMP5DATAA2                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAB2', 'U') IS NOT NULL DROP TABLE #TEMP5DATAB2                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATA_FINAL2', 'U') IS NOT NULL DROP TABLE #TEMP5DATA_FINAL2                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP6DATA2', 'U') IS NOT NULL DROP TABLE #TEMP6DATA2                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP8DATA2', 'U') IS NOT NULL DROP TABLE #TEMP8DATA2                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIMEDATA2', 'U') IS NOT NULL DROP TABLE #INDEX_TIMEDATA2                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME1DATA2', 'U') IS NOT NULL DROP TABLE #INDEX_TIME1DATA2                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME2DATA2', 'U') IS NOT NULL DROP TABLE #INDEX_TIME2DATA2                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIMEDATA2_FINAL2', 'U') IS NOT NULL DROP TABLE #INDEX_TIMEDATA2_FINAL2                        
IF OBJECT_ID('TEMPDB.DBO.#FINALRECORD2', 'U') IS NOT NULL DROP TABLE  #FINALRECORD2                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA_Set2', 'U') IS NOT NULL DROP TABLE  #TEMP5DATAA_Set2                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP6DATA_TOTAL2', 'U') IS NOT NULL DROP TABLE  #TEMP6DATA_TOTAL2     
IF OBJECT_ID('TEMPDB.DBO.#REFASSIGNDETAILS2', 'U') IS NOT NULL DROP TABLE #REFASSIGNDETAILS2                       
                        
                        
IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUEFILTEREDRECORDS2', 'U') IS NOT NULL DROP TABLE #WORKQUEUEFILTEREDRECORDS2                         
 SELECT * INTO #WORKQUEUEFILTEREDRECORDS2                         
FROM FOX_TBL_WORK_QUEUE where                        
  CONVERT(DATE, RECEIVE_DATE) >= DATEADD(M, -1, DATEADD(MM, DATEDIFF(M, 0, GETDATE()), 0))                         
  AND CONVERT(DATE, RECEIVE_DATE) < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)                        
AND CONVERT(TIME,RECEIVE_DATE) >=CONVERT(TIME,ISNULL(@BUSINESS_HOURS9,RECEIVE_DATE))                        
AND CONVERT(TIME,RECEIVE_DATE) <=CONVERT(TIME,ISNULL(@BUSINESS_HOURS5,RECEIVE_DATE))                        
AND (DATENAME(dw,RECEIVE_DATE)= ISNULL(@SATURDAYS,DATENAME(dw, RECEIVE_DATE)) OR DATENAME(dw,RECEIVE_DATE) = ISNULL(@SUNDAYS,DATENAME(dw,RECEIVE_DATE)))                        
                         
                        
 ------------------------------------QURIES-------------------------                        
                         
-----#TIME_ELAPSED_COMPLETION_TIME                        
IF OBJECT_ID('TEMPDB.DBO.#TOTALTIMEINSECONDS2', 'U') IS NOT NULL DROP TABLE #TOTALTIMEINSECONDS2                         
 SELECT (CONVERT(BIGINT,                          
DATEDIFF(MINUTE, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE))))                         
TOTALTIMEINSECONDS,RECEIVE_DATE,COMPLETED_DATE INTO #TOTALTIMEINSECONDS2                         
FROM #WORKQUEUEFILTEREDRECORDS2 WHERE COMPLETED_DATE IS NOT NULL                          
AND ISNULL(DELETED,0)=0                         
AND UPPER(WORK_STATUS)='COMPLETED'               
                        
                        
SELECT DISTINCT  CONVERT(DATE,W.RECEIVE_DATE) RECEIVE_DATE,E.ROLE_NAME,W.WORK_ID,                        
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,W.ASSIGNED_DATE), CONVERT(DATETIME,W.COMPLETED_DATE )))) as COMPLETEFILESCOUNT ,                        
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE)))) TOTALTIMEINSECONDS_TOCOMPLETE                        
 INTO #WORKQUEUE_DATA2 FROM FOX_TBL_WORK_QUEUE W                        
JOIN  FOX_TBL_APPLICATION_USER R ON R.USER_NAME = W.ASSIGNED_TO                         
LEFT JOIN FOX_TBL_ROLE E ON E.ROLE_ID =R.ROLE_ID                        
WHERE COMPLETED_DATE IS NOT NULL                          
AND ISNULL(W.DELETED,0)=0                         
AND UPPER(W.WORK_STATUS)='COMPLETED'                        
                        
SELECT * INTO #REFASSIGNDETAILS2 FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS    
WHERE WORK_ID IN (SELECT WORK_ID FROM #WORKQUEUEFILTEREDRECORDS2)      
ORDER BY WORK_ID,CREATED_DATE ASC    
                        
SELECT ASSIGNED_BY_DESIGNATION,WORK_ID,                        
(CONVERT(BIGINT, DATEDIFF(SS,CONVERT(DATETIME,(SELECT TOP 1 ASSIGNED_TIME FROM #REFASSIGNDETAILS2 WHERE WORK_ID=MAIN.WORK_ID AND ASSIGNED_TIME < MAIN.ASSIGNED_TIME                        
ORDER BY ASSIGNED_TIME DESC)), CONVERT(DATETIME,ASSIGNED_TIME)))) TOTALTIMEINSECONDS  INTO #LOGQUEUE_DATA2                        
FROM #REFASSIGNDETAILS2  AS MAIN                        
ORDER BY WORK_ID,CREATED_DATE ASC                        
                        
SELECT  Q.RECEIVE_DATE, A.* INTO #TEMP3DATA2 FROM #LOGQUEUE_DATA2 A                        
JOIN FOX_TBL_WORK_QUEUE Q ON A.WORK_ID = Q.WORK_ID AND UPPER(WORK_STATUS)='COMPLETED' AND ISNULL(DELETED,0)=0                        
                        
                        
SELECT RECEIVE_DATE, ROLE_NAME, SUM(COMPLETEFILESCOUNT) AS TOTAL_TIME,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP2DATA2 FROM (                        
 SELECT * FROM #WORKQUEUE_DATA2 UNION ALL SELECT *,0 TOTALTIMEINSECONDS_TOCOMPLETE FROM #TEMP3DATA2                        
)A                        
GROUP BY A.ROLE_NAME,RECEIVE_DATE                        
                        
 ----------------INDXER ASSIGN TIME---------------------------------                        
 SELECT D.WORK_ID,W.RECEIVE_DATE,D.CREATED_DATE INTO   #INDEX_TIMEDATA2 FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS D JOIN                         
 FOX_TBL_WORK_QUEUE W ON D.WORK_ID = W.WORK_ID                        
 WHERE UPPER(W.WORK_STATUS)='COMPLETED' AND W.COMPLETED_DATE IS NOT NULL AND ISNULL(W.DELETED,0) =0  AND UPPER(D.ASSIGNED_TO_DESIGNATION) = 'INDEXER'                        
 SELECT WORK_ID,RECEIVE_DATE,MIN(CREATED_DATE) CREATED_DATE INTO   #INDEX_TIME1DATA2 FROM #INDEX_TIMEDATA2                        
 GROUP BY WORK_ID,RECEIVE_DATE                        
                        
 SELECT WORK_ID,CONVERT(DATE,RECEIVE_DATE)  RECEIVE_DATE,(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,CREATED_DATE)))) INDEXER_ASSIGNMENT_TIME   INTO   #INDEX_TIMEDATA2_FINAL2 FROM #INDEX_TIME1DATA2                      
  
                        
 SELECT RECEIVE_DATE,SUM(INDEXER_ASSIGNMENT_TIME) INDEXER_ASSIGNMENT_TIME  INTO #FINALRECORD2 FROM #INDEX_TIMEDATA2_FINAL2                        
 GROUP BY RECEIVE_DATE                        
                        
SELECT DISTINCT  CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,                
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('INDEXER') THEN A.TOTAL_TIME ELSE 0 END) AS INDEXER_TOTAL_TIME,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('AGENT') THEN A.TOTAL_TIME ELSE 0 END) AS AGENT_TOTAL_TIME,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('SUPERVISOR') THEN A.TOTAL_TIME ELSE 0 END) AS SUPERVISOR_TOTAL_TIME                        
 INTO  #TEMP5DATAA_Set2                        
 FROM #TEMP2DATA2 A                        
--LEFT JOIN #TEMP2DATA2 B ON CONVERT(DATE,A.RECEIVE_DATE)=CONVERT(DATE,B.RECEIVE_DATE)                         
                        
                        
select                         
  CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,                        
SUM (isnull(a.INDEXER_TOTAL_TIME,0)) INDEXER_TOTAL_TIME,             
SUM (isnull(a.AGENT_TOTAL_TIME,0)) AGENT_TOTAL_TIME,                        
SUM (isnull(a.SUPERVISOR_TOTAL_TIME,0)) SUPERVISOR_TOTAL_TIME                        
 INTO  #TEMP5DATAA2                        
 FROM #TEMP5DATAA_Set2 A                        
GROUP BY CONVERT(DATE,a.RECEIVE_DATE)                        
ORDER BY 1                        
---------------------------------------------                        
                        
SELECT  CONVERT(DATE,A.RECEIVE_DATE) RECEIVE_DATE,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATAB2 FROM #WORKQUEUE_DATA2 A                        
GROUP BY CONVERT(DATE,A.RECEIVE_DATE)                        
ORDER BY 1                         
                        
SELECT A.*,B.TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATA_FINAL2 FROM #TEMP5DATAA2 A                        
LEFT JOIN #TEMP5DATAB2 B ON A.RECEIVE_DATE=B.RECEIVE_DATE                        
ORDER BY 1                        
                        
                        
IF OBJECT_ID('TEMPDB.DBO.#TIME_ELAPSED_COMPLETION_TIME2', 'U') IS NOT NULL DROP TABLE #TIME_ELAPSED_COMPLETION_TIME2                        
SELECT                         
CONVERT(DATE,RECEIVE_DATE)  AS DATE,                        
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 0 AND 15 THEN 1 ELSE 0 END) '0-15',                         
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 16 AND 30 THEN 1 ELSE 0 END) '16-30' ,                 
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 31 AND 45 THEN 1 ELSE 0 END) '31-45' ,                         
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 46 AND 60 THEN 1 ELSE 0 END) '46-60' ,                          
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 61 AND 120 THEN 1 ELSE 0 END) '1-2 HOUR',                        
SUM(CASE WHEN  TOTALTIMEINSECONDS > 120 THEN 1 ELSE 0 END) '> 2 HOUR',                        
COUNT(*) AS GRANDTOTAL   INTO #TIME_ELAPSED_COMPLETION_TIME2                        
FROM #TOTALTIMEINSECONDS2                         
GROUP BY CONVERT(DATE,RECEIVE_DATE)                         
ORDER BY CONVERT(DATE,RECEIVE_DATE) ASC                       
                      
                        
SELECT E.RECEIVE_DATE,CONVERT(INT,E.INDEXER_TOTAL_TIME/(T.GRANDTOTAL)) INDEXER_TOTAL_TIME ,                      
CONVERT(INT,E.AGENT_TOTAL_TIME/(T.GRANDTOTAL)) AGENT_TOTAL_TIME ,                      
CONVERT(INT,E.SUPERVISOR_TOTAL_TIME/(T.GRANDTOTAL)) SUPERVISOR_TOTAL_TIME,                      
CONVERT(INT,E.TOTALTIMEINSECONDS_TOCOMPLETE/(T.GRANDTOTAL)) TOTALTIMEINSECONDS_TOCOMPLETE,                      
CONVERT(INT,D.INDEXER_ASSIGNMENT_TIME/(T.GRANDTOTAL)) INDEXER_ASSIGNMENT_TIME,                     
T.[0-15],T.[16-30],T.[31-45],T.[46-60],T.[1-2 HOUR],T.[> 2 HOUR],T.GRANDTOTAL                    
,CASE WHEN  D.INDEXER_ASSIGNMENT_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_ASSIGNMENT_GRANDTOTAL                    
,CASE WHEN E.INDEXER_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_GRANDTOTAL                    
,CASE WHEN E.AGENT_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS AGENT_GRANDTOTAL                    
,CASE WHEN E.SUPERVISOR_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS SUPERVISOR_GRANDTOTAL                    
,CASE WHEN E.TOTALTIMEINSECONDS_TOCOMPLETE <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS TOTALTIME_TOCOMPLETE_GRANDTOTAL                    
  INTO    #TEMP6DATA2  FROM #TEMP5DATA_FINAL2 E                        
 JOIN   #TIME_ELAPSED_COMPLETION_TIME2 T                         
ON E.RECEIVE_DATE = T.DATE                         
LEFT JOIN  #FINALRECORD2 D  ON E.RECEIVE_DATE = D.RECEIVE_DATE                           
                     
                      
  SELECT E.*, D.INDEXER_ASSIGNMENT_TIME,                     
T.[0-15],T.[16-30],T.[31-45],T.[46-60],T.[1-2 HOUR],T.[> 2 HOUR],T.GRANDTOTAL                    
,CASE WHEN  D.INDEXER_ASSIGNMENT_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_ASSIGNMENT_GRANDTOTAL                    
,CASE WHEN E.INDEXER_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_GRANDTOTAL                
,CASE WHEN E.AGENT_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS AGENT_GRANDTOTAL                    
,CASE WHEN E.SUPERVISOR_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS SUPERVISOR_GRANDTOTAL                    
,CASE WHEN E.TOTALTIMEINSECONDS_TOCOMPLETE <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS TOTALTIME_TOCOMPLETE_GRANDTOTAL                    
INTO  #TEMP6DATA_TOTAL2                       
 FROM #TEMP5DATA_FINAL2 E                        
 JOIN   #TIME_ELAPSED_COMPLETION_TIME2 T                         
ON E.RECEIVE_DATE = T.DATE                         
LEFT JOIN  #FINALRECORD2 D  ON E.RECEIVE_DATE = D.RECEIVE_DATE                        
                      
                         
                        
                          
 SELECT CONVERT(VARCHAR, RECEIVE_DATE ,101) AS RECEIVE_DATE ,INDEXER_ASSIGNMENT_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME,                        
 [0-15],[16-30],[31-45],[46-60],[1-2 HOUR],[> 2 HOUR],GRANDTOTAL,TOTALTIMEINSECONDS_TOCOMPLETE INTO     #TEMP8DATA2 FROM #TEMP6DATA2                        
 UNION ALL                        
                      
                      
   SELECT  '1900/01/02'  DATE,                    
 CONVERT(INT, case when SUM(INDEXER_ASSIGNMENT_GRANDTOTAL) <> 0 then                    
 SUM(INDEXER_ASSIGNMENT_TIME)/SUM(INDEXER_ASSIGNMENT_GRANDTOTAL)else 0 end )                    
 AS INDEXER_ASSIGNMENT_TIME,                    
                    
  CONVERT(INT, case when SUM(INDEXER_GRANDTOTAL) <> 0 then                    
 SUM(INDEXER_TOTAL_TIME)/SUM(INDEXER_GRANDTOTAL)else 0 end )                    
 AS INDEXER_TOTAL_TIME,                    
                    
  CONVERT(INT, case when SUM(AGENT_GRANDTOTAL) <> 0 then                    
 SUM(AGENT_TOTAL_TIME)/SUM(AGENT_GRANDTOTAL)else 0 end )                    
 AS AGENT_TOTAL_TIME,                    
                    
  CONVERT(INT, case when SUM(SUPERVISOR_GRANDTOTAL) <> 0 then                    
 SUM(SUPERVISOR_TOTAL_TIME)/SUM(SUPERVISOR_GRANDTOTAL)else 0 end )                    
 AS SUPERVISOR_TOTAL_TIME,                    
               
 SUM([0-15])  AS  [0-15] ,SUM([16-30])  AS  [16-30]  ,SUM([31-45])  AS  [31-45]  ,SUM([46-60])  AS  [46-60]                       
 ,SUM([1-2 HOUR])  AS [1-2 HOUR] ,SUM([> 2 HOUR])  AS [> 2 HOUR] ,SUM(GRANDTOTAL) AS GRANDTOTAL                  
                      
    , CONVERT(INT, case when SUM(TOTALTIME_TOCOMPLETE_GRANDTOTAL) <> 0 then                    
 SUM(TOTALTIMEINSECONDS_TOCOMPLETE)/SUM(TOTALTIME_TOCOMPLETE_GRANDTOTAL)else 0 end )                    
 AS TOTALTIMEINSECONDS_TOCOMPLETE                   
                     
 FROM #TEMP6DATA_TOTAL2                       
                      
 UNION ALL                        
 SELECT  '1900/01/01'  DATE,CONVERT(INT, SUM(INDEXER_ASSIGNMENT_TIME)/COUNT(*)) AS INDEXER_ASSIGNMENT_TIME,CONVERT(INT,                         
 SUM(INDEXER_TOTAL_TIME)/COUNT(*)) AS INDEXER_TOTAL_TIME, CONVERT(INT,SUM(AGENT_TOTAL_TIME)/COUNT(*)) AS AGENT_TOTAL_TIME, CONVERT(INT,                         
 SUM(SUPERVISOR_TOTAL_TIME)/COUNT(*)) AS SUPERVISOR_TOTAL_TIME,                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([0-15])) /convert(decimal ,COUNT(*))))  AS  [0-15],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([16-30])) /convert(decimal ,COUNT(*))))  AS  [16-30],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([31-45])) /convert(decimal ,COUNT(*))))  AS  [31-45],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([46-60])) /convert(decimal ,COUNT(*))))  AS  [46-60],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([1-2 HOUR])) /convert(decimal ,COUNT(*))))  AS [1-2 HOUR],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([> 2 HOUR])) /convert(decimal ,COUNT(*))))  AS [> 2 HOUR],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM(GRANDTOTAL)) /convert(decimal ,COUNT(*))))  AS GRANDTOTAL,                        
 CONVERT(INT, SUM(TOTALTIMEINSECONDS_TOCOMPLETE)/COUNT(*)) AS TOTALTIMEINSECONDS_TOCOMPLETE                           
 FROM #TEMP6DATA2                       
                      
                      
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION_TBL2') IS NOT NULL           
     DROP TABLE #PAGINATION_TBL2          
 SELECT ROW_NUMBER() OVER(ORDER BY t.RECEIVE_DATE desc) AS ROW,                        
 * into #PAGINATION_TBL2 FROM (                        
 SELECT                         
CASE Y.RECEIVE_DATE WHEN '1900/01/01' THEN 'GRAND AVG' WHEN '1900/01/02' THEN 'GRAND TOTAL'  ELSE  Y.RECEIVE_DATE END   RECEIVE_DATE ,[0-15] as ZEROTO_FIFTEEN,                       
[16-30] as SIXTEENTO_THIRTY,[31-45] as THIRTYONETO_FOURTFIVE,[46-60] as FOURTYSIXTO_SIXTY,[1-2 HOUR] as GREATERTHAN_HOUR,[> 2 HOUR] as GREATERTHAN_TWOHOUR,GRANDTOTAL                        
,                        
CONVERT(VARCHAR, (y.INDEXER_ASSIGNMENT_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60) END 'INDEXER_ASSIGNMENT_TOTAL_TIME',                         
                        
CONVERT(VARCHAR, (y.INDEXER_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60) END 'INDEXER_TOTAL_TIME',                         
                        
CONVERT(VARCHAR, (y.AGENT_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, AGENT_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60) END 'AGENT_TOTAL_TIME',                        
                        
CONVERT(VARCHAR, (y.SUPERVISOR_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60) END 'SUPERVISOR_TOTAL_TIME',                        
                        
CONVERT(VARCHAR, (y.TOTALTIMEINSECONDS_TOCOMPLETE)/3600 ) +':'+                 
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60) END 'TOTALTIME_INSECONDS_TOCOMPLETE'                        
                        
FROM #TEMP8DATA2  Y LEFT JOIN #FINALRECORD2 U ON Y.RECEIVE_DATE = U.RECEIVE_DATE                        
                        
)t                        
                        
          
          
          
          
          
          
          
                 
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION4') IS NOT NULL DROP TABLE  #PAGINATION4          
SELECT *,RECEIVE_DATE AS REC_DATE INTO   #PAGINATION4 FROM #PAGINATION_TBL2          
          
UPDATE #PAGINATION4          
SET REC_DATE = ''          
WHERE REC_DATE LIKE '%[A^Z]%'          
          
          
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION5') IS NOT NULL DROP TABLE  #PAGINATION5              
SELECT DATENAME(dw,CONVERT(DATE,REC_DATE))AS RECEIVE_DAY,* INTO #PAGINATION5          
FROM #PAGINATION4          
          
          
--SELECT * FROM #PAGINATION5          
          
          
          
          
          
                        
declare @Total_Pages2 BIGINT                        
declare @Total_Records2 BIGINT                 
          
          
          
          
                 
IF(@EXCLUDEWEEKEND=1)                        
BEGIN                         
                        
SET @Total_Pages2  = (Select TOP 1 CAST(CEILING(COUNT(*) OVER()/(CONVERT(INT, @PAGESIZE )*1.0)) AS BIGINT) AS TOTAL_PAGE from #PAGINATION5                      
WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'                         
)                             
SET @Total_Records2  = (select Count(*) from #PAGINATION5 WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY')                        
                        
 select ROW,RECEIVE_DATE,ZEROTO_FIFTEEN,SIXTEENTO_THIRTY,THIRTYONETO_FOURTFIVE,FOURTYSIXTO_SIXTY,GREATERTHAN_HOUR,GREATERTHAN_TWOHOUR,GRANDTOTAL,          
 INDEXER_ASSIGNMENT_TOTAL_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME,TOTALTIME_INSECONDS_TOCOMPLETE,@Total_Pages2 as TOTAL_PAGE,@Total_Records2 as TOTAL_RECORDS                          
 from #PAGINATION5           
 WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'          
 AND ROW BETWEEN( @PAGEINDEX -1) *  @PAGESIZE  + 1 AND(((  @PAGEINDEX  -1) *   @PAGESIZE  + 1) +   @PAGESIZE ) - 1                         
OR ROW=1                        
  ORDER BY ROW                         
END                        
ELSE                        
BEGIN                        
SET @Total_Pages2  = (Select TOP 1 CAST(CEILING(COUNT(*) OVER()/(CONVERT(INT, @PAGESIZE )*1.0)) AS BIGINT) AS TOTAL_PAGE from #PAGINATION5)                             
 SET @Total_Records2  = (select Count(*) from #PAGINATION5)                        
 select ROW,RECEIVE_DATE,ZEROTO_FIFTEEN,SIXTEENTO_THIRTY,THIRTYONETO_FOURTFIVE,FOURTYSIXTO_SIXTY,GREATERTHAN_HOUR,GREATERTHAN_TWOHOUR,GRANDTOTAL,          
 INDEXER_ASSIGNMENT_TOTAL_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME,TOTALTIME_INSECONDS_TOCOMPLETE,@Total_Pages2 as TOTAL_PAGE,@Total_Records2 as TOTAL_RECORDS                         
                        
from #PAGINATION5 where ROW BETWEEN( @PAGEINDEX -1) *  @PAGESIZE  + 1 AND(((  @PAGEINDEX  -1) *   @PAGESIZE  + 1) +   @PAGESIZE ) - 1                         
 OR ROW=1                        
  ORDER BY ROW                         
-------------------Pagination query-----------------------------------------------------------------                        
                        
                        
end                        
                        
END                
          
if(@TIMEFRAME = 'LASTYEAR')                        
                        
begin                        
                        
IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUE_DATA3', 'U') IS NOT NULL DROP TABLE #WORKQUEUE_DATA3                        
IF OBJECT_ID('TEMPDB.DBO.#LOGQUEUE_DATA3', 'U') IS NOT NULL DROP TABLE #LOGQUEUE_DATA3                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP2DATA3', 'U') IS NOT NULL DROP TABLE #TEMP2DATA3                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP3DATA3', 'U') IS NOT NULL DROP TABLE #TEMP3DATA3                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP4DATA3', 'U') IS NOT NULL DROP TABLE #TEMP4DATA3                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA3', 'U') IS NOT NULL DROP TABLE #TEMP5DATAA3                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAB3', 'U') IS NOT NULL DROP TABLE #TEMP5DATAB3                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATA_FINAL3', 'U') IS NOT NULL DROP TABLE #TEMP5DATA_FINAL3                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP6DATA3', 'U') IS NOT NULL DROP TABLE #TEMP6DATA3                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP8DATA3', 'U') IS NOT NULL DROP TABLE #TEMP8DATA3                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIMEDATA3', 'U') IS NOT NULL DROP TABLE #INDEX_TIMEDATA3                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME1DATA3', 'U') IS NOT NULL DROP TABLE #INDEX_TIME1DATA3                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME2DATA3', 'U') IS NOT NULL DROP TABLE #INDEX_TIME2DATA3                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIMEDATA3_FINAL3', 'U') IS NOT NULL DROP TABLE #INDEX_TIMEDATA3_FINAL3                        
IF OBJECT_ID('TEMPDB.DBO.#FINALRECORD3', 'U') IS NOT NULL DROP TABLE  #FINALRECORD3                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA_Set3', 'U') IS NOT NULL DROP TABLE  #TEMP5DATAA_Set3                     
IF OBJECT_ID('TEMPDB.DBO.#TEMP6DATA_TOTAL3', 'U') IS NOT NULL DROP TABLE  #TEMP6DATA_TOTAL3    
IF OBJECT_ID('TEMPDB.DBO.#REFASSIGNDETAILS3', 'U') IS NOT NULL DROP TABLE #REFASSIGNDETAILS3                        
                       
                        
                        
IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUEFILTEREDRECORDS3', 'U') IS NOT NULL DROP TABLE #WORKQUEUEFILTEREDRECORDS3                         
 SELECT * INTO #WORKQUEUEFILTEREDRECORDS3                         
FROM FOX_TBL_WORK_QUEUE where                        
 CONVERT(DATE, RECEIVE_DATE) >= DATEADD(YY, DATEDIFF(YY, 0, GETDATE()) - 1, 0)                                                
AND CONVERT(DATE, RECEIVE_DATE) < DATEADD(DD, -1, DATEADD(YY, DATEDIFF(YY, 0, GETDATE()), 0))                        
AND CONVERT(TIME,RECEIVE_DATE) >=CONVERT(TIME,ISNULL(@BUSINESS_HOURS9,RECEIVE_DATE))                        
AND CONVERT(TIME,RECEIVE_DATE) <=CONVERT(TIME,ISNULL(@BUSINESS_HOURS5,RECEIVE_DATE))                        
AND (DATENAME(dw,RECEIVE_DATE)= ISNULL(@SATURDAYS,DATENAME(dw, RECEIVE_DATE)) OR DATENAME(dw,RECEIVE_DATE) = ISNULL(@SUNDAYS,DATENAME(dw,RECEIVE_DATE)))                        
------------------------------------QURIES-------------------------                        
-----#TIME_ELAPSED_COMPLETION_TIME                        
IF OBJECT_ID('TEMPDB.DBO.#TOTALTIMEINSECONDS3', 'U') IS NOT NULL DROP TABLE #TOTALTIMEINSECONDS3                         
 SELECT (CONVERT(BIGINT,                          
DATEDIFF(MINUTE, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE))))                         
TOTALTIMEINSECONDS,RECEIVE_DATE,COMPLETED_DATE INTO #TOTALTIMEINSECONDS3                         
FROM #WORKQUEUEFILTEREDRECORDS3 WHERE COMPLETED_DATE IS NOT NULL                          
AND ISNULL(DELETED,0)=0                         
AND UPPER(WORK_STATUS)='COMPLETED'                        
                        
                
SELECT DISTINCT  CONVERT(DATE,W.RECEIVE_DATE) RECEIVE_DATE,E.ROLE_NAME,W.WORK_ID,                        
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,W.ASSIGNED_DATE), CONVERT(DATETIME,W.COMPLETED_DATE )))) as COMPLETEFILESCOUNT ,                        
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE)))) TOTALTIMEINSECONDS_TOCOMPLETE                        
 INTO #WORKQUEUE_DATA3 FROM FOX_TBL_WORK_QUEUE W                        
JOIN  FOX_TBL_APPLICATION_USER R ON R.USER_NAME = W.ASSIGNED_TO                         
LEFT JOIN FOX_TBL_ROLE E ON E.ROLE_ID =R.ROLE_ID                        
WHERE COMPLETED_DATE IS NOT NULL                          
AND ISNULL(W.DELETED,0)=0                         
AND UPPER(W.WORK_STATUS)='COMPLETED'                        
    
SELECT * INTO #REFASSIGNDETAILS3 FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS    
WHERE WORK_ID IN (SELECT WORK_ID FROM #WORKQUEUEFILTEREDRECORDS3)      
ORDER BY WORK_ID,CREATED_DATE ASC    
                        
SELECT ASSIGNED_BY_DESIGNATION,WORK_ID,                        
(CONVERT(BIGINT, DATEDIFF(SS,CONVERT(DATETIME,(SELECT TOP 1 ASSIGNED_TIME FROM #REFASSIGNDETAILS3 WHERE WORK_ID=MAIN.WORK_ID AND ASSIGNED_TIME < MAIN.ASSIGNED_TIME                        
ORDER BY ASSIGNED_TIME DESC)), CONVERT(DATETIME,ASSIGNED_TIME)))) TOTALTIMEINSECONDS  INTO #LOGQUEUE_DATA3                        
FROM #REFASSIGNDETAILS3  AS MAIN                        
ORDER BY WORK_ID,CREATED_DATE ASC                        
                        
SELECT  Q.RECEIVE_DATE, A.* INTO #TEMP3DATA3 FROM #LOGQUEUE_DATA3 A                        
JOIN FOX_TBL_WORK_QUEUE Q ON A.WORK_ID = Q.WORK_ID AND UPPER(WORK_STATUS)='COMPLETED' AND ISNULL(DELETED,0)=0                        
                        
                        
SELECT RECEIVE_DATE, ROLE_NAME, SUM(COMPLETEFILESCOUNT) AS TOTAL_TIME,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP2DATA3 FROM (                        
 SELECT * FROM #WORKQUEUE_DATA3 UNION ALL SELECT *,0 TOTALTIMEINSECONDS_TOCOMPLETE FROM #TEMP3DATA3                        
)A                        
GROUP BY A.ROLE_NAME,RECEIVE_DATE                        
                        
----------------INDXER ASSIGN TIME---------------------------------                        
 SELECT D.WORK_ID,W.RECEIVE_DATE,D.CREATED_DATE INTO   #INDEX_TIMEDATA3 FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS D JOIN                         
 FOX_TBL_WORK_QUEUE W ON D.WORK_ID = W.WORK_ID                        
 WHERE UPPER(W.WORK_STATUS)='COMPLETED'  AND W.COMPLETED_DATE IS NOT NULL AND ISNULL(W.DELETED,0) =0  AND UPPER(D.ASSIGNED_TO_DESIGNATION) = 'INDEXER'                        
 SELECT WORK_ID,RECEIVE_DATE,MIN(CREATED_DATE) CREATED_DATE INTO   #INDEX_TIME1DATA3 FROM #INDEX_TIMEDATA3                        
 GROUP BY WORK_ID,RECEIVE_DATE                        
                        
 SELECT WORK_ID,CONVERT(DATE,RECEIVE_DATE)  RECEIVE_DATE,(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,CREATED_DATE)))) INDEXER_ASSIGNMENT_TIME   INTO   #INDEX_TIMEDATA3_FINAL3 FROM #INDEX_TIME1DATA3                      
  
                        
 SELECT RECEIVE_DATE,SUM(INDEXER_ASSIGNMENT_TIME) INDEXER_ASSIGNMENT_TIME  INTO #FINALRECORD3 FROM #INDEX_TIMEDATA3_FINAL3                        
 GROUP BY RECEIVE_DATE                        
                        
SELECT DISTINCT  CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('INDEXER') THEN A.TOTAL_TIME ELSE 0 END) AS INDEXER_TOTAL_TIME,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('AGENT') THEN A.TOTAL_TIME ELSE 0 END) AS AGENT_TOTAL_TIME,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('SUPERVISOR') THEN A.TOTAL_TIME ELSE 0 END) AS SUPERVISOR_TOTAL_TIME                        
 INTO  #TEMP5DATAA_Set3                        
 FROM #TEMP2DATA3 A                        
--LEFT JOIN #TEMP2DATA3 B ON CONVERT(DATE,A.RECEIVE_DATE)=CONVERT(DATE,B.RECEIVE_DATE)                         
                 
                        
select                         
  CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,                        
SUM (isnull(a.INDEXER_TOTAL_TIME,0)) INDEXER_TOTAL_TIME,                        
SUM (isnull(a.AGENT_TOTAL_TIME,0)) AGENT_TOTAL_TIME,                        
SUM (isnull(a.SUPERVISOR_TOTAL_TIME,0)) SUPERVISOR_TOTAL_TIME                        
 INTO  #TEMP5DATAA3                        
 FROM #TEMP5DATAA_Set3 A                        
GROUP BY CONVERT(DATE,a.RECEIVE_DATE)                        
ORDER BY 1                        
---------------------------------------------                        
                        
                        
SELECT  CONVERT(DATE,A.RECEIVE_DATE) RECEIVE_DATE,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATAB3 FROM #WORKQUEUE_DATA3 A                        
GROUP BY CONVERT(DATE,A.RECEIVE_DATE)                        
ORDER BY 1                         
                        
SELECT A.*,B.TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATA_FINAL3 FROM #TEMP5DATAA3 A                        
LEFT JOIN #TEMP5DATAB3 B ON A.RECEIVE_DATE=B.RECEIVE_DATE                        
ORDER BY 1                        
                        
IF OBJECT_ID('TEMPDB.DBO.#TIME_ELAPSED_COMPLETION_TIME3', 'U') IS NOT NULL DROP TABLE #TIME_ELAPSED_COMPLETION_TIME3                         
SELECT                         
CONVERT(DATE,RECEIVE_DATE)  AS DATE,                        
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 0 AND 15 THEN 1 ELSE 0 END) '0-15',                         
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 16 AND 30 THEN 1 ELSE 0 END) '16-30' ,                        
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 31 AND 45 THEN 1 ELSE 0 END) '31-45' ,                         
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 46 AND 60 THEN 1 ELSE 0 END) '46-60' ,                          
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 61 AND 120 THEN 1 ELSE 0 END) '1-2 HOUR',                        
SUM(CASE WHEN  TOTALTIMEINSECONDS > 120 THEN 1 ELSE 0 END) '> 2 HOUR',                        
COUNT(*) AS GRANDTOTAL   INTO #TIME_ELAPSED_COMPLETION_TIME3             
FROM #TOTALTIMEINSECONDS3                         
GROUP BY CONVERT(DATE,RECEIVE_DATE)                ORDER BY CONVERT(DATE,RECEIVE_DATE) ASC                         
                          
                       
                       
 SELECT E.RECEIVE_DATE,CONVERT(INT,E.INDEXER_TOTAL_TIME/(T.GRANDTOTAL)) INDEXER_TOTAL_TIME ,                      
CONVERT(INT,E.AGENT_TOTAL_TIME/(T.GRANDTOTAL)) AGENT_TOTAL_TIME ,                      
CONVERT(INT,E.SUPERVISOR_TOTAL_TIME/(T.GRANDTOTAL)) SUPERVISOR_TOTAL_TIME,                      
CONVERT(INT,E.TOTALTIMEINSECONDS_TOCOMPLETE/(T.GRANDTOTAL)) TOTALTIMEINSECONDS_TOCOMPLETE,                      
CONVERT(INT,D.INDEXER_ASSIGNMENT_TIME/(T.GRANDTOTAL)) INDEXER_ASSIGNMENT_TIME,                     
T.[0-15],T.[16-30],T.[31-45],T.[46-60],T.[1-2 HOUR],T.[> 2 HOUR],T.GRANDTOTAL                    
,CASE WHEN  D.INDEXER_ASSIGNMENT_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_ASSIGNMENT_GRANDTOTAL                    
,CASE WHEN E.INDEXER_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_GRANDTOTAL                    
,CASE WHEN E.AGENT_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS AGENT_GRANDTOTAL                    
,CASE WHEN E.SUPERVISOR_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS SUPERVISOR_GRANDTOTAL                    
,CASE WHEN E.TOTALTIMEINSECONDS_TOCOMPLETE <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS TOTALTIME_TOCOMPLETE_GRANDTOTAL                    
INTO  #TEMP6DATA3                        
FROM #TEMP5DATA_FINAL3 E                        
 JOIN   #TIME_ELAPSED_COMPLETION_TIME3 T                         
ON E.RECEIVE_DATE = T.DATE                         
LEFT JOIN  #FINALRECORD3 D  ON E.RECEIVE_DATE = D.RECEIVE_DATE                        
                       
                    
SELECT E.*, D.INDEXER_ASSIGNMENT_TIME,                     
T.[0-15],T.[16-30],T.[31-45],T.[46-60],T.[1-2 HOUR],T.[> 2 HOUR],T.GRANDTOTAL                    
,CASE WHEN  D.INDEXER_ASSIGNMENT_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_ASSIGNMENT_GRANDTOTAL                    
,CASE WHEN E.INDEXER_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_GRANDTOTAL                    
,CASE WHEN E.AGENT_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS AGENT_GRANDTOTAL                    
,CASE WHEN E.SUPERVISOR_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS SUPERVISOR_GRANDTOTAL                    
,CASE WHEN E.TOTALTIMEINSECONDS_TOCOMPLETE <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS TOTALTIME_TOCOMPLETE_GRANDTOTAL                    
INTO  #TEMP6DATA_TOTAL3                       
FROM #TEMP5DATA_FINAL3 E                        
 JOIN   #TIME_ELAPSED_COMPLETION_TIME3 T                         
ON E.RECEIVE_DATE = T.DATE                         
LEFT JOIN  #FINALRECORD3 D  ON E.RECEIVE_DATE = D.RECEIVE_DATE                         
                       
                       
                          
 SELECT CONVERT(VARCHAR, RECEIVE_DATE ,101) AS RECEIVE_DATE ,INDEXER_ASSIGNMENT_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,                        
 SUPERVISOR_TOTAL_TIME,[0-15],[16-30],[31-45],[46-60],[1-2 HOUR],[> 2 HOUR],GRANDTOTAL,TOTALTIMEINSECONDS_TOCOMPLETE INTO   #TEMP8DATA3 FROM #TEMP6DATA3                        
 UNION ALL                        
                      
    SELECT  '1900/01/02'  DATE,                    
 CONVERT(INT, case when SUM(INDEXER_ASSIGNMENT_GRANDTOTAL) <> 0 then                    
 SUM(INDEXER_ASSIGNMENT_TIME)/SUM(INDEXER_ASSIGNMENT_GRANDTOTAL)else 0 end )                    
 AS INDEXER_ASSIGNMENT_TIME,                    
                    
  CONVERT(INT, case when SUM(INDEXER_GRANDTOTAL) <> 0 then                    
 SUM(INDEXER_TOTAL_TIME)/SUM(INDEXER_GRANDTOTAL)else 0 end )                    
 AS INDEXER_TOTAL_TIME,                    
                    
  CONVERT(INT, case when SUM(AGENT_GRANDTOTAL) <> 0 then                    
 SUM(AGENT_TOTAL_TIME)/SUM(AGENT_GRANDTOTAL)else 0 end )                    
 AS AGENT_TOTAL_TIME,                    
                    
  CONVERT(INT, case when SUM(SUPERVISOR_GRANDTOTAL) <> 0 then                    
 SUM(SUPERVISOR_TOTAL_TIME)/SUM(SUPERVISOR_GRANDTOTAL)else 0 end )                    
 AS SUPERVISOR_TOTAL_TIME,                    
                    
 SUM([0-15])  AS  [0-15] ,SUM([16-30])  AS  [16-30]  ,SUM([31-45])  AS  [31-45]  ,SUM([46-60])  AS  [46-60]                       
 ,SUM([1-2 HOUR])  AS [1-2 HOUR] ,SUM([> 2 HOUR])  AS [> 2 HOUR] ,SUM(GRANDTOTAL) AS GRANDTOTAL                     
                  
  ,CONVERT(INT, case when SUM(TOTALTIME_TOCOMPLETE_GRANDTOTAL) <> 0 then                    
 SUM(TOTALTIMEINSECONDS_TOCOMPLETE)/SUM(TOTALTIME_TOCOMPLETE_GRANDTOTAL)else 0 end )                    
 AS TOTALTIMEINSECONDS_TOCOMPLETE                   
                    
                  
 FROM #TEMP6DATA_TOTAL3                       
                      
 UNION ALL                        
 SELECT  '1900/01/01'  DATE,CONVERT(INT, SUM(INDEXER_ASSIGNMENT_TIME)/COUNT(*)) AS INDEXER_ASSIGNMENT_TIME,CONVERT(INT,                         
 SUM(INDEXER_TOTAL_TIME)/COUNT(*)) AS INDEXER_TOTAL_TIME, CONVERT(INT,SUM(AGENT_TOTAL_TIME)/COUNT(*)) AS AGENT_TOTAL_TIME, CONVERT(INT,                         
 SUM(SUPERVISOR_TOTAL_TIME)/COUNT(*)) AS SUPERVISOR_TOTAL_TIME,                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([0-15])) /convert(decimal ,COUNT(*))))  AS  [0-15],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([16-30])) /convert(decimal ,COUNT(*))))  AS  [16-30],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([31-45])) /convert(decimal ,COUNT(*))))  AS  [31-45],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([46-60])) /convert(decimal ,COUNT(*))))  AS  [46-60],         
 Convert(INT,convert(decimal, convert(decimal ,SUM([1-2 HOUR])) /convert(decimal ,COUNT(*))))  AS [1-2 HOUR],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([> 2 HOUR])) /convert(decimal ,COUNT(*))))  AS [> 2 HOUR],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM(GRANDTOTAL)) /convert(decimal ,COUNT(*))))  AS GRANDTOTAL,                        
 CONVERT(INT, SUM(TOTALTIMEINSECONDS_TOCOMPLETE)/COUNT(*)) AS TOTALTIMEINSECONDS_TOCOMPLETE                           
  FROM #TEMP6DATA3                       
                        
                        
          
                 
           
          
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION_TBL3') IS NOT NULL           
DROP TABLE  #PAGINATION_TBL3          
 SELECT ROW_NUMBER() OVER(ORDER BY t.RECEIVE_DATE desc) AS ROW,                        
 * into #PAGINATION_TBL3 FROM (                        
 SELECT                         
CASE Y.RECEIVE_DATE WHEN '1900/01/01' THEN 'GRAND AVG' WHEN '1900/01/02' THEN 'GRAND TOTAL'  ELSE  Y.RECEIVE_DATE END  RECEIVE_DATE ,[0-15] as ZEROTO_FIFTEEN,[16-30] as SIXTEENTO_THIRTY,                        
[31-45] as THIRTYONETO_FOURTFIVE,[46-60] as FOURTYSIXTO_SIXTY,[1-2 HOUR] as GREATERTHAN_HOUR,[> 2 HOUR] as GREATERTHAN_TWOHOUR,GRANDTOTAL                        
,                        
CONVERT(VARCHAR, (y.INDEXER_ASSIGNMENT_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) END                        
   +':'+                    
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60) END 'INDEXER_ASSIGNMENT_TOTAL_TIME',                         
                        
CONVERT(VARCHAR, (y.INDEXER_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60) END 'INDEXER_TOTAL_TIME',                         
                        
CONVERT(VARCHAR, (y.AGENT_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, AGENT_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60) END 'AGENT_TOTAL_TIME',                        
                        
CONVERT(VARCHAR, (y.SUPERVISOR_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                
CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60) END 'SUPERVISOR_TOTAL_TIME',                        
                        
CONVERT(VARCHAR, (y.TOTALTIMEINSECONDS_TOCOMPLETE)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60) END 'TOTALTIME_INSECONDS_TOCOMPLETE'                        
FROM #TEMP8DATA3  Y LEFT JOIN #FINALRECORD3 U ON Y.RECEIVE_DATE = U.RECEIVE_DATE                        
                        
)t                        
                 
          
--END          
          
                 
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION6') IS NOT NULL DROP TABLE  #PAGINATION6          
SELECT *,RECEIVE_DATE AS REC_DATE INTO   #PAGINATION6 FROM #PAGINATION_TBL3          
          
UPDATE #PAGINATION6          
SET REC_DATE = ''          
WHERE REC_DATE LIKE '%[A^Z]%'          
          
          
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION7') IS NOT NULL DROP TABLE  #PAGINATION7              
SELECT DATENAME(dw,CONVERT(DATE,REC_DATE))AS RECEIVE_DAY,* INTO #PAGINATION7          
FROM #PAGINATION6          
           
                     
declare @Total_Pages3 BIGINT                        
declare @Total_Records3 BIGINT                        
          
IF(@EXCLUDEWEEKEND=1)                        
BEGIN                         
                        
SET @Total_Pages3  = (Select TOP 1 CAST(CEILING(COUNT(*) OVER()/(CONVERT(INT, @PAGESIZE )*1.0)) AS BIGINT) AS TOTAL_PAGE from #PAGINATION7                        
WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'                                    
)                             
SET @Total_Records3  = (select Count(*) from #PAGINATION7    WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY')                        
                        
 select ROW,RECEIVE_DATE,ZEROTO_FIFTEEN,SIXTEENTO_THIRTY,THIRTYONETO_FOURTFIVE,FOURTYSIXTO_SIXTY,GREATERTHAN_HOUR,GREATERTHAN_TWOHOUR,GRANDTOTAL,          
 INDEXER_ASSIGNMENT_TOTAL_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME,TOTALTIME_INSECONDS_TOCOMPLETE,@Total_Pages3 as TOTAL_PAGE,@Total_Records3 as TOTAL_RECORDS                          
 from #PAGINATION7 WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'                                  
 AND ROW BETWEEN( @PAGEINDEX -1) *  @PAGESIZE  + 1 AND(((  @PAGEINDEX  -1) *   @PAGESIZE  + 1) +   @PAGESIZE ) - 1                         
 OR ROW=1                        
  ORDER BY ROW                         
END                        
ELSE                        
BEGIN                        
SET @Total_Pages3  = (Select TOP 1 CAST(CEILING(COUNT(*) OVER()/(CONVERT(INT, @PAGESIZE )*1.0)) AS BIGINT) AS TOTAL_PAGE from #PAGINATION7)                             
 SET @Total_Records  = (select Count(*) from #PAGINATION_TBL3)                        
 select ROW,RECEIVE_DATE,ZEROTO_FIFTEEN,SIXTEENTO_THIRTY,THIRTYONETO_FOURTFIVE,FOURTYSIXTO_SIXTY,GREATERTHAN_HOUR,GREATERTHAN_TWOHOUR,GRANDTOTAL,          
 INDEXER_ASSIGNMENT_TOTAL_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME,TOTALTIME_INSECONDS_TOCOMPLETE,@Total_Pages3 as TOTAL_PAGE,@Total_Records3 as TOTAL_RECORDS                         
                        
 from #PAGINATION7 where ROW BETWEEN( @PAGEINDEX -1) *  @PAGESIZE  + 1 AND(((  @PAGEINDEX  -1) *   @PAGESIZE  + 1) +   @PAGESIZE ) - 1                         
 OR ROW=1                        
ORDER BY ROW               
          
          
--SELECT * FROM #PAGINATION7 WHERE RECEIVE_DAY ='SATURDAY'   OR  RECEIVE_DAY ='SUNDAY'          
          
                    
-------------------Pagination query-----------------------------------------------------------------                        
                        
                        
end                        
                        
END                
          
                  
if(@TIMEFRAME = 'LAST30DAYS')              
                    
begin                        
                        
IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUE_DATA4', 'U') IS NOT NULL DROP TABLE #WORKQUEUE_DATA4                        
IF OBJECT_ID('TEMPDB.DBO.#LOGQUEUE_DATA4', 'U') IS NOT NULL DROP TABLE #LOGQUEUE_DATA4              IF OBJECT_ID('TEMPDB.DBO.#TEMP2DATA4', 'U') IS NOT NULL DROP TABLE #TEMP2DATA4                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP3DATA4', 'U') IS NOT NULL DROP TABLE #TEMP3DATA4                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP4DATA4', 'U') IS NOT NULL DROP TABLE #TEMP4DATA4                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA4', 'U') IS NOT NULL DROP TABLE #TEMP5DATAA4                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAB4', 'U') IS NOT NULL DROP TABLE #TEMP5DATAB4                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATA_FINAL4', 'U') IS NOT NULL DROP TABLE #TEMP5DATA_FINAL4                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP6DATA4', 'U') IS NOT NULL DROP TABLE #TEMP6DATA4                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP8DATA4', 'U') IS NOT NULL DROP TABLE #TEMP8DATA4                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIMEDATA4', 'U') IS NOT NULL DROP TABLE #INDEX_TIMEDATA4                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME1DATA4', 'U') IS NOT NULL DROP TABLE #INDEX_TIME1DATA4                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME2DATA4', 'U') IS NOT NULL DROP TABLE #INDEX_TIME2DATA4                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIMEDATA4_FINAL4', 'U') IS NOT NULL DROP TABLE #INDEX_TIMEDATA4_FINAL4                        
IF OBJECT_ID('TEMPDB.DBO.#FINALRECORD4', 'U') IS NOT NULL DROP TABLE  #FINALRECORD4                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA_Set4', 'U') IS NOT NULL DROP TABLE  #TEMP5DATAA_Set4                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP6DATA_TOTAL4', 'U') IS NOT NULL DROP TABLE  #TEMP6DATA_TOTAL4    
IF OBJECT_ID('TEMPDB.DBO.#REFASSIGNDETAILS4', 'U') IS NOT NULL DROP TABLE #REFASSIGNDETAILS4                        
                    
                        
IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUEFILTEREDRECORDS4', 'U') IS NOT NULL DROP TABLE #WORKQUEUEFILTEREDRECORDS4                        
 SELECT * INTO #WORKQUEUEFILTEREDRECORDS4                         
FROM FOX_TBL_WORK_QUEUE where                        
 CONVERT(DATE, RECEIVE_DATE) >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), -30)                                                
 AND CONVERT(DATE, RECEIVE_DATE) <= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 1)                        
AND CONVERT(TIME,RECEIVE_DATE) >=CONVERT(TIME,ISNULL(@BUSINESS_HOURS9,RECEIVE_DATE))                        
AND CONVERT(TIME,RECEIVE_DATE) <=CONVERT(TIME,ISNULL(@BUSINESS_HOURS5,RECEIVE_DATE))                        
AND (DATENAME(dw,RECEIVE_DATE)= ISNULL(@SATURDAYS,DATENAME(dw, RECEIVE_DATE)) OR DATENAME(dw,RECEIVE_DATE) = ISNULL(@SUNDAYS,DATENAME(dw,RECEIVE_DATE)))                        
 ------------------------------------QURIES-------------------------                        
 -----#TIME_ELAPSED_COMPLETION_TIME                        
IF OBJECT_ID('TEMPDB.DBO.#TOTALTIMEINSECONDS4', 'U') IS NOT NULL DROP TABLE #TOTALTIMEINSECONDS4                         
 SELECT (CONVERT(BIGINT,                          
DATEDIFF(MINUTE, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE))))                         
TOTALTIMEINSECONDS,RECEIVE_DATE,COMPLETED_DATE INTO #TOTALTIMEINSECONDS4                         
FROM #WORKQUEUEFILTEREDRECORDS4 WHERE COMPLETED_DATE IS NOT NULL                          
AND ISNULL(DELETED,0)=0                         
AND UPPER(WORK_STATUS)='COMPLETED'                        
                        
                        
SELECT DISTINCT  CONVERT(DATE,W.RECEIVE_DATE) RECEIVE_DATE,E.ROLE_NAME,W.WORK_ID,                        
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,W.ASSIGNED_DATE), CONVERT(DATETIME,W.COMPLETED_DATE )))) as COMPLETEFILESCOUNT ,                        
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE)))) TOTALTIMEINSECONDS_TOCOMPLETE                        
 INTO #WORKQUEUE_DATA4 FROM FOX_TBL_WORK_QUEUE W                        
JOIN  FOX_TBL_APPLICATION_USER R ON R.USER_NAME = W.ASSIGNED_TO                         
LEFT JOIN FOX_TBL_ROLE E ON E.ROLE_ID =R.ROLE_ID                        
WHERE COMPLETED_DATE IS NOT NULL                          
AND ISNULL(W.DELETED,0)=0                         
AND UPPER(W.WORK_STATUS)='COMPLETED'                        
                        
SELECT * INTO #REFASSIGNDETAILS4 FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS    
WHERE WORK_ID IN (SELECT WORK_ID FROM #WORKQUEUEFILTEREDRECORDS4)      
ORDER BY WORK_ID,CREATED_DATE ASC    
                        
SELECT ASSIGNED_BY_DESIGNATION,WORK_ID,                        
(CONVERT(BIGINT, DATEDIFF(SS,CONVERT(DATETIME,(SELECT TOP 1 ASSIGNED_TIME FROM #REFASSIGNDETAILS4 WHERE WORK_ID=MAIN.WORK_ID AND ASSIGNED_TIME < MAIN.ASSIGNED_TIME                        
ORDER BY ASSIGNED_TIME DESC)), CONVERT(DATETIME,ASSIGNED_TIME)))) TOTALTIMEINSECONDS  INTO #LOGQUEUE_DATA4                        
FROM #REFASSIGNDETAILS4  AS MAIN                        
ORDER BY WORK_ID,CREATED_DATE ASC                        
                        
SELECT  Q.RECEIVE_DATE, A.* INTO #TEMP3DATA4 FROM #LOGQUEUE_DATA4 A                        
JOIN FOX_TBL_WORK_QUEUE Q ON A.WORK_ID = Q.WORK_ID AND UPPER(WORK_STATUS)='COMPLETED' AND ISNULL(DELETED,0)=0                        
                        
                        
SELECT RECEIVE_DATE, ROLE_NAME, SUM(COMPLETEFILESCOUNT) AS TOTAL_TIME,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP2DATA4 FROM (                        
 SELECT * FROM #WORKQUEUE_DATA4 UNION ALL SELECT *,0 TOTALTIMEINSECONDS_TOCOMPLETE FROM #TEMP3DATA4                        
)A                        
GROUP BY A.ROLE_NAME,RECEIVE_DATE                        
                        
 ----------------INDXER ASSIGN TIME---------------------------------                        
 SELECT D.WORK_ID,W.RECEIVE_DATE,D.CREATED_DATE INTO   #INDEX_TIMEDATA4 FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS D JOIN                         
 FOX_TBL_WORK_QUEUE W ON D.WORK_ID = W.WORK_ID                        
 WHERE UPPER(W.WORK_STATUS)='COMPLETED'  AND W.COMPLETED_DATE IS NOT NULL AND ISNULL(W.DELETED,0) =0  AND UPPER(D.ASSIGNED_TO_DESIGNATION) = 'INDEXER'                        
 SELECT WORK_ID,RECEIVE_DATE,MIN(CREATED_DATE) CREATED_DATE INTO   #INDEX_TIME1DATA4 FROM #INDEX_TIMEDATA4                        
 GROUP BY WORK_ID,RECEIVE_DATE                        
                        
 SELECT WORK_ID,CONVERT(DATE,RECEIVE_DATE)  RECEIVE_DATE,(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,CREATED_DATE)))) INDEXER_ASSIGNMENT_TIME   INTO   #INDEX_TIMEDATA4_FINAL4 FROM #INDEX_TIME1DATA4                      
  
                        
 SELECT RECEIVE_DATE,SUM(INDEXER_ASSIGNMENT_TIME) INDEXER_ASSIGNMENT_TIME  INTO #FINALRECORD4 FROM #INDEX_TIMEDATA4_FINAL4                        
 GROUP BY RECEIVE_DATE                        
                        
SELECT DISTINCT  CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('INDEXER') THEN A.TOTAL_TIME ELSE 0 END) AS INDEXER_TOTAL_TIME,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('AGENT') THEN A.TOTAL_TIME ELSE 0 END) AS AGENT_TOTAL_TIME,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('SUPERVISOR') THEN A.TOTAL_TIME ELSE 0 END) AS SUPERVISOR_TOTAL_TIME                        
 INTO  #TEMP5DATAA_Set4                        
 FROM #TEMP2DATA4 A                        
--LEFT JOIN #TEMP2DATA4 B ON CONVERT(DATE,A.RECEIVE_DATE)=CONVERT(DATE,B.RECEIVE_DATE)                         
                        
                        
select                         
  CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,                        
SUM (isnull(a.INDEXER_TOTAL_TIME,0)) INDEXER_TOTAL_TIME,                        
SUM (isnull(a.AGENT_TOTAL_TIME,0)) AGENT_TOTAL_TIME,                        
SUM (isnull(a.SUPERVISOR_TOTAL_TIME,0)) SUPERVISOR_TOTAL_TIME                        
 INTO  #TEMP5DATAA4                        
 FROM #TEMP5DATAA_Set4 A                        
GROUP BY CONVERT(DATE,a.RECEIVE_DATE)           
ORDER BY 1                        
---------------------------------------------                        
                        
SELECT  CONVERT(DATE,A.RECEIVE_DATE) RECEIVE_DATE,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATAB4 FROM #WORKQUEUE_DATA4 A                        
GROUP BY CONVERT(DATE,A.RECEIVE_DATE)                        
ORDER BY 1                         
                        
SELECT A.*,B.TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATA_FINAL4 FROM #TEMP5DATAA4 A                        
LEFT JOIN #TEMP5DATAB4 B ON A.RECEIVE_DATE=B.RECEIVE_DATE                        
ORDER BY 1                        
                        
                        
                        
IF OBJECT_ID('TEMPDB.DBO.#TIME_ELAPSED_COMPLETION_TIME4', 'U') IS NOT NULL DROP TABLE #TIME_ELAPSED_COMPLETION_TIME4                         
SELECT                         
CONVERT(DATE,RECEIVE_DATE)  AS DATE,                        
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 0 AND 15 THEN 1 ELSE 0 END) '0-15',                         
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 16 AND 30 THEN 1 ELSE 0 END) '16-30' ,                        
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 31 AND 45 THEN 1 ELSE 0 END) '31-45' ,                         
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 46 AND 60 THEN 1 ELSE 0 END) '46-60' ,                          
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 61 AND 120 THEN 1 ELSE 0 END) '1-2 HOUR',                        
SUM(CASE WHEN  TOTALTIMEINSECONDS > 120 THEN 1 ELSE 0 END) '> 2 HOUR',                        
COUNT(*) AS GRANDTOTAL   INTO #TIME_ELAPSED_COMPLETION_TIME4                        
FROM #TOTALTIMEINSECONDS4                        
GROUP BY CONVERT(DATE,RECEIVE_DATE)                         
ORDER BY CONVERT(DATE,RECEIVE_DATE) ASC                         
                        
                      
                        
                    
 SELECT E.RECEIVE_DATE,CONVERT(INT,E.INDEXER_TOTAL_TIME/(T.GRANDTOTAL)) INDEXER_TOTAL_TIME ,                      
CONVERT(INT,E.AGENT_TOTAL_TIME/(T.GRANDTOTAL)) AGENT_TOTAL_TIME ,                      
CONVERT(INT,E.SUPERVISOR_TOTAL_TIME/(T.GRANDTOTAL)) SUPERVISOR_TOTAL_TIME,                      
CONVERT(INT,E.TOTALTIMEINSECONDS_TOCOMPLETE/(T.GRANDTOTAL)) TOTALTIMEINSECONDS_TOCOMPLETE,                      
CONVERT(INT,D.INDEXER_ASSIGNMENT_TIME/(T.GRANDTOTAL)) INDEXER_ASSIGNMENT_TIME,                     
T.[0-15],T.[16-30],T.[31-45],T.[46-60],T.[1-2 HOUR],T.[> 2 HOUR],T.GRANDTOTAL                    
,CASE WHEN  D.INDEXER_ASSIGNMENT_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_ASSIGNMENT_GRANDTOTAL                    
,CASE WHEN E.INDEXER_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_GRANDTOTAL                    
,CASE WHEN E.AGENT_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS AGENT_GRANDTOTAL                    
,CASE WHEN E.SUPERVISOR_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS SUPERVISOR_GRANDTOTAL                    
,CASE WHEN E.TOTALTIMEINSECONDS_TOCOMPLETE <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS TOTALTIME_TOCOMPLETE_GRANDTOTAL                    
INTO  #TEMP6DATA4                        
 FROM #TEMP5DATA_FINAL4 E                        
 JOIN   #TIME_ELAPSED_COMPLETION_TIME4 T                         
ON E.RECEIVE_DATE = T.DATE                         
LEFT JOIN  #FINALRECORD4 D  ON E.RECEIVE_DATE = D.RECEIVE_DATE                          
                       
                    
SELECT E.*, D.INDEXER_ASSIGNMENT_TIME,                     
T.[0-15],T.[16-30],T.[31-45],T.[46-60],T.[1-2 HOUR],T.[> 2 HOUR],T.GRANDTOTAL                    
,CASE WHEN  D.INDEXER_ASSIGNMENT_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_ASSIGNMENT_GRANDTOTAL                    
,CASE WHEN E.INDEXER_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_GRANDTOTAL                    
,CASE WHEN E.AGENT_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS AGENT_GRANDTOTAL                    
,CASE WHEN E.SUPERVISOR_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS SUPERVISOR_GRANDTOTAL                    
,CASE WHEN E.TOTALTIMEINSECONDS_TOCOMPLETE <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS TOTALTIME_TOCOMPLETE_GRANDTOTAL                    
INTO  #TEMP6DATA_TOTAL4                      
 FROM #TEMP5DATA_FINAL4 E                        
 JOIN   #TIME_ELAPSED_COMPLETION_TIME4 T                         
ON E.RECEIVE_DATE = T.DATE                         
LEFT JOIN  #FINALRECORD4 D  ON E.RECEIVE_DATE = D.RECEIVE_DATE                                   
                    
                          
 SELECT CONVERT(VARCHAR, RECEIVE_DATE ,101) AS RECEIVE_DATE ,INDEXER_ASSIGNMENT_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME                        
 ,[0-15],[16-30],[31-45],[46-60],[1-2 HOUR],[> 2 HOUR],GRANDTOTAL,TOTALTIMEINSECONDS_TOCOMPLETE INTO   #TEMP8DATA4 FROM #TEMP6DATA4                        
 UNION ALL                     
                        
   SELECT  '1900/01/02'  DATE,                    
 CONVERT(INT, case when SUM(INDEXER_ASSIGNMENT_GRANDTOTAL) <> 0 then                    
 SUM(INDEXER_ASSIGNMENT_TIME)/SUM(INDEXER_ASSIGNMENT_GRANDTOTAL)else 0 end )                    
 AS INDEXER_ASSIGNMENT_TIME,                    
                    
  CONVERT(INT, case when SUM(INDEXER_GRANDTOTAL) <> 0 then                    
 SUM(INDEXER_TOTAL_TIME)/SUM(INDEXER_GRANDTOTAL)else 0 end )                    
 AS INDEXER_TOTAL_TIME,                    
                    
  CONVERT(INT, case when SUM(AGENT_GRANDTOTAL) <> 0 then                    
 SUM(AGENT_TOTAL_TIME)/SUM(AGENT_GRANDTOTAL)else 0 end )                    
 AS AGENT_TOTAL_TIME,                    
                    
  CONVERT(INT, case when SUM(SUPERVISOR_GRANDTOTAL) <> 0 then                    
 SUM(SUPERVISOR_TOTAL_TIME)/SUM(SUPERVISOR_GRANDTOTAL)else 0 end )                    
 AS SUPERVISOR_TOTAL_TIME,                    
                    
 SUM([0-15])  AS  [0-15] ,SUM([16-30])  AS  [16-30]  ,SUM([31-45])  AS  [31-45]  ,SUM([46-60])  AS  [46-60]                       
 ,SUM([1-2 HOUR])  AS [1-2 HOUR] ,SUM([> 2 HOUR])  AS [> 2 HOUR] ,SUM(GRANDTOTAL) AS GRANDTOTAL                     
                  
  ,CONVERT(INT, case when SUM(TOTALTIME_TOCOMPLETE_GRANDTOTAL) <> 0 then                    
 SUM(TOTALTIMEINSECONDS_TOCOMPLETE)/SUM(TOTALTIME_TOCOMPLETE_GRANDTOTAL)else 0 end )                    
 AS TOTALTIMEINSECONDS_TOCOMPLETE                   
                   
                   
 FROM #TEMP6DATA_TOTAL4                       
            
 UNION ALL                        
 SELECT  '1900/01/01'  DATE,CONVERT(INT, SUM(INDEXER_ASSIGNMENT_TIME)/COUNT(*)) AS INDEXER_ASSIGNMENT_TIME,CONVERT(INT,                         
 SUM(INDEXER_TOTAL_TIME)/COUNT(*)) AS INDEXER_TOTAL_TIME, CONVERT(INT,SUM(AGENT_TOTAL_TIME)/COUNT(*)) AS AGENT_TOTAL_TIME, CONVERT(INT,                         
 SUM(SUPERVISOR_TOTAL_TIME)/COUNT(*)) AS SUPERVISOR_TOTAL_TIME,                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([0-15])) /convert(decimal ,COUNT(*))))  AS  [0-15],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([16-30])) /convert(decimal ,COUNT(*))))  AS  [16-30],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([31-45])) /convert(decimal ,COUNT(*))))  AS  [31-45],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([46-60])) /convert(decimal ,COUNT(*))))  AS  [46-60],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([1-2 HOUR])) /convert(decimal ,COUNT(*))))  AS [1-2 HOUR],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([> 2 HOUR])) /convert(decimal ,COUNT(*))))  AS [> 2 HOUR],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM(GRANDTOTAL)) /convert(decimal ,COUNT(*))))  AS GRANDTOTAL,                        
 CONVERT(INT, SUM(TOTALTIMEINSECONDS_TOCOMPLETE)/COUNT(*)) AS TOTALTIMEINSECONDS_TOCOMPLETE                           
  FROM #TEMP6DATA4                       
                      
                     
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION_TBL4') IS NOT NULL           
 DROP TABLE  #PAGINATION_TBL4                  
SELECT ROW_NUMBER() OVER(ORDER BY t.RECEIVE_DATE desc) AS ROW, * into #PAGINATION_TBL4 FROM (                        
SELECT                         
CASE Y.RECEIVE_DATE WHEN '1900/01/01' THEN 'GRAND AVG' WHEN '1900/01/02' THEN 'GRAND TOTAL'  ELSE  Y.RECEIVE_DATE  END  RECEIVE_DATE ,[0-15] as ZEROTO_FIFTEEN,[16-30] as SIXTEENTO_THIRTY,                        
[31-45] as THIRTYONETO_FOURTFIVE,[46-60] as FOURTYSIXTO_SIXTY,[1-2 HOUR] as GREATERTHAN_HOUR,[> 2 HOUR] as GREATERTHAN_TWOHOUR,GRANDTOTAL              ,                        
CONVERT(VARCHAR, (y.INDEXER_ASSIGNMENT_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60) END 'INDEXER_ASSIGNMENT_TOTAL_TIME',                         
                        
CONVERT(VARCHAR, (y.INDEXER_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) END               
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60) END 'INDEXER_TOTAL_TIME',                         
                        
CONVERT(VARCHAR, (y.AGENT_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                        
CONVERT(VARCHAR, CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, AGENT_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60) END 'AGENT_TOTAL_TIME',                        
                        
CONVERT(VARCHAR, (y.SUPERVISOR_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                       
CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60) END 'SUPERVISOR_TOTAL_TIME',                        
                        
CONVERT(VARCHAR, (y.TOTALTIMEINSECONDS_TOCOMPLETE)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60) END 'TOTALTIME_INSECONDS_TOCOMPLETE'                        
                        
FROM #TEMP8DATA4  Y LEFT JOIN #FINALRECORD4 U ON Y.RECEIVE_DATE = U.RECEIVE_DATE                        
                        
)          
t                        
               
          
--SELECT * FROM #PAGINATION_TBL4          
          
          
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION8') IS NOT NULL DROP TABLE  #PAGINATION8          
SELECT *,RECEIVE_DATE AS REC_DATE INTO   #PAGINATION8 FROM #PAGINATION_TBL4          
          
UPDATE #PAGINATION8          
SET REC_DATE = ''          
WHERE REC_DATE LIKE '%[A^Z]%'          
          
          
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION9') IS NOT NULL DROP TABLE  #PAGINATION9              
SELECT DATENAME(dw,CONVERT(DATE,REC_DATE))AS RECEIVE_DAY,* INTO #PAGINATION9          
FROM #PAGINATION8            
            
            
                     
declare @Total_Pages4 BIGINT                        
declare @Total_Records4 BIGINT                    
              
IF(@EXCLUDEWEEKEND=1)                        
BEGIN                         
                        
SET @Total_Pages4  = (Select TOP 1 CAST(CEILING(COUNT(*) OVER()/(CONVERT(INT, @PAGESIZE )*1.0)) AS BIGINT) AS TOTAL_PAGE from #PAGINATION9                        
WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'  )                             
SET @Total_Records4  = (select Count(*) from #PAGINATION9                         
WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'  )                        
                        
 select  ROW,RECEIVE_DATE,ZEROTO_FIFTEEN,SIXTEENTO_THIRTY,THIRTYONETO_FOURTFIVE,FOURTYSIXTO_SIXTY,GREATERTHAN_HOUR,GREATERTHAN_TWOHOUR,GRANDTOTAL,          
 INDEXER_ASSIGNMENT_TOTAL_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME,TOTALTIME_INSECONDS_TOCOMPLETE,@Total_Pages4 as TOTAL_PAGE,@Total_Records4 as TOTAL_RECORDS                          
 from #PAGINATION9 WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'                       AND ROW BETWEEN( @PAGEINDEX -1) *  @PAGESIZE  + 1 AND(((  @PAGEINDEX  -1) *   @PAGESIZE  + 1) +   @PAGESIZE ) - 1                         
 OR ROW=1                        
  ORDER BY ROW                         
END                        
ELSE                        
BEGIN                        
SET @Total_Pages4  = (Select TOP 1 CAST(CEILING(COUNT(*) OVER()/(CONVERT(INT, @PAGESIZE )*1.0)) AS BIGINT) AS TOTAL_PAGE from #PAGINATION9)                             
 SET @Total_Records4  = (select Count(*) from #PAGINATION9)                
 select  ROW,RECEIVE_DATE,ZEROTO_FIFTEEN,SIXTEENTO_THIRTY,THIRTYONETO_FOURTFIVE,FOURTYSIXTO_SIXTY,GREATERTHAN_HOUR,GREATERTHAN_TWOHOUR,GRANDTOTAL,          
 INDEXER_ASSIGNMENT_TOTAL_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME,TOTALTIME_INSECONDS_TOCOMPLETE,@Total_Pages4 as TOTAL_PAGE,@Total_Records4 as TOTAL_RECORDS                         
                        
 from #PAGINATION9          
 where ROW BETWEEN( @PAGEINDEX -1) *  @PAGESIZE  + 1 AND(((  @PAGEINDEX  -1) *   @PAGESIZE  + 1) +   @PAGESIZE ) - 1                         
 OR ROW=1           ORDER BY ROW                        
-------------------Pagination query-----------------------------------------------------------------                        
               
END                        
END               
          
          
--SELECT * FROM #PAGINATION9  WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'                        
          
              
           
                
                        
if(@TIMEFRAME = 'DATERANGE')                        
                        
                        
begin                        
                        
IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUE_DATA5', 'U') IS NOT NULL DROP TABLE #WORKQUEUE_DATA5                        
IF OBJECT_ID('TEMPDB.DBO.#LOGQUEUE_DATA5', 'U') IS NOT NULL DROP TABLE #LOGQUEUE_DATA5                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP2DATA5', 'U') IS NOT NULL DROP TABLE #TEMP2DATA5                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP3DATA5', 'U') IS NOT NULL DROP TABLE #TEMP3DATA5                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP4DATA5', 'U') IS NOT NULL DROP TABLE #TEMP4DATA5                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA5', 'U') IS NOT NULL DROP TABLE #TEMP5DATAA5                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAB5', 'U') IS NOT NULL DROP TABLE #TEMP5DATAB5                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATA_FINAL5', 'U') IS NOT NULL DROP TABLE #TEMP5DATA_FINAL5                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP6DATA5', 'U') IS NOT NULL DROP TABLE #TEMP6DATA5                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP8DATA5', 'U') IS NOT NULL DROP TABLE #TEMP8DATA5                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIMEDATA5', 'U') IS NOT NULL DROP TABLE #INDEX_TIMEDATA5                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME1DATA5', 'U') IS NOT NULL DROP TABLE #INDEX_TIME1DATA5                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIME2DATA5', 'U') IS NOT NULL DROP TABLE #INDEX_TIME2DATA5                        
IF OBJECT_ID('TEMPDB.DBO.#INDEX_TIMEDATA5_FINAL5', 'U') IS NOT NULL DROP TABLE #INDEX_TIMEDATA5_FINAL5                        
IF OBJECT_ID('TEMPDB.DBO.#FINALRECORD5', 'U') IS NOT NULL DROP TABLE  #FINALRECORD5                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP6DATA_TOTAL5', 'U') IS NOT NULL DROP TABLE  #TEMP6DATA_TOTAL5    
IF OBJECT_ID('TEMPDB.DBO.#REFASSIGNDETAILS5', 'U') IS NOT NULL DROP TABLE #REFASSIGNDETAILS5                        
                    
                        
IF OBJECT_ID('TEMPDB.DBO.#WORKQUEUEFILTEREDRECORDS5', 'U') IS NOT NULL DROP TABLE #WORKQUEUEFILTEREDRECORDS5                         
 SELECT *  INTO #WORKQUEUEFILTEREDRECORDS5                         
FROM FOX_TBL_WORK_QUEUE where                        
CONVERT(DATE, RECEIVE_DATE) >= CONVERT(DATE, @DATEFROM)                                                
  AND CONVERT(DATE, RECEIVE_DATE) <= CONVERT(DATE, @DATETO)                        
AND CONVERT(TIME,RECEIVE_DATE) >=CONVERT(TIME,ISNULL(@BUSINESS_HOURS9,RECEIVE_DATE))              
AND CONVERT(TIME,RECEIVE_DATE) <=CONVERT(TIME,ISNULL(@BUSINESS_HOURS5,RECEIVE_DATE))                        
AND (DATENAME(dw,RECEIVE_DATE)= ISNULL(@SATURDAYS,DATENAME(dw, RECEIVE_DATE)) OR DATENAME(dw,RECEIVE_DATE) = ISNULL(@SUNDAYS,DATENAME(dw,RECEIVE_DATE)))                        
 ------------------------------------QURIES-------------------------                        
  -----#TIME_ELAPSED_COMPLETION_TIME                        
IF OBJECT_ID('TEMPDB.DBO.#TOTALTIMEINSECONDS5', 'U') IS NOT NULL DROP TABLE #TOTALTIMEINSECONDS5   
 SELECT (CONVERT(BIGINT,                          
DATEDIFF(MINUTE, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE))))                         
TOTALTIMEINSECONDS,RECEIVE_DATE,COMPLETED_DATE INTO #TOTALTIMEINSECONDS5                         
FROM #WORKQUEUEFILTEREDRECORDS5 WHERE COMPLETED_DATE IS NOT NULL                          
AND ISNULL(DELETED,0)=0                         
AND UPPER(WORK_STATUS)='COMPLETED'                        
                        
                        
SELECT DISTINCT  CONVERT(DATE,W.RECEIVE_DATE) RECEIVE_DATE,E.ROLE_NAME,W.WORK_ID,                        
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,W.ASSIGNED_DATE), CONVERT(DATETIME,W.COMPLETED_DATE )))) as COMPLETEFILESCOUNT ,                        
(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,COMPLETED_DATE)))) TOTALTIMEINSECONDS_TOCOMPLETE                        
 INTO #WORKQUEUE_DATA5 FROM FOX_TBL_WORK_QUEUE W                        
JOIN  FOX_TBL_APPLICATION_USER R ON R.USER_NAME = W.ASSIGNED_TO                         
LEFT JOIN FOX_TBL_ROLE E ON E.ROLE_ID =R.ROLE_ID                        
WHERE COMPLETED_DATE IS NOT NULL                          
AND ISNULL(W.DELETED,0)=0                         
AND UPPER(W.WORK_STATUS)='COMPLETED'                        
     
SELECT * INTO #REFASSIGNDETAILS5 FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS    
WHERE WORK_ID IN (SELECT WORK_ID FROM #WORKQUEUEFILTEREDRECORDS5)      
ORDER BY WORK_ID,CREATED_DATE ASC    
                        
SELECT ASSIGNED_BY_DESIGNATION,WORK_ID,                        
(CONVERT(BIGINT, DATEDIFF(SS,CONVERT(DATETIME,(SELECT TOP 1 ASSIGNED_TIME FROM #REFASSIGNDETAILS5 WHERE WORK_ID=MAIN.WORK_ID AND ASSIGNED_TIME < MAIN.ASSIGNED_TIME                        
ORDER BY ASSIGNED_TIME DESC)), CONVERT(DATETIME,ASSIGNED_TIME)))) TOTALTIMEINSECONDS  INTO #LOGQUEUE_DATA5                        
FROM #REFASSIGNDETAILS5  AS MAIN                        
ORDER BY WORK_ID,CREATED_DATE ASC                        
                        
SELECT  Q.RECEIVE_DATE, A.* INTO #TEMP3DATA5 FROM #LOGQUEUE_DATA5 A                        
JOIN FOX_TBL_WORK_QUEUE Q ON A.WORK_ID = Q.WORK_ID AND UPPER(WORK_STATUS)='COMPLETED' AND ISNULL(DELETED,0)=0                        
                        
                        
SELECT RECEIVE_DATE, ROLE_NAME, SUM(COMPLETEFILESCOUNT) AS TOTAL_TIME,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP2DATA5 FROM (                        
 SELECT * FROM #WORKQUEUE_DATA5 UNION ALL SELECT *,0 TOTALTIMEINSECONDS_TOCOMPLETE FROM #TEMP3DATA5                        
)A                        
GROUP BY A.ROLE_NAME,RECEIVE_DATE                        
                        
 ----------------INDXER ASSIGN TIME---------------------------------                        
 SELECT D.WORK_ID,W.RECEIVE_DATE,D.CREATED_DATE INTO   #INDEX_TIMEDATA5 FROM FOX_TBL_REFERRAL_ASSIGNMENT_DETAILS D JOIN                         
 FOX_TBL_WORK_QUEUE W ON D.WORK_ID = W.WORK_ID                        
 WHERE UPPER(W.WORK_STATUS)='COMPLETED'  AND W.COMPLETED_DATE IS NOT NULL AND ISNULL(W.DELETED,0) =0  AND UPPER(D.ASSIGNED_TO_DESIGNATION) = 'INDEXER'                        
 SELECT WORK_ID,RECEIVE_DATE,MIN(CREATED_DATE) CREATED_DATE INTO   #INDEX_TIME1DATA5 FROM #INDEX_TIMEDATA5                        
GROUP BY WORK_ID,RECEIVE_DATE                        
                        
 SELECT WORK_ID,CONVERT(DATE,RECEIVE_DATE)  RECEIVE_DATE,(CONVERT(BIGINT, DATEDIFF(SS, CONVERT(DATETIME,RECEIVE_DATE), CONVERT(DATETIME,CREATED_DATE)))) INDEXER_ASSIGNMENT_TIME   INTO   #INDEX_TIMEDATA5_FINAL5 FROM #INDEX_TIME1DATA5                      
  
                        
 SELECT RECEIVE_DATE,SUM(INDEXER_ASSIGNMENT_TIME) INDEXER_ASSIGNMENT_TIME  INTO #FINALRECORD5 FROM #INDEX_TIMEDATA5_FINAL5                        
 GROUP BY RECEIVE_DATE                        
                        
IF OBJECT_ID('TEMPDB.DBO.#TEMP5DATAA_SET5') IS NOT NULL           
     DROP TABLE #TEMP5DATAA_SET5          
SELECT DISTINCT  CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('INDEXER') THEN A.TOTAL_TIME ELSE 0 END) AS INDEXER_TOTAL_TIME,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('AGENT') THEN A.TOTAL_TIME ELSE 0 END) AS AGENT_TOTAL_TIME,                        
(CASE WHEN ISNULL(A.ROLE_NAME,'')=UPPER('SUPERVISOR') THEN A.TOTAL_TIME ELSE 0 END) AS SUPERVISOR_TOTAL_TIME                        
 INTO  #TEMP5DATAA_Set5              
 FROM #TEMP2DATA5 A                        
--LEFT JOIN #TEMP2DATA5 B ON CONVERT(DATE,A.RECEIVE_DATE)=CONVERT(DATE,B.RECEIVE_DATE)                         
                     
                    
select                         
CONVERT(DATE,a.RECEIVE_DATE) RECEIVE_DATE,                        
SUM (isnull(a.INDEXER_TOTAL_TIME,0)) INDEXER_TOTAL_TIME,                        
SUM (isnull(a.AGENT_TOTAL_TIME,0)) AGENT_TOTAL_TIME,                        
SUM (isnull(a.SUPERVISOR_TOTAL_TIME,0)) SUPERVISOR_TOTAL_TIME                        
 INTO  #TEMP5DATAA5                        
 FROM #TEMP5DATAA_Set5 A                        
GROUP BY CONVERT(DATE,a.RECEIVE_DATE)                        
ORDER BY 1                        
---------------------------------------------                        
                        
                        
SELECT  CONVERT(DATE,A.RECEIVE_DATE) RECEIVE_DATE,SUM(TOTALTIMEINSECONDS_TOCOMPLETE) TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATAB5 FROM #WORKQUEUE_DATA5 A                  
GROUP BY CONVERT(DATE,A.RECEIVE_DATE)                        
ORDER BY 1                         
                        
SELECT A.*,B.TOTALTIMEINSECONDS_TOCOMPLETE INTO #TEMP5DATA_FINAL5 FROM #TEMP5DATAA5 A                        
LEFT JOIN #TEMP5DATAB5 B ON A.RECEIVE_DATE=B.RECEIVE_DATE                        
ORDER BY 1                        
                        
                        
IF OBJECT_ID('TEMPDB.DBO.#TIME_ELAPSED_COMPLETION_TIME5', 'U') IS NOT NULL DROP TABLE #TIME_ELAPSED_COMPLETION_TIME5                         
SELECT                         
CONVERT(DATE,RECEIVE_DATE)  AS DATE,                        
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 0 AND 15 THEN 1 ELSE 0 END) '0-15',                         
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 16 AND 30 THEN 1 ELSE 0 END) '16-30' ,                        
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 31 AND 45 THEN 1 ELSE 0 END) '31-45' ,                         
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 46 AND 60 THEN 1 ELSE 0 END) '46-60' ,                          
SUM(CASE WHEN  TOTALTIMEINSECONDS BETWEEN 61 AND 120 THEN 1 ELSE 0 END) '1-2 HOUR',                        
SUM(CASE WHEN  TOTALTIMEINSECONDS > 120 THEN 1 ELSE 0 END) '> 2 HOUR',                        
COUNT(*) AS GRANDTOTAL   INTO #TIME_ELAPSED_COMPLETION_TIME5                        
FROM #TOTALTIMEINSECONDS5                         
GROUP BY CONVERT(DATE,RECEIVE_DATE)                         
ORDER BY CONVERT(DATE,RECEIVE_DATE) ASC                         
                        
                    
 SELECT E.RECEIVE_DATE,CONVERT(INT,E.INDEXER_TOTAL_TIME/(T.GRANDTOTAL)) INDEXER_TOTAL_TIME ,                      
CONVERT(INT,E.AGENT_TOTAL_TIME/(T.GRANDTOTAL)) AGENT_TOTAL_TIME ,                      
CONVERT(INT,E.SUPERVISOR_TOTAL_TIME/(T.GRANDTOTAL)) SUPERVISOR_TOTAL_TIME,                      
CONVERT(INT,E.TOTALTIMEINSECONDS_TOCOMPLETE/(T.GRANDTOTAL)) TOTALTIMEINSECONDS_TOCOMPLETE,                      
CONVERT(INT,D.INDEXER_ASSIGNMENT_TIME/(T.GRANDTOTAL)) INDEXER_ASSIGNMENT_TIME,                     
T.[0-15],T.[16-30],T.[31-45],T.[46-60],T.[1-2 HOUR],T.[> 2 HOUR],T.GRANDTOTAL                    
,CASE WHEN  D.INDEXER_ASSIGNMENT_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_ASSIGNMENT_GRANDTOTAL                    
,CASE WHEN E.INDEXER_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_GRANDTOTAL                    
,CASE WHEN E.AGENT_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS AGENT_GRANDTOTAL              
,CASE WHEN E.SUPERVISOR_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS SUPERVISOR_GRANDTOTAL                    
,CASE WHEN E.TOTALTIMEINSECONDS_TOCOMPLETE <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS TOTALTIME_TOCOMPLETE_GRANDTOTAL                    
INTO  #TEMP6DATA5                        
FROM #TEMP5DATA_FINAL5 E                        
 JOIN   #TIME_ELAPSED_COMPLETION_TIME5 T                         
ON E.RECEIVE_DATE = T.DATE                         
LEFT JOIN  #FINALRECORD5 D  ON E.RECEIVE_DATE = D.RECEIVE_DATE                           
                       
                    
SELECT E.*, D.INDEXER_ASSIGNMENT_TIME,                     
T.[0-15],T.[16-30],T.[31-45],T.[46-60],T.[1-2 HOUR],T.[> 2 HOUR],T.GRANDTOTAL         
,CASE WHEN  D.INDEXER_ASSIGNMENT_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_ASSIGNMENT_GRANDTOTAL                    
,CASE WHEN E.INDEXER_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS INDEXER_GRANDTOTAL                    
,CASE WHEN E.AGENT_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS AGENT_GRANDTOTAL                    
,CASE WHEN E.SUPERVISOR_TOTAL_TIME <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS SUPERVISOR_GRANDTOTAL                    
,CASE WHEN E.TOTALTIMEINSECONDS_TOCOMPLETE <> 0 THEN  T.GRANDTOTAL ELSE 0 END AS TOTALTIME_TOCOMPLETE_GRANDTOTAL                    
INTO  #TEMP6DATA_TOTAL5                      
FROM #TEMP5DATA_FINAL5 E                        
 JOIN   #TIME_ELAPSED_COMPLETION_TIME5 T                         
ON E.RECEIVE_DATE = T.DATE                         
LEFT JOIN  #FINALRECORD5 D  ON E.RECEIVE_DATE = D.RECEIVE_DATE                        
                      
                        
                          
 SELECT CONVERT(VARCHAR, RECEIVE_DATE) AS RECEIVE_DATE ,INDEXER_ASSIGNMENT_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,                        
 SUPERVISOR_TOTAL_TIME,[0-15],[16-30],[31-45],[46-60],[1-2 HOUR],[> 2 HOUR],GRANDTOTAL,TOTALTIMEINSECONDS_TOCOMPLETE           
 INTO #TEMP8DATA5 FROM #TEMP6DATA5             
                      
 UNION ALL                      
                       
                       
    SELECT  '1900/01/02'  DATE,                    
 CONVERT(INT, case when SUM(INDEXER_ASSIGNMENT_GRANDTOTAL) <> 0 then                    
 SUM(INDEXER_ASSIGNMENT_TIME)/SUM(INDEXER_ASSIGNMENT_GRANDTOTAL)else 0 end )                    
 AS INDEXER_ASSIGNMENT_TIME,                    
                    
  CONVERT(INT, case when SUM(INDEXER_GRANDTOTAL) <> 0 then                    
 SUM(INDEXER_TOTAL_TIME)/SUM(INDEXER_GRANDTOTAL)else 0 end )                    
 AS INDEXER_TOTAL_TIME,                    
                    
  CONVERT(INT, case when SUM(AGENT_GRANDTOTAL) <> 0 then                    
 SUM(AGENT_TOTAL_TIME)/SUM(AGENT_GRANDTOTAL)else 0 end )                    
 AS AGENT_TOTAL_TIME,                    
                    
  CONVERT(INT, case when SUM(SUPERVISOR_GRANDTOTAL) <> 0 then                    
 SUM(SUPERVISOR_TOTAL_TIME)/SUM(SUPERVISOR_GRANDTOTAL)else 0 end )                    
 AS SUPERVISOR_TOTAL_TIME,                    
                   
                    
 SUM([0-15])  AS  [0-15] ,SUM([16-30])  AS  [16-30]  ,SUM([31-45])  AS  [31-45]  ,SUM([46-60])  AS  [46-60]                       
 ,SUM([1-2 HOUR])  AS [1-2 HOUR] ,SUM([> 2 HOUR])  AS [> 2 HOUR] ,SUM(GRANDTOTAL) AS GRANDTOTAL                     
                    
                      
   ,CONVERT(INT, case when SUM(TOTALTIME_TOCOMPLETE_GRANDTOTAL) <> 0 then                    
 SUM(TOTALTIMEINSECONDS_TOCOMPLETE)/SUM(TOTALTIME_TOCOMPLETE_GRANDTOTAL)else 0 end )                    
 AS TOTALTIMEINSECONDS_TOCOMPLETE                   
                     
 FROM #TEMP6DATA_TOTAL5                       
                      
 UNION ALL                        
 SELECT  '1900/01/01'  DATE,CONVERT(INT, SUM(INDEXER_ASSIGNMENT_TIME)/COUNT(*)) AS INDEXER_ASSIGNMENT_TIME,CONVERT(INT,                         
 SUM(INDEXER_TOTAL_TIME)/COUNT(*)) AS INDEXER_TOTAL_TIME, CONVERT(INT,SUM(AGENT_TOTAL_TIME)/COUNT(*)) AS AGENT_TOTAL_TIME, CONVERT(INT,                         
 SUM(SUPERVISOR_TOTAL_TIME)/COUNT(*)) AS SUPERVISOR_TOTAL_TIME,                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([0-15])) /convert(decimal ,COUNT(*))))  AS  [0-15],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([16-30])) /convert(decimal ,COUNT(*))))  AS  [16-30],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([31-45])) /convert(decimal ,COUNT(*))))  AS  [31-45],                  
 Convert(INT,convert(decimal, convert(decimal ,SUM([46-60])) /convert(decimal ,COUNT(*))))  AS  [46-60],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([1-2 HOUR])) /convert(decimal ,COUNT(*))))  AS [1-2 HOUR],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM([> 2 HOUR])) /convert(decimal ,COUNT(*))))  AS [> 2 HOUR],                        
 Convert(INT,convert(decimal, convert(decimal ,SUM(GRANDTOTAL)) /convert(decimal ,COUNT(*))))  AS GRANDTOTAL,                        
 CONVERT(INT, SUM(TOTALTIMEINSECONDS_TOCOMPLETE)/COUNT(*)) AS TOTALTIMEINSECONDS_TOCOMPLETE                           
  FROM #TEMP6DATA5                       
                       
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION_TBL5') IS NOT NULL           
DROP TABLE #PAGINATION_TBL5          
                       
 SELECT ROW_NUMBER() OVER(ORDER BY t.RECEIVE_DATE desc) AS ROW,                        
 * into #PAGINATION_TBL5 FROM (                        
 SELECT                         
CASE Y.RECEIVE_DATE WHEN '1900/01/01' THEN 'GRAND AVG' WHEN '1900/01/02' THEN 'GRAND TOTAL'  ELSE  Y.RECEIVE_DATE END  RECEIVE_DATE ,[0-15] as ZEROTO_FIFTEEN                        
,[16-30] as SIXTEENTO_THIRTY,[31-45] as THIRTYONETO_FOURTFIVE,[46-60] as FOURTYSIXTO_SIXTY,[1-2 HOUR] as GREATERTHAN_HOUR,[> 2 HOUR] as GREATERTHAN_TWOHOUR,GRANDTOTAL                        
,                        
CONVERT(VARCHAR, (y.INDEXER_ASSIGNMENT_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_ASSIGNMENT_TIME)%60) END 'INDEXER_ASSIGNMENT_TOTAL_TIME',                         
                        
CONVERT(VARCHAR, (y.INDEXER_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%3600/60) END                        
   +':'+                        
 CASE WHEN                        
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.INDEXER_TOTAL_TIME)%60) END 'INDEXER_TOTAL_TIME',                         
                        
CONVERT(VARCHAR, (y.AGENT_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, AGENT_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN      
CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60) ELSE       
CONVERT(VARCHAR,CONVERT(BIGINT, y.AGENT_TOTAL_TIME)%60) END 'AGENT_TOTAL_TIME',                        
                        
CONVERT(VARCHAR, (y.SUPERVISOR_TOTAL_TIME)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.SUPERVISOR_TOTAL_TIME)%60) END 'SUPERVISOR_TOTAL_TIME',                        
                        
CONVERT(VARCHAR, (y.TOTALTIMEINSECONDS_TOCOMPLETE)/3600 ) +':'+                        
CASE WHEN                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) <10 THEN '0' + CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) ELSE                         
CONVERT(VARCHAR, CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%3600/60) END                        
   +':'+                        
   CASE WHEN                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60)<10  THEN '0' + CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60) ELSE                         
CONVERT(VARCHAR,CONVERT(BIGINT, y.TOTALTIMEINSECONDS_TOCOMPLETE)%60) END 'TOTALTIME_INSECONDS_TOCOMPLETE'                        
                        
FROM #TEMP8DATA5  Y LEFT JOIN #FINALRECORD5 U ON Y.RECEIVE_DATE = U.RECEIVE_DATE                        
                        
)t                        
                        
          
          
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION10') IS NOT NULL DROP TABLE  #PAGINATION10          
SELECT *,RECEIVE_DATE AS REC_DATE INTO   #PAGINATION10 FROM #PAGINATION_TBL5          
          
UPDATE #PAGINATION10          
SET REC_DATE = ''          
WHERE REC_DATE LIKE '%[A^Z]%'          
          
          
IF OBJECT_ID('TEMPDB.DBO.#PAGINATION11') IS NOT NULL DROP TABLE  #PAGINATION11              
SELECT DATENAME(dw,CONVERT(DATE,REC_DATE))AS RECEIVE_DAY,* INTO #PAGINATION11          
FROM #PAGINATION10           
            
          
declare @Total_Pages5 BIGINT                        
declare @Total_Records5 BIGINT             
          
          
          
                     
IF(@EXCLUDEWEEKEND=1)                        
BEGIN                         
                        
SET @Total_Pages5  = (Select TOP 1 CAST(CEILING(COUNT(*) OVER()/(CONVERT(INT, @PAGESIZE )*1.0)) AS BIGINT) AS TOTAL_PAGE from #PAGINATION11 WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'   )                             
SET @Total_Records5  = (select Count(*) from #PAGINATION11  WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'  )                       
                        
 select ROW,RECEIVE_DATE,ZEROTO_FIFTEEN,SIXTEENTO_THIRTY,THIRTYONETO_FOURTFIVE,FOURTYSIXTO_SIXTY,GREATERTHAN_HOUR,GREATERTHAN_TWOHOUR,GRANDTOTAL,          
 INDEXER_ASSIGNMENT_TOTAL_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME,TOTALTIME_INSECONDS_TOCOMPLETE,@Total_Pages5 as TOTAL_PAGE,@Total_Records5 as TOTAL_RECORDS                          
 from #PAGINATION11 WHERE RECEIVE_DAY <>'SATURDAY'   AND RECEIVE_DAY <>'SUNDAY'                   
 AND ROW BETWEEN( @PAGEINDEX -1) *  @PAGESIZE  + 1 AND(((  @PAGEINDEX  -1) *   @PAGESIZE + 1) +   @PAGESIZE ) - 1                         
 OR ROW=1                        
  ORDER BY ROW                         
END                        
ELSE                        
BEGIN                        
SET @Total_Pages5  = (Select TOP 1 CAST(CEILING(COUNT(*) OVER()/(CONVERT(INT, @PAGESIZE )*1.0)) AS BIGINT) AS TOTAL_PAGE from #PAGINATION11)        
 SET @Total_Records5  = (select Count(*) from #PAGINATION11)                        
 select ROW,RECEIVE_DATE,ZEROTO_FIFTEEN,SIXTEENTO_THIRTY,THIRTYONETO_FOURTFIVE,FOURTYSIXTO_SIXTY,GREATERTHAN_HOUR,GREATERTHAN_TWOHOUR,GRANDTOTAL,          
 INDEXER_ASSIGNMENT_TOTAL_TIME,INDEXER_TOTAL_TIME,AGENT_TOTAL_TIME,SUPERVISOR_TOTAL_TIME,TOTALTIME_INSECONDS_TOCOMPLETE,@Total_Pages5 as TOTAL_PAGE,@Total_Records5 as TOTAL_RECORDS                         
                        
 from #PAGINATION11 where ROW BETWEEN( @PAGEINDEX -1) *  @PAGESIZE  + 1 AND(((  @PAGEINDEX  -1) *   @PAGESIZE  + 1) +   @PAGESIZE ) - 1                
 OR ROW=1                        
  ORDER BY ROW                          
-------------------Pagination query-----------------------------------------------------------------                        
                        
                        
end                        
END          
END