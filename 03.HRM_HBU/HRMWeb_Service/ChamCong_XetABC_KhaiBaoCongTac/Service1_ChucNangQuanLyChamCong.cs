using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ERP_Core;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using HRMWeb_Business.Predefined;
using Newtonsoft.Json;
using System.Web.Configuration;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        #region Tìm kiếm
        //dem so mau tin
        /*
        public int QuanLyChamCong_FindCount(String publicKey, String token, int ngay, int thang, int nam, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                int count = 0;
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                if (ngay > 0)
                {
                    DateTime date = new DateTime(nam, thang, ngay);

                    count = factory.FindCount_QuanlyChamCong(date, boPhanId, trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu);
                }
                else
                {
                    count = factory.FindCount_QuanlyChamCong_ThangVaNam(thang, nam, boPhanId, trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu);
                }

                return count;
            }
            else
            {
                return 0;
            }
        }
        */

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
        /// <param name="boPhanId">Guid.Empty la tim tat ca bo phan</param>
        /// <param name="trangThaiChamCong">-1 Tat cả trạng thai, 0 chua cham cong, 1 da cham cong</param>
        /// <param name="diHoc"></param>
        /// <param name="maNhanSu"></param>
        /// <returns></returns>
        public IEnumerable<DTO_QuanLyChamCong_Find> QuanLyChamCong_Find(String publicKey, String token, int ngay, int thang, int nam, Guid? boPhanId, int trangThaiChamCong, Boolean? diHoc, String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_QuanLyChamCong_Find> list = null;
                if (ngay > 0)
                {
                    DateTime date = new DateTime(nam, thang, ngay);
                    list = factory.FindForQuanlyChamCong(date, boPhanId,
                        trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu);
                }
                else
                {
                    list = factory.FindForQuanlyChamChong_ThangVaNam(thang, nam, boPhanId, trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu);
                }
                //foreach (CC_ChamCongTheoNgay cc in tmpList)
                //{
                //    GetSetChild(cc);
                //}
                //IEnumerable<DTO_CC_ChamCongTheoNgay> list = tmpList.Map<DTO_CC_ChamCongTheoNgay>();
                return list;
            }
            else
            {
                return null;
            }
        }

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
        public String QuanLyChamCong_Find_Json(String publicKey, String token, int ngay, int thang, int nam, Guid? boPhanId, int trangThaiChamCong, Boolean? diHoc, String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu)
        {//DANG SD
            IEnumerable<DTO_QuanLyChamCong_Find> list = QuanLyChamCong_Find(publicKey, token, ngay, thang, nam, boPhanId, trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public bool QuanLyChamCong_CheckChot(String publicKey, String token, Guid boPhanID, int thang, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = ChiTietChamCongNhanVien_Factory.New();
                bool daTonTai = factory.CheckChot(thang, nam, boPhanID);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool QuanLyChamCong_CheckChotDonVi(String publicKey, String token, Guid boPhanID, int thang, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = ChiTietChamCongNhanVienDonVi_Factory.New();
                bool daTonTai = factory.CheckChot(thang, nam, boPhanID);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        /*
        //Tim co phan trang
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="token"></param>
        /// <param name="ngay"></param>
        /// <param name="thang"></param>
        /// <param name="nam"></param>
        /// <param name="boPhanId">Guid.Empty la tim tat ca bo phan</param>
        /// <param name="trangThaiChamCong">-1 Tat cả trạng thai, 0 chua cham cong, 1 da cham cong</param>
        /// <param name="diHoc"></param>
        /// <param name="maNhanSu"></param>
        /// <param name="trang"></param>
        /// <param name="soMauTinMoiTrang"></param>
        /// <returns></returns>
        public IEnumerable<DTO_QuanLyChamCong_Find> QuanLyChamCong_Find_PhanTrang(String publicKey, String token, int ngay, int thang, int nam, Guid? boPhanId, int trangThaiChamCong, Boolean? diHoc, String maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId, Guid? idLoaiNhanSu)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_QuanLyChamCong_Find> list = null;
                if (ngay > 0)
                {
                    DateTime date = new DateTime(nam, thang, ngay);

                    list = factory.FindForQuanlyChamCong_CoPhanTrang(date, boPhanId,
                        trangThaiChamCong, diHoc, maNhanSu, trang, soMauTinMoiTrang, webUserId, idLoaiNhanSu);
                }
                else
                {
                    list = factory.FindForQuanlyChamCong_ThangVaNam_CoPhanTrang(thang, nam, boPhanId,
                      trangThaiChamCong, diHoc, maNhanSu, trang, soMauTinMoiTrang, webUserId, idLoaiNhanSu);
                }
                if (list != null)
                {
                    //    foreach (CC_ChamCongTheoNgay cc in tmpList)
                    //    {
                    //        GetSetChild(cc);
                    //    }
                    //IEnumerable<DTO_CC_ChamCongTheoNgay> list = tmpList.Map<DTO_CC_ChamCongTheoNgay>();
                    return list;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="token"></param>
        /// <param name="ngay"></param>
        /// <param name="thang"></param>
        /// <param name="nam"></param>
        /// <param name="boPhanId">Guid.Empty la tim tat ca bo phan</param>
        /// <param name="trangThaiChamCong">-1 Tat cả trạng thái, 0 chua cham cong, 1 da cham cong</param>
        /// <param name="diHoc"></param>
        /// <param name="maNhanSu"></param>
        /// <param name="trang"></param>
        /// <param name="soMauTinMoiTrang"></param>
        /// <returns></returns>
        public String QuanLyChamCong_Find_PhanTrang_Json(String publicKey, String token, int ngay, int thang, int nam, Guid? boPhanId, int trangThaiChamCong, Boolean? diHoc, String maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId, Guid? idLoaiNhanSu)
        {
            //if (Helper.TrustTest(publicKey, token))
            //{
            //DateTime date = new DateTime(nam, thang, ngay);
            IEnumerable<DTO_QuanLyChamCong_Find> list = QuanLyChamCong_Find_PhanTrang(publicKey, token, ngay, thang, nam,
                    boPhanId, trangThaiChamCong, diHoc, maNhanSu, trang, soMauTinMoiTrang, webUserId, idLoaiNhanSu);
            int count = QuanLyChamCong_FindCount(publicKey, token, ngay, thang, nam, boPhanId, trangThaiChamCong, diHoc,
                maNhanSu, webUserId, idLoaiNhanSu);
            var result = new { Data = list, Total = count };
            String json = JsonConvert.SerializeObject(result);
            return json;
            //}
            //else
            //{
            //    return null;
            //}
        }
        */

        #endregion

        #region SAVE
        // Save////////////////////////////////////////////
        public bool QuanLyChamCong_Save(String publicKey, String token, DTO_QuanLyChamCong_Find obj)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                if (obj != null)
                {
                    CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                    CC_ChamCongTheoNgay objFromDB = factory.GetByID(obj.Oid);
                    if (objFromDB != null)
                    {
                        //    //them moi
                        //    //map sang entity
                        //    var newDBObject = factory.CreateManagedObject();
                        //    //newDBObject.CopyPropertiesFrom(obj);
                        //    newDBObject.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "IDHinhThucNghi", "DaChamCong" });
                        //    if (newDBObject.Oid == Guid.Empty)
                        //    {
                        //        newDBObject.Oid = Guid.NewGuid();
                        //    }
                        //}
                        //else
                        //{
                        //cap nhat
                        //objFromDB.CopyPropertiesFrom(obj);
                        objFromDB.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "IDHinhThucNghi", "DaChamCong" });
                    }
                    //////////////
                    try
                    {
                        factory.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                return false;
            }
            return false;
        }
        public bool QuanLyChamCong_Save_Json(String publicKey, String token, string jsonObject)
        {
            //chuyen jsonObject thanh object
            DTO_QuanLyChamCong_Find obj = JsonConvert.DeserializeObject<DTO_QuanLyChamCong_Find>(jsonObject);
            return QuanLyChamCong_Save(publicKey, token, obj);
        }




        public bool QuanLyChamCong_SaveList(String publicKey, String token, List<DTO_QuanLyChamCong_Find> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                foreach (var obj in objList)
                {
                    if (obj != null)
                    {

                        CC_ChamCongTheoNgay objFromDB = factory.GetByID(obj.Oid);
                        if (objFromDB != null)
                        {
                            //    //them moi
                            //    //map sang entity
                            //    var newDBObject = factory.CreateManagedObject();
                            //    //newDBObject.CopyPropertiesFrom(obj);  
                            //    newDBObject.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "IDHinhThucNghi", "DaChamCong" });
                            //}
                            //else
                            //{
                            //cap nhat
                            //objFromDB.CopyPropertiesFrom(obj);
                            objFromDB.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "IDHinhThucNghi", "DaChamCong" });
                        }

                    }
                }
                //////////////
                try
                {
                    factory.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                return false;
            }
            return false;
        }
        public bool QuanLyChamCong_SaveList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_QuanLyChamCong_Find> objList = JsonConvert.DeserializeObject<List<DTO_QuanLyChamCong_Find>>(jsonObjectList);
            return QuanLyChamCong_SaveList(publicKey, token, objList);
        }
        #endregion

        #region DELETE
        // XOA////////////////////////////////////////////
        public bool QuanLyChamCong_DeleteBy_Id(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                CC_ChamCongTheoNgay obj = factory.GetByID(id);
                if (obj != null)
                {
                    try
                    {
                        CC_ChamCongTheoNgay_Factory.FullDelete(factory.Context, obj);
                        factory.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public bool QuanLyChamCong_DeleteListBy_IdList(String publicKey, String token, List<Guid> idList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                Object[] objList = factory.GetListByIdList(idList).ToArray<Object>();
                if (objList != null)
                {
                    try
                    {
                        CC_ChamCongTheoNgay_Factory.FullDelete(factory.Context, objList);
                        factory.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
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
        public IEnumerable<DTO_QuanLyChamCong_BieuDoVaoRa> QuanLyChamCong_BieuDoVaoRa(String publicKey, String token,
           int ngay, int thang, int nam, Guid boPhanId)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_QuanLyChamCong_BieuDoVaoRa> list = null;

                list = factory.QuanLyChamCong_BieuDoVaoRa(ngay, thang, nam, boPhanId);


                return list;
            }
            else
            {
                return null;
            }
        }

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
        public String QuanLyChamCong_BieuDoVaoRa_Json(String publicKey, String token,
           int ngay, int thang, int nam, Guid boPhanId)
        {//DANG SD
            IEnumerable<DTO_QuanLyChamCong_BieuDoVaoRa> list = QuanLyChamCong_BieuDoVaoRa(publicKey, token, ngay, thang, nam, boPhanId);
            String json = JsonConvert.SerializeObject(list);
            return json;

        }







        // ////////////////////////////////////////////////////////


        public IEnumerable<DTO_QuanLyChamCong_BieuDoVaoRa> QuanLyChamCong_BieuDoVaoRa_Cua1NhanVien(String publicKey, String token,
   int ngay, int thang, int nam, Guid nhanVienID)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_QuanLyChamCong_BieuDoVaoRa> list = null;

                list = factory.QuanLyChamCong_BieuDoVaoRa_Cua1NhanVien(ngay, thang, nam, nhanVienID);


                return list;
            }
            else
            {
                return null;
            }
        }


        public String QuanLyChamCong_BieuDoVaoRa_Cua1NhanVien_Json(String publicKey, String token,
           int ngay, int thang, int nam, Guid nhanVienID)
        {//DANG SD
            IEnumerable<DTO_QuanLyChamCong_BieuDoVaoRa> list = QuanLyChamCong_BieuDoVaoRa_Cua1NhanVien(publicKey, token, ngay, thang, nam, nhanVienID);
            String json = JsonConvert.SerializeObject(list);
            return json;

        }


        // ////////////////////////////////////////////////////////













        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="token"></param>
        /// <param name="thang">bat buoc</param>
        /// <param name="nam">bat buoc</param>
        /// <param name="boPhanId">bat buoc</param>
        /// <param name="maNhanSu"></param>
        /// <returns></returns>

        public IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThang(String publicKey, String token,
            int thang, int nam, Guid boPhanId, String maNhanSu, Guid? idLoaiNhanSu)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> list = null;

                list = factory.QuanLyChamCong_ThongTinChamCongThang(thang, nam, boPhanId, maNhanSu, idLoaiNhanSu);


                return list;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThangDonVi(String publicKey, String token,
    int thang, int nam, Guid boPhanId, String maNhanSu, Guid? idLoaiNhanSu)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> list = null;

                list = factory.QuanLyChamCong_ThongTinChamCongThangDonVi(thang, nam, boPhanId, maNhanSu, idLoaiNhanSu);


                return list;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang_All> QuanLyChamCong_ThongTinChamCongThang_All(String publicKey, String token, int thang, int nam, string webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang_All> list = null;

                list = factory.QuanLyChamCong_ThongTinChamCongThang_All(thang, nam, webUserId);


                return list;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> DangKyChamCong_DoiCaLinhDong_Find(String publicKey, String token, DateTime ngay, string boPhanId)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> list = null;

                list = factory.DangKyChamCong_DoiCaLinhDong_Find(ngay, new Guid(boPhanId));


                return list;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_NgayChamCong> GetList_NgayTrongKyChamCong(String publicKey, String token, int thang, int nam)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_NgayChamCong> list = null;

                list = factory.GetList_NgayTrongKyChamCong(thang, nam);


                return list;
            }
            else
            {
                return null;
            }
        }

        public String QuanLyChamCong_ThongTinChamCongThang_Json(String publicKey, String token,
            int thang, int nam, Guid boPhanId, String maNhanSu, Guid? idLoaiNhanSu)
        {//DANG SD
            IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> list = QuanLyChamCong_ThongTinChamCongThang(publicKey, token, thang, nam, boPhanId, maNhanSu, idLoaiNhanSu);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public String QuanLyChamCong_ChamCongTheoNgayThayDoi_Json(String publicKey, String token,
    int thang, int nam, Guid boPhanId, String maNhanSu, Guid? idLoaiNhanSu)
        {//DANG SD
            IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> list = QuanLyChamCong_ThongTinChamCongThang(publicKey, token, thang, nam, boPhanId, maNhanSu, idLoaiNhanSu);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String QuanLyChamCong_ThongTinChamCongThangDonVi_Json(String publicKey, String token,
    int thang, int nam, Guid boPhanId, String maNhanSu, Guid? idLoaiNhanSu)
        {//DANG SD
            IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> list = QuanLyChamCong_ThongTinChamCongThangDonVi(publicKey, token, thang, nam, boPhanId, maNhanSu, idLoaiNhanSu);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String QuanLyChamCong_ThongTinChamCongThangAll_Json(String publicKey, String token, int thang, int nam, string webUserId)
        {//DANG SD
            IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang_All> list = QuanLyChamCong_ThongTinChamCongThang_All(publicKey, token, thang, nam, webUserId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String DangKyChamCong_DoiCaLinhDong_Find_Json(String publicKey, String token, DateTime ngay, string boPhanId)
        {//DANG SD
            IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> list = DangKyChamCong_DoiCaLinhDong_Find(publicKey, token, ngay, boPhanId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetList_NgayTrongKyChamCong_Json(String publicKey, String token, int thang, int nam)
        {//DANG SD
            IEnumerable<DTO_NgayChamCong> list = GetList_NgayTrongKyChamCong(publicKey, token, thang, nam);
            String json = JsonConvert.SerializeObject(list);
            return json;

        }
        public bool QuanLyChamCong_ThongTinChamCongThang_Save(String publicKey, String token,
            DTO_QuanLyChamCong_ThongTinChamCongThang thongTinChamCongThang)
        {
            if (thongTinChamCongThang != null && thongTinChamCongThang.ChiTietChamCong != null)
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                HinhThucNghi_Factory htnfac = HinhThucNghi_Factory.New();
                List<DTO_HinhThucNghi> listHinhThucNghi = htnfac.GetAll_GCRecordIsNull().Map<DTO_HinhThucNghi>().ToList();
                DTO_HinhThucNghi lamcangay = new DTO_HinhThucNghi();
                lamcangay.KyHieu =WebConfigurationManager.AppSettings["KyHieuLamCaNgay"];
                listHinhThucNghi.Add(lamcangay);
                foreach (var obj in thongTinChamCongThang.ChiTietChamCong)
                {

                    //lay doi tuong CC_ChamCongTheoNgay
                    CC_ChamCongTheoNgay objFromDbForSave = factory.GetByID(obj.CC_ChamCongTheoNgayOid);
                    //phan tich hinh thuc nghi 
                    Guid? idHinhThucNghiMoi = listHinhThucNghi.Where(h => h.KyHieu == obj.MaHinhThucNghi).Select(h => h.Oid).SingleOrDefault();
                    if (objFromDbForSave != null
                        && idHinhThucNghiMoi != (objFromDbForSave.IDHinhThucNghi ?? Guid.Empty)
                        )
                    {
                        if (idHinhThucNghiMoi != Guid.Empty)
                            objFromDbForSave.IDHinhThucNghi = idHinhThucNghiMoi;
                        else
                        {
                            objFromDbForSave.IDHinhThucNghi = null;
                        }
                    }
                }
                ///luu lai///////////
                try
                {
                    factory.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="token"></param>
        /// <param name="jsonObject">DTO_QuanLyChamCong_ThongTinChamCongThang</param>
        /// <returns></returns>

        public bool QuanLyChamCong_ThongTinChamCongThang_Save_Json(String publicKey, String token,
            string jsonObject)
        {
            //chuyen jsonObject thanh object
            var obj = JsonConvert.DeserializeObject<DTO_QuanLyChamCong_ThongTinChamCongThang>(jsonObject);
            return QuanLyChamCong_ThongTinChamCongThang_Save(publicKey, token, obj);
        }

        public bool QuanLyChamCong_ThongTinChamCongThang_SaveList(String publicKey, String token,
            List<DTO_QuanLyChamCong_ThongTinChamCongThang> objList, string idUser)
        {
            CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
            WebGroup_Factory groupFactory = WebGroup_Factory.New();
            HinhThucNghi_Factory htnfac = HinhThucNghi_Factory.New();
            CC_ChamCongTheoNgayThayDoi_Factory ccFactory = CC_ChamCongTheoNgayThayDoi_Factory.New();
            Guid userId = new Guid(idUser);

            List<DTO_HinhThucNghi> listHinhThucNghi = htnfac.GetAll_GCRecordIsNull().Map<DTO_HinhThucNghi>().ToList();
            DTO_HinhThucNghi lamcangay = new DTO_HinhThucNghi();
            lamcangay.KyHieu = WebConfigurationManager.AppSettings["KyHieuLamCaNgay"];
            listHinhThucNghi.Add(lamcangay);

            string webGroupId = groupFactory.GetWebGroupIdByUserId(userId);
            foreach (var thongTinChamCongThang in objList)
            {
                if (thongTinChamCongThang.ChiTietChamCong != null)
                {

                    foreach (var obj in thongTinChamCongThang.ChiTietChamCong)
                    {

                        //lay doi tuong CC_ChamCongTheoNgay
                        CC_ChamCongTheoNgay objFromDbForSave = factory.GetByID(obj.CC_ChamCongTheoNgayOid);

                        //Kiểm tra ngày có phải là t7 không
                        string thu = objFromDbForSave.Ngay.DayOfWeek.ToString();
                        HinhThucNghi hinhThucNghiMoi = htnfac.GetByKyHieu(obj.MaHinhThucNghi);

                        //Nếu là loại 3 (làm nửa ngày nghỉ nửa ngày) thì ko đc chấm vào thứ 7
                        if (hinhThucNghiMoi !=null && hinhThucNghiMoi.PhanLoai!=null && hinhThucNghiMoi.PhanLoai == 3 && thu == "Saturday")
                        {
                            //Không sửa
                        }
                        else
                        {
                            //phan tich hinh thuc nghi 
                            Guid? idHinhThucNghiMoi = listHinhThucNghi.Where(h => h.KyHieu == obj.MaHinhThucNghi).Select(h => h.Oid).SingleOrDefault();
                            //HinhThucNghiConst.HinhThucNghiDictionaryKyHieuToId[obj.MaHinhThucNghi];
                            if (objFromDbForSave != null
                                && idHinhThucNghiMoi != (objFromDbForSave.IDHinhThucNghi ?? null)
                                )
                            {
                                idHinhThucNghiMoi = idHinhThucNghiMoi != Guid.Empty ? idHinhThucNghiMoi : null;
                                if (objFromDbForSave.IDHinhThucNghi != idHinhThucNghiMoi)
                                {
                                    string webGroupObj = objFromDbForSave.IDUser != null ? groupFactory.GetWebGroupIdByUserId(objFromDbForSave.IDUser.Value) : "";

                                    //Nếu người sửa là user thư ký
                                    if (webGroupId == WebConfigurationManager.AppSettings["ThuKyGroupId"])
                                    {
                                        //Kiểm tra đã được admin sửa chưa
                                        bool exist = ccFactory.CheckExist(objFromDbForSave.Oid);

                                        //Nếu cc chưa có ai thay đổi
                                        if (objFromDbForSave.IDUser == null)
                                        {
                                            objFromDbForSave.IDHinhThucNghi = idHinhThucNghiMoi;
                                            objFromDbForSave.IDUser = userId;
                                        }
                                        //Nếu cc đã đc chấm bởi thư ký, admin chưa sửa
                                        else if (exist == false)
                                        {
                                            string objUserId = groupFactory.GetWebGroupIdByUserId(objFromDbForSave.IDUser ?? Guid.Empty);
                                            //nếu người đã sửa ko phải là admin (có thể là thư ký của phòng sau khi chuyển)
                                            if (objUserId != WebConfigurationManager.AppSettings["AdminGroupId"])
                                            {
                                                objFromDbForSave.IDHinhThucNghi = idHinhThucNghiMoi;
                                                objFromDbForSave.IDUser = userId;
                                                ccFactory.SaveChanges();
                                            }
                                            //Cập nhật thay đổi
                                            //CC_ChamCongTheoNgayThayDoi ccObj = ccFactory.CreateAloneObject();
                                            //ccObj = ccFactory.GetByID(objFromDbForSave.Oid);
                                            //if (ccObj != null)
                                            //{
                                            //    ccObj.IDHinhThucNghiThayDoi = idHinhThucNghiMoi;
                                            //    ccObj.IDHinhThucNghiDonViCham = idHinhThucNghiMoi;
                                            //}

                                        }
                                        //Nếu cc đã đc sửa bởi admin
                                        else
                                        {
                                            //Không sửa
                                        }

                                    }
                                    //Nếu người sửa là admin
                                    else if (webGroupId == WebConfigurationManager.AppSettings["AdminGroupId"])
                                    {
                                        //Nếu cc chưa có ai thay đổi
                                        if (objFromDbForSave.IDUser == null)
                                        {
                                            objFromDbForSave.IDUser = userId;
                                            SaveChamCongTheoNgayThayDoi(objFromDbForSave.Oid, idHinhThucNghiMoi, objFromDbForSave.IDHinhThucNghi);
                                            objFromDbForSave.IDHinhThucNghi = idHinhThucNghiMoi;
                                        }
                                        //Nếu cc đã đc chấm bởi thư ký
                                        else if (webGroupObj != WebConfigurationManager.AppSettings["AdminGroupId"])
                                        {
                                            CC_ChamCongTheoNgayThayDoi ccObj = ccFactory.GetByID(objFromDbForSave.Oid);

                                            //Nếu đã có thay đổi thì cập nhật thay đổi
                                            if (ccObj != null)
                                            {
                                                //Nếu đã chấm rồi sửa lại trùng với đơn vị chấm thì xóa
                                                if (idHinhThucNghiMoi == ccObj.IDHinhThucNghiDonViCham)
                                                {
                                                    ccFactory.DeleteObject(ccObj);
                                                    ccFactory.SaveChanges();
                                                }
                                                else
                                                {
                                                    ccObj.IDHinhThucNghiThayDoi = idHinhThucNghiMoi;
                                                    ccFactory.SaveChanges();
                                                }

                                            }

                                            //Nếu chưa có thì thêm mới
                                            else
                                            {
                                                SaveChamCongTheoNgayThayDoi(objFromDbForSave.Oid, idHinhThucNghiMoi, objFromDbForSave.IDHinhThucNghi);
                                            }

                                            //Cập nhật thay đổi
                                            objFromDbForSave.IDHinhThucNghi = idHinhThucNghiMoi;

                                        }
                                        //Nếu cc đã đc sửa bở admin
                                        else
                                        {
                                            CC_ChamCongTheoNgayThayDoi ccObj = ccFactory.GetByID(objFromDbForSave.Oid);

                                            //Nếu đã có thay đổi thì cập nhật thay đổi
                                            if (ccObj != null)
                                            {
                                                //Nếu đã chấm rồi sửa lại trùng với đơn vị chấm thì xóa
                                                if (idHinhThucNghiMoi == ccObj.IDHinhThucNghiDonViCham)
                                                {
                                                    ccFactory.DeleteObject(ccObj);
                                                    ccFactory.SaveChanges();
                                                }
                                                else
                                                {
                                                    ccObj.IDHinhThucNghiThayDoi = idHinhThucNghiMoi;
                                                    ccFactory.SaveChanges();
                                                }

                                            }
                                            objFromDbForSave.IDHinhThucNghi = idHinhThucNghiMoi;

                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            ///luu lai///////////
            try
            {
                factory.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }
        public bool QuanLyChamCong_ThongTinChamCongThang_SaveList_Json(String publicKey, String token, string jsonObjectList, string idUser)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_QuanLyChamCong_ThongTinChamCongThang> objList = JsonConvert.DeserializeObject<List<DTO_QuanLyChamCong_ThongTinChamCongThang>>(jsonObjectList);
            return QuanLyChamCong_ThongTinChamCongThang_SaveList(publicKey, token, objList, idUser);
        }
        public bool SaveChamCongTheoNgayThayDoi(Guid IDCC, Guid? IDThayDoi, Guid? IDDonVi)
        {
            CC_ChamCongTheoNgayThayDoi_Factory ccFactory = CC_ChamCongTheoNgayThayDoi_Factory.New();
            CC_ChamCongTheoNgayThayDoi ccObjSave = ccFactory.CreateManagedObject();
            ccObjSave.Oid = IDCC;
            ccObjSave.IDHinhThucNghiThayDoi = IDThayDoi;
            ccObjSave.IDHinhThucNghiDonViCham = IDDonVi;
            ccFactory.SaveChanges();
            return true;
        }


        //  //////////////////////////////


        public IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien(String publicKey, String token,
    int thang, int nam, Guid nhanVienID)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> list = null;

                list = factory.QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien(thang, nam, nhanVienID);


                return list;
            }
            else
            {
                return null;
            }
        }
        public String QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien_Json(String publicKey, String token,
            int thang, int nam, Guid nhanVienID)
        {//DANG SD
            IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> list = QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien(publicKey, token, thang, nam, nhanVienID);
            String json = JsonConvert.SerializeObject(list);
            return json;

        }

    }
}
