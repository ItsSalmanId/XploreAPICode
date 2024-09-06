using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoxRehabilitationAPI.Controllers
{
    public class HomeController : Controller
    {
        [ExcludeFromCodeCoverage]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
