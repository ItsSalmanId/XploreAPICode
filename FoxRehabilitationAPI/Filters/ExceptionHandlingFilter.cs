﻿using System;
using System.IO;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using FOX.DataModels.Models.Exceptions;
using BusinessOperations.CommonServices;
using BusinessOperations.CommonService;
using System.Web;
using FoxRehabilitationAPI.Models;
using FOX.DataModels.Models.Security;
using FOX.DataModels;
using System.Diagnostics.CodeAnalysis;

namespace FoxRehabilitationAPI.Filters
{
    [ExcludeFromCodeCoverage]
    public class ExceptionHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            UserProfile profile = ClaimsModel.GetUserProfile(HttpContext.Current.User.Identity as System.Security.Claims.ClaimsIdentity) ?? new UserProfile();
            string exceptionEnvironment = EntityHelper.isTalkRehab ? "Care Cloud Remote" : "Fox Portal";//Environment variable in email by irfan ullah
            try
            {
                var excpParam = JsonConvert.SerializeObject(context.ActionContext.ActionArguments.Values);
                var uri = context.ActionContext.Request.RequestUri.OriginalString;
                var excpMsg = context.Exception.Message;
                if (excpMsg.Contains("it is being used by another process"))
                {
                    return;
                }
                if (excpMsg.Contains("The context cannot be used while the model is being created. This exception may be thrown if the context is used inside the OnModelCreating method"))
                {
                    return;
                }
                var excpStackTrace = context.Exception.StackTrace;
                var excpInnerMessage = ((context.Exception.InnerException != null && context.Exception.InnerException.Message != null) ? (context.Exception.InnerException.Message.ToLower().Contains("inner exception") ? context.Exception.InnerException.InnerException.Message : context.Exception.InnerException.Message) : "NULL");
                //FOX DEV ONLY LOGIC START 
                if (context.Exception is BusinessException && (excpMsg.Contains("google") || excpMsg.Contains("no pages") || excpMsg.Contains("@SEND_TO_ID', which was not supplied.") || excpMsg.Contains("Object reference not set to an instance of an object")))
                {
                    //string directory = System.Web.HttpContext.Current.Server.MapPath(AppConfiguration.ErrorLogPath + "\\FoxBussinessErrors");
                    string directory = System.Web.HttpContext.Current.Server.MapPath("\\FoxDevErrorsLog");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    string filePath = directory + "\\errors_" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".txt";
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("Message: " + excpMsg + Environment.NewLine + Environment.NewLine + "URI: " + uri + Environment.NewLine + Environment.NewLine + "Request parameters: " + excpParam + Environment.NewLine + Environment.NewLine + "StackTrace: " + excpStackTrace + Environment.NewLine + Environment.NewLine +
                           "///------------------Inner Exception------------------///" + Environment.NewLine + excpInnerMessage + Environment.NewLine +
                           "Date: " + DateTime.Now.ToString() + Environment.NewLine + Environment.NewLine + "-------------------------------------------------------||||||||||||---End Current Exception---||||||||||||||||-------------------------------------------------------" + Environment.NewLine);
                        writer.Close();
                    }
                    return;
                }
                //Log Critical errors
                //string directoryOther = System.Web.HttpContext.Current.Server.MapPath(AppConfiguration.ErrorLogPath + "\\FoxCriticalErrors");
                string devdirectoryOther = System.Web.HttpContext.Current.Server.MapPath("\\FoxDevCriticalErrors");
                if (excpMsg.Contains("google") || excpMsg.Contains("The document has no pages"))
                {
                    if (!Directory.Exists(devdirectoryOther))
                    {
                        Directory.CreateDirectory(devdirectoryOther);
                    }
                    string devfilePathOther = devdirectoryOther + "\\errors_" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".txt";
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(devfilePathOther, true))
                        {
                            writer.WriteLine("Message: " + excpMsg + Environment.NewLine + Environment.NewLine + "URI:  " + uri + Environment.NewLine + Environment.NewLine + "Request parameters: " + excpParam + Environment.NewLine + Environment.NewLine + "StackTrace: " + excpStackTrace + Environment.NewLine + Environment.NewLine +
                               "///------------------Inner Exception------------------///" + Environment.NewLine + excpInnerMessage + "" + Environment.NewLine +
                               "Date: " + DateTime.Now.ToString() + Environment.NewLine + Environment.NewLine + "-------------------------------------------------------||||||||||||---End Current Exception---||||||||||||||||-------------------------------------------------------");
                            writer.Close();
                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        Helper.CustomExceptionLog(ex);
                        //Helper.SendExceptionsEmail(ex.Message, ex.ToString(), "Exception occurred in Exception Filter", exceptionEnvironment);
                    }
                }
                //FOX DEV LOGIC CLOSE

                if (context.Exception is BusinessException && (!excpMsg.Contains("google") || !excpMsg.Contains("no pages")))
                {
                    //string directory = System.Web.HttpContext.Current.Server.MapPath(AppConfiguration.ErrorLogPath + "\\FoxBussinessErrors");
                    string directory = System.Web.HttpContext.Current.Server.MapPath("\\FoxBussinessErrors");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    string filePath = directory + "\\errors_" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".txt";
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("Message: " + excpMsg + Environment.NewLine + Environment.NewLine + "URI: " + uri + Environment.NewLine + Environment.NewLine + "Request parameters: " + excpParam + Environment.NewLine + Environment.NewLine + "StackTrace: " + excpStackTrace + Environment.NewLine + Environment.NewLine +
                           "///------------------Inner Exception------------------///" + Environment.NewLine + excpInnerMessage + Environment.NewLine +
                           "Date: " + DateTime.Now.ToString() + Environment.NewLine + Environment.NewLine + "-------------------------------------------------------||||||||||||---End Current Exception---||||||||||||||||-------------------------------------------------------");
                        writer.Close();
                    }
                }
                //Log Critical errors
                //string directoryOther = System.Web.HttpContext.Current.Server.MapPath(AppConfiguration.ErrorLogPath + "\\FoxCriticalErrors");
                if (!excpMsg.Contains("google") || !excpMsg.Contains("no pages"))
                {
                    string directoryOther = System.Web.HttpContext.Current.Server.MapPath("\\FoxCriticalErrors");
                    if (!Directory.Exists(directoryOther))
                    {
                        Directory.CreateDirectory(directoryOther);
                    }
                    string filePathOther = directoryOther + "\\errors_" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".txt";
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(filePathOther, true))
                        {
                            writer.WriteLine("Message: " + excpMsg + Environment.NewLine + Environment.NewLine + "URI:  " + uri + Environment.NewLine + Environment.NewLine + "Request parameters: " + excpParam + Environment.NewLine + Environment.NewLine + "StackTrace: " + excpStackTrace + Environment.NewLine + Environment.NewLine +
                               "///------------------Inner Exception------------------///" + Environment.NewLine + excpInnerMessage + "" + Environment.NewLine +
                               "Date: " + DateTime.Now.ToString() + Environment.NewLine + Environment.NewLine + "-------------------------------------------------------||||||||||||---End Current Exception---||||||||||||||||-------------------------------------------------------");
                            writer.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.CustomExceptionLog(ex);
                        //Helper.SendEmailOnException(ex.Message, ex.ToString(), "Exception occurred in Exception Filter", exceptionEnvironment);
                    }

                }
                if (!uri.Contains("localhost"))
                {
                    var expmsg = Environment.NewLine + "URI:  " + uri + Environment.NewLine + Environment.NewLine + "Request parameters: " + excpParam + Environment.NewLine + Environment.NewLine + "StackTrace: " + excpStackTrace + Environment.NewLine + Environment.NewLine +
                             "///------------------Inner Exception------------------///" + Environment.NewLine + Environment.NewLine + excpInnerMessage + Environment.NewLine + Environment.NewLine + "Date: " + DateTime.Now.ToString()
                             + Environment.NewLine + Environment.NewLine + "-------------------------------------------------------||||||||||||---End Current Exception---||||||||||||||||------------------------------" +
                             "-------------------------" + Environment.NewLine;

                    Helper.CustomExceptionLogTest(expmsg);

                //Helper.SendExceptionsEmail(excpMsg, expmsg.ToString(), "Exception occurred in Exception Filter",exceptionEnvironment);
                }
            }
            catch (Exception ex)
            {
                Helper.CustomExceptionLog(ex);
                //Helper.SendEmailOnException(ex.Message, ex.ToString(), "Exception occurred in Exception Filter", exceptionEnvironment);
            }
        }
    }
}