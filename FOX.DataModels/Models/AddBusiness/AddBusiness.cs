using FOX.DataModels.Models.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FOX.DataModels.Models.AddBusiness.AddBusiness;

namespace FOX.DataModels.Models.AddBusiness
{
    public class AddBusiness
    {
        [Table("TBL_BUSINESS_DETAILS")]
        public class BusinessDetail
        {
            [Key]
            public long BUSINESS_DETAIL_ID { get; set; }
            public string PASSWORD { get; set; }
            public string EMAIL_ADDRESS { get; set; }
            public string BUSINESS_NAME { get; set; }
            public string BUSINESS_ADDRESS { get; set; }
            public string BUSINESS_ADDRESS_LINK { get; set; }
            public string BUSINESS_CITY { get; set; }
            public string BUSINESS_ZIP_CODE { get; set; }
            public string BUSINESS_HOURS { get; set; }
            public string BUSINESS_CATEGORY { get; set; }
            public string BUSINESS_CATEGORY_TYPE { get; set; }
            public string BUSINESS_PICTURE_ID { get; set; }
            public string BUSINESS_IMPORTANT_NOTES { get; set; }
            public string CREATED_BY { get; set; } = "TEAM XPLORE";
            public DateTime CREATED_DATE { get; set; }
            public string MODIFIED_BY { get; set; } = "TEAM XPLORE";
            public DateTime MODIFIED_DATE { get; set; }
            public bool DELETED { get; set; } = false;
            [NotMapped]
            public List<string> uploadedFilesName { get; set; } = new List<string>(); 
            [NotMapped]
            public List<BusinessFilesDetail> BusinessFilesDetail { get; set; } = new List<BusinessFilesDetail>(); // Initialize with an empty list

            public string MONDAY_OPENING_TIME { get; set; }
            public string MONDAY_CLOSING_TIME { get; set; }

            public string TUESDAY_OPENING_TIME { get; set; }
            public string TUESDAY_CLOSING_TIME { get; set; }

            public string WEDNESDAY_OPENING_TIME { get; set; }
            public string WEDNESDAY_CLOSING_TIME { get; set; }

            public string THURSDAY_OPENING_TIME { get; set; }
            public string THURSDAY_CLOSING_TIME { get; set; }

            public string FRIDAY_OPENING_TIME { get; set; }
            public string FRIDAY_CLOSING_TIME { get; set; }

            public string SATURDAY_OPENING_TIME { get; set; }
            public string SATURDAY_CLOSING_TIME { get; set; }

            public string SUNDAY_OPENING_TIME { get; set; }
            public string SUNDAY_CLOSING_TIME { get; set; }
            public string CONTACT_NO { get; set; }
            [NotMapped]
            public string OpenClose { get; set; }
            [NotMapped]
            public string CurrentDayOpeningTime { get; set; }
            [NotMapped]
            public string CurrentDayClosingTime { get; set; }


        }

        [Table("TBL_BUSINESS_BLOG_DETAILS")]
        public class BusinessBlogDetail
        {
            [Key]
            public long BUSINESS_BLOG_ID { get; set; }
            public string BLOG_TITLE { get; set; }
            public string EMAIL_ADDRESS { get; set; }
            public string AUTHOR_NAME { get; set; }
            public string BUSINESS_BLOG_ADDRESS { get; set; }
            public string BUSINESS_BLOG_CITY { get; set; }
            public string BUSINESS_BLOG_CATEGORY { get; set; }
            public string BUSINESS_IMPORTANT_NOTES { get; set; }
            public string CREATED_BY { get; set; } = "TEAM XPLORE";
            public DateTime CREATED_DATE { get; set; }
            public string MODIFIED_BY { get; set; } = "TEAM XPLORE";
            public DateTime MODIFIED_DATE { get; set; }
            public bool DELETED { get; set; } = false;
            [NotMapped]
            public List<string> uploadedFilesName { get; set; } = new List<string>();
            [NotMapped]
            public List<BusinessFilesDetail> BusinessFilesDetail { get; set; } = new List<BusinessFilesDetail>(); // Initialize with an empty list

        }

        [Table("TBL_BUSINESS_FILES_DTEAIL")]
        public class BusinessFilesDetail
        {
            [Key]
            public long BUSINESS_FILES_DTEAIL_ID { get; set; }
            public long? BUSINESS_DETAIL_ID { get; set; }
            public long? BUSINESS_BLOG_ID { get; set; }
            public string FILE_PATH { get; set; }
            public string FILE_PATH_1 { get; set; }
            public string CREATED_BY { get; set; } = "TEAM XPLORE";
            public DateTime CREATED_DATE { get; set; }
            public string MODIFIED_BY { get; set; } = "TEAM XPLORE";
            public DateTime MODIFIED_DATE { get; set; }
            public bool DELETED { get; set; } = false;
        }

        public class BusinessFilesDetailList
        {  
           public long BUSINESS_FILES_DTEAIL_ID { get; set; }
        }

        [Table("TBL_BUSINESS_RATING")]
        public class BusinessRating
        {
            [Key]
            public long TBL_BUSINESS_RATING_ID { get; set; }
            public string BUSINESS_ID { get; set; }
            public string CLEANLINESS_RATING { get; set; }
            public string ACCURACY_RATING { get; set; }
            public string LOCATION_RATING { get; set; }
            public string CHECKIN_RATING { get; set; }
            public string COMMUNICATION_RATING { get; set; }
            public string VALUE_RATING { get; set; }
            public string EMAIL_ADDRESS { get; set; }
            public string NAME { get; set; }
            public string FEEDBACK { get; set; }
            public string CREATED_BY { get; set; } = "TEAM XPLORE";
            public DateTime? CREATED_DATE { get; set; }
            public string MODIFIED_BY { get; set; } = "TEAM XPLORE";
            public DateTime? MODIFIED_DATE { get; set; }
            public bool DELETED { get; set; } = false;
            public bool SAVE_INFORMATION_OR_NOT { get; set; } = false;
            [NotMapped]
            public string AVG_RATING { get; set; }
        }
        [Table("FOX_TBL_SURVEY_QUESTION")]
        public class SurveyQuestions
        {
            [Key]
            public long SURVEY_QUESTIONS_ID { get; set; }
            public string SURVEY_QUESTIONS { get; set; }
        }
        [Table("TBL_PROFILE_TOKENS")]
        public class UserProfileToken //: Alachisoft.NCache.Runtime.Serialization.ICompactSerializable
        {
            [Key]
            public long TOKEN_ID { get; set; }
            public long USER_ID { get; set; }
            public string AUTH_TOKEN { get; set; }
            public System.DateTime ISSUED_ON { get; set; }
            public System.DateTime EXPIRES_ON { get; set; }
            public string PROFILE { get; set; }
            [NotMapped]
            public long? isMFAVerified { get; set; }
            [NotMapped]
            public long? isValidate { get; set; }
            public bool? IS_LOG_OUT { get; set; }
            //[NotMapped]
            //public UserProfile userProfile
            //{
            //    get
            //    {
            //        UserProfile UF = new UserProfile();
            //        UF = JsonConvert.DeserializeObject<UserProfile>(this.PROFILE);
            //        return UF;
            //    }
            //    set { this.PROFILE = JsonConvert.SerializeObject(value); }
            //}
            [NotMapped]
            public int? AUTO_LOCK_TIMESPAN { get; set; }

            [Table("FOX_TBL_PROFILE_TOKENS_SECURITY")]
            public class ProfileTokensSecurity //: Alachisoft.NCache.Runtime.Serialization.ICompactSerializable
            {
                [Key]
                public long TokenSecurityID { get; set; }
                public bool isLogOut { get; set; }
                public string AuthToken { get; set; }
                public System.DateTime IssuedOn { get; set; }
                public System.DateTime ExpiresOn { get; set; }
                public string CREATED_BY { get; set; }
                public DateTime CREATED_DATE { get; set; }
                public string MODIFIED_BY { get; set; }
                public DateTime MODIFIED_DATE { get; set; }
                public bool DELETED { get; set; }
            }
        }
        [Table("TBL_REELS_DETAILS")]
        public class ReelsDetails
        {
            [Key]
            public long REELS_DETAILS_ID { get; set; }
            public long? USER_ID { get; set; }
            public string EMAIL_ADDRESS { get; set; }
            public string USER_TYPE { get; set; }
            public string COMMENT_ID { get; set; }
            public string STATUS_SEEN_DETAIL { get; set; }
            public string CREATED_BY { get; set; } = "TEAM XPLORE";
            public DateTime CREATED_DATE { get; set; }
            public string MODIFIED_BY { get; set; } = "TEAM XPLORE";
            public DateTime MODIFIED_DATE { get; set; }
            public bool DELETED { get; set; } = false;
            [NotMapped]
            public List<string> uploadedFilesName { get; set; } = new List<string>();
            [NotMapped]
            public List<ReelsFilesDetails> BusinessFilesDetail { get; set; } = new List<ReelsFilesDetails>();
            [NotMapped]
            public string authorName { get; set; }
            [NotMapped]
            public string authorImg { get; set; }
            [NotMapped]
            public bool? isMuted { get; set; } = false;
            [NotMapped]
            public string description { get; set; }
            [NotMapped]
            public string musicName { get; set; }
            [NotMapped]
            public bool? liked { get; set; } = false;
            [NotMapped]
            public bool? isBookmarked { get; set; } = false;
            [NotMapped]
            public int? likesCount { get; set; }
            [NotMapped]
            public List<string> reelsCommentsDetailsList { get; set; } = new List<string>();
            [NotMapped]
            public List<ReelsCommentsDetails> reelsCommentsModelList { get; set; } = new List<ReelsCommentsDetails>();
            [NotMapped]
            public bool isClickOnReelLike { get; set; } = false;
            [NotMapped]
            public bool is_REEL_Liked { get; set; } = false;
            [NotMapped]
            public bool IS_REEL_LIKED_OR_NOT { get; set; } = false;
            [NotMapped]
            public bool isLikeUnlikeReel { get; set; } = false;
            [NotMapped]
            public bool IS_REEL_SAVED_OR_NOT { get; set; } = false;
            public int? REEL_LIKES_COUNT { get; set; }
            public bool? REEL_STATUS { get; set; } = false;
        }
        [Table("TBL_REELS_FILES_DETAILS")]
        public class ReelsFilesDetails
        {
            [Key]
            public long REELS_FILES_DETAILS_ID { get; set; }

            public long REELS_DETAILS_ID { get; set; }

            public long? USER_ID { get; set; }

            public string FILE_PATH { get; set; }

            public string FILE_PATH_1 { get; set; }

            public string CREATED_BY { get; set; } = "TEAM XPLORE";

            public DateTime CREATED_DATE { get; set; }

            public string MODIFIED_BY { get; set; } = "TEAM XPLORE";

            public DateTime MODIFIED_DATE { get; set; }

            public bool DELETED { get; set; } = false;
        }
        [Table("TBL_REELS_COMMENTS_DETAILS")]
        public class ReelsCommentsDetails
        {
            [Key]
            public long REELS_COMMENTS_DETAILS_ID { get; set; }
            public long REELS_DETAILS_ID { get; set; }
            public long? USER_ID { get; set; }
            public string COMMENT { get; set; }
            public int COMMENT_LIKE { get; set; }
            public string COMMENT_REPLAY { get; set; }
            public string CREATED_BY { get; set; } = "TEAM XPLORE";
            public DateTime CREATED_DATE { get; set; }
            public string MODIFIED_BY { get; set; } = "TEAM XPLORE";
            public DateTime MODIFIED_DATE { get; set; }
            public bool DELETED { get; set; } = false;
            public bool IS_REPLAY_COMMENT { get; set; } = false;
            [NotMapped]
            public bool IS_LIKED { get; set; } = false;
            [NotMapped]
            public bool IS_REPLAY_LIKED { get; set; } = false;
            [NotMapped]
            public bool isClickOnLike { get; set; } = false;
            [NotMapped]
            public bool LIKE_OR_DISLIKE { get; set; } = false;
            public int IS_REPLAY_LIKES { get; set; }
            public string REPLAY_COMMENT { get; set; }
            public long REPLAY_COMMENT_HEADER_ID { get; set; }

            public long REPLAY_COMMENT_ID { get; set; }
        }
        [Table("TBL_USER_FOLLOW_DETAILS")]
        public class UserFollowDetails
        {
            [Key]
            public long USER_FOLLOW_ID { get; set; }
            public long? USER_FOLLOWERS_ID { get; set; }
            public long? USER_ID { get; set; }
            public string CREATED_BY { get; set; } = "TEAM XPLORE";
            public DateTime CREATED_DATE { get; set; }
            public string MODIFIED_BY { get; set; } = "TEAM XPLORE";
            public DateTime MODIFIED_DATE { get; set; }
            public bool DELETED { get; set; } = false;
        }

        [Table("TBL_REELS_COMMENTS_LIKES")]

        public class ReelsCommentsLikes
        {
            [Key]
            public long COMMENT_LIKE_ID { get; set; }

            public long REELS_COMMENTS_DETAILS_ID { get; set; }

            public long USER_ID { get; set; }

            public string CREATED_BY { get; set; } = "TEAM XPLORE";

            public DateTime CREATED_DATE { get; set; }

            public string MODIFIED_BY { get; set; } = "TEAM XPLORE";

            public DateTime MODIFIED_DATE { get; set; }

            public bool DELETED { get; set; } = false;
        }
        [Table("TBL_REELS_LIKES_DETAILS")]
        public class ReelsLikes
        {
            [Key]
            public long REELS_LIKE_ID { get; set; }

            public long REELS_DETAILS_ID { get; set; }

            public long USER_ID { get; set; }

            public string CREATED_BY { get; set; } = "TEAM XPLORE";

            public DateTime CREATED_DATE { get; set; }

            public string MODIFIED_BY { get; set; } = "TEAM XPLORE";

            public DateTime MODIFIED_DATE { get; set; }

            public bool DELETED { get; set; } = false;
        }

        public class BusinessHoursStatus
        {
            public string OpenClose { get; set; }
            public string CurrentDayOpeningTime { get; set; }
            public string CurrentDayClosingTime { get; set; }
        }
        [Table("TBL_REELS_SAVED_DETAILS")]
        public class ReelSaved
        {
            [Key]
            public long REELS_SAVED_ID { get; set; }
            public long REELS_DETAILS_ID { get; set; }
            public long USER_ID { get; set; }
            public string CREATED_BY { get; set; } = "TEAM XPLORE";
            public DateTime CREATED_DATE { get; set; }
            public string MODIFIED_BY { get; set; } = "TEAM XPLORE";
            public DateTime MODIFIED_DATE { get; set; }
            public bool DELETED { get; set; } = false;
            [NotMapped]
            public bool isClickOnSave { get; set; }
        }
    }
}
