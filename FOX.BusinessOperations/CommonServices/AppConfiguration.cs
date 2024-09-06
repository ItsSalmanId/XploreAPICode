﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace BusinessOperations.CommonServices
{
    public static class AppConfiguration
    {
        public static bool IsLiveURL
        {

            get
            {
                string isLiveURL = WebConfigurationManager.AppSettings["IsLiveURL"];
                return !string.IsNullOrWhiteSpace(isLiveURL) ? Convert.ToBoolean(isLiveURL) : false;
            }
        }
        public static string ClientURL
        {

            get
            {
                string clientURL = WebConfigurationManager.AppSettings["ClientURL"];
                return !string.IsNullOrWhiteSpace(clientURL) ? clientURL : "http://localhost:4200";
            }
        }
        public static string RequestForOrderUploadImages
        {
            get
            {
                return @"FoxDocumentDirectory\RequestForOrder\UploadImages";
            }
        }
        public static string PHDFilesUploadImages
        {
            get
            {
                return @"FoxDocumentDirectory\PHDAttachmentFiles\UploadImages";
            }
        }
        public static string HrAutoEmailUploadImages
        {
            get
            {
                return @"FoxDocumentDirectory\HRAutoEmailAttachmentFiles";
            }
        }
        public static string ReconciliationOriginalFilesDirectory
        {
            get
            {
                return @"FoxDocumentDirectory\ReconciliationLedgers\OriginalFiles";
            }
        }

        public static string ReconciliationConvertedImagesDirectory
        {
            get
            {
                return @"FoxDocumentDirectory\ReconciliationLedgers\Images";
            }
        }

        public static string PdfPath
        {
            get
            {
                return @"FoxDocumentDirectory\Fox\";
            }
        }
        public static string ImagesPath
        {
            get
            {
                return @"FoxDocumentDirectory\Fox\Images";
            }
        }
        public static string PatientSurveyUploadFiles
        {
            get
            {
                return @"FoxDocumentDirectory\PatientSurvey\UploadFiles";
            }
        }
        public static string HREmailUploadFiles
        {
            get
            {
                return @"FoxDocumentDirectory\HRAutoEmailsUploadedFiles\UploadFiles";
            }
        }
        public static string SupervisorEmailAddress
        {
            get
            {
                return "StrategicAccounts@foxrehab.org";
            }
        }
        public static long GetPracticeCode
        {
            get
            {
                string getPracticeCode = WebConfigurationManager.AppSettings["GetPracticeCode"];
                if (!string.IsNullOrWhiteSpace(getPracticeCode))
                {
                    long _lg;
                    long.TryParse(getPracticeCode, out _lg);
                    return _lg;
                }
                return 1011163;
                //return 1012714;
            }
        }
        public static string ACUServiceURL
        {
            get
            {
                string _ACUServiceURL = WebConfigurationManager.AppSettings["ACUServiceURL"].ToString();
                if (!string.IsNullOrWhiteSpace(_ACUServiceURL))
                {
                    return _ACUServiceURL;
                }
                return "https://uat-webservices.mtbc.com/ACU-WebAPI/api/FoxSurveyCalls/MakeCall/"; // For live.
                //return "http://172.16.0.71/ACU_WebAPI/api/FoxSurveyCalls/MakeCall/"; // For QA and UAT.

            }
        }
        public static string ACUServiceTeamId
        {
            get
            {
                //return "BC0A6BAC-8536-432E-8A8F-3D1B6310D9BD"; // For QA and UAT.
                return "D74AB142-BDF8-4C49-9DD7-5D659B392E24"; // For live.
            }
        }
        public static string ACUServiceTeamIdFor8381
        {
            get
            {
                //return "BC0A6BAC-8536-432E-8A8F-3D1B6310D9BD"; // For QA and UAT.
                return "27640DF1-249C-461B-8EA4-1899CA7D4D6C"; // For live.
            }
        }
        public static string ACUServiceTeamIdFor8384
        {
            get
            {
                //return "BC0A6BAC-8536-432E-8A8F-3D1B6310D9BD"; // For QA and UAT.
                return "41892460-2AEA-4186-A9BC-437D97D1C25B"; // For live.
            }
        }
        public static string NPIRegistryURL
        {
            get
            {
                return "https://npiregistry.cms.hhs.gov/api/?version=2.1";
            }
        }
        public static string ExportedFilesPath
        {
            get
            {
                return @"FoxDocumentDirectory\ExportedFiles";
            }
        }
        //public static string ErrorLogPath => @"FoxDocumentDirectory\ErrorLogs";

        //Google Capcha Secret Key
        public static string SecretKey
        {
            get
            {
                string secretKey = WebConfigurationManager.AppSettings["SecretKey"];
                return secretKey;
            }
        }

        public static string PHRRoutingLink
        {
            get
            {
                //Dev
                //return @"http://10.10.30.64:4324/FOXPHR/VerifyID?info=";

                //QA
                //return @"http://drdemo4.mtbc.com/FOXPHR/VerifyID?info=";
                //UAT
                //return @"https://phruat.foxrehab.org/FOXPHR/VerifyID?info=";
                //Staging
                //return @"https://stagingphr.foxrehab.org/FOXPHR/VerifyID?info=";
                //Live
                return @"https://phr.foxrehab.org/FOXPHR/VerifyID?info=";
            }
        }

        public static string TasksAttachmentsPath => $@"FoxDocumentDirectory\TaskAttachments\{DateTime.Now.Year}\{DateTime.Now.ToString("MMM")}\{DateTime.Now.Day}";
        public static string DocumentAttachmentsPath => $@"FoxDocumentDirectory\DocumentAttachments\{DateTime.Now.Year}\{DateTime.Now.ToString("MMM")}\{DateTime.Now.Day}";

        //public static string ReconsiliationLedgerPath => $@"FoxDocumentDirectory\ReconsiliationLeger\{DateTime.Now.Year}\{DateTime.Now.ToString("MMM")}\{DateTime.Now.Day}";

        public static List<string> SendADExceptionCCList => new List<string>
        {
          
            "abdulsattar@carecloud.com",
            "aftabkhan@carecloud.com",
            "muhammadsalman7@carecloud.com",
        };

        public static string SendADExceptionTo => "abdulsattar@carecloud.com";
        //Active Directory
        public static class ActiveDirectoryViewModel
        {
            public static List<ADDetail> ADDetailList { get; set; } = new List<ADDetail>
            {
                new ADDetail("10.10.30.214", "rmb.com", 105, 1012714),
                //new ADDetail("ISB.MTBC.COM", "mtbc.com", 105, 1011163),
                new ADDetail("192.168.13.6", "gulfcoastbilling.com", 105, 1012714),
                //new ADDetail("72.82.236.130", "foxrehab.org", 105, 1012714), //Live AD server for FOX
                new ADDetail("40.143.53.71", "foxrehab.org", 105, 1012714) //Live Backup server for FOX
            };

            public class ADDetail
            {
                public string DomainURL { get; set; }
                public string DomainForSearch { get; set; }
                public long RoleId { get; set; }
                public long PracticeCode { get; set; }

                public ADDetail(string domainURL, string domainForSearch, long roleId, long practiceCode)
                {
                    DomainURL = domainURL;
                    DomainForSearch = domainForSearch;
                    RoleId = roleId;
                    PracticeCode = practiceCode;
                }
            }
        }

        public static string LogADUserStatus => $@"FOXLocalDocuments\ADUsersLogs\{DateTime.Now.Year}\{DateTime.Now.ToString("MMM")}";
        public static string QRCodeTempPath => $@"FOXLocalDocuments\TemporaryFiles\QRCode\{DateTime.Now.Year}\{DateTime.Now.ToString("MMM")}\{DateTime.Now.Day}\";
        public static string LogADProperties => $@"FOXLocalDocuments\ADProperties\";
        //Below are the email addresses on which email is send when a new user is created
        //For Live Only
        public static string SendEmailToAdminOnExternalUserSignUp_To
        {
            get
            {
                return "abdulsattar@carecloud.com";
            }
        }
        public static string SendEmailToAdminOnExternalUserSignUp_CC
        {
            get
            {
                return "support@foxrehab.org";
            }
        }


        //Below are the email addresses on which email is send when a new user is created
        //For All other environment
        //public static string SendEmailToAdminOnExternalUserSignUp_To => "yasiramin@mtbc.com";
        //public static string SendEmailToAdminOnExternalUserSignUp_CC => "waqarmirza@mtbc.com";
        public static string AppAccessTokenLink
        {
            get
            {
                //QA
                //return @"https://qa-webservices.mtbc.com/Fox/api/Fox/GetAccessToken";

                //UAT
                //return @"http://uat-webservices.mtbc.com/Fox/api/Fox/GetAccessToken";

                //Live
                return @"https://mhealth.mtbc.com/Fox/api/Fox/GetAccessToken";
            }
        }

        public static string AppLatLongLink
        {
            get
            {
                //QA
                //return @"https://qa-webservices.mtbc.com/Fox/api/User/GetLatLong?Address=";

                //UAT
                //return @"http://uat-webservices.mtbc.com/Fox/api/User/GetLatLong?Address=";

                //Live
                return @"https://mhealth.mtbc.com/Fox/api/User/GetLatLong?Address=";
            }
        }
        //public static string AppAccessTokenLink => $"https://mhealth.mtbc.com/Fox/api/Fox/GetAccessToken";

        //public static string AppLatLongLink => $"https://mhealth.mtbc.com/Fox/api/User/GetLatLong?Address=";

        public static int InvalidAttemptsCountToBlockUser
        {
            get
            {
                return 6;
            }
        }

        public static string SendEmailToQAOnExternalUserSignUp_To
        {
            get
            {
                return @"abdulsattar@carecloud.com";
            }
        }
        public static string NPPESNPIRegistry
        {
            get
            {
                return "https://npiregistry.cms.hhs.gov/api/?version=2.1";
            }
        }
    }
}