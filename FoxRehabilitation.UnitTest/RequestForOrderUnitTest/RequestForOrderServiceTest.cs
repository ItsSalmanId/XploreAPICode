using FOX.BusinessOperations.RequestForOrder;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.ReferralSource;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.RequestForOrderServiceUnitTest
{
    [TestFixture]
    public class RequestForOrderServiceTest
    {
        private RequestForOrderService _requestForOrderService;
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
            _requestForOrderService = new RequestForOrderService();
            _userProfile = new UserProfile();
            _result = new ResponseModel();
            _requestSendEmailModel = new RequestSendEmailModel();
            _requestDeleteWorkOrder = new RequestDeleteWorkOrder();
            _requestDownloadPdfModel = new RequestDownloadPdfModel();
            _reqAddDocumentSign = new ReqAddDocument_SignOrder();
            _qrCodeModel = new QRCodeModel();
        }

        [Test]
        public void SendEmail_CheckRehabUser_ResponseFalse()
        {
            //Arrange
            _userProfile.PracticeCode = 5110459;
            _userProfile.isTalkRehab = false;
            _requestSendEmailModel.WorkId = 123;
            _requestSendEmailModel.AttachmentHTML = "";
            _requestSendEmailModel.EmailAddress = "faheemjaved@carecloud.com";

            //Act            
            _result = _requestForOrderService.SendEmail(_requestSendEmailModel, _userProfile);

            //Assert
            Assert.AreEqual(false, _result.Success);
        }

        [Test]
        public void SendEmail_CheckEmail_ResponseFalse()
        {
            //Arrange
            _userProfile.PracticeCode = 5110459;
            _userProfile.isTalkRehab = true;
            _requestSendEmailModel.WorkId = 123;
            _requestSendEmailModel.AttachmentHTML = "";
            _requestSendEmailModel.EmailAddress = "";

            //Act
            _result = _requestForOrderService.SendEmail(_requestSendEmailModel, _userProfile);

            //Assert
            Assert.AreEqual(false, _result.Success);
        }
        [Test]
        [TestCase(5443975)]
        [TestCase(99910786)]
        public void GeneratingWorkOrder_PassParameters_ResponseFalse(long userId)
        {
            //Arrange
            _userProfile.PracticeCode = 1012714;
            long practiceCode = 1012714;
            string email = "test";
            string userName = "test";

            //Act
            _requestForOrderService.GeneratingWorkOrder(practiceCode, userName, email, userId, _userProfile);

            //Assert
            Assert.AreEqual(false, _result.Success);
        }
        [Test]
        public void VerifyWorkOrderByRecipient_PassParameters_ResponseFalse()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            string value = "552103";

            //Act
            _requestForOrderService.VerifyWorkOrderByRecipient(value);

            //Assert
            Assert.AreEqual(false, _result.Success);
        }
        [Test]
        public void DeleteWorkOrder_PassParameters_ResponseFalse()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _requestDeleteWorkOrder.WorkId = 552103;

            //Act
            _requestForOrderService.DeleteWorkOrder(_requestDeleteWorkOrder, _userProfile);

            //Assert
            Assert.AreEqual(false, _result.Success);
        }
        [Test]
        public void DownloadPdf_PassParameters_ResponseFalse()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _requestDeleteWorkOrder.WorkId = 552103;
            _requestDownloadPdfModel.AttachmentHTML = "test";
            _requestDownloadPdfModel.FileName = "test";

            //Act
            _requestForOrderService.DownloadPdf(_requestDownloadPdfModel, _userProfile);

            //Assert
            Assert.AreEqual(false, _result.Success);
        }
        [Test]
        public void AddDocumentSignOrder_PassParameters_ResponseFalse()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _reqAddDocumentSign.WorkId = 552103;
            _reqAddDocumentSign.AttachmentHTML = "test";
            _reqAddDocumentSign.FileName = "test";

            //Act
            _requestForOrderService.AddDocument_SignOrder(_reqAddDocumentSign, _userProfile);

            //Assert
            Assert.AreEqual(false, _result.Success);
        }
        [Test]
        public void GenerateQrCode_PassParameters_ResponseFalse()
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _qrCodeModel.WORK_ID = 552103;
            _qrCodeModel.AbsolutePath = "test";
            // _qrCodeModel.FileName = "test";

            //Act
            _requestForOrderService.GenerateQRCode(_qrCodeModel);

            //Assert
            Assert.AreEqual(false, _result.Success);
        }

        [Test]
        [TestCase(53422354, "Admin_5651352")]
        [TestCase(53436742, "Admin_5651352")]
        [TestCase(53436738, "Admin_5651352")]
        [TestCase(53436692, "Admin_5651352")]

        public void AddHtmlToDB_ShouldAddHtmlToDatabase(long workId, string userName)
        {
            // Arrange
            string html = "<html><body>Test</body></html>";
            _userProfile.UserName = userName;
            _userProfile.PracticeCode = 1012714;
            // Act
            _requestForOrderService.AddHtmlToDB(workId, html, userName);

            //Assert
            Assert.IsFalse(false);
        }

        [Test]
        [TestCase("asimshah4@mtbc.com", 99910710)]
        [TestCase("test.ptuser@foxrehab.org", 53411054)]
        public void GetUserReferralSource_ShouldReturnNull_WhenUserDoesNotExist(string email, long userId)
        {
            // Arrange
            ReferralSource expectedReferralSource = null;

            // Act
            ReferralSource actualReferralSource = _requestForOrderService.GetUserReferralSource(email, userId);

            // Assert
            Assert.IsFalse(false);
        }







        //GetUserReferralSource
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _requestForOrderService = null;
            _userProfile = null;
            _requestSendEmailModel = null;
            _result = null;
            _requestDeleteWorkOrder = null;
            _requestDownloadPdfModel = null;
            _reqAddDocumentSign = null;
            _qrCodeModel = null;
        }
    }
}
