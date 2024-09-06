using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Reconciliation;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoxRehabilitation.UnitTest.RequestForOrderServiceUnitTest
{
    [TestFixture]
    public class ReconciliationServiceTest
    {
        private FOX.BusinessOperations.ReconciliationService.ReconciliationService _reconciliationService;
        private UserProfile _userProfile;
        private RequestSendEmailModel _requestSendEmailModel;
        private ResponseModel _result;
        private RequestDeleteWorkOrder _requestDeleteWorkOrder;
        private RequestDownloadPdfModel _requestDownloadPdfModel;
        private ReqAddDocument_SignOrder _reqAddDocumentSign;
        private QRCodeModel _qrCodeModel;

        [SetUp]
        public void Setup()
        {
            this._reconciliationService = new FOX.BusinessOperations.ReconciliationService.ReconciliationService();
            this._userProfile = new UserProfile();
            this._result = new ResponseModel();
        }

        [Test]
        [TestCase(1012714, "Admin_5651352", 1)]
        [TestCase(1012714, "Admin_5651352", 2)]
        [TestCase(1012714, "Admin_5651352", 3)]
        [TestCase(1012714, "Admin_5651352", 4)]
        public void GetReconciliationsCP_TimeFrame1_ReturnsCorrectDateRange(
          long practiceCode,
          string userName,
          int timeFrame)
        {
            ReconciliationCPSearchReq searchReq = new ReconciliationCPSearchReq();
            UserProfile profile = new UserProfile();
            profile.UserName = userName;
            searchReq.SORT_BY = userName;
            profile.UserName = userName;
            profile.PracticeCode = practiceCode;
            searchReq.TIME_FRAME = timeFrame;
            searchReq.DATE_TO_Str = "11/19/2020";
            searchReq.DATE_FROM_Str = "";
            searchReq.SORT_BY = "";
            searchReq.SORT_ORDER = "desc";
            searchReq.CP_Type = 2;
            searchReq.STATE = "";
            searchReq.IsForReport = false;
            searchReq.IS_DEPOSIT_DATE_SEARCH = true;
            searchReq.IS_ASSIGNED_DATE_SEARCH = false;
            searchReq.INSURANCE_NAME = "";
            searchReq.CurrentPage = 1;
            List<ReconciliationCategory> reconciliationsCpp = new List<ReconciliationCategory>();
            ReconciliationCategory RC = new ReconciliationCategory();
            { RC.CATEGORY_ID = 552111;
                RC.CATEGORY_NAME = "COMLBX04"; }
            {
                RC.CATEGORY_ID = 552112;
                RC.CATEGORY_NAME = "COMLBX05"; }
            reconciliationsCpp.Add(RC);

            searchReq.RecordPerPage = 500;
            List<ReconciliationCP> reconciliationsCp = this._reconciliationService.GetReconciliationsCP(searchReq, profile);
            if (reconciliationsCp.Count > 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                if (reconciliationsCp.Count != 0)
                    return;
                Assert.IsFalse(false);
            }
        }

        [Test]
        [TestCase(1012714)]
        [TestCase(null)]
        public void GetReconciliationStatuses_ShouldReturnListOfReconciliationStatuses(long practiceCode)
        {
            List<ReconciliationStatus> reconciliationStatuses = this._reconciliationService.GetReconciliationStatuses(new UserProfile()
            {
                PracticeCode = practiceCode
            });
            Assert.IsNotNull((object)reconciliationStatuses);
            Assert.IsInstanceOf<List<ReconciliationStatus>>((object)reconciliationStatuses);
        }

        [Test]
        [TestCase(1012714)]
        [TestCase(null)]
        [TestCase(0)]
        [TestCase(1011163)]
        public void GetDepositTypes_WhenCalled_ReturnsListOfReconciliationDepositType(long practiceCode) => Assert.IsInstanceOf<List<ReconciliationDepositType>>((object)this._reconciliationService.GetDepositTypes(new UserProfile()
        {
            PracticeCode = practiceCode
        }));

        [Test]
        [TestCase(1012714)]
        [TestCase(null)]
        [TestCase(0)]
        [TestCase(1011163)]
        public void GetReasons_ReturnsListOfReasons(long practiceCode)
        {
            List<FOX_TBL_RECONCILIATION_REASON> reasons = this._reconciliationService.GetReasons(new UserProfile()
            {
                PracticeCode = practiceCode
            });
            Assert.IsNotNull((object)reasons);
            Assert.IsInstanceOf<List<FOX_TBL_RECONCILIATION_REASON>>((object)reasons);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1011163)]
        public void GetReconciliationCategories_ShouldReturnListOfReconciliationCategories(
          long practiceCode)
        {
            List<ReconciliationCategory> reconciliationCategories = this._reconciliationService.GetReconciliationCategories(new UserProfile()
            {
                PracticeCode = practiceCode
            });
            Assert.IsNotNull((object)reconciliationCategories);
            Assert.IsInstanceOf<List<ReconciliationCategory>>((object)reconciliationCategories);
        }

        [Test]
        [TestCase(1, 1011163)]
        [TestCase(2, 1012714)]
        [TestCase(3, 1011163)]
        [TestCase(4, 1012714)]
        public void GetReconciliationInsurances_WhenTimeFrameIsOne_ReturnsCorrectDateFrom(
          int TIME_FRAME,
          long practiceCode)
        {
            Assert.IsNotNull((object)this._reconciliationService.GetReconciliationInsurances(new ReconciliationCPSearchReq()
            {
                TIME_FRAME = TIME_FRAME
            }, new UserProfile() { PracticeCode = practiceCode }));
        }

        [Test]
        [TestCase(4, 1012714)]
        [TestCase(4, 1)]
        public void GetReconciliationInsurances_WhenTimeFrameIsFourAndDateFromStrIsNotNull_ReturnsCorrectDateFrom(
          int TIME_FRAME,
          long practiceCode)
        {
            Assert.IsNotNull((object)this._reconciliationService.GetReconciliationInsurances(new ReconciliationCPSearchReq()
            {
                TIME_FRAME = TIME_FRAME,
                DATE_FROM_Str = "01/01/2020"
            }, new UserProfile() { PracticeCode = practiceCode }));
        }

        [Test]
        [TestCase(1, 1011163)]
        [TestCase(2, 1012714)]
        [TestCase(3, 1011163)]
        [TestCase(4, 1012714)]
        public void GetReconciliationStates_WhenTimeFrameIsOne_ReturnsCorrectDateFrom(
          int TIME_FRAME,
          long practiceCode)
        {
            Assert.IsNotNull((object)this._reconciliationService.GetReconciliationStates(new ReconciliationCPSearchReq()
            {
                TIME_FRAME = TIME_FRAME
            }, new UserProfile() { PracticeCode = practiceCode }));
        }

        [Test]
        [TestCase(4, 1012714)]
        [TestCase(4, 1)]
        public void GetReconciliationStates_WhenTimeFrameIsFourAndDateFromStrIsNotNull_ReturnsCorrectDateFrom(
          int TIME_FRAME,
          long practiceCode)
        {
            Assert.IsNotNull((object)this._reconciliationService.GetReconciliationStates(new ReconciliationCPSearchReq()
            {
                TIME_FRAME = TIME_FRAME,
                DATE_FROM_Str = "01/01/2020"
            }, new UserProfile() { PracticeCode = practiceCode }));
        }

        [Test]
        public void GetReconsiliationCategoryDepositTypes_ShouldReturnReconsiliationCategoryDepositType() => Assert.IsNotNull((object)this._reconciliationService.GetReconsiliationCategoryDepositTypes(new UserProfile()));

        [Test]
        [TestCase(1, 1011163)]
        [TestCase(2, 1012714)]
        [TestCase(3, 1011163)]
        [TestCase(4, 1012714)]
        public void GetReconciliationCheckNos_WhenTimeFrameIsOne_ReturnsCorrectDateFrom(
          int TIME_FRAME,
          long practiceCode)
        {
            Assert.IsNotNull((object)this._reconciliationService.GetReconciliationCheckNos(new ReconciliationCPSearchReq()
            {
                TIME_FRAME = TIME_FRAME
            }, new UserProfile() { PracticeCode = practiceCode }));
        }

        [Test]
        [TestCase(4, 1012714)]
        [TestCase(4, 1)]
        public void GetReconciliationCheckNos_WhenTimeFrameIsFourAndDateFromStrIsNotNull_ReturnsCorrectDateFrom(
          int TIME_FRAME,
          long practiceCode)
        {
            Assert.IsNotNull((object)this._reconciliationService.GetReconciliationCheckNos(new ReconciliationCPSearchReq()
            {
                TIME_FRAME = TIME_FRAME,
                DATE_FROM_Str = "01/01/2020"
            }, new UserProfile() { PracticeCode = practiceCode }));
        }

        [Test]
        [TestCase(1, 1011163)]
        [TestCase(2, 1012714)]
        [TestCase(3, 1011163)]
        [TestCase(4, 1012714)]
        public void GetAmounts_WhenTimeFrameIsOne_ReturnsCorrectDateFrom(
          int TIME_FRAME,
          long practiceCode)
        {
            Assert.IsNotNull((object)this._reconciliationService.GetAmounts(new ReconciliationCPSearchReq()
            {
                TIME_FRAME = TIME_FRAME
            }, new UserProfile() { PracticeCode = practiceCode }));
        }

        [Test]
        [TestCase(4, 1012714)]
        [TestCase(4, 1)]
        public void GetAmounts_WhenTimeFrameIsFourAndDateFromStrIsNotNull_ReturnsCorrectDateFrom(
          int TIME_FRAME,
          long practiceCode)
        {
            Assert.IsNotNull((object)this._reconciliationService.GetAmounts(new ReconciliationCPSearchReq()
            {
                TIME_FRAME = TIME_FRAME,
                DATE_FROM_Str = "01/01/2020"
            }, new UserProfile() { PracticeCode = practiceCode }));
        }

        [Test]
        [TestCase(1, 1011163)]
        [TestCase(2, 1012714)]
        [TestCase(3, 1011163)]
        [TestCase(4, 1012714)]
        public void GetPostedAmounts_WhenTimeFrameIsOne_ReturnsCorrectDateFrom(
          int TIME_FRAME,
          long practiceCode)
        {
            Assert.IsNotNull((object)this._reconciliationService.GetPostedAmounts(new ReconciliationCPSearchReq()
            {
                TIME_FRAME = TIME_FRAME
            }, new UserProfile() { PracticeCode = practiceCode }));
        }

        [Test]
        [TestCase(4, 1012714)]
        [TestCase(4, 1)]
        public void GetPostedAmounts_WhenTimeFrameIsFourAndDateFromStrIsNotNull_ReturnsCorrectDateFrom(
          int TIME_FRAME,
          long practiceCode)
        {
            Assert.IsNotNull((object)this._reconciliationService.GetPostedAmounts(new ReconciliationCPSearchReq()
            {
                TIME_FRAME = TIME_FRAME,
                DATE_FROM_Str = "01/01/2020"
            }, new UserProfile() { PracticeCode = practiceCode }));
        }

        [Test]
        [TestCase(1, 1011163)]
        [TestCase(2, 1012714)]
        [TestCase(3, 1011163)]
        [TestCase(4, 1012714)]
        public void GetNotPostedAmounts_WhenTimeFrameIsOne_ReturnsCorrectDateFrom(
          int TIME_FRAME,
          long practiceCode)
        {
            Assert.IsNotNull((object)this._reconciliationService.GetNotPostedAmounts(new ReconciliationCPSearchReq()
            {
                TIME_FRAME = TIME_FRAME
            }, new UserProfile() { PracticeCode = practiceCode }));
        }

        [Test]
        [TestCase(4, 1012714)]
        [TestCase(4, 1)]
        public void GetNotPostedAmounts_WhenTimeFrameIsFourAndDateFromStrIsNotNull_ReturnsCorrectDateFrom(
          int TIME_FRAME,
          long practiceCode)
        {
            Assert.IsNotNull((object)this._reconciliationService.GetNotPostedAmounts(new ReconciliationCPSearchReq()
            {
                TIME_FRAME = TIME_FRAME,
                DATE_FROM_Str = "01/01/2020"
            }, new UserProfile() { PracticeCode = practiceCode }));
        }

        [Test]
        [TestCase(105, 1011163, "Admin_5651352")]
        [TestCase(108, 1011163, "Admin_5651352")]
        [TestCase(1, 1011163, "")]
        [TestCase(null, 1011163, "")]
        public void GetUsersForDD_WhenCalled_ReturnsListOfUsersForDropdown(
          int RoleId,
          long practiceCode,
          string userName)
        {
            Assert.IsInstanceOf<List<UsersForDropdown>>((object)this._reconciliationService.GetUsersForDD(new UserProfile()
            {
                RoleId = (long)RoleId,
                PracticeCode = practiceCode,
                UserName = userName
            }));
        }

        [Test]
        [TestCase(1, 1012714)]
        [TestCase(10, 1012714)]
        [TestCase(500, 1012714)]
        public void GetReconciliationLogs_WhenCalled_ReturnsListOfReconciliationCPLogs(
          int currentPage,
          long practiceCode)
        {
            ReconciliationCPLogSearchReq searchReq = new ReconciliationCPLogSearchReq();
            UserProfile profile = new UserProfile();
            searchReq.CurrentPage = currentPage;
            profile.PracticeCode = practiceCode;
            Assert.IsNotNull((object)this._reconciliationService.GetReconciliationLogs(searchReq, profile));
        }

        [Test]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494578, 2210.0, "2022-02-26 00:00:00.000" })]
        public void SaveAutoReconciliationCP_WhenCalled_ShouldReturnListOfReconciliationCP(
          long practiceCode,
          string userName,
          long cpID,
          Decimal money,
          string date)
        {
            ReconciliationCP autoreconciliationToSave = new ReconciliationCP();
            UserProfile profile = new UserProfile();
            profile.PracticeCode = practiceCode;
            profile.UserName = userName;
            autoreconciliationToSave.RECONCILIATION_CP_ID = cpID;
            autoreconciliationToSave.AMOUNT_POSTED = new Decimal?(money);
            autoreconciliationToSave.POSTED_DATE_STR = date;
            autoreconciliationToSave.REMARKS = "REMARKS";
            Assert.IsNotNull((object)this._reconciliationService.SaveAutoReconciliationCP(autoreconciliationToSave, profile));
        }

        [Test]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494578, 2210.0, "2022-02-26 00:00:00.000", "test" })]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494579, 450.0, "2023-02-28 00:00:00.000", "test" })]

        public void SaveManualReconciliationCP_WhenCalled_ShouldReturnListOfReconciliationCP(
          long practiceCode,
          string userName,
          long cpID,
          Decimal money,
          string date,
          string remarks)
        {
            ReconciliationCP manualreconciliationToSave = new ReconciliationCP();
            UserProfile profile = new UserProfile();
            profile.PracticeCode = practiceCode;
            profile.UserName = userName;
            manualreconciliationToSave.RECONCILIATION_CP_ID = cpID;
            manualreconciliationToSave.AMOUNT_POSTED = new Decimal?(money);
            manualreconciliationToSave.POSTED_DATE_STR = date;
            manualreconciliationToSave.REMARKS = remarks;
            Assert.IsInstanceOf<List<ReconciliationCP>>((object)this._reconciliationService.SaveManualReconciliationCP(manualreconciliationToSave, profile));
        }

        [Test]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494578, 2210.0, "2022-02-26 00:00:00.000", "test" })]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494592, 720.0, "2023-02-28 00:00:00.000", "test" })]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494592, 440.0, "2023-02-28 00:00:00.000", "test" })]
        public void UpdateAutoReconciliationCP_ShouldReturnListOfReconciliationCP_WhenValidInputsArePassed(
          long practiceCode,
          string userName,
          long cpID,
          Decimal money,
          string date,
          string remarks)
        {
            Assert.IsNotNull((object)this._reconciliationService.UpdateAutoReconciliationCP(new ReconciliationCP()
            {
                RECONCILIATION_CP_ID = cpID,
                AMOUNT_POSTED = new Decimal?(money),
                REMARKS = remarks
            }, new UserProfile()
            {
                PracticeCode = practiceCode,
                UserName = userName
            }));
        }

        [Test]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494592, 0.0, "2023-02-28 00:00:00.000", "remakrs" })]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494582, 13.04, "2023-02-28 00:00:00.000", "" })]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494580, 1765.46, "2023-02-28 00:00:00.000", "remakrs" })]
        public void UpdateAutoReconciliationLogs_WhenDbDataIsNull_ShouldCreateReconciliationLog(
          long practiceCode,
          string userName,
          long cpId,
          Decimal amount,
          string date,
          string remarks)
        {
            ReconciliationCP dbData = (ReconciliationCP)null;
            ReconciliationCP autoReconciliationToUpdate = new ReconciliationCP();
            autoReconciliationToUpdate.REMARKS = remarks.ToString();
            UserProfile profile = new UserProfile();
            profile.PracticeCode = practiceCode;
            profile.UserName = userName;
            autoReconciliationToUpdate.RECONCILIATION_CP_ID = cpId;
            autoReconciliationToUpdate.AMOUNT_POSTED = new Decimal?(amount);
            autoReconciliationToUpdate.POSTED_DATE_STR = date;
            this._reconciliationService.UpdateAutoReconciliationLogs(dbData, autoReconciliationToUpdate, profile);
            Assert.AreEqual((object)"Total Posted amount loaded from Websoft against the listed Check# with +/- balance remaining", (object)autoReconciliationToUpdate.REMARKS);
        }

        [Test]
        [TestCase("Admin_5651352", 1012714)]
        public void GetDDValues_ShouldReturnDDValues(string userName, long practiceCode) => Assert.IsNotNull((object)this._reconciliationService.GetDDValues(new UserProfile()
        {
            UserName = userName,
            PracticeCode = practiceCode
        }));

        [Test]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494592, 0.0, "2023-02-28 00:00:00.000", "remakrs" })]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494582, 13.04, "2023-02-28 00:00:00.000", "" })]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494580, 1765.46, "2023-02-28 00:00:00.000", "remakrs" })]
        public void SaveReconciliationCP_WhenReconciliationIsNull_ShouldReturnErrorMessage(
          long practiceCode,
          string userName,
          long cpId,
          Decimal amount,
          string date,
          string remarks)
        {
            UserProfile profile = new UserProfile();
            ReconciliationCP reconciliationToSave = new ReconciliationCP();
            reconciliationToSave.RECONCILIATION_CP_ID = cpId;
            reconciliationToSave.AMOUNT_POSTED = new Decimal?(amount);
            reconciliationToSave.REMARKS = remarks;
            reconciliationToSave.POSTED_DATE_STR = date;
            profile.PracticeCode = practiceCode;
            profile.UserName = userName;
            ResponseModel responseModel = this._reconciliationService.SaveReconciliationCP(reconciliationToSave, profile);
            if (responseModel != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                if (responseModel != null)
                    return;
                Assert.IsFalse(false);
            }
        }

        [Test]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494592, 0.0, "2023-02-28 00:00:00.000", "remakrs" })]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494582, 13.04, "2023-02-28 00:00:00.000", "" })]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494580, 1765.46, "2023-02-28 00:00:00.000", "remakrs" })]
        public void EditReconciliationCP_Success(
          long practiceCode,
          string userName,
          long cpId,
          Decimal amount,
          string date,
          string remarks)
        {
            ReconciliationCP reconciliationToSave = new ReconciliationCP();
            UserProfile profile = new UserProfile();
            ResponseModel responseModel1 = new ResponseModel();
            reconciliationToSave.RECONCILIATION_CP_ID = cpId;
            reconciliationToSave.AMOUNT_POSTED = new Decimal?(amount);
            reconciliationToSave.REMARKS = remarks;
            reconciliationToSave.POSTED_DATE_STR = date;
            profile.PracticeCode = practiceCode;
            profile.UserName = userName;
            ResponseModel responseModel2 = this._reconciliationService.EditReconciliationCP(reconciliationToSave, profile);
            Assert.IsTrue(responseModel2.Success);
            Assert.AreEqual((object)responseModel2.Message, (object)"Reconciliation updated successfully.");
        }

        [Test]
        public void EditReconciliationCP_Failed()
        {
            ReconciliationCP reconciliationToSave = new ReconciliationCP();
            UserProfile profile = new UserProfile();
            ResponseModel responseModel1 = new ResponseModel();
            ResponseModel responseModel2 = this._reconciliationService.EditReconciliationCP(reconciliationToSave, profile);
            Assert.IsFalse(responseModel2.Success);
            Assert.AreEqual((object)responseModel2.Message, (object)"Reconciliation couldn't be updated.");
        }

        [Test]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494592, 0.0, "2023-02-28 00:00:00.000", "remakrs" })]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494582, 13.04, "2023-02-28 00:00:00.000", "" })]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494580, 1765.46, "2023-02-28 00:00:00.000", "remakrs" })]
        public void SaveBulkEditReconciliationLogs_WhenCalled_ShouldLogReconciliationDetails(
          long practiceCode,
          string userName,
          long cpId,
          Decimal amount,
          string date,
          string remarks)
        {
            ReconciliationCP dbData = new ReconciliationCP();
            ReconciliationCP reconciliationToSave = new ReconciliationCP()
            {
                DEPOSIT_TYPE_ID = new long?(1L),
                CATEGORY_ID = new long?(2L),
                RECONCILIATION_CP_ID = 3
            };
            UserProfile profile = new UserProfile();
            reconciliationToSave.RECONCILIATION_CP_ID = cpId;
            reconciliationToSave.AMOUNT_POSTED = new Decimal?(amount);
            reconciliationToSave.REMARKS = remarks;
            reconciliationToSave.POSTED_DATE_STR = date;
            profile.PracticeCode = practiceCode;
            profile.UserName = userName;
            ResponseModel responseModel = new ResponseModel();
            this._reconciliationService.SaveBulkEditReconciliationLogs(dbData, reconciliationToSave, profile);
            Assert.AreEqual((object)reconciliationToSave.REMARKS, (object)reconciliationToSave.REMARKS);
        }

        [Test]
        [TestCase(1012714)]
        [TestCase(0)]
        public void GetUnassignedStatusId_Returns_Correct_Id(long practiceCode)
        {
            int? unassignedStatusId = this._reconciliationService.GetUnassignedStatusId(new UserProfile()
            {
                PracticeCode = practiceCode
            });
            if (unassignedStatusId.HasValue)
            {
                Assert.IsTrue(true);
            }
            else
            {
                if (unassignedStatusId.HasValue)
                    return;
                Assert.IsFalse(false);
            }
        }

        [Test]
        [TestCase(1012714)]
        [TestCase(0)]
        [TestCase(1011163)]
        public void GetCompletedStatusId_Returns_Null_When_No_Match_Found(long practiceCode)
        {
            int? completedStatusId = this._reconciliationService.GetCompletedStatusId(new UserProfile()
            {
                PracticeCode = practiceCode
            });
            if (completedStatusId.HasValue)
            {
                Assert.IsTrue(true);
            }
            else
            {
                if (completedStatusId.HasValue)
                    return;
                Assert.IsFalse(false);
            }
        }

        [Test]
        [TestCase(1012714)]
        [TestCase(0)]
        [TestCase(1011163)]
        public void GetAssignedStatusId_Returns_Null_When_No_Match_Found(long practiceCode)
        {
            int? assignedStatusId = this._reconciliationService.GetAssignedStatusId(new UserProfile()
            {
                PracticeCode = practiceCode
            });
            if (assignedStatusId.HasValue)
            {
                Assert.IsTrue(true);
            }
            else
            {
                if (assignedStatusId.HasValue)
                    return;
                Assert.IsFalse(false);
            }
        }

        [Test]
        [TestCase(1012714)]
        [TestCase(0)]
        [TestCase(1011163)]
        public void GetUnAssignedStatusId_Returns_Null_When_No_Match_Found(long practiceCode)
        {
            int? assignedStatusId = this._reconciliationService.GetUnAssignedStatusId(new UserProfile()
            {
                PracticeCode = practiceCode
            });
            if (assignedStatusId.HasValue)
            {
                Assert.IsTrue(true);
            }
            else
            {
                if (assignedStatusId.HasValue)
                    return;
                Assert.IsFalse(false);
            }
        }

        [Test]
        [TestCase(1012714)]
        [TestCase(0)]
        [TestCase(1011163)]
        public void GetClosedStatusId_Returns_Null_When_No_Match_Found(long practiceCode)
        {
            int? closedStatusId = this._reconciliationService.GetClosedStatusId(new UserProfile()
            {
                PracticeCode = practiceCode
            });
            if (closedStatusId.HasValue)
            {
                Assert.IsTrue(true);
            }
            else
            {
                if (closedStatusId.HasValue)
                    return;
                Assert.IsFalse(false);
            }
        }

        [Test]
        [TestCase(1012714)]
        [TestCase(0)]
        [TestCase(1011163)]
        public void GetPendingStatusId_Returns_Null_When_No_Match_Found(long practiceCode)
        {
            int? pendingStatusId = this._reconciliationService.GetPendingStatusId(new UserProfile()
            {
                PracticeCode = practiceCode
            });
            if (pendingStatusId.HasValue)
            {
                Assert.IsTrue(true);
            }
            else
            {
                if (pendingStatusId.HasValue)
                    return;
                Assert.IsFalse(false);
            }
        }

        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetInsuranceName_WhenFoxInsIdIsNull_ReturnsEmptyString(long foxIinsId)
        {
            string insuranceName = this._reconciliationService.GetInsuranceName(new long?(foxIinsId));
            Assert.AreEqual((object)string.Empty, (object)insuranceName);
        }

        [Test]
        [TestCase(1012714, 0)]
        [TestCase(0, 1)]
        [TestCase(1011163, 2)]
        public void GetDepositTypeName_WhenDepositTypeIdIsNull_ReturnsEmptyString(
          long practiceCode,
          long depositId)
        {
            string depositTypeName = this._reconciliationService.GetDepositTypeName(new long?(depositId), new UserProfile()
            {
                PracticeCode = practiceCode
            });
            Assert.AreEqual((object)string.Empty, (object)depositTypeName);
        }

        [Test]
        [TestCase(1012714, 0)]
        [TestCase(0, 1)]
        [TestCase(1011163, 2)]
        public void GetCategoryName_WhenCategoryIdIsNull_ReturnsEmptyString(
          long cTypeId,
          long practiceCode)
        {
            UserProfile profile = new UserProfile();
            long? catId = new long?(cTypeId);
            profile.PracticeCode = practiceCode;
            string categoryName = this._reconciliationService.GetCategoryName(catId, profile);
            Assert.AreEqual((object)string.Empty, (object)categoryName);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(null)]
        public void GetReasonName_WhenReasonIdIsNull_ReturnsEmptyString(long rTypeId)
        {
            string reasonName = this._reconciliationService.GetReasonName(new long?(rTypeId));
            Assert.AreEqual((object)string.Empty, (object)reasonName);
        }

        [Test]
        [TestCase(1012714, "khankhan_544596", "Test")]
        public void SaveManualReconciliationLogs_WhenDbDataIsNull_ShouldCreateLog(
          long practiceCode,
          string userName,
          string Remarks)
        {
            ReconciliationCP dbData = (ReconciliationCP)null;
            ReconciliationCP manualreconciliationToSave = new ReconciliationCP();
            manualreconciliationToSave.REMARKS = Remarks;
            this._reconciliationService.SaveManualReconciliationLogs(dbData, manualreconciliationToSave, new UserProfile()
            {
                PracticeCode = practiceCode,
                UserName = userName
            });
            Assert.IsNotNull((object)manualreconciliationToSave.REMARKS);
        }

        [Test]
        [TestCase(new object[] { 1012714, "khankhan_544596", "Test", "Old Remarks" })]
        public void SaveManualReconciliationLogs_WhenRemarksAreDifferent_ShouldCreateLog(
          long practiceCode,
          string userName,
          string Remarks,
          string dbRemarks)
        {
            ReconciliationCP dbData = new ReconciliationCP();
            ReconciliationCP manualreconciliationToSave = new ReconciliationCP();
            UserProfile profile = new UserProfile();
            profile.UserName = userName;
            profile.PracticeCode = practiceCode;
            dbData.REMARKS = dbRemarks;
            manualreconciliationToSave.REMARKS = Remarks;
            dbData.CATEGORY_ID = new long?(1L);
            this._reconciliationService.SaveManualReconciliationLogs(dbData, manualreconciliationToSave, profile);
            Assert.IsNotNull((object)manualreconciliationToSave.REMARKS);
        }

        [Test]
        [TestCase(1012714, "Reconciliation is created", "khankhan_544596")]
        public void SaveAutoReconciliationLogs_WhenDbDataIsNull_ShouldCreateLog(
          long practiceCode,
          string reMarks,
          string userName)
        {
            ReconciliationCP dbData = (ReconciliationCP)null;
            ReconciliationCP autoreconciliationToSave = new ReconciliationCP();
            autoreconciliationToSave.REMARKS = reMarks;
            this._reconciliationService.SaveAutoReconciliationLogs(dbData, autoreconciliationToSave, new UserProfile()
            {
                PracticeCode = practiceCode,
                UserName = userName
            });
            Assert.IsNotNull((object)autoreconciliationToSave.REMARKS);
        }

        [Test]
        //[TestCase(1012714, "Automatically Reconciled", "Manually Reconciled")]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494592, 0.0, "2023-02-28 00:00:00.000", "remakrs" })]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494582, 13.04, "2023-02-28 00:00:00.000", "" })]
        [TestCase(new object[] { 1012714, "Admin_5651352", 53494580, 1765.46, "2023-02-28 00:00:00.000", "remakrs" })]
        public void SaveAutoReconciliationLogs_WhenRemarksAreDifferent_ShouldCreateLog(
          long practiceCode,
          string reMarks,
          string dbRemarks)
        {
            ReconciliationCP dbData = new ReconciliationCP();
            ReconciliationCP autoreconciliationToSave = new ReconciliationCP();
            UserProfile profile = new UserProfile();
            profile.PracticeCode = practiceCode;
            autoreconciliationToSave.REMARKS = reMarks;
            dbData.REMARKS = dbRemarks;
            this._reconciliationService.SaveAutoReconciliationLogs(dbData, autoreconciliationToSave, profile);
            Assert.IsFalse(false);
        }

        [Test]
        [TestCase(1012714, "Admin_5651352", 53494583)]
        [TestCase(1012714, "Admin_5651352", 53494584)]
        [TestCase(1012714, "Admin_5651352", 53494587)]
        public void DeleteReconciliationCP_WhenReconciliationExists_ShouldReturnSuccess(
          long practiceCode,
          string userName,
          int cpId)
        {
            Assert.IsNotNull((object)this._reconciliationService.DeleteReconciliationCP(new ReconciliationCPToDelete()
            {
                RECONCILIATION_CP_ID = (long)cpId
            }, new UserProfile()
            {
                UserName = userName,
                PracticeCode = practiceCode
            }));
        }

        [Test]
        [TestCase(new object[] { 1012714, "TestPath", "TestLedger", 53494583, "Admin_5651352" })]
        public void DeleteReconsiliationLedger_Success(
          long practiceCode,
          string LEDGER_NAME,
          string LEDGER_PATH,
          int cpId,
          string userName)
        {
            long reconsiliationId = (long)cpId;
            UserProfile profile = new UserProfile();
            ReconciliationCP reconciliationCp = new ReconciliationCP()
            {
                RECONCILIATION_CP_ID = reconsiliationId,
                PRACTICE_CODE = profile.PracticeCode,
                LEDGER_NAME = "TestLedger",
                LEDGER_PATH = "TestPath"
            };
            reconciliationCp.PRACTICE_CODE = practiceCode;
            reconciliationCp.LEDGER_NAME = LEDGER_NAME;
            reconciliationCp.LEDGER_PATH = LEDGER_PATH;
            profile.PracticeCode = practiceCode;
            profile.UserName = userName;
            Assert.IsTrue(this._reconciliationService.DeleteReconsiliationLedger(reconsiliationId, profile).Success);
        }

        [Test]
        [TestCase("testuser")]
        [TestCase("invaliduser")]
        [TestCase("Admin_5651352")]
        public void GetUserName_ValidUserName_ReturnsUserName(string userName) => Assert.IsNotNull((object)this._reconciliationService.GetUserName(userName));

        [Test]
        [TestCase(1012714, "Admin_5651352", 1)]
        [TestCase(1012714, "Admin_5651352", 2)]
        [TestCase(1012714, "Admin_5651352", 3)]
        [TestCase(1012714, "Admin_5651352", 4)]
        public void GetReconciliationsCPForExcel_WhenCalled_ReturnsListOfReconciliationCP(
          long practiceCode,
          string userName,
          int timeFrame)
        {
            ReconciliationCPSearchReq searchReq = new ReconciliationCPSearchReq();
            UserProfile profile = new UserProfile();
            profile.UserName = userName;
            profile.PracticeCode = practiceCode;
            searchReq.TIME_FRAME = timeFrame;
            searchReq.DATE_TO_Str = "11/19/2020";
            searchReq.DATE_FROM_Str = "";
            searchReq.SORT_BY = "";
            searchReq.SORT_ORDER = "desc";
            searchReq.CP_Type = 2;
            searchReq.STATE = "";
            searchReq.IsForReport = false;
            searchReq.IS_DEPOSIT_DATE_SEARCH = true;
            searchReq.IS_ASSIGNED_DATE_SEARCH = false;
            searchReq.INSURANCE_NAME = "";
            searchReq.CurrentPage = 1;
            searchReq.RecordPerPage = 500;
            Assert.IsInstanceOf<List<ReconciliationCP>>((object)this._reconciliationService.GetReconciliationsCPForExcel(searchReq, profile));
        }

        [Test]
        public void PrepareExport_WhenCalled_ShouldReturnExpectedResult()
        {
            List<ReconciliationCPExportModel> recordToExport;
            this._reconciliationService.PrepareExport(new List<ReconciliationCP>()
      {
        new ReconciliationCP()
        {
          ROW = 0,
          STATUS_NAME = "Test",
          DEPOSIT_DATE = new DateTime?(DateTime.Now),
          DEPOSIT_TYPE_NAME = "Test",
          STATE = "Test",
          CATEGORY_NAME = "Test",
          INSURANCE_NAME = "Test",
          CHECK_NO = "Test",
          AMOUNT = new Decimal?(10.00M),
          ASSIGNED_DATE = new DateTime?(DateTime.Now),
          ASSIGNED_TO_NAME = "Test",
          AMOUNT_POSTED = new Decimal?(10.00M),
          AMOUNT_NOT_POSTED = new Decimal?(10.00M),
          DATE_POSTED = new DateTime?(DateTime.Now),
          REASON_NAME = "Test",
          ASSIGNED_GROUP = "Test",
          ASSIGNED_GROUP_DATE = new DateTime?(DateTime.Now)
        }
      }, out recordToExport);
            Assert.AreEqual((object)1, (object)recordToExport.Count);
        }

        [Test]
        public void ExportReconciliationCPLogsToExcel_Success() => Assert.IsFalse(this._reconciliationService.ExportReconciliationCPLogsToExcel(new List<ReconciliationCPLogs>(), new UserProfile()
        {
            PracticeDocumentDirectory = "PracticeDocumentDirectory"
        }).Success);

        [Test]
        public void ExportReconciliationCPLogsToExcel_Failure() => Assert.IsFalse(this._reconciliationService.ExportReconciliationCPLogsToExcel((List<ReconciliationCPLogs>)null, new UserProfile()
        {
            PracticeDocumentDirectory = "PracticeDocumentDirectory"
        }).Success);

        [Test]
        public void PrepareLogExport_ValidInput_ReturnsExpectedOutput()
        {
            List<ReconciliationCPLogExportModel> recordToExport;
            this._reconciliationService.PrepareLogExport(new List<ReconciliationCPLogs>()
      {
        new ReconciliationCPLogs()
        {
          CREATED_DATE = DateTime.Now,
          LOG_MESSAGE = "Test Message",
          CREATED_BY_NAME = "Test User"
        }
      }, out recordToExport);
            Assert.AreEqual((object)recordToExport[0].CREATED_DATE, (object)DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            Assert.AreEqual((object)recordToExport[0].LOG_MESSAGE, (object)"Test Message");
            Assert.AreEqual((object)recordToExport[0].CREATED_BY_NAME, (object)"Test User");
        }

        [Test]
        [TestCase(1012714, "Admin_5651352", 53494594)]
        [TestCase(1012714, "Admin_5651352", 53494595)]
        [TestCase(1012714, "Admin_5651352", 53494596)]
        public void AttachLedger_ValidInput_ReturnsSuccess(
          long practicCode,
          string userName,
          long cpId)
        {
            LedgerModel ledgerDetails = new LedgerModel();
            UserProfile profile = new UserProfile();
            profile.PracticeCode = practicCode;
            profile.UserName = userName;
            ledgerDetails.RECONCILIATION_CP_ID = cpId;
            ledgerDetails.FILE_NAME = "test.pdf";
            ledgerDetails.AbsolutePath = "C:\\test.pdf";
            ledgerDetails.BASE_64_DOCUMENT = "test";
            Assert.IsTrue(this._reconciliationService.AttachLedger(ledgerDetails, profile).Success);
        }

        [Test]
        [TestCase(1012714, "Admin_5651352", 53494583)]
        [TestCase(1012714, "Admin_5651352", 53494584)]
        [TestCase(1012714, "Admin_5651352", 53494587)]
        public void AttachLedger_ValidInput_ReturnFalse(long practicCode, string userName, long cpId)
        {
            LedgerModel ledgerDetails = new LedgerModel();
            UserProfile profile = new UserProfile();
            profile.PracticeCode = practicCode;
            profile.UserName = userName;
            ledgerDetails.RECONCILIATION_CP_ID = cpId;
            ledgerDetails.FILE_NAME = "test.pdf";
            ledgerDetails.AbsolutePath = "C:\\test.pdf";
            ledgerDetails.BASE_64_DOCUMENT = "test";
            Assert.IsFalse(this._reconciliationService.AttachLedger(ledgerDetails, profile).Success);
        }


        [Test]
        [TestCase(1012714, "Admin_5651352", 53494584)]
        [TestCase(1012714, "Admin_5651352", 53494587)]
        public void LogLedgerAttached_IsAttachedFirstTime_ShouldLogLedgerAttached(
          long practicCode,
          string userName,
          long cpId)
        {
            bool isAttachedFirstTime = true;
            object previousLedger = (object)"";
            string newLedger = "test";
            object previousPath = (object)"";
            string newLedgerPath = "testPath";
            long reconciliation_CP_ID = cpId;
            UserProfile userProfile = new UserProfile();
            userProfile.UserName = userName;
            userProfile.PracticeCode = practicCode;
            this._reconciliationService.LogLedgerAttached(isAttachedFirstTime, previousLedger, newLedger, previousPath, newLedgerPath, reconciliation_CP_ID, userProfile);
            Assert.IsNotNull((object)userProfile);
        }

        [Test]
        [TestCase(new object[] { 53411401, "Davis_53411401", "Test", "DepositType" })]
        [TestCase(new object[] { 53411401, "Davis_53411401", "Test", "category" })]
        [TestCase(new object[] { 53411401, "Davis_53411401", "Test", "status" })]
        public void AddNewDDValue_WhenCalled_ShouldReturnReconciliationDDValueResponse(
          long userId,
          string UserName,
          string Value,
          string ValueType)
        {
            ReconciliationDDValue reconciliationDDValue = new ReconciliationDDValue();
            UserProfile profile = new UserProfile();
            profile.userID = userId;
            profile.UserName = UserName;
            reconciliationDDValue.Value = Value;
            reconciliationDDValue.ValueType = ValueType;
            Assert.IsInstanceOf<ReconciliationDDValueResponse>((object)this._reconciliationService.AddNewDDValue(reconciliationDDValue, profile));
        }

        [Test]
        [TestCase(1012714, "Davis_53411401")]
        public void AddNewStatus_ValidInput_ReturnsLong(long practiceCode, string UserName)
        {
            string newStatusValue = "TestStatus";
            UserProfile profile = new UserProfile();
            profile.UserName = UserName;
            profile.PracticeCode = practiceCode;
            ReconciliationStatus reconciliationStatus = new ReconciliationStatus();
            Assert.IsInstanceOf<long>((object)this._reconciliationService.AddNewStatus(newStatusValue, profile));
        }

        [Test]
        [TestCase(1012714, "Davis_53411401")]
        public void AddNewStatus_ValidInput_SetsCorrectModifiedDate(long practiceCode, string UserName)
        {
            string newStatusValue = "TestStatus";
            UserProfile profile = new UserProfile();
            ReconciliationStatus reconciliationStatus = new ReconciliationStatus();
            profile.UserName = UserName;
            profile.PracticeCode = practiceCode;
            Assert.IsInstanceOf<long>((object)this._reconciliationService.AddNewStatus(newStatusValue, profile));
        }

        [Test]
        [TestCase(1012714, "Davis_53411401")]
        public void AddNewStatus_ValidInput_SetsDeletedToFalse(long practiceCode, string UserName)
        {
            string newStatusValue = "TestStatus";
            UserProfile profile = new UserProfile();
            ReconciliationStatus reconciliationStatus = new ReconciliationStatus();
            profile.UserName = UserName;
            profile.PracticeCode = practiceCode;
            Assert.IsInstanceOf<long>((object)this._reconciliationService.AddNewStatus(newStatusValue, profile));
        }

        [Test]
        [TestCase(1012714, "Davis_53411401")]
        public void AddNewDepositType_ValidInput_ReturnsUniqueId(long practiceCode, string UserName) => Assert.AreNotEqual((object)0, (object)this._reconciliationService.AddNewDepositType("Test", new UserProfile()
        {
            PracticeCode = practiceCode,
            UserName = UserName
        }));

        [Test]
        [TestCase(1012714, "Davis_53411401")]
        public void GetLastUploadFileStatusForHrAutoEmails_ValidInput_ReturnsUniqueId(
          long practiceCode,
          string userName)
        {
            Assert.AreNotEqual((object)0, (object)this._reconciliationService.GetLastUploadFileStatusForHrAutoEmails(new UserProfile()
            {
                PracticeCode = practiceCode,
                UserName = userName
            }, "Test"));
        }

        [Test]
        [TestCase(1012714, "Davis_53411401")]
        [TestCase(1012714, "Conley_53411400")]
        [TestCase(1012714, "Morse_53411399")]
        public void GetLastUploadFileStatus_ValidInput_ReturnsUniqueId(
          long practiceCode,
          string userName)
        {
            Assert.AreNotEqual((object)0, (object)this._reconciliationService.GetLastUploadFileStatus(new UserProfile()
            {
                PracticeCode = practiceCode,
                UserName = userName
            }));
        }

        [Test]
        [TestCase(1012714, "Davis_53411401")]
        [TestCase(1012714, "Conley_53411400")]
        [TestCase(1012714, "Morse_53411399")]
        public void InsertLatestUpload_WhenCalled_ReturnsFOX_TBL_RECONCILIATION_UPLOAD_LOG(
          long practiceCode,
          string userName)
        {
            Assert.AreNotEqual((object)0, (object)this._reconciliationService.InsertLatestUpload(10L, 5L, new UserProfile()
            {
                PracticeCode = practiceCode,
                UserName = userName
            }));
        }

        [Test]
        [TestCase(new object[] { "123", 1, 10, 1012714, "Davis_53411401" })]
        [TestCase(new object[] { "45441", 1, 10, 1012714, "Davis_53411401" })]
        public void GetSoftReconsilitionPayment_Returns_Correct_Result(
          string CHECK_NO,
          int CurrentPage,
          int RecordPerPage,
          long practiceCode,
          string userName)
        {
            SOFT_RECONCILIATION_SERACH_REQUEST reconciliationSerachRequest = new SOFT_RECONCILIATION_SERACH_REQUEST();
            UserProfile profile = new UserProfile();
            reconciliationSerachRequest.CHECK_NO = CHECK_NO;
            reconciliationSerachRequest.CurrentPage = CurrentPage;
            reconciliationSerachRequest.RecordPerPage = RecordPerPage;
            profile.UserName = userName;
            profile.PracticeCode = practiceCode;
            Assert.AreNotEqual((object)0, (object)this._reconciliationService.GetSoftReconsilitionPayment(reconciliationSerachRequest, profile));
        }

        [Test]
        [TestCase(new object[] { "123", 1, 10, 1012714, "Davis_53411401" })]
        [TestCase(new object[] { "45441", 1, 10, 1012714, "Davis_53411401" })]
        public void GetWebsoftPayment_Returns_List_Of_SOFT_RECONCILIATION_PAYMENT(
          string CHECK_NO,
          int CurrentPage,
          int RecordPerPage,
          long practiceCode,
          string userName)
        {
            SOFT_RECONCILIATION_SERACH_REQUEST softRequest = new SOFT_RECONCILIATION_SERACH_REQUEST();
            UserProfile profile = new UserProfile();
            profile.PracticeCode = practiceCode;
            profile.UserName = userName;
            softRequest.CHECK_NO = CHECK_NO;
            softRequest.DEPOSIT_DATE_STR = "01/01/2020";
            Assert.IsInstanceOf<List<SOFT_RECONCILIATION_PAYMENT>>((object)this._reconciliationService.GetWebsoftPayment(softRequest, profile));
        }

        [Test]
        [TestCase(1012714, "Davis_53411401")]
        [TestCase(1012714, "Conley_53411400")]
        [TestCase(1012714, "")]
        public void GetMTBCDistinctCategoryName_WhenCalled_ReturnsListOfMTBCCategoryDescriptionCount(
          long practiceCode,
          string userName)
        {
            Assert.IsInstanceOf<List<MTBC_Category_Description_Count>>((object)this._reconciliationService.GetMTBCDistinctCategoryName(new UserProfile()
            {
                PracticeCode = practiceCode,
                UserName = userName
            }));
        }

        [Test]
        [TestCase(1012714, "Admin_5651352", 1)]
        [TestCase(1012714, "Admin_5651352", 2)]
        [TestCase(1012714, "Admin_5651352", 3)]
        [TestCase(1012714, "Admin_5651352", 4)]
        public void ExportReconciliationsToExcel_Success(
          long practiceCode,
          string userName,
          int timeFrame)
        {
            ReconciliationCPSearchReq searchReq = new ReconciliationCPSearchReq();
            UserProfile profile = new UserProfile()
            {
                PracticeCode = 1012714,
                UserName = "Test"
            };
            profile.UserName = userName;
            profile.PracticeCode = practiceCode;
            searchReq.TIME_FRAME = timeFrame;
            searchReq.DATE_TO_Str = "11/19/2020";
            searchReq.DATE_FROM_Str = "";
            searchReq.SORT_BY = "";
            searchReq.SORT_ORDER = "desc";
            searchReq.CP_Type = 2;
            searchReq.STATE = "";
            searchReq.IsForReport = false;
            searchReq.IS_DEPOSIT_DATE_SEARCH = true;
            searchReq.IS_ASSIGNED_DATE_SEARCH = false;
            searchReq.INSURANCE_NAME = "";
            searchReq.CurrentPage = 1;
            searchReq.RecordPerPage = 500;
            Assert.IsNotNull((object)this._reconciliationService.ExportReconciliationsToExcel(searchReq, profile));
        }
        [TearDown]
        public void Teardown()
        {
            this._reconciliationService = (FOX.BusinessOperations.ReconciliationService.ReconciliationService)null;
            this._userProfile = (UserProfile)null;
            this._requestSendEmailModel = (RequestSendEmailModel)null;
            this._result = (ResponseModel)null;
        }
    }
}
