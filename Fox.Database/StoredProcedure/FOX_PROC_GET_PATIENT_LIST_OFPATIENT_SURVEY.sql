IF (OBJECT_ID('FOX_PROC_GET_PATIENT_LIST_OFPATIENT_SURVEY') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_PATIENT_LIST_OFPATIENT_SURVEY  
GO            
--[DBO].[FOX_PROC_GET_PATIENT_LIST_OFPATIENT_SURVEY] 1012714, '2045', 1            
CREATE PROCEDURE [DBO].[FOX_PROC_GET_PATIENT_LIST_OFPATIENT_SURVEY] --1012714, '12109', 1            
 @PRACTICE_CODE BIGINT                      
 ,@PATIENT_ACCOUNT VARCHAR(100)                      
 ,@IS_INCLUDE_SURVEYED BIT                      
                      
AS                      
BEGIN            
        
--DECLARE  @PRACTICE_CODE BIGINT    = 1012714                    
-- ,@PATIENT_ACCOUNT VARCHAR(100)   = '12107'                
--,@IS_INCLUDE_SURVEYED BIT     = 0        
 DECLARE @DOB DATETIME                      
                      
 IF ISDATE(@PATIENT_ACCOUNT) = 1                      
 BEGIN                      
  SET @DOB = CONVERT(DATETIME, @PATIENT_ACCOUNT)                      
 END                      
 ELSE                      
 BEGIN                      
  SET @DOB = NULL                      
 END                      
                      
 SELECT DISTINCT PATIENT_ACCOUNT_NUMBER                      
  ,PATIENT_FIRST_NAME                      
  ,PATIENT_LAST_NAME                      
  ,PATIENT_MIDDLE_INITIAL                      
  ,PATIENT_CITY                      
  ,PATIENT_STATE                      
  ,PATIENT_ZIP_CODE                      
  ,PATIENT_GENDER                    
  ,SURVEY_STATUS_CHILD                
 --,SURVEY_ID                  
  ,PS.IS_SURVEYED           
  ,ISNULL(SL.IS_PERFORMED_SURVEY, 0) AS IS_PERFORMED_SURVEY      
  ,CONVERT(INT, ROUND(DATEDIFF(hour, PATIENT_DATE_OF_BIRTH, GETDATE()) / 8766.0, 0)) AS PATIENT_AGE                      
  ,PATIENT_DATE_OF_BIRTH,          
  CASE          
 WHEN           
   (SL.IS_SMS = 1 OR SL.IS_EMAIL = 1) AND DATEDIFF(DAY, SL.MODIFIED_DATE, GETDATE()) < 15 THEN 'AUTOMATED SURVEY'          
   ELSE 'MANUAL SURVEY'          
 END AS SurveyCategory,           
 CASE          
   WHEN COALESCE(SL.IS_EMAIL, 0) = 1 AND COALESCE(SL.IS_SMS, 0) = 1 THEN 'SMS and Email'          
   WHEN COALESCE(SL.IS_SMS, 0) = 1 THEN 'SMS'          
   WHEN COALESCE(SL.IS_EMAIL, 0) = 1 THEN 'Email'           
   ELSE ''          
END AS SurveyMethod          
,          
CONVERT(VARCHAR, SL.MODIFIED_DATE, 120) AS SurveyTime    
,PS.SERVICE_OR_PAYMENT_DESCRIPTION    
 FROM FOX_TBL_PATIENT_SURVEY  PS WITH (NOLOCK)         
 left join FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG SL WITH (NOLOCK)on  SL.PATIENT_ACCOUNT = '00'+ PS.PATIENT_ACCOUNT_NUMBER and  SL.SURVEY_ID = PS.SURVEY_ID     
 WHERE ISNULL(PS.DELETED, 0) = 0                      
  AND ISNULL(IN_PROGRESS, 0) = 0                      
  AND PS.PRACTICE_CODE = @PRACTICE_CODE                      
  AND                      
  (                      
   @IS_INCLUDE_SURVEYED = 1                      
   OR ISNULL(IS_SURVEYED, 0) = @IS_INCLUDE_SURVEYED                      
  )                      
  AND @PATIENT_ACCOUNT IS NOT NULL                      
  AND                      
  (                      
   CAST(PATIENT_ACCOUNT_NUMBER AS VARCHAR(30)) LIKE '%' + @PATIENT_ACCOUNT + '%'                      
   OR PATIENT_FIRST_NAME LIKE '%' + @PATIENT_ACCOUNT + '%'                      
   OR PATIENT_LAST_NAME LIKE '%' + @PATIENT_ACCOUNT + '%'                      
   OR                      
   (                      
    @DOB IS NOT NULL                      
    AND PATIENT_DATE_OF_BIRTH = @DOB                      
   )                      
  )                      
  AND PATIENT_ACCOUNT_NUMBER IS NOT NULL                       
END  