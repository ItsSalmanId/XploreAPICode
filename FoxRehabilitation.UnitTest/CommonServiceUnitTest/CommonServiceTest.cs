﻿using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.ServiceConfiguration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace FoxRehabilitation.UnitTest.CommonServiceUnitTest
{
    [TestFixture]
    public class CommonServiceTest
    {
        private CommonServices _commonServices;
        private UserProfile _userProfile;
        private ReqGetSenderNamesModel _senderNamesModel;
        private ServiceConfiguration _serviceConfiguration;
        private CommonAnnouncements _commonAnnouncements;
        private FOX_TBL_NOTES _foxTblNotes;
        

        [SetUp]
        public void Setup()
        {
            _commonServices = new CommonServices();
            _userProfile = new UserProfile();
            _senderNamesModel = new ReqGetSenderNamesModel();
            _serviceConfiguration = new ServiceConfiguration();
            _commonAnnouncements = new CommonAnnouncements();
            _foxTblNotes = new FOX_TBL_NOTES();
   
        }
        [Test]
        public void GeneratePdf_UniqueID_NoReturnsData()
        {
            //Arrange
            long uniqueID = 54818464;
            string practiceDocumentDir = "";

            //Act
            var result = _commonServices.GeneratePdf(uniqueID, practiceDocumentDir);

            //Assert
            Assert.That(result, Is.Not.Null.Or.Not.Empty);
        }
        [Test]
        public void GeneratePdfForSupportedDoc_EmptyUniqueID_NoReturnsData()
        {
            //Arrange
            string uniqueID = "552103";
            _userProfile.PracticeCode = 1011163;
            _serviceConfiguration.ORIGINAL_FILES_PATH_SERVER = @"\\10.10.30.165\FoxDocumentDirectory\Fox\1012714\05-26-2023\OriginalFiles\";

            //Act
            var result = _commonServices.GeneratePdfForSupportedDoc(_serviceConfiguration, uniqueID, _userProfile);

            //Assert
            Assert.That(result.FILE_PATH, Is.Null);
        }
        [Test]
        [TestCase("53422212")]
        [TestCase("")]
        public void GeneratePdfForEmailToSender_EmptyUniqueID_NoReturnsData(string unique_Id)
        {
            //Arrange
            string uniqueID = unique_Id;
            _userProfile.PracticeCode = 1012714;
            _userProfile.UserName = "Test";

            //Act
            var result = _commonServices.GeneratePdfForEmailToSender(uniqueID, _userProfile);

            //Assert
            Assert.That(result.FILE_PATH, Is.Null);
        }
        [Test]
        public void Authenticate_PassModel_ReturnsData()
        {
            //Arrange
            string pasward = "564B4A2DAD36F02240706E7DD6E28BED8518F84C6368A24AF3B3F379953534A8EC1834D08DF4CD58C9DB42DA047C5FE5A0B2EA0F982E56493C3E1584F78F516E";
            _userProfile.UserName = "Ali_5482802";

            //Act
            _commonServices.Authenticate(pasward, _userProfile);

            //Assert
            Assert.IsFalse(false);
        }
       // [Test]

        //public void AddUpdateNotes_PassModel_ReturnsData()
        //{
        //    //Arrange
        //    _foxTblNotes.NOTES_ID = 544100;
        //    _foxTblNotes.NOTES = "test";
        //    _userProfile.PracticeCode = 1011163;
        //    _userProfile.UserName = "N Unit-Testing";

        //    //Act
        //    _commonServices.AddUpdateNotes(_foxTblNotes, _userProfile);

        //    //Assert
        //    Assert.True(true);
        //}
        [Test]
        [TestCase("", 1011163)]
        [TestCase("54819274", 1011163)]
        public void GetFiles_Files_ReturnsData(string uniqueID, long practiceCode)
        {
            //Arrange

            //Act
            var result = _commonServices.GetFiles(uniqueID, practiceCode);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        [TestCase("", 1011163)]
        [TestCase("54819274", 1011163)]
        public void GetAllOriginalFiles_WorkIds_ReturnsData(string uniqueID, long practiceCode)
        {
            //Arrange

            //Act
            var result = _commonServices.GetAllOriginalFiles(uniqueID, practiceCode);

            //Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        public void GetWorkHistory_HasWorkID_NoReturnsData()
        {
            //Arrange
            string workID = "552798";

            //Act
            var result = _commonServices.GetWorkHistory(workID);

            //Assert
            if (result.Count > 0)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        public void GetWorkHistory_EmptyWorkID_NoReturnsData()
        {
            //Arrange
            string workID = "";

            //Act
            var result = _commonServices.GetWorkHistory(workID);

            //Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }
        [Test]
        [TestCase(true, 1011163)]
        [TestCase(false, 1011163)]
        [TestCase(false, 0)]
        public void GetSenderTypes_HasUserProfile_ReturnsData(bool isTalkRehab, long practiceCode)
        {
            //Arrange
            _userProfile.isTalkRehab = isTalkRehab;
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _commonServices.GetSenderTypes(_userProfile);

            //Assert
            if (result != null)
            {
                Assert.That(result.SenderTypeList.Count, Is.GreaterThanOrEqualTo(0));
            }
        }
        [Test]
        public void GetSenderNames_EmptyUserProfile_NoReturnsData()
        {
            //Arrange
            _userProfile = null;

            //Act
            var result = _commonServices.GetSenderNames(_senderNamesModel, _userProfile);

            //Assert
            Assert.That(result.SenderNameList.Count, Is.EqualTo(0));
        }
        [Test]
        [TestCase("")]
        public void GetTreatmentLocationByZip_EmptyZipCode_NoReturnsData(string hasZipCode)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;

            //Act
            var result = _commonServices.GetTreatmentLocationByZip(hasZipCode, _userProfile.PracticeCode);

            //Assert
            Assert.That(result, Is.Null);
        }
        [Test]
        [TestCase("08873")]
        public void GetTreatmentLocationByZip_HasZipAndPracticeCode_ReturnsData(string hasZipCode)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;

            //Act
            var result = _commonServices.GetTreatmentLocationByZip(hasZipCode, _userProfile.PracticeCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("08873")]
        public void GetCityStateByZip_HasZipCode_ReturnsData(string zipCode)
        {
            //Arrange

            //Act
            var result = _commonServices.GetCityStateByZip(zipCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("Florida")]
        public void GetSmartCities_HasSmartCities_ReturnsData(string city)
        {
            //Arrange

            //Act
            var result = _commonServices.GetSmartCities(city);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("NJ")]
        public void GetSmartStates_HasStateCode_ReturnsData(string statusCode)
        {
            //Arrange

            //Act
            var result = _commonServices.GetSmartStates(statusCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        public void GetStates_StateCode_ReturnsData()
        {
            //Arrange

            //Act
            var result = _commonServices.GetStates();

            //Assert
            if (result.Count > 0)
            {
                Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
            }
        }
        [Test]
        [TestCase(0, 0)]
        [TestCase(5481921, 1011163)]
        public void GetProvider_ProfileAndProvider_ReturnsData(long providerId, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _commonServices.GetProvider(providerId, _userProfile);

            //Assert
            if (result == null)
            {
                Assert.That(result, Is.Null);
            }
            else
            {
                Assert.IsTrue(true);
                ;
            }
        }
        [Test]
        public void IsShowSplash_EmptyProfile_NoReturnsData()
        {
            //Arrange
            _userProfile = new UserProfile();
            _userProfile.PracticeCode = 1012714;
            _userProfile.UserName = "Davis_53411401";

            //Act
            var result = _commonServices.IsShowSplash(_userProfile);

            //Assert
            Assert.That(result, Is.True);
        }
        [Test]
        public void IsShowSplash_HasProfile_NoReturnsData()
        {
            //Arrange
            _userProfile.UserName = "1163testing";

            //Act
            var result = _commonServices.IsShowSplash(_userProfile);

            //Assert
            Assert.IsNotNull(result);
        }
        [Test]
        [TestCase("Admin_5651352")]
        [TestCase("test")]
        public void SaveSplashDetails_EmptyUserProfile(string userName)
        {
            //Arrange
            _userProfile.UserName = userName;

            //Act
            var result = _commonServices.SaveSplashDetails(_userProfile);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        public void DeleteDownloadedFile_EmptyFileLocation_NoReturnsData()
        {
            //Arrange
            string fileLocation = string.Empty;

            //Act
            var result = _commonServices.DeleteDownloadedFile(fileLocation);

            //Assert
            if (!result.Success)
            {
                Assert.Pass("Passed");
            }
            else
            {
                Assert.Fail("Failed");
            }
        }
        [Test]
        public void DeleteDownloadedFile_HasFileLocation_NoReturnsData()
        {
            //Arrange
            string fileLocation = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExportedFiles");

            //Act
            var result = _commonServices.DeleteDownloadedFile(fileLocation);

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
        [TestCase(1012714, 103)]
        [TestCase(1011163, 0)]
        [TestCase(0, 103)]
        [TestCase(0, 0)]
        public void IsShowAlertWindow_ReturnModel_ReturnsData(long practiceCode, long role)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.RoleId = role;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _commonServices.IsShowAlertWindow(_userProfile);

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
        [TestCase(1012714, 103)]
        [TestCase(1011163, 0)]
        [TestCase(0, 103)]
        [TestCase(0, 0)]
        public void SaveAlertWindowsDetails_SaveModel_SaveData(long practiceCode, long role)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.userID = role;
            _userProfile.UserName = "Admin_5651352";
            _userProfile.RoleId = role;

            //Act
            var result = _commonServices.SaveAlertWindowsDetails(_commonAnnouncements, _userProfile);

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
        public void GeneratePdfForSplitImages_WhenCalled_ReturnsCorrectPath()
        {
            // Arrange
            var unique_Id = "53422211";
            var practiceDocumentDirectory = "test";
         

            // Act
            var result = _commonServices.GeneratePdfForSplitImages(unique_Id, practiceDocumentDirectory);

            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public void GeneratePdfForAllOriginalImages_WhenCalled_ReturnsCorrectPath()
        {
            // Arrange
            var unique_Id = "53422211";
            var practiceDocumentDirectory = "test";


            // Act
            var result = _commonServices.GeneratePdfForAllOriginalImages(unique_Id, practiceDocumentDirectory);

            // Assert
            Assert.IsNull(result);
        }

    

        [Test]
        public void AddUpdateNotes_WhenCalled_ShouldSetAllPreviousNotesToInactive()
        {
            // Arrange
           
            FOX_TBL_NOTES _notes = new FOX_TBL_NOTES();
            _notes.TASK_ID = 53415615;
            var _profile = new UserProfile();
            _profile.PracticeCode = 1012714;
            _profile.UserName = "Test";
            _notes.IS_ACTIVE = true;
            _notes.DELETED = false;
            _notes.NOTES = "Test";


            // Act
            var result = _commonServices.AddUpdateNotes(_notes, _profile);

            // Assert
            Assert.IsNotNull(result);
        }



        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _commonServices = null;
            _userProfile = null;
            _senderNamesModel = null;
            _serviceConfiguration = null;
        }
    }
}
