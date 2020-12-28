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
        IEnumerable<DTO_LoaiNhanSu> GetList_LoaiNhanSu(String publicKey, String token);
        [OperationContract]
        String GetList_LoaiNhanSu_Json(String publicKey, String token);
    }
}
