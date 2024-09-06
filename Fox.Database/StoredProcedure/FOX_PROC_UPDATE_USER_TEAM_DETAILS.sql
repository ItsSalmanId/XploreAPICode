-- =============================================              
-- Author:    Muhammad Taseer Iqbal              
-- Create date: 05/09/2022              
-- Description:  This procedure is Update user team             
-- =============================================   
CREATE PROCEDURE FOX_PROC_UPDATE_USER_TEAM_DETAILS                
 @USER_ID BIGINT                  
AS                 
BEGIN               
UPDATE FOX_TBL_USER_TEAMS       
SET DELETED = 1       
WHERE [USER_ID]  = @USER_ID            
END   
  