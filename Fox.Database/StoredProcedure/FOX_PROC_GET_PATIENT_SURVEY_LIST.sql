-- =============================================                                                          
-- AUTHOR:  <AUTHOR,MUHAMMAD SALMAN>                                                          
-- CREATE DATE: <CREATE DATE,12/10/2023>           
-- MODIFIED BY: MUHAMMAD SALMAN                                                     
-- DESCRIPTION: <GET PATIENT SURVEY LIST>                         
-- exec FOX_PROC_GET_PATIENT_SURVEY_LIST_REVERT  1012714, 76718, 0                 
--EXEC [DBO].[FOX_PROC_GET_PATIENT_SURVEY_LIST_REVERT] 1012714, 76718, 0              
CREATE PROCEDURE [DBO].[FOX_PROC_GET_PATIENT_SURVEY_LIST] --1012714, 12107, 0                                
 @PRACTICE_CODE BIGINT                                
 ,@PATIENT_ACCOUNT BIGINT                                
 ,@IS_SURVEYED INT                                
AS                                
 --declare                                                                                                      
 --@PRACTICE_CODE BIGINT   = 1012714                              
 --,@PATIENT_ACCOUNT BIGINT  = 187021                              
 --,@IS_SURVEYED INT     = 1                              
BEGIN                                
 DECLARE @SURVEY_STATUS BIT;                                 
 IF ( @IS_SURVEYED = 1)                                
        BEGIN                                 
            SET @SURVEY_STATUS = 1;                                
        END                                
 ELSE                                
  BEGIN                                 
            SET @SURVEY_STATUS = 0;                                
        END                                   
 SELECT DISTINCT PS.PATIENT_ACCOUNT_NUMBER,ISNULL(PS.PATIENT_MIDDLE_INITIAL,'') AS PATIENT_MIDDLE_INITIAL, CONVERT(INT,ROUND(DATEDIFF(HOUR,PS.PATIENT_DATE_OF_BIRTH,GETDATE())/8766.0,0)) AS PATIENT_AGE,                            
      CONVERT(INT,ROUND(DATEDIFF(HOUR,PS.RESPONSIBLE_PARTY_DATE_OF_BIRTH,GETDATE())/8766.0,0)) AS RESPONSIBLE_PARTY_AGE,             
PS.SURVEY_ID            
,PS.PRACTICE_CODE            
,PS.FACILITY_OR_CLIENT_ID            
,PS.PATIENT_ACCOUNT_NUMBER            
,PS.RESPONSIBLE_PARTY_LAST_NAME            
,PS.RESPONSIBLE_PARTY_FIRST_NAME            
,PS.RESPONSIBLE_PARTY_MIDDLE_INITIAL            
,PS.RESPONSIBLE_PARTY_ADDRESS            
,PS.RESPONSIBLE_PARTY_CITY            
,PS.RESPONSIBLE_PARTY_STATE            
,PS.RESPONSIBLE_PARTY_ZIP_CODE            
,PS.RESPONSIBLE_PARTY_TELEPHONE            
,PS.RESPONSIBLE_PARTY_SSN            
,PS.RESPONSIBLE_PARTY_SEX            
,PS.RESPONSIBLE_PARTY_DATE_OF_BIRTH            
,PS.PATIENT_LAST_NAME            
,PS.PATIENT_FIRST_NAME            
,PS.PATIENT_MIDDLE_INITIAL            
,PS.PATIENT_ADDRESS            
,PS.PATIENT_CITY            
,PS.PATIENT_STATE            
,PS.PATIENT_ZIP_CODE            
,PS.PATIENT_TELEPHONE_NUMBER            
,PS.PATIENT_SOCIAL_SECURITY_NUMBER            
,PS.PATIENT_GENDER            
,PS.PATIENT_DATE_OF_BIRTH            
,PS.ALTERNATE_CONTACT_LAST_NAME            
,PS.ALTERNATE_CONTACT_FIRST_NAME            
,PS.ALTERNATE_CONTACT_MIDDLE_INITIAL            
,PS.ALTERNATE_CONTACT_TELEPHONE            
,PS.EMR_LOCATION_CODE            
,PS.EMR_LOCATION_DESCRIPTION            
,PS.SERVICE_OR_PAYMENT_DESCRIPTION            
,PS.PROVIDER            
,PS.REGION            
,PS.LAST_VISIT_DATE            
,PS.DISCHARGE_DATE            
,PS.ATTENDING_DOCTOR_NAME            
,PS.PT_OT_SLP            
,PS.REFERRAL_DATE            
,PS.PROCEDURE_OR_TRAN_CODE            
,PS.SERVICE_OR_PAYMENT_AMOUNT            
,PS.IS_CONTACT_HQ            
,PS.IS_RESPONSED_BY_HQ            
,PS.IS_REFERABLE            
,PS.IS_IMPROVED_SETISFACTION            
,PS.FEEDBACK            
,PS.SURVEY_STATUS_BASE            
,PS.CREATED_BY            
,PS.CREATED_DATE            
,PS.MODIFIED_BY            
,PS.MODIFIED_DATE            
,PS.IS_SURVEYED            
,PS.FILE_NAME            
,PS.SHEET_NAME            
,PS.TOTAL_RECORD_IN_FILE            
,PS.DELETED            
,PS.SURVEY_STATUS_CHILD            
,PS.IN_PROGRESS            
,PS.SURVEY_FLAG            
,PS.IS_QUESTION_ANSWERED            
,PS.SURVEY_FORMAT_TYPE            
,PS.IS_EXCEPTIONAL            
,PS.IS_PROTECTIVE_EQUIPMENT            
,PS.Survey_Completed_Date,                    
PS.NOT_ANSWERED_REASON,                     
  CASE                                                                                                   
       WHEN SL.IS_EMAIL = 1                                                                                                                         
       THEN 'Email'                                 
        WHEN SL.IS_SMS = 1                                                                                                                         
       THEN 'SMS'                                 
       Else                                
      AU.FIRST_NAME                                                                                                  
      END AS SURVEYED_BY_FNAME ,                                   
                                       
    CASE                                                                                              
       WHEN SL.IS_EMAIL = 1                                                                                                                           
       THEN ''                                 
       WHEN SL.IS_SMS = 1                                                                                                                         
       THEN ''                                
       Else                                
      AU.LAST_NAME + ','                                                                                                                                                                           
      END AS SURVEYED_BY_LNAME,                                                          
      ISNULL(PS.SURVEY_STATUS_CHILD, '') AS SURVEY_STATUS_CHILD,                            
      PS.SURVEY_FORMAT_TYPE,                              
 AD1.LAST_DIALED_TYPE,                                        
   AD2.PATIENT_CELL_NUMBER,                      
   AD2.PATIENT_WORK_NUMBER ,    
 CASE    
   WHEN COALESCE(SL.IS_EMAIL, 0) = 1 AND COALESCE(SL.IS_SMS, 0) = 1 THEN 'SMS and Email'    
   WHEN COALESCE(SL.IS_SMS, 0) = 1 THEN 'SMS'    
   WHEN COALESCE(SL.IS_EMAIL, 0) = 1 THEN 'Email'     
   ELSE ''    
END AS SurveyMethod,    
 CASE    
 WHEN     
   (SL.IS_SMS = 1 OR SL.IS_EMAIL = 1) AND DATEDIFF(DAY, SL.MODIFIED_DATE, GETDATE()) < 15 THEN 'AUTOMATED SURVEY'    
   ELSE 'MANUAL SURVEY'    
 END AS SurveyCategory   
 ,ISNULL(SL.IS_PERFORMED_SURVEY, 0) AS IS_PERFORMED_SURVEY  
                                
                            
 FROM FOX_TBL_PATIENT_SURVEY PS WITH (NOLOCK)                           
   LEFT JOIN FOX_TBL_APPLICATION_USER AU WITH (NOLOCK) ON AU.USER_NAME = PS.MODIFIED_BY                           
   LEFT JOIN FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG SL WITH (NOLOCK) ON PS.SURVEY_ID = SL.SURVEY_ID AND ISNULL(SL.DELETED, 0) = 0                            
   left JOIN (select * from FOX_TBL_PATIENT_SURVEY_ADDITIONAL_NUMBER WITH (NOLOCK) where ADDITIONAL_NUMBER_ID IN (SELECT distinct              
  MAX_ADDITIONAL_NUMBER_ID = MAX(ADDITIONAL_NUMBER_ID) OVER (PARTITION BY survey_id)    
FROM FOX_TBL_PATIENT_SURVEY_ADDITIONAL_NUMBER WITH (NOLOCK))) as AD1 ON AD1.SURVEY_ID = PS.SURVEY_ID              
left JOIN (select * from FOX_TBL_PATIENT_SURVEY_ADDITIONAL_NUMBER WITH (NOLOCK) where ADDITIONAL_NUMBER_ID IN (SELECT distinct              
  MIN_ADDITIONAL_NUMBER_ID = MIN(ADDITIONAL_NUMBER_ID) OVER (PARTITION BY survey_id)              
FROM FOX_TBL_PATIENT_SURVEY_ADDITIONAL_NUMBER WITH (NOLOCK))) as AD2 ON AD2.SURVEY_ID = PS.SURVEY_ID              
                 
                        
 WHERE ISNULL(PS.DELETED, 0) = 0                             
         AND PS.PRACTICE_CODE = @PRACTICE_CODE                            
   AND IS_SURVEYED = @SURVEY_STATUS                            
   AND PS.PATIENT_ACCOUNT_NUMBER = @PATIENT_ACCOUNT                            
END                    
   
