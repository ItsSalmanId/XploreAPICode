-- =============================================              
-- Author:  Muhammad Salman             
-- Create date: 12/01/2024              
-- Description: This SP trigger to INSERT Log of patient for survey automation service              
-- =============================================              
-- FOX_PROC_GET_PATIENT_SURVEY_DETAILS_SERVICE_SERVICE_COPY 1011163     

  
ALTER PROCEDURE FOX_PROC_GET_PATIENT_SURVEY_DETAILS_SERVICE --1012714               
 -- Add the parameters for the stored procedure here                
 @PRACTICE_CODE BIGINT                
AS                
BEGIN             
        
 DECLARE @TRIGGERED_FILE VARCHAR(MAX) =  (SELECT TOP 1 FILE_NAME FROM FOX_TBL_SURVEY_AUTOMATION_SERVICE_TRIGGER WITH (NOLOCK)     
                                                  WHERE PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED, 0) = 0 AND IS_SMS = 0 AND IS_EMAIL = 0 ORDER BY CREATED_DATE ASC)     
                  
    if((@TRIGGERED_FILE <> ''))    
    BEGIN     
          SELECT                 
                          PS.PATIENT_ACCOUNT_NUMBER,                
                          PS.SURVEY_ID,                
                          PS.PATIENT_FIRST_NAME,                
                          PS.PATIENT_LAST_NAME,                
                          P.Email_Address AS EMAIL_ADDRESS,                
                          P.Home_Phone AS HOME_PHONE,               
                          PS.SURVEY_FORMAT_TYPE,                
                          PS.REGION,                
                          PS.PROVIDER,                
                          PS.PT_OT_SLP,                
                          PS.FILE_NAME,    
        PS.RESPONSIBLE_PARTY_LAST_NAME,    
        PS.RESPONSIBLE_PARTY_FIRST_NAME,    
        PS.SERVICE_OR_PAYMENT_DESCRIPTION,     
        REGION,    
        LAST_VISIT_DATE,    
        PS.EXCEL_ROW_NUMBER    
                          FROM FOX_TBL_PATIENT_SURVEY AS PS WITH (NOLOCK)                
                          INNER JOIN PATIENT AS P WITH (NOLOCK) ON PS.PATIENT_ACCOUNT_NUMBER = + '00' + P.CHART_ID AND P.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(P.DELETED, 0) = 0           
                          WHERE PS.PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(PS.DELETED, 0) = 0 AND PS.FILE_NAME = @TRIGGERED_FILE   
						   AND NOT EXISTS (
          SELECT 1
          FROM FOX_TBL_SURVEY_AUTOMATION_SERVICE_LOG AS SL WITH (NOLOCK)
          WHERE SL.SURVEY_ID = PS.SURVEY_ID
		  --AND SL.DELETED = 0-- Assuming the column name for survey ID is SURVEY_ID
      )
            
    END    
END 
