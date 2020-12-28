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
        IEnumerable<DTO_WebGroup> WebGroup_GetList(String publicKey, String token);

        [OperationContract]
        String WebGroup_GetList_Json(String publicKey, String token);
        // ////////////////////////////////////////////
        [OperationContract]
        DTO_WebGroup WebGroup_GetBy_Id(String publicKey, String token, Guid id);

        [OperationContract]
        String WebGroup_GetBy_Id_Json(String publicKey, String token, Guid id);

    }
}
