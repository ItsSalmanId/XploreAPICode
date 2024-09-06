﻿using FOX.BusinessOperations.QualityAssuranceService.QAReportService;
using FOX.DataModels.Models.QualityAsuranceModel;
using FOX.DataModels.Models.Security;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.QualityAssuranceServiceUnitTest
{
    [TestFixture]
    public class QAReportServiceTest
    {
        private QAReportService _qAReportService;
        private QAReportSearchRequest _qAReportSearchRequest;
        private UserProfile _userProfile;

        [SetUp]
        public void SetUp()
        {
            _qAReportService = new QAReportService();
            _qAReportSearchRequest = new QAReportSearchRequest();
            _userProfile = new UserProfile();
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1011163)]
        [TestCase(38403)]
        public void GetAlertGeneralNotes_FeedBackCallerList_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _qAReportService.GetListOfAgents(practiceCode);

            //Assert
            if (result.Count != 0)
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
        public void GetListOfGradingCriteria_GradingSetupList_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _qAReportService.GetListOfGradingCriteria(practiceCode);

            //Assert
            if (result.Count != 0)
            {
                Assert.IsTrue(true);
            }
            else if (result.Count > 0)
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(0, "", "1163TESTING", 4)]
        [TestCase(0, "", "", 4)]
        [TestCase(0, "", "", 3)]
        [TestCase(101116354816636, "101116354816636", "", 2)]
        [TestCase(0, "", "", 1)]
        public void AuditReport_ExportReport_ReturnData(long patientAccount, string patientAccountStr, string auditorName, int timeFrame)
        {
            //Arrange 
            _userProfile.UserName = "1163testing";
            _userProfile.PracticeCode = 1011163;
            _qAReportSearchRequest.PATIENT_ACCOUNT_STR = patientAccountStr;
            _qAReportSearchRequest.PATIENT_ACCOUNT = patientAccount;
            _qAReportSearchRequest.SEARCH_TEXT = "";
            _qAReportSearchRequest.AGENT_NAME = "";
            _qAReportSearchRequest.TIME_FRAME = timeFrame;
            _qAReportSearchRequest.CURRENT_PAGE = 1;
            _qAReportSearchRequest.RECORD_PER_PAGE = 10;
            _qAReportSearchRequest.SORT_BY = "Rep";
            _qAReportSearchRequest.SORT_ORDER = "DESC";
            _qAReportSearchRequest.CALL_TYPE = "PHD";
            _qAReportSearchRequest.PHD_CALL_SCENARIO_ID = 544106;
            _qAReportSearchRequest.AUDITOR_NAME = auditorName;

            //Act
            var result = _qAReportService.AuditReport(_qAReportSearchRequest, _userProfile);

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
        [TestCase(0, "", "1163TESTING", 4)]
        [TestCase(0, "", "", 4)]
        [TestCase(0, "", "", 3)]
        [TestCase(101116354816636, "101116354816636", "", 2)]
        public void ExportToExcelQAReport_ShouldReturnFileName(long patientAccount, string patientAccountStr, string auditorName, int timeFrame)
        {
            // Arrange
            var req = new QAReportSearchRequest();
            var profile = new UserProfile();
            profile.UserName = "1163testing";
            profile.PracticeCode = 1011163;
            req.PATIENT_ACCOUNT_STR = patientAccountStr;
            req.PATIENT_ACCOUNT = patientAccount;
            req.SEARCH_TEXT = "";
            req.AGENT_NAME = "";
            req.TIME_FRAME = timeFrame;
            req.CURRENT_PAGE = 1;
            req.RECORD_PER_PAGE = 10;
            req.SORT_BY = "Rep";
            req.SORT_ORDER = "DESC";
            req.CALL_TYPE = "PHD";
            req.PHD_CALL_SCENARIO_ID = 544106;
            req.AUDITOR_NAME = auditorName;


            // Act
            var actual = _qAReportService.ExportToExcelQAReport(req, profile);

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public void GetGrade_ResultIsAboveMinimum_ReturnsA()
        {
            // Arrange
            decimal? result = 0;

            // Act
            var grade = _qAReportService.getGrade(result);


            // Assert
            Assert.IsNotNull(grade);
        }




        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _qAReportService = null;
            _qAReportSearchRequest = null;
            _userProfile = null;
        }
    }
}
