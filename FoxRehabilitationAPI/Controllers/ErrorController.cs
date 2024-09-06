using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoxRehabilitationAPI.Controllers
{
    [ExcludeFromCodeCoverage]
    public class ErrorController : Controller
    {
        // GET: Error
        //public ActionResult Index()
        //{
        //    return View();
        //}
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View();
        }
        public ViewResult InternalServerError()
        {
            Response.StatusCode = 500;  //you may want to set this to 200
            return View();
        }
    }
}