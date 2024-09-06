IF (OBJECT_ID('FOX_PROC_GET_EMAIL_EMERGENCY_LIST') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_EMAIL_EMERGENCY_LIST 
GO  
CREATE PROCEDURE [dbo].[FOX_PROC_GET_EMAIL_EMERGENCY_LIST]        
AS        
     BEGIN        
         SELECT EMAIL_ADDRESS         
         FROM FOX_TBL_EMAIL_CONFIG        
         WHERE ISNULL(DELETED, 0) = 0     
   AND IsUrgentEmail= 1       
     END; 