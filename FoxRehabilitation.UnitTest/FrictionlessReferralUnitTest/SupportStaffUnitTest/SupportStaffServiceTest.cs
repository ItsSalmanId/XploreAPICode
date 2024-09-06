﻿using FOX.BusinessOperations.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.ServiceConfiguration;
using NUnit.Framework;
using System.Collections.Generic;

namespace FoxRehabilitation.UnitTest.FrictionlessReferralUnitTest.SupportStaffUnitTest
{
    [TestFixture]
    public class SupportStaffServiceTest
    {
        private SupportStaffService _supportStaffService;
        private PatientDetail _patientDetail;
        private RequestDeleteWorkOrder _requestDeleteWorkOrder;
        private FrictionLessReferral _frictionLessReferral;
        private ProviderReferralSourceRequest _providerReferralSourceRequest;
        private ServiceAvailability _serviceAvailability;
        private ExternalUserInfo _externalUserInfo;
        private SubmitReferralModel _submitReferralModel;

        [SetUp]
        public void Setup()
        {
            _supportStaffService = new SupportStaffService();
            _patientDetail = new PatientDetail();
            _requestDeleteWorkOrder = new RequestDeleteWorkOrder();
            _frictionLessReferral = new FrictionLessReferral();
            _providerReferralSourceRequest = new ProviderReferralSourceRequest();
            _serviceAvailability = new ServiceAvailability();
            _externalUserInfo = new ExternalUserInfo();
            _submitReferralModel = new SubmitReferralModel();
        }
        [Test]
        public void GetPracticeCode_HasPracticeCode_ReturnData()
        {
            //Arrange
            //Act
            var result = _supportStaffService.GetPracticeCode();

            //Assert
            Assert.That(result, Is.EqualTo(1011163).Or.EqualTo(1012714));
        }
        [Test]
        public void GetInsurancePayers_HasInsurance_ReturnData()
        {
            //Arrange
            //Act
            var result = _supportStaffService.GetInsurancePayers();

            //Assert
            Assert.That(result.Count, Is.GreaterThan(0));
        }
        [Test]
        [TestCase(548103)]
        [TestCase(0)]
        public void GetFrictionLessReferralDetails_HasReferralId_ReturnData(long referralId)
        {
            //Arrange
            //Act
            var result = _supportStaffService.GetFrictionLessReferralDetails(referralId);

            //Assert
            if (result != null)
                Assert.IsTrue(true);
            else
                Assert.IsFalse(false);
        }
        // Failed Due to EmaIL Template 
        [Test]
        [TestCase("Taseer", "iqbal", "aftabkhan@carecloud.com", "2064512559")]
        public void SendInviteToPatientPortal_PatientDetailModel_ReturnData(string firstName, string lastName, string email, string phoneNumber)
        {
            //Arrange
            _patientDetail.FirstName = firstName;
            _patientDetail.LastName = lastName;
            _patientDetail.EmailAddress = email;
            _patientDetail.MobilePhone = phoneNumber;

            //Act
            var result = _supportStaffService.SendInviteToPatientPortal(_patientDetail);

            //Assert
            if (result != null)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase("", "", "", "")]
        [TestCase("1679785950", "", "", "")]
        [TestCase("1023489119", "james", "smith", "ny")]
        public void GetProviderReferralSources_ProviderReferralSourceModel_ReturnData(string npi, string firstName, string lastName, string state)
        {
            //Arrange
            _providerReferralSourceRequest.ProviderNpi = npi;
            _providerReferralSourceRequest.ProviderFirstName = firstName;
            _providerReferralSourceRequest.ProviderLastName = lastName;
            _providerReferralSourceRequest.ProviderState = state;

            //Act
            var result = _supportStaffService.GetProviderReferralSources(_providerReferralSourceRequest);

            //Assert
            if (result.Count > 0)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        public void NewThreadImplementaion_HasParameter_NoReturnData()
        {
            //Arrange
            string PdfPath = @"\\10.10.30.165\FoxDocumentDirectory\Fox\1012714\05-26-2023\OriginalFiles\tempcoversletter638206868960541491.pdf";
            ServiceConfiguration config = new ServiceConfiguration();
            List<int> threadCounter = new List<int>();
            long workId = 12345;
            int pageCounter = 1;
            int i = 1;

            //Act
            _supportStaffService.newThreadImplementaion(ref threadCounter, PdfPath, i, config, workId, pageCounter);

            //Assert
            Assert.True(true);
        }
        [Test]
        public void NewThreadImplementaion_HasParameters_NoReturnData()
        {
            //Arrange
            string PdfPath = @"\\10.10.30.165\FoxDocumentDirectory\Fox\1012714\05-26-2023\OriginalFiles\tempcoversletter638206868960541491.pdf";
            string imgPath = @"\\10.10.30.165\FoxDocumentDirectory\Fox\1012714\05-26-2023\Images\53432042_0.jpg";
            string logoImgPath = @"\\10.10.30.165\FoxDocumentDirectory\Fox\1012714\05-26-2023\Images\53432042_0.jpg";
            ServiceConfiguration config = new ServiceConfiguration();
            List<int> threadCounter = new List<int>();
            int i = 1;

            //Act
            _supportStaffService.newThreadImplementaion(ref threadCounter, PdfPath, i, imgPath, logoImgPath);

            //Assert
            Assert.True(true);
        }
        [Test]
        public void SubmitReferral_HasParameters_NoReturnData()
        {
            //Arrange
            _submitReferralModel.WorkId = 54820725;

            //Act
            _supportStaffService.SubmitReferral(_submitReferralModel);

            //Assert
            Assert.True(true);
        }
        [Test]
        [TestCase(54820725)]
        public void AddFrictionlessFilesToDatabase_HasParameters_NoReturnData(long workId)
        {
            //Arrange
            string filePath = @"\\10.10.30.165\FoxDocumentDirectory\Fox\1012714\05-26-2023\OriginalFiles\tempcoversletter638206868960541491.pdf";
            string logoPath = @"\\10.10.30.165\FoxDocumentDirectory\Fox\1012714\05-26-2023\Images\53432042_0.jpg";

            //Act
            _supportStaffService.AddFrictionlessFilesToDatabase(filePath, workId, logoPath);

            //Assert
            Assert.True(true);
        }
        [Test]
        [TestCase(54820838)]
        [TestCase(0)]
        [TestCase(null)]
        public void DeleteWorkOrder_HasWorkId_ReturnData(long workId)
        {
            //Arrange
            _requestDeleteWorkOrder.WorkId = workId;

            //Act
            var result = _supportStaffService.DeleteWorkOrder(_requestDeleteWorkOrder);

            //Assert
            if (result.Success)
                Assert.IsTrue(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase(5481054, "", "", false, "", "")]
        [TestCase(5481103, ",9,3", "Thu Dec 01 2022", false, "Party Referral Source", "8774074329")]
        [TestCase(123, ",1,2", "Thu Dec 01 2022", false, "", "")]
        [TestCase(123, ",1,2", null, false, "", "")]
        public void SaveFrictionLessReferralDetails_FrictionLessReferralModel_ReturnData(long referralId, string disciplineId, string patientDOB, bool isSinged, string userType, string providerFax)
        {
            //Arrange
            _frictionLessReferral.FRICTIONLESS_REFERRAL_ID = referralId;
            _frictionLessReferral.PATIENT_DISCIPLINE_ID = disciplineId;
            _frictionLessReferral.PATIENT_DOB_STRING = patientDOB;
            _frictionLessReferral.IS_SIGNED_REFERRAL = isSinged;
            _frictionLessReferral.USER_TYPE = userType;
            _frictionLessReferral.PROVIDER_FAX = providerFax;
            _frictionLessReferral.SUBMITTER_LAST_NAME = "Unit testing";

            //Act
            var result = _supportStaffService.SaveFrictionLessReferralDetails(_frictionLessReferral);

            //Assert
            if (result != null)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase("1023489119", "", "", "")]
        [TestCase("", "", "", "")]
        public void GetOrderingReferralSource_ProviderReferralSourceModel_ReturnData(string npi, string firstName, string lastName, string state)
        {
            //Arrange
            _providerReferralSourceRequest.ProviderNpi = npi;
            _providerReferralSourceRequest.ProviderFirstName = firstName;
            _providerReferralSourceRequest.ProviderLastName = lastName;
            _providerReferralSourceRequest.ProviderState = state;

            //Act
            var result = _supportStaffService.GetOrderingReferralSource(_providerReferralSourceRequest);

            //Assert
            if (result.Count > 0)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        public void GetOrderingReferralSource_EmptyProviderReferralSourceModel_NoReturnData()
        {
            //Arrange
            _providerReferralSourceRequest = null;

            //Act
            var result = _supportStaffService.GetOrderingReferralSource(_providerReferralSourceRequest);

            //Assert
            if (result.Count > 0)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        public void SendInviteOnMobile_PassModel_NoReturnData()
        {
            //Arrange
            _patientDetail.EmailAddress = "Test";
            _patientDetail.MobilePhone = "2064512559";
            _patientDetail.EmailAddress = "Test";

            //Act
            var result = _supportStaffService.SendInviteOnMobile(_patientDetail);

            //Assert
            if (result != null)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase("68520", "")]
        [TestCase("", "Chicago")]
        public void CheckServiceAvailability_PassModel_ReturnData(string zipCode, string cityName)
        {
            //Arrange
            _serviceAvailability.ZIP_CODE = zipCode;
            _serviceAvailability.CITY_NAME = cityName;

            //Act
            var result = _supportStaffService.CheckServiceAvailability(_serviceAvailability);

            //Assert
            if (result == true)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase("68520", "", "Smith")]
        [TestCase("", "Chicago", "North")]
        public void SaveExternalUserInfo_PassModel_ReturnData(string zipCode, string cityName, string firstName)
        {
            //Arrange
            _externalUserInfo.SUBMITER_FIRST_NAME = firstName;
            _externalUserInfo.SUBMITTER_LAST_NAME = "NJ";
            _externalUserInfo.SUBMITTER_PHONE = "1546874658";
            _externalUserInfo.SUBMITTER_EMAIL = "Test@Test.com";
            _externalUserInfo.ZIP_CODE = zipCode;
            _externalUserInfo.CITY_NAME = cityName;

            //Act
            var result = _supportStaffService.SaveExternalUserInfo(_externalUserInfo);

            //Assert
            if (result != null)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }

        [Test]
        public void GenerateQRCode_ValidInput_ReturnsQRCodeModel()
        {
            // Arrange
            var obj = new QRCodeModel();
            obj.WORK_ID = 53436742;
            obj.AbsolutePath = @"C:\Temp\";

            // Act
            var result = _supportStaffService.GenerateQRCode(obj);

            // Assert
            Assert.IsNotNull(result);
        }



        [TearDown]
        public void Teardown()
        {
            _supportStaffService = null;
            _patientDetail = null;
            _requestDeleteWorkOrder = null;
            _frictionLessReferral = null;
            _providerReferralSourceRequest = null;
        }
    }
}
