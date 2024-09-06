using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.IO;

namespace FoxRehabilitationAPI.Controllers
{
    [RoutePrefix("api/files")]
    public class FilesController : ApiController
    {
        private readonly string _basePath = @"C:\Users\Salman jani\source\repos\FoxRehabilitationAPI-1\FoxRehabilitationAPI\FoxDocumentDirectory\RequestForOrder\UploadImages";

        [HttpGet]
        [Route("images/{filename}")]
        public HttpResponseMessage GetImage(string filename)
        {
            var filePath = Path.Combine(_basePath, filename);

            if (!File.Exists(filePath))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(GetContentType(filePath));

            return result;
        }

        private string GetContentType(string filePath)
        {
            // Simple content type detection based on file extension
            var extension = Path.GetExtension(filePath).ToLowerInvariant();

            switch (extension)
            {
                case ".jpg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
