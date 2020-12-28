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
        IEnumerable<DTO_BoPhan> GetList_BoPhan(String publicKey, String token);

        [OperationContract]
        String GetList_BoPhan_Json(String publicKey, String token);

        [OperationContract]
        DTO_BoPhan Get_BoPhanBy_Id(String publicKey, String token, Guid id);

        [OperationContract]
        String Get_BoPhanBy_Id_Json(String publicKey, String token, Guid id);

        [OperationContract]
        IEnumerable<DTO_BoPhan> GetList_BoPhan_DuocPhanQuyenChoWebUserId(String publicKey, String token, Guid webUserId);

        [OperationContract]
        String GetList_BoPhan_DuocPhanQuyenChoWebUserId_Json(String publicKey, String token, Guid webUserId);

        [OperationContract]
        List<DTO_BoPhan> GetList_BoPhan_DuocPhanQuyenChoWebUserId_All(String publicKey, String token, Guid webUserId, Guid gropuId);

        [OperationContract]
        String GetList_BoPhan_DuocPhanQuyenChoWebUserId_Json_All(String publicKey, String token, Guid webUserId, Guid gropuId);
    }
}
