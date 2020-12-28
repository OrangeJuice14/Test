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
        IEnumerable<DTO_CC_KyChamCong> KyChamCong_GetAll(String publicKey, String token);
        [OperationContract]
        String KyChamCong_GetAll_Json(String publicKey, String token);

        ////////////////
        [OperationContract]
        DTO_CC_KyChamCong KyChamCong_ByID(String publicKey, String token, Guid oidKyChamCong);
        [OperationContract]
        String KyChamCong_ByID_Json(String publicKey, String token, Guid oidKyChamCong);
       
    }
}
