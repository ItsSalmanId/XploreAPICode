﻿using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using BusinessOperations.Security;
using FOX.DataModels.Models.SenderType;
using FOX.DataModels.Models.SenderName;
using iTextSharp.text;
using SelectPdf;
using iTextSharp.text.pdf;
using FOX.DataModels.Models.ExternalUserModel;
using BusinessOperations.CommonService;
using FOX.DataModels.Models.StatesModel;
using FOX.DataModels.Models.ServiceConfiguration;
using System.Web.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Security.Cryptography;
using System.Text;
using SautinSoft;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace BusinessOperations.CommonServices
{
    public class CommonServices : ICommonServices
    {
        //private readonly DBContextQueue _QueueContext = new DBContextQueue();
        //private readonly DbContextCommon _DbContextCommon = new DbContextCommon();
        private readonly GenericRepository<FOX_TBL_SENDER_TYPE> _FOX_TBL_SENDER_TYPE;
        private readonly GenericRepository<FOX_TBL_SENDER_NAME> _FOX_TBL_SENDER_NAME;
        private readonly GenericRepository<Zip_City_State> _zipCityStateRepository;
        private readonly GenericRepository<States> _statesRepository;
        private readonly GenericRepository<Provider> _providerRepository;
        private readonly GenericRepository<EmailFaxLog> _emailfaxlogRepository;
        private readonly GenericRepository<Splash> _splashRepository;
        private readonly GenericRepository<CommonAnnouncements> _announcementsRepository;
        private readonly GenericRepository<AnnouncementsHistory> _announcementsHistoryRepository;


        public CommonServices()
        {
            //_QueueRepository = new GenericRepository<OriginalQueue>(_QueueContext);
            //_OriginalQueueFilesRepository = new GenericRepository<OriginalQueueFiles>(_QueueContext);
            //_IndexInfoDocRepository = new GenericRepository<FOX_TBL_PATIENT_DOCUMENTS>(_QueueContext);
            //_FOX_TBL_SENDER_TYPE = new GenericRepository<FOX_TBL_SENDER_TYPE>(_DbContextCommon);
            //_FOX_TBL_SENDER_NAME = new GenericRepository<FOX_TBL_SENDER_NAME>(_DbContextCommon);
            //_zipCityStateRepository = new GenericRepository<Zip_City_State>(_DbContextCommon);
            //_notesRepository = new GenericRepository<FOX_TBL_NOTES>(_DbContextCommon);
            //_notesTypeRepository = new GenericRepository<FOX_TBL_NOTES_TYPE>(_DbContextCommon);
            //_statesRepository = new GenericRepository<States>(_DbContextCommon);
            //_providerRepository = new GenericRepository<Provider>(_DbContextCommon);
            //_emailfaxlogRepository = new GenericRepository<EmailFaxLog>(_DbContextCommon);
            //_splashRepository = new GenericRepository<Splash>(_DbContextCommon);
            //_announcementsRepository = new GenericRepository<CommonAnnouncements>(_DbContextCommon);
            //_announcementsHistoryRepository = new GenericRepository<AnnouncementsHistory>(_DbContextCommon);
            //_foxRolesRepository = new GenericRepository<FoxRoles>(_DbContextCommon);
        }
        [ExcludeFromCodeCoverage]
        private string HTMLToPDFSautinsoft(string htmlString, string fileName, string linkMessage = null)
        {
            try
            {
                fileName = fileName.Substring(0, fileName.LastIndexOf(".")) + "cover.pdf";
                PdfMetamorphosis p = new PdfMetamorphosis();
                //p.Serial = "10262870570";//server
                p.Serial = "10261942764";//development
                p.PageSettings.Size.A4();
                p.PageSettings.Orientation = PdfMetamorphosis.PageSetting.Orientations.Portrait;
                p.PageSettings.MarginLeft.Inch(0.1f);
                p.PageSettings.MarginRight.Inch(0.1f);
                if (p != null)
                {
                    if (p.HtmlToPdfConvertStringToFile(htmlString, fileName) == 0)
                    {
                        return fileName;
                    }
                    else
                    {
                        var ex = p.TraceSettings.ExceptionList.Count > 0 ? p.TraceSettings.ExceptionList[0] : null;
                        var msg = ex != null ? ex.Message + Environment.NewLine + ex.StackTrace : "An error occured during converting HTML to PDF!";
                        return "";
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public List<CommonFilePath> GetFiles(string uniqueId, long practiceCode)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var parmWorkId = new SqlParameter("UNIQUE_ID", SqlDbType.VarChar) { Value = uniqueId };
            var queue = SpRepository<CommonFilePath>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_ALL_FILES]  @PRACTICE_CODE, @UNIQUE_ID",
                parmPracticeCode, parmWorkId);
            return queue;
        }

        public List<CommonFilePath> GetAllOriginalFiles(string uniqueId, long practiceCode)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var parmWorkId = new SqlParameter("UNIQUE_ID", SqlDbType.VarChar) { Value = uniqueId };
            var queue = SpRepository<CommonFilePath>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_ALL_ORIGINAL_FILES]  @PRACTICE_CODE, @UNIQUE_ID",
                parmPracticeCode, parmWorkId);
            return queue;
        }

        public bool Authenticate(string password, UserProfile profile)
        {
            string EncPass = Encrypt.getEncryptedCode(password);
            var userName = new SqlParameter("USERNAME", SqlDbType.VarChar) { Value = profile.UserName };
            var Pass = new SqlParameter("PASSWORD", SqlDbType.VarChar) { Value = EncPass };
            var user = new User();
            try
            {
                user = SpRepository<User>.GetSingleObjectWithStoreProcedure(@"exec [FOX_PROC_AUTHENTICATE_USER] @USERNAME,@PASSWORD", userName, Pass);

                if (user.STATUS == 200) // VALID USER
                {
                    var UserName = new SqlParameter("USERNAME", SqlDbType.VarChar) { Value = profile.UserName };
                    var userEntity = SpRepository<User>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_USER @USERNAME", UserName);

                    if (userEntity.PASSWORD != null && userEntity.USER_NAME != null && userEntity.IS_ACTIVE != false)
                    {
                        return true;
                    }
                    else
                    {

                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ResponseGetSenderTypesModel GetSenderTypes(UserProfile profile)
        {
            try
            {
                List<FOX_TBL_SENDER_TYPE> senderTypeList = new List<FOX_TBL_SENDER_TYPE>();
                //if (profile.isTalkRehab)
                //{
                //    senderTypeList = _FOX_TBL_SENDER_TYPE.GetMany(t => !t.DELETED && t.DISPLAY_ORDER != null)
                //    .OrderBy(t => t.DISPLAY_ORDER)
                //    //.OrderBy(t => t.SENDER_TYPE_NAME)
                //    .ToList();
                //}
                //else
                //{

                senderTypeList = _FOX_TBL_SENDER_TYPE.GetMany(t => t.PRACTICE_CODE == profile.PracticeCode && !t.DELETED && t.DISPLAY_ORDER != null)
                .OrderBy(t => t.DISPLAY_ORDER)
                //.OrderBy(t => t.SENDER_TYPE_NAME)
                .ToList();

                // }
                return new ResponseGetSenderTypesModel() { SenderTypeList = senderTypeList, ErrorMessage = "", Message = "Get Sender Types List Successfully.", Success = true };
            }
            catch (Exception exception)
            {
                //throw exception;
                return new ResponseGetSenderTypesModel() { SenderTypeList = null, ErrorMessage = exception.ToString(), Message = "We encountered an error while processing your request.", Success = false };
            }
        }

        public ResponseGetSenderNamesModel GetSenderNames(ReqGetSenderNamesModel model, UserProfile profile = null)
        {
            long practiceCode = profile?.PracticeCode ?? (model?.PracticeCode ?? 0);
            string userName = profile?.UserName ?? (model?.UserName ?? "");

            if (string.IsNullOrWhiteSpace(model?.SearchValue ?? ""))
            {
                model.SearchValue = "";
            }
            var senderNameList = _FOX_TBL_SENDER_NAME.GetMany(
                                    t => t.PRACTICE_CODE == practiceCode
                                        && !t.DELETED
                                        && t.FOX_TBL_SENDER_TYPE_ID == model.SenderTypeId
                                        && (
                                            t.SENDER_NAME_CODE.Contains(model.SearchValue)
                                            || t.SENDER_NAME_DESCRIPTION.Contains(model.SearchValue)
                                        )
                                    )
                                    .Take(30)
                                    .ToList();

            var senderName = _FOX_TBL_SENDER_NAME.GetFirst(
                                    t => t.PRACTICE_CODE == practiceCode
                                        && !t.DELETED
                                        && t.SENDER_NAME_CODE.Equals(userName)
                                    );         
            if (senderName != null)
            {
                senderNameList.Insert(0, senderName);
            }

            return new ResponseGetSenderNamesModel() { SenderNameList = senderNameList, ErrorMessage = "", Message = "Get Sender Name List Successfully.", Success = true };
        }

        public string AddCoverPageForFax(string filePath, string fileName, string coverLetterTemplate)
        {
            try
            {
                string workOrderPDFpath = Path.Combine(filePath, fileName);
                string coverLetterPDFPath = HTMLToPDFSautinsoft(coverLetterTemplate, workOrderPDFpath);
                if (!string.IsNullOrEmpty(coverLetterPDFPath))
                {
                    using (var ms = new MemoryStream())
                    {
                        var newDocument = new Document(PageSize.A4, 0, 0, 0, 0);
                        newDocument.Open();
                        PdfWriter.GetInstance(newDocument, ms).SetFullCompression();

                        PdfReader coverPDF = new PdfReader(coverLetterPDFPath);
                        PdfReader workOrderPDF = new PdfReader(workOrderPDFpath);
                        fileName = fileName.Substring(0, fileName.LastIndexOf(".")) + "workorderwithcover.pdf";
                        string coverwithPagesPdfPath = Path.Combine(filePath, fileName);
                        //PdfCopy copy = new PdfCopy(newDocument, new FileStream(coverwithPagesPdfPath, FileMode.OpenOrCreate));
                        //copy.AddDocument(coverPDF);
                        //copy.AddDocument(workOrderPDF);
                        //newDocument.Close();
                        //coverPDF.Close();
                        //workOrderPDF.Close();

                        PdfStamper stamper = new PdfStamper(workOrderPDF, new FileStream(coverwithPagesPdfPath, FileMode.Create));
                        stamper.InsertPage(1, coverPDF.GetPageSizeWithRotation(1));
                        PdfContentByte page1 = stamper.GetOverContent(1);
                        PdfImportedPage page = stamper.GetImportedPage(coverPDF, 1);
                        page1.AddTemplate(page, 0, 0);
                        stamper.Close();
                        coverPDF.Close();
                        workOrderPDF.Close();
                    }
                    return fileName;
                }
                else
                {
                    throw new Exception("Unable to convert cover letter to PDF!");
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        [ExcludeFromCodeCoverage]
        private string HTMLToPDF(string coverTemplate, string path)
        {
            try
            {
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(coverTemplate);

                HtmlToPdf converter = new HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.MarginBottom = 10;
                converter.Options.MarginTop = 10;
                converter.Options.MarginLeft = 10;
                converter.Options.MarginRight = 10;
                converter.Options.DisplayFooter = false;
                converter.Options.DisplayHeader = false;
                converter.Options.WebPageWidth = 768;
                converter.Options.InternalLinksEnabled = true;
                converter.Options.ExternalLinksEnabled = true;
                //PdfTextSection text = new PdfTextSection(10, 10, "",
                //    new System.Drawing.Font("Arial", 10));

                // footer settings
                converter.Options.DisplayFooter = false;
                //converter.Footer.Height = 50;
                //converter.Footer.Add(text);

                SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlDoc.DocumentNode.OuterHtml);
                string coverPdfPath = path.Substring(0, path.LastIndexOf(".")) + "cover.pdf";
                //// save pdf document
                doc.Save(coverPdfPath);
                // close pdf document
                doc.Close();
                return coverPdfPath;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<ZipCityState> GetCityStateByZip(string zipCode)
        {
            if (zipCode.Contains("-"))
            {
                zipCode = zipCode.Replace("-", "");
            }
            var _zipCode = new SqlParameter { ParameterName = "@zipCode", Value = zipCode };
            var result = SpRepository<ZipCityState>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_CITY_STATE_BY_ZIP_CODE @zipCode", _zipCode);
            return result;
        }

        public List<CityStateModel> GetSmartCities(string city)
        {
            try
            {
                return _zipCityStateRepository.GetManyQueryable(e => e.City_Name.Contains(city) && !e.Deleted).GroupBy(g => g.City_Name).Select(w => new CityStateModel { NAME = w.Key }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CityStateModel> GetSmartStates(string stateCode)
        {
            try
            {
                return _zipCityStateRepository.GetManyQueryable(e => e.State_Code.Contains(stateCode) && !e.Deleted).GroupBy(g => g.State_Code).Select(w => new CityStateModel { NAME = w.Key }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<States> GetStates()
        {
            return _statesRepository.GetMany(row => row.Deleted != true).OrderBy(r => r.State_Code).ToList();
        }

        public Provider GetProvider(long providerId, UserProfile profile)
        {
            return _providerRepository.GetFirst(row => row.PRACTICE_CODE == profile.PracticeCode && row.FOX_PROVIDER_ID == providerId);
        }
        // Splash will be showing on the basics of user id 
        public bool IsShowSplash(UserProfile userProfile)
        {
            if (userProfile != null && !string.IsNullOrEmpty(userProfile.UserName))
            {
                var splashType = WebConfigurationManager.AppSettings["SplashType"];
                var splashUser = _splashRepository.GetFirst(s => s.DELETED == false && s.USER_ID == userProfile.userID && s.USER_NAME == userProfile.UserName && s.SPLASH_TYPE == splashType);
                if (splashUser != null && splashUser.SHOW_COUNT > 0)
                {
                    return false;
                }
                else
                {
                    DateTime start = Convert.ToDateTime(WebConfigurationManager.AppSettings["SplashStartTime"]);
                    DateTime end = Convert.ToDateTime(WebConfigurationManager.AppSettings["SplashEndTime"]);
                    var currentDateTime = DateTime.Now;
                    if (currentDateTime > start && currentDateTime < end)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // Description: This function is trigger to Splash will be showing on the basics of user id 
        public CommonAnnouncements IsShowAlertWindow(UserProfile userProfile)
        {
            ResponseModel response = new ResponseModel();
            CommonAnnouncements announcementsListWithData = new CommonAnnouncements();
            if (userProfile != null && userProfile.PracticeCode != 0 && !string.IsNullOrEmpty(userProfile.UserName))
            {
                var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = userProfile.PracticeCode };
                SqlParameter RoleId = new SqlParameter { ParameterName = "ROLE_ID", SqlDbType = SqlDbType.VarChar, Value = userProfile.RoleId };
                var announcement = SpRepository<CommonAnnouncements>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_ANNOUNCEMENT_DETAILS_FOR_POPUP  @PRACTICE_CODE, @ROLE_ID", PracticeCode, RoleId);
                if (announcement != null)
                {
                    SqlParameter AnnouncmentID = new SqlParameter("ANNOUNCEMENT_ID", announcement.ANNOUNCEMENT_ID);
                    SqlParameter PracticeCodeHistory = new SqlParameter("PRACTICE_CODE", userProfile.PracticeCode);
                    SqlParameter roleId = new SqlParameter("ROLE_ID", userProfile.RoleId);
                    SqlParameter UserName = new SqlParameter { ParameterName = "USER_NAME", SqlDbType = SqlDbType.VarChar, Value = userProfile.UserName };
                    announcementsListWithData = SpRepository<CommonAnnouncements>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_ANNOUNCEMENT_HISTORY_DETAILS @PRACTICE_CODE, @ANNOUNCEMENT_ID, @ROLE_ID, @USER_NAME", PracticeCodeHistory, AnnouncmentID, roleId, UserName);
                }
                if (announcementsListWithData == null)
                {
                    announcement.ANNOUNCEMENT_DETAILS = !string.IsNullOrEmpty(announcement.ANNOUNCEMENT_DETAILS) ? announcement.ANNOUNCEMENT_DETAILS.TrimStart().Replace("• ", "") : "";
                    announcement.ANNOUNCEMENT_DETAILS = announcement.ANNOUNCEMENT_DETAILS;
                    announcement.ANNOUNCEMENT_DETAILS = announcement.ANNOUNCEMENT_DETAILS.Replace("\n\n", "\n");
                    if (announcement.ANNOUNCEMENT_DETAILS.EndsWith("\n"))
                    {
                        announcement.ANNOUNCEMENT_DETAILS = announcement.ANNOUNCEMENT_DETAILS.TrimEnd('\n');
                    }
                    List<string> splitted = announcement.ANNOUNCEMENT_DETAILS.Split('\n').ToList();
                    announcement.SplittedBulletsPoints = splitted;
                    return announcement;
                }
                else
                {
                    announcementsListWithData = null;
                    return announcementsListWithData;
                }
            }
            else
            {
                announcementsListWithData = null;
                return announcementsListWithData;
            }
        }
        // Description: This function is trigger to data of alert window will be save to db
        public ResponseModel SaveAlertWindowsDetails(CommonAnnouncements objCommonAnnouncements, UserProfile userProfile)
        {
            ResponseModel response = new ResponseModel();
            if (userProfile != null && !string.IsNullOrEmpty(userProfile.UserName) && userProfile.PracticeCode != 0 && userProfile.userID != 0)
            {
                var getAlertWindowResponse = _announcementsHistoryRepository.GetFirst(s => s.DELETED == false && s.USER_ID == userProfile.userID && s.USER_NAME == userProfile.UserName);
                if (getAlertWindowResponse == null)
                {
                    long primaryKey = Helper.getMaximumId("FOX_TBL_ANNOUNCEMENT_HISTORY");
                    SqlParameter AnnouncmentHistoryId = new SqlParameter("ANNOUNCEMENT_HISTORY_ID", primaryKey);
                    SqlParameter AnnouncmentId = new SqlParameter("ANNOUNCEMENT_ID", objCommonAnnouncements.ANNOUNCEMENT_ID);
                    SqlParameter UserId = new SqlParameter("USER_ID", objCommonAnnouncements.ROLE_ID);
                    SqlParameter UserName = new SqlParameter("USER_NAME", userProfile.UserName);
                    SqlParameter ShowCount = new SqlParameter("SHOW_COUNT", 1);
                    SqlParameter ModifiedDate = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                    SqlParameter CreatedDate = new SqlParameter("CREATED_DATE", Helper.GetCurrentDate());
                    SqlParameter PracticeCode = new SqlParameter("PRACTICE_CODE", userProfile.PracticeCode);
                    SqlParameter Deleted = new SqlParameter("DELETED", false);
                    SqlParameter CreatedBy = new SqlParameter("CREATED_BY", userProfile.PracticeCode);
                    SqlParameter Operation = new SqlParameter("OPERATION", "ADD");
                    SpRepository<AnnouncementsHistory>.GetListWithStoreProcedure(@"exec FOX_PROC_CRUD_ANNOUNCEMENT_HISTORY @ANNOUNCEMENT_HISTORY_ID, @ANNOUNCEMENT_ID, @USER_ID, @USER_NAME, @SHOW_COUNT ,@MODIFIED_DATE, @CREATED_DATE, @PRACTICE_CODE, @DELETED, @CREATED_BY, @Operation", AnnouncmentHistoryId, AnnouncmentId, UserId, UserName, ShowCount, ModifiedDate, CreatedDate, PracticeCode, Deleted, CreatedBy, Operation);
                }
                else
                {
                    getAlertWindowResponse.SHOW_COUNT = getAlertWindowResponse.SHOW_COUNT + 1;
                    _announcementsHistoryRepository.Update(getAlertWindowResponse);
                    _announcementsHistoryRepository.Save();
                }
            }
            return response;
        }
        // Data of splash screen will be save to db
        public bool SaveSplashDetails(UserProfile userProfile)
        {
            if (userProfile != null && !string.IsNullOrEmpty(userProfile.UserName))
            {
                var getSplashResponse = _splashRepository.GetFirst(s => s.DELETED == false && s.USER_ID == userProfile.userID && s.USER_NAME == userProfile.UserName);
                if (getSplashResponse == null)
                {
                    Splash splashObj = new Splash();
                    splashObj.FOX_SPLASH_ID = Helper.getMaximumId("FOX_SPLASH_ID");
                    splashObj.USER_ID = userProfile.userID;
                    splashObj.USER_NAME = userProfile.UserName;
                    splashObj.SPLASH_TYPE = WebConfigurationManager.AppSettings["SplashType"];
                    splashObj.CREATED_BY = splashObj.MODIFIED_BY = "FOX_TEAM";
                    splashObj.CREATED_DATE = splashObj.MODIFIED_DATE = Helper.GetCurrentDate();
                    splashObj.SHOW_COUNT = 1;
                    _splashRepository.Insert(splashObj);
                    _splashRepository.Save();
                }
                else
                {
                    getSplashResponse.SHOW_COUNT = getSplashResponse.SHOW_COUNT + 1;
                    _splashRepository.Update(getSplashResponse);
                    _splashRepository.Save();
                }
            }
            return true;
        }
        // Delete Files From Server.

        public ResponseModel DeleteDownloadedFile(string fileLocation)
        {
            ResponseModel response = new ResponseModel();
            if (!string.IsNullOrEmpty(fileLocation))
            {
                fileLocation = Encrypt.DecrypStringEncryptedInClient(fileLocation);
                var completeFilePath = HttpContext.Current?.Server?.MapPath("~/" + fileLocation);
                if (!string.IsNullOrEmpty(completeFilePath) && File.Exists(Path.Combine(completeFilePath)))
                {
                    File.Delete(Path.Combine(completeFilePath));
                    response.Success = true;
                    response.Message = "Successfully deleted files";
                }
                else
                {
                    response.Success = false;
                    response.Message = "File not found";
                }
            }
            return response;
        }
    }
}