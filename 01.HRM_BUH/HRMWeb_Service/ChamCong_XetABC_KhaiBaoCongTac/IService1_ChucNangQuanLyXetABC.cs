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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="token"></param>
        /// <param name="thang">bat buoc</param>
        /// <param name="nam">bat buoc</param>
        /// <param name="idNhanVien">bat buoc</param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<DTO_QuanLyXetABC_BieuDoVaoRa> QuanLyXetABC_BieuDoVaoRa(String publicKey, String token,
            int thang, int nam, Guid idNhanVien);
        [OperationContract]
        DTO_QuanLyXetABC_ChiTietTheoNhanVien QuanLyXetABC_ChiTietTheoNhanVien(String publicKey, String token,
            int thang, int nam, Guid idNhanVien);
        [OperationContract]
        String QuanLyXetABC_ChiTietTheoNhanVien_Json(String publicKey, String token,
            int thang, int nam, Guid idNhanVien);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="token"></param>
        /// <param name="thang">bat buoc</param>
        /// <param name="nam">bat buoc</param>
        /// <param name="idNhanVien">bat buoc</param>
        /// <returns></returns>
        [OperationContract]
        String QuanLyXetABC_BieuDoVaoRa_Json(String publicKey, String token,
            int thang, int nam, Guid idNhanVien);


        #region QuanLyXetABC_Find
        //[OperationContract]
        //int QuanLyXetABC_FindCount(String publicKey, String token, int thang, int nam, Guid boPhanId,
        //   Guid? idLoaiNhanSu, bool? diHoc);

        [OperationContract]
        IEnumerable<DTO_ChiTietChamCongNhanVien> QuanLyXetABC_Find(String publicKey, String token, int thang, int nam,
           Guid boPhanId, Guid? idLoaiNhanSu, bool? diHoc);

   //     [OperationContract]
   //     IEnumerable<DTO_ChiTietChamCongNhanVien> QuanLyXetABC_Find(String publicKey, String token, int thang, int nam,
   //Guid boPhanId, Guid? idLoaiNhanSu, bool? diHoc, string ngayNghiFormat);
        [OperationContract]
        String QuanLyXetABC_Find_Json(String publicKey, String token, int thang, int nam, Guid boPhanId,
           Guid? idLoaiNhanSu, bool? diHoc);
   //     [OperationContract]
   //     String QuanLyXetABC_Find_Json(String publicKey, String token, int thang, int nam, Guid boPhanId,
   //Guid? idLoaiNhanSu, bool? diHoc, string ngayNghiFormat);

        #endregion

        #region Save
        // Save////////////////////////////////////////////
        [OperationContract]
        bool QuanLyXetABC_Save(String publicKey, String token, DTO_ChiTietChamCongNhanVien obj);
        [OperationContract]
        bool QuanLyXetABC_Save_Json(String publicKey, String token, string jsonObject);
        [OperationContract]
        bool QuanLyXetABC_SaveList(String publicKey, String token, List<DTO_ChiTietChamCongNhanVien> objList);
        [OperationContract]
        bool QuanLyXetABC_SaveList_Json(String publicKey, String token, string jsonObjectList);

        #endregion

        [OperationContract]
        bool QuanLyXetABC_KhoaVaMoKhoaList(String publicKey, String token, List<DTO_ChiTietChamCongNhanVien> objList,
            bool khoa);
        [OperationContract]
        bool QuanLyXetABC_KhoaVaMoKhoaList_Json(String publicKey, String token, string jsonObjectList, bool khoa);

         [OperationContract]
        int CauHinhXetABC_GetThoiGian(String publicKey, String token, Guid Oid);

         [OperationContract]
         bool CauHinhXetABC_Update(String publicKey, String token, Guid Oid, int day);

    }
}
