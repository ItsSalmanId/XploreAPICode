﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FoxRehabilitationAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        [ExcludeFromCodeCoverage]
        protected void Application_Start()
        {
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<Models.ApplicationDbContext>(null);
            Database.SetInitializer<Models.TalkRehabDBContext>(null);
        }
        [ExcludeFromCodeCoverage]
        private void Application_EndRequest(Object source, EventArgs e)
        {
            if (Request.Url.AbsoluteUri.Contains("Token")&& Response.StatusCode == 400)
            {
                Response.StatusCode = 200;
            }
        }
    }
}
