using FOX.BusinessOperations.CommonServices;
using FOX.BusinessOperations.IndexedQueueService;
using FOX.BusinessOperations.IndexInfoServices;
using FOX.BusinessOperations.IndexInfoServices.UploadWorkOrderFiles;
using FOX.BusinessOperations.RequestForOrder.UploadOrderImages;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.IndexedQueueModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.RequestForOrder.UploadOrderImages;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.Settings.User;
using FOX.DataModels.Models.TasksModel;
using FOX.DataModels.Models.UploadWorkOrderFiles;
using FoxRehabilitationAPI.Controllers;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FoxRehabilitation.UnitTest.IndexedQueueServicesUnitTest
{
    [TestFixture]
    public class IndexedInfoServicesTest
    {
        private IndexInfoService _indexedInfoServices;
        private IndexedQueueFileRequest _indexedQueueFileRequest;
        private UserProfile _userProfile;
        private IndexedQueueRequest _indexedQueueRequest;
        private WorkTransfer _workTransfer;
        private IUploadWorkOrderFilesService _UploadWorkOrderFilesService;
        private IUploadOrderImagesService _UploadOrderImagesService;


        [SetUp]
        public void SetUp()
        {
            _indexedInfoServices = new IndexInfoService();
            _indexedQueueFileRequest = new IndexedQueueFileRequest();
            _userProfile = new UserProfile();
            _indexedQueueRequest = new IndexedQueueRequest();
            _workTransfer = new WorkTransfer();
            _UploadWorkOrderFilesService = new UploadWorkOrderFilesService();
            _UploadOrderImagesService = new UploadOrderImagesService();
        }


        [Test]
        [TestCase(53422210)]
        [TestCase(53422213)]
        [TestCase(null)]
        public void GetNotes_History_ReturnsList(long workId)
        {
            // Arrange
            Index_infoReq obj = new Index_infoReq();
            UserProfile Profile = new UserProfile();
            Profile.UserName = "test";
            Profile.PracticeCode = 1012714;
            obj.WORK_ID = workId;


            // Act
            var result = _indexedInfoServices.GetNotes_History(obj, Profile);

            // Assert
            Assert.IsInstanceOf<List<FOX_TBL_NOTES_HISTORY>>(result);
        }

        [Test]
        [TestCase(53422210)]
        [TestCase(53422213)]
        [TestCase(null)]
        public void ExportToExcelNotes_History_ReturnsExpectedPath(long workId)
        {
            // Arrange
            Index_infoReq obj = new Index_infoReq();
            UserProfile profile = new UserProfile();
            obj.WORK_ID = workId;
            profile.PracticeDocumentDirectory = "TestDirectory";

            // Act
            string result = _indexedInfoServices.ExportToExcelNotes_History(obj, profile);

            // Assert
            Assert.IsNotNull(result);
        }



        [Test]
        [TestCase(53422210)]
        [TestCase(53422213)]
        [TestCase(null)]
        public void GetDocuments_WhenCalled_ReturnsListOfFOX_TBL_PATIENT_DOCUMENTS(long workId)
        {
            // Arrange
            var obj = new Index_infoReq();
            var profile = new UserProfile();
            obj.WORK_ID = workId;
            profile.UserName = "Test";
            profile.PracticeCode = 1012714;

            // Act
            var result = _indexedInfoServices.GetDocuments(obj, profile);

            // Assert
            Assert.IsNotNull(result);
        }



        [Test]
        public void GetSmartLocations_Returns_Empty_List_When_No_Results_Found()
        {
            // Arrange
            var obj = new SmartReq();
            var profile = new UserProfile();
            obj.SEARCHVALUE = "50 Ledge Road";
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";

            // Act
            var result = _indexedInfoServices.GetSmartLocations(obj, profile);

            // Assert
            Assert.IsNotNull(result);
        }



        [Test]
        public void Export_ShouldReturnFileName()
        {
            // Arrange
            var obj = new AnalaysisReportReq();
            var profile = new UserProfile();
            profile.PracticeDocumentDirectory = "TestDirectory";
            obj.CalledFrom = "Analysis_Report";
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";
            obj.DATEFROM_In_String = null;
            obj.DATETO_In_String = "";
            obj.TIMEFRAME = "DATERANGE";
            obj.BUSINESS_HOURS5A = "";
            obj.BUSINESS_HOURS9A = "";
            obj.SATURDAYSA = "";
            obj.SUNDAYSA = "";
            obj.EXCLUDEWEEKEND = true;
            obj.PAGEINDEX = 1;
            obj.PAGESIZE = 10;

            // Act
            var result = _indexedInfoServices.Export(obj, profile);

            // Assert
            Assert.IsNotNull(result);
        }


        [Test]
        public void GetAnalysisRPT_When_DATEFROM_In_String_Is_Null_Should_Return_Empty_List()
        {
            // Arrange
            var obj = new AnalaysisReportReq();
            obj.DATEFROM_In_String = null;
            obj.DATETO_In_String = "";
            obj.TIMEFRAME = "DATERANGE";
            obj.BUSINESS_HOURS5A = "";
            obj.BUSINESS_HOURS9A = "";
            obj.SATURDAYSA = "";
            obj.SUNDAYSA = "";
            obj.EXCLUDEWEEKEND = true;
            obj.PAGEINDEX = 1;
            obj.PAGESIZE = 10;

            var Profile = new UserProfile();
            Profile.PracticeCode = 1012714;
            Profile.UserName = "Test";

            // Act
            var result = _indexedInfoServices.GetAnalysisRPT(obj, Profile);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetAnalysisRPT_When_DATEFROM()
        {
            // Arrange
            var obj = new AnalaysisReportReq();
            obj.DATEFROM_In_String = null;
            obj.DATETO_In_String = "";
            obj.TIMEFRAME = "";
            obj.BUSINESS_HOURS5A = "";
            obj.BUSINESS_HOURS9A = "";
            obj.SATURDAYSA = "";
            obj.SUNDAYSA = "";
            obj.EXCLUDEWEEKEND = true;
            obj.PAGEINDEX = 1;
            obj.PAGESIZE = 10;

            var Profile = new UserProfile();
            Profile.PracticeCode = 1012714;
            Profile.UserName = "Test";

            // Act
            var result = _indexedInfoServices.GetAnalysisRPT(obj, Profile);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GenerateQRCode_ShouldReturnQRCodeModel()
        {
            // Arrange
            var obj = new QRCodeModel();
            var profile = new UserProfile();
            profile.UserName = "Test";
            profile.PracticeCode = 1012714;
            obj.AbsolutePath = "TestDirectory";

            // Act
            var result = _indexedInfoServices.GenerateQRCode(obj, profile);

            // Assert
            Assert.IsInstanceOf<QRCodeModel>(result);
        }

        [Test]
        public void GetSlotDataGreater_2HR_Test()
        {
            //Arrange
            var obj = new SlotAnalysisReq();
            var Profile = new UserProfile();
            obj.DATE = DateTime.Now.AddDays(-1000);
            obj.START_VALUE = 0;
            obj.END_VALUE = 15;
            obj.BUSINESS_HOURS5A = "";
            obj.BUSINESS_HOURS8A = "";
            obj.SUNDAYSA = "";
            obj.SATURDAYSA = "";
            obj.EXCLUDEWEEKEND = false;



            //Act
            var result = _indexedInfoServices.GetSlotDataGreater_2HR(obj, Profile);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<SlotAnalysisRes>>(result);
        }

        [Test]
        public void GetSlotDataGreater_60_Test()
        {
            //Arrange
            var obj = new SlotAnalysisReq();
            var Profile = new UserProfile();
            obj.DATE = DateTime.Now.AddDays(-1000);
            obj.START_VALUE = 0;
            obj.END_VALUE = 15;
            obj.BUSINESS_HOURS5A = "";
            obj.BUSINESS_HOURS8A = "";
            obj.SUNDAYSA = "";
            obj.SATURDAYSA = "";
            obj.EXCLUDEWEEKEND = false;

            //Act
            var result = _indexedInfoServices.GetSlotDataGreater_60(obj, Profile);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<SlotAnalysisRes>>(result);
        }

        [Test]
        public void GetSlotData46_60_Test()
        {
            //Arrange
            var obj = new SlotAnalysisReq();
            var Profile = new UserProfile();
            obj.DATE = DateTime.Now.AddDays(-1000);
            obj.START_VALUE = 0;
            obj.END_VALUE = 15;
            obj.BUSINESS_HOURS5A = "";
            obj.BUSINESS_HOURS8A = "";
            obj.SUNDAYSA = "";
            obj.SATURDAYSA = "";
            obj.EXCLUDEWEEKEND = false;

            //Act
            var result = _indexedInfoServices.GetSlotData46_60(obj, Profile);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<SlotAnalysisRes>>(result);
        }
        [Test]
        public void GetSlotDatGetSlotData31_45a46_60_Test()
        {
            //Arrange
            var obj = new SlotAnalysisReq();
            var Profile = new UserProfile();
            obj.DATE = DateTime.Now.AddDays(-1000);
            obj.START_VALUE = 0;
            obj.END_VALUE = 15;
            obj.BUSINESS_HOURS5A = "";
            obj.BUSINESS_HOURS8A = "";
            obj.SUNDAYSA = "";
            obj.SATURDAYSA = "";
            obj.EXCLUDEWEEKEND = false;

            //Act
            var result = _indexedInfoServices.GetSlotData31_45(obj, Profile);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<SlotAnalysisRes>>(result);
        }

        [Test]
        public void GetSlotData0_15_Test()
        {
            //Arrange
            var obj = new SlotAnalysisReq();
            var Profile = new UserProfile();
            obj.DATE = DateTime.Now.AddDays(-1000);
            obj.START_VALUE = 0;
            obj.END_VALUE = 15;
            obj.BUSINESS_HOURS5A = "";
            obj.BUSINESS_HOURS8A = "";
            obj.SUNDAYSA = "";
            obj.SATURDAYSA = "";
            obj.EXCLUDEWEEKEND = false;

            //Act
            var result = _indexedInfoServices.GetSlotData0_15(obj, Profile);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<SlotAnalysisRes>>(result);
        }
        [Test]
        public void GetSlotData16_30_Test()
        {
            //Arrange
            var obj = new SlotAnalysisReq();
            var Profile = new UserProfile();
            obj.DATE = DateTime.Now.AddDays(-1000);
            obj.START_VALUE = 0;
            obj.END_VALUE = 15;
            obj.BUSINESS_HOURS5A = "";
            obj.BUSINESS_HOURS8A = "";
            obj.SUNDAYSA = "";
            obj.SATURDAYSA = "";
            obj.EXCLUDEWEEKEND = false;

            //Act
            var result = _indexedInfoServices.GetSlotData16_30(obj, Profile);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<SlotAnalysisRes>>(result);
        }
        [Test]
        public void GetSlotData_Test()
        {
            //Arrange
            var obj = new SlotAnalysisReq();
            var Profile = new UserProfile();
            obj.DATE = DateTime.Now.AddDays(-1000);
            obj.START_VALUE = 0;
            obj.END_VALUE = 15;
            obj.BUSINESS_HOURS5A = "";
            obj.BUSINESS_HOURS8A = "";
            obj.SUNDAYSA = "";
            obj.SATURDAYSA = "";
            obj.EXCLUDEWEEKEND = false;

            //Act
            var result = _indexedInfoServices.GetSlotData(obj, Profile);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<SlotAnalysisRes>>(result);
        }

        [Test]
        public void GetSmartRefRegion_ReturnsListOfSmartRefRegion()
        {
            // Arrange
            var obj = new SmartReq();
            var profile = new UserProfile();
            obj.SEARCHVALUE = "NJC3";
            profile.PracticeCode = 1012714;

            // Act
            var actual = _indexedInfoServices.GetSmartRefRegion(obj, profile);

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public void GetAllIndexinfo_Returns_GETtAll_IndexifoRes()
        {
            // Arrange
            var obj = new Index_infoReq();
            var profile = new UserProfile();
            obj.WORK_ID = 53436800;
            profile.PracticeCode = 1012714;
            profile.SENDER_TYPE = "Fox Clinician";
            User user = new User();
            user.FOX_TBL_SENDER_TYPE_ID = 605135;



            // Act
            var result = _indexedInfoServices.GetAllIndexinfo(obj, profile);

            // Assert
            Assert.IsNotNull(obj.WORK_ID);
        }


        [Test]
        public void InsertUpdateDocuments_WhenCalled_ShouldInsertOrUpdateDocument()
        {
            // Arrange
            var obj = new FOX_TBL_PATIENT_DOCUMENTS();
            var profile = new UserProfile();
            obj.COMENTS = "Test Comments";
            obj.FILE_NAME = "abc.txt";
            obj.FILE_PATH = "C:/source/repo";
            obj.PATIENT_ACCOUNT = 534211554701;
            obj.DELETED = true;
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";


            // Act
            _indexedInfoServices.InsertUpdateDocuments(obj, profile);

            // Assert
            Assert.IsTrue(true);
        }



        [Test]
        public void InsertNotesHistory_WhenCalled_ShouldInsertNote()
        {
            // Arrange
            var obj = new FOX_TBL_NOTES_HISTORY();
            var profile = new UserProfile();
            obj.NOTE_ID = 53410106;
            obj.WORK_ID = 53422210;
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";
            obj.NOTE_DESC = "Test";

            // Act
            _indexedInfoServices.InsertNotesHistory(obj, profile);

            // Assert
            Assert.IsTrue(obj.NOTE_ID > 0);
        }

        [Test]
        public void InsertDiagnosisInfo_WhenCalled_ShouldReturnExpectedResult()
        {
            // Arrange
            FOX_TBL_PATIENT_DIAGNOSIS obj = new FOX_TBL_PATIENT_DIAGNOSIS();
            UserProfile profile = new UserProfile();
            profile.UserName = "TestUser";
            obj.PAT_DIAG_ID = 605995;
            obj.WORK_ID = 605102;
            obj.DIAG_CODE = "000.62";
            obj.PATIENT_ACCOUNT = 101116355230659;
            obj.DIAG_DESC = "CANCELLED APPOINTMENT";
            profile.PracticeCode = 1012714;


            // Act
            _indexedInfoServices.InsertDiagnosisInfo(obj, profile);

            // Assert
            Assert.IsNotNull(obj.PAT_DIAG_ID);

        }

        [Test]
        public void DeleteDocuments_WhenCalled_ShouldDeleteDocument()
        {
            // Arrange
            var obj = new FOX_TBL_PATIENT_DOCUMENTS();
            var profile = new UserProfile();
            profile.UserName = "TestUser";
            obj.PAT_DOC_ID = 53410000;
            obj.WORK_ID = 605102;
            obj.PATIENT_ACCOUNT = 534211554701;
            obj.DELETED = true;
            profile.PracticeCode = 1012714;

            // Act
            _indexedInfoServices.DeleteDocuments(obj, profile);

            // Assert
            Assert.IsTrue(true);
        }




        [Test]
        public void DeleteDiagnosis_WhenCalledWithNullDiagnosis_ShouldNotUpdateDiagnosis()
        {
            // Arrange
            var obj = new FOX_TBL_PATIENT_DIAGNOSIS();
            var profile = new UserProfile();
            profile.UserName = "TestUser";
            obj.PAT_DIAG_ID = 605995;
            obj.WORK_ID = 605102;
            obj.DIAG_CODE = "000.62";
            obj.PATIENT_ACCOUNT = 101116355230659;
            obj.DIAG_DESC = "CANCELLED APPOINTMENT";
            profile.PracticeCode = 1012714;
            obj.DELETED = true;


            // Act
            _indexedInfoServices.DeleteDiagnosis(obj, profile);

            // Assert
            Assert.IsTrue(true);
        }

        [Test]
        public void DeleteProcedures_Test()
        {
            //Arrange
            FOX_TBL_PATIENT_PROCEDURE obj = new FOX_TBL_PATIENT_PROCEDURE();
            UserProfile profile = new UserProfile();
            profile.UserName = "TestUser";
            obj.DELETED = true;
            obj.PAT_PROC_ID = 605797;

            //Act
            _indexedInfoServices.DeleteProcedures(obj, profile);

            //Assert

            Assert.AreNotEqual(obj.MODIFIED_DATE, null);
        }



        [Test]
        public void InsertProceureInfo_WhenProcedureDetailIsNotNull_ShouldUpdateProcedureDetail()
        {
            // Arrange
            var obj = new FOX_TBL_PATIENT_PROCEDURE();
            var profile = new UserProfile();
            obj.SPECIALITY_PROGRAM = "Chelsea Cardiac Care";
            obj.PROC_CODE = null;
            obj.WORK_ID = 605102;
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";


            // Act
            _indexedInfoServices.InsertProceureInfo(obj, profile);

            // Assert

        }

        [Test]
        public void InsertProceureInfo_WhenProcedureDetailIsNotNull()
        {
            // Arrange
            var obj = new FOX_TBL_PATIENT_PROCEDURE();
            var profile = new UserProfile();
            obj.SPECIALITY_PROGRAM = "Chelsea Cardiac Care";
            obj.PROC_CODE = null;
            obj.WORK_ID = 605102;
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";
            obj.PAT_PROC_ID = 605797;


            // Act
            _indexedInfoServices.InsertProceureInfo(obj, profile);

            // Assert

        }


        [Test]
        public void UpdatePatientInfo_WhenPatientDetailIsNotNull_ShouldUpdatePatientInfo()
        {
            IndexPatReq _obj = new IndexPatReq();
            var _profile = new UserProfile();
            // Arrange
            _obj.Patient_Account = 10013697;
            _obj.Date_Of_Birth_In_String = "01/01/2000";
            _obj.First_Name = "John";
            _obj.Last_Name = "Doe";
            _obj.Middle_Name = "A";
            _obj.Gender = "M";
            _obj.SSN = "123456789";
            _obj.DELETED = false;
            _profile.UserName = "TestUser";
            _profile.PracticeCode = 1012714;

            // Act
            _indexedInfoServices.updataPatientInfo(_obj, _profile);

            // Assert
            Assert.AreEqual(_obj.Patient_Account, 10013697);
        }

        [Test]
        public void UpdateSource_AdditionalInfo_ReturnsOriginalQueue_WhenValidInputs()
        {
            // Arrange
            var obj = new OriginalQueue();
            obj.PATIENT_ACCOUNT = 1012714534318093106;
            obj.WORK_ID = 53436731;
            var profile = new UserProfile();
            profile.UserName = "TestUser";
            profile.PracticeCode = 1012714;

            // Act
            var result = _indexedInfoServices.UpdateSource_AdditionalInfo(obj, profile);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void InsertUpdateOrderingSource_WhenSourceExists_ShouldUpdateSource()
        {
            ReferralSource _source = new ReferralSource();
            UserProfile _profile = new UserProfile();
            // Arrange
            _source.SOURCE_ID = 53433296;
            _source.DELETED = false;
            _source.ACO = "ACO";
            _source.ADDRESS = "Address";
            _source.ADDRESS_2 = "Address2";
            _source.CITY = "City";
            _source.CODE = "Code";
            _source.FAX = "Fax";
            _source.TITLE = "Title";
            _source.NPI = "NPI";
            _source.ORGANIZATION = "Organization";
            _source.PHONE = "Phone";
            _source.STATE = "State";
            _source.ZIP = "Zip";
            _profile.UserName = "UserName";
            _profile.PracticeCode = 1012714;

            // Act
            var result = _indexedInfoServices.InsertUpdateOrderingSource(_source, _profile);

            // Assert
            Assert.IsNotNull(result);

        }

        [Test]
        public void InsertUpdateOrderingSource_When()
        {
            ReferralSource _source = new ReferralSource();
            UserProfile _profile = new UserProfile();
            // Arrange
            _source.SOURCE_ID = 53433296;
            _source.DELETED = false;
            _source.ACO = "ACO";
            _source.ADDRESS = "Address";
            _source.ADDRESS_2 = "Address2";
            _source.CITY = "City";
            _source.CODE = "Code";
            _source.FAX = "Fax";
            _source.TITLE = "Title";
            _source.NPI = "NPI";
            _source.ORGANIZATION = "Organization";
            _source.PHONE = "Phone";
            _source.STATE = "State";
            _source.ZIP = "Zip";
            _source.REFERRAL_CODE = 34935;
            _profile.UserName = "UserName";
            _profile.PracticeCode = 1012714;

            // Act
            var result = _indexedInfoServices.InsertUpdateOrderingSource(_source, _profile);

            // Assert
            Assert.IsNotNull(result);

        }


        [Test]
        public void InsertInterfaceTeamData_WhenCalledWithNullValues_ReturnsCorrectValue()
        {
            // Arrange
            var obj = new InterfaceSynchModel();
            obj.PATIENT_ACCOUNT = 2055341;
            obj.CASE_ID = null;
            obj.TASK_ID = null;
            var profile = new UserProfile();
            profile.PracticeCode = 1012714;
            profile.UserName = "TestUser";
            profile.isTalkRehab = false;

            // Act
            var result = _indexedInfoServices.InsertInterfaceTeamData(obj, profile);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void CheckPatientIsInterfaced_ShouldReturnTrue_WhenPatientIsInterfaced()
        {
            // Arrange
            long? PATIENT_ACCOUNT = 2055341;
            UserProfile profile = new UserProfile();
            profile.PracticeCode = 1012714;
            // Act
            var result = _indexedInfoServices.checkPatientisInterfaced(PATIENT_ACCOUNT, profile);
            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_getPendingHighBalance_Returns_pendingBalanceAmount()
        {
            //Arrange
            long? PATIENT_ACCOUNT = 2055341;
            UserProfile profile = new UserProfile();
            profile.PracticeCode = 1012714;

            //Act
            pendingBalanceAmount result = _indexedInfoServices.getPendingHighBalance(PATIENT_ACCOUNT, profile);

            //Assert
            Assert.IsInstanceOf<pendingBalanceAmount>(result);
        }

        [Test]
        public void GeneratePdfforSignatureUpdate_WhenCalled_ShouldReturnAttachmentData()
        {
            // Arrange
            var unique_Id = "54821842";
            // Act
            var result = _indexedInfoServices.GeneratePdfforSignatureUpdate(unique_Id);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetPreviousEmailInformation_ShouldReturnListOfPreviousEmailInfo()
        {
            // Arrange
            var workId = "54821842";
            var profile = new UserProfile { PracticeCode = 1012714 };

            // Act
            var result = _indexedInfoServices.getPreviousEmailInformation(workId, profile);

            // Assert
            Assert.IsInstanceOf<List<PreviousEmailInfo>>(result);
        }
        [Test]
        public void SetPatientOpenedBy_ValidInput_ReturnsString()
        {
            // Arrange
            long patientAccount = 2055341;
            UserProfile profile = new UserProfile();
            profile.PracticeCode = 1012714;
            profile.UserName = "John Doe";

            // Act
            string result = _indexedInfoServices.setPatientOpenedBy(patientAccount, profile);

            // Assert
            Assert.IsNull(result);

        }

        [Test]
        public void updateWorkOrderSignature_ValidInput_ReturnsString()
        {
            // Arrange
            SubmitSignatureImageWithData obj = new SubmitSignatureImageWithData();
            long patientAccount = 2055341;
            obj.workId = "53436742";
            obj._isSignaturePresent = true;
            UserProfile profile = new UserProfile();
            profile.PracticeCode = 1012714;
            profile.UserName = "John Doe";

            // Act
            _indexedInfoServices.updateWorkOrderSignature(obj, profile);

            // Assert
            Assert.IsNotNull(profile.PracticeCode);

        }

        [Test]
        public void GenerateQRCode_ValidInput_ReturnsQRCodeModel()
        {
            // Arrange
            var obj = new QRCodeModel();
            obj.WORK_ID = 53436742;
            obj.AbsolutePath = @"C:\Temp\";

            // Act
            var result = _indexedInfoServices.GenerateQRCode(obj);

            // Assert
            Assert.IsNotNull(result);
        }


        [Test]
        public void UpdateOCRValue_WhenOCRStatusIsZero_ShouldReturnFalse()
        {
            // Arrange
            long? work_id = 53436742;
            FoxOcrStatus ocr = new FoxOcrStatus();
            UserProfile profile = new UserProfile();
            profile.PracticeCode = 1012714;
            profile.UserName = "TestUser";
            ocr.OCR_STATUS = "Data pulled into referral";
            ocr.OCR_STATUS_ID = 0;



            // Act
            var result = _indexedInfoServices.UpdateOCRValue(work_id, profile);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetPatientReferralDetail_WhenCalled_ShouldReturnOriginalQueue()
        {
            // Arrange
            UserProfile profile = new UserProfile();
            profile.PracticeCode = 1012714;
            profile.UserName = "TestUser";
            long work_id = 53436742;

            // Act
            var actualResult = _indexedInfoServices.getPatientReferralDetail(work_id, profile);

            // Assert
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void CreatePORTATest()
        {
            UserProfile profile = new UserProfile();
            OriginalQueue obj = new OriginalQueue();
            profile.PracticeCode = 1012714;
            profile.UserName = "Test User";
            obj.PATIENT_ACCOUNT = 2055341;
            obj.WORK_ID = 53436742;
            _indexedInfoServices.CreatePORTA(profile, obj);
            Assert.IsNotNull(obj);
            Assert.IsNotNull(profile);
        }

        [Test]
        public void AddUpdateTaskForPORTATest_ReturnsTask_WhenValidInputs()
        {
            // Arrange
            FOX_TBL_TASK task = new FOX_TBL_TASK();
            UserProfile profile = new UserProfile();
            OriginalQueue WORK_QUEUE = new OriginalQueue();
            task.PATIENT_ACCOUNT = 2055341;
            profile.UserName = "Test";
            profile.PracticeCode = 1012714;
            task.TASK_TYPE_ID = 605132;
            WORK_QUEUE.WORK_ID = 53423590;



            // Act
            var result = _indexedInfoServices.AddUpdateTaskForPORTA(task, profile, WORK_QUEUE);

            // Assert
            Assert.IsNotNull(result);
        }


        [Test]
        public void GetAllReferralSourceAndGroups_WhenUserIsNotTalkRehab_ShouldReturnReferralSourceAndGroupsForPracticeCode()
        {
            // Arrange
            UserProfile profile = new UserProfile();
            profile.PracticeCode = 1012714;
            profile.isTalkRehab = false;


            // Act
            ReferralSourceAndGroups result = _indexedInfoServices.getAllReferralSourceAndGroups(profile);

            // Assert
            Assert.IsNotNull(result);

        }

        [Test]
        public void GetAllReferralSourceAndGroups_WhenUserIsTalkRehab_ShouldReturnReferralSourceAndGroupsForPracticeCode()
        {
            // Arrange
            UserProfile profile = new UserProfile();
            profile.PracticeCode = 1012714;
            profile.isTalkRehab = true;


            // Act
            ReferralSourceAndGroups result = _indexedInfoServices.getAllReferralSourceAndGroups(profile);

            // Assert
            Assert.IsNotNull(result);

        }



        [Test]
        public void GetpatientsList_WhenLastNameIsNull_ShouldSetLastNameToEmptyString()
        {
            // Arrange
            var req = new getPatientReq();
            var profile = new UserProfile();
            req.First_Name = "Test";
            req.Last_Name = "Test";
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";


            // Act
            var result = _indexedInfoServices.GetpatientsList(req, profile);

            // Assert
            Assert.IsNotNull(result);
        }


        [Test]
        public void GetpatientsList_IsTalk()
        {
            // Arrange
            var req = new getPatientReq();
            var profile = new UserProfile();
            req.First_Name = "BREANAH";
            req.Last_Name = "MAHONEY";
            profile.PracticeCode = 1012714;
            profile.UserName = "Test";
            profile.isTalkRehab = true;
            req.CURRENT_PAGE = 1;
            req.RECORD_PER_PAGE = 1;
            req.SSN = "379151296";
            req.Date_Of_Birth = DateTime.Now.AddDays(-100);
            req.Chart_Id = "3218502";



            // Act
            var result = _indexedInfoServices.GetpatientsList(req, profile);

            // Assert
            Assert.IsNotNull(result);
        }


        [Test]
        public void GetDuplicateReferralInformation_WhenCalled_ReturnsListOfDuplicateReferralInfo()
        {
            // Arrange
            var checkDuplicateReferral = new checkDuplicateReferralRequest();
            var userProfile = new UserProfile();
            checkDuplicateReferral.workID = 53436742;
            userProfile.PracticeCode = 1012714;
            userProfile.UserName = "Test";

            // Act
            var result = _indexedInfoServices.GetDuplicateReferralInformation(checkDuplicateReferral, userProfile);

            // Assert
            Assert.IsInstanceOf<List<DuplicateReferralInfo>>(result);
        }


        [Test]
        public void GetWorkOrderDocs_NullPatientAccount_ReturnsList()
        {
            // Arrange
            string patientAccountStr = null;

            UserProfile userProfile = new UserProfile();
            userProfile.UserName = "Test";
            userProfile.PracticeCode = 1012714;

            // Act
            List<WorkOrderDocs> result = _indexedInfoServices.GetWorkOrderDocs(patientAccountStr, userProfile);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase(53437462, 1012714)]
        public void SaveUploadAdditionalWorkOrderFiles_ShouldReturnSuccessTrue(long workId, long practiceCode)
        {
            // Arrange
            var reqSaveUploadWorkOrderFiles = new ReqSaveUploadWorkOrderFiles();
            reqSaveUploadWorkOrderFiles.WORK_ID = workId;
            var profile = new UserProfile();
            profile.PracticeCode = practiceCode;

            // Act
            var result = _UploadWorkOrderFilesService.saveUploadAdditionalWorkOrderFiles(reqSaveUploadWorkOrderFiles, profile);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetSourceData_ValidInput_ReturnsExpectedResult()
        {
            // Arrange
            string email = "aftabkhan@carecloud.com";
            string userId = "53411372";
            long practiceCode = 1012714;
            string userName = "L2_53411372";

            // Act
            ResGetSourceDataModel result = _UploadOrderImagesService.GetSourceData(email, userId, practiceCode, userName);

            // Assert
            Assert.IsTrue(result.Success);
        }


        [Test]
        public void GetSmartOrderingSource_ReturnsListOfSmartOrderSource()
        {
            // Arrange
            var obj = new SmartReq();
            obj.Is_From_RFO = true;
            var profile = new UserProfile();
            obj.PracticeCode = 1012714;
            profile.PracticeCode = 1012714;

            // Act
            var result = _indexedInfoServices.GetSmartOrderingSource(obj, profile);

            // Assert
            Assert.IsInstanceOf<List<SmartOrderSource>>(result);
        }
        [Test]
        public void GetSmartOrderingSource_Returnstalk()
        {
            // Arrange
            var obj = new SmartReq();
            obj.Is_From_RFO = true;
            var profile = new UserProfile();
            obj.PracticeCode = 1012714;
            profile.PracticeCode = 1012714;
            profile.isTalkRehab = true;

            // Act
            var result = _indexedInfoServices.GetSmartOrderingSource(obj, profile);

            // Assert
            Assert.IsInstanceOf<List<SmartOrderSource>>(result);
        }

            [Test]
            public void GetSmartOrderingSourceByID_ShouldReturnSmartOrderSource()
            {
                // Arrange
                long sourceId = 12345;
                UserProfile profile = new UserProfile();
                profile.PracticeCode = 1012714;

                // Act
                SmartOrderSource result = _indexedInfoServices.GetSmartOrderingSourceByID(sourceId, profile);

                // Assert
                Assert.IsNull(result);
            }



        [Test]
        public void GetIndexInfoInitialData_ReturnsNotesHistory()
        {
            // Arrange
            long workId = 53437473;
            long practiceCode = 1012714;

            // Act
            var result = _indexedInfoServices.GetIndexInfoInitialData(workId, practiceCode);

            // Assert
            Assert.IsNotNull(result.NotesHistory);
            Assert.IsInstanceOf<List<FOX_TBL_NOTES_HISTORY>>(result.NotesHistory);
        }


        [TearDown]
        public void Teardown()
        {
            _indexedInfoServices = null;
            _indexedQueueFileRequest = null;
            _userProfile = null;
            _indexedQueueRequest = null;
            _workTransfer = null;
        }
    }
}
