using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Transactions;
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
        public IEnumerable<DTO_CC_CaChamCong> GetList_CaChamCong(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_CaChamCong_Factory factory = CC_CaChamCong_Factory.New();
                IEnumerable<DTO_CC_CaChamCong> list = factory.GetAll_GCRecordIsNull().Map<DTO_CC_CaChamCong>();
                foreach (DTO_CC_CaChamCong c in list)
                {
                    c.LoaiCa = c.LoaiCa == "1" ? "Nghỉ giữa giờ" : "Không nghỉ giữa giờ";
                }
                return list;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<CC_KyDangKyKhungGio> GetList_KyDangKy(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KyDangKyKhungGio_Factory factory = CC_KyDangKyKhungGio_Factory.New();
                IEnumerable<CC_KyDangKyKhungGio> list = factory.GetList_KyDangKy();
                return list;
            }
            else
            {
                return null;
            }
        }
        public CC_KyDangKyKhungGio GetKyDangKy(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KyDangKyKhungGio_Factory factory = CC_KyDangKyKhungGio_Factory.New();
                CC_KyDangKyKhungGio list = factory.GetKyDangKy(id);
                return list;
            }
            else
            {
                return null;
            }
        }
        public DTO_DangKyKhungGioLamViec GetDangKyByIDNV(String publicKey, String token, Guid IDNV)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_DangKyKhungGioLamViec_Factory factory = CC_DangKyKhungGioLamViec_Factory.New();
                DTO_DangKyKhungGioLamViec list = factory.GetDangKyByIDNV(IDNV).Map<DTO_DangKyKhungGioLamViec>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public CC_ThoiGianDangKyKhungGioLamViec GetList_ThoiGianDangKy(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ThoiGianDangKy_Factory factory = CC_ThoiGianDangKy_Factory.New();
                CC_ThoiGianDangKyKhungGioLamViec list = factory.GetList_ThoiGianDangKy();
                return list;
            }
            else
            {
                return null;
            }
        }
        public bool CaChamCong_CheckDangSuDung(String publicKey, String token, Guid Oid)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_ChamCongTheoNgay_Factory.New();
                bool khoaCC = factory.CheckDangSuDung(Oid);
                return khoaCC;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool CaChamCong_Delete(String publicKey, String token, Guid Oid)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_CaChamCong_Factory factory = CC_CaChamCong_Factory.New();
                    CC_CaChamCong obj = new CC_CaChamCong() { Oid = Oid };
                    factory.Attach(obj);
                    CC_CaChamCong_Factory.FullDelete(factory.Context, obj);
                    //////////////
                    try
                    {
                        factory.SaveChangesWithoutTransactionScope();
                        transaction.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return false;
        }


        public string DangKyKhungGio_GetDuLieuDaDangKy(String publicKey, String token, Guid idNhanVien)
        {
            string result = string.Empty;
            if (Helper.TrustTest(publicKey, token))
            {
                CC_DangKyKhungGioLamViec_Factory factory = CC_DangKyKhungGioLamViec_Factory.New();
                result = factory.DangKyKhungGio_GetDuLieuDaDangKy(idNhanVien);
            }
            return result;
        }
        public DTO_CC_CaChamCong CaChamCong_GetByID(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_CaChamCong_Factory factory = CC_CaChamCong_Factory.New();
                DTO_CC_CaChamCong obj = factory.GetByID(id).Map<DTO_CC_CaChamCong>();

                obj.GioVaoSang = ParseHourDTO(obj.ThoiGianVaoSang);
                obj.PhutVaoSang = ParseMinuteDTO(obj.ThoiGianVaoSang);

                obj.GioRaSang = ParseHourDTO(obj.ThoiGianRaSang);
                obj.PhutRaSang = ParseMinuteDTO(obj.ThoiGianRaSang);

                obj.GioBatDauNghi = ParseHourDTO(obj.ThoiGianBatDauNghiGiuaCa);
                obj.PhutBatDauNghi = ParseMinuteDTO(obj.ThoiGianBatDauNghiGiuaCa);

                obj.GioKetThucNghi = ParseHourDTO(obj.ThoiGianKetThucNghiGiuaCa);
                obj.PhutKetThucNghi = ParseMinuteDTO(obj.ThoiGianKetThucNghiGiuaCa);

                obj.GioVaoChieu = ParseHourDTO(obj.ThoiGianVaoChieu);
                obj.PhutVaoChieu = ParseMinuteDTO(obj.ThoiGianVaoChieu);

                obj.GioRaChieu = ParseHourDTO(obj.ThoiGianRaChieu);
                obj.PhutRaChieu = ParseMinuteDTO(obj.ThoiGianRaChieu);

                obj.GioBatDauQuet = ParseHourDTO(obj.ThoiGianBDQuetBuoiSang);
                obj.PhutBatDauQuet = ParseMinuteDTO(obj.ThoiGianBDQuetBuoiSang);

                obj.GioKetThucQuet = ParseHourDTO(obj.ThoiGianKTQuetBuoiChieu);
                obj.PhutKetThucQuet = ParseMinuteDTO(obj.ThoiGianKTQuetBuoiChieu);
                return obj;
            }
            else
            {
                return null;
            }
        }
        public List<DTO_DangKyKhungGioLamViec> DangKyChamCong_Find(String publicKey, String token, Guid? bophan, Guid ky, int trangthai)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_DangKyKhungGioLamViec_Factory factory = CC_DangKyKhungGioLamViec_Factory.New();
                List<DTO_DangKyKhungGioLamViec> obj = factory.DangKyChamCong_Find(bophan, ky, trangthai);
                return obj;
            }
            else
            {
                return null;
            }
        }

        public List<DTO_DangKyKhungGioLamViec> ThongKeKhungGioLamViec_Find(String publicKey, String token, Guid ky, Guid bophan,  string manhansu)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_DangKyKhungGioLamViec_Factory factory = CC_DangKyKhungGioLamViec_Factory.New();
                List<DTO_DangKyKhungGioLamViec> obj = factory.ThongKeKhungGioLamViec_Find(ky, bophan,  manhansu);
                return obj;
            }
            else
            {
                return null;
            }
        }

        public string ParseHourDTO(string time)
        {
            string result = "";
            result = time != null ? time.Substring(0, 2) : null;
            return result;
        }
        public string ParseMinuteDTO(string time)
        {
            string result = "";
            result = time != null ? time.Substring(3, 2) : null;
            return result;
        }
        public String GetList_CaChamCong_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_CC_CaChamCong> list = GetList_CaChamCong(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetList_KyDangKy_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_KyDangKyKhungGio> list = GetList_KyDangKy(publicKey, token).Map<DTO_KyDangKyKhungGio>();
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetKyDangKy_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_KyDangKyKhungGio list = GetKyDangKy(publicKey, token, id).Map<DTO_KyDangKyKhungGio>();
            list.NgayString="(" + String.Format("{0:dd/MM/yyyy}", list.TuNgay) + " - " + String.Format("{0:dd/MM/yyyy}", list.DenNgay) + ")";
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetDangKyByIDNV_Json(String publicKey, String token, Guid IDNV)
        {//DANG SD
            DTO_DangKyKhungGioLamViec list = GetDangKyByIDNV(publicKey, token, IDNV);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetList_ThoiGianDangKy_Json(String publicKey, String token)
        {//DANG SD
            CC_ThoiGianDangKyKhungGioLamViec list = GetList_ThoiGianDangKy(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String CaChamCong_GetByID_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_CC_CaChamCong obj = CaChamCong_GetByID(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public String DangKyChamCong_Find_Json(String publicKey, String token, Guid? bophan, Guid ky, int trangthai)
        {//DANG SD
            List<DTO_DangKyKhungGioLamViec> obj = DangKyChamCong_Find(publicKey, token, bophan, ky, trangthai);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public String ThongKeKhungGioLamViec_Find_Json(String publicKey, String token, Guid ky, Guid bophan, string manhansu)
        {//DANG SD
            List<DTO_DangKyKhungGioLamViec> obj = ThongKeKhungGioLamViec_Find(publicKey, token, ky, bophan, manhansu);
            //
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public String ParseTimeString(int Gio, int Phut)
        {
            String result = "";
            String GioString = "";
            String PhutString = "";
            GioString = Gio < 10 ? "0" + Gio.ToString() : Gio.ToString();
            PhutString = Phut < 10 ? "0" + Phut.ToString() : Phut.ToString();
            result = GioString + ":" + PhutString + ":00";
            return result;
        }
        public decimal ParseTime(int Gio, int Phut)
        {
            decimal result = Convert.ToDecimal(Gio + Phut * 0.01666666667);
            return Math.Round(result, 2);
        }
        public bool CaChamCong_Save(String publicKey, String token, Guid Oid, String TenCa, byte LoaiCa,
            int GioVaoSang, int PhutVaoSang, int GioRaSang, int PhutRaSang,
            int? GioBatDauNghi, int? PhutBatDauNghi, int? GioKetThucNghi, int? PhutKetThucNghi,
            int GioVaoChieu, int PhutVaoChieu, int GioRaChieu, int PhutRaChieu,
            int GioBatDauQuet, int PhutBatDauQuet, int GioKetThucQuet, int PhutKetThucQuet,
            int SoPhutCong, int SoPhutTru)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_CaChamCong_Factory factory = CC_CaChamCong_Factory.New();
                if (Oid != Guid.Empty)
                {
                    CC_CaChamCong obj = factory.GetByID(Oid);
                    if (obj != null)
                    {
                        obj.TenCa = TenCa;
                        obj.SoPhutCong = SoPhutCong;
                        obj.SoPhutTru = SoPhutTru;
                        obj.ThoiGianVaoSang = ParseTimeString(GioVaoSang, PhutVaoSang);
                        obj.ThoiGianRaSang = ParseTimeString(GioRaSang, PhutRaSang);
                        if (GioBatDauNghi != null && PhutBatDauNghi != null && GioKetThucNghi != null && PhutKetThucNghi != null)
                        {
                            int GBDN = GioBatDauNghi ?? default(int);
                            int PBDN = PhutBatDauNghi ?? default(int);
                            int GKTN = GioKetThucNghi ?? default(int);
                            int PKTN = PhutKetThucNghi ?? default(int);
                            obj.ThoiGianBatDauNghiGiuaCa = ParseTimeString(GBDN, PBDN);
                            obj.ThoiGianKetThucNghiGiuaCa = ParseTimeString(GKTN, PKTN);
                            obj.TongSoGioNghiGiuaCa = ParseTime(GKTN, PKTN) - ParseTime(GBDN, PBDN);
                        }
                        else
                        {
                            obj.ThoiGianBatDauNghiGiuaCa = null;
                            obj.ThoiGianKetThucNghiGiuaCa = null;
                        }

                        obj.ThoiGianVaoChieu = ParseTimeString(GioVaoChieu, PhutVaoChieu);
                        obj.ThoiGianRaChieu = ParseTimeString(GioRaChieu, PhutRaChieu);

                        obj.ThoiGianBDQuetBuoiSang = ParseTimeString(GioBatDauQuet, PhutBatDauQuet);
                        obj.ThoiGianKTQuetBuoiChieu = ParseTimeString(GioKetThucQuet, PhutKetThucQuet);

                        obj.TongSoGioLamViecBuoiSang = ParseTime(GioRaSang, PhutRaSang) - ParseTime(GioVaoSang, PhutVaoSang);
                        obj.TongSoGioLamViecBuoiChieu = ParseTime(GioRaChieu, PhutRaChieu) - ParseTime(GioVaoChieu, PhutVaoChieu);
                        obj.TongSoGioLamViecCaNgay = obj.TongSoGioLamViecBuoiSang + obj.TongSoGioLamViecBuoiChieu;
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
                else
                {
                    CC_CaChamCong obj = factory.CreateManagedObject();
                    obj.Oid = Guid.NewGuid();
                    obj.TenCa = TenCa;
                    obj.SoPhutCong = SoPhutCong;
                    obj.SoPhutTru = SoPhutTru;
                    obj.ThoiGianVaoSang = ParseTimeString(GioVaoSang, PhutVaoSang);
                    obj.ThoiGianRaSang = ParseTimeString(GioRaSang, PhutRaSang);
                    if (GioBatDauNghi != null && PhutBatDauNghi != null && GioKetThucNghi != null && PhutKetThucNghi != null)
                    {
                        int GBDN = GioBatDauNghi ?? default(int);
                        int PBDN = PhutBatDauNghi ?? default(int);
                        int GKTN = GioKetThucNghi ?? default(int);
                        int PKTN = PhutKetThucNghi ?? default(int);
                        obj.ThoiGianBatDauNghiGiuaCa = ParseTimeString(GBDN, PBDN);
                        obj.ThoiGianKetThucNghiGiuaCa = ParseTimeString(GKTN, PKTN);
                        obj.TongSoGioNghiGiuaCa = ParseTime(GKTN, PKTN) - ParseTime(GBDN, PBDN);
                    }
                    else
                    {
                        obj.ThoiGianBatDauNghiGiuaCa = null;
                        obj.ThoiGianKetThucNghiGiuaCa = null;
                    }
                    obj.ThoiGianVaoChieu = ParseTimeString(GioVaoChieu, PhutVaoChieu);
                    obj.ThoiGianRaChieu = ParseTimeString(GioRaChieu, PhutRaChieu);

                    obj.ThoiGianBDQuetBuoiSang = ParseTimeString(GioBatDauQuet, PhutBatDauQuet);
                    obj.ThoiGianKTQuetBuoiChieu = ParseTimeString(GioKetThucQuet, PhutKetThucQuet);

                    obj.TongSoGioLamViecBuoiSang = ParseTime(GioRaSang, PhutRaSang) - ParseTime(GioVaoSang, PhutVaoSang);
                    obj.TongSoGioLamViecBuoiChieu = ParseTime(GioRaChieu, PhutRaChieu) - ParseTime(GioVaoChieu, PhutVaoChieu);
                    obj.TongSoGioLamViecCaNgay = obj.TongSoGioLamViecBuoiSang + obj.TongSoGioLamViecBuoiChieu;
                    obj.Active = true;
                    obj.LoaiCa = LoaiCa;
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
            }
            return false;
        }
        public bool ThoiGianDangKy_Save(String publicKey, String token, CC_ThoiGianDangKyKhungGioLamViec obj)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ThoiGianDangKy_Factory factory = CC_ThoiGianDangKy_Factory.New();
                if (obj.Oid != Guid.Empty)
                {
                    CC_ThoiGianDangKyKhungGioLamViec objFromDB = factory.CreateAloneObject();
                    objFromDB = factory.GetThoiGianDangKy(obj.Oid);
                    if (objFromDB != null)
                    {
                        objFromDB.TuNgay = obj.TuNgay;
                        objFromDB.DenNgay = obj.DenNgay;
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
            }
            return false;
        }
        public bool DangKyChamCong_UpdateAll(String publicKey, String token, Guid ky, Guid ca)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_DangKyKhungGioLamViec_Factory factory = CC_DangKyKhungGioLamViec_Factory.New();
                CC_ChamCongTheoNgay_Factory ccfact = CC_ChamCongTheoNgay_Factory.New();
                List<DTO_DangKyKhungGioLamViec> temp = factory.DangKyChamCong_Find(null, ky, 0);
                using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(180)))
                {
                    try
                    {
                        foreach (DTO_DangKyKhungGioLamViec obj in temp)
                        {
                            CC_DangKyKhungGioLamViec dk = factory.CreateManagedObject();
                            dk.Oid = Guid.NewGuid();

                            dk.ThongTinNhanVien = obj.ThongTinNhanVien;
                            dk.CaChamCong = ca;
                            dk.KyDangKy = ky;
                            factory.SaveChanges();
                        }
                        transaction.Complete();
                        //
                        return true;
                    }
                    catch (Exception ex) { throw ex; }
                }
            }
            return false;
        }

        public bool DangKyKhungGioLamViec_Save(String publicKey, String token, DTO_DangKyKhungGioLamViec obj)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_DangKyKhungGioLamViec_Factory factory = CC_DangKyKhungGioLamViec_Factory.New();
                CC_DangKyKhungGioLamViec objFromDB = factory.CreateAloneObject();
                objFromDB = factory.GetDangKyByKy(obj.KyDangKy.Value, obj.ThongTinNhanVien.Value);
                if (objFromDB != null)
                {
                    objFromDB.CaChamCong = obj.CaChamCong;
                    objFromDB.KyDangKy = obj.KyDangKy;
                }
                else
                {
                    objFromDB = factory.CreateManagedObject();
                    objFromDB.Oid = Guid.NewGuid();
                    objFromDB.ThongTinNhanVien = obj.ThongTinNhanVien;
                    objFromDB.CaChamCong = obj.CaChamCong;
                    objFromDB.KyDangKy = obj.KyDangKy;
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
        public bool KyDangKyKhungGio_New(String publicKey, String token, Guid id, String tenky, DateTime tuNgay, DateTime denNgay)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    CC_KyDangKyKhungGio_Factory factory = CC_KyDangKyKhungGio_Factory.New();
                    CC_KyDangKyKhungGio newObj = factory.CreateManagedObject();
                    newObj.Oid = id != Guid.Empty ? newObj.Oid : Guid.NewGuid();
                    newObj.TenKy = tenky;
                    newObj.TuNgay = tuNgay;
                    newObj.DenNgay = denNgay;
                    factory.SaveChanges();

                    return true;
                }
                catch (Exception ex) { throw ex; }
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
            //
            return false;
        }
        public bool DangKyKhungGio_CheckChot(String publicKey, String token)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = CC_ThoiGianDangKy_Factory.New();
                bool hople = factory.DangKyKhungGio_CheckChot();
                return hople;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool DangKyKhungGio_CheckNgoaiThoiGian(String publicKey, String token)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = CC_ThoiGianDangKy_Factory.New();
                bool hople = factory.DangKyKhungGio_CheckNgoaiThoiGian();
                return hople;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool KyDangKy_KiemTraTuNgayDenNgayCoHopLe(String publicKey, String token, Guid? Oid, DateTime tuNgay, DateTime denNgay)
        {//DANG SD
            tuNgay = tuNgay.Date;
            denNgay = denNgay.Date;
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KyDangKyKhungGio_Factory factory = CC_KyDangKyKhungGio_Factory.New();
                bool isNew = true;
                bool result = true;
                if (Oid != null)
                {
                    var objFromDb = factory.GetKyDangKy(Oid.Value);
                    if (objFromDb != null)
                        isNew = false;
                }

                if (isNew)
                {
                    CC_KyDangKyKhungGio temp = (from o in factory.Context.CC_KyDangKyKhungGio
                                                where (o.TuNgay <= tuNgay && o.DenNgay >= tuNgay)
                                                ||
                                                (o.TuNgay <= denNgay && o.DenNgay >= denNgay)
                                                select o).FirstOrDefault();
                    if (temp != null)
                    {
                        result = false;
                    }
                }
                return result;

            }
            return false;
        }
        public bool KyDangKy_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<CC_KyDangKyKhungGio> objList = JsonConvert.DeserializeObject<List<CC_KyDangKyKhungGio>>(jsonObjectList);
            return KyDangKy_DeleteList(publicKey, token, objList);
        }
        public bool KyDangKy_DeleteList(String publicKey, String token, List<CC_KyDangKyKhungGio> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_KyDangKyKhungGio_Factory factory = CC_KyDangKyKhungGio_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {

                            CC_KyDangKyKhungGio stupidObj = new CC_KyDangKyKhungGio() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            bool check = factory.CheckExist(obj.Oid);
                            if (!check)
                            {
                                CC_KyDangKyKhungGio_Factory.FullDelete(factory.Context, stupidObj);
                            }
                        }
                    }
                    //////////////
                    try
                    {
                        factory.SaveChangesWithoutTransactionScope();
                        transaction.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return false;
        }
    }
}
