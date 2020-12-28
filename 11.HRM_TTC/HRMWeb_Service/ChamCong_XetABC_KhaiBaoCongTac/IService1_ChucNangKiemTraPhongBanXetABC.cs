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
        IEnumerable<DTO_KiemTraPhongBanXetABC> KiemTraPhongBanXetABC_Find(String publicKey, String token, int thang,
            int nam, Boolean? daXetXongAbc, Guid congTy);
        [OperationContract]
        String KiemTraPhongBanXetABC_Find_Json(String publicKey, String token, int thang, int nam, Boolean? daXetXongAbc, Guid congTy);
    }
}
