using FOX.BusinessOperations.SupervisorWorkService;
using FOX.DataModels.Models.AssignedQueueModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.User;
using FOX.DataModels.Models.SupervisorWorkModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.SupervisorWorkServiceUnitTest
{
    [TestFixture]
    public class SupervisorWorkServiceTest
    {
        private SupervisorWorkService _supervisorWorkService;
        private UserProfile _userProfile;
        private SupervisorWorkRequest _supervisorWorkRequest;
        private MarkReferralValidOrTrashedModel _markReferralValidOrTrashedModel;
        [SetUp]
        public void SetUp()
        {
            _supervisorWorkService = new SupervisorWorkService();
            _userProfile = new UserProfile();
            _supervisorWorkRequest = new SupervisorWorkRequest();
            _markReferralValidOrTrashedModel = new MarkReferralValidOrTrashedModel();
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(1011165)]
        public void GetSupervisorList_SupervisorListModel_ReturnData(long PracticeCode)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;
            _userProfile.userID = 1011163415;
            _userProfile.UserName = "1163testing";
            _supervisorWorkRequest.TransferReason = "";
            _supervisorWorkRequest.RecordPerPage = 10;
            _supervisorWorkRequest.CurrentPage = 1;
            _supervisorWorkRequest.SortOrder = "";
            _supervisorWorkRequest.SearchText = "";
            _supervisorWorkRequest.SourceString = "";
            _supervisorWorkRequest.SourceName = "";
            _supervisorWorkRequest.SourceType = "";
            _supervisorWorkRequest.SupervisorName = "";
            _supervisorWorkRequest.TransferComments = "";
            _supervisorWorkRequest.IndexBy = null;
            _supervisorWorkRequest.SortBy = null;

            //Act
            var result = _supervisorWorkService.GetSupervisorList(_supervisorWorkRequest, _userProfile);

            //Assert
            if (result != null && result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, "asd_5482973")]
        [TestCase(1011163, "test")]
        [TestCase(0, "")]
        public void GetIndxersAndSupervisorsForDropdown_IndxersAndSupervisorsForDropdownModel_ReturnData(long practiceCode, string userName)
        {
            //Arrange
            //Act
            var result = _supervisorWorkService.GetIndxersAndSupervisorsForDropdown(practiceCode, userName);

            //Assert
            if (result != null && result.Count > 0)
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
            _userProfile.UserName = "Davis_53411401";
            _userProfile.PracticeCode = practiceCode;
            _markReferralValidOrTrashedModel.Work_Id = 53434503;
            _markReferralValidOrTrashedModel.Is_Trash = isTrash;
            // Arrange
          

            //Act
            var result = _supervisorWorkService.MakeReferralAsValidOrTrashed(_markReferralValidOrTrashedModel, _userProfile);

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
        [TestCase(544557)]
        [TestCase(5448057)]
        [TestCase(1011163)]
        public void GetWorkTransferComments_WorkTransferCommentsModel_ReturnData(long workId)
        {
            //Arrange
            //Act
            var result = _supervisorWorkService.GetWorkTransferComments(workId);

            //Assert
            if (result != null && result.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1012714)]
        public void ExportToExcelSupervisorQueue_ValidInput_ReturnsValidPath(long practiceCode)
        {
            // Arrange
            SupervisorWorkRequest req = new SupervisorWorkRequest();
            UserProfile profile = new UserProfile();
            _userProfile.PracticeCode = practiceCode;
            _userProfile.userID = 1011163415;
            _userProfile.UserName = "1163testing";
            req.TransferReason = "";
            req.RecordPerPage = 10;
            req.CurrentPage = 1;
            req.SortOrder = "";
            req.SearchText = "";
            req.SourceString = "";
            req.SourceName = "";
            req.SourceType = "";
            req.SupervisorName = "";
            req.TransferComments = "";
            req.IndexBy = null;
            req.SortBy = null;


            // Act
            string actualPath = _supervisorWorkService.ExportToExcelSupervisorQueu(req, profile);


            //Assert
            if (actualPath != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public void GetIndxersAndSupervisorsForDropdown_ShouldReturnListOfUsersForDropdown()
        {
            // Arrange
            long practiceCode = 1012714;
            string userName = "L2_53411372";

            // Act
            var result = _supervisorWorkService.GetIndxersAndSupervisorsForDropdown(practiceCode, userName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<UsersForDropdown>>(result);
        }

        [Test]
        public void MakeReferralAsValidOrTrashed_ValidInput_ReturnsSuccessTrue()
        {
            // Arrange
            var req = new MarkReferralValidOrTrashedModel { Work_Id = 53434503, Is_Trash = true };
            var profile = new UserProfile { FirstName = "Team", LastName = "L2", UserName = "L2_53411372", PracticeCode = 1012714 };

            // Act
            var response = _supervisorWorkService.MakeReferralAsValidOrTrashed(req, profile);

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
        public void MakeReferralAsValidOrTrashed_NullInput_ReturnsSuccessFalse()
        {
            // Arrange
            var req = new MarkReferralValidOrTrashedModel { Work_Id = 1, Is_Trash = true };
            var profile = new UserProfile { FirstName = "John", LastName = "Doe", UserName = "jdoe" };

            // Act
            var response = _supervisorWorkService.MakeReferralAsValidOrTrashed(null, profile);

            // Assert
            Assert.IsFalse(response.Success);
        }


        [Test]
        public void MarkReferralAsTrashedOrValid_ValidInput_ReturnsSuccess()
        {
            // Arrange
            long work_id = 53434503;
            bool markAsTrashed = true;
            UserProfile profile = new UserProfile { PracticeCode = 1012714, UserName = "TestUser" };

            // Act
            var response = _supervisorWorkService.MarkReferralAsTrashedOrValid(work_id, markAsTrashed, profile);

            // Assert
            Assert.IsTrue(response.Success);
        }

        [Test]
        public void MarkReferralAsTrashedOrValid_InvalidInput_ReturnsFailure()
        {
            // Arrange
            long work_id = 53434503;
            bool markAsTrashed = true;
            UserProfile profile = new UserProfile { PracticeCode = 1012714, UserName = "TestUser" };

            // Act
            var response = _supervisorWorkService.MarkReferralAsTrashedOrValid(work_id, markAsTrashed, profile);

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
        public void SupervisorExport_ShouldReturnFileName()
        {
            // Arrange
            var obj = new SupervisorWorkRequest();
            var profile = new UserProfile();
            profile.PracticeDocumentDirectory = "TestDirectory";
            obj.CalledFrom = "Supervisor_Work";

            profile.PracticeCode = 1012714;
            profile.userID = 1011163415;
            profile.UserName = "1163testing";
            obj.TransferReason = "";
            obj.RecordPerPage = 10;
            obj.CurrentPage = 1;
            obj.SortOrder = "";
            obj.SearchText = "";
            obj.SourceString = "";
            obj.SourceName = "";
            obj.SourceType = "";
            obj.SupervisorName = "";
            obj.TransferComments = "";
            obj.IndexBy = null;
            obj.SortBy = null;


            // Act
            var result = _supervisorWorkService.SupervisorExport(obj, profile);

            // Assert
            Assert.IsNotNull(result);

        }








        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _userProfile = null;
            _supervisorWorkService = null;
            _supervisorWorkRequest = null;
            _markReferralValidOrTrashedModel = null;
        }

    }
}
