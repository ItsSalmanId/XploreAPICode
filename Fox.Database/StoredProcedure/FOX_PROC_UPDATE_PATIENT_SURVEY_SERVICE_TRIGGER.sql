
 -- =============================================              
-- Author:  Muhammad Salman             
-- Create date: 12/03/2024              
-- Description: This SP trigger to INSERT Log of patient for survey automation service              
-- =============================================              
-- FOX_PROC_INSERT_PATIENT_SURVEY_SERVICE_LOG 1011163              
CREATE PROCEDURE FOX_PROC_UPDATE_PATIENT_SURVEY_SERVICE_TRIGGER                         
 @FILE_NAME VARCHAR(MAX),              
 @PRACTICE_CODE BIGINT  
-- @SERVICE_FOR VARCHAR (10)  
AS              
BEGIN     
   UPDATE FOX_TBL_SURVEY_AUTOMATION_SERVICE_TRIGGER set IS_EMAIL = 1 ,IS_SMS = 1,  MODIFIED_DATE = GETDATE() ,MODIFIED_BY = 'FOX TEAM' where FILE_NAME = @FILE_NAME and @PRACTICE_CODE = PRACTICE_CODE  
END     