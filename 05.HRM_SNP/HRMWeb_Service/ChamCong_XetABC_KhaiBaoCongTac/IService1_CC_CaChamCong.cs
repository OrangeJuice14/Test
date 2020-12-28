using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using HRMWeb_Business.Model;

namespace HRMWeb_Service
{
    public partial interface IService1
    {

        [OperationContract]
        IEnumerable<DTO_CC_CaChamCong> GetList_CaChamCong(String publicKey, String token);
        [OperationContract]
        String GetList_CaChamCong_Json(String publicKey, String token);
        [OperationContract]
        DTO_CC_CaChamCong CaChamCong_GetByID(String publicKey, String token, Guid id);
        [OperationContract]
        String CaChamCong_GetByID_Json(String publicKey, String token, Guid id);
    }
}
