IF (OBJECT_ID('AF_PROC_GETINTERFAXDETAILS') IS NOT NULL) DROP PROCEDURE FOX_PROC_GET_WORK_QUEUE_DETAILS_UPDATE_EMAIL
GO
-- AUTHOR:  <AFTAB KHAN>                                                                                                    
-- CREATE DATE: <CREATE DATE, 11/13/2023>                                                                                                    
-- DESCRIPTION: <FOX_PROC_GET_WORK_QUEUE_DETAILS_UPDATE_EMAIL>          
-- EXEC [DBO].[FOX_PROC_GET_WORK_QUEUE_DETAILS_UPDATE_EMAIL]  'aftabkhan@carecloud.com' 584548, 1012714                                                                                                
CREATE PROCEDURE [dbo].[FOX_PROC_GET_WORK_QUEUE_DETAILS_UPDATE_EMAIL]                                                                                                                 
(   
  @EMAIL_ADDRESS VARCHAR,  
  @WORK_ID BIGINT,                          
  @PRACTICE_CODE BIGINT                                                                                                                                                           
)                                                                                                                      
AS                                                                                                                      
BEGIN                                                          
--SELECT * FROM FOX_TBL_WORK_QUEUE WITH (NOLOCK) WHERE WORK_ID = @WORK_ID AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED ,0) = 0   
UPDATE FOX_TBL_WORK_QUEUE SET REFERRAL_EMAIL_SENT_TO = @EMAIL_ADDRESS WHERE WORK_ID = @WORK_ID AND PRACTICE_CODE = @PRACTICE_CODE AND ISNULL(DELETED ,0) = 0  
END     

