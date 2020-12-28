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

        #region GET LIST

        //GetList lấy danh sách bộ phận////////////////////////////////////////////////////////// 
        [OperationContract]
        IEnumerable<DTO_CC_ChamCongTheoNgay> GetList_CC_ChamCongTheoNgay(String publicKey, String token);

        [OperationContract]
        String GetList_CC_ChamCongTheoNgay_Json(String publicKey, String token);
        //GetList By_HoSoNhanVienId ////////////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_CC_ChamCongTheoNgay> GetList_CC_ChamCongTheoNgayBy_HoSoNhanVienId(String publicKey,
            String token, Guid hoSoNhanVienId);

        [OperationContract]
        String GetList_CC_ChamCongTheoNgayBy_HoSoNhanVienId_Json(String publicKey, String token,
            Guid hoSoNhanVienId);
        //GetList By_Ngay ////////////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_CC_ChamCongTheoNgay> GetList_CC_ChamCongTheoNgayBy_Ngay(String publicKey, String token,
           DateTime ngay);

        [OperationContract]
        String GetList_CC_ChamCongTheoNgayBy_Ngay_Json(String publicKey, String token, DateTime ngay);
        // ////////////////////////////////////////////
        //GetList By_Ngay_HoSoId ////////////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_CC_ChamCongTheoNgay> GetList_CC_ChamCongTheoNgayBy_Ngay_HoSoNhanVienId(String publicKey,
            String token, DateTime ngay, Guid hoSoNhanVienId);

        [OperationContract]
        String GetList_CC_ChamCongTheoNgayBy_Ngay_HoSoNhanVienId_Json(String publicKey, String token,
           DateTime ngay, Guid hoSoNhanVienId);
        //GetList By_NgayThangNam_HoSoId ////////////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_CC_ChamCongTheoNgay> GetList_CC_ChamCongTheoNgayBy_ThangNam_HoSoNhanVienId(String publicKey,
            String token, int thang, int nam, Guid hoSoNhanVienId);

        [OperationContract]
        String GetList_CC_ChamCongTheoNgayBy_ThangNam_HoSoNhanVienId_Json(String publicKey, String token, int thang, int nam, Guid hoSoNhanVienId);
        //GetList By_BoPhanId ////////////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_CC_ChamCongTheoNgay> GetList_CC_ChamCongTheoNgayBy_BoPhanId(String publicKey,
            String token, Guid boPhanId);

        [OperationContract]
        String GetList_CC_ChamCongTheoNgayBy_BoPhanId_Json(String publicKey, String token, Guid boPhanId);
        //GetList By_BoPhanId ////////////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_CC_ChamCongTheoNgay> GetList_CC_ChamCongTheoNgayBy_Date_HoSoId_BoPhanId(String publicKey,
           String token, DateTime ngay, Guid hoSoNhanVienId, Guid boPhanId);

        [OperationContract]
        String GetList_CC_ChamCongTheoNgayBy_Date_HoSoId_BoPhanId_Json(String publicKey, String token,
           DateTime ngay, Guid hoSoNhanVienId, Guid boPhanId);

        #endregion



        #region GET SINGLE OBJECT
        //Get obj ////////////////////////////////////////////
        [OperationContract]
        DTO_CC_ChamCongTheoNgay Get_CC_ChamCongTheoNgayBy_Id(String publicKey, String token, Guid id);

        [OperationContract]
        String Get_CC_ChamCongTheoNgayBy_Id_Json(String publicKey, String token, Guid id);
        #endregion


    }
}
