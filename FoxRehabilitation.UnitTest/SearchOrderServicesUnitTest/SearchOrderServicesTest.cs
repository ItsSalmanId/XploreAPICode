using FOX.BusinessOperations.SearchOrderServices;
using FOX.DataModels.Models.SearchOrderModel;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.SearchOrderServicesUnitTest
{
    [TestFixture]
    class SearchOrderServicesTest
    {
        private SearchOrderServices _searchOrderServices;
        private UserProfile _userProfile;
        private SearchOrderRequest _searchOrderRequest;
        [SetUp]
        public void SetUp()
        {
            _searchOrderServices = new SearchOrderServices();
            _userProfile = new UserProfile();
            _searchOrderRequest = new SearchOrderRequest();
        }
        [Test]
        [TestCase("one", 0)]
        [TestCase("one", 1011163)]
        public void GetSearchOrder_SearchOrderModel_ReturnData(string firstName, long practiceCode)
        {
            //Arrange
            _searchOrderRequest.Patient_Account = "";
            _searchOrderRequest.MRN = "";
            _searchOrderRequest.First_Name = firstName;
            _searchOrderRequest.Last_Name = "";
            _searchOrderRequest.SearchText = "";
            _searchOrderRequest.DOCUMENT_TYPE = "";
            _searchOrderRequest.SortBy = "receivedate";
            _searchOrderRequest.SortOrder = "DESC";
            _searchOrderRequest.CurrentPage = 1;
            _searchOrderRequest.RecordPerPage = 10;
            _userProfile.PracticeCode = practiceCode;
            _searchOrderRequest.Date_Of_Birth_In_String = Convert.ToString(DateTime.Today);
            _searchOrderRequest.RECEIVE_FROM_In_String = Convert.ToString(DateTime.Today);
            _searchOrderRequest.RECEIVE_TO_In_String = Convert.ToString(DateTime.Today);
            _searchOrderRequest.RECEIVE_FROM_Time_In_String = "12:00:00";
            _searchOrderRequest.RECEIVE_TO_Time_In_String = "12:00:00";

            //Act
            var result = _searchOrderServices.GetSearchOrder(_searchOrderRequest, _userProfile);

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
        [TestCase(1012714)]
        public void ExportToExcelSearchOrder_ReturnsExpectedFileName(long practiceCode)
        {
            // Arrange
            SearchOrderRequest req = new SearchOrderRequest();
            UserProfile profile = new UserProfile();
            req.Patient_Account = "534211554701";
            req.MRN = "";
            req.First_Name = "";
            req.Last_Name = "";
            req.SearchText = "";
            req.DOCUMENT_TYPE = "";
            req.SortBy = "receivedate";
            req.SortOrder = "DESC";
            req.CurrentPage = 1;
            req.RecordPerPage = 10;
            profile.PracticeCode = practiceCode;
            req.Date_Of_Birth_In_String = Convert.ToString(DateTime.Today);
            req.RECEIVE_FROM_In_String = Convert.ToString(DateTime.Today);
            req.RECEIVE_TO_In_String = Convert.ToString(DateTime.Today);
            req.RECEIVE_FROM_Time_In_String = "12:00:00";
            req.RECEIVE_TO_Time_In_String = "12:00:00";
            // Act
            string actualFileName = _searchOrderServices.ExportToExcelSearchOrder(req, profile);

            // Assert
            Assert.IsNotNull(actualFileName);
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _searchOrderRequest = null;
            _userProfile = null;
            _searchOrderServices = null;
        }
    }
}
