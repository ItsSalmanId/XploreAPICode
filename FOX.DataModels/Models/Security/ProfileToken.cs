﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.Security
{
    [Serializable]
    [Table("FOX_TBL_PROFILE_TOKENS")]
    public class ProfileToken //: Alachisoft.NCache.Runtime.Serialization.ICompactSerializable
    {
        [Key]
        public long TokenId { get; set; }
        public long UserId { get; set; }
        public string AuthToken { get; set; }
        public System.DateTime IssuedOn { get; set; }
        public System.DateTime ExpiresOn { get; set; }
        public string Profile { get; set; }
        [NotMapped]
        public long? isMFAVerified { get; set; }
        [NotMapped]
        public long? isValidate { get; set; }
        public bool? isLogOut { get; set; }
        [NotMapped]
        public UserProfile userProfile
        {
            get
            {
                UserProfile UF = new UserProfile();
                UF = JsonConvert.DeserializeObject<UserProfile>(this.Profile);
                return UF;
            }
            set { this.Profile = JsonConvert.SerializeObject(value); }
        }
        [NotMapped]
        public int AUTO_LOCK_TIMESPAN { get; set; }

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

    public class TokensUserInfo //: Alachisoft.NCache.Runtime.Serialization.ICompactSerializable
    {
        public string TokenSecurityID { get; set; }
        public string AuthToken { get; set; }
        public long UserId { get; set; }
        public Boolean IS_ADMIN { get; set; }
    }


}