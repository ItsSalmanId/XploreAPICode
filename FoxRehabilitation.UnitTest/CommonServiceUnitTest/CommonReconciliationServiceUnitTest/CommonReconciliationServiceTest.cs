using FOX.BusinessOperations.CommonServices;
using FOX.BusinessOperations.RequestForOrder.IndexInformationServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Reconciliation;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.RequestForOrderUnitTest.IndexInformationServiceUnitTest
{
    [TestFixture]
    public class CommonReconciliationServiceTest
    {
        private IndexInformationService _indexInformationService;
        private CommonReconciliationService _CommonReconciliationService;
        private UserProfile _userProfile;
        private readonly DBContextReconciliations _reconciliationCPContext = new DBContextReconciliations();
        private readonly GenericRepository<ReconciliationFiles> _reconciliationFilesRepository;

        [SetUp]
        public void SetUp()
        {
            _indexInformationService = new IndexInformationService();
            _userProfile = new UserProfile();
            _CommonReconciliationService =  new CommonReconciliationService();


        }
        [Test]
        public void SavePdfToImages_ShouldCreateImages()
        {
            // Arrange
            UserProfile profile = new UserProfile();
            string orgPdfPath = @"\\10.10.30.165\FoxDocumentDirectory\Fox\1012714\08-29-2023\OriginalFiles\aa.pdf";
            long reconciliation_ID = 1;
            string reconciliationCategory = "Test";
            int noOfPages = 1;
            int pageCounter = 0;
            int pageCounterOut;
            profile.UserName = "1163testing";


            // Act
            _CommonReconciliationService.SavePdfToImages(profile, orgPdfPath, reconciliation_ID, reconciliationCategory, noOfPages, pageCounter, out pageCounterOut);

            // Assert
          
            Assert.IsNotNull(noOfPages);
        }

        [Test]
        public void Test_tifToImage()
        {
            //Arrange
            UserProfile profile = new UserProfile();
            string orgFilePath = @"\\10.10.30.165\FoxDocumentDirectory\Fox\1012714\08-29-2023\OriginalFiles\recv-fax-1883408.tif"; 
            string imgServerPath = "C:\\test\\imgServerPath";
            string imgDirPath = @"\\10.10.30.165\FoxDocumentDirectory\Fox\1012714\08-29-2023\OriginalFiles\recv-fax-1883408.tif"; 
            long reconciliation_ID = 123;
            string reconciliationCategory = "Test";
            int pageCounter = 1;
            int pageCounterOut;
            profile.UserName = "1163testing";

            //Act
            int imgCount = _CommonReconciliationService.tifToImage(profile, orgFilePath, imgServerPath, imgDirPath, reconciliation_ID, reconciliationCategory, pageCounter, out pageCounterOut);

            //Assert
            Assert.IsNotNull(pageCounter);
        }

        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _indexInformationService = null;
            _userProfile = null;
            _CommonReconciliationService = null;
        }
    }
}
