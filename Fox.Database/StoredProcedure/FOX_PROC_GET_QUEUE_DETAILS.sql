IF (OBJECT_ID('AF_PROC_GETINTERFAXDETAILS') IS NOT NULL) DROP PROCEDURE FOX_PROC_GET_QUEUE_DETAILS
GO
-- =============================================            
-- Author:  <AFTAB KHAN>            
-- Create date: <10/17/2023>            
-- Description: <Description,,>            
-- =============================================            
        
CREATE PROCEDURE FOX_PROC_GET_QUEUE_DETAILS       
        
 @WORK_ID BIGINT         
AS            
BEGIN            
 -- SET NOCOUNT ON added to prevent extra result sets from            
 -- interfering with SELECT statements.            
  SELECT TOP 1 * FROM FOX_TBL_WORK_QUEUE WITH (NOLOCK)                
  WHERE ISNULL(DELETED,0) = 0          
  AND WORK_ID = @WORK_ID        
      
END     

