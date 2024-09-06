using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.PatientSurvey;
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

namespace FOX.BusinessOperations.AddBusinessService
{
    public class AddBusinessService : IAddBusinessService
    {

        //#region PROPERTIES
        //private readonly DBContextSurveyAutomation _surveyAutomationContext = new DBContextSurveyAutomation();
        //private readonly GenericRepository<PatientSurveyHistory> _patientSurveyHistoryRepository;
        //private readonly GenericRepository<PatientSurvey> _patientSurveyRepository;
        //private readonly GenericRepository<AutomatedSurveyUnSubscription> _automatedSurveyUnSubscription;
        //private readonly GenericRepository<Patient> _patientRepository;
        //private readonly GenericRepository<SurveyServiceLog> _surveyServiceLogRepository;
        //public static string SurveyMethod = string.Empty;
        //private string passPhrase = "2657894562368456";

        //private readonly GenericRepository<UserAccount> _userAccountRepository;
        //#endregion

        //#region CONSTRUCTOR
        //public SurveyAutomationService()
        //{
        //    _patientSurveyHistoryRepository = new GenericRepository<PatientSurveyHistory>(_surveyAutomationContext);
        //    _patientSurveyRepository = new GenericRepository<PatientSurvey>(_surveyAutomationContext);
        //    _automatedSurveyUnSubscription = new GenericRepository<AutomatedSurveyUnSubscription>(_surveyAutomationContext);
        //    _patientRepository = new GenericRepository<Patient>(_surveyAutomationContext);
        //    _surveyServiceLogRepository = new GenericRepository<SurveyServiceLog>(_surveyAutomationContext);
        //    _userAccountRepository = new GenericRepository<UserAccount>(_surveyAutomationContext);
        //}
        //#endregion
    }
}
