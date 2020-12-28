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
        public String QuanLyChamCong_ThongTinChamCongThang_Json(String publicKey, String token,
            int thang, int nam, Guid boPhanId, String maNhanSu, Guid? idLoaiNhanSu)
        {//DANG SD
            IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> list = QuanLyChamCong_ThongTinChamCongThang(publicKey, token, thang, nam, boPhanId, maNhanSu, idLoaiNhanSu);
            String json = JsonConvert.SerializeObject(list);
            return json;

        }

        public bool QuanLyChamCong_ThongTinChamCongThang_Save(String publicKey, String token,
            DTO_QuanLyChamCong_ThongTinChamCongThang thongTinChamCongThang)
        {
            if (thongTinChamCongThang != null && thongTinChamCongThang.ChiTietChamCong != null)
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                foreach (var obj in thongTinChamCongThang.ChiTietChamCong)
                {

                    //lay doi tuong CC_ChamCongTheoNgay
                    CC_ChamCongTheoNgay objFromDbForSave = factory.GetByID(obj.CC_ChamCongTheoNgayOid);
                    //phan tich hinh thuc nghi 
                    Guid idHinhThucNghiMoi = HinhThucNghiConst.HinhThucNghiDictionaryKyHieuToId[obj.MaHinhThucNghi.ToUpper()];
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
            List<DTO_QuanLyChamCong_ThongTinChamCongThang> objList)
        {
            CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
            foreach (var thongTinChamCongThang in objList)
            {


                if (thongTinChamCongThang.ChiTietChamCong != null)
                {

                    foreach (var obj in thongTinChamCongThang.ChiTietChamCong)
                    {

                        //lay doi tuong CC_ChamCongTheoNgay
                        CC_ChamCongTheoNgay objFromDbForSave = factory.GetByID(obj.CC_ChamCongTheoNgayOid);
                        //phan tich hinh thuc nghi 
                        Guid idHinhThucNghiMoi =
                            HinhThucNghiConst.HinhThucNghiDictionaryKyHieuToId[obj.MaHinhThucNghi.ToUpper()];
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
        public bool QuanLyChamCong_ThongTinChamCongThang_SaveList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_QuanLyChamCong_ThongTinChamCongThang> objList = JsonConvert.DeserializeObject<List<DTO_QuanLyChamCong_ThongTinChamCongThang>>(jsonObjectList);
            return QuanLyChamCong_ThongTinChamCongThang_SaveList(publicKey, token, objList);
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
