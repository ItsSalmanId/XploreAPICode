﻿using FOX.DataModels.Models.IndexInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace FOX.DataModels.Models.OriginalQueueModel
{
    [Table("FOX_TBL_WORK_QUEUE")]
    public class OriginalQueue : BaseModel
    {
        [Key]
        public long WORK_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public string UNIQUE_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        [NotMapped]
        public string Patient_Account_Str
        {
            get
            {
                return PATIENT_ACCOUNT.ToString();
            }
            set
            {
                long _lg;
                PATIENT_ACCOUNT = long.TryParse(value, out _lg) ? _lg : 0;
            }
        }
        public long? PRACTICE_CODE { get; set; }
        public string SORCE_TYPE { get; set; }
        public string SORCE_NAME { get; set; }
        public string SORCE_NAME_FaxFormat
        {
            get
            {
                if (!string.IsNullOrEmpty(SORCE_NAME) && (SORCE_NAME.ToLower().Equals("anonymous") || Regex.IsMatch(SORCE_NAME, @"^[a-zA-Z0-9_]+$")))
                {
                    SORCE_NAME = "";
                }
                if (!string.IsNullOrWhiteSpace(SORCE_NAME) && SORCE_NAME.Contains("+1"))
                {
                    SORCE_NAME = SORCE_NAME.Replace("+1", "");
                    if (!string.IsNullOrEmpty(SORCE_NAME))
                    {
                        SORCE_NAME = String.Format("{0:(###) ###-####}", Int64.Parse(SORCE_NAME));
                    }
                }
                return SORCE_NAME;
            }
        }
        public string WORK_STATUS { get; set; }
        [NotMapped]
        public string Indexing_Status { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }
        [NotMapped]
        public string Received_Date_Str { get; set; }
        public int? TOTAL_PAGES { get; set; }
        public int? NO_OF_SPLITS { get; set; }
        public string FILE_PATH { get; set; }
        [NotMapped]
        public string FILE_PATH_LOGO { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string ASSIGNED_BY { get; set; }
        public DateTime? ASSIGNED_DATE { get; set; }
        public string COMPLETED_BY { get; set; }
        public DateTime? COMPLETED_DATE { get; set; }
        public long? DOCUMENT_TYPE { get; set; }
        public long? SENDER_ID { get; set; }
        public string FACILITY_NAME { get; set; }
        [NotMapped]
        public string FINANCIAL_CLASS_NAME { get; set; }
        [NotMapped]
        public int FINANCIAL_CLASS_ID { get; set; }
        public long? FACILITY_ID { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public bool IS_EMERGENCY_ORDER { get; set; }
        public string REASON_FOR_VISIT { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string UNIT_CASE_NO { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public double TOTAL_ROCORD_PAGES { get; set; }
        [NotMapped]
        public bool IsCompleted { get; set; }
        [NotMapped]
        public bool IsSaved { get; set; }
        public string INDEXED_BY { get; set; }
        public DateTime? INDEXED_DATE { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        public string FAX_ID { get; set; }
        public bool supervisor_status { get; set; }
        public DateTime? INDEXER_ASSIGN_DATE { get; set; }
        public DateTime? AGENT_ASSIGN_DATE { get; set; }
        public bool? IS_VERIFIED_BY_RECIPIENT { get; set; }
        public bool? IsSigned { get; set; }
        public long? SignedBy { get; set; }
        public long? FOX_TBL_SENDER_TYPE_ID { get; set; }
        public long? FOX_TBL_SENDER_NAME_ID { get; set; }
        [NotMapped]
        public bool? IS_UNSIGNED { get; set; }
        public string GuestID { get; set; }
        public string REASON_FOR_THE_URGENCY { get; set; }
        public bool? IS_POST_ACUTE { get; set; }
        public DateTime? EXPECTED_DISCHARGE_DATE { get; set; }
        [NotMapped]
        public string EXPECTED_DISCHARGE_DATE_STR { get; set; }
        [NotMapped]
        public bool IsRequestForOrder { get; set; }
        [NotMapped]
        public string SPECIALITY_PROGRAM { get; set; }
        public bool? IS_EVALUATE_TREAT { get; set; }
        public string HEALTH_NAME { get; set; }
        public string HEALTH_NUMBER { get; set; }
        public bool? IS_VERBAL_ORDER { get; set; }
        public long? VO_ON_BEHALF_OF { get; set; }
        public string VO_RECIEVED_BY { get; set; }
        public DateTime? VO_DATE_TIME { get; set; }
        [NotMapped]
        public string VO_DATE_TIME_STR { get; set; }

        public Nullable<System.DateTime> TRANSFER_DATE { get; set; }
        [NotMapped]
        public List<FOX_TBL_PATIENT_DIAGNOSIS> DIAGNOSIS { get; set; }
        [NotMapped]
        public string CURRENT_DATE_STR { get; set; }
        public bool? IS_TRASH_REFERRAL { get; set; }
        [NotMapped]
        public bool is_strategic_account { get; set; }
        [NotMapped]
        public bool IS_STRATEGIC { get; set; }
        public string RFO_Type { get; set; }
        public string REFERRAL_EMAIL_SENT_TO { get; set; }
        [NotMapped]
        public bool Is_Manual_ORS { get; set; }
        [NotMapped]
        public string ORS_NAME { get; set; }
        [NotMapped]
        public string ORS_PHONE { get; set; }
        [NotMapped]
        public string ORS_FAX { get; set; }
        [NotMapped]
        public string ORS_NPI { get; set; }
        public long? OCR_STATUS_ID { get; set; }
        [NotMapped]
        public string OCR_STATUS { get; set; }
        [NotMapped]
        public bool IS_INTERFACE { get; set; }
        [NotMapped]
        public string FOX_TBL_PHD_CALL_DETAIL_ID { get; set; }
        [NotMapped]
        public bool ISNoAssociate { get; set; }
        [NotMapped]
        public bool? IS_ORS { get; set; }
        public long? FOX_SOURCE_CATEGORY_ID { get; set; }

        [NotMapped]
        public List<string> FileNameList { get; set; }
        [NotMapped]
        public string THERAPY_TREATMENT_REFERRAL_REQUEST_HTML { get; set; }
        [NotMapped]
        public string guesttextarea { get; set; }
        [NotMapped]
        public string STATUS_TEXT { get; set; }
        [NotMapped]
        public long talkRehabWorkID { get; set; }
    }

    public class AddHtmlToDB
    {
        public long WORK_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string UNIQUE_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public long? SENDER_ID { get; set; }
        public string CREATED_BY { get; set; }
        public string FACILITY_NAME { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public bool? IS_VERBAL_ORDER { get; set; }
        public bool? IS_EVALUATE_TREAT { get; set; }
        public bool? IS_EMERGENCY_ORDER { get; set; }
        public string REASON_FOR_THE_URGENCY { get; set; }
        public string HEALTH_NAME { get; set; }
        public string HEALTH_NUMBER { get; set; }
        public long? VO_ON_BEHALF_OF { get; set; }
        public string VO_RECIEVED_BY { get; set; }
        public DateTime? VO_DATE_TIME { get; set; }
        public string VO_DATE_TIME_STR { get; set; }
        public string REASON_FOR_VISIT { get; set; }
        public string LAST_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string GENDER { get; set; }
        public DateTime? DOB { get; set; }
        public string CHART_ID { get; set; }
        public string DOCUMENT_NAME { get; set; }
        public string SPECIALITY_PROGRAM { get; set; }
        public string ORS_FIRST_NAME { get; set; }
        public string ORS_LAST_NAME { get; set; }
        public bool? is_strategic_account { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }

    }
    public class OriginalQueueRequest : BaseModel
    {
        public int RecordPerPage { get; set; }
        public bool IncludeArchive { get; set; }
        public string SearchText { get; set; }
        public string SorceType { get; set; }
        public string SorceString { get; set; }
        public string Client { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int CurrentPage { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }

        public string DateFrom_str { get; set; }
        public string DateTo_str { get; set; }
        public long PracticeCode { get; set; }
        [NotMapped]
        public long WORK_ID { get; set; }
        [NotMapped]
        public string STATUS_TEXT { get; set; }
    }

    public class ViewReferralRequestModel
    {
        public string ACC_NUMBER { get; set; }
        public string MRN { get; set; }
        public string PATIENT_FIRST_NAME { get; set; }
        public string PATIENT_LAST_NAME { get; set; }
        public DateTime? DATE_FROM { get; set; }
        public DateTime? DATE_TO { get; set; }
        public int CURRENT_PAGE { get; set; }
        public string SORT_BY { get; set; }
        public string SORT_ORDER { get; set; }
        public string DATE_FROM_STR { get; set; }
        public string DATE_TO_STR { get; set; }
        public int RECORD_PER_PAGE { get; set; }
        public string SEARCH_TEXT { get; set; }
        public string PROVIDER { get; set; }
        public long IS_SIGNED { get; set; }
        public int TIME_FRAME { get; set; }
        public List<ViewReferralModel> ReferralViewList { get; set; }
        public long WORK_ID { get; set; }
    }

    public class ViewReferralModel
    {

        public long WORK_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public string UNIQUE_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string SORCE_TYPE { get; set; }
        public string SORCE_NAME { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }
        public string PATIENT_FIRST_NAME { get; set; }
        public string PATIENT_LAST_NAME { get; set; }
        public string MRN { get; set; }
        public string FILE_PATH { get; set; }
        public bool DELETED { get; set; }
        public int TOTAL_RECORDS { get; set; }
        public double TOTAL_RECORD_PAGES { get; set; }
        public string REFERRAL_EMAIL_SENT_TO { get; set; }
    }
  


    [Table("FOX_TBL_WORK_QUEUE_File_All")]
    public class OriginalQueueFiles
    {
        [Key]
        public long FILE_ID { get; set; }
        public long WORK_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public string FILE_PATH { get; set; }
        public string FILE_PATH1 { get; set; }
        public bool deleted { get; set; }
    }

    public class WorkDetails
    {
        public long WORK_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public string WORK_STATUS { get; set; }
        public int? NO_OF_PAGES { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string COMPLETED_BY { get; set; }
        public string FILE_PATH { get; set; }
        public bool IS_EMERGENCY_ORDER { get; set; }
    }

    public class ResOriginalQueueModel
    {
        public List<OriginalQueue> OriginalQueueList { get; set; }
        public bool IsUniqueIdExist { get; set; } = false;
        public List<string> Unique_IdList { get; set; }
    }


    public class ReqOriginalQueueModel : BaseModel
    {
        public List<string> Unique_IdList { get; set; }
    }

    [Table("FOX_TBL_BLACKLISTED_WHITELISTED_SOURCE")]
    public class BlacklistedWhitelistedSource
    {
        [Key]
        public long BLACKLISTED_WHITELISTED_SOURCE_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string SOURCE_TYPE { get; set; }
        public string SOURCE_NAME { get; set; }
        public bool? IS_BLACK_LISTED { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_WORK_ORDER_ADDTIONAL_INFO")]
    public class WorkOrderAddtionalInfo
    {
        [Key]
        public long WORK_ORDER_ADDTIONAL_INFO_ID { get; set; }
        public long? WORK_ID { get; set; }
        public bool? IS_TRASH_REFERRAL { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("FOX_TBL_REFERRAL_SENDER")]
    public class ReferralSender
    {
        [Key]
        public long ID { get; set; }
        public string SENDER { get; set; }
        public bool FOR_STRATEGIC { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    [Table("DS_FOX_OCR")]
    public class DsFoxOcr
    {
        [Key]
        public long DS_FOX_RFO_ID           { get; set; }
        public string Patient_First_Name         { get; set; }
        public string Patient_Last_Name { get; set; }
        public string Provider_First_Name        { get; set; }
        public string Provider_Last_Name { get; set; }
        public string Patient_phone        { get; set; }
        public string Provider_phone       { get; set; }
        public DateTime? Patient_dob          { get; set; }
        public string Provider_fax         { get; set; }
        public string Pri_ins_name         { get; set; }
        public DateTime? Pri_Effective_date   { get; set; }
        public string Ssn                  { get; set; }
        public string Diagnosis            { get; set; }
        public DateTime? DOS                  { get; set; }
        public long? sec_ins_id           { get; set; }
        public string sec_ins_name         { get; set; }
        public DateTime? sec_effective_date   { get; set; }
        public string File_path_fox        { get; set; }
        public string File_path_ds         { get; set; }
        public DateTime? modified_Date        { get; set; }
        public string modified_by          { get; set; }
        public DateTime? created_date         { get; set; }
        public string created_by           { get; set; }
        public bool? Deleted           { get; set; }
        public string DEPARTMENT_ID        { get; set; }
        public long? FACILITY_ID          { get; set; }
        public long? SENDER_ID            { get; set; }
        public string REASON_FOR_VISIT     { get; set; }
        public long? PATIENT_ACCOUNT      { get; set; }
        public long? Pri_Ins_ID           { get; set; }
        public string Zip                  { get; set; }
        public string State                { get; set; }
        public string City                 { get; set; }
        public string Place_of_Service     { get; set; }
        public string Address              { get; set; }
        public string Primary_Policy_Number { get; set; }
        public string Secondary_Policy_Number { get; set; }
        public string REGION { get; set; }
        public long? work_id { get; set; }
        public string GENDER { get; set; }
    }

    [Table("FOX_TBL_OCR_STATUS")]
    public class FoxOcrStatus
    {
        [Key]
        public long OCR_STATUS_ID { get; set; }
        public string OCR_STATUS { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool? DELETED { get; set; }
    }
    public class CCREMOTEResOriginalQueueModel
    {
        public List<CCREMOTEOriginalQueueExcelMapping> CCREMOTEOriginalQueueList { get; set; }
        public bool IsUniqueIdExist { get; set; } = false;
        public List<string> Unique_IdList { get; set; }
        public List<OriginalQueue> OriginalQueueList { get; set; }
    }
    public class CCREMOTEOriginalQueueExcelMapping
    {
        [Key]
        public long WORK_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public string UNIQUE_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        [NotMapped]
        public string Patient_Account_Str
        {
            get
            {
                return PATIENT_ACCOUNT.ToString();
            }
            set
            {
                long _lg;
                PATIENT_ACCOUNT = long.TryParse(value, out _lg) ? _lg : 0;
            }
        }
        public long? PRACTICE_CODE { get; set; }
        public string SORCE_TYPE { get; set; }
        public string SORCE_NAME { get; set; }
        public string SORCE_NAME_FaxFormat { set; get; }
        public string WORK_STATUS { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }
        [NotMapped]
        public string Received_Date_Str { get; set; }
        public int? TOTAL_PAGES { get; set; }
        public int? NO_OF_SPLITS { get; set; }
        public string FILE_PATH { get; set; }
        [NotMapped]
        public string FILE_PATH_LOGO { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string ASSIGNED_BY { get; set; }
        public DateTime? ASSIGNED_DATE { get; set; }
        public string COMPLETED_BY { get; set; }
        public DateTime? COMPLETED_DATE { get; set; }
        public long? DOCUMENT_TYPE { get; set; }
        public long? SENDER_ID { get; set; }
        public string FACILITY_NAME { get; set; }
        [NotMapped]
        public string FINANCIAL_CLASS_NAME { get; set; }
        [NotMapped]
        public int FINANCIAL_CLASS_ID { get; set; }
        public long? FACILITY_ID { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public bool IS_EMERGENCY_ORDER { get; set; }
        public string REASON_FOR_VISIT { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string UNIT_CASE_NO { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        [NotMapped]
        public double TOTAL_ROCORD_PAGES { get; set; }
        [NotMapped]
        public bool IsCompleted { get; set; }
        [NotMapped]
        public bool IsSaved { get; set; }
        public string INDEXED_BY { get; set; }
        public DateTime? INDEXED_DATE { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        public string FAX_ID { get; set; }
        public bool supervisor_status { get; set; }
        public DateTime? INDEXER_ASSIGN_DATE { get; set; }
        public DateTime? AGENT_ASSIGN_DATE { get; set; }
        public bool? IS_VERIFIED_BY_RECIPIENT { get; set; }
        public bool? IsSigned { get; set; }
        public long? SignedBy { get; set; }
        public long? CCREMOTE_TBL_SENDER_TYPE_ID { get; set; }
        public long? CCREMOTE_TBL_SENDER_NAME_ID { get; set; }
        [NotMapped]
        public bool? IS_UNSIGNED { get; set; }
        public string GuestID { get; set; }
        public string REASON_FOR_THE_URGENCY { get; set; }
        public bool? IS_POST_ACUTE { get; set; }
        public DateTime? EXPECTED_DISCHARGE_DATE { get; set; }
        [NotMapped]
        public string EXPECTED_DISCHARGE_DATE_STR { get; set; }
        [NotMapped]
        public bool IsRequestForOrder { get; set; }
        [NotMapped]
        public string SPECIALITY_PROGRAM { get; set; }
        public bool? IS_EVALUATE_TREAT { get; set; }
        public string HEALTH_NAME { get; set; }
        public string HEALTH_NUMBER { get; set; }
        public bool? IS_VERBAL_ORDER { get; set; }
        public long? VO_ON_BEHALF_OF { get; set; }
        public string VO_RECIEVED_BY { get; set; }
        public DateTime? VO_DATE_TIME { get; set; }
        [NotMapped]
        public string VO_DATE_TIME_STR { get; set; }

        public Nullable<System.DateTime> TRANSFER_DATE { get; set; }
        [NotMapped]
        public List<FOX_TBL_PATIENT_DIAGNOSIS> DIAGNOSIS { get; set; }
        [NotMapped]
        public string CURRENT_DATE_STR { get; set; }
        public bool? IS_TRASH_REFERRAL { get; set; }
        [NotMapped]
        public bool is_strategic_account { get; set; }
        [NotMapped]
        public bool IS_STRATEGIC { get; set; }
        public string RFO_Type { get; set; }
        public string REFERRAL_EMAIL_SENT_TO { get; set; }
        [NotMapped]
        public bool Is_Manual_ORS { get; set; }
        [NotMapped]
        public string ORS_NAME { get; set; }
        [NotMapped]
        public string ORS_PHONE { get; set; }
        [NotMapped]
        public string ORS_FAX { get; set; }
        [NotMapped]
        public string ORS_NPI { get; set; }
        public long? OCR_STATUS_ID { get; set; }
        [NotMapped]
        public string OCR_STATUS { get; set; }
        [NotMapped]
        public bool IS_INTERFACE { get; set; }
        [NotMapped]
        public string CCREMOTE_TBL_PHD_CALL_DETAIL_ID { get; set; }
        [NotMapped]
        public bool ISNoAssociate { get; set; }
        [NotMapped]
        public bool? IS_ORS { get; set; }
        public long? CCREMOTE_SOURCE_CATEGORY_ID { get; set; }

        [NotMapped]
        public List<string> FileNameList { get; set; }
        [NotMapped]
        public string THERAPY_TREATMENT_REFERRAL_REQUEST_HTML { get; set; }
        [NotMapped]
        public string guesttextarea { get; set; }
        [NotMapped]
        public string STATUS_TEXT { get; set; }
    }
    public class OriginalQueueMapping : BaseModel
    {
        public long WORK_ID { get; set; }
        public int ROW { get; set; }
        public string UNIQUE_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public string Patient_Account_Str
        {
            get
            {
                return PATIENT_ACCOUNT.ToString();
            }
            set
            {
                long _lg;
                PATIENT_ACCOUNT = long.TryParse(value, out _lg) ? _lg : 0;
            }
        }
        public long? PRACTICE_CODE { get; set; }
        public string SORCE_TYPE { get; set; }
        public string SORCE_NAME { get; set; }
        public string SORCE_NAME_FaxFormat { set; get; }
        public string WORK_STATUS { get; set; }
        [NotMapped]
        public string Indexing_Status { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }
        public string Received_Date_Str { get; set; }
        public int? TOTAL_PAGES { get; set; }
        public int? NO_OF_SPLITS { get; set; }
        public string FILE_PATH { get; set; }
        public string FILE_PATH_LOGO { get; set; }
        public string ASSIGNED_TO { get; set; }
        public string ASSIGNED_BY { get; set; }
        public DateTime? ASSIGNED_DATE { get; set; }
        public string COMPLETED_BY { get; set; }
        public DateTime? COMPLETED_DATE { get; set; }
        public long? DOCUMENT_TYPE { get; set; }
        public long? SENDER_ID { get; set; }
        public string FACILITY_NAME { get; set; }
        public string FINANCIAL_CLASS_NAME { get; set; }
        public int FINANCIAL_CLASS_ID { get; set; }
        public long? FACILITY_ID { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public bool IS_EMERGENCY_ORDER { get; set; }
        public string REASON_FOR_VISIT { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string UNIT_CASE_NO { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public double TOTAL_ROCORD_PAGES { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsSaved { get; set; }
        public string INDEXED_BY { get; set; }
        public DateTime? INDEXED_DATE { get; set; }
        public int TOTAL_RECORDS { get; set; }
        public string FAX_ID { get; set; }
        public bool supervisor_status { get; set; }
        public DateTime? INDEXER_ASSIGN_DATE { get; set; }
        public DateTime? AGENT_ASSIGN_DATE { get; set; }
        public bool? IS_VERIFIED_BY_RECIPIENT { get; set; }
        public bool? IsSigned { get; set; }
        public long? SignedBy { get; set; }
        public long? FOX_TBL_SENDER_TYPE_ID { get; set; }
        public long? FOX_TBL_SENDER_NAME_ID { get; set; }
        public bool? IS_UNSIGNED { get; set; }
        public string GuestID { get; set; }
        public string REASON_FOR_THE_URGENCY { get; set; }
        public bool? IS_POST_ACUTE { get; set; }
        public DateTime? EXPECTED_DISCHARGE_DATE { get; set; }
        public string EXPECTED_DISCHARGE_DATE_STR { get; set; }
        public bool IsRequestForOrder { get; set; }
        public string SPECIALITY_PROGRAM { get; set; }
        public bool? IS_EVALUATE_TREAT { get; set; }
        public string HEALTH_NAME { get; set; }
        public string HEALTH_NUMBER { get; set; }
        public bool? IS_VERBAL_ORDER { get; set; }
        public long? VO_ON_BEHALF_OF { get; set; }
        public string VO_RECIEVED_BY { get; set; }
        public DateTime? VO_DATE_TIME { get; set; }
        public string VO_DATE_TIME_STR { get; set; }
        public Nullable<System.DateTime> TRANSFER_DATE { get; set; }
        public List<FOX_TBL_PATIENT_DIAGNOSIS> DIAGNOSIS { get; set; }
        public string CURRENT_DATE_STR { get; set; }
        public bool? IS_TRASH_REFERRAL { get; set; }
        public bool is_strategic_account { get; set; }
        public bool IS_STRATEGIC { get; set; }
        public string RFO_Type { get; set; }
        public string REFERRAL_EMAIL_SENT_TO { get; set; }
        public bool Is_Manual_ORS { get; set; }
        public string ORS_NAME { get; set; }
        public string ORS_PHONE { get; set; }
        public string ORS_FAX { get; set; }
        public string ORS_NPI { get; set; }
        public long? OCR_STATUS_ID { get; set; }
        public string OCR_STATUS { get; set; }
        public bool IS_INTERFACE { get; set; }
        public string FOX_TBL_PHD_CALL_DETAIL_ID { get; set; }
        public bool ISNoAssociate { get; set; }
        public bool? IS_ORS { get; set; }
        public long? FOX_SOURCE_CATEGORY_ID { get; set; }
        public List<string> FileNameList { get; set; }
        public string THERAPY_TREATMENT_REFERRAL_REQUEST_HTML { get; set; }
        public string guesttextarea { get; set; }
        public string STATUS_TEXT { get; set; }
    }
    public class ResOriginalQueueModelMapping
    {
        public List<OriginalQueueMapping> OriginalQueueListMapping { get; set; }
        public List<OriginalQueue> OriginalQueueList { get; set; }
        public bool IsUniqueIdExist { get; set; } = false;
        public List<string> Unique_IdList { get; set; }
    }

}