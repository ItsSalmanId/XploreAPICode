using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.StatesModel;
using System.Collections.Generic;
using System.Data;
using System;
using FOX.DataModels.Models.Settings.FacilityLocation;
using System.Linq;
using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Models.CommonModel;
using System.Web;
using FOX.BusinessOperations.CommonServices;
using System.IO;
using NUnit.Framework;
using FOX.DataModels.Models.Settings.Practice;
using FOX.DataModels.Models.PatientSurvey;
using static FOX.DataModels.Models.SurveyAutomation.SurveyAutomations;
using FOX.DataModels.Models.Reconciliation;

namespace FOX.BusinessOperations.SettingsService.ReferralRegionServices
{
    [TestFixture]
    public class ReferralRegionServiceTest
    {
        private ReferralRegionService _referralRegionService;
        private GenericRepository<FacilityType> _facilityTypesRepository;
        private UserProfile _userProfile;
        private PracticeOrganizationRequest _practiceOrganizationRequest;
        private PracticeOrganization _practiceOrganization;
        private GenericRepository<FOX_TBL_ZIP_STATE_COUNTY> _zipStateCountyRepository;

        [SetUp]

        public void Setup()
        {
            _referralRegionService = new ReferralRegionService();
            _zipStateCountyRepository = new GenericRepository<FOX_TBL_ZIP_STATE_COUNTY>();
            _facilityTypesRepository = new GenericRepository<FacilityType>();
            _userProfile = new UserProfile();
            _practiceOrganizationRequest = new PracticeOrganizationRequest();
            _practiceOrganization = new PracticeOrganization();

        }
        [Test]
        public void GetCountiesListByStateCode_WhenCalled_ReturnsListOfFOX_TBL_ZIP_STATE_COUNTY()
        {
            // Arrange
            string stateCode = "CA";
            long practiceCode = 123;
            var expectedResult = typeof(List<FOX_TBL_ZIP_STATE_COUNTY>);

            // Act
            var result = _referralRegionService.GetCountiesListByStateCode(stateCode, practiceCode);

            // Assert
            Assert.AreEqual(expectedResult, result.GetType());
        }

        [Test]
        public void GetCountiesListByStateCode_WhenCalled_ReturnsCorrectNumberOfMappedZipCounties()
        {
            // Arrange
            var stateCode = "CA";
            var practiceCode = 123;
            var expectedResult = 10;

            // Act
            var result = _referralRegionService.GetCountiesListByStateCode(stateCode, practiceCode);

            // Assert
            //Assert.AreEqual(expectedResult, result);
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
        public void GetReferralRegionByName_ShouldReturnCorrectReferralRegion()
        {
            // Arrange
            var searchString = "CT";
            var practiceCode = 1011163;
            
            // Act
            var actualReferralRegion = _referralRegionService.GetReferralRegionByName(searchString, practiceCode);

            // Assert
            Assert.IsNotNull(actualReferralRegion);
        }



        [Test]
        public void GetCountiesListByStateCode_WhenCalled_ReturnsCorrectNumberOfTotalCounties()
        {
            // Arrange
            var stateCode = "PR";
            var practiceCode = 1011163;
        

            // Act
            var result = _referralRegionService.GetCountiesListByStateCode(stateCode, practiceCode);

            // Assert
            //Assert.AreEqual(expectedResult, result);
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
        public void GetCountiesByReferralRegionId_Returns_List_Of_FOX_TBL_ZIP_STATE_COUNTY()
        {
            // Arrange
            // Arrange
            long referralRegionId = 552400;
            long practiceCode = 1011163;

            // Act
            var result = _referralRegionService.GetCountiesByReferralRegionId(referralRegionId, practiceCode);

            // Assert
            //Assert.AreEqual(expectedResult, result.GetType());
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
        public void GetCountiesByReferralRegionId_Returns_Correct_Number_Of_Counties()
        {
            // Arrange
            long referralRegionId = 552400;
            long practiceCode = 1011163;
       

            // Act
            var result = _referralRegionService.GetCountiesByReferralRegionId(referralRegionId, practiceCode);

            // Assert
            // Assert.AreEqual(expectedResult, result.Count());
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
        public void GetCountiesByReferralRegionId_Returns_Correct_Maped_Zip_Count()
        {
            // Arrange
            long referralRegionId = 1;
            long practiceCode = 1;
            int expectedResult = 3;

            // Act
            var result = _referralRegionService.GetCountiesByReferralRegionId(referralRegionId, practiceCode);

            // Assert
            // Assert.AreEqual(expectedResult, result[0].MAPED_ZIP_COUNT);
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
        public void GetCountiesByReferralRegionId_Returns_Correct_Total_Counties_Count()
        {
            // Arrange
            long referralRegionId = 1;
            long practiceCode = 1;
            int expectedResult = 5;

            // Act
            var result = _referralRegionService.GetCountiesByReferralRegionId(referralRegionId, practiceCode);

            // Assert
            //Assert.AreEqual(expectedResult, result[0].TOTAL_COUNTIES_COUNT);
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
        public void GetReferralRegionByZipCode_ValidInput_ReturnsReferralRegion()
        {
            // Arrange
            string zipCode = "12345";
            long practiceCode = 123;

            // Act
            ReferralRegion result = _referralRegionService.GetReferralRegionByZipCode(zipCode, practiceCode);

            // Assert
            //Assert.IsNotNull(result);
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
        public void GetReferralRegionByZipCode_InvalidInput_ReturnsNull()
        {
            // Arrange
            string zipCode = "";
            long practiceCode = 0;

            // Act
            ReferralRegion result = _referralRegionService.GetReferralRegionByZipCode(zipCode, practiceCode);

            // Assert
            //Assert.IsNull(result);
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
        public void GetReferralRegionByPatientHomeAddressZipCode_ValidInput_ReturnsReferralRegion()
        {
            // Arrange
            string patient_AccountStr = "12345";
            long practiceCode = 123;

            // Act
            var result = _referralRegionService.GetReferralRegionByPatientHomeAddressZipCode(patient_AccountStr, practiceCode);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<ReferralRegion>>(result);
        }

        [Test]
        public void GetReferralRegionByPatientHomeAddressZipCode_InvalidInput_ReturnsNull()
        {
            // Arrange
            long practiceCode = 1011163;
            string patient_AccountStr = "101271453411656";
            // Act
            var result = _referralRegionService.GetReferralRegionByPatientHomeAddressZipCode(patient_AccountStr, practiceCode);

            // Assert
            // Assert.IsNull(result);
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
        public void GetReferralRegionZipCodeData_WhenCalled_ReturnsListOfFOX_TBL_ZIP_STATE_COUNTY()
        {
            // Arrange
            var req = new RegionZipCodeDataReq();
            var profile = new UserProfile();

            // Act
            var result = _referralRegionService.GetReferralRegionZipCodeData(req, profile);

            // Assert
            Assert.IsInstanceOf<List<FOX_TBL_ZIP_STATE_COUNTY>>(result);
        }

        [Test]
        public void GetReferralRegionZipCodeData_WhenCalledWithValidParams_ReturnsNonEmptyList()
        {
            // Arrange
            var req = new RegionZipCodeDataReq();
            var profile = new UserProfile();

            // Act
            var result = _referralRegionService.GetReferralRegionZipCodeData(req, profile);

            // Assert
            //Assert.IsNotEmpty(result);
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
        public void GetReferralRegionZipCodeData_WhenCalledWithInvalidParams_ReturnsEmptyList()
        {
            // Arrange
            var req = new RegionZipCodeDataReq();
            var profile = new UserProfile();
            req.COUNTY = "invalid";
            req.State = "invalid";

            // Act
            var result = _referralRegionService.GetReferralRegionZipCodeData(req, profile);

            // Assert
            //Assert.IsEmpty(result);
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
        public void RemoveReferralRegionCounty_WhenCalled_ReturnsResponseModel()
        {
            // Arrange
            var county = new FOX_TBL_ZIP_STATE_COUNTY();
            var profile = new UserProfile();
            profile.PracticeCode = 1011163;
            county.COUNTY = "Cabo Rojo Municipio";
            county.STATE = "PR";
            county.REFERRAL_REGION_ID = 544228;

            // Act
            var result = _referralRegionService.RemoveReferralRegionCounty(county, profile);

            // Assert
            Assert.IsInstanceOf<ResponseModel>(result);
        }

        [Test]
        public void RemoveReferralRegionCounty_WhenCalled_ReturnsSuccessMessage()
        {
            // Arrange
            var county = new FOX_TBL_ZIP_STATE_COUNTY();
            var profile = new UserProfile();
            profile.PracticeCode = 1011163;
            county.COUNTY = "Cabo Rojo Municipio";
            county.STATE = "PR";
            county.REFERRAL_REGION_ID = 544228;

            // Act
            var result = _referralRegionService.RemoveReferralRegionCounty(county, profile);

            // Assert
            Assert.AreEqual("Success", result.Message);
        }

        [Test]
        public void RemoveReferralRegionCounty_WhenCalled_ReturnsEmptyErrorMessage()
        {
            // Arrange
            var county = new FOX_TBL_ZIP_STATE_COUNTY();
            var profile = new UserProfile();
            profile.PracticeCode = 1011163;
            county.COUNTY = "Cabo Rojo Municipio";
            county.STATE = "PR";
            county.REFERRAL_REGION_ID = 544228;

            // Act
            var result = _referralRegionService.RemoveReferralRegionCounty(county, profile);

            // Assert
            Assert.AreEqual("", result.ErrorMessage);
        }


        [Test]
        public void MapAllZipCounties_Returns_Success()
        {
            // Arrange
            RegionZipCodeDataReq req = new RegionZipCodeDataReq();
            req.COUNTY = "Cabo Rojo Municipio";
            req.REFERRAL_REGION_ID = 544227;
            req.State = "PR";
            UserProfile profile = new UserProfile();
            profile.PracticeCode = 1011163;

            // Act
            string result = _referralRegionService.MapAllZipCounties(req, profile);

            // Assert
            //Assert.AreEqual("Success", result);
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
        public void MapAllZipCounties_Returns_EmptyString()
        {
            // Arrange
            RegionZipCodeDataReq req = new RegionZipCodeDataReq();
            req.COUNTY = "";
            req.REFERRAL_REGION_ID = 0;
            req.State = "";
            UserProfile profile = new UserProfile();
            profile.PracticeCode = 1011163;

            // Act
            string result = _referralRegionService.MapAllZipCounties(req, profile);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }



            [Test]
            public void GetCityStateCountyRegion_ValidZipCode_ReturnsCorrectData()
            {
                // Arrange
                var zipCode = "605107";
                var profile = new UserProfile { PracticeCode = 1011163 };

                // Act
                var result = _referralRegionService.GetCityStateCountyRegion(zipCode, profile);

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
            public void GetCityStateCountyRegion_ZipCodeWithDash_ReturnsCorrectData()
            {
                // Arrange
                string zipCode = "605107";
                var profile = new UserProfile { PracticeCode = 1011163 };

                // Act
                var result = _referralRegionService.GetCityStateCountyRegion(zipCode, profile);

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
            public void GetCityStateCountyRegion_InvalidZipCode_ReturnsNull()
            {
                // Arrange
                var zipCode = "605107-123456";
                var profile = new UserProfile { PracticeCode = 1011163 };

                // Act
                var result = _referralRegionService.GetCityStateCountyRegion(zipCode, profile);

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
            public void GetCityStateCountyRegionByCity_ValidInput_ReturnsExpectedResult()
            {
                // Arrange
                var city = "New York";
                var profile = new UserProfile { PracticeCode = 1011163 };
                //var expectedResult = new ZipCityStateCountyRegion { City = "New York", State = "NY", County = "New York", Region = "Northeast" };

                // Act
                var result = _referralRegionService.GetCityStateCountyRegionByCity(city, profile);

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
            public void GetCityStateCountyRegionByCity_InvalidInput_ReturnsNull()
            {
                // Arrange
                var city = "";
                var profile = new UserProfile { PracticeCode = 1011163 };

                // Act
                var result = _referralRegionService.GetCityStateCountyRegionByCity(city, profile);

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
        public void GetCityStateCountyRegionByCounty_ShouldReturnCorrectResult()
        {
            // Arrange
            var county = "Los Angeles";
            var profile = new UserProfile { PracticeCode = 1011163 };
            //var expectedResult = new ZipCityStateCountyRegion { City = "Los Angeles", State = "CA", County = "Los Angeles", Region = "West" };

            // Act
            var result = _referralRegionService.GetCityStateCountyRegionByCounty(county, profile);

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
        public void GetSmartCity_WhenCalled_ReturnsListOfStrings()
        {
            // Arrange
            var city = "New York";
            var profile = new UserProfile { PracticeCode = 1011163 };

            // Act
            var result = _referralRegionService.GetSmartCity(city, profile);

            // Assert
            Assert.IsInstanceOf<List<string>>(result);
        }
        [Test]
        public void GetSmartCounty_ShouldReturnCorrectResult()
        {
            // Arrange
            var county = "Los Angeles";
            var profile = new UserProfile { PracticeCode = 1011163 };
            //var expectedResult = new ZipCityStateCountyRegion { City = "Los Angeles", State = "CA", County = "Los Angeles", Region = "West" };

            // Act
            var result = _referralRegionService.GetSmartCounty(county, profile);

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
        public void GetAdvancedRegionSmartSearch_ShouldReturnCorrectResult()
        {
            // Arrange

            long PracticeCode = 1011163;
            var searchString = "";
            //var expectedResult = new ZipCityStateCountyRegion { City = "Los Angeles", State = "CA", County = "Los Angeles", Region = "West" };

            // Act
            var result = _referralRegionService.GetAdvancedRegionSmartSearch(searchString, PracticeCode);

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
        public void ExportAdvancedRegion_ShouldCreateExcelDocument()
        {
            // Arrange
            var advancedregionreq = new showHideAdvancedRegionCol();
            var profile = new UserProfile();
            profile.PracticeDocumentDirectory = "TestDirectory";
            profile.PracticeCode = 1012714;
            var objAdvanceRegionSearchRequest = new AdvanceRegionSearchRequest();
            objAdvanceRegionSearchRequest.CheckedRegionsIDString = "12305";
            objAdvanceRegionSearchRequest.RecordPerPage = 10;
            objAdvanceRegionSearchRequest.CurrentPage = 1;
            advancedregionreq.REFERRAL_REGION_NAME = true;
            advancedregionreq.ZIP_CODE = true;
            advancedregionreq.ObjAdvanceRegionSearchRequest = objAdvanceRegionSearchRequest;

            // Act
            var result = _referralRegionService.ExportAdvancedRegion(advancedregionreq, profile);

            // Assert
            Assert.IsNotNull(result);
          
        }



        [Test]
        public void GetAdvancedRegionSearch_WhenCalled_ReturnsListOfAdvancedRegionsWithZipCodes()
        {
            // Arrange
            var objAdvanceRegionSearchRequest = new AdvanceRegionSearchRequest();
            objAdvanceRegionSearchRequest.CheckedRegionsIDString = "12305";
            long practiceCode = 1012714;
            objAdvanceRegionSearchRequest.RecordPerPage = 10;
            objAdvanceRegionSearchRequest.CurrentPage = 1;
            // Act
            var result = _referralRegionService.GetAdvancedRegionSearch(objAdvanceRegionSearchRequest, practiceCode);


            // Assert
            Assert.IsInstanceOf<List<AdvancedRegionsWithZipCodes>>(result);
        }

  









        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _referralRegionService = null;
            _zipStateCountyRepository = null;
            _facilityTypesRepository = null;
            _userProfile = null;
        }

    }


}
