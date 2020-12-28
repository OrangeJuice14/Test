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
        public DTO_CC_CaChamCong CaChamCong_GetByID(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_CaChamCong_Factory factory = CC_CaChamCong_Factory.New();
                DTO_CC_CaChamCong obj = factory.GetByID(id).Map<DTO_CC_CaChamCong>();

                obj.GioVaoSang = obj.ThoiGianVaoSang.Substring(0, 2);
                obj.PhutVaoSang = obj.ThoiGianVaoSang.Substring(3, 2);

                obj.GioRaSang = obj.ThoiGianRaSang.Substring(0, 2);
                obj.PhutRaSang = obj.ThoiGianRaSang.Substring(3, 2);

                obj.GioBatDauNghi = obj.ThoiGianBatDauNghiGiuaCa.Substring(0, 2);
                obj.PhutBatDauNghi = obj.ThoiGianBatDauNghiGiuaCa.Substring(3, 2);

                obj.GioKetThucNghi = obj.ThoiGianKetThucNghiGiuaCa.Substring(0, 2);
                obj.PhutKetThucNghi = obj.ThoiGianKetThucNghiGiuaCa.Substring(3, 2);

                obj.GioVaoChieu = obj.ThoiGianVaoChieu.Substring(0, 2);
                obj.PhutVaoChieu = obj.ThoiGianVaoChieu.Substring(3, 2);

                obj.GioRaChieu = obj.ThoiGianRaChieu.Substring(0, 2);
                obj.PhutRaChieu = obj.ThoiGianRaChieu.Substring(3, 2);

                return obj;
            }
            else
            {
                return null;
            }
        }
        public String GetList_CaChamCong_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_CC_CaChamCong> list = GetList_CaChamCong(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public String CaChamCong_GetByID_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_CC_CaChamCong obj = CaChamCong_GetByID(publicKey, token, id);
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
            decimal result = Convert.ToDecimal(Gio + Phut / 60);
            return Math.Round(result, 1);
        }
        public bool CaChamCong_Save(String publicKey, String token, Guid Oid, String TenCa,
            int GioVaoSang, int PhutVaoSang, int GioRaSang, int PhutRaSang,
            int GioBatDauNghi, int PhutBatDauNghi, int GioKetThucNghi, int PhutKetThucNghi,
            int GioVaoChieu, int PhutVaoChieu, int GioRaChieu, int PhutRaChieu,
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
                        obj.ThoiGianBatDauNghiGiuaCa = ParseTimeString(GioBatDauNghi, PhutBatDauNghi);
                        obj.ThoiGianKetThucNghiGiuaCa = ParseTimeString(GioKetThucNghi, PhutKetThucNghi);
                        obj.ThoiGianVaoChieu = ParseTimeString(GioVaoChieu, PhutVaoChieu);
                        obj.ThoiGianRaChieu = ParseTimeString(GioRaChieu, PhutRaChieu);
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
                    obj.ThoiGianBatDauNghiGiuaCa = ParseTimeString(GioBatDauNghi, PhutBatDauNghi);
                    obj.ThoiGianKetThucNghiGiuaCa = ParseTimeString(GioKetThucNghi, PhutKetThucNghi);
                    obj.ThoiGianVaoChieu = ParseTimeString(GioVaoChieu, PhutVaoChieu);
                    obj.ThoiGianRaChieu = ParseTimeString(GioRaChieu, PhutRaChieu);
                    obj.TongSoGioLamViecBuoiSang = ParseTime(GioRaSang, PhutRaSang) - ParseTime(GioVaoSang, PhutVaoSang);
                    obj.TongSoGioLamViecBuoiChieu = ParseTime(GioRaChieu, PhutRaChieu) - ParseTime(GioVaoChieu, PhutVaoChieu);
                    obj.TongSoGioLamViecCaNgay = obj.TongSoGioLamViecBuoiSang + obj.TongSoGioLamViecBuoiChieu;
                    obj.Active = true;
                    obj.LoaiCa = 1;
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
    }
}
