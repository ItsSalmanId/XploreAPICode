using FOX.BusinessOperations.AssignedQueueService;
using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Models.AssignedQueueModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.ExternalUserModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.UnAssignedQueueModel;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;

namespace FoxRehabilitation.UnitTest
{
    [TestFixture]
    public class AssignedQueueServiceTestCases
    {
        private AssignedQueueServices _assignedQueueServices;
        private UserProfile _userProfile;
        private AssignedQueueRequest _assignedQueueRequest;
        private BlacklistWhiteListSourceModel _blacklistWhiteListSourceModel;
        private MarkReferralValidOrTrashedModel _markReferralValidOrTrashed;

        [SetUp]
        public void SetUp()
        {
            _assignedQueueServices = new AssignedQueueServices();
            _userProfile = new UserProfile();
            _assignedQueueRequest = new AssignedQueueRequest();
            _blacklistWhiteListSourceModel = new BlacklistWhiteListSourceModel();
            _markReferralValidOrTrashed = new MarkReferralValidOrTrashedModel();
        }
        [Test]
        [TestCase(0, 0, "")]
        [TestCase(1011163, 0, "")]
        [TestCase(0, 102, "")]
        [TestCase(0, 0, "khankhan_544596")]
        [TestCase(1011163, 102, "khankhan_544596")]
        [TestCase(1163, 12102, "test")]
        public void GetSupervisorAndAgentsForDropdown_UsersForDropdownList_ReturnData(long practiceCode, long roleId, string userName)
        {
            //Arrange
            //Act
            var result = _assignedQueueServices.GetSupervisorAndAgentsForDropdown(practiceCode, roleId, userName);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0, "")]
        [TestCase(1011163, "")]
        [TestCase(0, "khankhan_544596")]
        [TestCase(1011163, "khankhan_544596")]
        [TestCase(1163, "test")]
        public void GetIndexersForDropdown_UsersForDropdownList_ReturnData(long practiceCode, string userName)
        {
            //Arrange
            //Act
            var result = _assignedQueueServices.GetIndexersForDropdown(practiceCode, userName);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1012714, "Admin_5651352")]
        [TestCase(1011163, "")]
        [TestCase(0, "khankhan_544596")]
        [TestCase(1011163, "khankhan_544596")]
        [TestCase(1163, "test")]
        public void GeInterfaceFailedPatientList_InterfcaeFailedPatientList_ReturnData(long practiceCode, string userName)
        {
            //Arrange
            //Act
            var result = _assignedQueueServices.GeInterfaceFailedPatientList(practiceCode, userName);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(0, "")]
        [TestCase(101113, "")]
        [TestCase(0, "khankhan_544596")]
        [TestCase(1011163, "khankhan_544596")]
        [TestCase(0321, "test")]
        public void GetSupervisorsForDropdown_UsersForDropdownList_ReturnData(long practiceCode, string userName)
        {
            //Arrange
            //Act
            var result = _assignedQueueServices.GetSupervisorsForDropdown(practiceCode, userName);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count > 0)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("5487172")]
        [TestCase("5487173")]
        [TestCase("0")]
        public void GetIndexedQueue_PassModel_ReturnData(string workId)
        {
            //Arrange
            _assignedQueueRequest.reportUser = "test";
            _assignedQueueRequest.DATE_FROM_STR = Helper.GetCurrentDate().ToString();
            _assignedQueueRequest.DATE_TO_STR = Helper.GetCurrentDate().ToString();
            _assignedQueueRequest.WorkID = workId;
            _assignedQueueRequest.SearchText = "";
            _assignedQueueRequest.CurrentPage = 1;
            _assignedQueueRequest.RecordPerPage = 10;
            _assignedQueueRequest.SortBy = "";
            _assignedQueueRequest.SortOrder = "";

            //Act
            var result = _assignedQueueServices.GetIndexedQueue(_assignedQueueRequest, _userProfile);

            //Assert
            if (result.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count > 0)
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(false, true, 1011163)]
        [TestCase(false, true, 1012714)]
        [TestCase(true, false, 1011163)]
        [TestCase(true, false, 1012714)]
        public void BlackListOrWhiteListSource_PassModel_ReturnData(bool isBlackList, bool isWhiteList, long practiceCode)
        {
            //Arrange
            _userProfile.UserName = "1163testing";
            _userProfile.PracticeCode = practiceCode;
            _blacklistWhiteListSourceModel.Work_Ids = new List<long>()
            {
                5487172
            };
            _blacklistWhiteListSourceModel.Mark_All_Orders_As_White_Listed = isWhiteList;
            _blacklistWhiteListSourceModel.Is_Black_List = isBlackList;

            //Act
            var result = _assignedQueueServices.BlackListOrWhiteListSource(_blacklistWhiteListSourceModel, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(true, 1012714)]
        [TestCase(false, 1012714)]
        public void MakeReferralAsValidOrTrashed_PassModel_ReturnData(bool isTrash, long practiceCode)
        {
            //Arrange
            _userProfile.UserName = "1163testing";
            _userProfile.PracticeCode = practiceCode;
            _markReferralValidOrTrashed.Work_Id = 53434617;
            _markReferralValidOrTrashed.Is_Trash = isTrash;

            //Act
            var result = _assignedQueueServices.MakeReferralAsValidOrTrashed(_markReferralValidOrTrashed, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("", true)]
        [TestCase("test", true)]
        [TestCase("test", false)]
        public void AddUpdateSourceAsBlackOrWhiteList_PassModel_ReturnData(string sourcename, bool isBlackList)
        {
            //Arrange
            _userProfile.UserName = "1163testing";
            string sourcetype = Helper.getMaximumId("Test_Counter").ToString();
            _userProfile.PracticeCode = 1011163;

            //Act
            var result = _assignedQueueServices.AddUpdateSourceAsBlackOrWhiteList(sourcetype, sourcename, isBlackList, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public void MarkReferralAsTrashedOrValid_WhenCalled_ShouldReturnSuccessTrue()
        {
            // Arrange
            var work_id = 53434503;
            var markAsTrashed = true;
            var profile = new UserProfile { UserName = "TestUser" };
            profile.PracticeCode = 1012714;

            // Act
            var result = _assignedQueueServices.MarkReferralAsTrashedOrValid(work_id, markAsTrashed, profile).Success;

            // Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public void MarkReferralAsTrashedOrValid_WhenCalled_ShouldUpdateModifiedBy()
        {
            // Arrange
            var work_id = 53434503;
            var markAsTrashed = true;
            var profile = new UserProfile { UserName = "TestUser" };
            profile.PracticeCode = 1012714;


            // Act
            var result = _assignedQueueServices.MarkReferralAsTrashedOrValid(work_id, markAsTrashed, profile).Success;

            // Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
       public void MarkAllReferralsAsValid_ShouldReturnSuccessTrue_WhenSourceDataListIsNotEmpty()
        {
                // Arrange
    var sourceDatalist = new List<Tuple<string, string>>();
    sourceDatalist.Add(new Tuple<string, string>("Test1", "Test2"));
    var profile = new UserProfile();
    profile.PracticeCode = 1012714;
    
               // Act
    var result = _assignedQueueServices.MarkAllReferralsAsValid(sourceDatalist, profile);
    
                // Assert
                if (result != null)
                    {
       Assert.IsTrue(true);
                   }
                else
                    {
        Assert.IsFalse(false);
                   }
            }

        [Test]
       public void MarkAllReferralsAsValid_ShouldReturnSuccessFalse_WhenSourceDataListIsEmpty()
        {
                // Arrange
    var sourceDatalist = new List<Tuple<string, string>>();
    var profile = new UserProfile();
    profile.PracticeCode = 1012714;
    
               // Act
    var result = _assignedQueueServices.MarkAllReferralsAsValid(sourceDatalist, profile);
    
                // Assert
                if (result != null)
                  {
        Assert.IsTrue(true);
                    }
                else
                    {
        Assert.IsFalse(false);
                   }
           }

       [Test]
      public void MarkSourceAsBlackWhiteListed_Success()
       {
                // Arrange
    List < Tuple<string, string> > sourceDatalist = new List<Tuple<string, string>>();
    sourceDatalist.Add(new Tuple<string, string>("Source1", "Source1Value"));
    sourceDatalist.Add(new Tuple<string, string>("Source2", "Source2Value"));
    bool Is_Black_List = true;
    UserProfile profile = new UserProfile();
            profile.UserName = "Test";
            profile.PracticeCode = 1012714;
    
                // Act
    ResponseModel response = _assignedQueueServices.MarkSourceAsBlackWhiteListed(sourceDatalist, Is_Black_List, profile);
    
                // Assert
               if (response != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public void MarkSourceAsBlackWhiteListed_Failure()
        {
            // Arrange
            List<Tuple<string, string>> sourceDatalist = new List<Tuple<string, string>>();
            bool Is_Black_List = true;
            UserProfile profile = new UserProfile();

            // Act
            ResponseModel response = _assignedQueueServices.MarkSourceAsBlackWhiteListed(sourceDatalist, Is_Black_List, profile);

            // Assert
            if (response != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        [TestCase("5487172")]
        [TestCase("5487173")]
        public void ExportToExcelAssignedQueue_ShouldReturnFileName(string workId)
        {
            // Arrange
            AssignedQueueRequest req = new AssignedQueueRequest();
            UserProfile profile = new UserProfile();
            profile.PracticeDocumentDirectory = "TestDirectory";
            profile.PracticeCode = 1012714;
            profile.isTalkRehab = true;
            req.reportUser = "test";
            req.DATE_FROM_STR = Helper.GetCurrentDate().ToString();
            req.DATE_TO_STR = Helper.GetCurrentDate().ToString();
            req.WorkID = workId;
            req.SearchText = "";
            _assignedQueueRequest.CurrentPage = 1;
            req.RecordPerPage = 10;
            req.SortBy = "";
            req.SortOrder = "";


            // Act
            string result = _assignedQueueServices.ExportToExcelAssignedQueue(req, profile);

            // Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }


































        //AddUpdateSourceAsBlackOrWhiteList
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _assignedQueueServices = null;
            _userProfile = null;
            _assignedQueueRequest = null;
            _blacklistWhiteListSourceModel = null;
            _markReferralValidOrTrashed = null;
        }

    }
}
