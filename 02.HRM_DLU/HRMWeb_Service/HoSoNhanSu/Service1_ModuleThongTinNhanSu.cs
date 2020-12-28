using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
        //bang luong
        #region bang luong
        public DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN ModuleThongTinNhanSu_BANGLUONGNHANVIEN(String publicKey, String token,
    Guid idNhanVien, Guid kyTinhLuong)
        {

            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN obj = new DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN();
                try
                {
                    obj.ChiTietThuNhapCaNhan =
                        factory.Context.spd_Web1_ChiTietThuNhapCaNhan(idNhanVien, kyTinhLuong)
                            .Map<DTO_ModuleThongTinNhanSu_ChiTietThuNhapCaNhan>()
                            .SingleOrDefault();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_ChiTietKhauTruLuong = factory.Context.spd_Web1_ChiTietKhauTruLuong(idNhanVien, kyTinhLuong).Map<DTO_ModuleThongTinNhanSu_ChiTietKhauTruLuong>();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_ChiTietKhenThuongPhucLoi = factory.Context.spd_Web1_ChiTietKhenThuongPhucLoi(idNhanVien, kyTinhLuong).Map<DTO_ModuleThongTinNhanSu_ChiTietKhenThuongPhucLoi>();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_ChiTietNgoaiGio = factory.Context.spd_Web1_ChiTietNgoaiGio(idNhanVien, kyTinhLuong).Map<DTO_ModuleThongTinNhanSu_ChiTietNgoaiGio>();
                }
                catch (Exception) { } try
                {
                    obj.DanhSach_ChiTietPhuCap = factory.Context.spd_Web1_ChiTietPhuCap(idNhanVien, kyTinhLuong).Map<DTO_ModuleThongTinNhanSu_ChiTietPhuCap>();
                }
                catch (Exception) { } try
                {
                    obj.DanhSach_ChiTietThuLaoGiangDay = factory.Context.spd_Web1_ChiTietThuLaoGiangDay(idNhanVien, kyTinhLuong).Map<DTO_ModuleThongTinNhanSu_ChiTietThuLaoGiangDay>();
                }
                catch (Exception) { } try
                {
                    obj.DanhSach_ChiTietThuNhapKhac = factory.Context.spd_Web1_ChiTietThuNhapKhac(idNhanVien, kyTinhLuong).Map<DTO_ModuleThongTinNhanSu_ChiTietThuNhapKhac>();
                }
                catch (Exception) { } try
                {
                    obj.ChiTietThueTNCN =
                        factory.Context.spd_Web1_ChiTietThueTNCN(idNhanVien, kyTinhLuong)
                            .Map<DTO_ModuleThongTinNhanSu_ChiTietThueTNCN>()
                            .SingleOrDefault();

                }
                catch (Exception) { }

                return obj;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_BangLuongDLU> BANGLUONG_DLU(String publicKey, String token,
   Guid idNhanVien, int thang, int nam)
        {

            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();

                IEnumerable<DTO_BangLuongDLU> obj = factory.Context.spd_Web1_ChiTietThuNhapCaNhanOld_DLU(thang,nam,idNhanVien).Map<DTO_BangLuongDLU>();
                double tongtien = obj.Sum(s => Convert.ToDouble(s.SoTien));
                string tongtienbangchu = So_chu(tongtien);
                foreach (DTO_BangLuongDLU dbl in obj)
                {
                    dbl.TongTien = tongtien;
                    dbl.TongTienBangChu = tongtienbangchu;
                }
                return obj.OrderBy(x=>x.NgayChi);

            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_BANGLUONGNHANVIEN_Json(String publicKey, String token, Guid idNhanVien, Guid kyTinhLuong)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN obj = ModuleThongTinNhanSu_BANGLUONGNHANVIEN(publicKey, token, idNhanVien, kyTinhLuong);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public String BANGLUONG_DLU_Json(String publicKey, String token, Guid idNhanVien, int thang, int nam)
        {//DANG SD
           IEnumerable<DTO_BangLuongDLU> obj = BANGLUONG_DLU(publicKey, token, idNhanVien, thang, nam);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion
        #region bang luong IUH
        public DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN_IUH ModuleThongTinNhanSu_BANGLUONGNHANVIEN_IUH(String publicKey, String token,
    Guid idNhanVien, Guid kyTinhLuong, byte? loaiLuong)
        {

            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN_IUH obj = new DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN_IUH();
                try
                {
                    obj.ChiTietThuNhapCaNhan =
                        factory.Context.spd_Web1_ChiTietThuNhapCaNhan_IUH(idNhanVien, kyTinhLuong, loaiLuong)
                            .Map<DTO_ModuleThongTinNhanSu_ChiTietThuNhapCaNhan_IUH>()
                            .SingleOrDefault();
                }
                catch (Exception) { }
                //
                try
                {
                    obj.DanhSach_ChiTietKhauTruLuong = factory.Context.spd_Web1_ChiTietKhauTruLuong_IUH(idNhanVien, kyTinhLuong, loaiLuong).Map<DTO_ModuleThongTinNhanSu_ChiTietKhauTruLuong_IUH>();
                }
                catch (Exception) { }
                //


                try
                {
                    obj.ChiTietThueTNCN =
                        factory.Context.spd_Web1_ChiTietThueTNCN_IUH(idNhanVien, kyTinhLuong)
                            .Map<DTO_ModuleThongTinNhanSu_ChiTietThueTNCN_IUH>()
                            .SingleOrDefault();

                }
                catch (Exception) { }

                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_BANGLUONGNHANVIEN_IUH_Json(String publicKey, String token, Guid idNhanVien, Guid kyTinhLuong, byte? loaiLuong)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN_IUH obj = ModuleThongTinNhanSu_BANGLUONGNHANVIEN_IUH(publicKey, token, idNhanVien, kyTinhLuong, loaiLuong);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion
        //so yeu ly lich
        #region so yeu ly lich
        public DTO_ModuleThongTinNhanSu_SoYeuLyLich ModuleThongTinNhanSu_SoYeuLyLich(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_SoYeuLyLich obj = WebUser_Factory.New().Context.spd_Web1_SoYeuLyLich(idNhanVien).SingleOrDefault().Map<DTO_ModuleThongTinNhanSu_SoYeuLyLich>();
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_SoYeuLyLich_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_SoYeuLyLich obj = ModuleThongTinNhanSu_SoYeuLyLich(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ModuleThongTinNhanSu_SoYeuLyLich ModuleThongTinNhanSu_SoYeuLyLich_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_SoYeuLyLich obj = WebUser_Factory.New().Context.spd_Web1_SoYeuLyLich_IUH(idNhanVien).SingleOrDefault().Map<DTO_ModuleThongTinNhanSu_SoYeuLyLich>();
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_SoYeuLyLich_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_SoYeuLyLich obj = ModuleThongTinNhanSu_SoYeuLyLich_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ModuleThongTinNhanSu_SoYeuLyLich ModuleThongTinNhanSu_SoYeuLyLich_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_SoYeuLyLich obj = WebUser_Factory.New().Context.spd_Web1_SoYeuLyLich_LUH(idNhanVien).SingleOrDefault().Map<DTO_ModuleThongTinNhanSu_SoYeuLyLich>();
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_SoYeuLyLich_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_SoYeuLyLich obj = ModuleThongTinNhanSu_SoYeuLyLich_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ModuleThongTinNhanSu_SoYeuLyLich ModuleThongTinNhanSu_SoYeuLyLich_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_SoYeuLyLich obj = WebUser_Factory.New().Context.spd_Web1_SoYeuLyLich_DLU(idNhanVien).SingleOrDefault().Map<DTO_ModuleThongTinNhanSu_SoYeuLyLich>();
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_SoYeuLyLich_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_SoYeuLyLich obj = ModuleThongTinNhanSu_SoYeuLyLich_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        #endregion

        //form thong tin luong
        #region form thong tin luong
        public DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN obj =
                    new DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN();
                try
                {
                    obj.DanhSach_NguoiPhuThuoc = factory.Context.spd_Web1_NguoiPhuThuoc(idNhanVien).Map<DTO_ModuleThongTinNhanSu_NguoiPhuThuoc>();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_TaiKhoanNganHang = factory.Context.spd_Web1_TaiKhoanNganHang(idNhanVien).Map<DTO_ModuleThongTinNhanSu_TaiKhoanNganHang>();
                }
                catch (Exception) { }
                try
                {
                    obj.ThongTinLuong = factory.Context.spd_Web1_ThongTinLuong(idNhanVien).Map<DTO_ModuleThongTinNhanSu_ThongTinLuong>().SingleOrDefault();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN obj = ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN obj =
                    new DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN();
                try
                {
                    obj.DanhSach_NguoiPhuThuoc = factory.Context.spd_Web1_NguoiPhuThuoc_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_NguoiPhuThuoc>();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_TaiKhoanNganHang = factory.Context.spd_Web1_TaiKhoanNganHang_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_TaiKhoanNganHang>();
                }
                catch (Exception) { }
                try
                {
                    obj.ThongTinLuong = factory.Context.spd_Web1_ThongTinLuong_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_ThongTinLuong>().SingleOrDefault();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN obj = ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN obj =
                    new DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN();
                try
                {
                    obj.DanhSach_NguoiPhuThuoc = factory.Context.spd_Web1_NguoiPhuThuoc_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_NguoiPhuThuoc>();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_TaiKhoanNganHang = factory.Context.spd_Web1_TaiKhoanNganHang_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_TaiKhoanNganHang>();
                }
                catch (Exception) { }
                try
                {
                    obj.ThongTinLuong = factory.Context.spd_Web1_ThongTinLuong_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_ThongTinLuong>().SingleOrDefault();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN obj = ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN obj =
                    new DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN();
                try
                {
                    obj.DanhSach_NguoiPhuThuoc = factory.Context.spd_Web1_NguoiPhuThuoc_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_NguoiPhuThuoc>();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_TaiKhoanNganHang = factory.Context.spd_Web1_TaiKhoanNganHang_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_TaiKhoanNganHang>();
                }
                catch (Exception) { }
                try
                {
                    obj.ThongTinLuong = factory.Context.spd_Web1_ThongTinLuong_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_ThongTinLuong>().SingleOrDefault();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN obj = ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion

        //form trinh do chuyen mon
        #region form trinh do chuyen mon
        public DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN obj =
                    new DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN();
                try
                {
                    obj.DanhSach_VanBang = factory.Context.spd_Web1_VanBang(idNhanVien).Map<DTO_ModuleThongTinNhanSu_VanBang>();
                }
                catch (Exception) { }

                try
                {
                    obj.DanhSach_ChungChi = factory.Context.spd_Web1_ChungChi(idNhanVien).Map<DTO_ModuleThongTinNhanSu_ChungChi>();
                }
                catch (Exception) { }

                try
                {
                    obj.DanhSach_NgoaiNgu =
                        factory.Context.spd_Web1_NgoaiNgu(idNhanVien).Map<DTO_ModuleThongTinNhanSu_NgoaiNgu>();
                }
                catch (Exception) { }
                try
                {
                    obj.TrinhDoChuyenMon = factory.Context.spd_Web1_TrinhDoChuyenMon(idNhanVien)
                            .Map<DTO_ModuleThongTinNhanSu_TrinhDoChuyenMon>()
                            .SingleOrDefault();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN obj = ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN obj =
                    new DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN();
                try
                {
                    obj.DanhSach_VanBang = factory.Context.spd_Web1_VanBang_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_VanBang>();
                }
                catch (Exception) { }

                try
                {
                    obj.DanhSach_ChungChi = factory.Context.spd_Web1_ChungChi_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_ChungChi>();
                }
                catch (Exception) { }

                try
                {
                    obj.DanhSach_NgoaiNgu =
                        factory.Context.spd_Web1_NgoaiNgu_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_NgoaiNgu>();
                }
                catch (Exception) { }
                try
                {
                    obj.TrinhDoChuyenMon = factory.Context.spd_Web1_TrinhDoChuyenMon_IUH(idNhanVien)
                            .Map<DTO_ModuleThongTinNhanSu_TrinhDoChuyenMon>()
                            .SingleOrDefault();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }
        
        public String ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN obj = ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN obj =
                    new DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN();
                try
                {
                    obj.DanhSach_VanBang = factory.Context.spd_Web1_VanBang_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_VanBang>();
                }
                catch (Exception) { }

                try
                {
                    obj.DanhSach_ChungChi = factory.Context.spd_Web1_ChungChi_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_ChungChi>();
                }
                catch (Exception) { }

                try
                {
                    obj.DanhSach_NgoaiNgu =
                        factory.Context.spd_Web1_NgoaiNgu_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_NgoaiNgu>();
                }
                catch (Exception) { }
                try
                {
                    obj.TrinhDoChuyenMon = factory.Context.spd_Web1_TrinhDoChuyenMon_LUH(idNhanVien)
                            .Map<DTO_ModuleThongTinNhanSu_TrinhDoChuyenMon>()
                            .SingleOrDefault();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN obj = ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN obj =
                    new DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN();
                try
                {
                    obj.DanhSach_VanBang = factory.Context.spd_Web1_VanBang_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_VanBang>();
                }
                catch (Exception) { }

                try
                {
                    obj.DanhSach_ChungChi = factory.Context.spd_Web1_ChungChi_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_ChungChi>();
                }
                catch (Exception) { }

                try
                {
                    obj.DanhSach_NgoaiNgu =
                        factory.Context.spd_Web1_NgoaiNgu_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_NgoaiNgu>();
                }
                catch (Exception) { }
                try
                {
                    obj.TrinhDoChuyenMon = factory.Context.spd_Web1_TrinhDoChuyenMon_DLU(idNhanVien)
                            .Map<DTO_ModuleThongTinNhanSu_TrinhDoChuyenMon>()
                            .SingleOrDefault();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN obj = ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion

        //form hop dong lao dong
        #region form hop dong lao dong
        public DTO_ModuleThongTinNhanSu_HOPDONGLAODONG ModuleThongTinNhanSu_HOPDONGLAODONG(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_HOPDONGLAODONG obj = new DTO_ModuleThongTinNhanSu_HOPDONGLAODONG();
                try
                {
                    obj.HopDong =
                        factory.Context.spd_Web1_HopDong(idNhanVien)
                            .Map<DTO_ModuleThongTinNhanSu_HopDong>()
                            .SingleOrDefault();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_HopDongDaKy =
                        factory.Context.spd_Web1_HopDongDaKy(idNhanVien).Map<DTO_ModuleThongTinNhanSu_HopDongDaKy>();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_HOPDONGLAODONG_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_HOPDONGLAODONG obj = ModuleThongTinNhanSu_HOPDONGLAODONG(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ModuleThongTinNhanSu_HOPDONGLAODONG ModuleThongTinNhanSu_HOPDONGLAODONG_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_HOPDONGLAODONG obj = new DTO_ModuleThongTinNhanSu_HOPDONGLAODONG();
                try
                {
                    obj.HopDong =
                        factory.Context.spd_Web1_HopDong_IUH(idNhanVien)
                            .Map<DTO_ModuleThongTinNhanSu_HopDong>()
                            .SingleOrDefault();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_HopDongDaKy =
                        factory.Context.spd_Web1_HopDongDaKy_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_HopDongDaKy>();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_HOPDONGLAODONG_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_HOPDONGLAODONG obj = ModuleThongTinNhanSu_HOPDONGLAODONG_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public DTO_ModuleThongTinNhanSu_HOPDONGLAODONG ModuleThongTinNhanSu_HOPDONGLAODONG_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_HOPDONGLAODONG obj = new DTO_ModuleThongTinNhanSu_HOPDONGLAODONG();
                try
                {
                    obj.HopDong =
                        factory.Context.spd_Web1_HopDong_LUH(idNhanVien)
                            .Map<DTO_ModuleThongTinNhanSu_HopDong>()
                            .SingleOrDefault();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_HopDongDaKy =
                        factory.Context.spd_Web1_HopDongDaKy_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_HopDongDaKy>();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_HOPDONGLAODONG_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_HOPDONGLAODONG obj = ModuleThongTinNhanSu_HOPDONGLAODONG_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ModuleThongTinNhanSu_HOPDONGLAODONG ModuleThongTinNhanSu_HOPDONGLAODONG_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_HOPDONGLAODONG obj = new DTO_ModuleThongTinNhanSu_HOPDONGLAODONG();
                try
                {
                    obj.HopDong =
                        factory.Context.spd_Web1_HopDong_DLU(idNhanVien)
                            .Map<DTO_ModuleThongTinNhanSu_HopDong>()
                            .SingleOrDefault();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_HopDongDaKy =
                        factory.Context.spd_Web1_HopDongDaKy_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_HopDongDaKy>();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_HOPDONGLAODONG_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_HOPDONGLAODONG obj = ModuleThongTinNhanSu_HOPDONGLAODONG_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        #endregion

        //Quan he gia dinh
        #region Quan he gia dinh
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> ModuleThongTinNhanSu_QuanHeGiaDinh(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> objList = WebUser_Factory.New().Context.spd_Web1_QuanHeGiaDinh(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuanHeGiaDinh_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> objList = ModuleThongTinNhanSu_QuanHeGiaDinh(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> ModuleThongTinNhanSu_QuanHeGiaDinh_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> objList = WebUser_Factory.New().Context.spd_Web1_QuanHeGiaDinh_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        
        public String ModuleThongTinNhanSu_QuanHeGiaDinh_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> objList = ModuleThongTinNhanSu_QuanHeGiaDinh_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> ModuleThongTinNhanSu_QuanHeGiaDinh_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> objList = WebUser_Factory.New().Context.spd_Web1_QuanHeGiaDinh_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh>();
                return objList;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_QuanHeGiaDinh_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> objList = ModuleThongTinNhanSu_QuanHeGiaDinh_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> ModuleThongTinNhanSu_QuanHeGiaDinh_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> objList = WebUser_Factory.New().Context.spd_Web1_QuanHeGiaDinh_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh>();
                return objList;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_QuanHeGiaDinh_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> objList = ModuleThongTinNhanSu_QuanHeGiaDinh_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        #endregion

        //Dien bien luong
        #region Dien bien luong
        public IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> ModuleThongTinNhanSu_DienBienLuong(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> objList = WebUser_Factory.New().Context.spd_Web1_DienBienLuong(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DienBienLuong>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_DienBienLuong_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> objList = ModuleThongTinNhanSu_DienBienLuong(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> ModuleThongTinNhanSu_DienBienLuong_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> objList = WebUser_Factory.New().Context.spd_Web1_DienBienLuong_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DienBienLuong>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        
        public String ModuleThongTinNhanSu_DienBienLuong_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> objList = ModuleThongTinNhanSu_DienBienLuong_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> ModuleThongTinNhanSu_DienBienLuong_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> objList = WebUser_Factory.New().Context.spd_Web1_DienBienLuong_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DienBienLuong>();
                return objList;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_DienBienLuong_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> objList = ModuleThongTinNhanSu_DienBienLuong_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> ModuleThongTinNhanSu_DienBienLuong_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> objList = WebUser_Factory.New().Context.spd_Web1_DienBienLuong_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DienBienLuong>();
                return objList;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_DienBienLuong_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> objList = ModuleThongTinNhanSu_DienBienLuong_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //lich su ban than
        #region lich su ban than
        public IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> ModuleThongTinNhanSu_LichSuBanThan(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> objList = WebUser_Factory.New().Context.spd_Web1_LichSuBanThan(idNhanVien).Map<DTO_ModuleThongTinNhanSu_LichSuBanThan>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_LichSuBanThan_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> objList = ModuleThongTinNhanSu_LichSuBanThan(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> ModuleThongTinNhanSu_LichSuBanThan_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> objList = WebUser_Factory.New().Context.spd_Web1_LichSuBanThan_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_LichSuBanThan>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_LichSuBanThan_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> objList = ModuleThongTinNhanSu_LichSuBanThan_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> ModuleThongTinNhanSu_LichSuBanThan_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> objList = WebUser_Factory.New().Context.spd_Web1_LichSuBanThan_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_LichSuBanThan>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_LichSuBanThan_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> objList = ModuleThongTinNhanSu_LichSuBanThan_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> ModuleThongTinNhanSu_LichSuBanThan_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> objList = WebUser_Factory.New().Context.spd_Web1_LichSuBanThan_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_LichSuBanThan>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_LichSuBanThan_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> objList = ModuleThongTinNhanSu_LichSuBanThan_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //qua trinh cong tac
        #region qua trinh cong tac
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> ModuleThongTinNhanSu_QuaTrinhCongTac(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhCongTac(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhCongTac_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> objList = ModuleThongTinNhanSu_QuaTrinhCongTac(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> ModuleThongTinNhanSu_QuaTrinhCongTac_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhCongTac_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhCongTac_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> objList = ModuleThongTinNhanSu_QuaTrinhCongTac_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> ModuleThongTinNhanSu_QuaTrinhCongTac_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhCongTac_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhCongTac_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> objList = ModuleThongTinNhanSu_QuaTrinhCongTac_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> ModuleThongTinNhanSu_QuaTrinhCongTac_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhCongTac_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhCongTac_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> objList = ModuleThongTinNhanSu_QuaTrinhCongTac_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        #endregion

        //QuaTrinhDaoTao
        #region QuaTrinhDaoTao
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> ModuleThongTinNhanSu_QuaTrinhDaoTao(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhDaoTao(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhDaoTao_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> objList = ModuleThongTinNhanSu_QuaTrinhDaoTao(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> ModuleThongTinNhanSu_QuaTrinhDaoTao_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhDaoTao_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhDaoTao_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> objList = ModuleThongTinNhanSu_QuaTrinhDaoTao_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> ModuleThongTinNhanSu_QuaTrinhDaoTao_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhDaoTao_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhDaoTao_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> objList = ModuleThongTinNhanSu_QuaTrinhDaoTao_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> ModuleThongTinNhanSu_QuaTrinhDaoTao_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhDaoTao_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhDaoTao_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> objList = ModuleThongTinNhanSu_QuaTrinhDaoTao_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //QuaTrinhBoiDuong
        #region QuaTrinhBoiDuong
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong> ModuleThongTinNhanSu_QuaTrinhBoiDuong(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhBoiDuong(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhBoiDuong_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong> objList = ModuleThongTinNhanSu_QuaTrinhBoiDuong(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong> ModuleThongTinNhanSu_QuaTrinhBoiDuong_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhBoiDuong_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhBoiDuong_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong> objList = ModuleThongTinNhanSu_QuaTrinhBoiDuong_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong> ModuleThongTinNhanSu_QuaTrinhBoiDuong_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhBoiDuong_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhBoiDuong_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong> objList = ModuleThongTinNhanSu_QuaTrinhBoiDuong_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong> ModuleThongTinNhanSu_QuaTrinhBoiDuong_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhBoiDuong_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhBoiDuong_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong> objList = ModuleThongTinNhanSu_QuaTrinhBoiDuong_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //QuaTrinhBoNhiem
        #region QuaTrinhBoNhiem
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> ModuleThongTinNhanSu_QuaTrinhBoNhiem(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhBoNhiem(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhBoNhiem_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> objList = ModuleThongTinNhanSu_QuaTrinhBoNhiem(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> ModuleThongTinNhanSu_QuaTrinhBoNhiem_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhBoNhiem_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhBoNhiem_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> objList = ModuleThongTinNhanSu_QuaTrinhBoNhiem_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> ModuleThongTinNhanSu_QuaTrinhBoNhiem_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhBoNhiem_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhBoNhiem_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> objList = ModuleThongTinNhanSu_QuaTrinhBoNhiem_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> ModuleThongTinNhanSu_QuaTrinhBoNhiem_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhBoNhiem_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhBoNhiem_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> objList = ModuleThongTinNhanSu_QuaTrinhBoNhiem_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //QuaTrinhDiNuocNgoai
        #region QuaTrinhDiNuocNgoai
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai> ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhDiNuocNgoai(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai> objList = ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai> ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhDiNuocNgoai_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai> objList = ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai> ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhDiNuocNgoai_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai> objList = ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai> ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhDiNuocNgoai_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai> objList = ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //QuaTrinhKhenThuong
        #region QuaTrinhKhenThuong
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> ModuleThongTinNhanSu_QuaTrinhKhenThuong(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhKhenThuong(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhKhenThuong_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> objList = ModuleThongTinNhanSu_QuaTrinhKhenThuong(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> ModuleThongTinNhanSu_QuaTrinhKhenThuong_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhKhenThuong_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhKhenThuong_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> objList = ModuleThongTinNhanSu_QuaTrinhKhenThuong_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> ModuleThongTinNhanSu_QuaTrinhKhenThuong_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhKhenThuong_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhKhenThuong_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> objList = ModuleThongTinNhanSu_QuaTrinhKhenThuong_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> ModuleThongTinNhanSu_QuaTrinhKhenThuong_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhKhenThuong_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhKhenThuong_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> objList = ModuleThongTinNhanSu_QuaTrinhKhenThuong_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //QuaTrinhKyLuat
        #region QuaTrinhKyLuat
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> ModuleThongTinNhanSu_QuaTrinhKyLuat(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhKyLuat(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhKyLuat_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> objList = ModuleThongTinNhanSu_QuaTrinhKyLuat(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> ModuleThongTinNhanSu_QuaTrinhKyLuat_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhKyLuat_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhKyLuat_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> objList = ModuleThongTinNhanSu_QuaTrinhKyLuat_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> ModuleThongTinNhanSu_QuaTrinhKyLuat_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhKyLuat_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhKyLuat_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> objList = ModuleThongTinNhanSu_QuaTrinhKyLuat_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> ModuleThongTinNhanSu_QuaTrinhKyLuat_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhKyLuat_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhKyLuat_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> objList = ModuleThongTinNhanSu_QuaTrinhKyLuat_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //QuaTrinhNghienCuuKhoaHoc
        #region QuaTrinhNghienCuuKhoaHoc
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhNghienCuuKhoaHoc(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> objList = ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhNghienCuuKhoaHoc_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> objList = ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhNghienCuuKhoaHoc_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> objList = ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhNghienCuuKhoaHoc_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> objList = ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //QuaTrinhThamGiaHoatDongXaHoi
        #region QuaTrinhThamGiaHoatDongXaHoi
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhThamGiaHoatDongXaHoi(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> objList = ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhThamGiaHoatDongXaHoi_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> objList = ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhThamGiaHoatDongXaHoi_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> objList = ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhThamGiaHoatDongXaHoi_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> objList = ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //QuaTrinhThamGiaHoiThao
        #region QuaTrinhThamGiaHoiThao
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhThamGiaHoiThao(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> objList = ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhThamGiaHoiThao_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> objList = ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhThamGiaHoiThao_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> objList = ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhThamGiaHoiThao_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> objList = ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //QuaTrinhThamGiaLucLuongVuTrang
        #region QuaTrinhThamGiaLucLuongVuTrang
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhThamGiaLucLuongVuTrang(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> objList = ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhThamGiaLucLuongVuTrang_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> objList = ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhThamGiaLucLuongVuTrang_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> objList = ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhThamGiaLucLuongVuTrang_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> objList = ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //BAOHIEMXAHOI
        #region BAOHIEMXAHOI
        public DTO_ModuleThongTinNhanSu_BAOHIEMXAHOI ModuleThongTinNhanSu_BAOHIEMXAHOI(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_BAOHIEMXAHOI obj = new DTO_ModuleThongTinNhanSu_BAOHIEMXAHOI();
                try
                {
                    obj.HoSoBaoHiem =
                        factory.Context.spd_Web1_HoSoBaoHiem(idNhanVien)
                            .Map<DTO_ModuleThongTinNhanSu_HoSoBaoHiem>()
                            .SingleOrDefault();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_QuaTrinhThamGiaBHXH =
                        factory.Context.spd_Web1_QuaTrinhThamGiaBHXH(idNhanVien)
                            .Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaBHXH>();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_BAOHIEMXAHOI_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_BAOHIEMXAHOI obj = ModuleThongTinNhanSu_BAOHIEMXAHOI(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion

        //DangVien
        #region DangVien
        public DTO_ModuleThongTinNhanSu_DangVien ModuleThongTinNhanSu_DangVien(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_DangVien obj = WebUser_Factory.New().Context.spd_Web1_DangVien(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DangVien>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_DangVien_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_DangVien obj = ModuleThongTinNhanSu_DangVien(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ModuleThongTinNhanSu_DangVien ModuleThongTinNhanSu_DangVien_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_DangVien obj = WebUser_Factory.New().Context.spd_Web1_DangVien_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DangVien>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }
        
        public String ModuleThongTinNhanSu_DangVien_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_DangVien obj = ModuleThongTinNhanSu_DangVien_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public DTO_ModuleThongTinNhanSu_DangVien ModuleThongTinNhanSu_DangVien_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_DangVien obj = WebUser_Factory.New().Context.spd_Web1_DangVien_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DangVien>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_DangVien_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_DangVien obj = ModuleThongTinNhanSu_DangVien_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ModuleThongTinNhanSu_DangVien ModuleThongTinNhanSu_DangVien_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_DangVien obj = WebUser_Factory.New().Context.spd_Web1_DangVien_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DangVien>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_DangVien_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_DangVien obj = ModuleThongTinNhanSu_DangVien_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion

        //DoanVien
        #region DoanVien
        public DTO_ModuleThongTinNhanSu_DoanVien ModuleThongTinNhanSu_DoanVien(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_DoanVien obj = WebUser_Factory.New().Context.spd_Web1_DoanVien(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DoanVien>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_DoanVien_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_DoanVien obj = ModuleThongTinNhanSu_DoanVien(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ModuleThongTinNhanSu_DoanVien ModuleThongTinNhanSu_DoanVien_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_DoanVien obj = WebUser_Factory.New().Context.spd_Web1_DoanVien_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DoanVien>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }
        
        public String ModuleThongTinNhanSu_DoanVien_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_DoanVien obj = ModuleThongTinNhanSu_DoanVien_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public DTO_ModuleThongTinNhanSu_DoanVien ModuleThongTinNhanSu_DoanVien_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_DoanVien obj = WebUser_Factory.New().Context.spd_Web1_DoanVien_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DoanVien>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_DoanVien_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_DoanVien obj = ModuleThongTinNhanSu_DoanVien_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public DTO_ModuleThongTinNhanSu_DoanVien ModuleThongTinNhanSu_DoanVien_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_DoanVien obj = WebUser_Factory.New().Context.spd_Web1_DoanVien_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DoanVien>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_DoanVien_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_DoanVien obj = ModuleThongTinNhanSu_DoanVien_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion

        //CongDoan
        #region CongDoan
        public DTO_ModuleThongTinNhanSu_CongDoan ModuleThongTinNhanSu_CongDoan(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_CongDoan obj = WebUser_Factory.New().Context.spd_Web1_CongDoan(idNhanVien).Map<DTO_ModuleThongTinNhanSu_CongDoan>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_CongDoan_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_CongDoan obj = ModuleThongTinNhanSu_CongDoan(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ModuleThongTinNhanSu_CongDoan ModuleThongTinNhanSu_CongDoan_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_CongDoan obj = WebUser_Factory.New().Context.spd_Web1_CongDoan_IUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_CongDoan>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }
        
        public String ModuleThongTinNhanSu_CongDoan_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_CongDoan obj = ModuleThongTinNhanSu_CongDoan_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ModuleThongTinNhanSu_CongDoan ModuleThongTinNhanSu_CongDoan_LUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_CongDoan obj = WebUser_Factory.New().Context.spd_Web1_CongDoan_LUH(idNhanVien).Map<DTO_ModuleThongTinNhanSu_CongDoan>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_CongDoan_LUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_CongDoan obj = ModuleThongTinNhanSu_CongDoan_LUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public DTO_ModuleThongTinNhanSu_CongDoan ModuleThongTinNhanSu_CongDoan_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_CongDoan obj = WebUser_Factory.New().Context.spd_Web1_CongDoan_DLU(idNhanVien).Map<DTO_ModuleThongTinNhanSu_CongDoan>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_CongDoan_DLU_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_CongDoan obj = ModuleThongTinNhanSu_CongDoan_DLU(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion





        //Giay xac nhan vay von ngan hang
        #region Giay xac nhan vay von ngan hang
        public DTO_ModuleThongTinNhanSu_GiayXacNhan ModuleThongTinNhanSu_GiayXacNhanVayVonNganHang(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_GiayXacNhan obj = WebUser_Factory.New().Context.spd_Web1_GiayChungNhan(idNhanVien).SingleOrDefault().Map<DTO_ModuleThongTinNhanSu_GiayXacNhan>();
                Web_GiayChungNhan_AutoNumber_Factory factory = Web_GiayChungNhan_AutoNumber_Factory.New();
                Web_GiayChungNhan_AutoNumber autoNumberObj = factory.CreateManagedObject();
                autoNumberObj.Id = Guid.NewGuid();
                factory.SaveChanges();
                factory.RefreshAll(RefreshMode.StoreWins);
                obj.SoChungTu = autoNumberObj.SoThuTuPhieu.ToString() + "/GCN-TCHC";
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_GiayXacNhanVayVonNganHang_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_GiayXacNhan obj = ModuleThongTinNhanSu_GiayXacNhanVayVonNganHang(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ModuleThongTinNhanSu_GiayXacNhan ModuleThongTinNhanSu_GiayXacNhanVayVonNganHang_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_GiayXacNhan obj = WebUser_Factory.New().Context.spd_Web1_GiayChungNhan_IUH(idNhanVien).SingleOrDefault().Map<DTO_ModuleThongTinNhanSu_GiayXacNhan>();
                Web_GiayChungNhan_AutoNumber_Factory factory = Web_GiayChungNhan_AutoNumber_Factory.New();
                Web_GiayChungNhan_AutoNumber autoNumberObj = factory.CreateManagedObject();
                autoNumberObj.Id = Guid.NewGuid();
                factory.SaveChanges();
                factory.RefreshAll(RefreshMode.StoreWins);
                obj.SoChungTu = autoNumberObj.SoThuTuPhieu.ToString() + "/GCN-TCHC";
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_GiayXacNhanVayVonNganHang_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_GiayXacNhan obj = ModuleThongTinNhanSu_GiayXacNhanVayVonNganHang_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion

        //Giay xac nhan vay can bo vien chuc
        #region Giay xac nhan vay can bo vien chuc
        public DTO_ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong obj = WebUser_Factory.New().Context.spd_Web1_GiayChungNhanCanBoVienChucTruong(idNhanVien).SingleOrDefault().Map<DTO_ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong>();
                Web_GiayChungNhan_AutoNumber_Factory factory = Web_GiayChungNhan_AutoNumber_Factory.New();
                Web_GiayChungNhan_AutoNumber autoNumberObj = factory.CreateManagedObject();
                autoNumberObj.Id = Guid.NewGuid();
                factory.SaveChanges();
                factory.RefreshAll(RefreshMode.StoreWins);
                obj.SoChungTu = autoNumberObj.SoThuTuPhieu.ToString() + "/GCN-TCHC";
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong_IUH_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong obj = ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong_IUH(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong_IUH(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong obj = WebUser_Factory.New().Context.spd_Web1_GiayChungNhanCanBoVienChucTruong_IUH(idNhanVien).SingleOrDefault().Map<DTO_ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong>();
                Web_GiayChungNhan_AutoNumber_Factory factory = Web_GiayChungNhan_AutoNumber_Factory.New();
                Web_GiayChungNhan_AutoNumber autoNumberObj = factory.CreateManagedObject();
                autoNumberObj.Id = Guid.NewGuid();
                factory.SaveChanges();
                factory.RefreshAll(RefreshMode.StoreWins);
                obj.SoChungTu = autoNumberObj.SoThuTuPhieu.ToString() + "/GCN-TCHC";
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong obj = ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        private static string Donvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }
        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }
        private static string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";

            }


            return Ktach;

        }
        public static string So_chu(double gNum)
        {
            if (gNum == 0)
                return "Không đồng";

            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            double Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng chẵn.";

            return lso_chu.ToString().Trim();

        }
        #endregion
    }
}
