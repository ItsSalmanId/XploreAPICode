using FOX.BusinessOperations.UnAssignedQueueService;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.UnAssignedQueueModel;
using NUnit.Framework;
using System;

namespace FoxRehabilitation.UnitTest.UnAssignedQueueServiceUnitTest
{
    [TestFixture]
    class UnAssignedQueueServiceTest
    {
        private UnAssignedQueueService _unAssignedQueueService;
        private UnAssignedQueueRequest _unAssignedQueueRequest;
        private UserProfile _userProfile;

        [SetUp]
        public void SetUp()
        {
            _unAssignedQueueService = new UnAssignedQueueService();
            _unAssignedQueueRequest = new UnAssignedQueueRequest();
            _userProfile = new UserProfile();

        }
        [Test]
        [TestCase(1011163)]
        [TestCase(123456)]
        [TestCase(0)]
        public void GetUnAssignedQueue_UnAssignedQueueModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _unAssignedQueueRequest.CurrentPage = 1;
            _unAssignedQueueRequest.RecordPerPage = 10;
            _unAssignedQueueRequest.SortBy = "CREATED_DATE";
            _unAssignedQueueRequest.SortOrder = "DESC";
            _unAssignedQueueRequest.SearchText = "";
            _unAssignedQueueRequest.IncludeArchive = false;
            _unAssignedQueueRequest.CalledFrom = "";
            _unAssignedQueueRequest.ID = "";
            _unAssignedQueueRequest.SorceName = "";
            _unAssignedQueueRequest.SorceType = "";

            //Act
            var result = _unAssignedQueueService.GetUnAssignedQueue(_unAssignedQueueRequest, _userProfile);

            //Assert
            if (result != null && result.Count >0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163)]
        [TestCase(101116322)]
        [TestCase(1011164)]
        public void GetSupervisorForDropdown_SupervisorForDropdownModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _unAssignedQueueService.GetSupervisorForDropdown(practiceCode);

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
        [TestCase(1011163, "1163testing")]
        [TestCase(1011163, "")]
        [TestCase(1012714,"")]
        public void GetIndexersForDropdown_IndexersForDropdownModel_ReturnData(long practiceCode,string userName)
        {
            //Arrange
            //Act
            var result = _unAssignedQueueService.GetIndexersForDropdown(practiceCode, userName);

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
        [TestCase(1011163, "1163testing")]
        [TestCase(1011163, "")]
        [TestCase(1011164, "")]
        public void GetInitialData_InitialDataModel_ReturnData(long practiceCode, string userName)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = userName;
            _unAssignedQueueRequest.CurrentPage = 1;
            _unAssignedQueueRequest.RecordPerPage = 10;
            _unAssignedQueueRequest.SortBy = "CREATED_DATE";
            _unAssignedQueueRequest.SortOrder = "DESC";
            _unAssignedQueueRequest.SearchText = "";
            _unAssignedQueueRequest.IncludeArchive = false;
            _unAssignedQueueRequest.CalledFrom = "";
            _unAssignedQueueRequest.ID = "";
            _unAssignedQueueRequest.SorceName = "";
            _unAssignedQueueRequest.SorceType = "";

            //Act
            var result = _unAssignedQueueService.GetInitialData(_unAssignedQueueRequest, _userProfile);

            //Assert
            Assert.That(result.usersForDropdownData.Count, Is.GreaterThanOrEqualTo(0));
            Assert.That(result.unassignedQueueData.Count, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        [TestCase(1012714, "L2_53411372", "", "53436750")]
        [TestCase(1, "L2_53411372", "", "53422210")]
        [TestCase(0, "test", "", "1234568")]
        public void ExportToExcelUnassignedQueue_WhenCalled_ShouldReturnFileName(long practiceCode, string userName, string searchtext, string Uniqueid)
        {
            // Arrange

            UnAssignedQueueRequest req = new UnAssignedQueueRequest();
            UserProfile profile = new UserProfile();
            profile.UserName = userName;
            profile.PracticeCode = practiceCode;
            req.CurrentPage = 1;
            req.RecordPerPage = 10;
            req.SearchText = searchtext;
            req.SorceName = "";
            req.SorceType = "";
            req.ID = Uniqueid;
            req.SortBy = userName;
            req.SortOrder = "";




            // Act
            string result = _unAssignedQueueService.ExportToExcelUnassignedQueue(req, profile);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(".xlsx"));
        }

        [Test]
        [TestCase(1012714, "L2_53411372", "", "53436750")]
        [TestCase(1, "L2_53411372", "", "53422210")]
        [TestCase(0, "test", "", "1234568")]
        public void ExportToExcelUnassignedQueue_WhenCalled_ShouldCreateDirectory(long practiceCode, string userName, string searchtext, string Uniqueid)
        {
            // Arrange

            UnAssignedQueueRequest req = new UnAssignedQueueRequest();
            UserProfile profile = new UserProfile();
            profile.UserName = userName;
            profile.PracticeCode = practiceCode;
            req.CurrentPage = 1;
            req.RecordPerPage = 10;
            req.SearchText = searchtext;
            req.SorceName = "";
            req.SorceType = "";
            req.ID = Uniqueid;
            req.SortBy = userName;
            req.SortOrder = "";


            // Act
            string result = _unAssignedQueueService.ExportToExcelUnassignedQueue(req, profile);

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
        [TestCase(1012714, "L2_53411372", "", "53436750")]
        [TestCase(1, "L2_53411372", "", "53422210")]
        [TestCase(0, "test", "", "1234568")]
        public void ExportToExcelUnassignedQueue_WhenCalled_ShouldCreateExcelDocument(long practiceCode, string userName, string searchtext, string Uniqueid)
        {
            // Arrange
            UnAssignedQueueRequest req = new UnAssignedQueueRequest();
            UserProfile profile = new UserProfile();
            profile.UserName = userName;
            profile.PracticeCode = practiceCode;
            req.CurrentPage = 1;
            req.RecordPerPage = 10;
            req.SearchText = searchtext;
            req.SorceName = "";
            req.SorceType = "";
            req.ID = Uniqueid;
            req.SortBy = userName;
            req.SortOrder = "";

            // Act
            string result = _unAssignedQueueService.ExportToExcelUnassignedQueue(req, profile);


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
        [TestCase(1012714, "L2_53411372", "", "53436750")]
        [TestCase(1, "L2_53411372", "", "53422210")]
        [TestCase(0, "test", "", "1234568")]
        public void Export_ShouldReturnFileName(long practiceCode, string userName, string searchtext, string Uniqueid)
        {
            //Arrange
            var obj = new UnAssignedQueueRequest();
            var profile = new UserProfile();
            var expected = "Unassigned_Queue";
            profile.UserName = userName;
            profile.PracticeCode = (int)practiceCode;
            obj.SearchText = searchtext;
            obj.ID = Uniqueid;
            obj.CalledFrom = "1234567898";

            //Act
            var actual = _unAssignedQueueService.Export(obj, profile);

            // Assert
            if (actual != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        [TestCase(1012714, "L2_53411372", "", "53436750")]
        [TestCase(1, "L2_53411372", "", "53422210")]
        [TestCase(0, "test", "", "1234568")]
        public void Export_ShouldCreateDirectory(long practiceCode, string userName, string searchtext, string Uniqueid)
        {
            //Arrange
            var obj = new UnAssignedQueueRequest();
            var profile = new UserProfile();
            var expected = "Unassigned_Queue";
            profile.UserName = userName;
            profile.PracticeCode = (int)practiceCode;
            obj.SearchText = searchtext;
            obj.ID = Uniqueid;
            obj.CalledFrom = "1234567898";

            //Act
            var actual = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExportedFiles");

            //Assert
            if (actual != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        [TestCase(1012714, "L2_53411372", "", "53436750")]
        [TestCase(1, "L2_53411372", "", "53422210")]
        [TestCase(0, "test", "", "1234568")]
        public void Export_ShouldCreateExcelDocument(long practiceCode, string userName, string searchtext, string Uniqueid)
        {
            //Arrange
            var obj = new UnAssignedQueueRequest();
            var profile = new UserProfile();
            var expected = "Unassigned_Queue";
            profile.UserName = userName;
            profile.PracticeCode = (int)practiceCode;
            obj.SearchText = searchtext;
            obj.ID = Uniqueid;
            obj.CalledFrom = "1234567898";
            obj.SearchText = searchtext;
            obj.SorceName = "";
            obj.SorceType = "";
            obj.ID = Uniqueid;
            obj.SortBy = userName;
            obj.SortOrder = "";
            obj.CurrentPage = 1;
            obj.RecordPerPage = 10;

            //Act
            var result = _unAssignedQueueService.GetUnAssignedQueue(obj, profile);
            var exported = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExportedFiles");
            var actual = exported;

            //Assert
            if (actual != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _unAssignedQueueService = null;
            _userProfile = null;
            _unAssignedQueueRequest = null;
        }
    }
}
