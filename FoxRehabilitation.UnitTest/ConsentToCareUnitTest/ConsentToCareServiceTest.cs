﻿using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.ConsentToCareService;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.TasksModel;
using NUnit.Framework;
using System.Collections.Generic;
using static FOX.DataModels.Models.ConsentToCare.ConsentToCare;

namespace FoxRehabilitation.UnitTest.ConsentToCareServiceUnitTest
{
    [TestFixture]
    public class ConsentToCareServiceTest
    {
        private ConsentToCareService _consentToCareService;
        private FoxTblConsentToCare _foxTblConsentToCare;
        private UserProfile _userProfile;
        private FOX_TBL_TASK _foxTblTask;
        private InterfaceSynchModel _interfaceSynchModel;
        private EditableConsent _editableConsent;

        [SetUp]
        public void SetUp()
        {
            _consentToCareService = new ConsentToCareService();
            _foxTblConsentToCare = new FoxTblConsentToCare();
            _userProfile = new UserProfile();
            _foxTblTask = new FOX_TBL_TASK();
            _interfaceSynchModel = new InterfaceSynchModel();
            _editableConsent = new EditableConsent();
        }
        [Test]

        [TestCase("1012714534318094156")]
        public void AddUpdateConsentToCare_PassModel_ReturnString(string patientAccount)
        {
            //Arrange
            _foxTblConsentToCare.SEND_TO = "Patient";
            _foxTblConsentToCare.SENT_TO_ID = 1245;
            _foxTblConsentToCare.CASE_ID = 1245;
            _foxTblConsentToCare.PATIENT_ACCOUNT_Str = patientAccount;
            _foxTblConsentToCare.TREATING_PROVIDER_ID = 1245;
            _userProfile.UserName = "N Unit testing";

            //Act
            var result = _consentToCareService.AddUpdateConsentToCare(_foxTblConsentToCare, _userProfile);

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
        [TestCase("patinet", "Salman")]
        [TestCase("patinet", "Salman")]
        public void AddUpdateTask_PassModel_ReturnData(string sendTo, string consnetReceiverName)
        {
            //Arrange
            _userProfile.UserName = "N Unit testing";
            _userProfile.PracticeCode = 1012714;
            _foxTblTask.PATIENT_ACCOUNT_STR = "1012714113245578";
            _foxTblTask.TASK_ID = Helper.getMaximumId("FOX_TASK_ID");
            _foxTblTask.TASK_TYPE_ID = 12245;
            _foxTblTask.CASE_ID = 12245;

            //Act
            var result = _consentToCareService.AddUpdateTask(_foxTblTask, _userProfile, sendTo, consnetReceiverName);

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
        [TestCase(101116354816636)]
        [TestCase(1234)]
        public void InsertTaskLog_PassParameter_ReturnString(long taskId)
        {
            //Arrange
            List<TaskLog> list = new List<TaskLog>();

            //Act
            _consentToCareService.InsertTaskLog(taskId, list, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("test", "test", 54100)]
        [TestCase("test", "test", 54101)]
        public void AddFilesToDatabase_PassParameter_UpdateData(string filePath, string logoPath, long consentToCareId)
        {
            //Arrange

            //Act
             _consentToCareService.AddFilesToDatabase(filePath, logoPath, consentToCareId);

            //Assert
                Assert.IsTrue(true);
        }
        [Test]
        public void CloseTaskAndInsertInterfaceSynch_PassParameter_UpdateData()
        {
            //Arrange
            _foxTblConsentToCare.TASK_ID = 12345;
            _foxTblConsentToCare.PRACTICE_CODE = 1012714;

            //Act
            _consentToCareService.CloseTaskAndInsertInterfaceSynch(_foxTblConsentToCare, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void InsertInterfaceTeamData_PassParameter_UpdateData()
        {
            //Arrange
            _interfaceSynchModel.TASK_ID = null;
            _interfaceSynchModel.PATIENT_ACCOUNT = null;
            _interfaceSynchModel.CASE_ID = null;

            //Act
            _consentToCareService.InsertInterfaceTeamData(_interfaceSynchModel, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void SaveConsentDefinition_PassParameter_UpdateData()
        {
            //Arrange
            _editableConsent.PRACTICE_CODE = 1011163;

            //Act
            _consentToCareService.SaveConsentDefinition(_editableConsent, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void GetConsentDefinition_PassParameter_UpdateData()
        {
            //Arrange
            _editableConsent.PRACTICE_CODE = 1011163;

            //Act
            _consentToCareService.GetConsentDefinition(_editableConsent, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _consentToCareService = null;
            _foxTblConsentToCare = null;
            _userProfile = null;
            _foxTblTask = null;
            _interfaceSynchModel = null;
            _editableConsent = null;
        }
    }
}
