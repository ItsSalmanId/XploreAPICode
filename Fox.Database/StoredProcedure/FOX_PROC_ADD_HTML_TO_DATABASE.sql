IF (OBJECT_ID('AF_PROC_GETINTERFAXDETAILS') IS NOT NULL) DROP PROCEDURE FOX_PROC_ADD_HTML_TO_DATABASE
GO
-- AUTHOR:  <AFTAB KHAN>                                                                                                    
-- CREATE DATE: <CREATE DATE, 11/13/2023>                                                                                                    
-- DESCRIPTION: <FOX_PROC_ADD_HTML_TO_DATABASE>    
--EXEC FOX_PROC_ADD_HTML_TO_DATABASE '53437247'      
ALTER PROCEDURE FOX_PROC_ADD_HTML_TO_DATABASE      
    @WORK_ID VARCHAR(80)       
AS      
BEGIN      
    SELECT      
        WQ.WORK_ID,       
        WQ.PRACTICE_CODE,       
        WQ.UNIQUE_ID,      
        WQ.PATIENT_ACCOUNT,      
        WQ.SENDER_ID,       
        WQ.CREATED_BY,      
        WQ.FACILITY_NAME,      
        WQ.DEPARTMENT_ID,      
        WQ.IS_VERBAL_ORDER,      
        WQ.IS_EVALUATE_TREAT,      
        WQ.IS_EMERGENCY_ORDER,      
        WQ.REASON_FOR_THE_URGENCY,      
        WQ.HEALTH_NAME,      
        WQ.HEALTH_NUMBER,      
        WQ.VO_ON_BEHALF_OF,       
        WQ.VO_RECIEVED_BY,      
        WQ.VO_DATE_TIME,      
        WQ.REASON_FOR_VISIT,      
        P.Last_Name AS LAST_NAME,       
        P.First_Name AS FIRST_NAME,       
        P.Gender AS GENDER,      
        P.Date_Of_Birth AS DOB,       
        P.Chart_Id AS CHART_ID,      
        PT.NAME AS DOCUMENT_NAME,       
        --PP.PAT_PROC_ID,      
        --PP.PROC_CODE,      
        PP.SPECIALITY_PROGRAM,      
        --PT.DESCRIPTION AS DOCUMENT_DESCRIPTION,      
        ORS.FIRST_NAME AS ORS_FIRST_NAME,      
        ORS.LAST_NAME AS ORS_LAST_NAME,    
        PA.[ADDRESS],    
  PA.CITY,    
  PA.[STATE],    
  PA.ZIP    
  --PA.ADDRESS_TYPE    
    FROM FOX_TBL_WORK_QUEUE WQ WITH (NOLOCK)      
    LEFT JOIN PATIENT P WITH (NOLOCK) ON P.Patient_Account = WQ.PATIENT_ACCOUNT       
    LEFT JOIN FOX_TBL_DOCUMENT_TYPE PT WITH (NOLOCK) ON PT.DOCUMENT_TYPE_ID = WQ.DOCUMENT_TYPE        
    LEFT JOIN FOX_TBL_ORDERING_REF_SOURCE ORS WITH (NOLOCK) ON ORS.SOURCE_ID = WQ.SENDER_ID      
    LEFT JOIN FOX_TBL_PATIENT_PROCEDURE PP WITH (NOLOCK) ON PP.WORK_ID = @WORK_ID    
    OUTER APPLY (    
        SELECT TOP 1 *    
        FROM FOX_TBL_PATIENT_ADDRESS PA WITH (NOLOCK) 
        WHERE PA.PATIENT_ACCOUNT = WQ.PATIENT_ACCOUNT    
          AND PA.ADDRESS_TYPE = 'Home Address'    
          AND ISNULL(PA.DELETED, 0) = 0    
        ORDER BY PA.MODIFIED_DATE DESC    
    ) PA    
    WHERE       
        ISNULL(WQ.DELETED, 0) = 0 AND WQ.WORK_ID = @WORK_ID --AND ISNULL(WQ.DOCUMENT_TYPE, 0) <> 0  
  ;    
END 
