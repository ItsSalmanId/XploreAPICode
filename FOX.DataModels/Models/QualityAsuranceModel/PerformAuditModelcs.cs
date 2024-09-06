﻿using FOX.DataModels.Models.FoxPHD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.QualityAsuranceModel
{
    [Table("FOX_TBL_SURVEY_AUDIT_SCORES")]

    public class SurveyAuditScores : BaseModel
    {
        [Key]
        public long SURVEY_AUDIT_SCORES_ID { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public long? SURVEY_CALL_ID { get; set; }
        public long SURVEY_ID { get; set; }
        public long? GREETINGS { get; set; }
        public long? CALL_HANDLING_SKILLS { get; set; }
        public long? GRAMMER_PRONOUNCIATION_VOCABULARY { get; set; }
        public long? VERIFIED_PATIENT_ACCOUNT { get; set; }
        public long? ACCOUNT_AND_SCRIPT_READINESS { get; set; }
        //public long? PATIENT_VERIFICATION_AND_INSURANCE { get; set; }
        public long? CASE_HANDLING { get; set; }
        public long? EMAIL_TO_STAKE_HOLDERS { get; set; }
        public long? ESCALATION { get; set; }
        public int? BONUS_POINTS { get; set; }
        public int? PERFORMANCE_KILLER { get; set; }
        public long? GREETINGS_TOTAL { get; set; }
        public long? CALL_HANDLING_SKILLS_TOTAL { get; set; }
        public long? GRAMMER_PRONOUNCIATION_VOCABULARY_TOTAL { get; set; }
        public long? VERIFIED_PATIENT_ACCOUNT_TOTAL { get; set; }
        public long? ACCOUNT_AND_SCRIPT_READINESS_TOTAL { get; set; }
        public long? CASE_HANDLING_TOTAL { get; set; }
        public long? EMAIL_TO_STAKE_HOLDERS_TOTAL { get; set; }
        public long? ESCALATION_TOTAL { get; set; }
        public long? BONUS_POINTS_TOTAL { get; set; }
        public long? PERFORMANCE_KILLER_TOTAL { get; set; }
        public int? TOTAL_POINTS { get; set; }
        public string AUDITOR_NAME { get; set; }
        public string AGENT_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
        public long? CLIENT_EXPERIENCE_TOTAL { get; set; }
        public long? SYSTEM_PROCESS_TOTAL { get; set; }
        public string CLIENT_EXPERIENCE_COMMENT { get; set; }
        public string SYSTEM_PROCESS_COMMENT { get; set; }
        public string WOW_FACTOR_COMMENT { get; set; }
        public long? PHD_CALL_ID { get; set; }
        public string CALL_TYPE { get; set; }
        public long? PRODUCT_KNOWLEDGE { get; set; }
        public long? CORRECT_NOTES_AND_TRACKING { get; set; }
        public long? PRODUCT_KNOWLEDGE_TOTAL { get; set; }
        public long? CORRECT_NOTES_AND_TRACKING_TOTAL { get; set; }
        public string GRADE { get; set; }
        [NotMapped]
        public long? EVALUATIONS { get; set; }
        [NotMapped]
        public double TOTAL_RECORD_PAGES { get; set; }
        [NotMapped]
        public int TOTAL_RECORDS { get; set; }
        [NotMapped]
        public string HTML_TEMPLETE { get; set; }
        [NotMapped]
        public string CALL_SCANARIO { get; set; }
        [NotMapped]
        public DateTime EVALUATION_CALL_DATE { get; set; }
        [NotMapped]
        public string MRN { get; set; }
        [NotMapped]
        public string AGENT_EMAIL { get; set; }
        public int? PHD_CALL_SCENARIO_ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        [NotMapped]
        public string PATIENT_ACCOUNT_STR { get; set; }
        [NotMapped]
        public bool EDIT_AUDIT_REPORT { get; set; }
        [NotMapped]
        public string CALL_RECORDING_URL { get; set; }
        public long? DOCUMENTED_DATE_TIME { get; set; }
        public long? DOCUMENTED_RELEVANT_DETAILS { get; set; }
        public long? MARKED_QUSTIONS_CORRECTLY { get; set; }
        public long? MARKED_SURVEY_STATUS { get; set; }
        public long? CORRECTLY_IDENTIFY_FLAG { get; set; }
        public long? CORRECTLY_SEPARATE_QUESTION { get; set; }
        public long? FOLLOWED_SCRIPTED_SURVEY { get; set; }
        public long? FOLLOWED_CLOSING_SCRIPT { get; set; }
        public long? DOCUMENTED_DATE_TIME_TOTAL { get; set; }
        public long? DOCUMENTED_RELEVANT_DETAILS_TOTAL { get; set; }
        public long? MARKED_QUSTIONS_CORRECTLY_TOTAL { get; set; }
        public long? MARKED_SURVEY_STATUS_TOTAL { get; set; }
        public long? CORRECTLY_IDENTIFY_FLAG_TOTAL { get; set; }
        public long? CORRECTLY_SEPARATE_QUESTION_TOTAL { get; set; }
        public long? FOLLOWED_SCRIPTED_SURVEY_TOTAL { get; set; }
        public long? FOLLOWED_CLOSING_SCRIPT_TOTAL { get; set; }
        public string SCORING_CRITERIA { get; set; }
        public long? APPROPRIATE_GREETING { get; set; }
        public long? APPROPRIATE_GREETING_TOTAL { get; set; }
        public long? TONE_OF_PATIENT { get; set; }
        public long? TONE_OF_PATIENT_TOTAL { get; set; }
        public long? COMPASSION_AND_EMPATHY { get; set; }
        public long? COMPASSION_AND_EMPATHY_TOTAL { get; set; }
        public long? GRAMMAR_AND_PRONUNCIATION { get; set; }
        public long? GRAMMAR_AND_PRONUNCIATION_TOTAL { get; set; }
        public long? PATIENT_IDENTITY { get; set; }
        public long? PATIENT_IDENTITY_TOTAL { get; set; }
        public long? ANSWER_PATIENT_QUESTIONS { get; set; }
        public long? ANSWER_PATIENT_QUESTIONS_TOTAL { get; set; }
        public long? STRONG_PRODUCT_KNOWLEDGE { get; set; }
        public long? STRONG_PRODUCT_KNOWLEDGE_TOTAL { get; set; }
        public long? COMMUNICATE_INFORMATION { get; set; }
        public long? COMMUNICATE_INFORMATION_TOTAL { get; set; }
        public long? DOCUMENTATION_COMPLETED_COMMUNICATED { get; set; }
        public long? DOCUMENTATION_COMPLETED_COMMUNICATED_TOTAL { get; set; }
        public long? APPROPRIATE_CLOSING { get; set; }
        public long? APPROPRIATE_CLOSING_TOTAL { get; set; }
        public long? MAX_POTENTIAL_SCORE { get; set; }
        public decimal? CLIENT_EXPERIENCE_TOTAL_PER { get; set; }
        public decimal? SYSTEM_PROCESS_TOTAL_PER { get; set; }
        public decimal? CALL_QUALITY_TOTAL_PER { get; set; }
        public decimal? SYSTEM_USAGE_TOTAL_PER { get; set; }
        [NotMapped]
        public bool? IS_ASSOCIATED_CALL { get; set; }
    }

    //public class CallLogModel : BaseModel
    //{
    //    public long SURVEY_CALL_ID { get; set; }
    //    public long? PRACTICE_CODE { get; set; }
    //    public long? SURVEY_ID { get; set; }
    //    public long? ACU_CALL_ID { get; set; }
    //    public long? PATIENT_ACCOUNT { get; set; }
    //    public string FILE_NAME { get; set; }
    //    public bool? IS_RECEIVED { get; set; }
    //    public string CALL_DURATION { get; set; }
    //    public bool? IS_TO_PATIENT { get; set; }
    //    public string CREATED_BY { get; set; }
    //    public DateTime? CREATED_DATE { get; set; }
    //    public string MODIFIED_BY { get; set; }
    //    public DateTime? MODIFIED_DATE { get; set; }
    //    public bool DELETED { get; set; }

    //    [NotMapped]
    //    public bool IS_AUDITED { get; set; }
    //    [NotMapped]
    //    public bool LOGS { get; set; }

    //}
    public class TotalNumbers : BaseModel
    {
        public List<EvaluationCriteriaCategories> EvaluationCriteriaCategories { get; set; }
        public List<WowFactor> WowFactor { get; set; }
        public List<PhdCallScenario> PhdCallScenarios { get; set; }
    }
    public class RequestCallList : BaseModel
    {
        public string SURVEY_BY { get; set; }
        public int TIME_FRAME { get; set; }
        public DateTime? DATE_FROM { get; set; }
        public string DATE_FROM_STR { get; set; }
        public DateTime? DATE_TO { get; set; }
        public string DATE_TO_STR { get; set; }
        public string CALL_TYPE { get; set; }
        public int PHD_CALL_SCENARIO_ID { get; set; }
        public bool IS_READ_ONLY_MODE { get; set; }
        public int PAGE_NUMBER { get; set; }

    }

    public class FOX_TBL_SURVEY_AUDIT_SCORES : BaseModel
    {
        public long? SURVEY_CALL_ID { get; set; }
        public long? SURVEY_ID { get; set; }
        public long? GREETINGS { get; set; }
        public long? CALL_HANDLING_SKILLS { get; set; }
        public long? GRAMMER_PRONOUNCIATION_VOCABULARY { get; set; }
        public long? VERIFIED_PATIENT_ACCOUNT { get; set; }
        public long? ACCOUNT_AND_SCRIPT_READINESS { get; set; }
        public long? PATIENT_VERIFICATION_AND_INSURANCE { get; set; }
        public long? CASE_HANDLING { get; set; }
        public long? EMAIL_TO_STAKE_HOLDERS { get; set; }
        public long? ESCALATION { get; set; }
        public long? BONUS_POINTS { get; set; }
        public long? PERFORMANCE_KILLER { get; set; }
        public long? TOTAL_POINTS { get; set; }
        public string AUDITOR_NAME { get; set; }
        public string AGENT_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    public class RequestCallFromQA : BaseModel
    {
        public long? PRACTICE_CODE { get; set; }
        public string SEARCH_TEXT { get; set; }
        public string AGENT_NAME { get; set; }
        public string AUDITOR_NAME { get; set; }
        public DateTime? DATE_FROM { get; set; }
        public string DATE_FROM_STR { get; set; }
        public DateTime? DATE_TO { get; set; }
        public string DATE_TO_STR { get; set; }
        public int TIME_FRAME { get; set; }
        public string CALL_TYPE { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public string PATIENT_ACCOUNT_STR { get; set; }
    }
    public class FeedBackCaller : BaseModel
    {
        public string USER_NAME { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }

    }
    public class FeedBackCallerAndPhdCallReasons : BaseModel
    {
        public List<PhdCallReason> PhdCallReasons { get; set; }
        public List<FeedBackCaller> FeedBackCaller { get; set; }



    }

    public class CallLogModel : BaseModel
    {
        public long ID { get; set; }
        public long? PATIENT_ACCOUNT { get; set; }
        public string FILE_NAME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string LOGS { get; set; }
        public bool IS_AUDITED { get; set; }
        public string NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string MRN { get; set; }
        public int? PHD_CALL_SCENARIO_ID { get; set; }
        public string CALL_SCANARIO { get; set; }
        public string SCORING_CRITERIA { get; set; }
        public string SURVEY_FLAG { get; set; }
    }
    public class TeamMemberNameModel : BaseModel
    {
        public string USER_NAME { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }

    }
}
