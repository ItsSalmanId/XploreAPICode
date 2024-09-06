using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Security;
using FOX.ExternalServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using static FOX.DataModels.Models.AddBusiness.AddBusiness;
using static FOX.DataModels.Models.ConsentToCare.ConsentToCare;
using static FOX.DataModels.Models.SurveyAutomations.SurveyAutomations;
using System.Globalization;
using DocumentFormat.OpenXml.Spreadsheet;


namespace FOX.BusinessOperations.SurveyAutomationService
{
    public class SurveyAutomationService : ISurveyAutomationService
    {

        #region PROPERTIES
        private readonly DBContextSurveyAutomation _surveyAutomationContext = new DBContextSurveyAutomation();
        private readonly GenericRepository<PatientSurveyHistory> _patientSurveyHistoryRepository;
        private readonly GenericRepository<PatientSurvey> _patientSurveyRepository;
        private readonly GenericRepository<AutomatedSurveyUnSubscription> _automatedSurveyUnSubscription;
        private readonly GenericRepository<Patient> _patientRepository;
        private readonly GenericRepository<SurveyServiceLog> _surveyServiceLogRepository;
        public static string SurveyMethod = string.Empty;
        private string passPhrase = "2657894562368456";

        private readonly GenericRepository<UserAccount> _userAccountRepository;
        private readonly DBContextAddBusiness _addBusinessContext = new DBContextAddBusiness();
        private readonly GenericRepository<BusinessDetail> _businessDetailRepository;
        private readonly GenericRepository<BusinessBlogDetail> _businessBlogDetailRepository;
        private readonly GenericRepository<BusinessFilesDetail> _businessFilesDetailRepository;
        private readonly GenericRepository<BusinessRating> _businessRatingRepository;
        private readonly GenericRepository<UserProfileToken> _userProfileTokenRepository;
        private readonly GenericRepository<ReelsFilesDetails> _reelsFilesDetailsRepository;
        private readonly GenericRepository<ReelsDetails> _reelsDetailsRepository;
        private readonly GenericRepository<ReelsCommentsDetails> _reelsCommentsRepository;
        private readonly GenericRepository<UserFollowDetails> _userFollowRepository;


        #endregion

        #region CONSTRUCTOR
        public SurveyAutomationService()
        {
            _patientSurveyHistoryRepository = new GenericRepository<PatientSurveyHistory>(_surveyAutomationContext);
            _patientSurveyRepository = new GenericRepository<PatientSurvey>(_surveyAutomationContext);
            _automatedSurveyUnSubscription = new GenericRepository<AutomatedSurveyUnSubscription>(_surveyAutomationContext);
            _patientRepository = new GenericRepository<Patient>(_surveyAutomationContext);
            _surveyServiceLogRepository = new GenericRepository<SurveyServiceLog>(_surveyAutomationContext);
            _userAccountRepository = new GenericRepository<UserAccount>(_surveyAutomationContext);
            _businessDetailRepository = new GenericRepository<BusinessDetail>(_addBusinessContext);
            _businessFilesDetailRepository = new GenericRepository<BusinessFilesDetail>(_addBusinessContext);
            _businessBlogDetailRepository = new GenericRepository<BusinessBlogDetail>(_addBusinessContext);
            _businessRatingRepository = new GenericRepository<BusinessRating>(_addBusinessContext);
            _userProfileTokenRepository = new GenericRepository<UserProfileToken>(_addBusinessContext);
            _reelsFilesDetailsRepository = new GenericRepository<ReelsFilesDetails>(_addBusinessContext);
            _reelsDetailsRepository = new GenericRepository<ReelsDetails>(_addBusinessContext);
            _reelsCommentsRepository = new GenericRepository<ReelsCommentsDetails>(_addBusinessContext);
            _userFollowRepository = new GenericRepository<UserFollowDetails>(_addBusinessContext);
        }
        #endregion
        #region FUNCTIONS
        // Description: This function is decrypt patient account number & handle the flow of Unsubscribe Email & SMS
        public SurveyLink DecryptionUrl(SurveyLink objSurveyLink)
        {
            if (objSurveyLink != null && !string.IsNullOrEmpty(objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT))
            {
                var decryptionSurveyId = Decryption(objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT.ToString());
               objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT = decryptionSurveyId;
                if (!string.IsNullOrEmpty(objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT))
                {
                    var decryptionSurveyMethod = Decryption(objSurveyLink.SURVEY_METHOD.ToString());
                    string surveyMethod = decryptionSurveyMethod;
                    if (!string.IsNullOrEmpty(surveyMethod))
                    {
                        if (surveyMethod == "SMS" || surveyMethod == "EMAIL")
                        {
                            objSurveyLink.OPEN_SURVEY_METHOD = surveyMethod;
                        }
                        if (!string.IsNullOrEmpty(objSurveyLink.SURVEY_METHOD) && (surveyMethod == "UnsubscribeEmail" || surveyMethod == "UnsubscribeSMS"))
                        {
                            var surveyId = long.Parse(objSurveyLink.ENCRYPTED_PATIENT_ACCOUNT);
                            var existingPatientDetails = _patientSurveyRepository.GetFirst(r => r.SURVEY_ID == surveyId && r.DELETED == false);
                            if (existingPatientDetails != null && existingPatientDetails.PATIENT_ACCOUNT_NUMBER != 0)
                            {
                                var patientAccount = long.Parse(existingPatientDetails.PATIENT_ACCOUNT_NUMBER.ToString());
                                string charId = patientAccount.ToString();
                                var patientDetails = _patientRepository.GetFirst(r => r.Chart_Id == "00" + charId && r.DELETED == false);
                                if (surveyMethod == "UnsubscribeEmail")
                                {
                                    var unsubscribeEmailDetail = _automatedSurveyUnSubscription.GetFirst(r => r.PATIENT_ACCOUNT == patientAccount && r.EMAIL_UNSUBSCRIBE == true && r.DELETED == false);
                                    if (unsubscribeEmailDetail == null)
                                    {
                                        AutomatedSurveyUnSubscription objAutomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                                        objAutomatedSurveyUnSubscription = _automatedSurveyUnSubscription.GetFirst(r => r.PATIENT_ACCOUNT == patientAccount && r.DELETED == false);
                                        if (objAutomatedSurveyUnSubscription == null)
                                        {
                                            AutomatedSurveyUnSubscription objAutomatedSurveyUnSubscriptions = new AutomatedSurveyUnSubscription
                                            {
                                                AUTOMATED_SURVEY_UNSUBSCRIPTION_ID = Helper.getMaximumId("AUTOMATED_SURVEY_UNSUBSCRIPTION_ID"),
                                                PATIENT_ACCOUNT = patientAccount,
                                                PRACTICE_CODE = AppConfiguration.GetPracticeCode,
                                                SMS_UNSUBSCRIBE = false,
                                                EMAIL_UNSUBSCRIBE = true,
                                                SURVEY_ID = surveyId,
                                                CREATED_DATE = Helper.GetCurrentDate(),
                                                CREATED_BY = "FOX_TEAM"
                                            };
                                            _automatedSurveyUnSubscription.Insert(objAutomatedSurveyUnSubscriptions);
                                            _automatedSurveyUnSubscription.Save();
                                        }
                                        else
                                        {
                                            objAutomatedSurveyUnSubscription.EMAIL_UNSUBSCRIBE = true;
                                            _automatedSurveyUnSubscription.Update(objAutomatedSurveyUnSubscription);
                                            _automatedSurveyUnSubscription.Save();
                                        }
                                        objSurveyLink.SURVEY_METHOD = "Email Unsubscribe";
                                        // Get List of CC Users
                                        var cc = GetEmailCCList();
                                        // Get List of BCC Users
                                        var bcc = GetEmailBCCList();
                                        var emailBody = unsubscribeEmailBody(existingPatientDetails.PATIENT_FIRST_NAME);
                                        SendEmail(patientDetails.Email_Address, "FOX Patient Survey", emailBody, cc, bcc);
                                    }
                                    else
                                    {
                                        objSurveyLink.SURVEY_METHOD = "Link Expire";
                                    }
                                }
                                if (surveyMethod == "UnsubscribeSMS")
                                {
                                    var unsubscribeSMSDetail = _automatedSurveyUnSubscription.GetFirst(r => r.PATIENT_ACCOUNT == patientAccount && r.SMS_UNSUBSCRIBE == true && r.DELETED == false);
                                    if (unsubscribeSMSDetail == null)
                                    {
                                        AutomatedSurveyUnSubscription objAutomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                                        objAutomatedSurveyUnSubscription = _automatedSurveyUnSubscription.GetFirst(r => r.PATIENT_ACCOUNT == patientAccount && r.DELETED == false);
                                        if (objAutomatedSurveyUnSubscription == null)
                                        {
                                            AutomatedSurveyUnSubscription objAutomatedSurveyUnSubscriptions = new AutomatedSurveyUnSubscription
                                            {
                                                AUTOMATED_SURVEY_UNSUBSCRIPTION_ID = Helper.getMaximumId("AUTOMATED_SURVEY_UNSUBSCRIPTION_ID"),
                                                PATIENT_ACCOUNT = patientAccount,
                                                PRACTICE_CODE = AppConfiguration.GetPracticeCode,
                                                SMS_UNSUBSCRIBE = true,
                                                EMAIL_UNSUBSCRIBE = false,
                                                SURVEY_ID = surveyId,
                                                CREATED_DATE = Helper.GetCurrentDate(),
                                                CREATED_BY = "FOX_TEAM"
                                            };
                                            _automatedSurveyUnSubscription.Insert(objAutomatedSurveyUnSubscriptions);
                                            _automatedSurveyUnSubscription.Save();
                                        }
                                        else
                                        {
                                            objAutomatedSurveyUnSubscription.SMS_UNSUBSCRIBE = true;
                                            _automatedSurveyUnSubscription.Update(objAutomatedSurveyUnSubscription);
                                            _automatedSurveyUnSubscription.Save();
                                        }

                                        var smsBody = SmsBody(patientDetails.FirstName);
                                        var status = SmsService.SMSTwilio(patientDetails.Home_Phone, smsBody);
                                        objSurveyLink.SURVEY_METHOD = "SMS Unsubscribe";
                                    }
                                    else
                                    {
                                        objSurveyLink.SURVEY_METHOD = "Link Expire";
                                    }
                                }
                            }
                        }
                        else
                        {
                            objSurveyLink.SURVEY_METHOD = null;
                        }
                    }
                    else
                    {
                        objSurveyLink.SURVEY_METHOD = "Link Expire";
                    }
                }
                else
                {
                    objSurveyLink = null;
                }
            }
            return objSurveyLink;
        }
        // Description: This function is get survey method details
        public static string GetSurveyMethod(string surveyMethodRemoveChar)
        {
            switch (surveyMethodRemoveChar)
            {
                case "1":
                    surveyMethodRemoveChar = "SMS";
                    break;
                case "2":
                    surveyMethodRemoveChar = "EMAIL";
                    break;
                case "3":
                    surveyMethodRemoveChar = "UnsubscribeSMS";
                    break;
                case "4":
                    surveyMethodRemoveChar = "UnsubscribeEmail";
                    break;
                default:
                    surveyMethodRemoveChar = "";
                    break;
            }
            return surveyMethodRemoveChar;
        }

        // Description: This function is get patient details
        public SurveyAutomation GetPatientDetails(SurveyAutomation objSurveyAutomation)
        {
            if (objSurveyAutomation != null && objSurveyAutomation.PATIENT_ACCOUNT != 0)
            {
                long tempSurveyId = objSurveyAutomation.PATIENT_ACCOUNT;
                var existingPatientDetails = _patientSurveyRepository.GetFirst(r => r.SURVEY_ID == tempSurveyId && r.DELETED == false);
                if (existingPatientDetails != null && existingPatientDetails.PATIENT_ACCOUNT_NUMBER != null)
                {
                    long getPracticeCode = AppConfiguration.GetPracticeCode;
                    SqlParameter pracCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = getPracticeCode };
                    SqlParameter patientAccountNumber = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = existingPatientDetails.PATIENT_ACCOUNT_NUMBER };
                    SqlParameter surveyId = new SqlParameter { ParameterName = "@SURVEY_ID", SqlDbType = SqlDbType.BigInt, Value = tempSurveyId };
                    var performSurveyHistory = SpRepository<SurveyServiceLog>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_PERFORM_SURVEY_PATIENT_DETAILS @PATIENT_ACCOUNT, @PRACTICE_CODE, @SURVEY_ID", patientAccountNumber, pracCode, surveyId);
                    if (performSurveyHistory != null)
                    {
                        objSurveyAutomation = null;
                    }
                    else
                    {
                        SqlParameter practiceCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = getPracticeCode };
                        SqlParameter patientAccount = new SqlParameter { ParameterName = "@PATIENT_ACCOUNT", SqlDbType = SqlDbType.BigInt, Value = existingPatientDetails.PATIENT_ACCOUNT_NUMBER };
                        SqlParameter surveyIdd = new SqlParameter { ParameterName = "@SURVEY_ID", SqlDbType = SqlDbType.BigInt, Value = tempSurveyId };
                        objSurveyAutomation = SpRepository<SurveyAutomation>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_SURVEY_PATIENT_DETAILS @PATIENT_ACCOUNT, @PRACTICE_CODE, @SURVEY_ID", patientAccount, practiceCode, surveyIdd);
                    }
                }
                else
                {
                    objSurveyAutomation = null;
                }
            }
            else
            {
                objSurveyAutomation = null;
            }
            return objSurveyAutomation;
        }
        // Description: This function is trigger to update patient survey model
        public ResponseModel UpdatePatientSurvey(PatientSurvey objPatientSurvey)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (objPatientSurvey != null && objPatientSurvey.PATIENT_ACCOUNT_NUMBER != 0)
                {
                    var existingSurveyDetails = _patientSurveyRepository.GetFirst(r => r.PATIENT_ACCOUNT_NUMBER == objPatientSurvey.PATIENT_ACCOUNT_NUMBER && r.SURVEY_ID == objPatientSurvey.SURVEY_ID && r.IS_SURVEYED == true && r.DELETED == false);
                    if (existingSurveyDetails == null)
                    {
                        SurveyServiceLog objSurveyServiceLog = new SurveyServiceLog();
                        long practiceCode = AppConfiguration.GetPracticeCode;
                        objSurveyServiceLog = _surveyServiceLogRepository.GetFirst(r => r.PATIENT_ACCOUNT == objPatientSurvey.PATIENT_ACCOUNT_NUMBER && r.SURVEY_ID == objPatientSurvey.SURVEY_ID && r.PRACTICE_CODE == practiceCode && r.DELETED == false);
                        if (objSurveyServiceLog != null)
                        {
                            if (objPatientSurvey.IS_SMS == true)
                            {
                                objSurveyServiceLog = _surveyServiceLogRepository.GetByID(objSurveyServiceLog.SURVEY_AUTOMATION_LOG_ID);
                                objSurveyServiceLog.IS_SMS = true;
                                objSurveyServiceLog.IS_EMAIL = false;
                                objSurveyServiceLog.IS_PERFORMED_SURVEY = true;
                                _surveyServiceLogRepository.Update(objSurveyServiceLog);
                                _surveyServiceLogRepository.Save();
                            }
                            else
                            {
                                objSurveyServiceLog = _surveyServiceLogRepository.GetByID(objSurveyServiceLog.SURVEY_AUTOMATION_LOG_ID);
                                objSurveyServiceLog.IS_SMS = false;
                                objSurveyServiceLog.IS_EMAIL = true;
                                objSurveyServiceLog.IS_PERFORMED_SURVEY = true;
                                _surveyServiceLogRepository.Update(objSurveyServiceLog);
                                _surveyServiceLogRepository.Save();
                            }
                        }
                        var existingPatientDetails = _patientSurveyRepository.GetFirst(r => r.PATIENT_ACCOUNT_NUMBER == objPatientSurvey.PATIENT_ACCOUNT_NUMBER && r.SURVEY_ID == objPatientSurvey.SURVEY_ID && r.DELETED == false);
                        PatientSurvey patientSurvey = new PatientSurvey();
                        if (existingPatientDetails != null)
                        {
                            objPatientSurvey.SURVEY_ID = existingPatientDetails.SURVEY_ID;
                            AddPatientSurvey(objPatientSurvey);
                            existingPatientDetails.IS_CONTACT_HQ = objPatientSurvey.IS_CONTACT_HQ;
                            existingPatientDetails.IS_REFERABLE = objPatientSurvey.IS_REFERABLE;
                            existingPatientDetails.IS_IMPROVED_SETISFACTION = objPatientSurvey.IS_IMPROVED_SETISFACTION;
                            existingPatientDetails.FEEDBACK = objPatientSurvey.FEEDBACK;
                            existingPatientDetails.SURVEY_STATUS_BASE = "Completed";
                            existingPatientDetails.SURVEY_STATUS_CHILD = "Completed Survey";
                            existingPatientDetails.MODIFIED_BY = "Automation_5483326";
                            existingPatientDetails.IS_SURVEYED = true;
                            existingPatientDetails.IN_PROGRESS = false;
                            existingPatientDetails.SURVEY_FORMAT_TYPE = "New Format";
                            existingPatientDetails.SURVEY_COMPLETED_DATE = Helper.GetCurrentDate();
                            existingPatientDetails.MODIFIED_DATE = Helper.GetCurrentDate();
                            existingPatientDetails.DELETED = false;
                            if (objPatientSurvey.IS_REFERABLE == true && objPatientSurvey.IS_REFERABLE != null)
                            {
                                existingPatientDetails.SURVEY_FLAG = "Green";
                            }
                            else
                            {
                                existingPatientDetails.SURVEY_FLAG = "Red";
                            }
                            _patientSurveyRepository.Update(existingPatientDetails);
                            _patientSurveyRepository.Save();
                            response.ErrorMessage = "";
                            response.Message = "Suvery completed successfully";
                            response.Success = true;
                        }
                        else
                        {
                            response.ErrorMessage = "";
                            response.Message = "Suvery not completed successfully";
                            response.Success = false;
                        }
                    }
                    else
                    {
                        SurveyServiceLog objSurveyServiceLog = new SurveyServiceLog();
                        long practiceCode = AppConfiguration.GetPracticeCode;
                        objSurveyServiceLog = _surveyServiceLogRepository.GetFirst(r => r.PATIENT_ACCOUNT == objPatientSurvey.PATIENT_ACCOUNT_NUMBER && r.SURVEY_ID == objPatientSurvey.SURVEY_ID && r.PRACTICE_CODE == practiceCode && r.DELETED == false);
                        if (objSurveyServiceLog != null)
                        {
                            if (objSurveyServiceLog.IS_SMS == true && objSurveyServiceLog != null)
                            {
                                response.ErrorMessage = "";
                                response.Message = "Suvery completed via SMS";
                                response.Success = false;
                            }
                            else if (objSurveyServiceLog.IS_EMAIL == true && objSurveyServiceLog != null)
                            {
                                response.ErrorMessage = "";
                                response.Message = "Suvery completed via Email";
                                response.Success = false;
                            }
                        }
                        else
                        {
                            response.ErrorMessage = "";
                            response.Message = "";
                            response.Success = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        // Description: This function is trigger to add patient survey model
        private void AddPatientSurvey(PatientSurvey objPatientSurvey)
        {
            if (objPatientSurvey != null)
            {
                long practiceCode = AppConfiguration.GetPracticeCode;
                PatientSurveyHistory patientSurveyHistory = new PatientSurveyHistory
                {
                    SURVEY_HISTORY_ID = Helper.getMaximumId("SURVEY_HISTORY_ID"),
                    SURVEY_ID = objPatientSurvey.SURVEY_ID,
                    PATIENT_ACCOUNT = objPatientSurvey.PATIENT_ACCOUNT_NUMBER,
                    PRACTICE_CODE = practiceCode,
                    IS_CONTACT_HQ = objPatientSurvey.IS_CONTACT_HQ,
                    IS_REFERABLE = objPatientSurvey.IS_REFERABLE,
                    IS_IMPROVED_SETISFACTION = objPatientSurvey.IS_IMPROVED_SETISFACTION,
                    FEEDBACK = objPatientSurvey.FEEDBACK,
                    SURVEY_STATUS_BASE = "Completed",
                    SURVEY_STATUS_CHILD = "Completed Survey",
                    DELETED = false,
                    CREATED_BY = "Automation_5483326",
                    SURVEY_DATE = Helper.GetCurrentDate(),
                    CREATED_DATE = Helper.GetCurrentDate()
                };
                if (objPatientSurvey.IS_REFERABLE == true && objPatientSurvey.IS_REFERABLE != null)
                {
                    patientSurveyHistory.SURVEY_FLAG = "Green";
                }
                else
                {
                    patientSurveyHistory.SURVEY_FLAG = "Red";
                }
                _patientSurveyHistoryRepository.Insert(patientSurveyHistory);
                _patientSurveyHistoryRepository.Save();
            }
        }
        #endregion
        #region Email & SMS body 
        // Description: This function is used for Email body
        public static string unsubscribeEmailBody(string patientFirstName)
        {
            string mailBody = string.Empty;
            string templatePathOfSenderEmail = AppDomain.CurrentDomain.BaseDirectory;
            templatePathOfSenderEmail = templatePathOfSenderEmail.Replace(@"\bin\Debug", "") + "HtmlTemplates\\UnsubscribeAutomatedPatientSurveyEmailTemplate.html";
            if (File.Exists(templatePathOfSenderEmail))
            {
                mailBody = File.ReadAllText(templatePathOfSenderEmail);
                mailBody = mailBody.Replace("[[PATIENT_FIRST_NAME]]", patientFirstName);
            }
            return mailBody ?? "";
        }
        public static string subscribeEmailBody(string patientFirstName)
        {
            string mailBody = string.Empty;
            string templatePathOfSenderEmail = AppDomain.CurrentDomain.BaseDirectory;
            templatePathOfSenderEmail = templatePathOfSenderEmail.Replace(@"\bin\Debug", "") + "HtmlTemplates\\SubscribeAutomatedPatientSurveyEmailTemplate.html";
            if (File.Exists(templatePathOfSenderEmail))
            {
                mailBody = File.ReadAllText(templatePathOfSenderEmail);
                mailBody = mailBody.Replace("[[PATIENT_FIRST_NAME]]", patientFirstName);
            }
            return mailBody ?? "";
        }
        // Description: This function is used for send email
        public static bool SendEmail(string to, string subject, string body, List<string> CC = null, List<string> BCC = null, string AttachmentFilePaths = null, string from = "foxrehab@carecloud.com")
        {
            bool IsMailSent = false;
            var bodyHTML = "";
            bodyHTML += "<body>";
            bodyHTML += body;
            bodyHTML += "</body>";
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(from);
                        mail.To.Add(new MailAddress(to));
                        mail.Subject = subject;
                        mail.Body = bodyHTML;
                        mail.IsBodyHtml = true;
                        mail.SubjectEncoding = Encoding.UTF8;
                        if (CC != null && CC.Count > 0)
                        {
                            foreach (var item in CC) { mail.CC.Add(item); }
                        }
                        if (BCC != null && BCC.Count > 0)
                        {
                            foreach (var item in BCC) { mail.Bcc.Add(item); }
                        }
                        if (AttachmentFilePaths != null)
                        {
                            if (File.Exists(AttachmentFilePaths)) { mail.Attachments.Add(new Attachment(AttachmentFilePaths)); }
                        }
                        smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["FoxRehabUserName"], ConfigurationManager.AppSettings["FoxRehabPassword"]);
                        smtp.Send(mail);
                        IsMailSent = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IsMailSent;
        }
        // Description: This function is used for get list of Email CC
        public static List<string> GetEmailCCList()
        {
            List<string> cc = new List<string>();
            var ccUsers = ConfigurationManager.AppSettings?["SurveyAutomationCCList"].ToString();
            if (!string.IsNullOrEmpty(ccUsers))
            {
                cc = ccUsers.Split(',').ToList();
            }
            return cc ?? null;
        }
        // Description: This function is used for get list of Email BCC
        public static List<string> GetEmailBCCList()
        {
            List<string> bcc = new List<string>();
            var ccUsers = ConfigurationManager.AppSettings?["SurveyAutomationBCCList"].ToString();
            if (!string.IsNullOrEmpty(ccUsers))
            {
                bcc = ccUsers.Split(',').ToList();
            }
            return bcc ?? null;
        }
        // Description: This function is used forcreate SMS body
        public static string SmsBody(string patientFirstName)
        {
            string smsBody = "Hello " + patientFirstName + "!\n \nYour request to unsubscribe from receiving patient surveys is received. You will not receive any messages with patient survey link in future.\n\nRegards\n\nFox Rehab Team ";
            return smsBody ?? "";
        }
        #endregion
        private byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }
        public string Decryption(string encryptedText)
        {

            try
            {
                var encryptedBytes = Convert.FromBase64String(encryptedText);
                return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(passPhrase)));
            }
            catch (Exception ex)
            {
                return "exception : " + ex.Message;
            }

        }
        // To handle encryption/decryption java,C#
        private static RijndaelManaged GetRijndaelManaged(string secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }
        public static string SmsBody(string patientFirstName, string link, string unsubscribe, string clinicianName)
        {
            string smsBody = "Hello " + patientFirstName + "!" + "\nTo continue providing the very best care for our patients, your opinion regarding your recent service from " + clinicianName + " is very important to us. Please take a few moments and complete our online survey. All information provided will be kept confidential.\nThank you for your participation.\nPlease Tap on the link below to start the surey.\n\n" + link + "\n\nUnsubscribe\n\n" + unsubscribe + "\n\nRegards\nFox Rehab Team";
            return smsBody ?? "";
        }
        // Description: This function is trigger to update patient survey model
        public ResponseModel SendPatientSurveyLink(PatientSurvey objPatientSurvey, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var cc = GetEmailCCList();
                // Get List of BCC Users
                var bcc = GetEmailBCCList();
                var encryptedSMSURL = GenerateEncryptedURL(objPatientSurvey.SURVEY_ID, "SMS");
                var unsubscribeSmsencryptedURL = GenerateEncryptedURL(objPatientSurvey.SURVEY_ID, "UnsubscribeSMS");
                var encryptedEmailURL = GenerateEncryptedURL(objPatientSurvey.SURVEY_ID, "EMAIL");
                var unsubscribeEmailencryptedURL = GenerateEncryptedURL(objPatientSurvey.SURVEY_ID, "UnsubscribeEmail");
                var smsBody = SurveySmsBody(objPatientSurvey.PATIENT_FIRST_NAME, encryptedSMSURL, unsubscribeSmsencryptedURL, objPatientSurvey.PROVIDER);

                if (objPatientSurvey.SurveyLinkSendVia == "SMS")
                {
                    objPatientSurvey.IS_SMS = true;
                    objPatientSurvey.IS_EMAIL = objPatientSurvey.IS_EMAIL == null ? false : objPatientSurvey.IS_EMAIL;
                    
                    Thread sendSmsThread = new Thread(() =>
                    {
                        //var status = SmsService.SMSTwilio(objPatientSurvey.PATIENT_CELL_NUMBER, smsBody);
                    });
                    sendSmsThread.Start();
                }
                else if (objPatientSurvey.SurveyLinkSendVia == "EMAIL")
                {
                    objPatientSurvey.IS_SMS = objPatientSurvey.IS_SMS == null ? false : objPatientSurvey.IS_SMS;
                    objPatientSurvey.IS_EMAIL = true;
                    Thread sendEmailThread = new Thread(() =>
                    {
                        var emailBody = EmailBody(objPatientSurvey.PATIENT_FIRST_NAME, encryptedEmailURL, unsubscribeEmailencryptedURL, objPatientSurvey.PROVIDER);
                        SendSurveyEmail(objPatientSurvey.PatientEmail, "FOX Patient Survey", emailBody, cc, bcc);
                    });
                    sendEmailThread.Start();
                }
                else if (objPatientSurvey.SurveyLinkSendVia == "BOTH")
                {
                    objPatientSurvey.IS_SMS = true;
                    objPatientSurvey.IS_EMAIL = true;
                    
                    Thread sendEmailSmsThread = new Thread(() =>
                    {
                        var emailBody = EmailBody(objPatientSurvey.PATIENT_FIRST_NAME, encryptedEmailURL, unsubscribeEmailencryptedURL, objPatientSurvey.PROVIDER);
                        SendSurveyEmail(objPatientSurvey.PatientEmail, "FOX Patient Survey", emailBody, cc, bcc);
                        //var smsBody = SmsBody(objPatientSurvey.PATIENT_FIRST_NAME, encryptedSMSURL, unsubscribeSmsencryptedURL, objPatientSurvey.PROVIDER);
                        //var status = SmsService.SMSTwilio(objPatientSurvey.PatientHomeNumber, smsBody);
                    });
                    sendEmailSmsThread.Start();
                    //var status = SmsService.SMSTwilio(objPatientSurvey.PATIENT_CELL_NUMBER, smsBody);
                    //var isSendSms = SmsService.SMSTwilio(objPatientSurvey.PATIENT_CELL_NUMBER, smsBody);

                }

                SurveyServiceLog surveyServiceLogDetails = new SurveyServiceLog();
                long practiceCode = AppConfiguration.GetPracticeCode;
                surveyServiceLogDetails = _surveyServiceLogRepository.GetFirst(r => r.PATIENT_ACCOUNT == objPatientSurvey.PATIENT_ACCOUNT_NUMBER && r.SURVEY_ID == objPatientSurvey.SURVEY_ID && r.PRACTICE_CODE == practiceCode && r.DELETED == false);
                if (surveyServiceLogDetails != null)
                {

                    surveyServiceLogDetails = _surveyServiceLogRepository.GetByID(surveyServiceLogDetails.SURVEY_AUTOMATION_LOG_ID);
                    //surveyServiceLogDetails.IS_PERFORMED_SURVEY = true;
                    if (objPatientSurvey.IS_SMS == true)
                    {
                        surveyServiceLogDetails.IS_SMS = objPatientSurvey.IS_SMS == null ? false : objPatientSurvey.IS_SMS;
                    }
                    if (objPatientSurvey.IS_EMAIL == true)
                    {
                        surveyServiceLogDetails.IS_EMAIL = objPatientSurvey.IS_EMAIL == null ? false : objPatientSurvey.IS_EMAIL;
                    }
                    //surveyServiceLogDetails.IS_EMAIL = objPatientSurvey.IS_EMAIL;
                    //surveyServiceLogDetails.IS_SMS = objPatientSurvey.IS_SMS;
                    surveyServiceLogDetails.IS_PERFORMED_SURVEY = objPatientSurvey.IS_PERFORMED_SURVEY;
                    surveyServiceLogDetails.MODIFIED_BY = profile.UserName;

                    surveyServiceLogDetails.MODIFIED_DATE = GetDateTime();
                    //surveyServiceLogDetails.MODIFIED_DATE = Helper.GetCurrentDate(); 

                    _surveyServiceLogRepository.Update(surveyServiceLogDetails);
                    _surveyServiceLogRepository.Save();
                    if (objPatientSurvey.IS_EMAIL == true && objPatientSurvey.IS_SMS == false)
                    {
                        response.Message = "Survey link has been successfully sent to patient via Email.";
                    }
                    else if (objPatientSurvey.IS_SMS == true && objPatientSurvey.IS_EMAIL == false)
                    {
                        response.Message = "Survey link has been successfully sent to patient via SMS.";
                    }
                    else if (objPatientSurvey.IS_EMAIL == true && objPatientSurvey.IS_SMS == true)
                    {
                        response.Message = "Survey link has been successfully sent to patient via Email and SMS.";
                    }
                }
                else
                {
                    SurveyServiceLog objSurveyServiceLog = new SurveyServiceLog();
                    objSurveyServiceLog.SURVEY_AUTOMATION_LOG_ID = Helper.getMaximumId("SURVEY_AUTOMATION_LOG_ID");
                    objSurveyServiceLog.SURVEY_ID = objPatientSurvey.SURVEY_ID;
                    objSurveyServiceLog.PATIENT_ACCOUNT = objPatientSurvey.PATIENT_ACCOUNT_NUMBER;
                    objSurveyServiceLog.FILE_NAME = objPatientSurvey.FILE_NAME;
                    objSurveyServiceLog.IS_EMAIL = objPatientSurvey.IS_EMAIL == null ? false : objPatientSurvey.IS_EMAIL;
                    objSurveyServiceLog.IS_SMS = objPatientSurvey.IS_SMS == null ? false : objPatientSurvey.IS_SMS;
                    objSurveyServiceLog.IS_PERFORMED_SURVEY = objPatientSurvey.IS_PERFORMED_SURVEY;
                    objSurveyServiceLog.PRACTICE_CODE = AppConfiguration.GetPracticeCode;
                    objSurveyServiceLog.CREATED_BY = objSurveyServiceLog.MODIFIED_BY = profile.UserName;
                    objSurveyServiceLog.CREATED_DATE = objSurveyServiceLog.MODIFIED_DATE = GetDateTime();
                    _surveyServiceLogRepository.Insert(objSurveyServiceLog);
                    _surveyServiceLogRepository.Save();
                    if (objPatientSurvey.IS_EMAIL == true && objPatientSurvey.IS_SMS == false)
                    {
                        response.Message = "Survey link has been successfully sent to patient via Email.";
                    }
                    else if (objPatientSurvey.IS_SMS == true && objPatientSurvey.IS_EMAIL == false)
                    {
                        response.Message = "Survey link has been successfully sent to patient via SMS.";
                    }
                    else if (objPatientSurvey.IS_EMAIL == true && objPatientSurvey.IS_SMS == true)
                    {
                        response.Message = "Survey link has been successfully sent to patient via Email and SMS.";
                    }
                }

                var temp = objPatientSurvey.SubscriptionOption;
                if (objPatientSurvey.SubscriptionOption == "EMAIL")
                {
                    objPatientSurvey.SubscriptionOption = "Subscribe";
                    objPatientSurvey.SurveyLinkSendVia = "SubscriptionEmail";
                    SubscribleUnsubscribleSurvey(objPatientSurvey, profile);
                    response.Message = "Survey link has been sent via SMS, and the status has been updated to Subscribed for Email delivery.";
                }
                else if (objPatientSurvey.SubscriptionOption == "SMS")
                {
                    objPatientSurvey.SubscriptionOption = "Subscribe";
                    objPatientSurvey.SurveyLinkSendVia = "SubscriptionSms";
                    SubscribleUnsubscribleSurvey(objPatientSurvey, profile);
                    response.Message = "Survey link has been sent via SMS, and the status has been updated to Subscribed for SMS delivery.";
                }
                else if (objPatientSurvey.SubscriptionOption == "BOTH")
                {
                    objPatientSurvey.SubscriptionOption = "Subscribe";
                    objPatientSurvey.SurveyLinkSendVia = "SubscriptionBoth";
                    SubscribleUnsubscribleSurvey(objPatientSurvey, profile);
                    response.Message = "Survey link has been sent via SMS, and the status has been updated to Subscribed for SMS and Email delivery.";
                }
                response.ErrorMessage = "";
                //response.Message = "Patient has been successfully Unsubscribed to receive survey link(s) via SMS";
                //response.Message = "Patient has been successfully subscribed to receive survey link(s) via SMS";
                response.Success = true;


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        public DateTime GetDateTime()
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            // Get the current time in UTC
            DateTime utcNow = DateTime.UtcNow;

            // Convert the current UTC time to Eastern Time
            DateTime newJerseyTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, easternZone);

            // Set the variable
            DateTime currentTimeInNewJersey = newJerseyTime;

            // Print the current time in New Jersey
            return newJerseyTime;
        }
        public static string GenerateEncryptedURL(long surveyId, string type)
        {
            string encryptedUrl = string.Empty;
            try
            {
                if (surveyId != 0)
                {
                    var encyptedSurveyId = Encryption(surveyId.ToString());
                    var surveyMethod = Encryption(type);
                    string environmentURL = ConfigurationManager.AppSettings?["ClientURL"].ToString();
                    encryptedUrl = environmentURL + "?" + surveyMethod + "&" + encyptedSurveyId;
                }
            }
            catch (Exception ex)
            {
                return encryptedUrl;
            }
            return encryptedUrl;
        }
        public static string Encryption(string plainText)
        {
            try
            {
                // To handle encryption/decryption Objective-C,C#
                string passPhrase = "2657894562368456";
                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(passPhrase)));
            }
            catch (Exception ex)
            {
                return "exception : " + ex.Message;
            }
        }
        private static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }
        public static string SurveySmsBody(string patientFirstName, string link, string unsubscribe, string clinicianName)
        {
            string smsBody = "Hello " + patientFirstName + "!" + "\nTo continue providing the very best care for our patients, your opinion regarding your recent service from " + clinicianName + " is very important to us. Please take a few moments and complete our online survey. All information provided will be kept confidential.\nThank you for your participation.\nPlease Tap on the link below to start the surey.\n\n" + link + "\n\nUnsubscribe\n\n" + unsubscribe + "\n\nRegards\nFox Rehab Team";
            return smsBody ?? "";
        }
        public static string EmailBody(string patientFirstName, string link, string unsubscribeencryptedURL, string clinicianName)
        {
            string mailBody = string.Empty;
            try
            {
                string templatePathOfSenderEmail = AppDomain.CurrentDomain.BaseDirectory;
                templatePathOfSenderEmail = templatePathOfSenderEmail.Replace(@"\bin\Debug", "") + "HtmlTemplates\\InvitePatientSurveyTemplate.html";
                if (File.Exists(templatePathOfSenderEmail))
                {
                    mailBody = File.ReadAllText(templatePathOfSenderEmail);
                    mailBody = mailBody.Replace("[[PATIENT_FIRST_NAME]]", patientFirstName);
                    mailBody = mailBody.Replace("[[LINK]]", link);
                    mailBody = mailBody.Replace("[[UNSUBSCRIBE_LINK]]", unsubscribeencryptedURL);
                    mailBody = mailBody.Replace("[[CLINICIAN_NAME]]", clinicianName);
                }
            }
            catch (Exception ex)
            {
                return mailBody ?? "";
            }
            return mailBody ?? "";
        }
        public static void SendSurveyEmail(string to, string subject, string body, List<string> CC = null, List<string> BCC = null, string AttachmentFilePaths = null, string from = "foxrehab@carecloud.com")
        {
            var bodyHTML = "";
            bodyHTML += "<body>";
            bodyHTML += body;
            bodyHTML += "</body>";
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(from);
                        mail.To.Add(new MailAddress(to));
                        mail.Subject = subject;
                        mail.Body = bodyHTML;
                        mail.IsBodyHtml = true;
                        mail.SubjectEncoding = Encoding.UTF8;
                        if (CC != null && CC.Count > 0)
                        {
                            foreach (var item in CC) { mail.CC.Add(item); }
                        }
                        if (BCC != null && BCC.Count > 0)
                        {
                            foreach (var item in BCC) { mail.Bcc.Add(item); }
                        }
                        if (AttachmentFilePaths != null)
                        {
                            if (File.Exists(AttachmentFilePaths)) { mail.Attachments.Add(new Attachment(AttachmentFilePaths)); }
                        }
                        smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["FoxRehabUserName"], ConfigurationManager.AppSettings["FoxRehabPassword"]);
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            //return IsMailSent;
        }
        public void sendUnsubscribeEmail(PatientSurvey objPatientSurvey)
        {
            // Get List of CC Users
            var cc = GetEmailCCList();
            // Get List of BCC Users
            var bcc = GetEmailBCCList();
            var emailBody = unsubscribeEmailBody(objPatientSurvey.PATIENT_FIRST_NAME);
            SendEmail(objPatientSurvey.PatientEmail, "FOX Patient Survey", emailBody, cc, bcc);
        }
        public void sendSubscribleEmail(PatientSurvey objPatientSurvey)
        {
            // Get List of CC Users
            var cc = GetEmailCCList();
            // Get List of BCC Users
            var bcc = GetEmailBCCList();
            var emailBody = subscribeEmailBody(objPatientSurvey.PATIENT_FIRST_NAME);
            SendEmail(objPatientSurvey.PatientEmail, "FOX Patient Survey", emailBody, cc, bcc);
        }

        // Description: This function is trigger to update patient survey model
        public ResponseModel SubscribleUnsubscribleSurvey(PatientSurvey objPatientSurvey, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var subscriptionEmail = false;
                var subscriptionSms = false;
                if (objPatientSurvey.SurveyLinkSendVia == "SubscriptionEmail")
                {
                    subscriptionEmail = true;
                }
                else if(objPatientSurvey.SurveyLinkSendVia == "SubscriptionSms")
                {
                       subscriptionSms = true;
                }
                
                    AutomatedSurveyUnSubscription automatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                long practiceCode = AppConfiguration.GetPracticeCode;
                automatedSurveyUnSubscription = _automatedSurveyUnSubscription.GetFirst(r => r.PATIENT_ACCOUNT == objPatientSurvey.PATIENT_ACCOUNT_NUMBER  && r.PRACTICE_CODE == practiceCode && r.DELETED == false);
                if (objPatientSurvey.SubscriptionOption == "Unsubscrible")
                {

                    if (automatedSurveyUnSubscription != null)
                    {

                        automatedSurveyUnSubscription = _automatedSurveyUnSubscription.GetByID(automatedSurveyUnSubscription.AUTOMATED_SURVEY_UNSUBSCRIPTION_ID);
                        //if (automatedSurveyUnSubscription.SMS_UNSUBSCRIBE == false && subscriptionSms == true)
                        //{
                        //    automatedSurveyUnSubscription.SMS_UNSUBSCRIBE = subscriptionSms;
                        //}
                        //else
                        //{
                        //    automatedSurveyUnSubscription.SMS_UNSUBSCRIBE = true;
                        //}
                        //if (automatedSurveyUnSubscription.EMAIL_UNSUBSCRIBE == false && subscriptionEmail == true)
                        //{
                        //    automatedSurveyUnSubscription.EMAIL_UNSUBSCRIBE = subscriptionEmail;
                        //}
                        //else
                        //{
                        //    automatedSurveyUnSubscription.EMAIL_UNSUBSCRIBE = true;
                        //}
                        if (subscriptionSms == true)
                        {
                            automatedSurveyUnSubscription.SMS_UNSUBSCRIBE = subscriptionSms;
                        }
                        else if (subscriptionEmail == true)
                        {
                            automatedSurveyUnSubscription.EMAIL_UNSUBSCRIBE = subscriptionEmail;
                        }

                        //automatedSurveyUnSubscription.SMS_UNSUBSCRIBE = subscriptionSms;

                        //automatedSurveyUnSubscription.EMAIL_UNSUBSCRIBE = subscriptionEmail;
                        automatedSurveyUnSubscription.MODIFIED_BY = profile.UserName;
                        automatedSurveyUnSubscription.MODIFIED_DATE = Helper.GetCurrentDate();
                        _automatedSurveyUnSubscription.Update(automatedSurveyUnSubscription);
                        _automatedSurveyUnSubscription.Save();
                        //response.ErrorMessage = "";
                        //response.Message = "Patient has been successfully Unsubscribed to receive survey link(s) via Email.” will be displayed";
                        //response.Success = true;
                        response.ErrorMessage = "";
                        if (subscriptionEmail == true)
                        {
                            response.Message = "Patient has been successfully Unsubscribed to receive survey link(s) via Email";
                            Thread unsubscribeThread = new Thread(() =>
                            {
                                sendUnsubscribeEmail(objPatientSurvey);
                            });
                            unsubscribeThread.Start();
                        }
                        else if (subscriptionSms == true)
                        {
                            response.Message = "Patient has been successfully Unsubscribed to receive survey link(s) via SMS";
                        }
                        //response.Message = "Patient has been successfully subscribed to receive survey link(s) via SMS";
                        response.Success = true;
                    }
                    else
                    {
                        AutomatedSurveyUnSubscription objautomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                        objautomatedSurveyUnSubscription.AUTOMATED_SURVEY_UNSUBSCRIPTION_ID = Helper.getMaximumId("AUTOMATED_SURVEY_UNSUBSCRIPTION_ID");
                        objautomatedSurveyUnSubscription.SURVEY_ID = objPatientSurvey.SURVEY_ID;
                        var patinetAccount = Convert.ToInt64(objPatientSurvey.PATIENT_ACCOUNT_NUMBER);
                        objautomatedSurveyUnSubscription.PATIENT_ACCOUNT = patinetAccount;
                        objautomatedSurveyUnSubscription.SMS_UNSUBSCRIBE = subscriptionSms;
                        objautomatedSurveyUnSubscription.EMAIL_UNSUBSCRIBE = subscriptionEmail;
                        objautomatedSurveyUnSubscription.PRACTICE_CODE = AppConfiguration.GetPracticeCode;
                        objautomatedSurveyUnSubscription.CREATED_BY = objautomatedSurveyUnSubscription.MODIFIED_BY = profile.UserName;
                        objautomatedSurveyUnSubscription.CREATED_DATE = objautomatedSurveyUnSubscription.MODIFIED_DATE = Helper.GetCurrentDate();
                        _automatedSurveyUnSubscription.Insert(objautomatedSurveyUnSubscription);
                        _automatedSurveyUnSubscription.Save();
                        response.ErrorMessage = "";
                        if(subscriptionEmail == true)
                        {
                            response.Message = "Patient has been successfully Unsubscribed to receive survey link(s) via Email";
                            Thread unsubscribeThread = new Thread(() =>
                            {
                                sendUnsubscribeEmail(objPatientSurvey);
                            });
                            unsubscribeThread.Start();
                        }
                        else if(subscriptionSms == true)
                        {
                            response.Message = "Patient has been successfully Unsubscribed to receive survey link(s) via SMS";
                        }
                        //response.Message = "Patient has been successfully subscribed to receive survey link(s) via SMS";
                        response.Success = true;
                    }
                }
                else
                {
                    automatedSurveyUnSubscription = _automatedSurveyUnSubscription.GetByID(automatedSurveyUnSubscription.AUTOMATED_SURVEY_UNSUBSCRIPTION_ID);

                    if(objPatientSurvey.SurveyLinkSendVia == "SubscriptionBoth")
                    {
                        automatedSurveyUnSubscription.EMAIL_UNSUBSCRIBE = subscriptionEmail = false;
                        automatedSurveyUnSubscription.SMS_UNSUBSCRIBE = subscriptionSms = false;
                    }
                    else
                    {
                        if (subscriptionSms == true)
                        {
                            automatedSurveyUnSubscription.SMS_UNSUBSCRIBE = false;
                        }

                        if (subscriptionEmail == true)
                        {
                            automatedSurveyUnSubscription.EMAIL_UNSUBSCRIBE = false;
                        }
                    }
                    automatedSurveyUnSubscription.MODIFIED_BY = profile.UserName;
                    automatedSurveyUnSubscription.MODIFIED_DATE = Helper.GetCurrentDate();
                    _automatedSurveyUnSubscription.Update(automatedSurveyUnSubscription);
                    _automatedSurveyUnSubscription.Save();
                    if(subscriptionEmail == true )
                    {
                        Thread subscribeThread = new Thread(() =>
                        {
                            sendSubscribleEmail(objPatientSurvey);
                        });
                        subscribeThread.Start();
                        response.Message = "Patient has been successfully subscribed to receive survey link(s) via Email";
                    }
                    else if(subscriptionSms == true)
                    {
                        response.Message = "Patient has been successfully subscribed to receive survey link(s) via SMS";
                    }
                    response.ErrorMessage = "";
                    //response.Message = "Patient has been successfully subscribed to receive survey link(s) via SMS";
                    response.Success = true;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public ResponseModel Register(UserAccount objUserAccount)
        {
            ResponseModel response = new ResponseModel();
            UserAccount userAccount = new UserAccount();
            long practiceCode = AppConfiguration.GetPracticeCode;
            userAccount = _userAccountRepository.GetFirst(r => r.EMAIL_ADDRESS == objUserAccount.EMAIL_ADDRESS && r.DELETED == false);


            if (objUserAccount != null && userAccount == null)
            {
                //UserAccount objautomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                objUserAccount.APPLICATION_USER_ACCOUNTS_ID = Helper.getMaximumId("APPLICATION_USER_ACCOUNTS_ID");
                //objautomatedSurveyUnSubscription.SURVEY_ID = objPatientSurvey.SURVEY_ID;
               // var patinetAccount = Convert.ToInt64(objPatientSurvey.PATIENT_ACCOUNT_NUMBER);
                //objUserAccount.User_Name = objUserAccount;
                //objUserAccount.EMAIL_ADDRESS = subscriptionSms;
                //objUserAccount.PASSWORD = subscriptionEmail;

                objUserAccount.CREATED_BY = objUserAccount.MODIFIED_BY = "Team";
                objUserAccount.CREATED_DATE = objUserAccount.MODIFIED_DATE = Helper.GetCurrentDate();
                _userAccountRepository.Insert(objUserAccount);
                _userAccountRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;

            }
            else
            {
                response.Message = "already exist";
                response.Success = false;
            }
            return response;
            //throw new NotImplementedException();
        }
        public UserAccount Login(UserAccount objUserAccount)
        {
            ResponseModel response = new ResponseModel();
            UserAccount userAccount = new UserAccount();
            long practiceCode = AppConfiguration.GetPracticeCode;

            userAccount = _userAccountRepository.GetFirst(r =>
    (r.EMAIL_ADDRESS.Equals(objUserAccount.UserNameEmail, StringComparison.OrdinalIgnoreCase) ||
    r.User_Name.Equals(objUserAccount.UserNameEmail, StringComparison.OrdinalIgnoreCase)) &&
    !r.DELETED &&
    r.PASSWORD == objUserAccount.LoginPassword
);

            //userAccount = _userAccountRepository.GetFirst(r => (r.EMAIL_ADDRESS == objUserAccount.UserNameEmail 
            //|| r.User_Name == objUserAccount.UserNameEmail) && r.DELETED == false
            //&& r.PASSWORD == objUserAccount.LoginPassword
            //);
            if(userAccount != null)
            {
                response.Message = "login successfully";
                response.Success = true;
            }
            else
            {
                response.Message = "not login";
                response.Success = true;

            }
            return userAccount;
            //throw new NotImplementedException();
        }
        public ResponseModel AddUpdateBusiness(BusinessDetail objBusinessDetail)
        {
            ResponseModel response = new ResponseModel();
            BusinessDetail businessDetail = new BusinessDetail();
            long practiceCode = AppConfiguration.GetPracticeCode;
            businessDetail = _businessDetailRepository.GetFirst(r => r.BUSINESS_DETAIL_ID == objBusinessDetail.BUSINESS_DETAIL_ID && r.DELETED == false);
            if (businessDetail == null)
            {
                //UserAccount objautomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                objBusinessDetail.BUSINESS_DETAIL_ID = Helper.getMaximumId("BUSINESS_DETAIL_ID");
                //objautomatedSurveyUnSubscription.SURVEY_ID = objPatientSurvey.SURVEY_ID;
                // var patinetAccount = Convert.ToInt64(objPatientSurvey.PATIENT_ACCOUNT_NUMBER);
                //objUserAccount.User_Name = objUserAccount;
                //objUserAccount.EMAIL_ADDRESS = subscriptionSms;
                //objUserAccount.PASSWORD = subscriptionEmail;

                objBusinessDetail.CREATED_BY = objBusinessDetail.MODIFIED_BY = "Team";
                objBusinessDetail.CREATED_DATE = objBusinessDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                _businessDetailRepository.Insert(objBusinessDetail);
                _businessDetailRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;
                foreach (var fileName in objBusinessDetail.uploadedFilesName)
                {
                    BusinessFilesDetail businessFilesDetail = new BusinessFilesDetail();

                    businessFilesDetail.BUSINESS_FILES_DTEAIL_ID = Helper.getMaximumId("BUSINESS_FILES_DTEAIL_ID");
                    businessFilesDetail.BUSINESS_DETAIL_ID = objBusinessDetail.BUSINESS_DETAIL_ID;
                    businessFilesDetail.FILE_PATH = fileName;


                    //objautomatedSurveyUnSubscription.SURVEY_ID = objPatientSurvey.SURVEY_ID;
                    // var patinetAccount = Convert.ToInt64(objPatientSurvey.PATIENT_ACCOUNT_NUMBER);
                    //objUserAccount.User_Name = objUserAccount;
                    //objUserAccount.EMAIL_ADDRESS = subscriptionSms;
                    //objUserAccount.PASSWORD = subscriptionEmail;

                    businessFilesDetail.CREATED_BY = businessFilesDetail.MODIFIED_BY = "Team";
                    businessFilesDetail.CREATED_DATE = businessFilesDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                    _businessFilesDetailRepository.Insert(businessFilesDetail);
                    _businessFilesDetailRepository.Save();
                    Console.WriteLine(fileName);
                }
               
            }
            else
            {
                businessDetail.BUSINESS_IMPORTANT_NOTES = objBusinessDetail.BUSINESS_IMPORTANT_NOTES;
                businessDetail.BUSINESS_PICTURE_ID = objBusinessDetail.BUSINESS_PICTURE_ID;
                businessDetail.BUSINESS_CATEGORY_TYPE = objBusinessDetail.BUSINESS_CATEGORY_TYPE;
                businessDetail.BUSINESS_CATEGORY = objBusinessDetail.BUSINESS_CATEGORY;
                businessDetail.BUSINESS_HOURS = objBusinessDetail.BUSINESS_HOURS;
                businessDetail.BUSINESS_ZIP_CODE = objBusinessDetail.BUSINESS_ZIP_CODE;
                businessDetail.BUSINESS_CITY = objBusinessDetail.BUSINESS_CITY;
                businessDetail.BUSINESS_ADDRESS = objBusinessDetail.BUSINESS_ADDRESS;
                businessDetail.BUSINESS_NAME = objBusinessDetail.BUSINESS_NAME;
                businessDetail.EMAIL_ADDRESS = objBusinessDetail.EMAIL_ADDRESS;
                businessDetail.CONTACT_NO = objBusinessDetail.CONTACT_NO;
                businessDetail.PASSWORD = objBusinessDetail.PASSWORD;
                businessDetail.BUSINESS_ADDRESS = objBusinessDetail.BUSINESS_ADDRESS;
                businessDetail.MODIFIED_BY = "Team";
                businessDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                _businessDetailRepository.Update(businessDetail);
                _businessDetailRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;

                foreach (var fileName in objBusinessDetail.uploadedFilesName)
                {
                    BusinessFilesDetail businessFilesDetail = new BusinessFilesDetail();

                    businessFilesDetail.BUSINESS_FILES_DTEAIL_ID = Helper.getMaximumId("BUSINESS_FILES_DTEAIL_ID");
                    businessFilesDetail.BUSINESS_DETAIL_ID = objBusinessDetail.BUSINESS_DETAIL_ID;
                    businessFilesDetail.FILE_PATH = fileName;
                    businessFilesDetail.CREATED_BY = businessFilesDetail.MODIFIED_BY = "Team";
                    businessFilesDetail.CREATED_DATE = businessFilesDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                    _businessFilesDetailRepository.Insert(businessFilesDetail);
                    _businessFilesDetailRepository.Save();
                    Console.WriteLine(fileName);
                }

            }

            return response;
        }
        public ResponseModel AddUpdateBlogBusiness(BusinessBlogDetail objBusinessBlogDetail)
        {
            ResponseModel response = new ResponseModel();
            BusinessBlogDetail businessBlogDetail = new BusinessBlogDetail();
            long practiceCode = AppConfiguration.GetPracticeCode;
            businessBlogDetail = _businessBlogDetailRepository.GetFirst(r => r.BUSINESS_BLOG_ID == objBusinessBlogDetail.BUSINESS_BLOG_ID && r.DELETED == false);
            if (businessBlogDetail == null)
            {
                //UserAccount objautomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                objBusinessBlogDetail.BUSINESS_BLOG_ID = Helper.getMaximumId("BUSINESS_BLOG_ID");
                //objautomatedSurveyUnSubscription.SURVEY_ID = objPatientSurvey.SURVEY_ID;
                // var patinetAccount = Convert.ToInt64(objPatientSurvey.PATIENT_ACCOUNT_NUMBER);
                //objUserAccount.User_Name = objUserAccount;
                //objUserAccount.EMAIL_ADDRESS = subscriptionSms;
                //objUserAccount.PASSWORD = subscriptionEmail;

                objBusinessBlogDetail.CREATED_BY = objBusinessBlogDetail.MODIFIED_BY = "Team";
                objBusinessBlogDetail.CREATED_DATE = objBusinessBlogDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                _businessBlogDetailRepository.Insert(objBusinessBlogDetail);
                _businessBlogDetailRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;

                foreach (var fileName in objBusinessBlogDetail.uploadedFilesName)
                {
                    BusinessFilesDetail businessFilesDetail = new BusinessFilesDetail();

                    businessFilesDetail.BUSINESS_FILES_DTEAIL_ID = Helper.getMaximumId("BUSINESS_FILES_DTEAIL_ID");
                    //businessFilesDetail.BUSINESS_DETAIL_ID = objBusinessBlogDetail.BUSINESS_DETAIL_ID;
                    businessFilesDetail.BUSINESS_BLOG_ID = objBusinessBlogDetail.BUSINESS_BLOG_ID;
                    businessFilesDetail.FILE_PATH = fileName;


                    //objautomatedSurveyUnSubscription.SURVEY_ID = objPatientSurvey.SURVEY_ID;
                    // var patinetAccount = Convert.ToInt64(objPatientSurvey.PATIENT_ACCOUNT_NUMBER);
                    //objUserAccount.User_Name = objUserAccount;
                    //objUserAccount.EMAIL_ADDRESS = subscriptionSms;
                    //objUserAccount.PASSWORD = subscriptionEmail;

                    businessFilesDetail.CREATED_BY = businessFilesDetail.MODIFIED_BY = "Team";
                    businessFilesDetail.CREATED_DATE = businessFilesDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                    _businessFilesDetailRepository.Insert(businessFilesDetail);
                    _businessFilesDetailRepository.Save();
                    Console.WriteLine(fileName);
                }
               
            }
            else
            {
                businessBlogDetail.BUSINESS_IMPORTANT_NOTES = businessBlogDetail.BUSINESS_IMPORTANT_NOTES;
                //businessBlogDetail.BUSINESS_PICTURE_ID = businessBlogDetail.BUSINESS_PICTURE_ID;
                businessBlogDetail.BUSINESS_BLOG_CATEGORY = businessBlogDetail.BUSINESS_BLOG_CATEGORY;
                businessBlogDetail.BLOG_TITLE = businessBlogDetail.BLOG_TITLE;
                businessBlogDetail.EMAIL_ADDRESS = businessBlogDetail.EMAIL_ADDRESS;
                businessBlogDetail.AUTHOR_NAME = businessBlogDetail.AUTHOR_NAME;
                businessBlogDetail.BUSINESS_BLOG_CITY = businessBlogDetail.BUSINESS_BLOG_CITY;
                businessBlogDetail.EMAIL_ADDRESS = businessBlogDetail.EMAIL_ADDRESS;
                businessBlogDetail.MODIFIED_BY = "Team";
                businessBlogDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                _businessBlogDetailRepository.Update(businessBlogDetail);
                _businessBlogDetailRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;

                foreach (var fileName in objBusinessBlogDetail.uploadedFilesName)
                {
                    BusinessFilesDetail businessFilesDetail = new BusinessFilesDetail();

                    businessFilesDetail.BUSINESS_FILES_DTEAIL_ID = Helper.getMaximumId("BUSINESS_FILES_DTEAIL_ID");
                    //businessFilesDetail.BUSINESS_DETAIL_ID = objBusinessBlogDetail.BUSINESS_DETAIL_ID;
                    businessFilesDetail.BUSINESS_BLOG_ID = objBusinessBlogDetail.BUSINESS_BLOG_ID;
                    businessFilesDetail.FILE_PATH = fileName;
                    businessFilesDetail.CREATED_BY = businessFilesDetail.MODIFIED_BY = "Team";
                    businessFilesDetail.CREATED_DATE = businessFilesDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                    _businessFilesDetailRepository.Insert(businessFilesDetail);
                    _businessFilesDetailRepository.Save();
                    Console.WriteLine(fileName);
                }

            }

            return response;
        }
        public List<BusinessDetail> GetBusinessDetails(BusinessDetail objBusinessDetail)
        {
            ResponseModel response = new ResponseModel();
            BusinessDetail businessDetail = new BusinessDetail();
            List<BusinessDetail> obj = new List<BusinessDetail>();
            long practiceCode = AppConfiguration.GetPracticeCode;
            //obj = _businessDetailRepository.GetMany(r => r.EMAIL_ADDRESS == objBusinessDetail.EMAIL_ADDRESS && r.DELETED == false).ToList();


            if (objBusinessDetail != null)
            {
                var emailAddress = new SqlParameter("@EMAIL_ADDRESS", SqlDbType.VarChar) { Value = objBusinessDetail.EMAIL_ADDRESS };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                obj = SpRepository<BusinessDetail>.GetListWithStoreProcedure(@"EXEC SP_GET_BUSINESS_DETAILS @EMAIL_ADDRESS", emailAddress);
                foreach (var businessDetailId in obj)
                {

                    BusinessHoursStatus businessHoursStatus = new BusinessHoursStatus();
                    businessHoursStatus = GetBusinessHoursStatus(businessDetailId);
                    businessDetailId.OpenClose = businessHoursStatus.OpenClose;
                    // Convert time strings to "h:mm tt" format
                    businessDetailId.CurrentDayOpeningTime = businessHoursStatus.CurrentDayOpeningTime;
                    businessDetailId.CurrentDayClosingTime = businessHoursStatus.CurrentDayClosingTime;

                    List<BusinessFilesDetail> filesList = new List<BusinessFilesDetail>();

                    var businessId = new SqlParameter("@BUSINESS_DETAIL_ID", SqlDbType.BigInt) { Value = businessDetailId.BUSINESS_DETAIL_ID };
                    //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                    filesList = SpRepository<BusinessFilesDetail>.GetListWithStoreProcedure(@"EXEC SP_GET_BUSINESS_FILES_DETAILS @BUSINESS_DETAIL_ID", businessId);
                    //businessDetailId.uploadedFilesName.AddRange(filesList);
                    List<string> filePaths = filesList.Select(file => "http://localhost:11492/FoxDocumentDirectory/RequestForOrder/UploadImages/" + file.FILE_PATH).ToList();
                    businessDetailId.uploadedFilesName.AddRange(filePaths);
                }
            }
                return obj;
        }

        public ResponseModel DeleteBusinessDetails(long businessId)
        {
            ResponseModel response = new ResponseModel();
            BusinessDetail businessDetail = new BusinessDetail();
            List<BusinessDetail> obj = new List<BusinessDetail>();
            long practiceCode = AppConfiguration.GetPracticeCode;
            //obj = _businessDetailRepository.GetMany(r => r.EMAIL_ADDRESS == objBusinessDetail.EMAIL_ADDRESS && r.DELETED == false).ToList();


            if (businessId != 0)
            {
                var businessDetailsId = new SqlParameter("@BUSINESS_DETAIL_ID", SqlDbType.BigInt) { Value = businessId };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                businessDetail = SpRepository<BusinessDetail>.GetSingleObjectWithStoreProcedure(@"EXEC SP_DELETE_BUSINESS_DETAILS @BUSINESS_DETAIL_ID", businessDetailsId);
            }
                return response;
        }
        private static TimeSpan ParseTime(string timeString, string format)
        {
            if (string.IsNullOrWhiteSpace(timeString))
            {
                Console.WriteLine("Time string is null or empty.");
                return TimeSpan.Zero;
            }

            try
            {
                // Assuming timeString is in format HH:mm
                DateTime dateTime = DateTime.ParseExact(timeString, format, CultureInfo.InvariantCulture);
                return dateTime.TimeOfDay;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Failed to parse time string '{timeString}': {ex.Message}");
                return TimeSpan.Zero;
            }
        }
        private static string ConvertTo24HourFormat(string timeString)
        {
            if (string.IsNullOrWhiteSpace(timeString))
            {
                return "12:00 AM"; // Default to midnight if no time is provided
            }
            //if (DateTime.TryParse(timeString, dateTime))
            //{
            //    // Format the time to "h:mm tt"
            //    timeString = dateTime.ToString("h:mm tt");
            //    /timeString = dateTime.ToString("h:mm").ToUpper() + (dateTime.ToString("tt").ToUpper() == "AM" ? " AM" : " PM");

            //}
            // Add ":00" to the time string
            return timeString;
        }
        public BusinessHoursStatus GetBusinessHoursStatus(BusinessDetail businessDetail)
        {
            // Get current day of the week and format it to upper case
            string currentDay = DateTime.Now.DayOfWeek.ToString().ToUpper();

            // Determine the property names for the current day's opening and closing times
            string currentDayOpeningTimeProp = $"{currentDay}_OPENING_TIME";
            string currentDayClosingTimeProp = $"{currentDay}_CLOSING_TIME";

            // Get opening and closing time strings
            string openingTimeStr = businessDetail.GetType().GetProperty(currentDayOpeningTimeProp)?.GetValue(businessDetail, null) as string;
            string closingTimeStr = businessDetail.GetType().GetProperty(currentDayClosingTimeProp)?.GetValue(businessDetail, null) as string;

            // Define time format
            string timeFormat = "h tt";

            // Parse the times
            TimeSpan openingTime = ParseTime(openingTimeStr, timeFormat);
            TimeSpan closingTime = ParseTime(closingTimeStr, timeFormat);
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            // Check if the current time is within opening and closing times
            bool isOpenNow = currentTime >= openingTime && currentTime <= closingTime;

            // Create the result object
            var result = new BusinessHoursStatus
            {
                OpenClose = isOpenNow ? "Open Now" : "Closed",
                CurrentDayOpeningTime = ConvertTo24HourFormat(openingTimeStr),
                CurrentDayClosingTime = ConvertTo24HourFormat(closingTimeStr)
            };

            return result;
        }
        public BusinessDetail GetSelectedBusiness(BusinessDetail objBusinessDetail)
        {
            ResponseModel response = new ResponseModel();
            BusinessDetail businessDetail = new BusinessDetail();
            List<BusinessDetail> obj = new List<BusinessDetail>();
            long practiceCode = AppConfiguration.GetPracticeCode;
            //obj = _businessDetailRepository.GetMany(r => r.EMAIL_ADDRESS == objBusinessDetail.EMAIL_ADDRESS && r.DELETED == false).ToList();


            if (objBusinessDetail.BUSINESS_DETAIL_ID != 0)
            {
                var businessIdd = new SqlParameter("@BUSINESS_DETAIL_ID", SqlDbType.BigInt) { Value = objBusinessDetail.BUSINESS_DETAIL_ID };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                businessDetail = SpRepository<BusinessDetail>.GetSingleObjectWithStoreProcedure(@"EXEC SP_GET_SINGEL_BUSINESS_DETAILS @BUSINESS_DETAIL_ID", businessIdd);
                
                BusinessHoursStatus businessHoursStatus = new BusinessHoursStatus();
                businessHoursStatus = GetBusinessHoursStatus(businessDetail);
                businessDetail.OpenClose = businessHoursStatus.OpenClose;
                // Convert time strings to "h:mm tt" format
                businessDetail.CurrentDayOpeningTime = businessDetail.CurrentDayOpeningTime;
                businessDetail.CurrentDayClosingTime = businessDetail.CurrentDayClosingTime;

                string currentDay = DateTime.Now.DayOfWeek.ToString().ToUpper();

                // Determine the property name for the current day
                string currentDayOpeningTime = $"{currentDay}_OPENING_TIME";
                string currentDayClosingTime = $"{currentDay}_CLOSING_TIME";
                var openingTimeStr = businessDetail.GetType().GetProperty(currentDayOpeningTime)?.GetValue(businessDetail, null) as string; ;
                var closingTimeStr = businessDetail.GetType().GetProperty(currentDayClosingTime)?.GetValue(businessDetail, null) as string; ;
                // Parse the times
                //TimeSpan openingTime = TimeSpan.TryParse(openingTimeStr, out var parsedOpening) ? parsedOpening : TimeSpan.Zero;
                //TimeSpan closingTime = TimeSpan.TryParse(closingTimeStr, out var parsedClosing) ? parsedClosing : TimeSpan.Zero;
                //TimeSpan currentTime = DateTime.Now.TimeOfDay;
                string timeFormat = "h tt";
                TimeSpan openingTime = ParseTime(openingTimeStr, timeFormat);
                TimeSpan closingTime = ParseTime(closingTimeStr, timeFormat);
                TimeSpan currentTime = DateTime.Now.TimeOfDay;



                // Check if current time is within opening and closing times
                bool isOpenNow = currentTime >= openingTime && currentTime <= closingTime;

                // Output the result
                string status = isOpenNow ? "Open Now" : "Closed";
                Console.WriteLine(status);
                businessDetail.OpenClose = status;
                // Convert time strings to "h:mm tt" format
                businessDetail.CurrentDayOpeningTime = ConvertTo24HourFormat(openingTimeStr);
                businessDetail.CurrentDayClosingTime = ConvertTo24HourFormat(closingTimeStr);





                if (businessDetail != null)
                {
                    List<BusinessFilesDetail> filesList = new List<BusinessFilesDetail>();

                    var businessId = new SqlParameter("@BUSINESS_DETAIL_ID", SqlDbType.BigInt) { Value = businessDetail.BUSINESS_DETAIL_ID };
                    //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                    filesList = SpRepository<BusinessFilesDetail>.GetListWithStoreProcedure(@"EXEC SP_GET_BUSINESS_FILES_DETAILS @BUSINESS_DETAIL_ID", businessId);
                    //businessDetailId.uploadedFilesName.AddRange(filesList);
                    List<string> filePaths = filesList.Select(file => "http://localhost:11492/FoxDocumentDirectory/RequestForOrder/UploadImages/" + file.FILE_PATH).ToList();
                    businessDetail.uploadedFilesName.AddRange(filePaths);

                    string baseUrl = "http://localhost:11492/FoxDocumentDirectory/RequestForOrder/UploadImages/";

                    foreach (var file in filesList)
                    {
                        file.FILE_PATH = baseUrl + file.FILE_PATH;
                    }
                    businessDetail.BusinessFilesDetail.AddRange(filesList);

                }
            }
            return businessDetail;
        }

        public ResponseModel DeleteSelectedImage(BusinessFilesDetailList[] objbusinessFilesList)
        {
            ResponseModel response = new ResponseModel();
            BusinessDetail businessDetail = new BusinessDetail();
            List<BusinessDetail> obj = new List<BusinessDetail>();
            long practiceCode = AppConfiguration.GetPracticeCode;
            //obj = _businessDetailRepository.GetMany(r => r.EMAIL_ADDRESS == objBusinessDetail.EMAIL_ADDRESS && r.DELETED == false).ToList();


            if (objbusinessFilesList != null)
            {
                foreach (var item in objbusinessFilesList)
                {
                    var businessDetailsId = new SqlParameter("@BUSINESS_FILES_DTEAIL_ID", SqlDbType.BigInt) { Value = item.BUSINESS_FILES_DTEAIL_ID };
                    //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                    businessDetail = SpRepository<BusinessDetail>.GetSingleObjectWithStoreProcedure(@"EXEC SP_DELETE_BUSINESS_FILES_DETAILS @BUSINESS_FILES_DTEAIL_ID", businessDetailsId);
                }                
            }
            return response;
        }
        public List<UserAccount> GetUserDetails(UserAccount[] objUserAccountList)
        {
            ResponseModel response = new ResponseModel();
            BusinessDetail businessDetail = new BusinessDetail();
            List<UserAccount> obj = new List<UserAccount>();
            long practiceCode = AppConfiguration.GetPracticeCode;
            //obj = _businessDetailRepository.GetMany(r => r.EMAIL_ADDRESS == objBusinessDetail.EMAIL_ADDRESS && r.DELETED == false).ToList();


            if (objUserAccountList != null)
            {
                
                    //var businessDetailsId = new SqlParameter("@BUSINESS_FILES_DTEAIL_ID", SqlDbType.BigInt) { Value = item.BUSINESS_FILES_DTEAIL_ID };
                    //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                    obj = SpRepository<UserAccount>.GetListWithStoreProcedure(@"EXEC SP_GET_USER_DETAILS");
                //}                
            }
            return obj;
        }
        public ResponseModel DeleteUser(UserAccount objUserAccount)
        {
            ResponseModel response = new ResponseModel();
            BusinessDetail businessDetail = new BusinessDetail();
            List<UserAccount> obj = new List<UserAccount>();
            long practiceCode = AppConfiguration.GetPracticeCode;
            //obj = _businessDetailRepository.GetMany(r => r.EMAIL_ADDRESS == objBusinessDetail.EMAIL_ADDRESS && r.DELETED == false).ToList();


            if (objUserAccount.APPLICATION_USER_ACCOUNTS_ID != 0)
            {
                var userId = new SqlParameter("@APPLICATION_USER_ACCOUNTS_ID", SqlDbType.BigInt) { Value = objUserAccount.APPLICATION_USER_ACCOUNTS_ID };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                objUserAccount = SpRepository<UserAccount>.GetSingleObjectWithStoreProcedure(@"EXEC SP_DELETE_USER_DETAILS @APPLICATION_USER_ACCOUNTS_ID", userId);
                response.Message = "User deleted successfully.";
                response.Success = true;
            }
            return response;
        }
        public ResponseModel EditUser(UserAccount objUserAccount)
        {
            ResponseModel response = new ResponseModel();
            UserAccount userAccount = new UserAccount();
            long practiceCode = AppConfiguration.GetPracticeCode;
            userAccount = _userAccountRepository.GetFirst(r => r.APPLICATION_USER_ACCOUNTS_ID == objUserAccount.APPLICATION_USER_ACCOUNTS_ID && r.DELETED == false);


            if (objUserAccount != null && userAccount != null)
            {
                //UserAccount objautomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                //objUserAccount.APPLICATION_USER_ACCOUNTS_ID = Helper.getMaximumId("APPLICATION_USER_ACCOUNTS_ID");
                //objautomatedSurveyUnSubscription.SURVEY_ID = objPatientSurvey.SURVEY_ID;
                // var patinetAccount = Convert.ToInt64(objPatientSurvey.PATIENT_ACCOUNT_NUMBER);
                //objUserAccount.User_Name = objUserAccount;
                //objUserAccount.EMAIL_ADDRESS = subscriptionSms;
                userAccount.PASSWORD = objUserAccount.PASSWORD;
                userAccount.EMAIL_ADDRESS = objUserAccount.EMAIL_ADDRESS;
                userAccount.Blocked = objUserAccount.Blocked;
                userAccount.User_Name = objUserAccount.User_Name;
                userAccount.ACCOUNT_TYPE = objUserAccount.ACCOUNT_TYPE;
                userAccount.MODIFIED_BY = objUserAccount.MODIFIED_BY = "Team";
                userAccount.CREATED_DATE = objUserAccount.MODIFIED_DATE = Helper.GetCurrentDate();
                _userAccountRepository.Update(userAccount);
                _userAccountRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;

            }
            return response;
        }
        public List<BusinessBlogDetail> GetBlogsDetails(BusinessBlogDetail objBusinessBlogDetail)
        {
            ResponseModel response = new ResponseModel();
            BusinessDetail businessDetail = new BusinessDetail();
            List<BusinessBlogDetail> obj = new List<BusinessBlogDetail>();
            long practiceCode = AppConfiguration.GetPracticeCode;
            //obj = _businessDetailRepository.GetMany(r => r.EMAIL_ADDRESS == objBusinessDetail.EMAIL_ADDRESS && r.DELETED == false).ToList();


            if (objBusinessBlogDetail != null)
            {
                var emailAddress = new SqlParameter("@EMAIL_ADDRESS", SqlDbType.VarChar) { Value = objBusinessBlogDetail.EMAIL_ADDRESS };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                obj = SpRepository<BusinessBlogDetail>.GetListWithStoreProcedure(@"EXEC SP_GET_BUSINESS_BLOGS_DETAILS");
                foreach (var businessDetailId in obj)
                {
                    List<BusinessFilesDetail> filesList = new List<BusinessFilesDetail>();

                    var businessId = new SqlParameter("@BUSINESS_BLOG_ID", SqlDbType.BigInt) { Value = businessDetailId.BUSINESS_BLOG_ID };
                    //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                    filesList = SpRepository<BusinessFilesDetail>.GetListWithStoreProcedure(@"EXEC SP_GET_BUSINESS_BLOGS_FILES_DETAILS @BUSINESS_BLOG_ID", businessId);
                    //businessDetailId.uploadedFilesName.AddRange(filesList);
                    List<string> filePaths = filesList.Select(file => "http://localhost:11492/FoxDocumentDirectory/RequestForOrder/UploadImages/" + file.FILE_PATH).ToList();
                    businessDetailId.uploadedFilesName.AddRange(filePaths);
                }
            }
            return obj;
        }
        public ResponseModel SubmitRating(BusinessRating objBusinessRating)
        {
            ResponseModel response = new ResponseModel();
            BusinessRating businessRating = new BusinessRating();
            long practiceCode = AppConfiguration.GetPracticeCode;
            businessRating = _businessRatingRepository.GetFirst(r => r.TBL_BUSINESS_RATING_ID == objBusinessRating.TBL_BUSINESS_RATING_ID && r.DELETED == false);
            if (businessRating == null)
            {
                //UserAccount objautomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                objBusinessRating.TBL_BUSINESS_RATING_ID = Helper.getMaximumId("BUSINESS_DETAIL_ID");
                //objautomatedSurveyUnSubscription.SURVEY_ID = objPatientSurvey.SURVEY_ID;
                // var patinetAccount = Convert.ToInt64(objPatientSurvey.PATIENT_ACCOUNT_NUMBER);
                //objUserAccount.User_Name = objUserAccount;
                //objUserAccount.EMAIL_ADDRESS = subscriptionSms;
                //objUserAccount.PASSWORD = subscriptionEmail;

                objBusinessRating.CREATED_BY = objBusinessRating.MODIFIED_BY = "Team";
                objBusinessRating.CREATED_DATE = objBusinessRating.MODIFIED_DATE = Helper.GetCurrentDate();
                _businessRatingRepository.Insert(objBusinessRating);
                _businessRatingRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;
            }
            else
            {
                businessRating.TBL_BUSINESS_RATING_ID = objBusinessRating.TBL_BUSINESS_RATING_ID;
                businessRating.BUSINESS_ID = objBusinessRating.BUSINESS_ID;
                businessRating.CLEANLINESS_RATING = objBusinessRating.CLEANLINESS_RATING;
                businessRating.ACCURACY_RATING = objBusinessRating.ACCURACY_RATING;
                businessRating.LOCATION_RATING = objBusinessRating.LOCATION_RATING;
                businessRating.CHECKIN_RATING = objBusinessRating.CHECKIN_RATING;
                businessRating.COMMUNICATION_RATING = objBusinessRating.COMMUNICATION_RATING;
                businessRating.VALUE_RATING = objBusinessRating.VALUE_RATING;
                businessRating.EMAIL_ADDRESS = objBusinessRating.EMAIL_ADDRESS;
                businessRating.NAME = objBusinessRating.NAME;
                businessRating.FEEDBACK = objBusinessRating.FEEDBACK;
                businessRating.MODIFIED_BY = "Team";
                businessRating.MODIFIED_DATE = Helper.GetCurrentDate();
                _businessRatingRepository.Update(businessRating);
                _businessDetailRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;
            }

            return response;
        }
        // This method calculates the average rating
        public double GetCombinedRating(BusinessRating objBusinessRating)
        {
            // Calculate the average of the ratings
            //double averageRating = (Convert.ToInt64(objBusinessRating.COMMUNICATION_RATING) + Convert.ToInt64(objBusinessRating.CLEANLINESS_RATING) 
            //    + Convert.ToInt64((objBusinessRating.ACCURACY_RATING) + Convert.ToInt64(objBusinessRating.LOCATION_RATING) + Convert.ToInt64(objBusinessRating.VALUE_RATING))) / 5.0;

            double averageRating = (Convert.ToInt64(objBusinessRating.COMMUNICATION_RATING) + Convert.ToInt64(objBusinessRating.CLEANLINESS_RATING)
               + Convert.ToInt64((objBusinessRating.ACCURACY_RATING) + Convert.ToInt64(objBusinessRating.LOCATION_RATING) + Convert.ToInt64(objBusinessRating.VALUE_RATING))) / 5.0;


            // Round to 1 decimal place, if needed
            return Math.Round(averageRating, 1);
        }
        public List<BusinessRating> GetBusinessRating(BusinessRating objBusinessRating)
        {
            ResponseModel response = new ResponseModel();
            BusinessDetail businessDetail = new BusinessDetail();
            List<BusinessRating> obj = new List<BusinessRating>();
            long practiceCode = AppConfiguration.GetPracticeCode;
            //obj = _businessDetailRepository.GetMany(r => r.EMAIL_ADDRESS == objBusinessDetail.EMAIL_ADDRESS && r.DELETED == false).ToList();


            if (objBusinessRating != null)
            {
                var businessId = new SqlParameter("@BUSINESS_ID", SqlDbType.BigInt) { Value = objBusinessRating.BUSINESS_ID };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                obj = SpRepository<BusinessRating>.GetListWithStoreProcedure(@"EXEC SP_GET_BUSINESS_RATING_DETAILS @BUSINESS_ID", businessId);
                //if(obj != null)
                //{

                //    double combinedRating = GetCombinedRating(obj[0]);
                //}
            }
            return obj;
        }

        public ResponseModel GenerateToken(UserProfileToken objUserProfileToken)
        {
            ResponseModel response = new ResponseModel();
            UserProfileToken userProfileToken = new UserProfileToken();
            long practiceCode = AppConfiguration.GetPracticeCode;
            //businessRating = _businessRatingRepository.GetFirst(r => r.TBL_BUSINESS_RATING_ID == objBusinessRating.TBL_BUSINESS_RATING_ID && r.DELETED == false);
            if (objUserProfileToken != null)
            {
                //UserAccount objautomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                objUserProfileToken.TOKEN_ID = Helper.getMaximumId("BUSINESS_DETAIL_ID");
                //objautomatedSurveyUnSubscription.SURVEY_ID = objPatientSurvey.SURVEY_ID;
                // var patinetAccount = Convert.ToInt64(objPatientSurvey.PATIENT_ACCOUNT_NUMBER);
                //objUserAccount.User_Name = objUserAccount;
                //objUserAccount.EMAIL_ADDRESS = subscriptionSms;
                //objUserAccount.PASSWORD = subscriptionEmail;
                objUserProfileToken.ISSUED_ON = DateTime.UtcNow;
                objUserProfileToken.EXPIRES_ON = DateTime.UtcNow.AddHours(1);
                _userProfileTokenRepository.Insert(objUserProfileToken);
                _userProfileTokenRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;
            }
           

            return response;
        }
        public UserAccount ValidateUser(UserAccount objUserAccount)
        {
            ResponseModel response = new ResponseModel();
            UserAccount userAccount = new UserAccount();
            //UserProfileToken userProfileToken = new UserProfileToken();
            //long practiceCode = AppConfiguration.GetPracticeCode;

            //var userId = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objUserAccount.APPLICATION_USER_ACCOUNTS_ID };
            ////var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
            //userProfileToken = SpRepository<UserProfileToken>.GetSingleObjectWithStoreProcedure(@"EXEC SP_GET_BUSINESS_RATING_DETAILS @USER_ID", userId);


            userAccount = _userAccountRepository.GetFirst(r =>
    (r.APPLICATION_USER_ACCOUNTS_ID == objUserAccount .APPLICATION_USER_ACCOUNTS_ID &&
    !r.DELETED )
);

            //userAccount = _userAccountRepository.GetFirst(r => (r.EMAIL_ADDRESS == objUserAccount.UserNameEmail 
            //|| r.User_Name == objUserAccount.UserNameEmail) && r.DELETED == false
            //&& r.PASSWORD == objUserAccount.LoginPassword
            //);
            if (userAccount != null)
            {
                response.Message = "login successfully";
                response.Success = true;
            }
            else
            {
                response.Message = "not login";
                response.Success = true;

            }
            return userAccount;
            //throw new NotImplementedException();
        }
        public UserProfileToken UserToken(UserProfileToken objUserProfileToken)
        {
            ResponseModel response = new ResponseModel();
            UserAccount userAccount = new UserAccount();
            UserProfileToken userProfileToken = new UserProfileToken();
            //long practiceCode = AppConfiguration.GetPracticeCode;

            var userId = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objUserProfileToken.USER_ID };
            //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
            userProfileToken = SpRepository<UserProfileToken>.GetSingleObjectWithStoreProcedure(@"EXEC SP_CHECK_USER_TOKEN @USER_ID", userId);
            if (userAccount != null)
            {
                response.Message = "login successfully";
                response.Success = true;
            }
            else
            {
                response.Message = "not login";
                response.Success = true;

            }
            return userProfileToken;
            //throw new NotImplementedException();
        }

         public UserAccount LogoutUser(UserAccount objUserAccount)
        {
            ResponseModel response = new ResponseModel();
            UserAccount userAccount = new UserAccount();
            UserProfileToken userProfileToken = new UserProfileToken();
            //long practiceCode = AppConfiguration.GetPracticeCode;

            var userId = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objUserAccount.APPLICATION_USER_ACCOUNTS_ID };
            //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
            userAccount = SpRepository<UserAccount>.GetSingleObjectWithStoreProcedure(@"EXEC SP_EXPIRE_USER_TOKEN @USER_ID", userId);
            if (userAccount != null)
            {
                response.Message = "login successfully";
                response.Success = true;
            }
            else
            {
                response.Message = "not login";
                response.Success = true;

            }
            return userAccount;
            //throw new NotImplementedException();
        }

        //reels
        public static void AddRemoveReelLike(ReelsDetails objReelsDetails)
        {
            if (objReelsDetails.isClickOnReelLike == true)
            {
                ReelsLikes reelsCommentsLikes = new ReelsLikes();
                var likeId = new SqlParameter("@REELS_LIKE_ID", SqlDbType.BigInt) { Value = Helper.getMaximumId("REELS_LIKE_ID") };
                var reelsCommentId = new SqlParameter("@REELS_DETAILS_ID", SqlDbType.BigInt) { Value = objReelsDetails.REELS_DETAILS_ID };
                var userId = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objReelsDetails.USER_ID };
                var action = new SqlParameter("@ACTION", SqlDbType.VarChar) { Value = "Add" };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                reelsCommentsLikes = SpRepository<ReelsLikes>.GetSingleObjectWithStoreProcedure(@"EXEC SP_ADD_UPDATE_REELS_LIKES 
                   @REELS_LIKE_ID, @REELS_DETAILS_ID, @USER_ID, @ACTION", likeId, reelsCommentId, userId, action);
            }
            else
            {
                ReelsLikes reelsCommentsLikes = new ReelsLikes();
                var likeId = new SqlParameter("@REELS_LIKE_ID", SqlDbType.BigInt) { Value = Helper.getMaximumId("REELS_LIKE_ID") };
                var reelsCommentId = new SqlParameter("@REELS_DETAILS_ID", SqlDbType.BigInt) { Value = objReelsDetails.REELS_DETAILS_ID };
                var userId = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objReelsDetails.USER_ID };
                var action = new SqlParameter("@ACTION", SqlDbType.VarChar) { Value = "Remove" };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                reelsCommentsLikes = SpRepository<ReelsLikes>.GetSingleObjectWithStoreProcedure(@"EXEC SP_ADD_UPDATE_REELS_LIKES 
                   @REELS_LIKE_ID, @REELS_DETAILS_ID, @USER_ID, @ACTION", likeId, reelsCommentId, userId, action);

            }
        }

        public ResponseModel AddUpdateReels(ReelsDetails objReelsDetails)
        {
            ResponseModel response = new ResponseModel();
            ReelsDetails businessDetail = new ReelsDetails();
            long practiceCode = AppConfiguration.GetPracticeCode;
            businessDetail = _reelsDetailsRepository.GetFirst(r => r.REELS_DETAILS_ID == objReelsDetails.REELS_DETAILS_ID && r.DELETED == false);
            if (businessDetail == null)
            {
                //UserAccount objautomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                objReelsDetails.REELS_DETAILS_ID = Helper.getMaximumId("BUSINESS_DETAIL_ID");
                objReelsDetails.CREATED_BY = objReelsDetails.MODIFIED_BY = "Team";
                objReelsDetails.CREATED_DATE = objReelsDetails.MODIFIED_DATE = Helper.GetCurrentDate();
                _reelsDetailsRepository.Insert(objReelsDetails);
                _reelsDetailsRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;
                foreach (var fileName in objReelsDetails.uploadedFilesName)
                {
                    ReelsFilesDetails reelsFilesDetails = new ReelsFilesDetails();

                    reelsFilesDetails.REELS_FILES_DETAILS_ID = Helper.getMaximumId("BUSINESS_FILES_DTEAIL_ID");
                    reelsFilesDetails.REELS_DETAILS_ID = objReelsDetails.REELS_DETAILS_ID;
                    reelsFilesDetails.FILE_PATH = fileName;
                    reelsFilesDetails.CREATED_BY =   reelsFilesDetails.MODIFIED_BY = "Team";
                    reelsFilesDetails.CREATED_DATE = reelsFilesDetails.MODIFIED_DATE = Helper.GetCurrentDate();
                    _reelsFilesDetailsRepository.Insert(reelsFilesDetails);
                    _reelsFilesDetailsRepository.Save();
                    Console.WriteLine(fileName);
                }

            }
            else
            {
                if(objReelsDetails.isLikeUnlikeReel == true)
                {
                    businessDetail.REEL_LIKES_COUNT = +objReelsDetails.REEL_LIKES_COUNT;
                    AddRemoveReelLike(objReelsDetails);
                }
                if (objReelsDetails.isLikeUnlikeReel == false)
                {
                    businessDetail.USER_TYPE = objReelsDetails.USER_TYPE;
                    businessDetail.EMAIL_ADDRESS = objReelsDetails.EMAIL_ADDRESS;
                }
                
                businessDetail.MODIFIED_BY = "Team";
                businessDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                _reelsDetailsRepository.Update(businessDetail);
                _reelsDetailsRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;
                if (objReelsDetails.isLikeUnlikeReel == false)
                {
                    foreach (var fileName in businessDetail.uploadedFilesName)
                    {
                        ReelsFilesDetails reelsFilesDetails = new ReelsFilesDetails();

                        reelsFilesDetails.REELS_FILES_DETAILS_ID = Helper.getMaximumId("BUSINESS_FILES_DTEAIL_ID");
                        reelsFilesDetails.REELS_DETAILS_ID = objReelsDetails.REELS_DETAILS_ID;
                        reelsFilesDetails.FILE_PATH = fileName;
                        reelsFilesDetails.CREATED_BY = reelsFilesDetails.MODIFIED_BY = "Team";
                        reelsFilesDetails.CREATED_DATE = reelsFilesDetails.MODIFIED_DATE = Helper.GetCurrentDate();
                        _reelsFilesDetailsRepository.Insert(reelsFilesDetails);
                        _reelsFilesDetailsRepository.Save();
                        Console.WriteLine(fileName);
                    }
                }

            }

            return response;
        }

        public List<ReelsDetails> GetReelsDetails(ReelsDetails objReelsDetails)
        {
            ResponseModel response = new ResponseModel();
            ReelsDetails reelsDetails = new ReelsDetails();
            List<ReelsDetails> obj = new List<ReelsDetails>();
            long practiceCode = AppConfiguration.GetPracticeCode;
            //obj = _businessDetailRepository.GetMany(r => r.EMAIL_ADDRESS == objBusinessDetail.EMAIL_ADDRESS && r.DELETED == false).ToList();


            if (objReelsDetails != null)
            {
                var userId = new SqlParameter("@USER_ID", SqlDbType.VarChar) { Value = objReelsDetails.USER_ID };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                obj = SpRepository<ReelsDetails>.GetListWithStoreProcedure(@"EXEC SP_GET_REELS_DETAILS @USER_ID", userId);
                foreach (var businessDetailId in obj)
                {
                    List<ReelsFilesDetails> filesList = new List<ReelsFilesDetails>();
                    List<ReelsCommentsDetails> reelsCommentsDetails = new List<ReelsCommentsDetails>();
                    var reelsCommentId = new SqlParameter("@REELS_DETAILS_ID", SqlDbType.BigInt) { Value = businessDetailId.REELS_DETAILS_ID };
                    var userID = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objReelsDetails.USER_ID };
                    //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                    reelsCommentsDetails = SpRepository<ReelsCommentsDetails>.GetListWithStoreProcedure(@"EXEC SP_GET_REELS_COMMENTS_DETAILS 
                @REELS_DETAILS_ID, @USER_ID", reelsCommentId, userID);


                    var reelsId = new SqlParameter("@REELS_DETAILS_ID", SqlDbType.BigInt) { Value = businessDetailId.REELS_DETAILS_ID };
                    //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                    filesList = SpRepository<ReelsFilesDetails>.GetListWithStoreProcedure(@"EXEC SP_GET_REELS_FILES_DETAILS @REELS_DETAILS_ID", reelsId);
                    //businessDetailId.uploadedFilesName.AddRange(filesList);
                    List<string> filePaths = filesList.Select(file => "http://localhost:11492/FoxDocumentDirectory/RequestForOrder/UploadImages/" + file.FILE_PATH).ToList();
                    businessDetailId.uploadedFilesName.AddRange(filePaths);
                    businessDetailId.reelsCommentsModelList.AddRange(reelsCommentsDetails);
                }
            }
            return obj;
        }
        public static void AddRemoveCommentsLike(ReelsCommentsDetails objReelsComments)
        {
            if (objReelsComments.isClickOnLike == true)
            {
                ReelsCommentsLikes reelsCommentsLikes = new ReelsCommentsLikes();
                var likeId = new SqlParameter("@COMMENT_LIKE_ID", SqlDbType.BigInt) { Value = Helper.getMaximumId("COMMENT_LIKE_ID") };
                var reelsCommentId = new SqlParameter("@REELS_COMMENTS_DETAILS_ID", SqlDbType.BigInt) { Value = objReelsComments.REELS_COMMENTS_DETAILS_ID };
                var userId = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objReelsComments.USER_ID };
                var action = new SqlParameter("@ACTION", SqlDbType.VarChar) { Value = "Add" };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                reelsCommentsLikes = SpRepository<ReelsCommentsLikes>.GetSingleObjectWithStoreProcedure(@"EXEC SP_ADD_REELS_COMMENTS_LIKES 
                   @COMMENT_LIKE_ID, @REELS_COMMENTS_DETAILS_ID, @USER_ID, @ACTION", likeId, reelsCommentId, userId, action);
            }
            else
            {
                ReelsCommentsLikes reelsCommentsLikes = new ReelsCommentsLikes();
                var likeId = new SqlParameter("@COMMENT_LIKE_ID", SqlDbType.BigInt) { Value = Helper.getMaximumId("COMMENT_LIKE_ID") };
                var reelsCommentId = new SqlParameter("@REELS_COMMENTS_DETAILS_ID", SqlDbType.BigInt) { Value = objReelsComments.REELS_COMMENTS_DETAILS_ID };
                var userId = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objReelsComments.USER_ID };
                var action = new SqlParameter("@ACTION", SqlDbType.VarChar) { Value = "Remove" };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                reelsCommentsLikes = SpRepository<ReelsCommentsLikes>.GetSingleObjectWithStoreProcedure(@"EXEC SP_ADD_REELS_COMMENTS_LIKES 
                   @COMMENT_LIKE_ID, @REELS_COMMENTS_DETAILS_ID, @USER_ID, @ACTION", likeId, reelsCommentId, userId, action);

            }
        }

        public ResponseModel PostComment(ReelsCommentsDetails objReelsComments)
        {
            ResponseModel response = new ResponseModel();
            ReelsCommentsDetails reelsCommentsDetails = new ReelsCommentsDetails();
            long practiceCode = AppConfiguration.GetPracticeCode;
            

            reelsCommentsDetails = _reelsCommentsRepository.GetFirst(r => r.REELS_COMMENTS_DETAILS_ID == objReelsComments.REELS_COMMENTS_DETAILS_ID && r.DELETED == false);
            if (reelsCommentsDetails == null)
            {
                //UserAccount objautomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                objReelsComments.REELS_COMMENTS_DETAILS_ID = Helper.getMaximumId("REELS_COMMENTS_DETAILS_ID");
                if(objReelsComments.LIKE_OR_DISLIKE == true)
                {
                    AddRemoveCommentsLike(objReelsComments);
                }
                if (objReelsComments.IS_REPLAY_COMMENT == true)
                {
                    objReelsComments.COMMENT_REPLAY = objReelsComments.COMMENT;
                    objReelsComments.REPLAY_COMMENT_ID = Helper.getMaximumId("REELS_COMMENTS_DETAILS_ID");
                    objReelsComments.COMMENT = null;
                }
                if(objReelsComments.IS_REPLAY_LIKED == true)
                {
                    objReelsComments.IS_REPLAY_LIKES = objReelsComments.IS_REPLAY_LIKES;
                    objReelsComments.COMMENT_LIKE = 0;
                }

                objReelsComments.CREATED_BY = objReelsComments.MODIFIED_BY = "Team";
                objReelsComments.CREATED_DATE = objReelsComments.MODIFIED_DATE = Helper.GetCurrentDate();
                _reelsCommentsRepository.Insert(objReelsComments);
                _reelsCommentsRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;

            }
            else
            {
                if (objReelsComments.LIKE_OR_DISLIKE == true)
                {
                    AddRemoveCommentsLike(objReelsComments);
                }
                reelsCommentsDetails.USER_ID = objReelsComments.USER_ID;
                reelsCommentsDetails.COMMENT = objReelsComments.COMMENT;
                reelsCommentsDetails.COMMENT_LIKE = objReelsComments.COMMENT_LIKE;
                //if (objReelsComments.IS_REPLAY_LIKED  == true)
                //{
                //    reelsCommentsDetails.IS_REPLAY_LIKES = objReelsComments.IS_REPLAY_LIKES;
                //    objReelsComments.COMMENT_LIKE = 0;
                //}
                reelsCommentsDetails.IS_REPLAY_LIKES = objReelsComments.IS_REPLAY_LIKES;
                reelsCommentsDetails.COMMENT_REPLAY = objReelsComments.COMMENT_REPLAY;
                reelsCommentsDetails.MODIFIED_BY = "Team";
                reelsCommentsDetails.MODIFIED_DATE = Helper.GetCurrentDate();
                _reelsCommentsRepository.Update(reelsCommentsDetails);
                _reelsCommentsRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;
            } 

            return response;
        }

        public List<ReelsCommentsDetails> GetCommentsByReel(ReelsCommentsDetails objReelsDetails)
        {
            List<ReelsCommentsDetails> reelsCommentsDetails = new List<ReelsCommentsDetails>();
            if (objReelsDetails != null)
            {
                var reelsCommentId = new SqlParameter("@REELS_DETAILS_ID", SqlDbType.BigInt) { Value = objReelsDetails.REELS_DETAILS_ID };
                var userId = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objReelsDetails.USER_ID };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                reelsCommentsDetails = SpRepository<ReelsCommentsDetails>.GetListWithStoreProcedure(@"EXEC SP_GET_REELS_COMMENTS_DETAILS 
                @REELS_DETAILS_ID, @USER_ID", reelsCommentId, userId);
            }
            return reelsCommentsDetails;
        }

        public ResponseModel AddUpdateFollower(UserFollowDetails objUserFollowDetails)
        {
            ResponseModel response = new ResponseModel();
            UserFollowDetails userFollowDetails = new UserFollowDetails();
            long practiceCode = AppConfiguration.GetPracticeCode;
            userFollowDetails = _userFollowRepository.GetFirst(r => r.USER_FOLLOWERS_ID == objUserFollowDetails.USER_FOLLOWERS_ID
            && r.USER_ID == objUserFollowDetails.USER_ID
            && r.DELETED == false);

            if (userFollowDetails == null)
            {
                objUserFollowDetails.USER_FOLLOW_ID = Helper.getMaximumId("USER_FOLLOW_ID");
                objUserFollowDetails.CREATED_BY = objUserFollowDetails.MODIFIED_BY = "Team";
                objUserFollowDetails.CREATED_DATE = objUserFollowDetails.MODIFIED_DATE = Helper.GetCurrentDate();
                _userFollowRepository.Insert(objUserFollowDetails);
                _userFollowRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;
            }
            else
            {
                userFollowDetails.MODIFIED_BY = "Team";
                userFollowDetails.MODIFIED_DATE = Helper.GetCurrentDate();
                _userFollowRepository.Update(userFollowDetails);
                _userFollowRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;
            }

            return response;
        }

        public List<UserAccount> GetUserReels(UserAccount objUserAccount)
        {
            ResponseModel response = new ResponseModel();
            BusinessDetail businessDetail = new BusinessDetail();
            List<UserAccount> obj = new List<UserAccount>();
            long practiceCode = AppConfiguration.GetPracticeCode;
            //obj = _businessDetailRepository.GetMany(r => r.EMAIL_ADDRESS == objBusinessDetail.EMAIL_ADDRESS && r.DELETED == false).ToList();


            if (objUserAccount != null)
            {

                var userId = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objUserAccount.APPLICATION_USER_ACCOUNTS_ID };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                obj = SpRepository<UserAccount>.GetListWithStoreProcedure(@"EXEC SP_GET_REELS_USER_DETAILS @USER_ID",userId);
                //}                
            }
            return obj;
        }
        public List<UserAccount> GetReelsUserProfile(UserAccount objUserAccount)
        {
            ResponseModel response = new ResponseModel();
            List<UserAccount> userDetails = new List<UserAccount>();
            if (objUserAccount != null)
            {

                var userId = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objUserAccount.APPLICATION_USER_ACCOUNTS_ID };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                userDetails = SpRepository<UserAccount>.GetListWithStoreProcedure(@"EXEC SP_GET_REELS_USER_PROFILE_DETAILS @USER_ID", userId);
                
                //ResponseModel response = new ResponseModel();
                ReelsDetails reelsDetails = new ReelsDetails();
                List<ReelsDetails> reelsDetailsList = new List<ReelsDetails>();
                List<ReelsDetails> savedReelsDetailsList = new List<ReelsDetails>();
                long practiceCode = AppConfiguration.GetPracticeCode;
                //obj = _businessDetailRepository.GetMany(r => r.EMAIL_ADDRESS == objBusinessDetail.EMAIL_ADDRESS && r.DELETED == false).ToList();


                if (userDetails != null)
                {
                    var reelUserId = new SqlParameter("@USER_ID", SqlDbType.VarChar) { Value = objUserAccount.APPLICATION_USER_ACCOUNTS_ID };
                    //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                    reelsDetailsList = SpRepository<ReelsDetails>.GetListWithStoreProcedure(@"EXEC SP_GET_USER_REELS_DETAILS @USER_ID", reelUserId);
                    foreach (var businessDetailId in reelsDetailsList)
                    {
                        List<ReelsFilesDetails> filesList = new List<ReelsFilesDetails>();
                        List<ReelsCommentsDetails> reelsCommentsDetails = new List<ReelsCommentsDetails>();
                        var reelsCommentId = new SqlParameter("@REELS_DETAILS_ID", SqlDbType.BigInt) { Value = businessDetailId.REELS_DETAILS_ID };
                        var userID = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objUserAccount.APPLICATION_USER_ACCOUNTS_ID  };
                        //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                        reelsCommentsDetails = SpRepository<ReelsCommentsDetails>.GetListWithStoreProcedure(@"EXEC SP_GET_REELS_COMMENTS_DETAILS 
                @REELS_DETAILS_ID, @USER_ID", reelsCommentId, userID);


                        var reelsId = new SqlParameter("@REELS_DETAILS_ID", SqlDbType.BigInt) { Value = businessDetailId.REELS_DETAILS_ID };
                        //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                        filesList = SpRepository<ReelsFilesDetails>.GetListWithStoreProcedure(@"EXEC SP_GET_REELS_FILES_DETAILS @REELS_DETAILS_ID", reelsId);
                        //businessDetailId.uploadedFilesName.AddRange(filesList);
                        List<string> filePaths = filesList.Select(file => "http://localhost:11492/FoxDocumentDirectory/RequestForOrder/UploadImages/" + file.FILE_PATH).ToList();
                        businessDetailId.uploadedFilesName.AddRange(filePaths);
                        businessDetailId.reelsCommentsModelList.AddRange(reelsCommentsDetails);
                    }

                    userDetails[0].UserReelsDetails.AddRange(reelsDetailsList);

                }
                if (userDetails != null)
                {
                    var reelUserId = new SqlParameter("@USER_ID", SqlDbType.VarChar) { Value = objUserAccount.APPLICATION_USER_ACCOUNTS_ID };
                    //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                    savedReelsDetailsList = SpRepository<ReelsDetails>.GetListWithStoreProcedure(@"EXEC SP_GET_USER_SAVED_DETAILS @USER_ID", reelUserId);
                    foreach (var businessDetailId in savedReelsDetailsList)
                    {
                        List<ReelsFilesDetails> filesList = new List<ReelsFilesDetails>();
                        List<ReelsCommentsDetails> reelsCommentsDetails = new List<ReelsCommentsDetails>();
                        var reelsCommentId = new SqlParameter("@REELS_DETAILS_ID", SqlDbType.BigInt) { Value = businessDetailId.REELS_DETAILS_ID };
                        var userID = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objUserAccount.APPLICATION_USER_ACCOUNTS_ID };
                        //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                        reelsCommentsDetails = SpRepository<ReelsCommentsDetails>.GetListWithStoreProcedure(@"EXEC SP_GET_REELS_COMMENTS_DETAILS 
                @REELS_DETAILS_ID, @USER_ID", reelsCommentId, userID);


                        var reelsId = new SqlParameter("@REELS_DETAILS_ID", SqlDbType.BigInt) { Value = businessDetailId.REELS_DETAILS_ID };
                        //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                        filesList = SpRepository<ReelsFilesDetails>.GetListWithStoreProcedure(@"EXEC SP_GET_REELS_FILES_DETAILS @REELS_DETAILS_ID", reelsId);
                        //businessDetailId.uploadedFilesName.AddRange(filesList);
                        List<string> filePaths = filesList.Select(file => "http://localhost:11492/FoxDocumentDirectory/RequestForOrder/UploadImages/" + file.FILE_PATH).ToList();
                        businessDetailId.uploadedFilesName.AddRange(filePaths);
                        businessDetailId.reelsCommentsModelList.AddRange(reelsCommentsDetails);
                    }

                    userDetails[0].UserSavedReelsDetails.AddRange(savedReelsDetailsList);

                }

                //}                
            }
            return userDetails;
        }

        public ReelsDetails GetLikeByReel(ReelsDetails objReelsDetails)
        {
            ReelsDetails reelsLikeDetails = new ReelsDetails();
            if (objReelsDetails != null)
            {
                var reelsCommentId = new SqlParameter("@REELS_DETAILS_ID", SqlDbType.BigInt) { Value = objReelsDetails.REELS_DETAILS_ID };
                var userId = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objReelsDetails.USER_ID };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                reelsLikeDetails = SpRepository<ReelsDetails>.GetSingleObjectWithStoreProcedure(@"EXEC SP_GET_REELS_LIKES_DETAILS 
                @REELS_DETAILS_ID, @USER_ID", reelsCommentId, userId);
            }
            return reelsLikeDetails;
        }
        public ReelSaved SavedPost(ReelSaved objReelSaved)
        {
            if (objReelSaved.isClickOnSave == true)
            {
                ReelSaved reelsSaved = new ReelSaved();
                var likeId = new SqlParameter("@REELS_SAVED_ID", SqlDbType.BigInt) { Value = Helper.getMaximumId("REELS_SAVED_ID") };
                var reelsCommentId = new SqlParameter("@REELS_DETAILS_ID", SqlDbType.BigInt) { Value = objReelSaved.REELS_DETAILS_ID };
                var userId = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objReelSaved.USER_ID };
                var action = new SqlParameter("@ACTION", SqlDbType.VarChar) { Value = "Add" };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                reelsSaved = SpRepository<ReelSaved>.GetSingleObjectWithStoreProcedure(@"EXEC SP_ADD_REELS_SAVED_DETAILS 
                   @REELS_SAVED_ID, @REELS_DETAILS_ID, @USER_ID, @ACTION", likeId, reelsCommentId, userId, action);
            }
            else
            {
                ReelSaved reelsSaved = new ReelSaved();
                var likeId = new SqlParameter("@REELS_SAVED_ID", SqlDbType.BigInt) { Value = Helper.getMaximumId("REELS_SAVED_ID") };
                var reelsCommentId = new SqlParameter("@REELS_DETAILS_ID", SqlDbType.BigInt) { Value = objReelSaved.REELS_DETAILS_ID };
                var userId = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objReelSaved.USER_ID };
                var action = new SqlParameter("@ACTION", SqlDbType.VarChar) { Value = "Remove" };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                reelsSaved = SpRepository<ReelSaved>.GetSingleObjectWithStoreProcedure(@"EXEC SP_ADD_REELS_SAVED_DETAILS 
                   @REELS_SAVED_ID, @REELS_DETAILS_ID, @USER_ID, @ACTION", likeId, reelsCommentId, userId, action);

            }
            return objReelSaved;
        }
        public ResponseModel AddUpdateReelsStatus(ReelsDetails objReelsDetails)
        {
            ResponseModel response = new ResponseModel();
            ReelsDetails businessDetail = new ReelsDetails();
            long practiceCode = AppConfiguration.GetPracticeCode;
            businessDetail = _reelsDetailsRepository.GetFirst(r => r.REELS_DETAILS_ID == objReelsDetails.REELS_DETAILS_ID && r.DELETED == false);
            if (businessDetail == null)
            {
                //UserAccount objautomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                objReelsDetails.REELS_DETAILS_ID = Helper.getMaximumId("BUSINESS_DETAIL_ID");
                objReelsDetails.CREATED_BY = objReelsDetails.MODIFIED_BY = "Team";
                objReelsDetails.CREATED_DATE = objReelsDetails.MODIFIED_DATE = Helper.GetCurrentDate();
                _reelsDetailsRepository.Insert(objReelsDetails);
                _reelsDetailsRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;
                foreach (var fileName in objReelsDetails.uploadedFilesName)
                {
                    ReelsFilesDetails reelsFilesDetails = new ReelsFilesDetails();

                    reelsFilesDetails.REELS_FILES_DETAILS_ID = Helper.getMaximumId("BUSINESS_FILES_DTEAIL_ID");
                    reelsFilesDetails.REELS_DETAILS_ID = objReelsDetails.REELS_DETAILS_ID;
                    reelsFilesDetails.FILE_PATH = fileName;
                    reelsFilesDetails.CREATED_BY = reelsFilesDetails.MODIFIED_BY = "Team";
                    reelsFilesDetails.CREATED_DATE = reelsFilesDetails.MODIFIED_DATE = Helper.GetCurrentDate();
                    _reelsFilesDetailsRepository.Insert(reelsFilesDetails);
                    _reelsFilesDetailsRepository.Save();
                    Console.WriteLine(fileName);
                }

            }
            else
            {
                if (objReelsDetails.isLikeUnlikeReel == true)
                {
                    businessDetail.REEL_LIKES_COUNT = +objReelsDetails.REEL_LIKES_COUNT;
                    AddRemoveReelLike(objReelsDetails);
                }
                if (objReelsDetails.isLikeUnlikeReel == false)
                {
                    businessDetail.USER_TYPE = objReelsDetails.USER_TYPE;
                    businessDetail.EMAIL_ADDRESS = objReelsDetails.EMAIL_ADDRESS;
                }

                businessDetail.MODIFIED_BY = "Team";
                businessDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                _reelsDetailsRepository.Update(businessDetail);
                _reelsDetailsRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;
                if (objReelsDetails.isLikeUnlikeReel == false)
                {
                    foreach (var fileName in businessDetail.uploadedFilesName)
                    {
                        ReelsFilesDetails reelsFilesDetails = new ReelsFilesDetails();

                        reelsFilesDetails.REELS_FILES_DETAILS_ID = Helper.getMaximumId("BUSINESS_FILES_DTEAIL_ID");
                        reelsFilesDetails.REELS_DETAILS_ID = objReelsDetails.REELS_DETAILS_ID;
                        reelsFilesDetails.FILE_PATH = fileName;
                        reelsFilesDetails.CREATED_BY = reelsFilesDetails.MODIFIED_BY = "Team";
                        reelsFilesDetails.CREATED_DATE = reelsFilesDetails.MODIFIED_DATE = Helper.GetCurrentDate();
                        _reelsFilesDetailsRepository.Insert(reelsFilesDetails);
                        _reelsFilesDetailsRepository.Save();
                        Console.WriteLine(fileName);
                    }
                }

            }

            return response;
        }
        public List<ReelsDetails> GetReelsStatus(ReelsDetails objReelsDetails)
        {
            ResponseModel response = new ResponseModel();
            ReelsDetails reelsDetails = new ReelsDetails();
            List<ReelsDetails> obj = new List<ReelsDetails>();
            long practiceCode = AppConfiguration.GetPracticeCode;
            //obj = _businessDetailRepository.GetMany(r => r.EMAIL_ADDRESS == objBusinessDetail.EMAIL_ADDRESS && r.DELETED == false).ToList();


            if (objReelsDetails != null)
            {
                var userId = new SqlParameter("@USER_ID", SqlDbType.VarChar) { Value = objReelsDetails.USER_ID };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                obj = SpRepository<ReelsDetails>.GetListWithStoreProcedure(@"EXEC SP_GET_REELS_STATUS_DETAILS @USER_ID", userId);
                foreach (var businessDetailId in obj)
                {
                    List<ReelsFilesDetails> filesList = new List<ReelsFilesDetails>();
                    List<ReelsCommentsDetails> reelsCommentsDetails = new List<ReelsCommentsDetails>();
                    var reelsCommentId = new SqlParameter("@REELS_DETAILS_ID", SqlDbType.BigInt) { Value = businessDetailId.REELS_DETAILS_ID };
                    var userID = new SqlParameter("@USER_ID", SqlDbType.BigInt) { Value = objReelsDetails.USER_ID };
                    //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                    reelsCommentsDetails = SpRepository<ReelsCommentsDetails>.GetListWithStoreProcedure(@"EXEC SP_GET_REELS_COMMENTS_DETAILS 
                @REELS_DETAILS_ID, @USER_ID", reelsCommentId, userID);


                    var reelsId = new SqlParameter("@REELS_DETAILS_ID", SqlDbType.BigInt) { Value = businessDetailId.REELS_DETAILS_ID };
                    //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                    filesList = SpRepository<ReelsFilesDetails>.GetListWithStoreProcedure(@"EXEC SP_GET_REELS_FILES_DETAILS @REELS_DETAILS_ID", reelsId);
                    //businessDetailId.uploadedFilesName.AddRange(filesList);
                    List<string> filePaths = filesList.Select(file => "http://localhost:11492/FoxDocumentDirectory/RequestForOrder/UploadImages/" + file.FILE_PATH).ToList();
                    businessDetailId.uploadedFilesName.AddRange(filePaths);
                    businessDetailId.reelsCommentsModelList.AddRange(reelsCommentsDetails);
                }
            }
            return obj;
        }

    }
}
