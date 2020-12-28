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
        DTO_HoSoNhanVien Get_HoSoNhanVienBy_Id(String publicKey, String token, Guid id);

        [OperationContract]
        String Get_HoSoNhanVienBy_Id_Json(String publicKey, String token, Guid id);
        // ///////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_HoSoNhanVien> GetList_HoSoNhanVien(String publicKey, String token);

        [OperationContract]
        String GetList_HoSoNhanVien_Json(String publicKey, String token);
        // ///////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_HoSoNhanVien> GetList_HoSoNhanVienBy_MaBoPhan(String publicKey, String token, Guid? maBoPhan);

        [OperationContract]
        String GetList_HoSoNhanVienBy_MaBoPhan_Json(String publicKey, String token, Guid? maBoPhan);
    }
}
