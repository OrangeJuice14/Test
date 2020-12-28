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


        private static void GetSetChild(ChiTietChamCongNhanVien chiTietChamCongNhanVien, String ngayNghiFormat)
        {
            chiTietChamCongNhanVien.HoVaTen = chiTietChamCongNhanVien.NhanVien.HoSo.HoTen;
            chiTietChamCongNhanVien.MaNhanSu = chiTietChamCongNhanVien.NhanVien.HoSo.MaQuanLy;
            chiTietChamCongNhanVien.NgayNghiFormat = String.Format(ngayNghiFormat, chiTietChamCongNhanVien.NghiNuaNgay ?? 0,
                chiTietChamCongNhanVien.NghiCoPhep ?? 0, chiTietChamCongNhanVien.NghiRo ?? 0, chiTietChamCongNhanVien.NghiThaiSan ?? 0, chiTietChamCongNhanVien.NghiHe ?? 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="token"></param>
        /// <param name="thang">bat buoc</param>
        /// <param name="nam">bat buoc</param>
        /// <param name="idNhanVien">bat buoc</param>
        /// <returns></returns>
        public IEnumerable<DTO_QuanLyXetABC_BieuDoVaoRa> QuanLyXetABC_BieuDoVaoRa(String publicKey, String token,
           int thang, int nam, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                IEnumerable<DTO_QuanLyXetABC_BieuDoVaoRa> list = null;

                list = factory.XetABC_BieuDoVaoRa(thang, nam, idNhanVien);



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
        /// <param name="thang">bat buoc</param>
        /// <param name="nam">bat buoc</param>
        /// <param name="idNhanVien">bat buoc</param>
        /// <returns></returns>
        public String QuanLyXetABC_BieuDoVaoRa_Json(String publicKey, String token,
           int thang, int nam, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_QuanLyXetABC_BieuDoVaoRa> list = QuanLyXetABC_BieuDoVaoRa(publicKey, token, thang, nam, idNhanVien);
            String json = JsonConvert.SerializeObject(list);
            return json;

        }
        // ////////////////////////////////////////////
        public DTO_QuanLyXetABC_ChiTietTheoNhanVien QuanLyXetABC_ChiTietTheoNhanVien(String publicKey, String token,
   int thang, int nam, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongTheoNgay_Factory factory = CC_ChamCongTheoNgay_Factory.New();
                DTO_QuanLyXetABC_ChiTietTheoNhanVien obj = null;

                obj = factory.XetABC_ChiTietTheoNhanVien(thang, nam, idNhanVien);



                return obj;
            }
            else
            {
                return null;
            }
        }

        public String QuanLyXetABC_ChiTietTheoNhanVien_Json(String publicKey, String token,
   int thang, int nam, Guid idNhanVien)
        {//DANG SD
            DTO_QuanLyXetABC_ChiTietTheoNhanVien obj = QuanLyXetABC_ChiTietTheoNhanVien(publicKey, token, thang, nam, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;

        }
        #region QuanLyXetABC_Find
        //dem so mau tin
        /*
        public int QuanLyXetABC_FindCount(String publicKey, String token, int thang, int nam, Guid boPhanId, Guid? idLoaiNhanSu, bool? diHoc)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                int count = 0;
                ChiTietChamCongNhanVien_Factory factory = ChiTietChamCongNhanVien_Factory.New();
                count = factory.QuanLyXetABC_FindCount(thang, nam, boPhanId, idLoaiNhanSu, diHoc);


                return count;
            }
            else
            {
                return 0;
            }
        }
        */
        public IEnumerable<DTO_ChiTietChamCongNhanVien> QuanLyXetABC_Find(String publicKey, String token, int thang, int nam, Guid boPhanId, Guid? idLoaiNhanSu, bool? diHoc, String ngayNghiFormat)
        {

            if (Helper.TrustTest(publicKey, token))
            {
                var factory = ChiTietChamCongNhanVien_Factory.New();
                var tmpList = factory.QuanLyXetABC_Find(thang, nam, boPhanId, idLoaiNhanSu, diHoc);
                if (tmpList != null)
                {
                    foreach (var chiTietChamCongNhanVien in tmpList)
                    {
                        GetSetChild(chiTietChamCongNhanVien, ngayNghiFormat);
                    }
                    IEnumerable<DTO_ChiTietChamCongNhanVien> list = tmpList.Map<DTO_ChiTietChamCongNhanVien>().ToList();
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
        public String QuanLyXetABC_Find_Json(String publicKey, String token, int thang, int nam, Guid boPhanId, Guid? idLoaiNhanSu, bool? diHoc, String ngayNghiFormat)
        {//DANG SD
            IEnumerable<DTO_ChiTietChamCongNhanVien> list = QuanLyXetABC_Find(publicKey, token, thang, nam, boPhanId, idLoaiNhanSu, diHoc, ngayNghiFormat);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        /*
        public IEnumerable<DTO_ChiTietChamCongNhanVien> QuanLyXetABC_Find_PhanTrang(String publicKey, String token, int thang,
            int nam, Guid boPhanId, Guid? idLoaiNhanSu, Boolean? diHoc, int trang, int soMauTinMoiTrang)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                ChiTietChamCongNhanVien_Factory factory = ChiTietChamCongNhanVien_Factory.New();
                var tmpList = factory.QuanLyXetABCFind_CoPhanTrang(thang, nam, boPhanId, idLoaiNhanSu, diHoc, trang,
                    soMauTinMoiTrang);
                if (tmpList != null)
                {
                    foreach (var chiTietChamCongNhanVien in tmpList)
                    {
                        GetSetChild(chiTietChamCongNhanVien);
                    }


                    IEnumerable<DTO_ChiTietChamCongNhanVien> list = tmpList.Map<DTO_ChiTietChamCongNhanVien>();
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

        public String QuanLyXetABC_Find_PhanTrang_Json(String publicKey, String token, int thang, int nam, Guid boPhanId, Guid? idLoaiNhanSu, bool? diHoc, int trang, int soMauTinMoiTrang)
        {
            //IEnumerable<DTO_ChiTietChamCongNhanVien> list = QuanLyXetABC_Find_PhanTrang(publicKey, token, thang, nam, boPhanId, idLoaiNhanSu, diHoc, trang, soMauTinMoiTrang);
            //String json = JsonConvert.SerializeObject(list);
            //return json;

            //if (Helper.TrustTest(publicKey, token))
            //{
            //DateTime date = new DateTime(nam, thang, ngay);
            IEnumerable<DTO_ChiTietChamCongNhanVien> list = QuanLyXetABC_Find_PhanTrang(publicKey, token, thang, nam, boPhanId, idLoaiNhanSu, diHoc, trang, soMauTinMoiTrang);
            int count = QuanLyXetABC_FindCount(publicKey, token, thang, nam, boPhanId, idLoaiNhanSu, diHoc);
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

        #region Save
        // Save////////////////////////////////////////////
        public bool QuanLyXetABC_Save(String publicKey, String token, DTO_ChiTietChamCongNhanVien obj)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                if (obj != null)
                {
                    ChiTietChamCongNhanVien_Factory factory = ChiTietChamCongNhanVien_Factory.New();
                    ChiTietChamCongNhanVien objFromDB = factory.GetById(obj.Oid);
                    if (objFromDB != null && (objFromDB.Khoa ?? false) == false)
                    {
                        //    //them moi
                        //    //map sang entity
                        //    var newDBObject = factory.CreateManagedObject();
                        //    newDBObject.CopyIncludedPropertiesFrom(obj, "DanhGia", "DienGiai");
                        //    if (newDBObject.Oid == Guid.Empty)
                        //    {
                        //        newDBObject.Oid = Guid.NewGuid();
                        //    }
                        //}
                        //else
                        //{
                        //cap nhat
                        objFromDB.CopyIncludedPropertiesFrom(obj, "DanhGia", "DanhGiaTruocDieuChinh", "DienGiai", "TrangThai");
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
        public bool QuanLyXetABC_Save_Json(String publicKey, String token, string jsonObject)
        {
            //chuyen jsonObject thanh object
            DTO_ChiTietChamCongNhanVien obj = JsonConvert.DeserializeObject<DTO_ChiTietChamCongNhanVien>(jsonObject);
            return QuanLyXetABC_Save(publicKey, token, obj);
        }
        public bool QuanLyXetABC_SaveList(String publicKey, String token, List<DTO_ChiTietChamCongNhanVien> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                ChiTietChamCongNhanVien_Factory factory = ChiTietChamCongNhanVien_Factory.New();
                foreach (var obj in objList)
                {
                    if (obj != null)
                    {

                        ChiTietChamCongNhanVien objFromDB = factory.GetById(obj.Oid);
                        if (objFromDB != null && (objFromDB.Khoa ?? false) == false)
                        {
                            //    //them moi
                            //    //map sang entity
                            //    var newDBObject = factory.CreateManagedObject();
                            //    newDBObject.CopyIncludedPropertiesFrom(obj,"DanhGia", "DienGiai");
                            //    if (newDBObject.Oid == Guid.Empty)
                            //    {
                            //        newDBObject.Oid = Guid.NewGuid();
                            //    }
                            //}
                            //else
                            //{
                            //cap nhat
                            objFromDB.CopyIncludedPropertiesFrom(obj, "DanhGia", "DanhGiaTruocDieuChinh", "DienGiai", "TrangThai");
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
        public bool QuanLyXetABC_SaveList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChiTietChamCongNhanVien> objList = JsonConvert.DeserializeObject<List<DTO_ChiTietChamCongNhanVien>>(jsonObjectList);
            return QuanLyXetABC_SaveList(publicKey, token, objList);
        }
        #endregion


        public bool QuanLyXetABC_KhoaVaMoKhoaList(String publicKey, String token, List<DTO_ChiTietChamCongNhanVien> objList, bool khoa)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                ChiTietChamCongNhanVien_Factory factory = ChiTietChamCongNhanVien_Factory.New();
                foreach (var obj in objList)
                {
                    if (obj != null)
                    {

                        ChiTietChamCongNhanVien objFromDB = factory.GetById(obj.Oid);
                        if (objFromDB != null)
                        {
                            objFromDB.Khoa = khoa;
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

        public bool QuanLyXetABC_KhoaVaMoKhoaList_Json(String publicKey, String token, string jsonObjectList, bool khoa)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChiTietChamCongNhanVien> objList = JsonConvert.DeserializeObject<List<DTO_ChiTietChamCongNhanVien>>(jsonObjectList);
            return QuanLyXetABC_KhoaVaMoKhoaList(publicKey, token, objList, khoa);
        }

        public int CauHinhXetABC_GetThoiGian(String publicKey, String token,Guid Oid)
        {
                CauHinhXetABC_Factory factory = CauHinhXetABC_Factory.New();
                DTO_CauHinhXetABC obj = factory.GetCauHinhXetABC(Oid).Map<DTO_CauHinhXetABC>();
                return obj.ThoiGian??0;
        }

        public bool CauHinhXetABC_Update(String publicKey, String token, Guid Oid, int day)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CauHinhXetABC_Factory factory = CauHinhXetABC_Factory.New();
                var obj = factory.GetCauHinhXetABC(Oid);
                if (obj != null)
                {
                    obj.ThoiGian = day;
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
            }
            return false;
        }

    }
}
