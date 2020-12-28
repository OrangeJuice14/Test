﻿using System;
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
        public IEnumerable<DTO_KyTinhLuong> KyTinhLuong_GetAll(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                KyTinhLuong_Factory factory = KyTinhLuong_Factory.New();
                IEnumerable<DTO_KyTinhLuong> list = factory.GetAll_GCRecordIsNull().Map<DTO_KyTinhLuong>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String KyTinhLuong_GetAll_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_KyTinhLuong> list = KyTinhLuong_GetAll(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public String GetSoChungTuByKyTinhLuong(String publicKey, String token, Guid kyTinhLuong)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                KyTinhLuong_Factory factory = KyTinhLuong_Factory.New();
                var list = factory.Context.spd_Service_GetSoChungTuHienWeb(kyTinhLuong);
                return JsonConvert.SerializeObject(list);
            }
            else return null;
        }
        public String TongHopTienGiangVaThuNhapKhac(String publicKey, String token, Guid nhanVien, Guid kyTinhLuong, Guid? soChungTu, Guid? kyTinhPMS)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var list = Utils.DataClassHelper.spd_Service_TongHopTienGiangVaThuNhapKhac(kyTinhLuong, soChungTu, nhanVien, kyTinhPMS);
                return JsonConvert.SerializeObject(list);
            }
            else return null;
        }
        public String TongHopThueTNCNTrongThang(String publicKey, String token, Guid nhanVien, Guid kyTinhLuong, Guid soChungTu)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                KyTinhLuong_Factory factory = KyTinhLuong_Factory.New();
                var list = factory.Context.spd_Service_TongHopThueTNCNTrongThang(nhanVien, kyTinhLuong, soChungTu).FirstOrDefault();
                return JsonConvert.SerializeObject(list);
            }
            else return null;
        }
        public String ChiTietThanhToanPMS(String publicKey, String token, Guid nhanVien, Guid kyTinhLuong, Guid? soChungTu, Guid? kyTinhPMS)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var list = Utils.DataClassHelper.spd_BangThuLaoNhanVien_ChiTietThanhToanPMS(nhanVien, kyTinhLuong, soChungTu, kyTinhPMS);
                return JsonConvert.SerializeObject(list);
            }
            else return null;
        }
        public String ChiTietTheoDoiTruTietChuan(String publicKey, String token, Guid nhanVien, Guid kyTinhLuong, Guid soChungTu)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var list = Utils.DataClassHelper.spd_PMS_Web_ChiTietTheoDoiTruTietChuan(nhanVien, kyTinhLuong, soChungTu);
                return JsonConvert.SerializeObject(list);
            }
            else return null;
        }
        public String WebUser_KiemTraLoaiNhanVien(String publicKey, String token, string MaQuanLy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var list = Utils.DataClassHelper.spd_WebUser_KiemTraLoaiNhanVien(MaQuanLy);
                return JsonConvert.SerializeObject(list);
            }
            else return null;
        }
        public String ThuLaoGiangDay_ThinhGiang(String publicKey, String token, Guid nhanVien, Guid kyTinhLuong, Guid kyTinhPMS)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var list = Utils.DataClassHelper.spd_PMS_ThuLaoGiangDay_ThinhGiang(nhanVien, kyTinhLuong, kyTinhPMS);
                return JsonConvert.SerializeObject(list);
            }
            else return null;
        }
        public String GetNamHocPMS(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var list = Utils.DataClassHelper.GetNamHocPMS();
                return JsonConvert.SerializeObject(list);
            }
            else return null;
        }
        public String GetKyTinhPMSTheoNamHoc(String publicKey, String token, Guid NamHocOid)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var list = Utils.DataClassHelper.GetKyTinhPMSTheoNamHoc(NamHocOid);
                return JsonConvert.SerializeObject(list);
            }
            else return null;
        }

        // /////////////////////////////////////////////////////
        public DTO_KyTinhLuong KyTinhLuong_ByMonthAndYear(String publicKey, String token,int month,int year)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                KyTinhLuong_Factory factory = KyTinhLuong_Factory.New();
                DTO_KyTinhLuong obj = factory.GetKyTinhLuong_GCRecordIsNull_ByMonthAndYear(month,year).Map<DTO_KyTinhLuong>();
                return obj;
            }
            else
            {
                return null;
            }
        }


        public String KyTinhLuong_ByMonthAndYear_Json(String publicKey, String token, int month, int year)
        {//DANG SD
            DTO_KyTinhLuong obj = KyTinhLuong_ByMonthAndYear(publicKey, token,month,year);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public bool KiemTraKhoaSo_KyTinhLuong(String publicKey, String token, int month, int year)
        {
            KyTinhLuong_Factory factory = KyTinhLuong_Factory.New();
            var obj = factory.GetKyTinhLuong_GCRecordIsNull_ByMonthAndYear(month, year);
            if (obj != null)
                return obj.KhoaSo ?? false;
            else return false;
        }

        public bool KyTinhLuong_Check(String publicKey, String token, int thang, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = KyTinhLuong_Factory.New();
                bool daTonTai = factory.ExistsByThangNam(thang, nam);
                return daTonTai;

            }
            else
            {
                throw new Exception("Xác thực không hợp lệ.");
            }
        }
    }
}