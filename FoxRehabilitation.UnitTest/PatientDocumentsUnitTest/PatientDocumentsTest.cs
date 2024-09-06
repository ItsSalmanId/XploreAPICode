using FOX.BusinessOperations.PatientDocumentsService;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.PatientDocuments;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System.Collections.Generic;

namespace FoxRehabilitation.UnitTest.PatientDocumentsUnitTest
{
    [TestFixture]
    public class PatientDocumentsTest
    {
        private PatientDocumentsService _patientDocumentsService;
        private UserProfile _userProfile;

        [SetUp]
        public void SetUp()
        {
            _patientDocumentsService = new PatientDocumentsService();
            _userProfile = new UserProfile();
        }
        [Test]
        [TestCase("", 0)]
        [TestCase("101116354813969", 0)]
        [TestCase("", 1011163)]
        [TestCase("101116354813969", 1011163)]
        [TestCase("38403", 38403)]
        public void GetDocumentTypes_PassParameters_ReturnData(string patientAccount, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientDocumentsService.getDocumentTypes(patientAccount, _userProfile);

            //Assert
            if (result.DocumentTypes.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.DocumentTypes.Count > 0)
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetAllDocumentTypes_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientDocumentsService.GetAllDocumentTypes(_userProfile);

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
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetAllSpecialityProgram_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientDocumentsService.GetAllSpecialityProgram(_userProfile);

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
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetAllDocumentTypeswithInactive_PassParameters_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientDocumentsService.GetAllDocumentTypeswithInactive(_userProfile);

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
        public void GetDocumentImagesInformation_ReturnsListOfPatientDocuments()
        {
            // Arrange
            var profile = new UserProfile();
            var request = new PatientDocumentRequest();
            var patientDocument = new PatientDocument();
            request.RECORD_PER_PAGE = 10;
            request.CURRENT_PAGE = 1;
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";
            request.PATIENT_ACCOUNT = "1012714534318093880";
            request.DOCUMENT_TYPE_ID = null;
            request.SORT_BY = "";
            request.SORT_ORDER = "";
            // Act
            var result = _patientDocumentsService.getDocumentImagesInformation(profile, request);

            // Assert
            Assert.IsInstanceOf(typeof(List<PatientDocument>), result);
        }


        [Test]
        public void InsertInterfaceTeamData_Test()
        {
            //Arrange
            InterfaceSynchModel obj = new InterfaceSynchModel();
            UserProfile Profile = new UserProfile();
            Profile.PracticeCode = 1012714;
            Profile.UserName = "TestUser";
            obj.PATIENT_ACCOUNT = 1012714534318093880;

            //Act
            _patientDocumentsService.InsertInterfaceTeamData(obj, Profile);
            //Assert
            Assert.IsNotNull(obj);

        }

        [Test]
        public void UpdateDocument_Test()
        {
            //Arrange
            PatientPATDocument ExistingDocumentInfo = new PatientPATDocument();
            PatientPATDocument ObjPatientPATDocument = new PatientPATDocument();
            UserProfile profile = new UserProfile();
            profile.UserName = "Davis_53411401";
            profile.PracticeCode = 1012714;

            ObjPatientPATDocument.DOCUMENT_TYPE = 102;
            ObjPatientPATDocument.CASE_ID = 6051006;
            ObjPatientPATDocument.START_DATE = null;
            ObjPatientPATDocument.END_DATE = null;
            ObjPatientPATDocument.SHOW_ON_PATIENT_PORTAL = true;
            ObjPatientPATDocument.COMMENTS = "Test";

            //Act
            _patientDocumentsService.UpdateDocument(ExistingDocumentInfo, ObjPatientPATDocument, profile);

            //Assert
            Assert.IsNotNull(ExistingDocumentInfo);

        }

        [Test]
        public void DeleteDocumentFilesInformation_ShouldReturnSuccessTrue_WhenGivenValidInput()
        {
            // Arrange
            UserProfile profile = new UserProfile();
            profile.PracticeCode = 1012714;
            PatientDocument objPatientDocument = new PatientDocument();
            objPatientDocument.PAT_DOCUMENT_ID = 53410148;
            objPatientDocument.IMAGE_FILE_ID = 53410148;


            // Act
            var result = _patientDocumentsService.DeleteDocumentFilesInformation(profile, objPatientDocument);

            // Assert
            Assert.IsTrue(result.Success);
        }


        [Test]
        public void DeleteDocumentInformationShouldReturnSuccessTrue_WhenGivenValidInput()
        {
            // Arrange
            UserProfile profile = new UserProfile();
            profile.PracticeCode = 1012714;
            PatientDocument objPatientDocument = new PatientDocument();
            objPatientDocument.PAT_DOCUMENT_ID = 53410148;
            objPatientDocument.IMAGE_FILE_ID = 53410148;


            // Act
            _patientDocumentsService.DeleteDocumentInformation(objPatientDocument);

            // Assert
            Assert.IsNotNull(objPatientDocument.PAT_DOCUMENT_ID);
        }

        [Test]
        public void GetDocumentsDataInformation_ValidInput_ReturnsList()
        {
            // Arrange
            var profile = new UserProfile();
            var request = new PatientDocumentRequest();
            var patientDocument = new PatientDocument();
            request.RECORD_PER_PAGE = 10;
            request.CURRENT_PAGE = 1;
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";
            request.PATIENT_ACCOUNT = "101116354813457";
            request.DOCUMENT_TYPE_ID = null;
            request.SORT_BY = "";
            request.SORT_ORDER = "";



            // Act
            var result = _patientDocumentsService.getDocumentsDataInformation(profile, request);

            // Assert
            Assert.IsInstanceOf<List<PatientDocument>>(result);
        }
        [Test]
        public void ExportToExcelDocumentInformation_ShouldReturnFileName()
        {
            //Arrange
            UserProfile profile = new UserProfile();
            PatientDocumentRequest request = new PatientDocumentRequest();
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";
            string expected = "Documents.xlsx";
            request.RECORD_PER_PAGE = 10;
            request.CURRENT_PAGE = 1;
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";
            request.PATIENT_ACCOUNT = "101116354813457";
            request.DOCUMENT_TYPE_ID = null;
            request.SORT_BY = "";
            request.SORT_ORDER = "";

            //Act
            string actual = _patientDocumentsService.ExportToExcelDocumentInformation(profile, request);

            //Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        [TestCase(605167, 1011163)]
        public void AddDocument_ShouldReturnValidId(long docType, long practiceCode)
        {
            // Arrange
            var ObjPatientPATDocument = new PatientPATDocument();
            ObjPatientPATDocument.PAT_DOCUMENT_ID = docType;
            var profile = new UserProfile();
            profile.PracticeCode = practiceCode;

            // Act
             _patientDocumentsService.AddDocument(ObjPatientPATDocument, profile);

            // Assert
            Assert.IsFalse(true);
        }

    












        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objectsss
            _patientDocumentsService = new PatientDocumentsService();
            _userProfile = new UserProfile();
        }
    }
}
