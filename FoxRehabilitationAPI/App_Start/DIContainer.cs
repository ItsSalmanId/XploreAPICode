using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using System.Diagnostics.CodeAnalysis;

namespace FoxRehabilitationAPI.App_Start
{
    [ExcludeFromCodeCoverage]
    public static class DIContainer
    {
        public static UnityContainer GetContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<BusinessOperations.Security.ITokenService, BusinessOperations.Security.TokenService>();
            container.RegisterType<BusinessOperations.Security.IUserServices, BusinessOperations.Security.UserService>();
            //container.RegisterType<BusinessOperations.SettingsService.UserMangementService.IUserManagementService, BusinessOperations.SettingsService.UserMangementService.UserManagementService>();
            //container.RegisterType<IOriginalQueueService, OriginalQueueService>();
            //container.RegisterType<BusinessOperations.PatientServices.IPatientService, BusinessOperations.PatientServices.PatientService>();
            //container.RegisterType<BusinessOperations.IndexedQueueService.IIndexedQueueServices, BusinessOperations.IndexedQueueService.IndexedQueueServices>();
            //container.RegisterType<IUnAssignedQueueService, UnAssignedQueueService>();
            //container.RegisterType<ICompleteQueueService, CompleteQueueService>();
            //container.RegisterType<BusinessOperations.AssignedQueueService.IAssignedQueueServices, BusinessOperations.AssignedQueueService.AssignedQueueServices>();
            //container.RegisterType<BusinessOperations.DashboardServices.IDashboardService, BusinessOperations.DashboardServices.DashboardService>();
            //container.RegisterType<BusinessOperations.SearchOrderServices.ISearchOrderServices, BusinessOperations.SearchOrderServices.SearchOrderServices>();
            //container.RegisterType<BusinessOperations.IndexInfoServices.IIndexInfoService, BusinessOperations.IndexInfoServices.IndexInfoService>();
            //container.RegisterType<BusinessOperations.SupervisorWorkService.ISupervisorWorkService, BusinessOperations.SupervisorWorkService.SupervisorWorkService>();
            //container.RegisterType<BusinessOperations.CommonServices.ICommonServices, BusinessOperations.CommonServices.CommonServices>();
            //container.RegisterType<BusinessOperations.StoreProcedure.SettingsService.ReferralSourceService.IReferralSourceService, BusinessOperations.StoreProcedure.SettingsService.ReferralSourceService.ReferralSourceService>();
            //container.RegisterType<BusinessOperations.SettingsService.FacilityLocationService.IFacilityLocationService, BusinessOperations.SettingsService.FacilityLocationService.FacilityLocationService>();
            //container.RegisterType<BusinessOperations.RequestForOrder.IRequestForOrderService, BusinessOperations.RequestForOrder.RequestForOrderService>();
            //container.RegisterType<BusinessOperations.ReportingServices.ReferralReportServices.IReferralReportServices, BusinessOperations.ReportingServices.ReferralReportServices.ReferralReportServices>();
            //container.RegisterType<BusinessOperations.RequestForOrder.UploadOrderImages.IUploadOrderImagesService, BusinessOperations.RequestForOrder.UploadOrderImages.UploadOrderImagesService>();
            
            container.RegisterType<BusinessOperations.CommonServices.UploadFiles.IUploadFilesServices, BusinessOperations.CommonServices.UploadFiles.UploadFilesServices>();
            //container.RegisterType<BusinessOperations.PatientSurveyService.IPatientSurveyService, BusinessOperations.PatientSurveyService.PatientSurveyService>();
            //container.RegisterType<BusinessOperations.PatientSurveyService.SearchSurveyService.ISearchSurveyService, BusinessOperations.PatientSurveyService.SearchSurveyService.SearchSurveyService>();
            //container.RegisterType<BusinessOperations.PatientSurveyService.UploadDataService.IUploadDataService, BusinessOperations.PatientSurveyService.UploadDataService.UploadDataService>();
            //container.RegisterType<BusinessOperations.PatientSurveyService.SurveyReportsService.ISurveyReportsService, BusinessOperations.PatientSurveyService.SurveyReportsService.SurveyReportsService>();
            //container.RegisterType<BusinessOperations.CaseServices.ICaseServices, CaseServices>();
            //container.RegisterType<BusinessOperations.TaskServices.ITaskServices, TaskServices>();
            
            container.RegisterType<BusinessOperations.Security.IPasswordHistoryService, BusinessOperations.Security.PasswordHistoryService>();
            //container.RegisterType<BusinessOperations.RequestForOrder.IndexInformationServices.IIndexInformationService, BusinessOperations.RequestForOrder.IndexInformationServices.IndexInformationService>();
            //container.RegisterType<IAccountServices, AccountServices>();
            //container.RegisterType<IGeneralNotesServices, GeneralNotesServices>();
            //container.RegisterType<BusinessOperations.SettingsService.ReferralRegionServices.IReferralRegionService, BusinessOperations.SettingsService.ReferralRegionServices.ReferralRegionService>();
            //container.RegisterType<BusinessOperations.FoxPHDService.IFoxPHDService, BusinessOperations.FoxPHDService.FoxPHDService>();
            //container.RegisterType<BusinessOperations.PatientDocumentsService.IPatientDocumentsService, BusinessOperations.PatientDocumentsService.PatientDocumentsService>();
            //container.RegisterType<BusinessOperations.ReconciliationService.IReconciliationService, BusinessOperations.ReconciliationService.ReconciliationService>();

            //container.RegisterType<IGroupService, GroupService>();
            //ontainer.RegisterType<BusinessOperations.SettingsService.AnnouncementService.IAnnouncementService, BusinessOperations.SettingsService.AnnouncementService.AnnouncementService>();
            //ontainer.RegisterType<BusinessOperations.SettingsService.ClinicianSetupService.IClinicianSetupService, BusinessOperations.SettingsService.ClinicianSetupService.ClinicianSetupService>();
            //ontainer.RegisterType<BusinessOperations.PatientMaintenanceService.PatientInsuranceService.IPatientInsuranceService, BusinessOperations.PatientMaintenanceService.PatientInsuranceService.PatientInsuranceService>();
            //ontainer.RegisterType<BusinessOperations.PatientMaintenanceService.IPatientMaintenanceService, BusinessOperations.PatientMaintenanceService.PatientMaintenanceService>();
            //ontainer.RegisterType<BusinessOperations.IndexInfoServices.UploadWorkOrderFiles.IUploadWorkOrderFilesService, BusinessOperations.IndexInfoServices.UploadWorkOrderFiles.UploadWorkOrderFilesService>();
            //ontainer.RegisterType<BusinessOperations.QualityAssuranceService.EvaluationSetupService.IEvaluationSetupService, BusinessOperations.QualityAssuranceService.EvaluationSetupService.EvaluationSetupService>();
            //ontainer.RegisterType<BusinessOperations.QualityAssuranceService.PerformAuditService.IPerformAuditService, BusinessOperations.QualityAssuranceService.PerformAuditService.PerformAuditService>();
            //ontainer.RegisterType<BusinessOperations.QualityAssuranceService.QADashboardService.IQADashboardService, BusinessOperations.QualityAssuranceService.QADashboardService.QADashboardService>();
            //ontainer.RegisterType<BusinessOperations.QualityAssuranceService.QAReportService.IQAReportService, BusinessOperations.QualityAssuranceService.QAReportService.QAReportService>();
            //ontainer.RegisterType<BusinessOperations.Scheduler.ISchedulerService, BusinessOperations.Scheduler.SchedulerService>();
            //ontainer.RegisterType<BusinessOperations.HrAutoEmail.IHrAutoEmailService, BusinessOperations.HrAutoEmail.HrAutoEmailService>();
            //ontainer.RegisterType<BusinessOperations.SignatureRequiredServices.ISignatureRequiredService, BusinessOperations.SignatureRequiredServices.SignatureRequiredService>();
            //ontainer.RegisterType<ISupportStaffService, SupportStaffService>();
            container.RegisterType<BusinessOperations.SurveyAutomationService.ISurveyAutomationService, BusinessOperations.SurveyAutomationService.SurveyAutomationService>();
            //container.RegisterType<IConsentToCareService, ConsentToCareService>();
            //container.RegisterType<IPowerOfAttorneyService, PowerOfAttorneyService>();
            return container;
        }
    }
}