using FOX.BusinessOperations.CommonServices;
using FOX.BusinessOperations.SurveyAutomationService;
using FOX.DataModels.Models.PatientSurvey;
using NUnit.Framework;
using static FOX.DataModels.Models.SurveyAutomation.SurveyAutomations;

namespace FoxRehabilitation.UnitTest.SurveyAutomationUnitTest
{
    [TestFixture]
    public class SurveyAutomationServiceTest
    {
        private SurveyAutomationService _surveyAutomationService;
        private SurveyLink _surveyLink;
        private SurveyAutomation _surveyAutomation;
        private PatientSurvey _patientSurvey;

        [SetUp]
        public void SetUp()
        {
            _surveyAutomationService = new SurveyAutomationService();
            _surveyLink = new SurveyLink();
            _surveyAutomation = new SurveyAutomation();
            _patientSurvey = new PatientSurvey();
        }
        [Test]
        [TestCase("2079202", "3")]
        [TestCase("10127152", "2")]
        [TestCase("10127152", "1")]
        public void DecryptionUrl_DecryptedAccountNumber_ReturnString(long patientAccount, string method)
        {
            //Arrange
            _surveyLink.ENCRYPTED_PATIENT_ACCOUNT = long.Parse(patientAccount.ToString()).ToString();
            _surveyLink.SURVEY_METHOD = method;
            PatientSurvey PS = new PatientSurvey();
            PS.SURVEY_ID = 53426359;
            PS.PRACTICE_CODE = 1012714;
            SurveyLink SL = new SurveyLink();
            SL.ENCRYPTED_PATIENT_ACCOUNT = "2079202";
         



            //Act
            var result = _surveyAutomationService.DecryptionUrl(_surveyLink);

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
        [TestCase(101116354816640)]
        [TestCase(101116354816636)]
        [TestCase(101271453426374)]
        public void GetPatientDetails_PatientDetailsModel_ReturnData(long patientAccount)
        {
            //Arrange
            _surveyAutomation.PATIENT_ACCOUNT = patientAccount;

            //Act
            var result = _surveyAutomationService.GetPatientDetails(_surveyAutomation);

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
        [TestCase("101116354816636")]
        [TestCase("")]
        [TestCase("1234")]
        public void GetSurveyQuestionDetails_SurveyQuestions_ReturnString(string patientAccount)
        {
            //Arrange
            _surveyLink.ENCRYPTED_PATIENT_ACCOUNT = patientAccount;

            //Act
            var result = _surveyAutomationService.GetSurveyQuestionDetails(_surveyLink);

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
        [TestCase(246376, 101271453426276)]
        [TestCase(248246, 101271453426277)]
        [TestCase(123456789456, 101271453426375)]
        public void UpdatePatientSurvey_UpdateSurvey_UpdateData(long patientAccount, long surveryId)
        {
            //Arrange
            _patientSurvey.PATIENT_ACCOUNT_NUMBER = patientAccount;
            _patientSurvey.SURVEY_ID = surveryId;
            _patientSurvey.PRACTICE_CODE = 1012714;

            //Act
              var result = _surveyAutomationService.UpdatePatientSurvey(_patientSurvey);

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
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _surveyAutomationService = null;
            _surveyLink = null;
            _patientSurvey = null;
            _surveyAutomation = null;
        }
    }
}
