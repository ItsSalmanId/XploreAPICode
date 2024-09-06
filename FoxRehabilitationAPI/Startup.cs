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



[assembly: OwinStartup(typeof(FoxRehabilitationAPI.Startup))]

namespace FoxRehabilitationAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
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
