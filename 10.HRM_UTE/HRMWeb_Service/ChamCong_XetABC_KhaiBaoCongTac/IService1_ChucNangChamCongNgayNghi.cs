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
        DTO_ChamCongNgayNghi_Find ChamCongNgayNghi_GetByID(String publicKey, String token, Guid id);
      
        [OperationContract]
        String ChamCongNgayNghi_GetByID_Json(String publicKey, String token, Guid id);
        // Find /////////////////////////
        [OperationContract]
        IEnumerable<DTO_ChamCongNgayNghi_Find> ChamCongNgayNghi_Find(String publicKey, String token, int thang,
            int nam, Guid? boPhanId, String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu,int trangThai, bool? isAdmin);

        [OperationContract]
        String ChamCongNgayNghi_Find_Json(String publicKey, String token, int thang, int nam, Guid? boPhanId,
            String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu, int trangThai, bool? isAdmin);

        // Create //////////////////
        // Delete /////////////////////////
        [OperationContract]
        bool ChamCongNgayNghi_DeleteList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList);

        [OperationContract]
        bool ChamCongNgayNghi_DeleteList_Json(String publicKey, String token, string jsonObjectList);


        //kiem tra 
        [OperationContract]
        bool ChamCongNgayNghi_KiemTraTuNgayDenNgayCoHopLe(String publicKey, String token, Guid? chamCongNgayNghiOid,
            DateTime tuNgay, DateTime denNgay, Guid nhanVienID);
        // Save////////////////////////////////////////////
        [OperationContract]
        bool ChamCongNgayNghi_Save(String publicKey, String token, DTO_ChamCongNgayNghi_Find obj);

        [OperationContract]
        bool ChamCongNgayNghi_Save_Json(String publicKey, String token, string jsonObject);

        [OperationContract]
        bool ChamCongNgayNghi_SaveList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList);

        [OperationContract]
        bool ChamCongNgayNghi_SaveList_Json(String publicKey, String token, string jsonObjectList);


    }
}
