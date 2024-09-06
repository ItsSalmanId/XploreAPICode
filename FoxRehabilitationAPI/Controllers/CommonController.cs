﻿using BusinessOperations.CommonService;
using BusinessOperations.CommonServices;
using BusinessOperations.Security;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using FoxRehabilitationAPI.Filters;
using FoxRehabilitationAPI.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [ExceptionHandlingFilter]
    [Authorize]
    [ExcludeFromCodeCoverage]
    public class CommonController : BaseApiController
    {
        private readonly ICommonServices _CommonService;
        private string CompleteFilePath = "";
        public CommonController(ICommonServices assignService)
        {
            CompleteFilePath = GetHeaderValues("FilePath");
            _CommonService = assignService;
        }

       
       

        [HttpGet]
        public HttpResponseMessage GeneratePdfFileForSplitRecord1()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private string GetHeaderValues(string Key)
        {
            return HttpContext.Current.Request.Headers.Get(Key) != null ? HttpContext.Current.Request.Headers.GetValues(Key).FirstOrDefault() : "";
        }

        [HttpGet]
        public HttpResponseMessage DownloadSingleFile()
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            string FilePath = CompleteFilePath;
            if (!string.IsNullOrEmpty(FilePath))
            {
                try
                {
                    string fileName = Path.GetFileName(FilePath);
                    string test = Path.GetDirectoryName(FilePath);
                    string fileExtension = Path.GetExtension(FilePath);
                    if (!string.IsNullOrEmpty(fileName) && new[] { ".xml", ".zip", ".jpeg", ".jpg", ".png", ".pdf", ".tiff", ".tif", ".docx", ".doc", ".xls", ".xlsx", ".csv", ".ppt", ".pptx", ".mp3", ".wav",".txt", ".opus" }.Contains(fileExtension))
                    {
                        if (!Directory.Exists(test))
                            FilePath = HttpContext.Current.Server.MapPath("~/" + FilePath);
                        //FilePath = @"\\\\It-bkp\\d\\IT-126\\" + FilePath;                    
                        if (File.Exists(FilePath))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                using (FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                                {
                                    byte[] bytes = new byte[file.Length];
                                    file.Read(bytes, 0, (int)file.Length);
                                    ms.Write(bytes, 0, (int)file.Length);
                                    httpResponseMessage.Content = new ByteArrayContent(bytes.ToArray());
                                    httpResponseMessage.Content.Headers.Add("x-filename", fileName);
                                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                                    httpResponseMessage.StatusCode = HttpStatusCode.OK;
                                    ms.Close();
                                    return httpResponseMessage;

                                }
                            }
                        }
                    }
                    return this.Request.CreateResponse(HttpStatusCode.OK, "File not found.");
                }
                catch
                {
                    throw;
                    //Helper.CustomExceptionLog(ex);
                    //return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                }
            }
            return this.Request.CreateResponse(HttpStatusCode.OK, "Path not found.");
        }

        [HttpGet]
        public HttpResponseMessage GetFiles(string unique_Id)
        {
            var profile = GetProfile();
            var users = _CommonService.GetFiles(unique_Id, profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetAllOriginalFiles(string unique_Id)
        {
            var profile = GetProfile();
            var users = _CommonService.GetAllOriginalFiles(unique_Id, profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            return response;
        }


        [HttpPost]
        public async Task<HttpResponseMessage> Authenticate([FromBody] ValidatePassword obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await FindProfileAsync(GetProfile().UserName, obj.password));
        }

        [HttpGet]
        public HttpResponseMessage GetSenderTypes()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _CommonService.GetSenderTypes(profile));
        }

        [HttpPost]
        public HttpResponseMessage GetSenderNames(ReqGetSenderNamesModel model)
        {
            var senderTyes = _CommonService.GetSenderNames(model, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, senderTyes);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetCityStateByZipCode(string zipCode)
        {
            var result = _CommonService.GetCityStateByZip(zipCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetSmartCities(string city)
        {
            var result = _CommonService.GetSmartCities(city);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetSmartStates(string stateCode)
        {
            var result = _CommonService.GetSmartStates(stateCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetStates()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CommonService.GetStates());
        }
        [HttpGet]
        public HttpResponseMessage GetSplashDetails()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CommonService.IsShowSplash(GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetAlertWindowsDetails()
        {

            return Request.CreateResponse(HttpStatusCode.OK, _CommonService.IsShowAlertWindow(GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage SaveSplashDetails()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _CommonService.SaveSplashDetails(GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage SaveAlertWindowsDetails(CommonAnnouncements objCommonAnnouncements)
        {
            if (objCommonAnnouncements != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _CommonService.SaveAlertWindowsDetails(objCommonAnnouncements, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Alert Windows is empty");
            }
        }
        [HttpGet]
        public HttpResponseMessage DeleteDownloadedFile(string fileLocation)
        {
            if (!string.IsNullOrEmpty(fileLocation))
            {
                return Request.CreateResponse(HttpStatusCode.OK, _CommonService.DeleteDownloadedFile(fileLocation));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "File Location Empty");
            }
        }
    }
}