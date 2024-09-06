-- =============================================
-- Author: <AFTAB AHMED KHAN>
-- Create date: <01/12/2024>
-- Description: <Description,,>
-- =============================================
--select * from FOX_TBL_PATIENT_DIAGNOSIS where WORK_ID = 53437992
ALTER PROCEDURE FOX_PROC_DELETE_UPDATE_DIAGNOSIS_INFO
    @WORK_ID BIGINT = NULL,
    @PATIENT_ACCOUNT BIGINT = NULL,
    @USER_NAME VARCHAR(70) = NULL

AS
BEGIN
    DECLARE @ID BIGINT

    IF EXISTS (
            SELECT TOP 1 *
            FROM FOX_TBL_PATIENT_DIAGNOSIS WITH (NOLOCK)
            WHERE WORK_ID = @WORK_ID
                AND PATIENT_ACCOUNT = @PATIENT_ACCOUNT
                AND DELETED = 0
        )
    BEGIN
        UPDATE FOX_TBL_PATIENT_DIAGNOSIS
        SET DELETED = 1, MODIFIED_BY = @USER_NAME
        WHERE WORK_ID = @WORK_ID
            AND PATIENT_ACCOUNT = @PATIENT_ACCOUNT
            AND DELETED = 0;
    END
	END