using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;

namespace BusinessOperations.CommonServices.UploadFiles
{
    public interface IUploadFilesServices
    {
        ResponseUploadFilesModel UploadFiles(RequestUploadFilesModel requestUploadFilesAPIModel);
        ResponseLedgerUploadFilesModel UploadReconsiliationLedger(RequestUploadFilesModel requestUploadFilesAPIModel, string reconsiliationId);
        ResponseUploadFilesModel UploadHAutoEmailFiles(RequestUploadFilesModel requestUploadFilesAPIModel, UserProfile userProfile);
    }
}
