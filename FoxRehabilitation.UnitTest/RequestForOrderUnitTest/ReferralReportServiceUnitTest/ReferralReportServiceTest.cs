using FOX.DataModels.Models.Reporting;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;

namespace FoxRehabilitation.UnitTest.RequestForOrderServiceUnitTest
{
    [TestFixture]
    public class ReferralReportServiceTest
    {
        private FOX.BusinessOperations.ReportingServices.ReferralReportServices.ReferralReportServices _referralReportServices;

        [SetUp]
        public void Setup() => this._referralReportServices = new FOX.BusinessOperations.ReportingServices.ReferralReportServices.ReferralReportServices();

        [Test]
        public void Export_ShouldReturnVirtualPath()
        {
            TaskListRequest taskListRequest = new TaskListRequest();
            UserProfile profile = new UserProfile();
            taskListRequest.CalledFrom = "Task_Report";
            taskListRequest.DATE_FROM_STR = "2017-01-01 12:01:01.000";
            taskListRequest.DATE_TO_STR = (string)null;
            profile.PracticeCode = 1012714L;
            taskListRequest.ROLE = "ALL";
            taskListRequest.CURRENT_PAGE = 1;
            taskListRequest.RECORD_PER_PAGE = 1;
            taskListRequest.SEARCH_STRING = "";
            taskListRequest.SORT_BY = "";
            taskListRequest.SORT_ORDER = "";
            Assert.IsNotNull((object)this._referralReportServices.Export(taskListRequest, profile));
        }

        [Test]
        public void GetHighBalanceReportList_ShouldThrowException()
        {
            HighBalanceReportReq obj = new HighBalanceReportReq();
            UserProfile profile = new UserProfile();
            obj.CurrentPage = 1;
            obj.RecordPerPage = 1;
            obj.SEARCH_STRING = "";
            obj.SORT_BY = "";
            obj.SORT_ORDER = "";
            profile.PracticeCode = 1012714L;
            var result  = this._referralReportServices.getHighBalanceReportList(obj, profile);
            Assert.IsNotNull(result);
        }

        [Test]
        public void ExportToExcelHighBalanceReport_ShouldReturnFileName()
        {
            HighBalanceReportReq balanceReportReq = new HighBalanceReportReq();
            UserProfile profile = new UserProfile();
            balanceReportReq.CurrentPage = 1;
            balanceReportReq.RecordPerPage = 1;
            balanceReportReq.SEARCH_STRING = "";
            balanceReportReq.SORT_BY = "";
            balanceReportReq.SORT_ORDER = "";
            profile.PracticeCode = 1012714L;
            Assert.IsNotNull((object)this._referralReportServices.ExportToExcelHighBalanceReport(balanceReportReq, profile));
        }

        [Test]
        [TestCase(1012714, 1)]
        [TestCase(1012714, 2)]
        [TestCase(1012714, 3)]
        [TestCase(1012714, 4)]
        public void GetInterfaceLogReportList_WhenTimeFrameIs2_ShouldReturn15Days(
          long practiceCode,
          int timeFrame)
        {
            InterfaceLogReportReq request = new InterfaceLogReportReq();
            UserProfile profile = new UserProfile();
            request.CurrentPage = 1;
            request.RecordPerPage = 1;
            request.SEARCH_STRING = "";
            profile.PracticeCode = practiceCode;
            profile.UserName = "Test";
            request.STATUS = "";
            request.Application = "";
            request.DATE_FROM = new DateTime?(DateTime.Now.AddDays(-1000.0));
            request.DATE_TO = new DateTime?(DateTime.Now.AddDays(-100.0));
            request.TYPE = "ALL";
            request.SORT_BY = "";
            request.SORT_ORDER = "";
            request.TIME_FRAME = timeFrame;
            Assert.IsNotNull((object)this._referralReportServices.getInterfaceLogReportList(request, profile));
        }

        [Test]
        [TestCase(1012714, 1)]
        [TestCase(1012714, 2)]
        [TestCase(1012714, 3)]
        [TestCase(1012714, 4)]
        public void ExportToExcelInterfaceLogReport_WhenCalled_ShouldReturnFileName(
          long practiceCode,
          int timeFrame)
        {
            InterfaceLogReportReq interfaceLogReportReq = new InterfaceLogReportReq();
            UserProfile profile = new UserProfile();
            interfaceLogReportReq.CurrentPage = 1;
            interfaceLogReportReq.RecordPerPage = 1;
            interfaceLogReportReq.SEARCH_STRING = "";
            profile.PracticeCode = practiceCode;
            profile.UserName = "Test";
            interfaceLogReportReq.STATUS = "";
            interfaceLogReportReq.Application = "";
            interfaceLogReportReq.DATE_FROM = new DateTime?(DateTime.Now.AddDays(-1000.0));
            interfaceLogReportReq.DATE_TO = new DateTime?(DateTime.Now.AddDays(-100.0));
            interfaceLogReportReq.TYPE = "ALL";
            interfaceLogReportReq.SORT_BY = "";
            interfaceLogReportReq.SORT_ORDER = "";
            interfaceLogReportReq.TIME_FRAME = timeFrame;
            string interfaceLogReport = this._referralReportServices.ExportToExcelInterfaceLogReport(interfaceLogReportReq, profile);
            Assert.IsNotNull((object)interfaceLogReport);
            Assert.IsTrue(interfaceLogReport.Contains(".xlsx"));
        }

        [Test]
        public void ExportToExcelRequestToPHRReport_ShouldReturnFileName()
        {
            PHRReportReq phrReportReq = new PHRReportReq();
            UserProfile profile = new UserProfile();
            profile.PracticeDocumentDirectory = "TestDirectory";
            profile.UserName = "Test";
            profile.PracticeCode = 1012714L;
            phrReportReq.CurrentPage = 1;
            phrReportReq.RecordPerPage = 1;
            phrReportReq.SEARCH_STRING = "";
            profile.PracticeCode = 1012714L;
            profile.UserName = "Test";
            phrReportReq.Invitation_STATUS = "";
            phrReportReq.SORT_BY = "";
            phrReportReq.SORT_ORDER = "";
            phrReportReq.Patient_STATUS = "";
            Assert.IsNotNull((object)this._referralReportServices.ExportToExcelRequestToPHRReport(phrReportReq, profile));
        }

        [Test]
        public void GetPHRReportList_ShouldReturnCorrectSortOrder()
        {
            PHRReportReq request = new PHRReportReq();
            UserProfile profile = new UserProfile();
            request.CurrentPage = 1;
            request.RecordPerPage = 1;
            request.SEARCH_STRING = "";
            profile.PracticeCode = 1012714L;
            profile.UserName = "Test";
            request.Invitation_STATUS = "";
            request.SORT_BY = "";
            request.SORT_ORDER = "";
            request.Patient_STATUS = "";
            Assert.IsNotNull((object)this._referralReportServices.getPHRReportList(request, profile));
        }

        [Test]
        [TestCase(1012714, 1)]
        [TestCase(1012714, 2)]
        [TestCase(1012714, 3)]
        [TestCase(1012714, 4)]
        public void GetPHRUsersLoginList_Test(long practiceCode, int timeFrame)
        {
            PHRUserLastLoginRequest request = new PHRUserLastLoginRequest();
            UserProfile profile = new UserProfile();
            request.CURRENT_PAGE = 1;
            request.RECORD_PER_PAGE = 1;
            request.SEARCH_TEXT = "";
            profile.PracticeCode = practiceCode;
            profile.UserName = "Test";
            request.DATE_FROM = new DateTime?(DateTime.Now.AddDays(-1000.0));
            request.DATE_TO = new DateTime?(DateTime.Now.AddDays(-100.0));
            request.SORT_BY = "";
            request.SORT_ORDER = "";
            request.TIME_FRAME = timeFrame;
            Assert.IsNotNull((object)this._referralReportServices.GetPHRUsersLoginList(request, profile));
        }

        [Test]
        [TestCase(1012714, 1)]
        [TestCase(1012714, 2)]
        [TestCase(1012714, 3)]
        [TestCase(1012714, 4)]
        public void ExportPHRUserLastLoginReport_Test(long practiceCode, int timeFrame)
        {
            PHRUserLastLoginRequest request = new PHRUserLastLoginRequest();
            UserProfile profile = new UserProfile();
            request.CURRENT_PAGE = 1;
            request.RECORD_PER_PAGE = 1;
            request.SEARCH_TEXT = "";
            profile.PracticeCode = practiceCode;
            profile.UserName = "Test";
            request.DATE_FROM = new DateTime?(DateTime.Now.AddDays(-1000.0));
            request.DATE_TO = new DateTime?(DateTime.Now.AddDays(-100.0));
            request.SORT_BY = "";
            request.SORT_ORDER = "";
            request.TIME_FRAME = timeFrame;
            Assert.IsNotNull((object)this._referralReportServices.ExportPHRUserLastLoginReport(request, profile));
        }

        [TearDown]
        public void Teardown() => this._referralReportServices = (FOX.BusinessOperations.ReportingServices.ReferralReportServices.ReferralReportServices)null;
    }
}
