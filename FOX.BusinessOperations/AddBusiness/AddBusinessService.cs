using BusinessOperations.CommonService;
using BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using FOX.ExternalServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static FOX.DataModels.Models.AddBusiness.AddBusiness;

namespace BusinessOperations.AddBusinessService
{
    public class AddBusinessService : IAddBusinessService
    {

        #region PROPERTIES
        private readonly DBContextAddBusiness _addBusinessContext = new DBContextAddBusiness();
        //private readonly GenericRepository<PatientSurveyHistory> _patientSurveyHistoryRepository;
        //private readonly GenericRepository<PatientSurvey> _patientSurveyRepository;
        //private readonly GenericRepository<AutomatedSurveyUnSubscription> _automatedSurveyUnSubscription;
        //private readonly GenericRepository<Patient> _patientRepository;
        private readonly GenericRepository<BusinessDetail> _businessDetailRepository;
        public static string SurveyMethod = string.Empty;
        private string passPhrase = "2657894562368456";

        #endregion

        #region CONSTRUCTOR
        public AddBusinessService()
        {
            //_patientSurveyHistoryRepository = new GenericRepository<PatientSurveyHistory>(_surveyAutomationContext);
            //_patientSurveyRepository = new GenericRepository<PatientSurvey>(_surveyAutomationContext);
            //_automatedSurveyUnSubscription = new GenericRepository<AutomatedSurveyUnSubscription>(_surveyAutomationContext);
            //_patientRepository = new GenericRepository<Patient>(_surveyAutomationContext);
            //_surveyServiceLogRepository = new GenericRepository<SurveyServiceLog>(_surveyAutomationContext);
            _businessDetailRepository = new GenericRepository<BusinessDetail>(_addBusinessContext);
        }
        #endregion FUNCTION
        #region
        public ResponseModel Register(BusinessDetail objBusinessDetail)
        {
            ResponseModel response = new ResponseModel();
            //UserAccount userAccount = new UserAccount();
            //long practiceCode = AppConfiguration.GetPracticeCode;
            //userAccount = _userAccountRepository.GetFirst(r => r.EMAIL_ADDRESS == objUserAccount.EMAIL_ADDRESS && r.DELETED == false);


            //if (objUserAccount != null && userAccount == null)
            //{
            //    //UserAccount objautomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
            //    objUserAccount.APPLICATION_USER_ACCOUNTS_ID = Helper.getMaximumId("APPLICATION_USER_ACCOUNTS_ID");
            //    //objautomatedSurveyUnSubscription.SURVEY_ID = objPatientSurvey.SURVEY_ID;
            //    // var patinetAccount = Convert.ToInt64(objPatientSurvey.PATIENT_ACCOUNT_NUMBER);
            //    //objUserAccount.User_Name = objUserAccount;
            //    //objUserAccount.EMAIL_ADDRESS = subscriptionSms;
            //    //objUserAccount.PASSWORD = subscriptionEmail;

            //    objUserAccount.CREATED_BY = objUserAccount.MODIFIED_BY = "Team";
            //    objUserAccount.CREATED_DATE = objUserAccount.MODIFIED_DATE = Helper.GetCurrentDate();
            //    _userAccountRepository.Insert(objUserAccount);
            //    _userAccountRepository.Save();
            //    response.Message = "added sussfully";
            //    response.Success = true;

            //}
            //else
            //{
            //    response.Message = "already exist";
            //    response.Success = false;
            //}
            return response;
            //throw new NotImplementedException();
        }

        public ResponseModel AddUpdateBusiness(BusinessDetail objBusinessDetail)
        {
            ResponseModel response = new ResponseModel();
            BusinessDetail businessDetail = new BusinessDetail();
            long practiceCode = AppConfiguration.GetPracticeCode;
            businessDetail = _businessDetailRepository.GetFirst(r => r.BUSINESS_DETAIL_ID == objBusinessDetail.BUSINESS_DETAIL_ID && r.DELETED == false);
            if(businessDetail == null)
            {
                //UserAccount objautomatedSurveyUnSubscription = new AutomatedSurveyUnSubscription();
                objBusinessDetail.BUSINESS_DETAIL_ID = Helper.getMaximumId("BUSINESS_DETAIL_ID");
                //objautomatedSurveyUnSubscription.SURVEY_ID = objPatientSurvey.SURVEY_ID;
                // var patinetAccount = Convert.ToInt64(objPatientSurvey.PATIENT_ACCOUNT_NUMBER);
                //objUserAccount.User_Name = objUserAccount;
                //objUserAccount.EMAIL_ADDRESS = subscriptionSms;
                //objUserAccount.PASSWORD = subscriptionEmail;

                objBusinessDetail.CREATED_BY = objBusinessDetail.MODIFIED_BY = "Team";
                objBusinessDetail.CREATED_DATE = objBusinessDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                _businessDetailRepository.Insert(objBusinessDetail);
                _businessDetailRepository.Save();
                response.Message = "added sussfully";
                response.Success = true;
            }

            return response;
        }
        #endregion
    }
}
