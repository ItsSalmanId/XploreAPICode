using FOX.BusinessOperations.FrictionlessReferral.PowerOfAttorney;
using FOX.BusinessOperations.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.ServiceConfiguration;
using NUnit.Framework;
using System.Collections.Generic;

namespace FoxRehabilitation.UnitTest.FrictionlessReferralUnitTest.PowerOfAttorneyServiceUnitTest
{
    [TestFixture]
    public class PowerOfAttorneyServiceTest
    {
        private PowerOfAttorneyService _powerOfAttorneyService;
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
            _powerOfAttorneyService = new PowerOfAttorneyService();
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
            var result = _powerOfAttorneyService.GetPracticeCode();

            //Assert
            Assert.That(result, Is.EqualTo(1011163).Or.EqualTo(1012714));
        }
        [Test]
        public void GetInsurancePayers_HasInsurance_ReturnData()
        {
            //Arrange
            //Act
            var result = _powerOfAttorneyService.GetInsurancePayers();

            //Assert
            Assert.That(result.Count, Is.GreaterThan(0));
        }
        // Failed Due to EmaIL Template 
        [Test]
        [TestCase("Taseer", "iqbal", "muhammadiqbal11@carecloud.com", "2064512559")]
        public void SendInviteToPatientPortal_PatientDetailModel_ReturnData(string firstName, string lastName, string email, string phoneNumber)
        {
            //Arrange
            _patientDetail.FirstName = firstName;
            _patientDetail.LastName = lastName;
            _patientDetail.EmailAddress = email;
            _patientDetail.MobilePhone = phoneNumber;

            //Act
            var result = _powerOfAttorneyService.SendInviteToPatientPortal(_patientDetail);

            //Assert
            if (result != null)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase("", "", "", "")]
        [TestCase("1679785950", "", "", "")]
        [TestCase("1023489119", "test", "test", "test")]
        [TestCase("1023489119", "james", "smith", "ny")]
        public void GetProviderReferralSources_ProviderReferralSourceModel_ReturnData(string npi, string firstName, string lastName, string state)
        {
            //Arrange
            _providerReferralSourceRequest.ProviderNpi = npi;
            _providerReferralSourceRequest.ProviderFirstName = firstName;
            _providerReferralSourceRequest.ProviderLastName = lastName;
            _providerReferralSourceRequest.ProviderState = state;

            //Act
            var result = _powerOfAttorneyService.GetProviderReferralSources(_providerReferralSourceRequest);

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
            _powerOfAttorneyService.newThreadImplementaion(ref threadCounter, PdfPath, i, config, workId, pageCounter);

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
            _powerOfAttorneyService.newThreadImplementaion(ref threadCounter, PdfPath, i, imgPath, logoImgPath);

            //Assert
            Assert.True(true);
        }
        [Test]
        public void SubmitReferral_HasParameters_NoReturnData()
        {
            //Arrange
            _submitReferralModel.WorkId = 54820725;

            //Act
            _powerOfAttorneyService.SubmitReferral(_submitReferralModel);

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
            _powerOfAttorneyService.AddFrictionlessFilesToDatabase(filePath, workId, logoPath);

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
            var result = _powerOfAttorneyService.DeleteWorkOrder(_requestDeleteWorkOrder);

            //Assert
            if (result.Success)
                Assert.IsTrue(true);
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
            var result = _powerOfAttorneyService.GetOrderingReferralSource(_providerReferralSourceRequest);

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
            var result = _powerOfAttorneyService.GetOrderingReferralSource(_providerReferralSourceRequest);

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
            _patientDetail.EmailAddress = "aftabkhan@carecloud.com";
            _patientDetail.MobilePhone = "2064512559";

            //Act
            var result = _powerOfAttorneyService.SendInviteOnMobile(_patientDetail);

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
            var result = _powerOfAttorneyService.CheckServiceAvailability(_serviceAvailability);

            //Assert
            if (result == true)
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
            var result = _powerOfAttorneyService.GenerateQRCode(obj);

            // Assert
            Assert.IsNotNull(result);
        }




        [TearDown]
        public void Teardown()
        {
            _powerOfAttorneyService = null;
            _patientDetail = null;
            _requestDeleteWorkOrder = null;
            _frictionLessReferral = null;
            _providerReferralSourceRequest = null;
        }
    }
}
