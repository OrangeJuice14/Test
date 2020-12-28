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
                        factory.Context.spd_Report_Luong_ChiTietTongThuNhapTungNhanVien(kyTinhLuong, idNhanVien)
                            .Map<DTO_ModuleThongTinNhanSu_ChiTietThuNhapCaNhan>()
                            .SingleOrDefault();
                }
                catch (Exception ex)
                {
                }
                return obj;
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
        #endregion

        //so yeu ly lich
        #region so yeu ly lich
        public DTO_ModuleThongTinNhanSu_SoYeuLyLich ModuleThongTinNhanSu_SoYeuLyLich_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_SoYeuLyLich obj = WebUser_Factory.New().Context.spd_Web1_SoYeuLyLich_UTE(idNhanVien).SingleOrDefault().Map<DTO_ModuleThongTinNhanSu_SoYeuLyLich>();
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_SoYeuLyLich_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_SoYeuLyLich obj = ModuleThongTinNhanSu_SoYeuLyLich_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        #endregion

        //form thong tin luong
        #region form thong tin luong

        public DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN obj =
                    new DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN();
                try
                {
                    obj.DanhSach_NguoiPhuThuoc = factory.Context.spd_Web1_NguoiPhuThuoc_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_NguoiPhuThuoc>();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_TaiKhoanNganHang = factory.Context.spd_Web1_TaiKhoanNganHang_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_TaiKhoanNganHang>();
                }
                catch (Exception) { }
                try
                {
                    obj.ThongTinLuong = factory.Context.spd_Web1_ThongTinLuong_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_ThongTinLuong>().SingleOrDefault();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN obj = ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion

        //form trinh do chuyen mon
        #region form trinh do chuyen mon

            
        public DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN obj =
                    new DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN();
                try
                {
                    obj.DanhSach_VanBang = factory.Context.spd_Web1_VanBang_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_VanBang>();
                }
                catch (Exception) { }

                try
                {
                    obj.DanhSach_ChungChi = factory.Context.spd_Web1_ChungChi_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_ChungChi>();
                }
                catch (Exception) { }

                try
                {
                    obj.DanhSach_NgoaiNgu =
                        factory.Context.spd_Web1_NgoaiNgu_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_NgoaiNgu>();
                }
                catch (Exception) { }
                try
                {
                    obj.TrinhDoChuyenMon = factory.Context.spd_Web1_TrinhDoChuyenMon_UTE(idNhanVien)
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

        public String ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN obj = ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion

        //form hop dong lao dong
        #region form hop dong lao dong
        public DTO_ModuleThongTinNhanSu_HOPDONGLAODONG ModuleThongTinNhanSu_HOPDONGLAODONG_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_HOPDONGLAODONG obj = new DTO_ModuleThongTinNhanSu_HOPDONGLAODONG();
                try
                {
                    obj.HopDong =
                        factory.Context.spd_Web1_HopDong_UTE(idNhanVien)
                            .Map<DTO_ModuleThongTinNhanSu_HopDong>()
                            .SingleOrDefault();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_HopDongDaKy =
                        factory.Context.spd_Web1_HopDongDaKy_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_HopDongDaKy>();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_HOPDONGLAODONG_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_HOPDONGLAODONG obj = ModuleThongTinNhanSu_HOPDONGLAODONG_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        #endregion

        //Quan he gia dinh
        #region Quan he gia dinh
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> ModuleThongTinNhanSu_QuanHeGiaDinh_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> objList = WebUser_Factory.New().Context.spd_Web1_QuanHeGiaDinh_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh>();
                return objList;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_QuanHeGiaDinh_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> objList = ModuleThongTinNhanSu_QuanHeGiaDinh_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        #endregion

        //Dien bien luong
        #region Dien bien luong
        public IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> ModuleThongTinNhanSu_DienBienLuong_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> objList = WebUser_Factory.New().Context.spd_Web1_DienBienLuong_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DienBienLuong>();
                return objList;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_DienBienLuong_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> objList = ModuleThongTinNhanSu_DienBienLuong_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //lich su ban than
        #region lich su ban than
        public IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> ModuleThongTinNhanSu_LichSuBanThan_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> objList = WebUser_Factory.New().Context.spd_Web1_LichSuBanThan_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_LichSuBanThan>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_LichSuBanThan_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> objList = ModuleThongTinNhanSu_LichSuBanThan_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //qua trinh cong tac
        #region qua trinh cong tac
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> ModuleThongTinNhanSu_QuaTrinhCongTac_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhCongTac_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhCongTac_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> objList = ModuleThongTinNhanSu_QuaTrinhCongTac_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        #endregion

        //QuaTrinhHuanLuyenDaoTao
        #region QuaTrinhHuanLuyenDaoTao
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhHuanLuyenDaoTao> ModuleThongTinNhanSu_QuaTrinhHuanLuyenDaoTao_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhHuanLuyenDaoTao> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhHuanLuyenDaoTao_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhHuanLuyenDaoTao>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhHuanLuyenDaoTao_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhHuanLuyenDaoTao> objList = ModuleThongTinNhanSu_QuaTrinhHuanLuyenDaoTao_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //QuaTrinhBoNhiem
        #region QuaTrinhBoNhiem

        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> ModuleThongTinNhanSu_QuaTrinhBoNhiem_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhBoNhiem_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhBoNhiem_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> objList = ModuleThongTinNhanSu_QuaTrinhBoNhiem_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion
        
        //QuaTrinhHoiThao
        #region QuaTrinhHoiThao
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhHoiThao> ModuleThongTinNhanSu_QuaTrinhHoiThao_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhHoiThao> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhHoiThao_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhHoiThao>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhHoiThao_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhHoiThao> objList = ModuleThongTinNhanSu_QuaTrinhHoiThao_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //QuaTrinhSangKien
        #region QuaTrinhSangKien
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhSangKien> ModuleThongTinNhanSu_QuaTrinhSangKien_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhSangKien> objList = WebUser_Factory.New().Context.spd_Web1_QuaTrinhSangKien_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhSangKien>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhSangKien_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhSangKien> objList = ModuleThongTinNhanSu_QuaTrinhSangKien_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }
        #endregion

        //BAOHIEMXAHOI
        #region BAOHIEMXAHOI

        #endregion

        //DangVien
        #region DangVien
        public DTO_ModuleThongTinNhanSu_DangVien ModuleThongTinNhanSu_DangVien_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_DangVien obj = WebUser_Factory.New().Context.spd_Web1_DangVien_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DangVien>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_DangVien_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_DangVien obj = ModuleThongTinNhanSu_DangVien_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion

        //DoanVien
        #region DoanVien

        public DTO_ModuleThongTinNhanSu_DoanVien ModuleThongTinNhanSu_DoanVien_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_DoanVien obj = WebUser_Factory.New().Context.spd_Web1_DoanVien_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DoanVien>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_DoanVien_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_DoanVien obj = ModuleThongTinNhanSu_DoanVien_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion

        //CongDoan
        #region CongDoan
           public DTO_ModuleThongTinNhanSu_CongDoan ModuleThongTinNhanSu_CongDoan_UTE(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_CongDoan obj = WebUser_Factory.New().Context.spd_Web1_CongDoan_UTE(idNhanVien).Map<DTO_ModuleThongTinNhanSu_CongDoan>().SingleOrDefault();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String ModuleThongTinNhanSu_CongDoan_UTE_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_CongDoan obj = ModuleThongTinNhanSu_CongDoan_UTE(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion
       
    }
}
