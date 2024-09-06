using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessOperations.AddBusinessService;
using FoxRehabilitationAPI.Filters;
using static FOX.DataModels.Models.AddBusiness.AddBusiness;

namespace FoxRehabilitationAPI.Controllers
{
    [ExceptionHandlingFilter]
    [AllowAnonymous]
    [ExcludeFromCodeCoverage]
    public class AddBusinessController : BaseApiController
    {
        private readonly IAddBusinessService _addBusinessService;
        public AddBusinessController(IAddBusinessService addBusinessService)
        {
            _addBusinessService = addBusinessService;
        }

        [HttpPost]
        public HttpResponseMessage AddUpdateBusinessDetails(BusinessDetail objBusinessDetail)
        {
            if (objBusinessDetail != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _addBusinessService.AddUpdateBusiness(objBusinessDetail));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }
    }
}
