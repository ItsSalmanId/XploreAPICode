IF (OBJECT_ID('FOX_PROC_GET_TASK_PATIENT') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_TASK_PATIENT  
GO 
-- =============================================        
-- Author:  <Muhammad Imran>        
-- Create date: <09/30/2019>        
-- Description: <Description,,> 
-- Modified By:  <AFTAB KHAN>
-- Modified date: <03/11/2024>   
-- =============================================        
  --  EXEC FOX_PROC_GET_TASK_PATIENT  1012714534318095359,  1012714    
CREATE PROCEDURE FOX_PROC_GET_TASK_PATIENT    
    @PATIENT_ACCOUNT BIGINT,    
    @PRACTICE_CODE BIGINT    
AS    
BEGIN    
    -- SET NOCOUNT ON added to prevent extra result sets from    
    -- interfering with SELECT statements.    
    SET NOCOUNT ON;    
    
    SELECT PA.ADDRESS AS POS_ADDRESS, PA.CITY AS POS_CITY, PA.ZIP AS POS_ZIP, PA.ADDRESS_TYPE AS POS_TYPE,  P.*    
    FROM FOX_TBL_PATIENT_ADDRESS PA   WITH(NOLOCK) 
    RIGHT JOIN PATIENT P WITH (NOLOCK) ON PA.PATIENT_ACCOUNT = P.PATIENT_ACCOUNT AND PA.ADDRESS_TYPE = 'Statement Address'      
    WHERE P.PATIENT_ACCOUNT = @PATIENT_ACCOUNT    
        AND P.PRACTICE_CODE = @PRACTICE_CODE    
        AND ISNULL(P.DELETED, 0) = 0;  
END;    
    