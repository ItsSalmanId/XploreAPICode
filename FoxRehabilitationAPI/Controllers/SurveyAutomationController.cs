using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using BusinessOperations.SurveyAutomationService;
using FoxRehabilitationAPI.Filters;
using static FOX.DataModels.Models.AddBusiness.AddBusiness;
using static FOX.DataModels.Models.SurveyAutomations.SurveyAutomations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace FoxRehabilitationAPI.Controllers
{
    [ExceptionHandlingFilter]
    [AllowAnonymous]
    [ExcludeFromCodeCoverage]
    public class SurveyAutomationController : BaseApiController
    {
        private readonly ISurveyAutomationService _surveyAutomationService;
        public SurveyAutomationController(ISurveyAutomationService surveyAutomationService)
        {
            _surveyAutomationService = surveyAutomationService;
        }
      
     

        [HttpPost]
        public HttpResponseMessage RegisterUser(UserAccount objUserAccount)
        {
            if (objUserAccount != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.Register(objUserAccount));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "SurveyLink model is empty");
            }
        }

        [HttpPost]
        public HttpResponseMessage LoginUser(UserAccount objUserAccount)
        {
            if (objUserAccount != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.Login(objUserAccount));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "SurveyLink model is empty");
            }
        }

        

        [HttpPost]
        public IHttpActionResult Login(UserAccount objUserAccount)
        {
            if (objUserAccount.EMAIL_ADDRESS != null) // Replace with actual logic
            {
                var token = GenerateJwtToken(objUserAccount.EMAIL_ADDRESS);
                UserProfileToken userProfileToken = new UserProfileToken();
                userProfileToken.USER_ID = objUserAccount.APPLICATION_USER_ACCOUNTS_ID;
                userProfileToken.AUTH_TOKEN = token;
                Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GenerateToken(userProfileToken));
                return Ok(new { token });
            }
            return Unauthorized();
        }

        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("YourSuperStrongSecretKey123456789@!"); // Use a strong secret key
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        [HttpPost]
        public HttpResponseMessage AddUpdateBusinessDetails(BusinessDetail objBusinessDetail)
        {
            if (objBusinessDetail != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.AddUpdateBusiness(objBusinessDetail));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }

        [HttpPost]
        public HttpResponseMessage AddUpdateBlogBusiness(BusinessBlogDetail objBusinessBlogDetail)
        {
            if (objBusinessBlogDetail != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.AddUpdateBlogBusiness(objBusinessBlogDetail));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }

        [HttpPost]
        public HttpResponseMessage GetBusiness(BusinessDetail objBusinessDetail)
        {
            if (objBusinessDetail != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetBusinessDetails(objBusinessDetail));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetSelectedBusiness(BusinessDetail objBusinessDetail)
        {
            if (objBusinessDetail != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetSelectedBusiness(objBusinessDetail));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }

        [HttpGet]
        public HttpResponseMessage DeleteBusinessDetails(long businessId)
        {
            if (businessId != 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.DeleteBusinessDetails(businessId));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Frictionless Referral ID is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage DeleteSelectedImage(BusinessFilesDetailList[] objbusinessFilesList)
        {
            if (objbusinessFilesList != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.DeleteSelectedImage(objbusinessFilesList));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Frictionless Referral ID is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetUserDetails(UserAccount[] objUserAccountList)
        {
            if (objUserAccountList != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetUserDetails(objUserAccountList));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Frictionless Referral ID is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage DeleteUser(UserAccount objUserAccount)
        {
            if (objUserAccount != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.DeleteUser(objUserAccount));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Frictionless Referral ID is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage EditUser(UserAccount objUserAccount)
        {
            if (objUserAccount != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.EditUser(objUserAccount));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Frictionless Referral ID is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetBlogsDetails(BusinessBlogDetail objBusinessBlogDetail)
        {
            if (objBusinessBlogDetail != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetBlogsDetails(objBusinessBlogDetail));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage SubmitRating(BusinessRating objBusinessRating)
        {
            if (objBusinessRating != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.SubmitRating(objBusinessRating));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetBusinessRating(BusinessRating objBusinessRating)
        {
            if (objBusinessRating != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetBusinessRating(objBusinessRating));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage validateUser(UserAccount objUserAccount)
        {
            if (objUserAccount != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.ValidateUser(objUserAccount));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "SurveyLink model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage UserToken(UserProfileToken objUserProfileToken)
        {
            if (objUserProfileToken != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.UserToken(objUserProfileToken));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "SurveyLink model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage LogoutUser(UserAccount objUserAccount)
        {
            if (objUserAccount != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.LogoutUser(objUserAccount));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "SurveyLink model is empty");
            }
        }

        //reels
        [HttpPost]
        public HttpResponseMessage AddUpdateReels(ReelsDetails objReelsDetails)
        {
            if (objReelsDetails != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.AddUpdateReels(objReelsDetails));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }

        [HttpPost]
        public HttpResponseMessage GetReelsDetails(ReelsDetails objReelsDetails)
        {
            if (objReelsDetails != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetReelsDetails(objReelsDetails));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage PostComment(ReelsCommentsDetails objReelsComments)
        {
            if (objReelsComments != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.PostComment(objReelsComments));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetCommentsByReel(ReelsCommentsDetails objReelsComments)
        {
            if (objReelsComments != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetCommentsByReel(objReelsComments));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }

        [HttpPost]
        public HttpResponseMessage AddUpdateFollower(UserFollowDetails objUserFollowDetails)
        {
            if (objUserFollowDetails != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.AddUpdateFollower(objUserFollowDetails));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetUserReels(UserAccount objUserAccountList)
        {
            if (objUserAccountList != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetUserReels(objUserAccountList));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Frictionless Referral ID is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetReelsUserProfile(UserAccount objUserAccountList)
        {
            if (objUserAccountList != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetReelsUserProfile(objUserAccountList));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Frictionless Referral ID is Empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetLikeByReel(ReelsDetails objReelsDetails)
        {
            if (objReelsDetails != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetLikeByReel(objReelsDetails));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage SavedPost(ReelSaved objReelSaved)
        {
            if (objReelSaved != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.SavedPost(objReelSaved));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage AddUpdateReelsStatus(ReelsDetails objReelsDetails)
        {
            if (objReelsDetails != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.AddUpdateReelsStatus(objReelsDetails));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }
        [HttpPost]
        public HttpResponseMessage GetReelsStatus(ReelsDetails objReelsDetails)
        {
            if (objReelsDetails != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _surveyAutomationService.GetReelsStatus(objReelsDetails));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "BusinessDetail model is empty");
            }
        }
    }
}
