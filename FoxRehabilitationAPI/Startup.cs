//using System;
//using System.IO;
//using Microsoft.Owin;
//using Owin;

//[assembly: OwinStartup(typeof(FoxRehabilitationAPI.Startup))]

//namespace FoxRehabilitationAPI
//{
//    public partial class Startup
//    {
//        public void Configuration(IAppBuilder app)
//        {
//            ConfigureAuth(app);
//            ConfigureStaticFiles(app);
//        }

//        private void ConfigureStaticFiles(IAppBuilder app)
//        {
//            // Middleware to handle serving static files
//            app.Use(async (context, next) =>
//            {
//                var requestPath = context.Request.Path;

//                // Check if the request path starts with "/UploadImages"
//                if (requestPath.StartsWithSegments(new PathString("/UploadImages"), out var remainingPath))
//                {
//                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FoxDocumentDirectory/RequestForOrder/UploadImages", remainingPath.Value.TrimStart('/'));

//                    // Check if the file exists
//                    if (File.Exists(filePath))
//                    {
//                        context.Response.ContentType = GetContentType(filePath); // Set appropriate content type based on file type
//                        await context.Response.WriteAsync(File.ReadAllBytes(filePath));
//                        return;
//                    }
//                }

//                // Call the next middleware in the pipeline
//                await next();
//            });
//        }
//        private string GetContentType(string filePath)
//        {
//            // Simple content type detection based on file extension
//            var extension = Path.GetExtension(filePath).ToLowerInvariant();

//            switch (extension)
//            {
//                case ".jpg":
//                    return "image/jpeg";
//                case ".png":
//                    return "image/png";
//                case ".gif":
//                    return "image/gif";
//                default:
//                    return "application/octet-stream";
//            }
//        }
//    }
//}



using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.IO;
using System.Text;
using Microsoft.Owin.Cors;
using System.Web.Cors;



[assembly: OwinStartup(typeof(FoxRehabilitationAPI.Startup))]

namespace FoxRehabilitationAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Define a custom CORS policy
            var corsPolicy = new CorsPolicy
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                SupportsCredentials = true
            };

            // Add allowed origins based on environment
            corsPolicy.Origins.Add("http://localhost:4200"); // Local development origin
            //corsPolicy.Origins.Add("https://45.61.134.14"); // Production origin

            var corsOptions = new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => System.Threading.Tasks.Task.FromResult(corsPolicy)
                }
            };

            // Ensure CORS is applied before other middleware
            app.UseCors(corsOptions);

            // Debugging: Log headers
            app.Use(async (context, next) =>
            {
                await next.Invoke();
                var responseHeaders = context.Response.Headers;
                // Log response headers for debugging purposes
                Console.WriteLine("Response Headers: " + string.Join(", ", responseHeaders));
            });


            ConfigureAuth(app);
            //ConfigureStaticFiles(app);
        }
       
        //private void ConfigureStaticFiles(IAppBuilder app)
        //{
        //    // Middleware to handle serving static files
        //    app.Use(async (context, next) =>
        //    {
        //        var requestPath = context.Request.Path;

        //        // Check if the request path starts with "/UploadImages"
        //        if (requestPath.StartsWithSegments(new PathString("/UploadImages"), out var remainingPath))
        //        {
        //            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FoxDocumentDirectory/RequestForOrder/UploadImages", remainingPath.Value.TrimStart('/'));

        //            // Check if the file exists
        //            if (File.Exists(filePath))
        //            {
        //                context.Response.ContentType = GetContentType(filePath); // Set appropriate content type based on file type
        //                await context.Response.WriteAsync(File.ReadAllBytes(filePath));
        //                return;
        //            }
        //        }

        //        // Call the next middleware in the pipeline
        //        await next();
        //    });
        //}
        //private string GetContentType(string filePath)
        //{
        //    // Simple content type detection based on file extension
        //    var extension = Path.GetExtension(filePath).ToLowerInvariant();

        //    switch (extension)
        //    {
        //        case ".jpg":
        //            return "image/jpeg";
        //        case ".png":
        //            return "image/png";
        //        case ".gif":
        //            return "image/gif";
        //        default:
        //            return "application/octet-stream";
        //    }
        //}
    }
}
