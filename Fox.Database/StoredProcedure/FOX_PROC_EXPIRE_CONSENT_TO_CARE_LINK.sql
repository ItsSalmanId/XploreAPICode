 -- =============================================                
-- Author:  <Abdul Sattar>                
-- Create date: <08/22/2023>                
-- Description: <Description,update record change status from sent to expire and insert task logs>          
---FOX_PROC_EXPIRE_CONSENT_TO_CARE_LINK 1012714,554558, 554100, 600558            
-- =============================================          
ALTER PROCEDURE FOX_PROC_EXPIRE_CONSENT_TO_CARE_LINK          
(        
@PRACTICE_CODE BIGINT,        
@CONSENT_TO_CARE_ID BIGINT,        
@EXPIRE_STATUS_ID BIGINT,        
@TASK_ID BIGINT)          
AS          
BEGIN          
UPDATE FOX_TBL_CONSENT_TO_CARE SET STATUS_ID = @EXPIRE_STATUS_ID where CONSENT_TO_CARE_ID = @CONSENT_TO_CARE_ID        
Update FOX_TBL_TASK SET IS_SENDTO_MARK_COMPLETE = 1 , Completed_Date = GETDATE() where TASK_ID = @TASK_ID and PRACTICE_CODE = @PRACTICE_CODE  
  
DECLARE @TAB_TASK_LOG TABLE (MAX_TASK_LOG_ID BIGINT)          
DECLARE @MAXID BIGINT          
INSERT INTO @TAB_TASK_LOG EXEC Web_GetMaxColumnID 'FOX_TASK_LOG_ID'          
SET @MAXID = (SELECT TOP 1 MAX_TASK_LOG_ID FROM @TAB_TASK_LOG)          
insert into FOX_TBL_TASK_LOG (TASK_LOG_ID,PRACTICE_CODE,TASK_ID,[ACTION],ACTION_DETAIL)        
values         
(@MAXID,@PRACTICE_CODE,@TASK_ID,'Task Comment','No response is received. Consent to Care link has been expired.')         
END  
