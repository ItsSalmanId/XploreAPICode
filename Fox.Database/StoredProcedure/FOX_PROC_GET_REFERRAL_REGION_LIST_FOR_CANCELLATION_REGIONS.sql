 --EXEC [FOX_PROC_GET_REFERRAL_REGION_LIST_FOR_CANCELLATION_REGIONS] 1012714,'', '', '', 1, 10, 'createdDate' , 'DESC'                                               
 -- AUTHOR:  <DEVELOPER, AFTAB KHAN>      
-- CREATE DATE: <CREATE DATE, 01/31/2024>  
-- MODIFIED BY:  <DEVELOPER, AFTAB KHAN>   
-- MODIFY DATE: <MODIFIED DATE, 01/02/2024>         
-- DESCRIPTION: <[FOX_PROC_GET_REFERRAL_REGION_LIST_FOR_CANCELLATION_REGIONS]>   
CREATE PROCEDURE [dbo].[FOX_PROC_GET_REFERRAL_REGION_LIST_FOR_CANCELLATION_REGIONS] 
@PRACTICE_CODE BIGINT   
AS       
BEGIN                           
SELECT *      
FROM FOX_TBL_REFERRAL_REGION  WITH (NOLOCK)    
WHERE ISNULL(DELETED,0)=0 AND PRACTICE_CODE = @PRACTICE_CODE;      
END;  
