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
using ERP_Core.Common;

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
        public IEnumerable<DTO_QuanLyChamCong_Find> QuanLyChamCong_Find(String publicKey, String token, int ngay, int thang, int nam, Guid? boPhanId, int trangThaiChamCong, Boolean? diHoc, String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_QuanLyChamCong_Find> list = null;
                if (ngay > 0)
                {
                    DateTime date = new DateTime(nam, thang, ngay);
                    list = factory.FindForQuanlyChamCong(date, boPhanId,trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu, congTy);
                }
                else
                {
                    list = factory.FindForQuanlyChamChong_ThangVaNam(thang, nam, boPhanId, trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu, congTy);
                }
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
        public String QuanLyChamCong_Find_Json(String publicKey, String token, int ngay, int thang, int nam, Guid? boPhanId, int trangThaiChamCong, Boolean? diHoc, String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu, Guid congTy)
        {//DANG SD
            IEnumerable<DTO_QuanLyChamCong_Find> list = QuanLyChamCong_Find(publicKey, token, ngay, thang, nam, boPhanId, trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu,congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public bool QuanLyChamCong_CheckChot(String publicKey, String token, Guid boPhanID, int thang, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_ChiTietChamCongNhanVien_Factory.New();
                bool daTonTai = factory.CheckChot(thang, nam, boPhanID);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }

        public bool ChotChamCongThang_ChamCongThangCheckLock(String publicKey, String token, int thang, int nam, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_QuanLyChamCongNhanVien_Factory.New();
                bool daTonTai = factory.CheckKhoaQLCCNV(thang, nam,congTy);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }

        public string QuanLyChamCong_CheckDangKyKhungGio(String publicKey, String token, Guid Oid, int ngay, int thang, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                bool daDangKy = false;
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                CC_ChamCongTheoNgay cc = factory.GetByID(Oid);
                if (cc != null)
                {
                    DateTime date = new DateTime(nam, thang, ngay);
                    daDangKy = factory.CheckDangKyKhungGio(date, cc.IDNhanVien);
                }
                String json = JsonConvert.SerializeObject(daDangKy);
                return json; ;
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
        public bool QuanLyChamCong_RemoveList(String publicKey, String token, List<DTO_QuanLyChamCong_Find> objList)
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
                            objFromDB.DaChamCong = false;
                            objFromDB.CC_HinhThucNghi = null;
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
        public bool QuanLyChamCong_DoiCaChamCong(String publicKey, String token, List<DTO_QuanLyChamCong_Find> objList,Guid caChamCongId)
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
                            if (caChamCongId!=Guid.Empty)
                            {
                                objFromDB.CC_CaChamCong = caChamCongId;
                            }
                            else
                            {
                                objFromDB.CC_CaChamCong =null;
                            }
                            
                        }

                    }
                }
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
        public bool QuanLyChamCong_RemoveList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_QuanLyChamCong_Find> objList = JsonConvert.DeserializeObject<List<DTO_QuanLyChamCong_Find>>(jsonObjectList);
            return QuanLyChamCong_RemoveList(publicKey, token, objList);
        }
        public bool QuanLyChamCong_DoiCaChamCong_Json(String publicKey, String token, string jsonObjectList, Guid caChamCongId)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_QuanLyChamCong_Find> objList = JsonConvert.DeserializeObject<List<DTO_QuanLyChamCong_Find>>(jsonObjectList);
            return QuanLyChamCong_DoiCaChamCong(publicKey, token, objList, caChamCongId);
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

        public IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThang(String publicKey, String token, int thang, int nam, Guid boPhanId, String maNhanSu, Guid idLoaiNhanSu,Guid userId, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> list = null;
                //
                list = factory.QuanLyChamCong_ThongTinChamCongThang(thang, nam, boPhanId, maNhanSu, idLoaiNhanSu, userId,congTy);
                //
                return list;
            }
            else
            {
                return null;
            }
        }
        public String QuanLyChamCong_ThongTinChamCongThang_Json(String publicKey, String token, int thang, int nam, Guid boPhanId, String maNhanSu, Guid idLoaiNhanSu, Guid userId, Guid congTy)
        {
            IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> list = QuanLyChamCong_ThongTinChamCongThang(publicKey, token, thang, nam, boPhanId, maNhanSu, idLoaiNhanSu,userId,congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public String QuanLyChamCong_GetDepartmentOfStaff(Guid idNhanVien)
        {//DANG SD
            CC_QuanLyChamCongNhanVien_Factory factory = CC_QuanLyChamCongNhanVien_Factory.New();
            return factory.QuanLyChamCong_GetDepartmentOfStaff(idNhanVien);

        }

        public bool QuanLyChamCong_ThongTinChamCongThang_Save(String publicKey, String token,DTO_QuanLyChamCong_ThongTinChamCongThang thongTinChamCongThang)
        {
            if (thongTinChamCongThang != null && thongTinChamCongThang.ChiTietChamCong != null)
            {
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                foreach (var obj in thongTinChamCongThang.ChiTietChamCong)
                {
                    //lay doi tuong CC_ChamCongTheoNgay
                    CC_ChamCongTheoNgay objFromDbForSave = factory.GetByID(obj.CC_ChamCongTheoNgayOid);
                    //phan tich hinh thuc nghi 
                    Guid idHinhThucNghiMoi = new CC_HinhThucNghi_Factory().GetByKyHieu(obj.MaHinhThucNghi).Oid;
                    //
                    if (objFromDbForSave != null && idHinhThucNghiMoi != (objFromDbForSave.IDHinhThucNghi ?? Guid.Empty) )
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

        public bool QuanLyChamCong_ThongTinChamCongThang_SaveList(String publicKey, String token, List<DTO_QuanLyChamCong_ThongTinChamCongThang> objList)
        {
            CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
            foreach (var thongTinChamCongThang in objList)
            {
                //
                if (thongTinChamCongThang.ChiTietChamCong != null)
                {
                    //
                    foreach (var obj in thongTinChamCongThang.ChiTietChamCong)
                    {
                        //Lấy chấm công theo ngày
                        CC_ChamCongTheoNgay objFromDbForSave = factory.GetByID(obj.CC_ChamCongTheoNgayOid);
                        obj.MaHinhThucNghi= obj.MaHinhThucNghi == null ? "" : obj.MaHinhThucNghi;

                        //1. Trường hợp cả ngày
                        if (obj.MaHinhThucNghi.Trim().Length == 1 || obj.MaHinhThucNghi == "")
                        {
                            Guid idHinhThucNghiMoi = new CC_HinhThucNghi_Factory().GetByKyHieu(obj.MaHinhThucNghi).Oid;
                            //
                            if (objFromDbForSave != null && idHinhThucNghiMoi != (objFromDbForSave.IDHinhThucNghi ?? Guid.Empty))
                            {
                                if (idHinhThucNghiMoi != Guid.Empty)
                                {
                                    objFromDbForSave.IDHinhThucNghi = idHinhThucNghiMoi;
                                    objFromDbForSave.NguoiDungChinhSua = true;
                                }
                            }
                        }
                        //2. Trường hợp nữa ngày
                        else
                        {
                            string hinhThucNghiSang = obj.MaHinhThucNghi.Substring(0,1);
                            string hinhThucNghiChieu = obj.MaHinhThucNghi.Substring(3,1);
                            //
                            Guid idHinhThucNghiSang = new CC_HinhThucNghi_Factory().GetByKyHieu(hinhThucNghiSang).Oid;
                            Guid idHinhThucNghiChieu = new CC_HinhThucNghi_Factory().GetByKyHieu(hinhThucNghiChieu).Oid;
                            //
                            if (idHinhThucNghiSang != null && idHinhThucNghiChieu != null)
                            {
                                if (objFromDbForSave != null && objFromDbForSave.CC_HinhThucKhac != null)
                                {
                                    objFromDbForSave.CC_HinhThucKhac.HinhThucSang = idHinhThucNghiSang;
                                    objFromDbForSave.CC_HinhThucKhac.HinhThucChieu = idHinhThucNghiChieu;
                                    objFromDbForSave.CC_HinhThucKhac.MaQuanLy = obj.MaHinhThucNghi;
                                }
                                else
                                {
                                    CC_HinhThucKhac_Factory factory_HTKhac = new CC_HinhThucKhac_Factory();
                                    CC_HinhThucKhac hinhThucKhac = factory_HTKhac.CreateManagedObject();
                                    hinhThucKhac.Oid = Guid.NewGuid();
                                    hinhThucKhac.HinhThucSang = idHinhThucNghiSang;
                                    hinhThucKhac.HinhThucChieu = idHinhThucNghiChieu;
                                    hinhThucKhac.MaQuanLy = obj.MaHinhThucNghi;
                                    factory_HTKhac.SaveChanges();
                                    //
                                    objFromDbForSave.IDHinhThucKhac = hinhThucKhac.Oid;
                                }
                                //
                                objFromDbForSave.NguoiDungChinhSua = true;
                            }
                        }
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
        }
        public bool QuanLyChamCong_ThongTinChamCongThang_SaveList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_QuanLyChamCong_ThongTinChamCongThang> objList = JsonConvert.DeserializeObject<List<DTO_QuanLyChamCong_ThongTinChamCongThang>>(jsonObjectList);
            return QuanLyChamCong_ThongTinChamCongThang_SaveList(publicKey, token, objList);
        }
        public bool QuanLyChamCong_CapNhatKhungGioLamViec(String publicKey, String token, Guid Oid, int ngay,int thang,int nam,int loai,Guid ca)
        {//DANG SD
            CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
            CC_ChamCongTheoNgay cc = factory.GetByID(Oid);
            if (cc!=null)
            {
                DateTime ngayDoiCa = new DateTime(nam, thang, ngay);
                //
                if (loai==1)
                {
                    //Cập nhật cho cả tháng
                    var listcc = factory.GetBy_ThangNam_HoSoId(thang, nam, cc.IDNhanVien);

                    //Ngày trước đó
                    DateTime ngayTruocDo = HamDungChung.SetTime(ngayDoiCa,2).AddDays(-1);
                    var objTruocDo = factory.GetBy_DateNVId(ngayTruocDo, cc.IDNhanVien);
                    //
                    if (objTruocDo == null || (objTruocDo != null && objTruocDo.CC_CaChamCong != ca))
                    {
                        foreach (CC_ChamCongTheoNgay cctn in listcc)
                        {
                            if (!cctn.DaChamCong)
                            {
                                cctn.CC_CaChamCong = ca;
                                cctn.NgayDoiCa = HamDungChung.SetTime(ngayDoiCa, 2);
                            }
                        }
                    }
                    else
                    {
                        foreach (CC_ChamCongTheoNgay cctn in listcc)
                        {
                            if (!cctn.DaChamCong)
                            {
                                cctn.CC_CaChamCong = objTruocDo.CC_CaChamCong;
                                cctn.NgayDoiCa = objTruocDo.NgayDoiCa;
                            }
                        }
                    }
                    //
                    factory.SaveChanges();
                }
               else
                {  
                    var objCurrent = factory.CreateAloneObject();
                    objCurrent = factory.GetBy_DateNVId(ngayDoiCa, cc.IDNhanVien);
                    //
                    if (objCurrent != null && !objCurrent.DaChamCong)
                    {
                        objCurrent.CC_CaChamCong = ca;

                        //Ngày trước đó
                        DateTime ngayTruocDo = ngayDoiCa.AddDays(-1);
                        var objTruocDo = factory.GetBy_DateNVId(ngayTruocDo, cc.IDNhanVien);
                        if (objTruocDo.CC_CaChamCong != ca)
                        {
                            objCurrent.NgayDoiCa = ngayDoiCa;
                        }
                        else
                        {
                            objCurrent.NgayDoiCa = objTruocDo.NgayDoiCa;
                        }
                        //Cập nhật ngày đổi ca tiếp theo là một ngày khác thì mới đúng
                        DateTime tuNgay = ngayDoiCa.AddDays(1);
                        DateTime denNgay = HamDungChung.SetTime(ngayDoiCa, 3); //Lấy hết tháng này
                        IQueryable<CC_ChamCongTheoNgay> objNextList = factory.GetListBy_DateAndId(tuNgay, denNgay, cc.IDNhanVien);
                        foreach (CC_ChamCongTheoNgay itemNext in objNextList)
                        {
                            itemNext.NgayDoiCa = tuNgay;
                        }
                    }         
                    //
                    factory.SaveChanges();
                }
            }
            return true;
        }
        //  //////////////////////////////


        public IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien(String publicKey, String token, int thang, int nam, Guid nhanVienID, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> list = null;

                list = factory.QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien(thang, nam, nhanVienID,congTy);
                //
                return list;
            }
            else
            {
                return null;
            }
        }
        public String QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien_Json(String publicKey, String token, int thang, int nam, Guid nhanVienID, Guid congTy)
        {
            //
            IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> list = QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien(publicKey, token, thang, nam, nhanVienID,congTy);
            //
            String json = JsonConvert.SerializeObject(list);
            return json;

        }

        public bool GetDuLieuTuMayChamCong_Json(String publicKey, String token, DateTime tungay, DateTime denngay)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                if (tungay != DateTime.MinValue && denngay != DateTime.MinValue)
                {
                    try
                    {
                        CC_ChamCongTheoNgay_Factory factory = new CC_ChamCongTheoNgay_Factory();
                        factory.Context.spd_WebChamCong_LayDuLieuChamCongTuMayChamCong(tungay, denngay);
                        //
                        return true;
                    }
                    catch (Exception ex) { }
                }
            }
            //
            return false;
        }
        public bool ChotDuLieuTuMayChamCongTuNgay_DenNgay_Json(String publicKey, String token,Guid idNhanVien, DateTime tungay, DateTime denngay, bool type)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                if (tungay != DateTime.MinValue && denngay != DateTime.MinValue)
                {
                    try
                    {
                        CC_ChamCongTheoNgay_Factory factory = new CC_ChamCongTheoNgay_Factory();

                        if (type) // Chốt
                        {
                           
                            factory.Context.spd_WebChamCong_ChotChamCongTuNgay_DenNgay(tungay, denngay,idNhanVien,idNhanVien == Guid.Empty ? true : false);
                        }
                        else //Hủy chốt
                        {
                            factory.Context.spd_WebChamCong_HuyChotChamCongTuNgay_DenNgay(tungay, denngay, idNhanVien,idNhanVien == Guid.Empty ? true : false);
                        }
                        //
                        return true;
                    }
                    catch (Exception ex) { throw ex; }
                }
            }
            //
            return false;
        }
        public String GetList_NgayTrongKyChamCong_Json(String publicKey, String token, int thang, int nam, Guid bophanId, Guid? webGroupId, Guid congTy)
        {
            IEnumerable<DTO_NgayChamCong> list = GetList_NgayTrongKyChamCong(publicKey, token, thang, nam, bophanId, webGroupId,congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;

        }
        public IEnumerable<DTO_NgayChamCong> GetList_NgayTrongKyChamCong(String publicKey, String token, int thang, int nam, Guid bophanId, Guid? webGroupId, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_NgayChamCong> list = null;
                //
                list = factory.GetList_NgayTrongKyChamCong(thang, nam,bophanId,webGroupId,congTy);
                //
                return list;
            }
            else
            {
                return null;
            }
        }
    }
}
