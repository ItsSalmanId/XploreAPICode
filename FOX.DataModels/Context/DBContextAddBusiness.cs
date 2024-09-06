//using FOX.DataModels.Models.Patient;
//using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.AddBusiness;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FOX.DataModels.Models.AddBusiness.AddBusiness;

namespace FOX.DataModels.Context
{
    public class DBContextAddBusiness : DbContext
    {
        public DBContextAddBusiness() : base(EntityHelper.getConnectionStringName())
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<BusinessDetail>().Property(t => t.BUSINESS_DETAIL_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<BusinessFilesDetail>().Property(t => t.BUSINESS_FILES_DTEAIL_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<BusinessBlogDetail>().Property(t => t.BUSINESS_BLOG_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<BusinessRating>().Property(t => t.TBL_BUSINESS_RATING_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<UserProfileToken>().Property(t => t.TOKEN_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReelsFilesDetails>().Property(t => t.REELS_FILES_DETAILS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReelsDetails>().Property(t => t.REELS_DETAILS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<ReelsCommentsDetails>().Property(t => t.REELS_COMMENTS_DETAILS_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<UserFollowDetails>().Property(t => t.USER_FOLLOW_ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }

       public virtual DbSet<BusinessDetail> BusinessDetail { get; set; }
       public virtual DbSet<BusinessFilesDetail> BusinessFilesDetail { get; set; }
       public virtual DbSet<BusinessBlogDetail> BusinessBlogDetail { get; set; }
       public virtual DbSet<BusinessRating> BusinessRating { get; set; }
       public virtual DbSet<UserProfileToken> UserProfileToken { get; set; }
       public virtual DbSet<ReelsFilesDetails> ReelsFilesDetails { get; set; }
       public virtual DbSet<ReelsDetails> ReelsDetails { get; set; }
       public virtual DbSet<ReelsCommentsDetails> ReelsCommentsDetails { get; set; }
       public virtual DbSet<UserFollowDetails> UserFollowDetails { get; set; }
    }
}
