-- =============================================                                          
-- AUTHOR:  <Muhammad Salman>                                          
-- CREATE DATE: <CREATE DATE, 08/02/2023>                                          
-- DESCRIPTION: <GET_CONSENT_TO_CARE>                                                        
-- [dbo].[FOX_PROC_SAVE_EXTERNAL_USER_INFO] 1012714, '', '','','','','', 1012714                         
CREATE PROCEDURE FOX_PROC_GET_CONSENT_TO_CARE_TASK_DETAILS_BY_CASE_ID                                 
@CASE_ID  BIGINT,                                                
@PRACTICE_CODE  BIGINT                                
AS                       
BEGIN                 
Declare @SEND_STATUS_ID BIGINT =(select top 10 CONSENT_TO_CARE_STATUS_ID From   FOX_TBL_CONSENT_TO_CARE_STATUS WITH(NOLOCK) where STATUS_NAME = 'Sent' and ISNULL(DELETED, 0) = 0  and PRACTICE_CODE = @PRACTICE_CODE)            
select  TASK_ID From FOX_TBL_CONSENT_TO_CARE  WITH(NOLOCK)  
where     
CASE_ID = @CASE_ID      
and     
STATUS_ID = @SEND_STATUS_ID    
and ISNULL(DELETED, 0) = 0  and PRACTICE_CODE = @PRACTICE_CODE    
                                       
END 
