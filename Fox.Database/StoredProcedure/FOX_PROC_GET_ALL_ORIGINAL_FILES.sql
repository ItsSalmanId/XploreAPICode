IF (OBJECT_ID('FOX_PROC_GET_ALL_ORIGINAL_FILES') IS NOT NULL ) DROP PROCEDURE FOX_PROC_GET_ALL_ORIGINAL_FILES
GO  
CREATE Procedure [dbo].[FOX_PROC_GET_ALL_ORIGINAL_FILES]-- 1011163,'5447894'  
(  
 @PRACTICE_CODE BIGINT,  
 @UNIQUE_ID varchar(100)  
)  
AS  
BEGIN  
SET NOCOUNT ON;  
  
SELECT WQ.WORK_ID,F.FILE_PATH,F.FILE_PATH1,F.UNIQUE_ID  
FROM FOX_TBL_WORK_QUEUE WQ   
JOIN FOX_TBL_WORK_QUEUE_FILE_ALL F ON WQ.UNIQUE_ID=F.UNIQUE_ID AND WQ.UNIQUE_ID = @UNIQUE_ID   
  
WHERE PRACTICE_CODE=@PRACTICE_CODE  
  
END  
  
