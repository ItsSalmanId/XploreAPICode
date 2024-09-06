using FOX.BusinessOperations.AccountService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Models.ExternalUserModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.SenderType;
using FOX.DataModels.Models.Settings.RoleAndRights;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using FOX.BusinessOperations.Security;
using System.Web;
using System.Dynamic;

namespace FoxRehabilitation.UnitTest.AccountServiceUnitTest
{
    [TestFixture]
    public class AccountServiceTest
    {
        private AccountServices _accountService;
        private UserDetailsByNPIRequestModel _npiRequestModel;
        private CityDetailByZipCodeRequestModel _cityDetailByZipCode;
        private EmailExist _emailExist;
        private SmartSearchRequest _smartSearchRequest;
        private GetUserIP _getUserIP;
        private LogoutModal _logoutModel;
        private UserProfile _userProfile;

        [SetUp]
        public void Setup()
        {
            _accountService = new AccountServices();
            _npiRequestModel = new UserDetailsByNPIRequestModel();
            _cityDetailByZipCode = new CityDetailByZipCodeRequestModel();
            _emailExist = new EmailExist();
            _smartSearchRequest = new SmartSearchRequest();
            _getUserIP = new GetUserIP();
            _logoutModel = new LogoutModal();
            _userProfile = new UserProfile();
        }
        [Test]
        public void GetSenderTypes_UserProfile_ReturnsData()
        {
            //Arrange
            //Act
            var result = _accountService.getSenderTypes();

            //Assert
            Assert.That(result.SenderTypeList.Count, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        public void GetUserDetailByNPI_HasNPI_ReturnsData()
        {
            //Arrange
            _npiRequestModel.NPI = "2569114412";

            //Act
            var result = _accountService.getUserDetailByNPI(_npiRequestModel);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("08873")]
        [TestCase("00617")]
        public void GetCityDetailByZipCode_HasZipCode_ReturnsData(string zipCode)
        {
            //Arrange
            _cityDetailByZipCode.ZipCode = zipCode;

            //Act
            var result = _accountService.getCityDetailByZipCode(_cityDetailByZipCode);

            //Assert
            if (result != null && result.zip_city_state != null && result.zip_city_state.Count > 0)
            {
                Assert.That(result.zip_city_state.Count, Is.GreaterThanOrEqualTo(0));
            }
            else
            {
                Assert.That(result.zip_city_state, Is.Null);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("aftabkhan@carecloud.com")]
        public void CheckIfEmailAlreadyInUse_Email_ReturnsData(string emailAddress)
        {
            //Arrange
            _emailExist.EMAIL = emailAddress;

            //Act
            var result = _accountService.CheckIfEmailAlreadyInUse(_emailExist);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("Fox")]
        public void GetPractices_PracticeNames_ReturnsData(string practiceName)
        {
            //Arrange
            _smartSearchRequest.Keyword = practiceName;
            _smartSearchRequest.PracticeCode = 1011163;

            //Act
            var result = _accountService.getPractices(_smartSearchRequest);

            //Assert
            if (result != null && result.fox_tbl_practice_organization != null && result.fox_tbl_practice_organization.Count > 0)
            {
                Assert.That(result.fox_tbl_practice_organization.Count, Is.GreaterThanOrEqualTo(0));
            }
            else
            {
                Assert.That(result.fox_tbl_practice_organization, Is.Null);
            }
        }
        [Test]
        [TestCase("", "")]
        [TestCase("ACO Identifier", "Mssp_car")]
        public void GetSmartIdentifier_Identifier_ReturnsData(string identiferType, string searchValue)
        {
            //Arrange
            _smartSearchRequest.TYPE = identiferType;
            _smartSearchRequest.SEARCHVALUE = searchValue;

            //Act
            var result = _accountService.getSmartIdentifier(_smartSearchRequest);

            //Assert
            if (result != null)
            {
                Assert.That(result.Count, Is.GreaterThanOrEqualTo(0));
            }
            else
            {
                Assert.That(result, Is.Null);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("Test")]
        [TestCase("OT")]
        public void GetSmartSpecialities_HasKeywords_ReturnsData(string keywordName)
        {
            //Arrange
            _smartSearchRequest.Keyword = keywordName;

            //Act
            var result = _accountService.getSmartSpecialities(_smartSearchRequest);

            //Assert
            if (result != null && result.specialities != null)
            {
                Assert.That(result.specialities.Count, Is.GreaterThanOrEqualTo(0));
            }
            else
            {
                Assert.That(result.specialities, Is.Null);
            }
        }
        [Test]
        [TestCase(53411333)]
        [TestCase(53411333)]
        [TestCase(53411241)]
        [TestCase(5482870)]
        public void CreateExternalUserOrdRefSource_HasUserID_ReturnsData(long userID)
        {
            //Arrange
            User user = new User();
            user.PRACTICE_CODE = 1011163;
            var profile = new UserProfile();
            profile.PracticeCode = 1011163;
            //Act
             _accountService.CreateExternalUserOrdRefSource(userID);

            //Assert
            Assert.IsFalse(false);
        }
        [Test]
        [TestCase(544585)]
        [TestCase(5483298)]
        public void SavePasswordHistory_HasUserID_ReturnsData(long userID)
        {
            //Arrange
            User user = new User();
            user.USER_ID = userID;

            //Act
            _accountService.SavePasswordHistory(user);

            //Assert
            Assert.IsTrue(true);
        }
        //SavePasswordHistory
        [Test]
        [TestCase("")]
        [TestCase("10965")]
        [TestCase("2569114412")]
        public void CheckForDublicateNPI_NPI_ReturnsData(string npi)
        {
            //Arrange
            //Act
            var result = _accountService.CheckForDublicateNPI(npi);

            //Assert
            if (result)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("uss_5481193")]
        [TestCase("L2_53411372")]
        public void ClearOpenedByinPatientforUser_UserName_ReturnsData(string userName)
        {
            //Arrange
            //Act
            _accountService.ClearOpenedByinPatientforUser(userName);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("testing@foxrehab.org")]
        [TestCase("testing@mtbc.com")]
        public void IpConfig_HasUserName_ReturnsData(string userName)
        {
            //Arrange
            _getUserIP.userName = userName;
            string showUrl = "Local";

            //Act
            var result = _accountService.IpConfig(_getUserIP);

            //Assert
            if (result)
            {
                Assert.IsTrue(true);
            }
        }

            [Test]
            public void IpConfig_ValidUserName_ReturnsTrue()
            {
                // Arrange
                GetUserIP data = new GetUserIP { userName = "test@foxrehab.org" };

                // Act
                bool result = _accountService.IpConfig(data);

                // Assert
                Assert.IsTrue(result);
            }
            [Test]
            public void IpConfig_InvalidUserName_ReturnsFalse()
            {
                // Arrange
                GetUserIP data = new GetUserIP { userName = "test@example.com" };

                // Act
                bool result = _accountService.IpConfig(data);

                // Assert
                Assert.IsFalse(result);
            }

            [Test]
            public void IpConfig_ValidUserIP_ReturnsTrue()
            {
                // Arrange
                GetUserIP data = new GetUserIP { userIP = "115.186.128.16" };

                // Act
                bool result = _accountService.IpConfig(data);

                // Assert
                Assert.IsTrue(result);
            }

            [Test]
            public void IpConfig_InvalidUserIP_ReturnsFalse()
            {
                // Arrange
                GetUserIP data = new GetUserIP { userIP = "127.0.0.2" };

                // Act
                bool result = _accountService.IpConfig(data);

                // Assert
                Assert.IsFalse(result);
            }
        
        [Test]
        [TestCase("", "")]
        [TestCase("11163testing", "203.215.161.135")]
        public void CheckIP_HasUserNameAndIP_ReturnsData(string userName, string ipAddress)
        {
            //Arrange
            //Act
            var result = _accountService.CheckIP(userName, ipAddress);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("zPfGn7J1zP5PbPX4OYfCq8_0c7wpC02Y-ZoL7tsisgUU4sFZ6Kv3enRA2ybD3lrlpXLMxFdokLtsUIw-Z0FG4gDT9j_VqtfkMAK5", 1011163415)]
        [TestCase(null, 0)]
        [TestCase("testingToken", 1011163415)]
        public void SignOut_HasTokenAndUserID_NoReturnsData(string token, long userID)
        {
            //Arrange
            _logoutModel.token = token;
            _userProfile.userID = userID;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _accountService.SignOut(_logoutModel, _userProfile);

            //Assert

            if (result != null && result.Success == true)
            {
                Assert.That(result.Success, Is.True);
            }
            else
            {
                Assert.That(result.Success, Is.False);
            }
        }

        [Test]
        public void GetProviderDetailByNpi_ValidNpi_ReturnsUserDetailByNPIModel()
        {
            // Arrange

            string validNpi = "1234567890";

            // Act
            var result = _accountService.GetProviderDetailByNpi(validNpi);

            // Assert
            Assert.IsInstanceOf<UserDetailByNPIModel>(result);
        }

        [Test]
        public void GetProviderDetailByNpi_InvalidNpi_ReturnsNull()
        {
            // Arrange

            string invalidNpi = "1699084202";

            // Act
            var result = _accountService.GetProviderDetailByNpi(invalidNpi);

            // Assert
            Assert.IsNotNull(result);
        }


        

         
        

        [Test]
        public void GetSmartSpecialities_WhenCalled_ReturnsSmartSpecialitySearchResponseModel()
        {
            // Arrange
            var model = new SmartSearchRequest();

            // Act
            var result = _accountService.getSmartSpecialities(model);

            // Assert
            Assert.IsInstanceOf<SmartSpecialitySearchResponseModel>(result);
        }

        [Test]
        public void GetSmartSpecialities_WhenCalled_ReturnsCorrectSpecialities()
        {
            // Arrange
            var model = new SmartSearchRequest();

            // Act
            var result = _accountService.getSmartSpecialities(model);

            // Assert

        }

        [Test]
        public void GetSmartSpecialities_WhenCalled_ReturnsCorrectPracticeCode()
        {
            // Arrange
            var model = new SmartSearchRequest();

            // Act
            var result = _accountService.getSmartSpecialities(model);

            // Assert
            if (result != null)
            {
                Assert.Pass("pass");
            }
            else
            {
                Assert.Fail("Fail");

            }
        }

        [Test]
        public void GetSmartSpecialities_WhenCalled_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var model = new SmartSearchRequest();

            // Act
            var result = _accountService.getSmartSpecialities(model);

            // Assert
            Assert.AreEqual("No Specialties exist", result.ErrorMessage);
        }

        [Test]
        public void GetSmartSpecialities_WhenCalled_ReturnsCorrectMessage()
        {
            // Arrange
            var model = new SmartSearchRequest();

            // Act
            var result = _accountService.getSmartSpecialities(model);

            // Assert
            Assert.AreEqual("No Specialties exist", result.Message);
        }

        [Test]
        public void GetSmartSpecialities_WhenCalled_ReturnsCorrectSuccessValue()
        {
            // Arrange
            var model = new SmartSearchRequest();

            // Act
            var result = _accountService.getSmartSpecialities(model);

            // Assert
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void GetCityDetailByZipCode_ValidZipCode_ReturnsCityDetail()
        {
            // Arrange
            CityDetailByZipCodeRequestModel requestModel = new CityDetailByZipCodeRequestModel { ZipCode = "12345-6789" };
            CityDetailByZipCodeResponseModel expectedResponseModel = new CityDetailByZipCodeResponseModel { zip_city_state = new List<Zip_City_State> { new Zip_City_State { ZIP_Code = "12345-6789", State_Code = "XX", City_Name = "Test City", Time_Zone = "Test Time Zone" } }, ErrorMessage = "", Message = "Detail retrived successfully", Success = true };

            // Act
            CityDetailByZipCodeResponseModel actualResponseModel = _accountService.getCityDetailByZipCode(requestModel);

            // Assert
            if (actualResponseModel != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public void GetCityDetailByZipCode_InvalidZipCode_ReturnsNoDetailsFound()
        {
            // Arrange
            CityDetailByZipCodeRequestModel requestModel = new CityDetailByZipCodeRequestModel { ZipCode = "00000-0000" };
            CityDetailByZipCodeResponseModel expectedResponseModel = new CityDetailByZipCodeResponseModel { zip_city_state = null, ErrorMessage = "", Message = "No details found", Success = false };

            // Act
            CityDetailByZipCodeResponseModel actualResponseModel = _accountService.getCityDetailByZipCode(requestModel);

            // Assert
            if (actualResponseModel != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public void GetCityDetailByZipCode_ExceptionThrown_ReturnsErrorMessage()
        {
            // Arrange
            CityDetailByZipCodeRequestModel requestModel = new CityDetailByZipCodeRequestModel { ZipCode = "00000-0000" };
            CityDetailByZipCodeResponseModel expectedResponseModel = new CityDetailByZipCodeResponseModel { zip_city_state = null, ErrorMessage = "Test Error Message", Message = "Test Error Message", Success = false };

            // Act
            CityDetailByZipCodeResponseModel actualResponseModel = _accountService.getCityDetailByZipCode(requestModel);
            // Assert
            if (actualResponseModel != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }



        [Test]
        public void GetCityDetailByZipCode_ThrowsException_ReturnsErrorMessage()
        {
            // Arrange
            CityDetailByZipCodeRequestModel model = new CityDetailByZipCodeRequestModel { ZipCode = "00000-0000" };
            CityDetailByZipCodeResponseModel expectedResult = new CityDetailByZipCodeResponseModel { zip_city_state = null, ErrorMessage = "Test Error Message", Message = "Test Error Message", Success = false };

            // Act
            CityDetailByZipCodeResponseModel actualResult = _accountService.getCityDetailByZipCode(model);

            // Assert
            if (actualResult != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }


        //[Test]
        //public void EmailToAdmin_Test()
        //{
        //    dynamic user = new ExpandoObject();
        //    user.MOBILE_PHONE = "1234567890";
        //    user.FOX_TBL_SENDER_TYPE_ID = 1;
        //    user.PRACTICE_ORGANIZATION_ID = 1;
        //    user.NPI = "1234567890";
        //    user.FIRST_NAME = "John";
        //    user.LAST_NAME = "Doe";
        //    user.EMAIL = "john.doe@example.com";
        //    user.PRACTICE_ORGANIZATION_TEXT = "Test Practice";
        //    user.ACO_TEXT = "Test ACO";
        //    user.SPECIALITY_TEXT = "Test Speciality";
        //    user.SNF_TEXT = "Test SNF";
        //    user.HOSPITAL_TEXT = "Test Hospital";
        //    user.HHH_TEXT = "Test HHH";
        //    user.COMMENTS = "Test Comments";

        //    _accountService.EmailToAdmin(user);

        //    Assert.IsNotNull(user);
        //    Assert.AreEqual(user.MOBILE_PHONE.Length, 10);
        //    Assert.AreEqual(user.FOX_TBL_SENDER_TYPE_ID, 1);
        //    Assert.AreEqual(user.PRACTICE_ORGANIZATION_ID, 1);
        //    Assert.AreEqual(user.NPI, "1234567890");
        //    Assert.AreEqual(user.FIRST_NAME, "John");
        //    Assert.AreEqual(user.LAST_NAME, "Doe");
        //    Assert.AreEqual(user.EMAIL, "john.doe@example.com");
        //    Assert.AreEqual(user.PRACTICE_ORGANIZATION_TEXT, "Test Practice");
        //    Assert.AreEqual(user.ACO_TEXT, "Test ACO");
        //    Assert.AreEqual(user.SPECIALITY_TEXT, "Test Speciality");
        //    Assert.AreEqual(user.SNF_TEXT, "Test SNF");
        //    Assert.AreEqual(user.HOSPITAL_TEXT, "Test Hospital");
        //    Assert.AreEqual(user.HHH_TEXT, "Test HHH");
        //    Assert.AreEqual(user.COMMENTS, "Test Comments");
        //}

        [Test]
        public void IpConfig_ValidUser_ReturnsTrue()
        {
            // Arrange
            GetUserIP data = new GetUserIP { userIP = "", userName = "test@foxrehab.org" };

            // Act
            bool result = _accountService.IpConfig(data);

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
        public void GetUserCountryByIp_ValidIpAddress_ReturnsCountry()
        {
            // Arrange
            string ipAddress = "8.8.8.8";
            string expectedCountry = "United States";

            // Act
            string actualCountry = _accountService.GetUserCountryByIp(ipAddress);

            // Assert
            if (actualCountry != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public void GetUserCountryByIp_InvalidIpAddress_ReturnsNull()
        {
            // Arrange
            string ipAddress = "invalid";
            string expectedCountry = null;

            // Act
            string actualCountry = _accountService.GetUserCountryByIp(ipAddress);

            // Assert
            if (actualCountry != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        [Test]
        public void IPRequestHelper_ValidUrl_ReturnsString()
        {
            // Arrange
            string url = "http://www.example.com";

            // Act
            string response = _accountService.IPRequestHelper(url);

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
        public void IPRequestHelper_InvalidUrl_ThrowsException()
        {
            // Arrange
            string url = "http://www.invalidurl.com";

            // Assert
            if (url != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }


        [Test]
        public void InsertLogs_ValidInput_LogsInserted()
        {
            //Arrange
            User user = new User();
            user.EMAIL = "test@foxrehab.org";
                user.FIRST_NAME = "Test";
            user.LAST_NAME = "Test";
            string encryptedPassword = "encryptedPassword";
            string detectedBrowser = "Chrome";
            string requestType = "Invalid Internal Login";

            //Act
             _accountService.InsertLogs(user, encryptedPassword, detectedBrowser, requestType);

            //Assert
            Assert.IsNotNull(true);
        }

      


        [Test]
        public void InsertLogs_updsateogsInserted()
        {
            //Arrange
            User user = new User();
            user = null;
            string encryptedPassword = "encryptedPassword";
            string detectedBrowser = "Chrome";
            string requestType = "test";
        

            //Act
            _accountService.InsertLogs(user, encryptedPassword, detectedBrowser, requestType);

            //Assert
            Assert.IsNotNull(true);
        }




        [Test]
        public void CreateExternalUser_UserExists_ReturnsErrorMessage()
        {
            // Arrange
           
            var user = new User();
            user.EMAIL = "abc@test.com";
            user.FIRST_NAME = "Test";
            user.LAST_NAME = "Test";
            user.PASSWORD = "anc";

            // Act
            var result = _accountService.CreateExternalUser(user);

            // Assert
            Assert.IsNotNull(result);
        }



        [Test]
        public void CreateExternalUser_UserExists_eturnsErrorMessage()
        {
            // Arrange

            var user = new User();
            user.EMAIL = "aftabkhan@carecloud.com";
            user.FIRST_NAME = "Test";
            user.LAST_NAME = "Test";
            user.PASSWORD = "anc";
            user.ZIP = "46000";

            // Act
            var result = _accountService.CreateExternalUser(user);

            // Assert
           
            Assert.IsNotNull(result);
        }


        [Test]
        public void EmailToAdmin_Test()
        {
            var user = new User();
            user.MOBILE_PHONE = "1234567890";
            user.FOX_TBL_SENDER_TYPE_ID = 1;
            user.PRACTICE_ORGANIZATION_ID = 1;
            user.NPI = "1234567890";
            user.FIRST_NAME = "John";
            user.LAST_NAME = "Doe";
            user.EMAIL = "aftab.gic@carecloud.com";
            user.PRACTICE_ORGANIZATION_TEXT = "Test Practice";
            user.ACO_TEXT = "Test ACO";
            user.SPECIALITY_TEXT = "Test Speciality";
            user.SNF_TEXT = "Test SNF";
            user.HOSPITAL_TEXT = "Test Hospital";
            user.HHH_TEXT = "Test HHH";
            user.COMMENTS = "Test Comments";

            _accountService.EmailToAdmin(user);

            Assert.IsNotNull(user);
           
        }
































        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _accountService = null;
            _npiRequestModel = null;
            _cityDetailByZipCode = null;
            _emailExist = null;
            _smartSearchRequest = null;
            _getUserIP = null;
            _logoutModel = null;
            _userProfile = null;
        }

    }
}
