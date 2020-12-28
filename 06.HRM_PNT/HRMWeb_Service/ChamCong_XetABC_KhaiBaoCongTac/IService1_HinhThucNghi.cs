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
        IEnumerable<DTO_HinhThucNghi> GetList_HinhThucNghi(String publicKey, String token);

        [OperationContract]
        String GetList_HinhThucNghi_Json(String publicKey, String token);

        // ////////////////////////////////////////////

        [OperationContract]
        DTO_BoPhan Get_HinhThucNghiBy_Id(String publicKey, String token, Guid id);

        [OperationContract]
        String Get_HinhThucNghiBy_Id_Json(String publicKey, String token, Guid id);
    }
}
