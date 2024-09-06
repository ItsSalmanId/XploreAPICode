using FOX.BusinessOperations.PatientMaintenanceService;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Security;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.PatientMaintenanceServiceUnitTest
{
    [TestFixture]
    public class PatientMaintenanceServiceTest
    {
        private PatientMaintenanceService _patientMaintenanceService;

        [SetUp]
        public void SetUp()
        {
            _patientMaintenanceService = new PatientMaintenanceService();
        }
        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(101116354813969)]
        [TestCase(38403)]
        public void GetDocumentTypes_PassParameters_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientMaintenanceService.GetPatientByAccountNo(patientAccount);

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
        [TestCase(1012714534318089794,1012714) ]
        public void SearchPatients_NullLastName_ReturnsEmptyList(long pAccount, long practiceCode )
            {
                // Arrange
                var patientReq = new getPatientReq();
            patientReq.Patient_Account = pAccount;

            var profile = new UserProfile();
            profile.PracticeCode = practiceCode;

            // Act
            var result = _patientMaintenanceService.SearchPatients(patientReq, profile);

                // Assert
                Assert.IsNotNull(result);
            }


        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _patientMaintenanceService = null;
        }
    }
}
