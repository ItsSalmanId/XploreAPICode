-- =============================================              
-- AUTHOR:  <DEVELOPER, TASEER IQBAL>              
-- CREATE DATE: <CREATE DATE, 23/06/2022>              
-- Description:  This procedure is Get Team member list on the basis of team ID          
-- =============================================     
create PROCEDURE FOX_PROC_GET_TEAM_MEMBER_LIST               
  @PRACTICE_CODE BIGINT,                
  @CALL_SCANRIO_ID BIGINT                
                 
AS                
BEGIN              
  IF(@CALL_SCANRIO_ID = 0)      
  BEGIN      
   SELECT  TBL_USER.USER_NAME, (TBL_USER.FIRST_NAME + ' '+ TBL_USER.LAST_NAME) AS NAME, TBL_USER.EMAIL  FROM  FOX_TBL_APPLICATION_USER TBL_USER         
   JOIN FOX_TBL_USER_TEAMS TBL_USER_TEAM ON TBL_USER.USER_ID = TBL_USER_TEAM.USER_ID        
   WHERE ISNULL(TBL_USER_TEAM.DELETED, 0) = 0  AND TBL_USER_TEAM.PRACTICE_CODE = @PRACTICE_CODE        
   UNION        
   SELECT  DISTINCT PHD.CREATED_BY AS USER_NAME, (AU.FIRST_NAME + ' '+ AU.LAST_NAME) AS NAME , EMAIL            
   FROM FOX_TBL_PHD_CALL_DETAILS PHD              
   LEFT JOIN FOX_TBL_APPLICATION_USER  AS AU ON  PHD.CREATED_BY = AU.USER_NAME AND AU.PRACTICE_CODE = @PRACTICE_CODE                
   WHERE              
   ISNULL(PHD.DELETED, 0) = 0              
   AND PHD.PRACTICE_CODE = @PRACTICE_CODE  AND ISNULL(AU.FIRST_NAME, '') <> ''  AND ISNULL(AU.LAST_NAME, '') <> ''     
   AND ISNULL(AU.DELETED,0)= 0              
  ORDER BY NAME ASC      
  END      
  ELSE      
  BEGIN      
    SELECT  TBL_USER.USER_NAME, (TBL_USER.FIRST_NAME + ' '+ TBL_USER.LAST_NAME) AS NAME, TBL_USER.EMAIL  FROM  FOX_TBL_APPLICATION_USER TBL_USER         
  JOIN FOX_TBL_USER_TEAMS TBL_USER_TEAM ON TBL_USER.USER_ID = TBL_USER_TEAM.USER_ID        
  WHERE TBL_USER_TEAM.PHD_CALL_SCENARIO_ID = @CALL_SCANRIO_ID AND ISNULL(TBL_USER_TEAM.DELETED, 0) = 0  AND TBL_USER_TEAM.PRACTICE_CODE = @PRACTICE_CODE        
   ORDER BY NAME ASC           
  END      
END 