using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using FoxRehabilitationAPI.Models;
using System.Linq;
using System.Data.Entity;
using FOX.DataModels.Models.Security;
using System;
using BusinessOperations.Security;
using FoxRehabilitationAPI.App_Start;
using static BusinessOperations.CommonServices.AppConfiguration.ActiveDirectoryViewModel;
using BusinessOperations.CommonService;
using BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using System.IO;
using System.Web;
using FoxRehabilitationAPI.Controllers;
using System.Collections;
using System.Web.Configuration;
using System.Collections.Specialized;
using FOX.DataModels;
using FOX.DataModels.Models.SenderName;
using System.Diagnostics.CodeAnalysis;

namespace FoxRehabilitationAPI
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    [ExcludeFromCodeCoverage]
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
        }

        //LDAPUser GetUserFromAD(string userName, string password)
        //{
        //    LDAPClient _LDAPClient = new LDAPClient();
        //    LDAPUser aduser = _LDAPClient.GetADUser(DomainURL, userName.Split('@')[0], password);
        //    return aduser;
        //}


        public async Task<Tuple<ApplicationUser, UserProfile>> FindProfileAsync(string userName, string password)
        {
            ApplicationUser user = new ApplicationUser();
            UserProfile userProfile = new UserProfile();
            //UserManagementService userService = new UserManagementService();
            //AccountController accountController = new AccountController();
            //AccountServices accountServices = new AccountServices();
            ApplicationUserManager applicationUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUserManager applicationUserManagerTalkRehab = new ApplicationUserManager(new UserStore<ApplicationUser>(new TalkRehabDBContext()));
            ADDetail _ADDetail = null;

            userName = userName.Trim();
            if (userName.Contains("@"))
            {
                _ADDetail = ADDetailList.FirstOrDefault(t => t.DomainForSearch.Equals(userName.Split('@')[1].ToLower()));
            }

            #region New Logic

            //using (ApplicationDbContext dbContext = new ApplicationDbContext())
            //{
            //    //Get User from Database

            //    //Validate User from Active Directory
            //    LDAPUser adUser = GetUserFromAD(userName, password);
            //    //User Doesnot exists in Active Directory
            //    if (adUser == null)
            //    {
            //        //check user from database 
            //        //user = await dbContext.Users.Where(u => (u.Email.ToLower() == userName.ToLower() || u.UserName.ToLower() == userName.ToLower())).FirstOrDefaultAsync();
            //        userProfile = userService.GetUserProfileByName(userName);
            //        user = await base.FindAsync(userProfile.UserName, password);
            //        if (user != null)
            //        {
            //            //userProfile = userService.GetUserProfileByName(userName);
            //            user.ApplicationUserRoles = userProfile.ApplicationUserRoles = userService.GetCurrentUserRights(user.ROLE_ID ?? 0, user.PRACTICE_CODE);
            //            return new Tuple<ApplicationUser, UserProfile>(user, userProfile);
            //        }
            //    }
            //    else
            //    {
            //        //Check from database that user exists 
            //        user = await dbContext.Users.Where(u => (u.Email.ToLower() == userName.ToLower() || u.UserName.ToLower() == userName.ToLower())).FirstOrDefaultAsync();
            //        //user doesnot exists in database
            //        if (user == null)
            //        {
            //            //Add User To database
            //            user = new ApplicationUser();
            //            user.FIRST_NAME = adUser.FirstName;
            //            user.LAST_NAME = adUser.LastName;
            //            user.USER_NAME = adUser.FullName;
            //            user.EMAIL = adUser.Email;
            //            user.USER_ID = Helper.getMaximumId("USER_ID");
            //            user.CREATED_BY = user.MODIFIED_BY = "FOX Team";
            //            user.CREATED_DATE = user.MODIFIED_DATE = Helper.GetCurrentDate();
            //            user.PRACTICE_CODE = AppConfiguration.GetPracticeCode;
            //            user.ROLE_ID = RoleId;
            //            user.IS_ACTIVE = true;
            //            user.IS_APPROVED = true;
            //            user.USER_TYPE = "Internal";
            //            FOX.DataModels.Context.DbContextCommon _DbContextCommon = new FOX.DataModels.Context.DbContextCommon();
            //            var _FOX_TBL_SENDER_TYPE = _DbContextCommon.insertUpdateSenderType.FirstOrDefault(t => t.SENDER_TYPE_NAME.ToLower().Contains("Fox Account Manager".ToLower()));
            //            if (_FOX_TBL_SENDER_TYPE != null)
            //            {
            //                user.FOX_TBL_SENDER_TYPE_ID = _FOX_TBL_SENDER_TYPE?.FOX_TBL_SENDER_TYPE_ID;
            //            }
            //            user.PASSWORD = Encrypt.getEncryptedCode(password);
            //            IdentityResult result = await applicationUserManager.CreateAsync(user, password);
            //            if (result.Succeeded)
            //            {
            //                userService.SavePasswordHisotry(user, user.PRACTICE_CODE);
            //                userService.SaveSenderName(user, user.PRACTICE_CODE);
            //            }
            //            userProfile = userService.GetUserProfileByName(userName);
            //            user.ApplicationUserRoles = userProfile.ApplicationUserRoles = userService.GetCurrentUserRights(user.ROLE_ID ?? 0, user.PRACTICE_CODE);
            //            return new Tuple<ApplicationUser, UserProfile>(user, userProfile);
            //        }
            //        else //user exists in database than return that user and if required update it's password in database
            //        {
            //            userProfile = userService.GetUserProfileByName(userName);
            //            user.USER_NAME = userProfile.UserName;
            //            user.PasswordHash = base.PasswordHasher.HashPassword(password);
            //            userService.UpdateADUserPassword(Encrypt.getEncryptedCode(password), user.PasswordHash, userProfile);
            //            //await dbContext.SaveChangesAsync();
            //            user.ApplicationUserRoles = userProfile.ApplicationUserRoles = userService.GetCurrentUserRights(user.ROLE_ID ?? 0, user.PRACTICE_CODE);
            //            return new Tuple<ApplicationUser, UserProfile>(user, userProfile);

            //        }

            //    }

            //}

            #endregion New Logic
            return new Tuple<ApplicationUser, UserProfile>(null, null);
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
            // Configure validation logic for passwords
            //manager.PasswordValidator = new PasswordValidator
            //{
            //    RequiredLength = 8,
            //    RequireNonLetterOrDigit = true,
            //    RequireDigit = true,
            //    RequireLowercase = true,
            //    RequireUppercase = true,
            //};
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public bool CheckUserExistingLoginAttempts(string userName)
        {
            //UserManagementService user = new UserManagementService();
            //try
            //{
            //    return user.CheckValidUserLoginAttempt(userName: userName);
            //}
            //catch (Exception)
            //{
            //    return user.CheckValidUserLoginAttempt(userName: userName);
            //}
            return false;
        }

        public bool IsCheckedUserBlocked(string userName)
        {
            return false;
        }

        public int GetInvalidAttempts(string userName)
        {
            return 0;
        }
        public Mfa_Login_Attempts GetInvalidMFAAttempts(string userName)
        {
            
            return null;
        }
        public bool AddInvalidLoginAttempt(string userName)
        {
            return false;
        }

        public bool AddValidLoginAttempt(string userName)
        {
            return false;
        }
        public bool AddMFAValidLoginAttempt(string userName)
        {
            return false;
        }
    }


    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> store) : base(store)
        {

        }
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
        }
    }
}
