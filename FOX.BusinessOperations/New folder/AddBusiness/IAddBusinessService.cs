using System.Collections.Generic;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Security;
using static FOX.DataModels.Models.AddBusiness.AddBusiness;

namespace FOX.BusinessOperations.AddBusinessService
{
    public interface IAddBusinessService
    {
        #region FUNCTIONS
        ResponseModel AddUpdateBusiness(BusinessDetail objBusinessDetail);

        #endregion
    }
}
