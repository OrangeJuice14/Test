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
        #region Tìm kiếm
        //dem so mau tin
        //[OperationContract]
        //int QuanLyChamCong_FindCount(String publicKey, String token, int ngay, int thang, int nam, Guid? boPhanId,
        //   int trangThaiChamCong, bool? diHoc, String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu);

        //Tim kiem
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="token"></param>
        /// <param name="ngay"></param>
        /// <param name="thang"></param>
        /// <param name="nam"></param>
        /// <param name="boPhanId">null la tim tat ca bo phan</param>
        /// <param name="trangThaiChamCong">-1 Tat cả trạng thai, 0 chua cham cong, 1 da cham cong</param>
        /// <param name="diHoc"></param>
        /// <param name="maNhanSu"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<DTO_QuanLyChamCong_Find> QuanLyChamCong_Find(String publicKey, String token,
           int ngay, int thang, int nam, Guid? boPhanId, int trangThaiChamCong, Boolean? diHoc, String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="token"></param>
        /// <param name="ngay"></param>
        /// <param name="thang"></param>
        /// <param name="nam"></param>
        /// <param name="boPhanId">null la tim tat ca bo phan</param>
        /// <param name="trangThaiChamCong">-1 Tat cả trạng thai, 0 chua cham cong, 1 da cham cong</param>
        /// <param name="diHoc"></param>
        /// <param name="maNhanSu"></param>
        /// <param name="webUserId">bat buoc</param>
        /// <returns></returns>
        [OperationContract]
        String QuanLyChamCong_Find_Json(String publicKey, String token, int ngay, int thang, int nam, Guid? boPhanId,
            int trangThaiChamCong, Boolean? diHoc, String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu);



        //Tim co phan trang
        //
        /*
        [OperationContract]
        IEnumerable<DTO_QuanLyChamCong_Find> QuanLyChamCong_Find_PhanTrang(String publicKey, String token,
            int ngay, int thang, int nam, Guid? boPhanId, int trangThaiChamCong, Boolean? diHoc, String maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId, Guid? idLoaiNhanSu);
        [OperationContract]
        String QuanLyChamCong_Find_PhanTrang_Json(String publicKey, String token, int ngay, int thang, int nam, Guid? boPhanId, int trangThaiChamCong, Boolean? diHoc, String maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId, Guid? idLoaiNhanSu);

        */
        #endregion

        #region SAVE
        // Save////////////////////////////////////////////
        [OperationContract]
        bool QuanLyChamCong_Save(String publicKey, String token, DTO_QuanLyChamCong_Find obj);
        [OperationContract]
        bool QuanLyChamCong_Save_Json(String publicKey, String token, string jsonObject);



        [OperationContract]
        bool QuanLyChamCong_SaveList(String publicKey, String token, List<DTO_QuanLyChamCong_Find> objList);
        [OperationContract]
        bool QuanLyChamCong_SaveList_Json(String publicKey, String token, string jsonObjectList);
        #endregion

        #region DELETE
        // XOA////////////////////////////////////////////
        [OperationContract]
        bool QuanLyChamCong_DeleteBy_Id(String publicKey, String token, Guid id);
        [OperationContract]
        bool QuanLyChamCong_DeleteListBy_IdList(String publicKey, String token, List<Guid> idList);
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="token"></param>
        /// <param name="ngay">bat buoc</param>
        /// <param name="thang">bat buoc</param>
        /// <param name="nam">bat buoc</param>
        /// <param name="boPhanId">bat buoc</param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<DTO_QuanLyChamCong_BieuDoVaoRa> QuanLyChamCong_BieuDoVaoRa(String publicKey, String token,
           int ngay, int thang, int nam, Guid boPhanId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="token"></param>
        /// <param name="ngay">bat buoc</param>
        /// <param name="thang">bat buoc</param>
        /// <param name="nam">bat buoc</param>
        /// <param name="boPhanId">bat buoc</param>
        /// <returns></returns>
        [OperationContract]
        String QuanLyChamCong_BieuDoVaoRa_Json(String publicKey, String token,
            int ngay, int thang, int nam, Guid boPhanId);


        [OperationContract]
        IEnumerable<DTO_QuanLyChamCong_BieuDoVaoRa> QuanLyChamCong_BieuDoVaoRa_Cua1NhanVien(String publicKey,
            String token,
            int ngay, int thang, int nam, Guid nhanVienID);

        [OperationContract]
        String QuanLyChamCong_BieuDoVaoRa_Cua1NhanVien_Json(String publicKey, String token,
            int ngay, int thang, int nam, Guid nhanVienID);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="token"></param>
        /// <param name="thang">bat buoc</param>
        /// <param name="nam">bat buoc</param>
        /// <param name="boPhanId">bat buoc</param>
        /// <param name="maNhanSu"></param>
        /// <param name="webUserId">bat buoc</param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThang(String publicKey, String token,
            int thang, int nam, Guid boPhanId, String maNhanSu, Guid? idLoaiNhanSu);
        [OperationContract]
        String QuanLyChamCong_ThongTinChamCongThang_Json(String publicKey, String token,
            int thang, int nam, Guid boPhanId, String maNhanSu, Guid? idLoaiNhanSu);

        [OperationContract]
        bool QuanLyChamCong_ThongTinChamCongThang_Save(String publicKey, String token,
            DTO_QuanLyChamCong_ThongTinChamCongThang thongTinChamCongThang);
        [OperationContract]
        bool QuanLyChamCong_ThongTinChamCongThang_SaveList(String publicKey, String token,
            List<DTO_QuanLyChamCong_ThongTinChamCongThang> objList, string idWebUsers);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="token"></param>
        /// <param name="jsonObject">DTO_QuanLyChamCong_ThongTinChamCongThang</param>
        /// <returns></returns>
        [OperationContract]
        bool QuanLyChamCong_ThongTinChamCongThang_Save_Json(String publicKey, String token,
            string jsonObject);
        [OperationContract]
        bool QuanLyChamCong_ThongTinChamCongThang_SaveList_Json(String publicKey, String token,
            string jsonObject, string idWebUsers);



        [OperationContract]
        IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien(
            String publicKey, String token,
            int thang, int nam, Guid nhanVienID);

        [OperationContract]
        String QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien_Json(String publicKey, String token,
            int thang, int nam, Guid nhanVienID);
    }
}
