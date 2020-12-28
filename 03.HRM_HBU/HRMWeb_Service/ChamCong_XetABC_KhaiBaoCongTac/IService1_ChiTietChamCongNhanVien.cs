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
        IEnumerable<DTO_ChiTietChamCongNhanVien> GetList_ChiTietChamCongNhanVien(String publicKey, String token);

        [OperationContract]
        String GetList_ChiTietChamCongNhanVien_Json(String publicKey, String token);

        // ////////////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_ChiTietChamCongNhanVien> GetList_ChiTietChamCongNhanVienBy_HoSoNhanVienId(String publicKey,
            String token, Guid hoSoNhanVienId);

        [OperationContract]
        String GetList_ChiTietChamCongNhanVienBy_HoSoNhanVienId_Json(String publicKey, String token,
           Guid hoSoNhanVienId);
        // ////////////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_ChiTietChamCongNhanVien> GetList_ChiTietChamCongNhanVienBy_BoPhanId(String publicKey,
           String token, Guid boPhanId);

        [OperationContract]
        String GetList_ChiTietChamCongNhanVienBy_BoPhanId_Json(String publicKey, String token, Guid boPhanId);
        // ////////////////////////////////////////////
        [OperationContract]
        DTO_ChiTietChamCongNhanVien Get_ChiTietChamCongNhanVienBy_Id(String publicKey, String token, Guid id);

        [OperationContract]
        String GetList_ChiTietChamCongNhanVienBy_Id_Json(String publicKey, String token, Guid id);
        // Save////////////////////////////////////////////
        [OperationContract]
        bool Save_ChiTietChamCongNhanVien(String publicKey, String token, DTO_ChiTietChamCongNhanVien obj);

        [OperationContract]
        bool Save_ChiTietChamCongNhanVien_Json(String publicKey, String token, string jsonObject);
        // XOA////////////////////////////////////////////
        [OperationContract]
        bool Delete_ChiTietChamCongNhanVienBy_Id(String publicKey, String token, Guid id);
        [OperationContract]
        bool DeleteList_ChiTietChamCongNhanVienBy_IdList(String publicKey, String token, List<Guid> idList);
    }
}
