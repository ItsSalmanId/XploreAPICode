 -- =============================================                
-- Author:  <Abdul Sattar>                
-- Create date: <08/22/2023>                
-- Description: <GET_ALL_GENERAL_NOTES>          
---FOX_PROC_GET_ALL_GENERAL_NOTES 1012714,554558, 554100, 600558,'',34344,'',''            
-- =============================================                
ALTER PROCEDURE [dbo].[FOX_PROC_GET_ALL_GENERAL_NOTES]       
@PRACTICE_CODE    BIGINT,             
  @CURRENT_PAGE     INT,             
  @RECORDS_PER_PAGE INT,             
  @SEARCH_TEXT      VARCHAR(50),             
  @PATIENT_ACCOUNT  BIGINT,             
  @SORT_BY          VARCHAR(50),             
  @SORT_ORDER       VARCHAR(5)            
 With Recompile                                                   
AS                                                    
BEGIN           
          
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED              
         SET @CURRENT_PAGE = @CURRENT_PAGE - 1;            
         DECLARE @START_FROM INT= @CURRENT_PAGE * @RECORDS_PER_PAGE;            
         DECLARE @TOTAL_PAGES FLOAT;            
         DECLARE @TOTAL_RECORDS FLOAT;            
         DECLARE @HISTORY_COUNT INT;            
         DECLARE @TAB TABLE            
         (HISTORY_COUNT   INT,             
          GENERAL_NOTE_ID BIGINT            
         );            
         INSERT INTO @TAB            
         (HISTORY_COUNT,             
          GENERAL_NOTE_ID            
         )            
         (            
             SELECT COUNT(H.PARENT_GENERAL_NOTE_ID) - 1 AS HISTORY_COUNT,             
                    MIN(H.GENERAL_NOTE_ID) AS GENERAL_NOTE_ID            
             FROM FOX_TBL_GENERAL_NOTE N  with(nolock, nowait)          
                  INNER JOIN FOX_TBL_GENERAL_NOTE H with(nolock, nowait) ON N.GENERAL_NOTE_ID = H.PARENT_GENERAL_NOTE_ID    
              WHERE N.PATIENT_ACCOUNT = @PATIENT_ACCOUNT    
        AND H.PATIENT_ACCOUNT = @PATIENT_ACCOUNT    
             GROUP BY H.PARENT_GENERAL_NOTE_ID            
         );            
         SELECT @TOTAL_RECORDS =            
         (            
             SELECT COUNT(GENERAL_NOTE_ID)            
             FROM FOX_TBL_GENERAL_NOTE with(nolock, nowait)            
             WHERE PATIENT_ACCOUNT = @PATIENT_ACCOUNT            
                   AND DELETED = 0            
                   AND PRACTICE_CODE = @PRACTICE_CODE            
                   AND NOTE_DESCRIPTION LIKE '%'+@SEARCH_TEXT+'%'            
                   AND GENERAL_NOTE_ID IN            
             (            
                 SELECT GENERAL_NOTE_ID            
                 FROM @TAB            
             )            
         );            
         IF(@RECORDS_PER_PAGE = 0)            
             BEGIN            
                 SET @RECORDS_PER_PAGE = @TOTAL_RECORDS;            
             END;            
             ELSE            
             BEGIN            
                 SET @RECORDS_PER_PAGE = @RECORDS_PER_PAGE;            
             END;            
         SET @TOTAL_PAGES = CEILING(@TOTAL_RECORDS / @RECORDS_PER_PAGE);            
         SELECT *,             
                @TOTAL_PAGES AS TOTAL_PAGES,             
                @TOTAL_RECORDS AS TOTAL_RECORDS            
         FROM            
         (            
             SELECT N.*,             
                    C.CASE_NO AS CASE_NO, isnull(c.RT_CASE_NO,'') RT_CASE_NO,             
             (            
                 SELECT COUNT(H.GENERAL_NOTE_ID) - 1            
                 FROM FOX_TBL_GENERAL_NOTE AS H with(nolock, nowait)            
                 WHERE N.PARENT_GENERAL_NOTE_ID = H.PARENT_GENERAL_NOTE_ID            
             ) AS HISTORY_COUNT,             
             (            
                 SELECT COUNT(T.GENERAL_NOTE_ID)            
                 FROM FOX_TBL_TASK AS T  with(nolock, nowait)           
                 WHERE N.PARENT_GENERAL_NOTE_ID = T.GENERAL_NOTE_ID            
                       AND T.DELETED = 0            
                       AND T.PRACTICE_CODE = @PRACTICE_CODE            
             ) AS TASK_COUNT,             
             (            
                 SELECT T.ATTACHMENT_PATH            
                 FROM FOX_TBL_TASK AS T   with(nolock, nowait)          
                 WHERE N.PARENT_GENERAL_NOTE_ID = T.GENERAL_NOTE_ID            
                       AND T.DELETED = 0            
     AND T.PRACTICE_CODE = @PRACTICE_CODE            
             ) AS ATTACHMENT_PATH,             
             (            
                 SELECT T.ATTACHMENT_TITLE            
                 FROM FOX_TBL_TASK AS T   with(nolock, nowait)          
                 WHERE N.PARENT_GENERAL_NOTE_ID = T.GENERAL_NOTE_ID            
                       AND T.DELETED = 0            
                       AND T.PRACTICE_CODE = @PRACTICE_CODE            
             ) AS ATTACHMENT_TITLE,             
                    CASE                           
   WHEN ISNULL(U.LAST_NAME, '') = '' THEN ISNULL(U.FIRST_NAME, '')                          
   ELSE ISNULL(U.LAST_NAME, '') + ', ' + ISNULL(U.FIRST_NAME, '')                          
   END AS CREATED_BY_FULL_NAME,             
                    --U.LAST_NAME+', '+U.FIRST_NAME AS CREATED_BY_FULL_NAME,             
             (            
                 SELECT ftgn.IS_PATIENT_ALERT            
                 FROM dbo.FOX_TBL_GENERAL_NOTE ftgn  with(nolock, nowait)           
                 WHERE ftgn.GENERAL_NOTE_ID = N.PARENT_GENERAL_NOTE_ID            
   ) AS IS_PARENT_NOTE_ALERT            
            FROM FOX_TBL_GENERAL_NOTE N with(nolock, nowait)            
                  LEFT JOIN FOX_TBL_CASE C with(nolock, nowait) ON N.CASE_ID = C.CASE_ID            
                  LEFT JOIN FOX_TBL_APPLICATION_USER U with(nolock, nowait) ON N.CREATED_BY = U.USER_NAME            
             WHERE N.GENERAL_NOTE_ID IN            
             (            
                 SELECT GENERAL_NOTE_ID            
                 FROM @TAB            
             )            
                   AND N.PRACTICE_CODE = @PRACTICE_CODE            
                   AND N.PATIENT_ACCOUNT = @PATIENT_ACCOUNT            
                   AND N.DELETED = 0            
                   AND N.NOTE_DESCRIPTION LIKE '%'+@SEARCH_TEXT+'%'            
         ) AS NOTES            
         ORDER BY CASE            
                      WHEN @SORT_BY = 'NOTE_DESCRIPTION'            
                           AND @SORT_ORDER = 'ASC'            
                      THEN NOTE_DESCRIPTION            
                  END ASC,            
                  CASE            
                      WHEN @SORT_BY = 'CASE_ID'            
                           AND @SORT_ORDER = 'ASC'            
                      THEN CASE_ID            
                  END ASC,            
                  CASE            
                      WHEN @SORT_BY = 'CREATED_BY'            
                           AND @SORT_ORDER = 'ASC'            
                      THEN CREATED_BY            
                  END ASC,            
                  CASE            
                      WHEN @SORT_BY = 'CREATED_DATE'            
                           AND @SORT_ORDER = 'ASC'            
                      THEN CREATED_DATE            
                  END ASC,            
                  CASE            
                      WHEN @SORT_BY = 'NOTE_DESCRIPTION'            
                           AND @SORT_ORDER = 'DESC'            
                      THEN NOTE_DESCRIPTION            
                  END DESC,            
                  CASE            
                      WHEN @SORT_BY = 'CASE_ID'            
                           AND @SORT_ORDER = 'DESC'            
                      THEN CASE_ID            
                  END DESC,            
                  CASE            
                      WHEN @SORT_BY = 'CREATED_BY'            
                           AND @SORT_ORDER = 'DESC'            
                      THEN CREATED_BY            
                  END DESC,            
                  CASE            
                      WHEN @SORT_BY = 'CREATED_DATE'            
                           AND @SORT_ORDER = 'DESC'            
                      THEN CREATED_DATE            
                  END DESC            
         OFFSET @START_FROM ROWS FETCH NEXT @RECORDS_PER_PAGE ROWS ONLY;            
     END; 



