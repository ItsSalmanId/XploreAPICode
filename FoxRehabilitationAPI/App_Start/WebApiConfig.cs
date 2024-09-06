using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Unity;
using FoxRehabilitationAPI.App_Start;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FoxRehabilitationAPI.Filters;
using System.Diagnostics.CodeAnalysis;
using System.Configuration;

namespace FoxRehabilitationAPI
{
    [ExcludeFromCodeCoverage]
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.DependencyResolver = new UnityResolver(DIContainer.GetContainer());

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new AuthorizationHandlerAttribute());

            string Origins = ConfigurationManager.AppSettings["Origins"];
            var corsAttr = new EnableCorsAttribute(Origins, "*", "*");
            //var corsAttr = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsAttr);
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
               name: "ActionBased",
               routeTemplate: "api/{controller}/{action}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );
            config.MessageHandlers.Add(new WorkAroundForOperationCancelledException());
        }
    }

 
}
