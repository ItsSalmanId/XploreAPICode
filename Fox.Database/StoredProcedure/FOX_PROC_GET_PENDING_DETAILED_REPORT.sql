IF (OBJECT_ID('FOX_PROC_GET_PENDING_DETAILED_REPORT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PENDING_DETAILED_REPORT  
GO
 -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------                              
-- EXEC [DBO].[FOX_PROC_GET_PENDING_DETAILED_REPORT]   '1011163', '2/10/1921', '2/10/2021', '', '', '', 'ALL', '', 'Pending', 1, 5000, '', 'SURVEYCOMPLETEDDATE', 'DESC'                                                                                                    
 CREATE PROCEDURE [DBO].[FOX_PROC_GET_PENDING_DETAILED_REPORT]                                                              
 (@PRACTICE_CODE   BIGINT,                         
  @DATE_FROM       DATETIME,                                                               
  @DATE_TO         DATETIME,                                                               
  @PROVIDER        VARCHAR(100),                                                               
  @REGION          VARCHAR(100),                                                               
  @STATE           VARCHAR(10),                                                               
  @FORMAT          VARCHAR(10),                                     
  @SURVEYED_BY     VARCHAR(100),                                                               
  @SURVEYED_STATUS VARCHAR(500),                                                               
  @CURRENT_PAGE    INT,                                                               
  @RECORD_PER_PAGE INT,                                                               
  @SEARCH_TEXT     VARCHAR(100),                                                               
  @SORT_BY         VARCHAR(50),                                                               
  @SORT_ORDER      VARCHAR(5)                                                              
 )                                                              
 AS                                        
 --declare                                  
 -- @PRACTICE_CODE   BIGINT = '1011163',                                                               
 -- @DATE_FROM       DATETIME = '2/2/2021',                                                               
 -- @DATE_TO         DATETIME = '2/9/2021',                                                               
 -- @PROVIDER        VARCHAR(100) = '',                                                               
 -- @REGION          VARCHAR(100) = '',                                                               
 -- @STATE           VARCHAR(10) = '',                                                               
 -- @FORMAT          VARCHAR(10) = 'ALL',                                                               
 -- @SURVEYED_BY     VARCHAR(100) = '',                                                               
 -- @SURVEYED_STATUS VARCHAR(500) = 'Completed Survey,Deceased,Callback,Not Answered,Not Interested,Not Enough Services Provided',                                                               
 -- @CURRENT_PAGE    INT = 1,                                                               
 -- @RECORD_PER_PAGE INT = 0,                                                               
 -- @SEARCH_TEXT     VARCHAR(100) = '',                                                               
 -- @SORT_BY         VARCHAR(50) = 'r',                                                               
 -- @SORT_ORDER      VARCHAR(5)  = 'Desc'                                     
                                                         
   BEGIN                                       
  --Declare @IS_SURVEYED varchar(5)                                      
  --if(@SORT_BY <> 'SURVEYCOMPLETEDDATE')                                     
  --begin                                      
  --set @IS_SURVEYED = NULL;                                      
  --end                          
  --else                                      
  --begin                   
  --set @IS_SURVEYED = '';                                      
  --end                                         
                                                                  
    SET @CURRENT_PAGE = @CURRENT_PAGE - 1                                      
    DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORD_PER_PAGE                                                 
    DECLARE @TOATL_PAGESUDM FLOAT                                                              
                  
    --                                                              
    --DECLARE @IS_NOT_SURVEYED BIT                                                              
    --IF @SURVEYED_STATUS = 'Pending'                                                              
    --    BEGIN                             
    --        SET @IS_NOT_SURVEYED = 0                                                              
    --    END                                                              
    --    ELSE                                             
    --    BEGIN                                                              
    --        SET @IS_NOT_SURVEYED = 1                                                              
    --    END                                                              
                                                  
 --if(@SURVEYED_STATUS = 'Pending')                                
 --begin                                
 --set @DATE_FROM = null                                
 --set @DATE_TO = null                                
 --end                                              
    --                                                              
    SELECT @TOATL_PAGESUDM = COUNT(*)                                                              
     FROM FOX_TBL_PATIENT_SURVEY PS                                                              
       LEFT JOIN FOX_TBL_APPLICATION_USER AU ON AU.USER_NAME = PS.MODIFIED_BY                                                              
     WHERE ISNULL(PS.DELETED, 0) = 0                                                              
        AND PS.PRACTICE_CODE = @PRACTICE_CODE                                           
        AND PS.REGION <> ''                                           
     --AND (@IS_SURVEYED IS NULL OR ISNULL(SURVEY_COMPLETED_DATE, '') <> @IS_SURVEYED)                                      
     AND (@SURVEYED_STATUS = 'Pending'                                                            
     OR PS.SURVEY_FORMAT_TYPE LIKE                                              
     CASE                                                             
     WHEN @FORMAT = 'ALL'                                                            
     THEN '%Format%'                                                            
     ELSE @FORMAT                                                            
     END                                                            
     )                                                 
        AND PS.REGION LIKE '%'+@REGION+'%'                                                              
        AND PS.PROVIDER LIKE '%'+@PROVIDER+'%'                                                              
        AND PS.PATIENT_STATE LIKE '%'+@STATE+'%'                                                              
        AND AU.USER_NAME LIKE '%'+@SURVEYED_BY+'%'                          
        AND (                                                              
     (                                                              
   PS.SURVEY_STATUS_CHILD IN                                                              
    (                                                              
     SELECT Item                                                              
     FROM dbo.SplitStrings_CTE(@SURVEYED_STATUS, N',')                                                              
    )                                                              
    AND @SURVEYED_STATUS <> 'Pending'                                                              
   ----AND ISNULL(PS.IS_SURVEYED, 0) = @IS_NOT_SURVEYED                                                              
    )                                                              
     OR (@SURVEYED_STATUS = 'Pending' AND ISNULL(PS.IS_SURVEYED, 0) = 0)                                                              
   --  --OR ISNULL(PS.IS_SURVEYED, 0) = @IS_NOT_SURVEYED                                                              
   )                                                              
       AND (@DATE_FROM IS NULL                
         OR @DATE_TO IS NULL                                                              
         OR CONVERT(DATE, PS.CREATED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO))                                                              
       AND (PS.PATIENT_ACCOUNT_NUMBER LIKE @SEARCH_TEXT+'%'                                                              
         OR PS.PATIENT_LAST_NAME LIKE @SEARCH_TEXT+'%'                                                          
         OR PS.PATIENT_FIRST_NAME LIKE @SEARCH_TEXT+'%'                                                              
         OR PS.PATIENT_STATE LIKE @SEARCH_TEXT+'%'                              
         OR PS.PT_OT_SLP LIKE @SEARCH_TEXT+'%'                                                              
         OR PS.REGION LIKE '%' + @SEARCH_TEXT+'%'                       
  OR PS.PROVIDER LIKE '%' + @SEARCH_TEXT+'%'                                                              
         OR PS.SURVEY_STATUS_CHILD LIKE @SEARCH_TEXT+'%'                             
   OR PS.SURVEY_STATUS_BASE LIKE @SEARCH_TEXT+'%'                                                   
   OR PS.FEEDBACK LIKE '%' +@SEARCH_TEXT+'%'                       
      OR CONVERT(VARCHAR, PS.MODIFIED_DATE, 101) LIKE  '%' + @SEARCH_TEXT+'%'                           
      OR CONVERT(VARCHAR, PS.MODIFIED_DATE, 100) LIKE  '%' + @SEARCH_TEXT+'%'                                                             
         OR AU.FIRST_NAME LIKE @SEARCH_TEXT+'%'                                                              
         OR AU.LAST_NAME LIKE @SEARCH_TEXT+'%'                                                                                     
         OR PS.ATTENDING_DOCTOR_NAME LIKE  '%' + @SEARCH_TEXT+'%')                                                              
                                                                      
                                                              
    --                                                              
    IF(@RECORD_PER_PAGE = 0)                                                              
     BEGIN                                                     
      SET @RECORD_PER_PAGE = @TOATL_PAGESUDM                                                              
     END                                                              
     ELSE                                                              
     BEGIN                              
      SET @RECORD_PER_PAGE = @RECORD_PER_PAGE                                                              
     END                                                              
                                                              
    --                                                             
    IF(@RECORD_PER_PAGE <> 0)                                    BEGIN                                                            
    DECLARE @TOTAL_RECORDS INT= @TOATL_PAGESUDM                                                              
    SET @TOATL_PAGESUDM = CEILING(@TOATL_PAGESUDM / @RECORD_PER_PAGE)                             
                                                              
    --                                                              
 SELECT *,                                                               
     @TOATL_PAGESUDM AS TOTAL_RECORD_PAGES,                                       
     @TOTAL_RECORDS TOTAL_RECORDS                                                              
    FROM                                                              
    (                                                              
     SELECT PS.IS_SURVEYED,                                                               
      PS.PATIENT_ACCOUNT_NUMBER,                                                               
      PS.PATIENT_FIRST_NAME,                                                               
   PS.PATIENT_MIDDLE_INITIAL,                                                               
      PS.PATIENT_LAST_NAME,                                                               
      PS.PATIENT_STATE,                                            
      PS.PT_OT_SLP,                                                               
      PS.REGION,                   
      PS.PROVIDER,                                                          
   PS.ATTENDING_DOCTOR_NAME,                                        
   PS.FEEDBACK,                                                              
      CASE                                                              
       WHEN ISNULL(PS.SURVEY_STATUS_CHILD, '') = ''                                                              
       THEN 'Pending'                                                              
       ELSE PS.SURVEY_STATUS_CHILD                                                              
      END AS SURVEY_STATUS_CHILD,                  
      PS.SURVEY_STATUS_BASE AS SURVEY_STATUS_BASE,                                                               
      AU.FIRST_NAME AS SURVEYED_BY_FNAME,                                                               
      AU.LAST_NAME AS SURVEYED_BY_LNAME,                                                               
      PS.MODIFIED_DATE,                                        
      PS.SURVEY_COMPLETED_DATE,                   
     CONVERT(VARCHAR(10), CAST( PS.SURVEY_COMPLETED_DATE AS TIME), 0) AS SURVEY_COMPLETED_TIME_STR,                
  CONVERT(VARCHAR(15), CAST( PS.SURVEY_COMPLETED_DATE AS DATE), 0) AS SURVEY_COMPLETED_DATE_STR,                
      PS.Created_Date,                                                               
      CONVERT(VARCHAR, PS.MODIFIED_DATE) AS Modified_Date_Str,                                                               
      PS.SURVEY_FLAG,                                                               
      PS.IS_CONTACT_HQ,                                                              
      CASE                                                              
       WHEN PS.IS_CONTACT_HQ = 0                                                              
       THEN 'NO'                                                              
       WHEN PS.IS_CONTACT_HQ = 1                                                              
       THEN 'YES'                                                              
      END AS Is_Contact_HQ_Str,                                                               
      PS.IS_RESPONSED_BY_HQ,                                                              
      CASE                                                              
       WHEN PS.IS_RESPONSED_BY_HQ = 0                                                              
       THEN 'NO'                                                              
       WHEN PS.IS_RESPONSED_BY_HQ = 1                                                              
       THEN 'YES'                                                              
      END AS Is_Responsed_By_HQ_Str,                                                               
      PS.IS_QUESTION_ANSWERED,                                                              
 CASE                                                              
       WHEN PS.IS_QUESTION_ANSWERED = 0                                                              
       THEN 'NO'                                                              
       WHEN PS.IS_QUESTION_ANSWERED = 1                                                              
       THEN 'YES'                                                   
      END AS Is_Questioned_Answered_Str,                                                               
      PS.IS_REFERABLE,                                                  
      CASE                                                              
       WHEN PS.IS_REFERABLE = 0                                                              
       THEN 'NO'                                                 
       WHEN PS.IS_REFERABLE = 1                                                              
       THEN 'YES'                                                              
      END AS Is_Referrable_Str,                                                         
      PS.IS_IMPROVED_SETISFACTION,                                    
      CASE                                                              
       WHEN PS.IS_IMPROVED_SETISFACTION = 0                                                              
       THEN 'NO'                                                              
       WHEN PS.IS_IMPROVED_SETISFACTION = 1                                 
       THEN 'YES'                                     
      END AS Is_improved_Satisfaction_Str,                                            
     PS.IS_EXCEPTIONAL,                                            
                                                 
   CASE                                                              
       WHEN PS.IS_EXCEPTIONAL = 0                             
       THEN 'NO'                                                              
       WHEN PS.IS_EXCEPTIONAL = 1                                                              
       THEN 'YES'                                                              
      END AS Is_exceptional_Str,                                            
                                            
   PS.IS_PROTECTIVE_EQUIPMENT,                    
   CASE                                                
       WHEN PS.IS_PROTECTIVE_EQUIPMENT = 0                                                              
       THEN 'NO'                                                              
       WHEN PS.IS_PROTECTIVE_EQUIPMENT = 1                                                              
       THEN 'YES'                                                              
      END AS Is_protective_equipment_Str,                              
                                                                  
      ROW_NUMBER() OVER(ORDER BY PS.MODIFIED_DATE DESC) AS ACTIVEROW                                                    
                                                            
     FROM FOX_TBL_PATIENT_SURVEY PS                                                              
       LEFT JOIN FOX_TBL_APPLICATION_USER AU ON AU.USER_NAME = PS.MODIFIED_BY                                                              
     WHERE ISNULL(PS.DELETED, 0) = 0                                                              
    AND PS.PRACTICE_CODE = @PRACTICE_CODE                                           
   AND PS.REGION <> ''                                                         
    --AND ((@IS_SURVEYED IS NULL AND ISNULL(SURVEY_COMPLETED_DATE, '') = '') OR ISNULL(SURVEY_COMPLETED_DATE, '') <> @IS_SURVEYED)                                      
        AND (@SURVEYED_STATUS = 'Pending'                                                            
    OR PS.SURVEY_FORMAT_TYPE LIKE                                    
     CASE                                                             
     WHEN @FORMAT = 'ALL'                                                            
     THEN '%Format%'                                                            
     ELSE @FORMAT                                                            
     END                                                           
     )                                           
        AND PS.REGION LIKE '%'+@REGION+'%'                                                              
    AND PS.PROVIDER LIKE '%'+@PROVIDER+'%'                                                              
        AND PS.PATIENT_STATE LIKE '%'+@STATE+'%'                                                              
        AND AU.USER_NAME LIKE '%'+@SURVEYED_BY+'%'                         
        AND (                                                              
     (                                                              
    PS.SURVEY_STATUS_CHILD IN                                               
    (                                                              
     SELECT Item                                                              
     FROM dbo.SplitStrings_CTE(@SURVEYED_STATUS, N',')                                                         
    )                                                              
    AND @SURVEYED_STATUS <> 'Pending'                      
   --AND ISNULL(PS.IS_SURVEYED, 0) = @IS_NOT_SURVEYED                                                              
     )                                                              
     OR (@SURVEYED_STATUS = 'Pending' AND ISNULL(PS.IS_SURVEYED, 0) = 0)                                                              
     --OR ISNULL(PS.IS_SURVEYED, 0) = @IS_NOT_SURVEYED                   
    )                                                              
   AND (@DATE_FROM IS NULL                                                              
         OR @DATE_TO IS NULL                                                              
         OR CONVERT(DATE, PS.CREATED_DATE) BETWEEN CONVERT(DATE, @DATE_FROM) AND CONVERT(DATE, @DATE_TO))                                                              
     AND (PS.PATIENT_ACCOUNT_NUMBER LIKE @SEARCH_TEXT+'%'                                                              
   OR PS.PATIENT_LAST_NAME LIKE @SEARCH_TEXT+'%'                                                              
 OR PS.PATIENT_FIRST_NAME LIKE @SEARCH_TEXT+'%'                                                              
    OR PS.PATIENT_STATE LIKE @SEARCH_TEXT+'%'                                                              
         OR PS.PT_OT_SLP LIKE @SEARCH_TEXT+'%'                                                              
         OR PS.REGION LIKE '%' + @SEARCH_TEXT+'%'                                                              
         OR PS.PROVIDER LIKE '%' + @SEARCH_TEXT+'%'                                                          
   OR PS.ATTENDING_DOCTOR_NAME LIKE '%' + @SEARCH_TEXT+'%'                                    
         OR PS.SURVEY_STATUS_CHILD LIKE @SEARCH_TEXT+'%'                                                             
   OR PS.SURVEY_STATUS_BASE LIKE @SEARCH_TEXT+'%'                                                            
   OR PS.FEEDBACK LIKE '%' + @SEARCH_TEXT+'%'                       
   OR CONVERT(VARCHAR, PS.MODIFIED_DATE, 101) LIKE  '%' + @SEARCH_TEXT+'%'                       
      OR CONVERT(VARCHAR, PS.MODIFIED_DATE, 100) LIKE  '%' + @SEARCH_TEXT+'%'                                                                                               
         OR AU.FIRST_NAME LIKE @SEARCH_TEXT+'%'                                                              
         OR AU.LAST_NAME LIKE @SEARCH_TEXT+'%')                                                              
    ) AS SURVEY                                                              
    ORDER BY CASE                                                              
        WHEN @SORT_BY = 'PATIENTACCOUNTNUMBER'                                                              
          AND @SORT_ORDER = 'ASC'                                                        
        THEN PATIENT_ACCOUNT_NUMBER                                                                     END ASC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'PATIENTACCOUNTNUMBER'                                                              
    AND @SORT_ORDER = 'DESC'                                                              
        THEN PATIENT_ACCOUNT_NUMBER                                                              
       END DESC,                                                    
       CASE                                                              
        WHEN @SORT_BY = 'PATIENTNAME'                                                              
          AND @SORT_ORDER = 'ASC'                         
  THEN PATIENT_LAST_NAME                                                              
       END ASC,                                                         
       CASE                                                              
        WHEN @SORT_BY = 'PATIENTNAME'                                                              
          AND @SORT_ORDER = 'DESC'                                                              
        THEN PATIENT_LAST_NAME                                                
       END DESC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'STATE'                                                              
          AND @SORT_ORDER = 'ASC'                                                              
        THEN PATIENT_STATE                                                              
       END ASC,                                                             
     CASE                                                              
        WHEN @SORT_BY = 'STATE'                                                 
          AND @SORT_ORDER = 'DESC'                                                              
        THEN PATIENT_STATE                                 
       END DESC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'PTOTST'                                                              
          AND @SORT_ORDER = 'ASC'                                                              
        THEN PT_OT_SLP                                                              
       END ASC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'PTOTST'                                                              
          AND @SORT_ORDER = 'DESC'                                                              
        THEN PT_OT_SLP                                                              
       END DESC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'REGION'                  
          AND @SORT_ORDER = 'ASC'                                                       
        THEN REGION                                                              
       END ASC,                                                              
       CASE                                                      
        WHEN @SORT_BY = 'REGION'                                                              
          AND @SORT_ORDER = 'DESC'                                                              
        THEN REGION                                                              
  END DESC,                                                              
       CASE                                          
   WHEN @SORT_BY = 'PROVIDER'                                                              
          AND @SORT_ORDER = 'ASC'         
        THEN PROVIDER                                                              
       END ASC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'PROVIDER'                                                              
          AND @SORT_ORDER = 'DESC'                                                              
        THEN PROVIDER                                                              
       END DESC,                                                        
    CASE                                                              
        WHEN @SORT_BY = 'ATTENDING_DOCTOR_NAME'                                                              
          AND @SORT_ORDER = 'ASC'                                           
        THEN ATTENDING_DOCTOR_NAME                                                              
       END ASC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'ATTENDING_DOCTOR_NAME'                                                              
          AND @SORT_ORDER = 'DESC'                                                              
        THEN ATTENDING_DOCTOR_NAME         
 END DESC,                                                                    
       CASE                                                              
        WHEN @SORT_BY = 'surveyStatusBase'                                                              
          AND @SORT_ORDER = 'ASC'                                                              
        THEN SURVEY_STATUS_BASE                                                         
       END ASC,                                                              
       CASE                             
        WHEN @SORT_BY = 'surveyStatusBase'                                            
          AND @SORT_ORDER = 'DESC'                                                              
        THEN SURVEY_STATUS_BASE                                                              
       END DESC,                                        
       CASE                                                              
        WHEN @SORT_BY = 'SURVEYSTATUS'                                                              
       AND @SORT_ORDER = 'ASC'                                                              
     THEN SURVEY_STATUS_CHILD                                                              
       END ASC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'SURVEYSTATUS'                                                              
          AND @SORT_ORDER = 'DESC'                                                         
        THEN SURVEY_STATUS_CHILD                                                              
       END DESC,                                                              
       CASE                                                              
     WHEN @SORT_BY = 'SURVEYDATE'                             
          AND @SORT_ORDER = 'ASC'                                                              
 THEN MODIFIED_DATE                                                              
      END ASC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'SURVEYDATE'                                                              
AND @SORT_ORDER = 'DESC'                                                              
        THEN MODIFIED_DATE                                                              
       END DESC,                                            
    CASE                                          
     WHEN @SORT_BY = 'CREATEDDATE'                                      
          AND @SORT_ORDER = 'ASC'                                                              
        THEN CREATED_DATE                                                              
   END ASC,                                                              
     CASE                                                              
        WHEN @SORT_BY = 'CREATEDDATE'                                                 
          AND @SORT_ORDER = 'DESC'                                                              
        THEN CREATED_DATE                                                              
       END DESC,                                             
    CASE                                          
     WHEN @SORT_BY = 'SURVEYCOMPLETEDDATE'                                                              
          AND @SORT_ORDER = 'ASC'                                          
        THEN SURVEY_COMPLETED_DATE_STR                                                              
   END ASC,                                                              
    CASE                                                              
        WHEN @SORT_BY = 'SURVEYCOMPLETEDDATE'                                                              
          AND @SORT_ORDER = 'DESC'                                                              
        THEN SURVEY_COMPLETED_DATE_STR                                                              
       END DESC,                
    CASE                                  
     WHEN @SORT_BY = 'SURVEYCOMPLETEDTIME'                                                              
          AND @SORT_ORDER = 'ASC'                                                              
        THEN SURVEY_COMPLETED_TIME_STR                                                              
   END ASC,                                                              
    CASE                                                              
        WHEN @SORT_BY = 'SURVEYCOMPLETEDTIME'                                                              
          AND @SORT_ORDER = 'DESC'                                                              
        THEN SURVEY_COMPLETED_TIME_STR                                                              
       END DESC,                
     CASE                                        
        WHEN @SORT_BY = 'SURVEYBY'                                                              
          AND @SORT_ORDER = 'ASC'                                                              
        THEN SURVEYED_BY_LNAME                                                              
       END ASC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'SURVEYBY'                                                              
          AND @SORT_ORDER = 'DESC'                                                              
        THEN SURVEYED_BY_LNAME                                                              
       END DESC,                                                              
  CASE                                                              
        WHEN @SORT_BY = 'setisfactionImproved'                                                              
          AND @SORT_ORDER = 'ASC'                                                              
        THEN IS_IMPROVED_SETISFACTION                                                              
       END ASC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'setisfactionImproved'                                                              
          AND @SORT_ORDER = 'DESC'                                                        
        THEN IS_IMPROVED_SETISFACTION                                                              
       END DESC,                                                           
       CASE                                                              
        WHEN @SORT_BY = 'referable'                                                           
          AND @SORT_ORDER = 'ASC'                                
        THEN IS_REFERABLE                                                              
       END ASC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'referable'                                                              
          AND @SORT_ORDER = 'DESC'                                                              
        THEN IS_REFERABLE                                                              
       END DESC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'contactHQ'                                                              
          AND @SORT_ORDER = 'ASC'                                                              
        THEN IS_CONTACT_HQ                                      
       END ASC,                                                              
 CASE                                                              
        WHEN @SORT_BY = 'contactHQ'                                                              
          AND @SORT_ORDER = 'DESC'                                                              
  THEN IS_CONTACT_HQ                                                              
       END DESC,       
       CASE                                                              
        WHEN @SORT_BY = 'reponsedHQ'                                                              
          AND @SORT_ORDER = 'ASC'                                                          
        THEN IS_RESPONSED_BY_HQ                                                              
       END ASC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'reponsedHQ'                                                              
          AND @SORT_ORDER = 'DESC'                                                              
        THEN IS_RESPONSED_BY_HQ                                                              
       END DESC,                                                             
       CASE                                                              
       WHEN @SORT_BY = 'questionAnswered'                                                              
          AND @SORT_ORDER = 'ASC'                                                              
        THEN IS_QUESTION_ANSWERED                                                              
       END ASC,                             
       CASE                                                             
        WHEN @SORT_BY = 'questionAnswered'                                                  
          AND @SORT_ORDER = 'DESC'                            
        THEN IS_QUESTION_ANSWERED                                                              
       END DESC,                                                
                                                      
     CASE                                                              
        WHEN @SORT_BY = 'ProTective Equipment'                                                              
          AND @SORT_ORDER = 'ASC'                                                              
        THEN  IS_PROTECTIVE_EQUIPMENT                                                             
       END ASC,                                                              
       CASE              
        WHEN @SORT_BY = 'ProTective Equipment'                                                              
AND @SORT_ORDER = 'DESC'                                                              
        THEN IS_PROTECTIVE_EQUIPMENT                                                              
       END DESC,                                                                 
   CASE                                                              
        WHEN @SORT_BY = 'Feedback'                                                              
          AND @SORT_ORDER = 'ASC'                                                              
        THEN FEEDBACK                                                              
       END ASC,                                                              
       CASE                                                              
        WHEN @SORT_BY = 'Feedback'                                     
          AND @SORT_ORDER = 'DESC'                                                              
        THEN IS_PROTECTIVE_EQUIPMENT                                                              
       END DESC          
                                                      
                                                                         
    OFFSET @START_FROM ROWS FETCH NEXT @RECORD_PER_PAGE ROWS ONLY                                                              
    END                                                            
   END 