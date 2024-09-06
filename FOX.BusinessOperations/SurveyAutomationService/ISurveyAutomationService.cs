using System.Collections.Generic;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using static FOX.DataModels.Models.AddBusiness.AddBusiness;
using static FOX.DataModels.Models.SurveyAutomations.SurveyAutomations;

namespace BusinessOperations.SurveyAutomationService
{
    public interface ISurveyAutomationService
    {
        #region FUNCTIONS
        //SurveyAutomation GetPatientDetails(SurveyAutomation objSurveyAutomation);
       // SurveyLink DecryptionUrl(SurveyLink objSurveyLink);
        ResponseModel Register(UserAccount objUserAccount);
        UserAccount Login(UserAccount objUserAccount);
        ResponseModel AddUpdateBusiness(BusinessDetail objBusinessDetail);
        ResponseModel AddUpdateBlogBusiness(BusinessBlogDetail objBusinessBlogDetail);
        List<BusinessDetail> GetBusinessDetails(BusinessDetail objBusinessDetail);
        BusinessDetail GetSelectedBusiness(BusinessDetail objBusinessDetail);
        ResponseModel DeleteBusinessDetails(long businessId);
        ResponseModel DeleteSelectedImage(BusinessFilesDetailList[] objbusinessFilesList);
        List<UserAccount> GetUserDetails(UserAccount[] objUserAccountList);
        ResponseModel DeleteUser(UserAccount objUserAccountList);
        ResponseModel EditUser(UserAccount objUserAccountList);
        List<BusinessBlogDetail> GetBlogsDetails(BusinessBlogDetail objBusinessBlogDetail);
        ResponseModel SubmitRating(BusinessRating objBusinessRating);
        ResponseModel GenerateToken(UserProfileToken objProfileToken);
        List<BusinessRating> GetBusinessRating(BusinessRating objBusinessRating);
        UserAccount ValidateUser(UserAccount objUserAccount);
        UserProfileToken UserToken(UserProfileToken objUserProfileToken);
        UserAccount LogoutUser(UserAccount objUserAccount);
        ResponseModel AddUpdateReels(ReelsDetails objReelsDetails);
        List<ReelsDetails> GetReelsDetails(ReelsDetails objReelsDetails);
        ResponseModel PostComment(ReelsCommentsDetails objReelsComments);

        List<ReelsCommentsDetails> GetCommentsByReel(ReelsCommentsDetails objReelsDetails);
        ReelsDetails GetLikeByReel(ReelsDetails objReelsDetails);
        ResponseModel AddUpdateFollower(UserFollowDetails objUserFollowDetails);
        List<UserAccount> GetUserReels(UserAccount objUserAccountList);
        List<UserAccount> GetReelsUserProfile(UserAccount objUserAccountList);
        ReelSaved SavedPost(ReelSaved objReelSaved);
        ResponseModel AddUpdateReelsStatus(ReelsDetails objReelsDetails);
        List<ReelsDetails> GetReelsStatus(ReelsDetails objReelsDetails);

        //ResponseModel Login(SurveyLink objSurveyLink);

        #endregion
    }
}
