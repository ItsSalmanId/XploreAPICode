﻿using FOX.BusinessOperations.PatientServices;
using FOX.DataModels.Models.Authorization;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.PatientDocuments;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.ServiceConfiguration;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.TasksModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace FoxRehabilitation.UnitTest.PatientServicesUnitTest
{
    [TestFixture]
    public class PatientServicesTest
    {
        private PatientService _patientService;
        private UserProfile _userProfile;
        private PatientSearchRequest _patientSearchRequest;
        private PatientUpdateHistory _patientUpdateHistory;
        private SmartSearchInsuranceReq _smartSearchInsuranceReq;
        private Patient _patient;
        private PatientContact _patientContact;
        private FacilityLocation _facilityLocation;
        private SSNExist _ssnExist;
        private PatientExist _patientExist;
        private SubscriberSearchReq _subscriberSearchReq;
        private EmployerSearchReq _employerSearchReq;
        private AdvanceInsuranceSearch _advanceInsuranceSearch;
        private AdvancePatientSearch _advancePatientSearch;
        private DefaultInsuranceParameters _defaultInsuranceParameters;
        private SubscriberInfoRequest _subscriberInfoRequest;
        private AutoPopulateModel _autoPopulateModel;
        private SuggestedMCPayer _suggestedMCPayer;
        private SmartSearchCountriesReq _smartSearchCountriesReq;
        private PatientPOSLocation _patientPOSLocation;
        private SmartOrderSource _smartOrderSource;
        private PatientEligibilitySearchModel _patientEligibilitySearchModel;
        private PatientInsuranceInformation _patientInsuranceInformation;
        private PatientInsurance _patientInsurance;
        private MedicareLimit _medicareLimit;
        private MedicareLimitHistorySearchReq _medicareLimitHistorySearchReq;
        private PHR _phr;
        private FOX_TBL_TASK _fOX_TBL_TASK;
        private PatientAlias _patientAlias;
        private CheckDuplicatePatientsReq _checkDuplicatePatientsReq;
        private Subscriber _subscriber;
        private PayorDataModel _payorDataModel;
        private PatientPATDocument _patientPatDocument;
        private ServiceConfiguration _serviceConfiguration;
        private PatientInsuranceAuthDetails _patientInsuranceAuthDetails;
        private FOX_TBL_AUTH_CHARGES _foxTblAuthCharge;
        private InterfaceSynchModel _interfaceSynchModel;
        private WORK_ORDER_INFO_REQ _workOrderInfoReq;
        private PatientInsuranceDetail _patientInsuranceDetail;

        [SetUp]
        public void SetUp()
        {
            _patientService = new PatientService();
            _userProfile = new UserProfile();
            _patientSearchRequest = new PatientSearchRequest();
            _patientUpdateHistory = new PatientUpdateHistory();
            _smartSearchInsuranceReq = new SmartSearchInsuranceReq();
            _patient = new Patient();
            _patientContact = new PatientContact();
            _facilityLocation = new FacilityLocation();
            _ssnExist = new SSNExist();
            _patientExist = new PatientExist();
            _subscriberSearchReq = new SubscriberSearchReq();
            _employerSearchReq = new EmployerSearchReq();
            _advanceInsuranceSearch = new AdvanceInsuranceSearch();
            _advancePatientSearch = new AdvancePatientSearch();
            _defaultInsuranceParameters = new DefaultInsuranceParameters();
            _subscriberInfoRequest = new SubscriberInfoRequest();
            _autoPopulateModel = new AutoPopulateModel();
            _suggestedMCPayer = new SuggestedMCPayer();
            _smartSearchCountriesReq = new SmartSearchCountriesReq();
            _patientPOSLocation = new PatientPOSLocation();
            _smartOrderSource = new SmartOrderSource();
            _patientEligibilitySearchModel = new PatientEligibilitySearchModel();
            _patientInsuranceInformation = new PatientInsuranceInformation();
            _patientInsurance = new PatientInsurance();
            _medicareLimit = new MedicareLimit();
            _medicareLimitHistorySearchReq = new MedicareLimitHistorySearchReq();
            _phr = new PHR();
            _fOX_TBL_TASK = new FOX_TBL_TASK();
            _patientAlias = new PatientAlias();
            _checkDuplicatePatientsReq = new CheckDuplicatePatientsReq();
            _subscriber = new Subscriber();
            _payorDataModel = new PayorDataModel();
            _patientPatDocument = new PatientPATDocument();
            _serviceConfiguration = new ServiceConfiguration();
            _patientInsuranceAuthDetails = new PatientInsuranceAuthDetails();
            _foxTblAuthCharge = new FOX_TBL_AUTH_CHARGES();
            _interfaceSynchModel = new InterfaceSynchModel();
            _workOrderInfoReq = new WORK_ORDER_INFO_REQ();
            _patientInsuranceDetail = new PatientInsuranceDetail();

        }
        [Test]
        [TestCase(1011163, false, 101116354816630)]
        [TestCase(1011163, false, 101116354816001)]
        public void GetPatientList_PatientListModel_ReturnData(long practiceCode, bool isTalkRehab, long Patient_Account)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSearchRequest.Patient_Account = "";
            _patientSearchRequest.FirstName = "";
            _patientSearchRequest.MiddleName = "";
            _patientSearchRequest.LastName = "";
            _patientSearchRequest.MRN = "";
            _patientSearchRequest.SSN = "";
            _patientSearchRequest.CreatedBy = "";
            _patientSearchRequest.ModifiedBy = "";
            _patientSearchRequest.CurrentPage = 1;
            _patientSearchRequest.RecordPerPage = 10;
            _patientSearchRequest.SearchText = "";
            _patientSearchRequest.SortBy = "";
            _patientSearchRequest.SortOrder = "";
            _patientSearchRequest.INCLUDE_ALIAS = true;
            _userProfile.isTalkRehab = isTalkRehab;
            _patientSearchRequest.DOBInString = Convert.ToString(DateTime.Today);
            _patientSearchRequest.CreatedDateInString = Convert.ToString(DateTime.Today);
            _patientSearchRequest.Patient_Account = Patient_Account.ToString();


            //Act
            var result = _patientService.GetPatientList(_patientSearchRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354813049)]
        [TestCase(101116354412270)]
        public void GetPatientAddress_PatientAddressModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientAddress(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354813049)]
        [TestCase(101116354412270)]
        public void GetPatientAddressesToDisplay_PatientAddressesToDisplayModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientAddressesToDisplay(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354412221)]
        [TestCase(101116354412224)]
        public void GetPatientUpdateHistory_PatientUpdateHistoryModel_ReturnData(long PATIENT_ACCOUNT)
        {
            //Arrange
            _patientUpdateHistory.PATIENT_ACCOUNT = PATIENT_ACCOUNT;
            //Act
            var result = _patientService.GetPatientUpdateHistory(_patientUpdateHistory);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("06404")]
        [TestCase("-00544")]
        public void GetCityStateByZip_CityStateByZipModel_ReturnData(string zipCode)
        {
            //Arrange
            //Act
            var result = _patientService.GetCityStateByZip(zipCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("Alaska")]
        [TestCase("Atlantic")]
        [TestCase("Eastern")]
        [TestCase("Chamorro")]
        [TestCase("Marshall")]
        [TestCase("Pacific")]
        [TestCase("Central")]
        [TestCase("Mountain")]
        [TestCase("Pohnpei")]
        [TestCase("default")]
        public void GetTimeZoneName_ReturnTimeZoneName_ReturnData(string timeZone)
        {
            //Arrange
            //Act
            var result = _patientService.GetTimeZoneName(timeZone);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("06404", 1011163)]
        [TestCase("-00544", 1011163)]
        [TestCase("", 1011163)]
        public void GetRegionByZip_RegionByZipModel_ReturnData(string zipCode, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientService.GetRegionByZip(zipCode, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("06404")]
        [TestCase("-00544")]
        public void GetCitiesByZip_CitiesByZipModel_ReturnData(string zipCode)
        {
            //Arrange
            //Act
            var result = _patientService.GetCitiesByZip(zipCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("AUTO CLUB INSURANCE ASSOC")]
        [TestCase("AMERICAN CASUALTY COMPANY OF READING PA")]
        public void GetInsurancePayers_InsurancePayersModel_ReturnData(string insurance_Name)
        {
            //Arrange
            _smartSearchInsuranceReq.Insurance_Name = insurance_Name;
            _smartSearchInsuranceReq.Patient_Account = "101116354412334";

            //Act
            var result = _patientService.GetInsurancePayers(_smartSearchInsuranceReq);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        [TestCase(101116354813049)]
        [TestCase(101116354610685)]
        public void checkPatientisInterfaced_PatientisInterfacedCheck_ReturnData(long patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;

            //Act
            var result = _patientService.checkPatientisInterfaced(patientAccount, _userProfile);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354816561)]
        [TestCase(101116354610685)]
        public void GetRestOfPatientData_ReturnPatientDetails_ReturnData(long patientAccount)
        {
            //Arrange
            _patient.Patient_Account = patientAccount;

            //Act
            var result = _patientService.GetRestOfPatientData(_patient);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SetFlags_Flags_ReturnData(bool flag)
        {
            //Arrange
            _patientContact.Flag_Financially_Responsible_Party = flag;
            _patientContact.Flag_Emergency_Contact = flag;
            _patientContact.Flag_Lives_In_Household_SLC = flag;
            _patientContact.Flag_Power_Of_Attorney = flag;
            _patientContact.Flag_Power_Of_Attorney_Financial = flag;
            _patientContact.Flag_Power_Of_Attorney_Medical = flag;
            _patientContact.Flag_Preferred_Contact = flag;
            _patientContact.Flag_Service_Location = flag;

            //Act
            var result = _patientService.SetFlags(_patientContact);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354816561)]
        [TestCase(101116354610685)]
        public void GetPatientContactTypes_EmptyModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientContactTypes(practiceCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354816561, false)]
        [TestCase(101116354610685, true)]
        public void GetPatientBestTimeToCall_EmptyModel_ReturnData(long practiceCode, bool isTalkRehab)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientBestTimeToCall(practiceCode, isTalkRehab);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, true)]
        [TestCase(1011163, false)]
        [TestCase(102, false)]
        public void GetAllPatientContactTypes_AllPatientContactTypesModel_ReturnData(long practiceCode, bool isTalkRehab)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.isTalkRehab = isTalkRehab;
            //Act
            var result = _patientService.GetAllPatientContactTypes(_userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(544101)]
        [TestCase(544102)]
        public void GetPatientContactDetails_PatientContactDetailsModel_ReturnData(long contactid)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientContactDetails(contactid);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "101116354816561")]
        [TestCase(1011163, "101116354412338")]
        public void SsnExists_SsnExistsModel_ReturnData(long practiceCode, string patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _ssnExist.Patient_Account = patientAccount;

            //Act
            var result = _patientService.SSNExists(_ssnExist, _userProfile);

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
        [TestCase(2456124, "101116354816561", 1011163, false)]
        [TestCase(1201254, "101116354412338", 1011163, false)]
        public void UpdatePrimaryPhysicianInCases_CaseModel_NoReturnData(long PCP_ID, long Patient_Account, long practiceCode, bool isTalkRehab)
        {
            //Arrange

            //Act
            _patientService.UpdatePrimaryPhysicianInCases(PCP_ID, Patient_Account, practiceCode, isTalkRehab);

            //Assert
            Assert.IsTrue(true);

        }
        [Test]
        [TestCase("", false, 000000)]
        [TestCase("1163Testing", true, 145252665)]
        [TestCase("1163Testing", true, 101116356510281)]
        [TestCase("1163Testing", false, 101116356510281)]
        [TestCase("", true, 101116356510281)]
        [TestCase("", false, 101116356510281)]
        public void SaveRestOfPatientDetails_PatientModel_NoReturnData(string username, bool isTalkRehab, long patient_account)
        {
            //Arrange
            _patient.Patient_Account = patient_account;
            _patient.Address = "";
            _patient.City = "";
            _patient.FirstName = "";
            _patient.MIDDLE_NAME = "";
            _patient.Last_Name = "";
            _patient.USER_NAME = "";
            _patient.Date_Of_Birth = DateTime.Now;

            //Act
            _patientService.SaveRestOfPatientDetails(_patient, username, isTalkRehab);


            //Assert
            Assert.IsTrue(true);

        }
        [Test]
        [TestCase(1011163, "101116354816561")]
        [TestCase(1011163, "101116354412338")]
        public void PatientExists_PatientExistsModel_ReturnData(long practiceCode, string patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientExist.Patient_Account = patientAccount;
            _patientExist.First_Name = "";
            _patientExist.Middle_Name = "";
            _patientExist.Last_Name = "";

            //Act
            var result = _patientService.PatientExists(_patientExist, _userProfile);

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
        [TestCase(1011163, "101116354816561")]
        [TestCase(1011163, "101116354412338")]
        public void PatientDemographicsExists_PatientDemographicsExistsModel_ReturnData(long practiceCode, string patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientExist.Patient_Account = patientAccount;
            _patientExist.First_Name = "one";
            _patientExist.Middle_Name = "one";
            _patientExist.Last_Name = "one";

            //Act
            var result = _patientService.PatientDemographicsExists(_patientExist, _userProfile);

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
        [TestCase(544101)]
        [TestCase(00000000)]
        public void GetFoxPayorName_ReturnFoxPayorName_ReturnData(long insuranceId)
        {
            //Arrange
            //Act
            var result = _patientService.GetFoxPayorName(insuranceId);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354815798)]
        [TestCase(101116354813344)]
        public void GetPrimaryInsurance_PrimaryInsuranceName_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPrimaryInsurance(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354815798)]
        [TestCase(101116354813344)]
        public void GetLatestPrimaryInsurance_LatestPrimaryInsuranceName_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetLatestPrimaryInsurance(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "")]
        [TestCase(1011163, "test")]
        public void GetSubscribers_SubscribersModel_ReturnData(long practiceCode, string searchValue)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _subscriberSearchReq.SEARCHVALUE = searchValue;

            //Act
            var result = _patientService.GetSubscribers(_subscriberSearchReq, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "")]
        [TestCase(1011163, "test")]
        public void GetEmployers_EmployersModel_ReturnData(long practiceCode, string searchValue)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _employerSearchReq.SEARCHVALUE = searchValue;

            //Act
            var result = _patientService.GetEmployers(_employerSearchReq, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354815798)]
        [TestCase(101116354813344)]
        public void GetPatientCasesForDD_PatientCasesForDDModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientCasesForDD(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354815798)]
        [TestCase(101116354813344)]
        public void GetPatientCasesForPR_PatientCasesForPRModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientCasesForPR(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("P", true)]
        [TestCase("S", true)]
        [TestCase("T", true)]
        [TestCase("Q", true)]
        [TestCase("PR", true)]
        [TestCase("PR", false)]
        [TestCase("default", false)]
        public void GetInsuraneTypeName_InsuraneTypeNameRModel_ReturnData(string priSecOthType, bool isPP)
        {
            //Arrange
            //Act
            var result = _patientService.GetInsuraneTypeName(priSecOthType, isPP);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("1011163", true)]
        [TestCase("0", false)]
        public void GetFinancialClassDDValues_FinancialClassDDValuesModel_ReturnData(string practiceCode, bool isTalkRehab)
        {
            //Arrange
            //Act
            var result = _patientService.GetFinancialClassDDValues(practiceCode, isTalkRehab);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "Connecticut Claims", "Indianapolis", "IN", "462066185")]
        [TestCase(1011163, "", "", "", "")]
        [TestCase(1011163, "PO Box 3030", "Mechanicsburg", "PA", "170551802")]
        public void GetFinancialClassDDValues_InsurancePayersForAdvanceSearchModel_ReturnData(long practiceCode, string insuranceAddress, string insuranceCity, string insuranceState, string insuranceZip)
        {
            //Arrange
            _advanceInsuranceSearch.Practice_Code = practiceCode;
            _advanceInsuranceSearch.CurrentPage = 1;
            _advanceInsuranceSearch.RecordPerPage = 10;
            _advanceInsuranceSearch.Insurance_Address = insuranceAddress;
            _advanceInsuranceSearch.Insurance_City = insuranceCity;
            _advanceInsuranceSearch.Insurance_State = insuranceState;
            _advanceInsuranceSearch.Insurance_Zip = insuranceZip;
            _advanceInsuranceSearch.SearchString = "test";
            _advanceInsuranceSearch.FINANCIAL_CLASS_ID = 1;

            //Act
            var result = _patientService.GetInsurancePayersForAdvanceSearch(_advanceInsuranceSearch);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "One", "Test")]
        [TestCase(1011163, "", "")]
        [TestCase(1011163, "Test", "Test")]
        public void GetPatientsForAdvanceSearch_PatientsForAdvanceSearchModel_ReturnData(long practiceCode, string firstName, string lastName)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _advancePatientSearch.CurrentPage = 1;
            _advancePatientSearch.RecordPerPage = 10;
            _advancePatientSearch.First_Name = firstName;
            _advancePatientSearch.Last_Name = lastName;
            _advancePatientSearch.Date_Of_Birth_In_String = Convert.ToString(DateTime.Today);
            _advancePatientSearch.Created_Date_Str = Convert.ToString(DateTime.Today);

            //Act
            var result = _patientService.GetPatientsForAdvanceSearch(_advancePatientSearch, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354816561)]
        [TestCase(10111635481861)]
        public void GetPatientDetail_PatientDetailModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientDetail(patientAccount);

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
        [TestCase(1011163, "")]
        [TestCase(1011163, "Test")]
        public void GetSmartPatient_SmartPatientModel_ReturnData(long practiceCode, string searchText)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientService.GetSmartPatient(searchText, _userProfile);

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
        [TestCase(1011163, "")]
        [TestCase(1011163, "Test")]
        public void GetSmartPatientForTask_SmartPatientForTaskModel_ReturnData(long practiceCode, string searchText)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientService.GetSmartPatientForTask(searchText, _userProfile);

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
        [TestCase(1011163, " - ")]
        [TestCase(1011163, "Test")]
        public void GetSmartModifiedBy_SmartModifiedByModel_ReturnData(long practiceCode, string searchText)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientService.getSmartModifiedBy(searchText, _userProfile);

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
        [TestCase(1011163, "170551802", "PA")]
        [TestCase(1011163, "462066185", "IN")]
        public void GetDefaultPrimaryInsurance_EmptyModel_ReturnData(long practiceCode, string zip, string state)
        {
            //Arrange
            _defaultInsuranceParameters.ZIP = zip;
            _defaultInsuranceParameters.State = state;
            _defaultInsuranceParameters.patientAccount = 101116354816561;

            //Act
            var result = _patientService.GetDefaultPrimaryInsurance(_defaultInsuranceParameters, practiceCode);

            //Assert
            if (result == null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(101116354816561)]
        [TestCase(10111635481861)]
        public void GetSubscriberInfo_SubscriberInfoModel_ReturnData(long patientAccount)
        {
            //Arrange
            _subscriberInfoRequest.patientAccount = patientAccount;

            //Act
            var result = _patientService.GetSubscriberInfo(_subscriberInfoRequest);

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
        [TestCase("101116354816561", 1011163, "Aetna Medicare Advantage")]
        [TestCase("10111635481861", 1011163, "Novitas Medicare NJ1")]
        public void GetSmartInsurancePayers_SmartInsurancePayersModel_ReturnData(string patientAccount, long practiceCode, string insuranceName)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _smartSearchInsuranceReq.Insurance_Name = insuranceName;
            _smartSearchInsuranceReq.Patient_Account = patientAccount;
            _smartSearchInsuranceReq.FINANCIAL_CLASS_ID = 0;
            _smartSearchInsuranceReq.Pri_Sec_Oth_Type = "";
            _smartSearchInsuranceReq.Zip = "";
            _smartSearchInsuranceReq.State = "";

            //Act
            var result = _patientService.GetSmartInsurancePayers(_smartSearchInsuranceReq, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163)]
        public void GetAutoPopulateInsurance_GetAutoPopulateInsuranceModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _autoPopulateModel.patientAccount = "101116354412362";
            _autoPopulateModel.CASE_ID = 544107;
            _autoPopulateModel.patientAccount = "0";
            _autoPopulateModel.Pri_Sec_Oth_Type = "0";


            //Act
            var result = _patientService.GetAutoPopulateInsurance(_autoPopulateModel, _userProfile);

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
        [TestCase("101116354816561", "GA", 1011163)]
        [TestCase("101116354412309", "IN", 1011163)]
        [TestCase("", "", 1011163)]
        public void GetPatientPrivateHomes_EmptyPatientPrivateHomesModel_ReturnData(string patientAccount, string stateCode, long practiceCode)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientPrivateHomes(patientAccount, stateCode, practiceCode);

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
        [TestCase(1011163, "99503", "AK")]
        [TestCase(1011163, "10968", "NY")]
        [TestCase(1011163, "85339", "AZ")]
        public void GetSuggestedMCPayer_SuggestedMCPayerModel_ReturnData(long practiceCode, string zip, string state)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _suggestedMCPayer.Zip = zip;
            _suggestedMCPayer.State = state;

            //Act
            var result = _patientService.GetSuggestedMCPayer(_suggestedMCPayer, _userProfile);

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
        [TestCase(1011163, "101116354816561")]
        [TestCase(1011163, "101116354412309")]
        [TestCase(1011163, "10111635441309")]
        public void GetWorkOrderDocs_WorkOrderDocsModel_ReturnData(long practiceCode, string patientAccountStr)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientService.GetWorkOrderDocs(patientAccountStr, _userProfile);

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
        [TestCase(1011163, "101116354812860")]
        [TestCase(1011163, "101116354813049")]
        [TestCase(1011163, "101116354813475")]
        public void GetPatientInviteStatus_PatientInviteStatusModel_ReturnData(long practiceCode, string patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientService.GetPatientInviteStatus(patientAccount, _userProfile);

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
        [TestCase(101116354814556)]
        [TestCase(101116354814574)]
        [TestCase(101116354814575)]
        public void GetPatientAliasListForSpecificPatient_PatientAliasListForSpecificPatientModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientAliasListForSpecificPatient(patientAccount);

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
        [TestCase("Afghanistan")]
        [TestCase("Austria")]
        [TestCase(null)]
        [TestCase("-Austria")]
        public void GetCountries_CountriesModel_ReturnData(string searchValue)
        {
            //Arrange
            _smartSearchCountriesReq.SEARCHVALUE = searchValue;

            //Act
            var result = _patientService.getCountries(_smartSearchCountriesReq);

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
        [TestCase(1011222)]
        [TestCase(1011163)]
        public void GetAllCountries_AllCountriesModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _patientService.GetAllCountries(practiceCode);

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
        [TestCase(544101)]
        [TestCase(6002572)]
        public void GetInsuranc_InsurancModel_ReturnData(long id)
        {
            //Arrange
            //Act
            var result = _patientService.GetInsuranc(id, _userProfile);

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
        [TestCase(101116354816000)]
        [TestCase(101116354816561)]
        public void GetPatientAddressesIncludingPOS_PatientAddressesIncludingPOSe_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientAddressesIncludingPOS(patientAccount);

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
        [TestCase(1012714534318094156)]
        [TestCase(1012714534318094156)]
        public void GetPatientInsurance_GetPatientInsurance_ReturnData(long patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = 1012714;
            _userProfile.UserName = "Davis_53411401";

            //Act
            var result = _patientService.GetPatientInsurance(patientAccount, _userProfile);

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
        [TestCase(1012714534318094156)]
        [TestCase(1012714534318094156)]
        public void CheckNecessaryDataForEligibility_DataEligibilityCheck_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.CheckNecessaryDataForEligibility(patientAccount);

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
        [TestCase("1012714534318094156", 53419820903964)]
        [TestCase("1012714534318094156", 53419820903965)]
        [TestCase("101116354816562", null)]
        public void CheckNecessaryDataForLoadEligibility_DataEligibilityCheck_ReturnData(string patientAccount, long patientInsuranceId)
        {
            //Arrange
            _patientEligibilitySearchModel.Patient_Account_Str = patientAccount;
            _patientEligibilitySearchModel.Patient_Insurance_id = patientInsuranceId;

            //Act
            var result = _patientService.CheckNecessaryDataForLoadEligibility(_patientEligibilitySearchModel);

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
        [TestCase("")]
        [TestCase("Test")]
        public void RemoveStyleNodeFromHtmlForMVP_RemoveStyle_ReturnData(string html)
        {
            //Arrange
            //Act
            var result = _patientService.RemoveStyleNodeFromHtmlForMVP(html);

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
        [TestCase("*//div[contains(@id,'main-container-eligibility')]//div[contains(@class,'custom-panel-head')][1]//h3[1]")]
        [TestCase("Test")]
        public void UpdatePayerNameInHTML_UpdatePayerNameInHTML_ReturnData(string html)
        {
            //Arrange
            //Act
            var result = _patientService.UpdatePayerNameInHTML(html, _patientInsuranceInformation);

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
        [TestCase(101271460537654)]
        [TestCase(101271460617791)]
        public void GetCurrentPatientDemographics_PassParameters_ReturnData(long patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _patientService.GetCurrentPatientDemographics(patientAccount, _userProfile);

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
        [TestCase(101116354818050, "private home")]
        [TestCase(101116354817999, "private home")]
        public void UpdateCoordinates_PassParameters_ReturnData(long patientAccount, string name)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientPOSLocation.Patient_Account_Str = patientAccount.ToString();
            _patientPOSLocation.Effective_From_In_String = "01/01/2000";
            _patientPOSLocation.Effective_To_In_String = "01/01/2000";
            _patientPOSLocation.IsNew = true;
            _patientPOSLocation.Patient_POS_Details = new FacilityLocation()
            {
                LOC_ID = 123,
                Phone = "12345",
                NAME = name,
                Zip = "12345"
            };

            //Act
            var result = _patientService.AddUpdatePatientPOSLocation(_patientPOSLocation, _userProfile);

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
        [TestCase(1012714534318094111, "ABC")]
        public void UpdateCoordinates_PassParameters(long patientAccount, string name)
        {
            //Arrange
            _userProfile.PracticeCode = 1012714;
            _userProfile.UserName = "1163testing";
            _patientPOSLocation.Patient_Account_Str = patientAccount.ToString();
            _patientPOSLocation.Effective_From_In_String = "01/01/2000";
            _patientPOSLocation.Effective_To_In_String = "01/01/2000";
            _patientPOSLocation.IsAddition = true;
            _patientPOSLocation.IsNew = false;
            _patientPOSLocation.Patient_POS_Details = new FacilityLocation()
            {
                LOC_ID = 6051794,
                Phone = "12345",
                NAME = name,
                Zip = "12345",
                Address = "ABC TESTING"
            };

            //Act
            var result = _patientService.AddUpdatePatientPOSLocation(_patientPOSLocation, _userProfile);

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
        [TestCase(1012714534318094111, "ABC")]
        public void UpdateCoordinates_PassParameters_Updated(long patientAccount, string name)
        {
            //Arrange
            _userProfile.PracticeCode = 1012714;
            _userProfile.UserName = "1163testing";
            _patientPOSLocation.Patient_Account_Str = patientAccount.ToString();
            _patientPOSLocation.Effective_From_In_String = "01/01/2000";
            _patientPOSLocation.Effective_To_In_String = "01/01/2000";
            _patientPOSLocation.IsAddition = false;
            _patientPOSLocation.IsNew = false;
            _patientPOSLocation.IsUpdation = true;
            _patientPOSLocation.Patient_POS_Details = new FacilityLocation()
            {
                LOC_ID = 6051794,
                Phone = "12345",
                NAME = name,
                Zip = "12345",
                Address = "ABC TESTING",
                Is_Default = true

            };
            _patientPOSLocation.Patient_POS_ID = 53499958312818;

            //Act
            var result = _patientService.AddUpdatePatientPOSLocation(_patientPOSLocation, _userProfile);

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
        [TestCase(101116354818050, "Test")]
        public void UpdateCoordinates_PassParameters_isNew(long patientAccount, string name)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientPOSLocation.Patient_Account_Str = patientAccount.ToString();
            _patientPOSLocation.Effective_From_In_String = "01/01/2000";
            _patientPOSLocation.Effective_To_In_String = "01/01/2000";
            _patientPOSLocation.IsNew = true;
            _patientPOSLocation.Patient_POS_Details = new FacilityLocation()
            {
                LOC_ID = 112562,
                Phone = "12345",
                NAME = name,
                Zip = "12345"
            };

            //Act
            var result = _patientService.AddUpdatePatientPOSLocation(_patientPOSLocation, _userProfile);

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
        [TestCase(101116354818050, "Test")]
        public void UpdateCoordinates_PassParameters_isAddition(long patientAccount, string name)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientPOSLocation.Patient_Account_Str = patientAccount.ToString();
            _patientPOSLocation.Effective_From_In_String = "01/01/2000";
            _patientPOSLocation.Effective_To_In_String = "01/01/2000";
            _patientPOSLocation.IsAddition = true;
            _patientPOSLocation.Patient_POS_Details = new FacilityLocation()
            {
                LOC_ID = 112562,
                Phone = "12345",
                NAME = name,
                Zip = "12345"
            };

            //Act
            var result = _patientService.AddUpdatePatientPOSLocation(_patientPOSLocation, _userProfile);

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
        [TestCase(1, 101271460558217311, 605100)]
        public void UpdateDefaultLocations_PassParameters_ReturnData(long defaultLocId, long patAcc, long posId)
        {
            //Arrange
            //Act
            _patientService.UpdateDefaultLocations(defaultLocId, patAcc, posId);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("101116354817999")]
        public void CheckAndUpdatePrSubscriber_UpdatePayerNameInHTML_ReturnData(string patientAccount)
        {
            //Arrange
            //Act
            _patientService.CheckAndUpdatePRSubscriber(patientAccount, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(112562)]
        [TestCase(600102)]
        public void GetCoordinates_Coordinates_ReturnData(long locId)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _facilityLocation.LOC_ID = locId;

            //Act
            var result = _patientService.AddFacilityLocation(_facilityLocation, _userProfile);

            //Assert
            if (result != 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163605232776)]
        [TestCase(19991)]
        [TestCase(199445)]
        [TestCase(544101)]
        public void SaveContact_Contact_ReturnData(long contactID)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientContact.Start_Date_In_String = "8/12/2000";
            _patientContact.End_Date_In_String = "8/12/2024";
            _patientContact.Patient_Account_Str = "";
            _patientContact.STATEMENT_ADDRESS_MARKED = false;
            _patientContact.Flag_Financially_Responsible_Party = false;
            _patientContact.Contact_ID = contactID;
            _userProfile.isTalkRehab = true;

            //Act
            var result = _patientService.SaveContact(_patientContact, _userProfile);

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
        [TestCase(19991, 600109)]
        [TestCase(544101, 600115)]
        public void SaveContact_Contact_ReturnData_isTalkehr(long contactID, long contactTypeId)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientContact.Start_Date_In_String = "8/12/2000";
            _patientContact.End_Date_In_String = "8/12/2024";
            _patientContact.Patient_Account_Str = "";
            _patientContact.STATEMENT_ADDRESS_MARKED = false;
            _patientContact.Flag_Financially_Responsible_Party = false;
            _patientContact.Contact_ID = contactID;
            _userProfile.isTalkRehab = true;
            _patientContact.Contact_Type_Id = contactTypeId;

            //Act
            var result = _patientService.SaveContact(_patientContact, _userProfile);

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
        [TestCase(53412283, 605142)]
        [TestCase(544101, 600115)]
        public void SaveContact_Contact_ReturnData_null(long contactID, long contactTypeId)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientContact.Contact_ID = contactID;
            _patientContact.Contact_Type_Id = contactTypeId;
            _userProfile.isTalkRehab = true;

            //Act
            var result = _patientService.SaveContact(_patientContact, _userProfile);

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
        [TestCase(101116354412336)]
        public void GetPatientContacts_PatientContactsModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientContacts(patientAccount);

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
        [TestCase(101271499910054)]
        public void GetPatientContactsForInsurance_PatientContactsForInsuranceModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientContactsForInsurance(patientAccount);

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
        [TestCase(1012714534318093880)]
        [TestCase(1012714534318093399)]
        public void GetCurrentPatientInsurances_CurrentPatientInsurancesModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetCurrentPatientInsurances(patientAccount, _userProfile);

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
        [TestCase(101116354412336)]
        [TestCase(101116354816561)]
        public void CreateUpdateInsuranceInMTBC_UpdateModel_ReturnData(long patientAccount)
        {
            //Arrange
            _patientInsurance.MTBC_Patient_Insurance_Id = 5174;
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientInsurance.Pri_Sec_Oth_Type = "Q";
            _patientInsurance.Effective_Date = DateTime.Today;
            _patientInsurance.Termination_Date = DateTime.Today;
            _patientInsurance.Policy_Number = "test";
            _patientInsurance.Plan_Name = "test";
            _patientInsurance.Relationship = "test";
            _patientInsurance.Eligibility_Status = "test";

            //Act
            var result = _patientService.CreateUpdateInsuranceInMTBC(_patientInsurance, patientAccount, _userProfile);

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
        [TestCase(123456, "ABN")]
        [TestCase(123456, "Hospice")]
        [TestCase(544100, "ABN")]
        public void CreateNewLimit_NewLimit_ReturnData(long? oldLimID, string limType)
        {
            //Arrange
            _medicareLimit.Patient_Account = 101116354815798;
            _medicareLimit.CASE_ID = 12345678;
            _medicareLimit.MODIFIED_BY = Convert.ToString(DateTime.Today);
            _medicareLimit.EFFECTIVE_DATE_IN_STRING = Convert.ToString(DateTime.Today);
            //END_DATE   ABN_EST_WK_COST   ABN_COMMENTS
            _userProfile.UserName = "1163testing";

            // EFFECTIVE_DATE_IN_STRING
            //Act
            var result = _patientService.CreateNewLimit(oldLimID, _medicareLimit, limType, _userProfile);

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
        [TestCase(544100, "ABN")]
        [TestCase(544100, "Hospice")]
        [TestCase(544100, "")]
        public void ABN_MedicareLimitDataChanged_DataChanged_ReturnData(long? newCaseId, string medicareLimitTypeName)
        {
            //Arrange

            _medicareLimit.MEDICARE_LIMIT_TYPE_NAME = medicareLimitTypeName;
            _medicareLimit.CASE_ID = 544100;
            _medicareLimit.EFFECTIVE_DATE = DateTime.Today;
            _medicareLimit.END_DATE = DateTime.Today;
            _medicareLimit.ABN_EST_WK_COST = 0;
            _medicareLimit.ABN_COMMENTS = "ABN";
            _medicareLimit.NPI = "ABN";

            //Act
            var result = _patientService.MedicareDataChanged(_medicareLimit, _medicareLimit, newCaseId);

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
        public void FetchEligibilityRecords_FetchRecords_ReturnData()
        {
            //Arrange 
            _medicareLimitHistorySearchReq.Patient_Account = "101116354815798";
            _medicareLimitHistorySearchReq.MedicareLimitTypeId = 544100;
            _userProfile.PracticeCode = 1011163;

            //Act
            var result = _patientService.GetMedicareLimitHistory(_medicareLimitHistorySearchReq, _userProfile);

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
        [TestCase(548350)]
        [TestCase(544139)]
        public void Getsplitauthorizations_splitauthorization_ReturnData(long parentid)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;

            //Act
            var result = _patientService.getsplitauthorization(parentid, _userProfile);

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
        [TestCase("Test")]
        [TestCase("")]
        public void GetPrivateHomeFacilityByCode_PrivateHomeFacilityByCode_ReturnData(string code)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _patientService.GetPrivateHomeFacilityByCode(code, _userProfile);

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
        [TestCase("SP", 101271499910024, "male")]
        [TestCase("SP", 101271499910024, "female")]
        [TestCase("C", 101271499910025, "female")]
        [TestCase("C", 101271499910043, "female")]
        public void SavePatientContactfromInsuranceSubscriber_AddToDb_ReturnData(string relation, long patientAccount, string gender)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _subscriber.GUARANT_GENDER = gender;

            //Act
            _patientService.SavePatientContactfromInsuranceSubscriber(_subscriber, relation, patientAccount, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void ExportPatientListToExcel_ExportExcel_ReturnData()
        {
            //Arrange 
            _phr.EMAIL_ADDRESS = "babarazam@mailinator.com";
            _phr.PRACTICE_CODE = 1011163;
            _userProfile.PracticeCode = 1011163;

            //Act
            var result = _patientService.GetInvitedPatient(_phr, _userProfile);

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
        [TestCase(548158)]
        [TestCase(123456)]
        public void UnBlockPatientFromPHR_UnBlockPatient_ReturnData(long userId)
        {
            //Arrange 
            _phr.EMAIL_ADDRESS = "babarazam@mailinator.com";
            _phr.PRACTICE_CODE = 1011163;
            _userProfile.PracticeCode = 1011163;
            _phr.USER_ID = userId;
            _userProfile.userID = userId;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _patientService.UnBlockPatientFromPHR(_phr, _userProfile);

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
        [TestCase(548158)]
        [TestCase(123456)]
        public void BlockPatientFromPHR_BlockPatient_ReturnData(long userId)
        {
            //Arrange 
            _phr.EMAIL_ADDRESS = "babarazam@mailinator.com";
            _phr.PRACTICE_CODE = 1011163;
            _userProfile.PracticeCode = 1011163;
            _phr.USER_ID = userId;
            _userProfile.userID = userId;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _patientService.BlockPatientFromPHR(_phr, _userProfile);

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
        [TestCase(548158)]
        [TestCase(123456)]
        public void CancelPatientRequestFromPHR_CancelRequest_ReturnData(long userId)
        {
            //Arrange 
            _phr.EMAIL_ADDRESS = "babarazam@mailinator.com";
            _phr.PRACTICE_CODE = 1011163;
            _userProfile.PracticeCode = 1011163;
            _phr.USER_ID = userId;
            _userProfile.userID = userId;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _patientService.CancelPatientRequestFromPHR(_phr, _userProfile);

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
        [TestCase("test")]
        public void AddUpdateTask_TaskAddToDb_ReturnData(string eligibilityMspData)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _fOX_TBL_TASK.PATIENT_ACCOUNT_STR = "1012714";
            _fOX_TBL_TASK.PATIENT_ACCOUNT = 1012714;
            _fOX_TBL_TASK.TASK_ID = 123456;

            //Act
            var result = _patientService.AddUpdateTask(_fOX_TBL_TASK, _userProfile, eligibilityMspData);

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
        [TestCase("test")]
        [TestCase("")]
        public void PrivateHOMExists_PrivateHOMExists_ReturnData(string statecode)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _patientService.PrivateHOMExists(statecode, _userProfile);

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
        [TestCase(null)]
        [TestCase(548100)]
        public void SavePatientAlias_AddData_ReturnData(long patientAliasId)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientAlias.PATIENT_ACCOUNT_STR = "101116354814556";
            _patientAlias.PATIENT_ALIAS_ID = patientAliasId;

            //Act
            var result = _patientService.SavePatientAlias(_patientAlias, _userProfile);

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
        [TestCase("101116354814877")]
        [TestCase("101116354816563")]
        public void CheckDuplicatePatients_CheckDuplicate_ReturnData(string patientAccount)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            _checkDuplicatePatientsReq.PATIENT_ACCOUNT = patientAccount;
            _checkDuplicatePatientsReq.First_Name = "";
            _checkDuplicatePatientsReq.Last_Name = "";
            _checkDuplicatePatientsReq.Date_Of_Birth_In_String = "2000-08-12";
            _checkDuplicatePatientsReq.Gender = "";

            //Act
            var result = _patientService.CheckDuplicatePatients(_checkDuplicatePatientsReq, _userProfile);

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
        [TestCase("101271460617791")]
        public void SearchCityStateAddressByApi_PassParameter_ReturnData(string patientAccount)
        {
            //Arrange 
            _userProfile.PracticeCode = 1012714;
            _userProfile.UserName = "Ahmaad_53411357";
            PatientInfoChecklist pat = new PatientInfoChecklist();
            Subscriber ObjSubscriber = new Subscriber();
            ObjSubscriber.GUARANT_FNAME = "Test";
            ObjSubscriber.GUARANT_LNAME = "Test";
            SubscriberInformation patient_misc_data = new SubscriberInformation();
            patient_misc_data.patientinfo.First_Name = "Test";
            patient_misc_data.patientinfo.Last_Name = "Test";
           patient_misc_data.patientinfo.Address = "";



            //Act
            _patientService.SaveDynamicPatientResponsibilityInsurance(patientAccount, _userProfile);

            //Assert
            Assert.IsFalse(false);
        }
        [Test]
        [TestCase(101116354816563)]
        public void SetTerminationDateForExistingPayers_PassParameter_ReturnData(long claimNo)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            _patientInsurance.InsPayer_Description = "test";
            _patientInsurance.Pri_Sec_Oth_Type = "test";
            _patientInsurance.IS_PRIVATE_PAY = true;

            //Act
            _patientService.AddClaimNotes(_patientInsurance, claimNo, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(101116354816563)]
        public void SaveReconcileLatestData_PassParameter_ReturnData(long claimNo)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            _payorDataModel.PayorZip = "12345";
            _payorDataModel.PayorDOB = "01/01/2000";

            //Act
            _patientService.SaveReconcileLatestData(_payorDataModel, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(101116354816563, 101116354816563, "test")]
        public void SaveEligibilityHtml_PassParameter_ReturnData(long patientAccount, long patientInsuranceId, string html)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            _payorDataModel.PayorZip = "12345";
            _payorDataModel.PayorDOB = "01/01/2000";

            //Act
            _patientService.SaveEligibilityHtml(patientAccount, patientInsuranceId, html, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("101116354816563", 101116354816563)]
        public void FetchEligibilityRecords_PassParameter_ReturnData(string patientAccountStr, long patientInsuranceId)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            _patientEligibilitySearchModel.Patient_Account_Str = patientAccountStr;
            _patientEligibilitySearchModel.Patient_Insurance_id = patientInsuranceId;

            //Act
            _patientService.GetLatestEligibilityRecords(_patientEligibilitySearchModel, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("101116354816563", 101116354816563)]
        public void GetEligibilityInformation_PassParameter_ReturnData(string elgString, long patientAccount)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";

            //Act
            _patientService.GetEligibilityInformation(elgString, patientAccount, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("101116354816563", 101116354816563)]
        public void SaveReconcileLatestData_PassParameter_ReturnData(string elgString, long patientAccount)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            _payorDataModel.PayorDOB = "01/01/2000";

            //Act
            _patientService.SaveReconcileLatestData(_payorDataModel, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(false, 53422214)]
        [TestCase(true, 53422215)]
        [TestCase(true, null)]
        public void AddUpdateNewDocumentInformation_PassParameter_ReturnData(bool newDocument, long workId)
        {
            //Arrange 
            _userProfile.PracticeCode = 1012714;

            _userProfile.UserName = "1163Testing";
            _patientPatDocument.PATIENT_ACCOUNT_str = "101116354816563";
            _patientPatDocument.PATIENT_ACCOUNT = 101116354816563;
            _patientPatDocument.PAT_DOCUMENT_ID = 101116354816563;
            _patientPatDocument.WORK_ID = workId;
            _patientPatDocument.DOCUMENT_TYPE = 54;
            _patientPatDocument.PAT_DOCUMENT_ID = 54;


            //Act
            _patientService.AddUpdateNewDocumentInformation(_patientPatDocument, _userProfile, newDocument);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(53410358, true)]
        public void AbnMedicareLimitDataChanged_PassParameters_ReturnData(long abnId, out bool abnInfoChanged)
        {
            //Arrange 
            List<MedicareLimit> medicareLimits = new List<MedicareLimit>()
            {
             new MedicareLimit
             {
               MEDICARE_LIMIT_TYPE_NAME = "ABN"

             }
            };

            //Act
            _patientService.ABN_MedicareLimitDataChanged(abnId, medicareLimits, out abnInfoChanged);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(544100, true)]
        public void HosMedicareLimitDataChanged_PassParameters_ReturnData(long hosId, out bool abnInfoChanged)
        {
            //Arrange 
            List<MedicareLimit> medicareLimits = new List<MedicareLimit>()
            {
             new MedicareLimit
             {
               MEDICARE_LIMIT_TYPE_NAME = "Hospice"
             }
            };

            //Act
            _patientService.HOS_MedicareLimitDataChanged(hosId, medicareLimits, out abnInfoChanged);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(544100, true)]
        public void HhMedicareLimitDataChanged_PassParameters_ReturnData(long hosId, out bool abnInfoChanged)
        {
            //Arrange 
            List<MedicareLimit> medicareLimits = new List<MedicareLimit>()
            {
             new MedicareLimit
             {
               MEDICARE_LIMIT_TYPE_NAME = "Home Health Episode"
             }
            };

            //Act
            _patientService.HH_MedicareLimitDataChanged(hosId, medicareLimits, out abnInfoChanged);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void FetchEligibilityRecord_PassParameters_ReturnData()
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientInsuranceDetail.Patient_Insurance_Id = 123456;
            _patientInsuranceDetail.MTBC_Patient_Insurance_Id = 54863328635107;

            //Act
            _patientService.DeleteInsuranceInformation(_patientInsuranceDetail, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(@"\\10.10.30.165\FoxDocumentDirectory\Fox\1012714\05-26-2023\OriginalFiles\tempcoversletter638206868960541491.pdf")]
        public void SavePdftoImagesEligibilty_PassParameter_ReturnData(string pdfPath)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _serviceConfiguration.IMAGES_PATH_SERVER = @"\\10.10.30.165\FoxDocumentDirectory\Fox\1012714\05-26-2023\OriginalFiles";
            int noOfPages = 1;
            string sorcetype = "test";
            string patientaccountstr = "123456";
            long patientInsuranceId = 0;
            bool newDocument = true;
            long work_id = 0;

            //Act
            _patientService.SavePdftoImagesEligibilty(pdfPath, _serviceConfiguration, noOfPages, sorcetype, _userProfile, patientaccountstr, patientInsuranceId, newDocument, work_id);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(101271453411656)]
        [TestCase(1011163605232776)]
        [TestCase(1011163605232776)]
        [TestCase(101116354813306)]
        [TestCase(101116354412334)]
        [TestCase(101116354813584)]
        public void GetCurrentPatientAuthorizations_PassParameter_ReturnData(long patientAccount)
        {
            //Arrange 
            _userProfile.PracticeCode = 1012714;
            _userProfile.UserName = "Ahmad_53411370";

            //Act
            _patientService.GetCurrentPatientAuthorizations(patientAccount, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(101116354818276)]
        [TestCase(101116354815798)]
        [TestCase(101116351010069)]
        [TestCase(101116354813306)]
        [TestCase(101116354412334)]
        [TestCase(101116354813584)]
        public void SaveAuthDetails_PassParameters_ReturnData(long patientAccount)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientInsuranceAuthDetails.Patient_Account_Str = patientAccount.ToString();
            _patientInsuranceAuthDetails.AuthToCreateUpdate = new FOX_TBL_AUTH();
            {
                new FOX_TBL_AUTH
                {
                    EFFECTIVE_FROM_IN_STR = "01/01/2023",
                    EFFECTIVE_TO_IN_STR = "01/01/2023",
                    RECORD_TIME_IN_STR = "01/01/2023",
                    REQUESTED_ON_IN_STR = "01/01/2023",
                    EFFECTIVE_FROM = Convert.ToDateTime("01/01/2023"),
                    EFFECTIVE_TO = Convert.ToDateTime("01/01/2023"),
                    REQUESTED_ON = Convert.ToDateTime("01/01/2023"),
                    AUTH_ID = 544139,
                    AUTH_PARENT_ID = 544139,
                };
            };

            //Act
            _patientService.SaveAuthDetails(_patientInsuranceAuthDetails, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(544105)]
        [TestCase(544106)]
        public void SaveAuthAppointments_PassParameters_ReturnData(long authId)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            List<int> authAppointmentIdsList = new List<int>
            {
                  1234,
            };
            long auth_Id = authId;

            //Act
            _patientService.SaveAuthAppointments(authAppointmentIdsList, auth_Id, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(544105, true)]
        [TestCase(544105, false)]
        [TestCase(544106, true)]
        [TestCase(544106, false)]
        [TestCase(12345, false)]
        public void SaveAuthCharges_PassParameters_ReturnData(long authId, bool isSimpleAuth)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            long auth_Id = authId;
            List<FOX_TBL_AUTH_CHARGES> authorizationChargesList = new List<FOX_TBL_AUTH_CHARGES>()
            {
                new FOX_TBL_AUTH_CHARGES
                {
                    AUTH_CHARGES_ID = 544100,
                    CPT_RANGE_FROM_CODE = "test"
                }
            };

            //Act
            _patientService.SaveAuthCharges(authorizationChargesList, auth_Id, isSimpleAuth, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(544105)]
        [TestCase(544106)]
        [TestCase(12345)]
        public void SaveAuthComments_PassParameters_ReturnData(long authId)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientInsuranceAuthDetails.AuthToCreateUpdate = new FOX_TBL_AUTH()
            {
                AUTH_ID = authId,
                AuthComments = "test"
            };

            //Act
            _patientService.SaveAuthComments(_patientInsuranceAuthDetails, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void ChargeObjHasData_PassParameters_ReturnData()
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _foxTblAuthCharge.CPT_RANGE_FROM_CODE = "test";
            _foxTblAuthCharge.CHARGES = "test";
            _foxTblAuthCharge.IS_EXEMPT = true;
            _foxTblAuthCharge.DIAGNOSIS_CODE = "test";

            //Act
            _patientService.ChargeObjHasData(_foxTblAuthCharge);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void InsertInterfaceTeamData_PassParameters_ReturnData()
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _interfaceSynchModel.PATIENT_ACCOUNT = 123456;
            _interfaceSynchModel.CASE_ID = 123456;
            _interfaceSynchModel.TASK_ID = 123456;

            //Act
            _patientService.InsertInterfaceTeamData(_interfaceSynchModel, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }

        [Test]
        [TestCase("544105")]
        [TestCase("544107")]
        [TestCase("5441056")]
        public void GetWorkOrderInfo_PassParameters_ReturnData(string patientAccountStr)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _workOrderInfoReq.PATIENT_ACCOUNT = patientAccountStr;

            //Act
            _patientService.GetWorkOrderInfo(_workOrderInfoReq, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void PrepareLogExport_PassParameters_ReturnData()
        {
            //Arrange 
            List<PatientExportToExcelModel> recordToExport = new List<PatientExportToExcelModel>();
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            List<Patient> obj = new List<Patient>()
            {
                new Patient
                {
                    Patient_Account = 1010624506101096,
                }
            };

            //Act
            _patientService.PrepareLogExport(obj, out recordToExport);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void EligibilityAlreadyLoadedInThisMonth_PassParameter_ReturnData()
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientInsurance.Patient_Account = 101116354816450;
            _patientInsurance.Pri_Sec_Oth_Type = "P";

            //Act
            _patientService.EligibilityAlreadyLoadedInThisMonth(_patientInsurance);

            //Assert
            Assert.IsTrue(true);
        }
        [Test]
        public void ResetCoordinates_PassParameter_ReturnData()
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _facilityLocation.FACILITY_TYPE_NAME = "private home";
            _facilityLocation.PATIENT_ACCOUNT = 101116354813109;
            _facilityLocation.UpdatePatientAddress = false;

            //Act
            _patientService.ResetCoordinates(_facilityLocation, _userProfile);

            //Assert
            Assert.IsTrue(true);
        }


        [Test]
        public void SearchCityStateAddressByAPI_WhenZipCodeIsNull_ShouldReturnEmptyList()
        {
            // Arrange
            var zipCode = "00659";
            var address = "HATILLO";


            // Act
            var actual = _patientService.SearchCityStateAddressByAPI(zipCode, address);

            // Assert
            Assert.IsNotNull(actual);
        }

   
        //[Test]
        //[TestCase(1011163, 101116356510281, 605105)]
        //[TestCase(1011163, 101116356510281, 605100)]
        //[TestCase(1012714, 1012714534318092895, 605105)]
        //public void AddPatientPOSTest_FL(long practiceCode, long pAccount, long Loc_ID)
        //{
        //    //Arrange
        //    List<PatientPOSLocation> posLocationList = new List<PatientPOSLocation>();
        //    PatientPOSLocation location1 = new PatientPOSLocation();

        //    UserProfile profile = new UserProfile();
        //    long patientAccount = pAccount;
        //    profile.UserName = "Admin_99910674";
        //    profile.PracticeCode = practiceCode;
        //    location1.Patient_POS_ID = 53499958312796;
        //    location1.Patient_Account = 1012714534318092895;
        //    location1.Loc_ID = Loc_ID;
        //    posLocationList.Add(location1);

        //    FacilityLocation FL = new FacilityLocation();
        //    FL.CODE = "SC017";

        //    //Act
        //    _patientService.AddPatientPOS(posLocationList, profile, patientAccount);

        //    //Assert
        //    Assert.IsNotNull(posLocationList);
        //    Assert.IsNotNull(profile);

        //}


        


        //[Test]
        //public void AddUpdatePatientInsurance_ShouldAddPatientInsurance()
        //{
        //    // Arrange
        //    List<PatientInsurance> patientInsurance = new List<PatientInsurance>();
        //    UserProfile profile = new UserProfile();
        //    long patientAccount = 0;

        //    // Act
        //    _patientService.AddUpdatePatientInsurance(patientInsurance, profile, patientAccount);

        //    // Assert
        //    Assert.IsNotNull(patientInsurance);
        //}















        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _patientService = null;
            _patientSearchRequest = null;
            _userProfile = null;
            _patientUpdateHistory = null;
            _smartSearchInsuranceReq = null;
            _patient = null;
            _patientContact = null;
            _facilityLocation = null;
            _ssnExist = null;
            _subscriberSearchReq = null;
            _patientExist = null;
            _employerSearchReq = null;
            _advanceInsuranceSearch = null;
            _advancePatientSearch = null;
            _defaultInsuranceParameters = null;
            _subscriberInfoRequest = null;
            _autoPopulateModel = null;
            _suggestedMCPayer = null;
            _smartSearchCountriesReq = null;
            _subscriber = null;
            _payorDataModel = null;
            _patientPatDocument = null;
            _serviceConfiguration = null;
            _patientInsuranceAuthDetails = null;
            _foxTblAuthCharge = null;
            _interfaceSynchModel = null;
            _workOrderInfoReq = null;
            _patientInsuranceDetail = null;
        }
    }
}

