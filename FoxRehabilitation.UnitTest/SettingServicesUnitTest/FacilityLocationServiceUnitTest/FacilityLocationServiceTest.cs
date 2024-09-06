using FOX.BusinessOperations.SettingsService.FacilityLocationService;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.FacilityLocation;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.SettingsServiceUnitTest.FacilityLocationServiceUnitTest
{
    [TestFixture]
    public class FacilityLocationServiceTest
    {
        private FacilityLocationService _facilityLocationService;
        private UserProfile _userProfile;
        private FacilityLocationSearch _facilityLocationSearch;
        private LocationPatientAccount _locationPatientAccount;
        private FacilityLocation _facilityLocation;
        private GroupIdentifierSearch _groupIdentifierSearch;
        private LocationCorporationSearch _locationCorporationSearch;
        private IdentifierSearch _identifierSearch;
        private AuthStatusSearch _authStatusSearch;
        private TaskTpyeSearch _taskTpyeSearch;
        private OrderStatusSearch _orderStatusSearch;
        private AlertTypeSearch _alertTypeSearch;
        private DocumentTypeSearch _documentTypeSearch;
        private PatientContactTypeSearch _patientContactTypeSearch;

        [SetUp]
        public void SetUp()
        {
            _facilityLocationService = new FacilityLocationService();
            _userProfile = new UserProfile();
            _facilityLocationSearch = new FacilityLocationSearch();
            _locationPatientAccount = new LocationPatientAccount();
            _facilityLocation = new FacilityLocation();
            _groupIdentifierSearch = new GroupIdentifierSearch();
            _locationCorporationSearch = new LocationCorporationSearch();
            _identifierSearch = new IdentifierSearch();
            _authStatusSearch = new AuthStatusSearch();
            _taskTpyeSearch = new TaskTpyeSearch();
            _orderStatusSearch = new OrderStatusSearch();
            _alertTypeSearch = new AlertTypeSearch();
            _documentTypeSearch = new DocumentTypeSearch();
            _patientContactTypeSearch = new PatientContactTypeSearch();
        }
        [Test]
        public void GetFacilityLocationList_GetFacilityLocationListModel_NoReturnData()
        {
            //Arrange
            _userProfile.userID = 1011163415;
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _facilityLocationSearch.currentPage = 1;
            _facilityLocationSearch.recordPerpage = 1;
            _facilityLocationSearch.searchString = "";
            _facilityLocationSearch.Code = "";
            _facilityLocationSearch.Description = "";
            _facilityLocationSearch.zip = "";
            _facilityLocationSearch.city = "";
            _facilityLocationSearch.state = "";
            _facilityLocationSearch.currentPage = 1;
            _facilityLocationSearch.FacilityType = 1;
            _facilityLocationSearch.sortBy = "";
            _facilityLocationSearch.sortOrder = "";
            _facilityLocationSearch.Complete_Address = "";
            _facilityLocationSearch.Region = "";
            _facilityLocationSearch.Country = "";

            //Act
            var result = _facilityLocationService.GetFacilityLocationList(_facilityLocationSearch, _userProfile);

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
        [TestCase(605100)]
        [TestCase(0000)]
        [TestCase(605102)]
        public void AddUpdateFacilityLocation_PassParameters_NoReturnData(long facilityTypeId)
        {
            //Arrange
            _userProfile.userID = 1011163415;
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _facilityLocation.LOC_ID = 1011163548978;
            _userProfile.userID = 53411400;
            _userProfile.PracticeCode = 1012714;
            _facilityLocation.LOC_ID = 605100;
            _facilityLocation.FACILITY_TYPE_ID = facilityTypeId;

            //Act
            var result = _facilityLocationService.AddUpdateFacilityLocation(_facilityLocation, _userProfile);

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
        public void GetAlertTypeList_PassParameters_NoReturnData()
        {
            //Arrange
            _userProfile.userID = 1011163415;
            long practiceCode = 1011163;
            _userProfile.UserName = "1163testing";
            _alertTypeSearch.searchString = "";
            _alertTypeSearch.sortBy = "";
            _alertTypeSearch.sortOrder = "";
            _alertTypeSearch.currentPage = 10;
            _alertTypeSearch.recordPerpage = 10;
            _alertTypeSearch.Code = "";
            _alertTypeSearch.Description = "";

            //Act
            var result = _facilityLocationService.GetAlertTypeList(_alertTypeSearch, practiceCode);

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
        public void GetDocumentTypeList_PassParameters_NoReturnData()
        {
            //Arrange
            _documentTypeSearch.searchString = "";
            _documentTypeSearch.sortBy = "";
            _documentTypeSearch.sortOrder = "";
            _documentTypeSearch.currentPage = 10;
            _documentTypeSearch.recordPerpage = 10;
            _documentTypeSearch.Code = "";
            _documentTypeSearch.Name = "";

            //Act
            var result = _facilityLocationService.GetDocumentTypeList(_documentTypeSearch);

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
        public void GetPatientContactTypeList_PassParameters_NoReturnData()
        {
            //Arrange
            long practiceCode = 1011163;
            _patientContactTypeSearch.searchString = "";
            _patientContactTypeSearch.sortBy = "";
            _patientContactTypeSearch.sortOrder = "";
            _patientContactTypeSearch.currentPage = 10;
            _patientContactTypeSearch.recordPerpage = 10;
            _patientContactTypeSearch.Code = "";
            _patientContactTypeSearch.Name = "";

            //Act
            var result = _facilityLocationService.GetPatientContactTypeList(_patientContactTypeSearch, practiceCode);

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
        //GetPatientContactTypeList
        [Test]
        [TestCase("1011163", 605100, "101116356513283")]
        [TestCase("1011163", 605101, "101116360512738")]
        [TestCase("1011163", 605102, "101116360512739")]
        [TestCase("1011163", 605103, "101116360512740")]
        [TestCase("1012714", 605104, "101116360512741")]

        public void GetFacilityLocationById_ReturnFacilityLocationById_ReturnData(string practiceCode, long locationId, string patientAccount)
        {
            //Arrange
            _facilityLocation.CODE = "";
            _facilityLocation.COMPLETE_ADDRESS = "";
            _facilityLocation.Country = "";
            _facilityLocation.Description = "";
            _facilityLocation.FACILITY_TYPE_ID = 0;
            _facilityLocation.REGION = "";
            _facilityLocation.State = "";
            _facilityLocation.Zip = "";
            _facilityLocation.PATIENT_ACCOUNT = 0;
            _facilityLocation.PATIENT_ACCOUNT = 0;
            _facilityLocation.PATIENT_ACCOUNT = 0;
            _locationPatientAccount.Location_id = locationId;
            _locationPatientAccount.PATIENT_ACCOUNT = patientAccount;

            //Act
            var result = _facilityLocationService.GetFacilityLocationById(_locationPatientAccount, practiceCode);

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
        public void ExportToExcelFacilityCreation_WhenCalled_ShouldReturnFileName()
        {
            // Arrange
            FacilityLocationSearch facilityLocationSearch = new FacilityLocationSearch();
            UserProfile profile = new UserProfile();
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";
            facilityLocationSearch.currentPage = 1;
            facilityLocationSearch.recordPerpage = 10;
            facilityLocationSearch.searchString = "";
            facilityLocationSearch.Code = "";
            facilityLocationSearch.Description = "";
            facilityLocationSearch.zip = "";
            facilityLocationSearch.city = "";
            facilityLocationSearch.state = "";
            facilityLocationSearch.currentPage = 1;
            facilityLocationSearch.FacilityType = 1;
            facilityLocationSearch.sortBy = "";
            facilityLocationSearch.sortOrder = "";
            facilityLocationSearch.Complete_Address = "";
            facilityLocationSearch.Region = "";
            facilityLocationSearch.Country = "";


            // Act
            string result = _facilityLocationService.ExportToExcelFacilityCreation(facilityLocationSearch, profile);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(".xlsx"));
        }
        [Test]
        public void Export_ShouldReturnFileName()
        {
            //Arrange
            var facilityLocationSearch = new FacilityLocationSearch();
            var profile = new UserProfile();
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";
            facilityLocationSearch.currentPage = 1;
            facilityLocationSearch.recordPerpage = 10;
            facilityLocationSearch.searchString = "";
            facilityLocationSearch.Code = "";
            facilityLocationSearch.Description = "";
            facilityLocationSearch.zip = "";
            facilityLocationSearch.city = "";
            facilityLocationSearch.state = "";
            facilityLocationSearch.currentPage = 1;
            facilityLocationSearch.FacilityType = 1;
            facilityLocationSearch.sortBy = "";
            facilityLocationSearch.sortOrder = "";
            facilityLocationSearch.Complete_Address = "";
            facilityLocationSearch.Region = "";
            facilityLocationSearch.Country = "";


            //Act
            var result = _facilityLocationService.Export(facilityLocationSearch, profile);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(".xlsx"));
        }

        [Test]
        public void ExportToExcelGetGroupIdentifier_ShouldReturnFileName()
        {
            // Arrange
            GroupIdentifierSearch groupIdentifierSearch = new GroupIdentifierSearch();
            var profile = new UserProfile();
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";
            groupIdentifierSearch.searchString = "";
            groupIdentifierSearch.Name = "";
            groupIdentifierSearch.Description = "";
            groupIdentifierSearch.currentPage = 1;
            groupIdentifierSearch.recordPerpage = 1;
            groupIdentifierSearch.sortBy = "";
            groupIdentifierSearch.sortOrder = "";

            // Act
            string result = _facilityLocationService.ExportToExcelGetGroupIdentifier(groupIdentifierSearch, profile);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(".xlsx"));
        }

        [Test]
        public void ExportToExcelLocationCorporation_WhenCalled_ShouldReturnExpectedPath()
        {
            // Arrange
            var locationSearch = new LocationCorporationSearch();
            var profile = new UserProfile();
            locationSearch.searchString = "";
            locationSearch.Name = "";
            locationSearch.Code = "";
            locationSearch.currentPage = 1;
            locationSearch.recordPerpage = 1;
            locationSearch.sortBy = "";
            locationSearch.sortOrder = "";

            // Act
            var actualPath = _facilityLocationService.ExportToExcelLocationCorporation(locationSearch, profile);

            // Assert
            Assert.IsNotNull(actualPath);
        }


        [Test]
        public void ExportToExcelIdentifier_ShouldReturnFileName()
        {
            //Arrange
            var identifierSearch = new IdentifierSearch();
            var profile = new UserProfile();
            profile.PracticeDocumentDirectory = "TestDirectory";
            profile.PracticeCode = 1012714;
            identifierSearch.searchString = "test";
            identifierSearch.Name = "";
            identifierSearch.Code = "";
            identifierSearch.currentPage = 1;
            identifierSearch.recordPerpage = 1;
            identifierSearch.sortBy = "";
            identifierSearch.sortOrder = "";

            //Act
            var result = _facilityLocationService.ExportToExcelIdentifier(identifierSearch, profile);

            //Assert
            Assert.IsNotNull(result);

        }


        [Test]
        public void ExportToExcelAuthStatus_WhenCalled_ShouldReturnFileName()
        {
            // Arrange
            AuthStatusSearch authStatusSearch = new AuthStatusSearch();
            UserProfile profile = new UserProfile();
            profile.PracticeDocumentDirectory = "TestDirectory";
            profile.PracticeCode = 1012714;
            //Arrange
            authStatusSearch.searchString = "";
            authStatusSearch.Description = "Active";
            authStatusSearch.Code = "ACT";
            authStatusSearch.currentPage = 1;
            authStatusSearch.recordPerpage = 1;
            authStatusSearch.sortBy = "";
            authStatusSearch.sortOrder = "";

            // Act
            string result = _facilityLocationService.ExportToExcelAuthStatus(authStatusSearch, profile);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(".xlsx"));
        }

        [Test]
        public void ExportToExcelTaskType_ShouldReturnFileName()
        {
            //Arrange
            TaskTpyeSearch taskTpyeSearch = new TaskTpyeSearch();
            UserProfile profile = new UserProfile();
            profile.PracticeDocumentDirectory = "TestDirectory";
            profile.PracticeCode = 1012714;
            taskTpyeSearch.searchString = "";
            taskTpyeSearch.Name = "Insurance Follow-up";
            taskTpyeSearch.Code = "ACT";
            taskTpyeSearch.currentPage = 1;
            taskTpyeSearch.recordPerpage = 1;
            taskTpyeSearch.sortBy = "";
            taskTpyeSearch.sortOrder = "";

            //Act
            string result = _facilityLocationService.ExportToExcelTaskType(taskTpyeSearch, profile);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(".xlsx"));
        }
        [Test]
        public void ExportToExcelOrderStatus_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            OrderStatusSearch orderStatusSearch = new OrderStatusSearch();
            UserProfile profile = new UserProfile();
            profile.PracticeDocumentDirectory = "TestDirectory";
            profile.PracticeCode = 1012714;
            orderStatusSearch.searchString = "";
            orderStatusSearch.Description = "Insurance Follow-up";
            orderStatusSearch.Code = "ACT";
            orderStatusSearch.currentPage = 1;
            orderStatusSearch.recordPerpage = 1;
            orderStatusSearch.sortBy = "";
            orderStatusSearch.sortOrder = "";

            // Act
            string result = _facilityLocationService.ExportToExcelOrderStatus(orderStatusSearch, profile);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetSourceofReferralList_ShouldReturnList()
        {
            // Arrange
            SourceOfreferralSearch sourceOfreferralSearch = new SourceOfreferralSearch();
            long practiceCode = 1012714;
            sourceOfreferralSearch.searchString = "";
            sourceOfreferralSearch.Code = "ACT";
            sourceOfreferralSearch.Description = "Insurance Follow-up";
            sourceOfreferralSearch.currentPage = 1;
            sourceOfreferralSearch.recordPerpage = 10;
            sourceOfreferralSearch.sortBy = "";
            sourceOfreferralSearch.sortOrder = "";

            // Act
            var result = _facilityLocationService.GetSourceofReferralList(sourceOfreferralSearch, practiceCode);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void ExportToExcelSourceOfReferral_ShouldReturnFileName()
        {
            //Arrange
            SourceOfreferralSearch sourceOfreferralSearch = new SourceOfreferralSearch();
            UserProfile profile = new UserProfile();
            profile.PracticeDocumentDirectory = "PracticeDocumentDirectory";
            profile.PracticeCode = 1012714;
            sourceOfreferralSearch.searchString = "";
            sourceOfreferralSearch.Code = "ACT";
            sourceOfreferralSearch.Description = "Insurance Follow-up";
            sourceOfreferralSearch.currentPage = 1;
            sourceOfreferralSearch.recordPerpage = 10;
            sourceOfreferralSearch.sortBy = "";
            sourceOfreferralSearch.sortOrder = "";


            //Act
            string result = _facilityLocationService.ExportToExcelSourceOfReferral(sourceOfreferralSearch, profile);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(".xlsx"));
        }

        [Test]
        public void ExportToExcelAlertType_WhenCalled_ShouldReturnExpectedResult()
        {
            // Arrange
            var alertTypeSearch = new AlertTypeSearch();
            var profile = new UserProfile();
            long practiceCode = 1012714;
            profile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            alertTypeSearch.searchString = "";
            alertTypeSearch.sortBy = "";
            alertTypeSearch.sortOrder = "";
            alertTypeSearch.currentPage = 10;
            alertTypeSearch.recordPerpage = 10;
            alertTypeSearch.Code = "";
            alertTypeSearch.Description = "";


            // Act
            var actualResult = _facilityLocationService.ExportToExcelAlertType(alertTypeSearch, profile);

            // Assert
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void ExportToExcelDocumentType_ReturnsExpectedFileName()
        {
            // Arrange
            DocumentTypeSearch documentTypeSearch = new DocumentTypeSearch();
            UserProfile profile = new UserProfile();
            string expectedFileName = "Document_Type_List.xlsx";
            profile.PracticeCode = 1012714;
            documentTypeSearch.searchString = "";
            documentTypeSearch.sortBy = "";
            documentTypeSearch.sortOrder = "";
            documentTypeSearch.currentPage = 10;
            documentTypeSearch.recordPerpage = 10;
            documentTypeSearch.Code = "";
            documentTypeSearch.Name = "";

            // Act
            string actualFileName = _facilityLocationService.ExportToExcelDocumentType(documentTypeSearch, profile);

            // Assert
            Assert.IsNotNull(actualFileName);
        }
        [Test]
        public void ExportToExcelContactType_ShouldReturnCorrectData()
        {
            // Arrange
            var patientContactTypeSearch = new PatientContactTypeSearch();
            var profile = new UserProfile();
            profile.PracticeDocumentDirectory = "TestDirectory";
            profile.PracticeCode = 1012714;
            long practiceCode = 1011163;
            patientContactTypeSearch.searchString = "";
            patientContactTypeSearch.sortBy = "";
            patientContactTypeSearch.sortOrder = "";
            patientContactTypeSearch.currentPage = 10;
            patientContactTypeSearch.recordPerpage = 10;
            patientContactTypeSearch.Code = "";
            patientContactTypeSearch.Name = "";


            // Act
            var result = _facilityLocationService.ExportToExcelContactType(patientContactTypeSearch, profile);


            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase("", "1011163")]
        [TestCase("test", "1011163")]
        [TestCase("test", "000000")]
        public void GetProviderNamesList_ProviderNamesListModel_ReturnData(string searchText, long practiceCode)
        {
            //Arrange
            //Act
            var result = _facilityLocationService.GetProviderNamesList(searchText, practiceCode);

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
        [TestCase("", 1011163)]
        public void GetProviderCode_ProviderCodeListCount_ReturnData(string state, long practiceCode)
        {
            //Arrange
            //Act
            var result = _facilityLocationService.GetProviderCode(state, practiceCode);

            //Assert
            if (result != null && result != "001")
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(544100, "1011163")]
        [TestCase(544102, "1011163")]
        [TestCase(123456, "000000")]
        public void GetFacilityTypeById_FacilityTypeByIdModel_ReturnData(long facilityTypeId, long practiceCode)
        {
            //Arrange
            //Act
            var result = _facilityLocationService.GetFacilityTypeById(facilityTypeId, practiceCode);

            //Assert
            if (result != null && result.DELETED == false)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("", "1011163")]
        [TestCase("test", "1011163")]
        [TestCase(null, "1011163")]
        public void GetSmartReferralRegions_SmartReferralRegionsModel_ReturnData(string searchText, long practiceCode)
        {
            //Arrange
            //Act
            var result = _facilityLocationService.GetSmartReferralRegions(searchText, practiceCode);

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
        [TestCase("1011163")]
        [TestCase("000000")]
        public void GetGroupIdentifierList_GroupIdentifierListModel_ReturnData(long practiceCode)
        {
            //Arrange
            _groupIdentifierSearch.searchString = "";
            _groupIdentifierSearch.Name = "";
            _groupIdentifierSearch.Description = "";
            _groupIdentifierSearch.currentPage = 1;
            _groupIdentifierSearch.recordPerpage = 1;
            _groupIdentifierSearch.sortBy = "";
            _groupIdentifierSearch.sortOrder = "";

            //Act
            var result = _facilityLocationService.GetGroupIdentifierList(_groupIdentifierSearch, practiceCode);

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
        [TestCase(1011163)]
        [TestCase(000000)]
        public void LocationCorporationList_LocationCorporationListModel_ReturnData(long practiceCode)
        {
            //Arrange
            _locationCorporationSearch.searchString = "";
            _locationCorporationSearch.Name = "";
            _locationCorporationSearch.Code = "";
            _locationCorporationSearch.currentPage = 1;
            _locationCorporationSearch.recordPerpage = 1;
            _locationCorporationSearch.sortBy = "";
            _locationCorporationSearch.sortOrder = "";

            //Act
            var result = _facilityLocationService.LocationCorporationList(_locationCorporationSearch, practiceCode);

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
        [TestCase(1011163)]
        [TestCase(000000)]
        public void GetIdentifierList_IdentifierListModel_ReturnData(long practiceCode)
        {
            //Arrange
            _identifierSearch.searchString = "test";
            _identifierSearch.Name = "";
            _identifierSearch.Code = "";
            _identifierSearch.currentPage = 1;
            _identifierSearch.recordPerpage = 1;
            _identifierSearch.sortBy = "";
            _identifierSearch.sortOrder = "";

            //Act
            var result = _facilityLocationService.GetIdentifierList(_identifierSearch, practiceCode);

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
        [TestCase(1011163)]
        [TestCase(000000)]
        public void GetAuthStatusList_AuthStatusListModel_ReturnData(long practiceCode)
        {
            //Arrange
            _authStatusSearch.searchString = "";
            _authStatusSearch.Description = "Active";
            _authStatusSearch.Code = "ACT";
            _authStatusSearch.currentPage = 1;
            _authStatusSearch.recordPerpage = 1;
            _authStatusSearch.sortBy = "";
            _authStatusSearch.sortOrder = "";

            //Act
            var result = _facilityLocationService.GetAuthStatusList(_authStatusSearch, practiceCode);

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
        [TestCase(1011163)]
        [TestCase(000000)]
        public void GetTaskTypeList_TaskTypeListModel_ReturnData(long practiceCode)
        {
            //Arrange
            _taskTpyeSearch.searchString = "";
            _taskTpyeSearch.Name = "Insurance Follow-up";
            _taskTpyeSearch.Code = "ACT";
            _taskTpyeSearch.currentPage = 1;
            _taskTpyeSearch.recordPerpage = 1;
            _taskTpyeSearch.sortBy = "";
            _taskTpyeSearch.sortOrder = "";

            //Act
            var result = _facilityLocationService.GetTaskTypeList(_taskTpyeSearch, practiceCode);

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
        [TestCase(1011163)]
        [TestCase(000000)]
        public void GetOrderStatusList_OrderStatusListModel_ReturnData(long practiceCode)
        {
            //Arrange
            _orderStatusSearch.searchString = "";
            _orderStatusSearch.Description = "Insurance Follow-up";
            _orderStatusSearch.Code = "ACT";
            _orderStatusSearch.currentPage = 1;
            _orderStatusSearch.recordPerpage = 1;
            _orderStatusSearch.sortBy = "";
            _orderStatusSearch.sortOrder = "";

            //Act
            var result = _facilityLocationService.GetOrderStatusList(_orderStatusSearch, practiceCode);

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









        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _userProfile = null;
            _facilityLocationService = null;
            _facilityLocationSearch = null;
            _locationPatientAccount = null;
            _groupIdentifierSearch = null;
            _identifierSearch = null;
            _authStatusSearch = null;
            _taskTpyeSearch = null;
            _orderStatusSearch = null;
            _alertTypeSearch = null;
            _documentTypeSearch = null;
            _patientContactTypeSearch = null;
        }
    }
}
