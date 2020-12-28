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
        // ///////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_Web_UpdateHoSoNhanVien> GetListByBoPhan_Web_UpdateHoSoNhanVien(String publicKey, String token, Guid idBoPhan);

        [OperationContract]
        String GetListByBoPhan_Web_UpdateHoSoNhanVien_Json(String publicKey, String token, Guid idBoPhan);

        // ///////////////////////////////////////
        [OperationContract]
        DTO_Web_UpdateHoSoNhanVien GetListByOid_Web_UpdateHoSoNhanVien(String publicKey, String token, Guid oidHoSoNhanSu);

        [OperationContract]
        String GetListByOid_Web_UpdateHoSoNhanVien_Json(String publicKey, String token, Guid oidHoSoNhanSu);

    }
}
