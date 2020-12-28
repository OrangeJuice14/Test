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
        public DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN ModuleThongTinNhanSu_BANGLUONGNHANVIEN(String publicKey, String token,Guid idNhanVien, Guid kyTinhLuong,Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN obj = new DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN();
                try
                {
                    obj.ChiTietThuNhapCaNhan = factory.Context.spd_Web_TongThuNhapNhanVien(kyTinhLuong, idNhanVien,congTy).Map<DTO_ModuleThongTinNhanSu_ChiTietThuNhapCaNhan>().SingleOrDefault();
                }
                catch (Exception ex )
                {

                }
                return obj;
            }
            else
            {
                return null;
            }
        }
      public String ModuleThongTinNhanSu_BANGLUONGNHANVIEN_Json(String publicKey, String token, Guid idNhanVien, Guid kyTinhLuong, Guid congTy)
        {//DANG SD
            DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN obj = ModuleThongTinNhanSu_BANGLUONGNHANVIEN(publicKey, token, idNhanVien, kyTinhLuong,congTy);
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
                DTO_ModuleThongTinNhanSu_SoYeuLyLich obj = null;
                try
                {
                    obj = WebUser_Factory.New().Context.spd_Web_SoYeuLyLich(idNhanVien).SingleOrDefault().Map<DTO_ModuleThongTinNhanSu_SoYeuLyLich>();
                    
                }
                catch (Exception ex)
                {

                }
                //
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
                    obj.DanhSach_NguoiPhuThuoc = factory.Context.spd_Web_NguoiPhuThuoc(idNhanVien).Map<DTO_ModuleThongTinNhanSu_NguoiPhuThuoc>();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_TaiKhoanNganHang = factory.Context.spd_Web_TaiKhoanNganHang(idNhanVien).Map<DTO_ModuleThongTinNhanSu_TaiKhoanNganHang>();
                }
                catch (Exception) { }
                try
                {
                    obj.ThongTinLuong = factory.Context.spd_Web_ThongTinLuong(idNhanVien).Map<DTO_ModuleThongTinNhanSu_ThongTinLuong>().SingleOrDefault();
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
           
        public DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN obj =
                    new DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN();
                try
                {
                    obj.DanhSach_NguoiPhuThuoc = factory.Context.spd_Web_NguoiPhuThuoc(idNhanVien).Map<DTO_ModuleThongTinNhanSu_NguoiPhuThuoc>();
                }
                catch (Exception) { }
                try
                {
                    obj.DanhSach_TaiKhoanNganHang = factory.Context.spd_Web_TaiKhoanNganHang(idNhanVien).Map<DTO_ModuleThongTinNhanSu_TaiKhoanNganHang>();
                }
                catch (Exception) { }
                try
                {
                    obj.ThongTinLuong = factory.Context.spd_Web_ThongTinLuong(idNhanVien).Map<DTO_ModuleThongTinNhanSu_ThongTinLuong>().SingleOrDefault();
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
                    obj.DanhSach_VanBang = factory.Context.spd_Web_VanBang(idNhanVien).Map<DTO_ModuleThongTinNhanSu_VanBang>();
                }
                catch (Exception) { }

                try
                {
                    obj.DanhSach_ChungChi = factory.Context.spd_Web_ChungChi(idNhanVien).Map<DTO_ModuleThongTinNhanSu_ChungChi>();
                }
                catch (Exception) { }

                try
                {
                    obj.DanhSach_NgoaiNgu =
                        factory.Context.spd_Web_NgoaiNgu(idNhanVien).Map<DTO_ModuleThongTinNhanSu_NgoaiNgu>();
                }
                catch (Exception) { }
                try
                {
                    obj.TrinhDoChuyenMon = factory.Context.spd_Web_TrinhDoChuyenMon(idNhanVien)
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
                        factory.Context.spd_Web_HopDong(idNhanVien)
                            .Map<DTO_ModuleThongTinNhanSu_HopDong>()
                            .SingleOrDefault();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    obj.DanhSach_HopDongDaKy =
                        factory.Context.spd_Web_HopDongDaKy(idNhanVien).Map<DTO_ModuleThongTinNhanSu_HopDongDaKy>();
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
        public DTO_ModuleThongTinNhanSu_HOPDONGLAODONG ModuleThongTinNhanSu_HOPDONGLAODONG_DLU(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_ModuleThongTinNhanSu_HOPDONGLAODONG obj = new DTO_ModuleThongTinNhanSu_HOPDONGLAODONG();
                try
                {
                    obj.HopDong =
                        factory.Context.spd_Web_HopDong(idNhanVien)
                            .Map<DTO_ModuleThongTinNhanSu_HopDong>()
                            .SingleOrDefault();
                }
                catch (Exception ex)
                { }
                try
                {
                    obj.DanhSach_HopDongDaKy =
                        factory.Context.spd_Web_HopDongDaKy(idNhanVien).Map<DTO_ModuleThongTinNhanSu_HopDongDaKy>();
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

                IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> objList = WebUser_Factory.New().Context.spd_Web_QuanHeGiaDinh(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh>();
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

        #endregion

        //Dien bien luong
        #region Dien bien luong
        public IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> ModuleThongTinNhanSu_DienBienLuong(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> objList = WebUser_Factory.New().Context.spd_Web_DienBienLuong(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DienBienLuong>();
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
        #endregion

        //lich su ban than
        #region lich su ban than
        public IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> ModuleThongTinNhanSu_LichSuBanThan(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> objList = WebUser_Factory.New().Context.spd_Web_LichSuBanThan(idNhanVien).Map<DTO_ModuleThongTinNhanSu_LichSuBanThan>();
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

        #endregion

        //qua trinh cong tac
        #region qua trinh cong tac
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> ModuleThongTinNhanSu_QuaTrinhCongTac(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> objList = WebUser_Factory.New().Context.spd_Web_QuaTrinhCongTac(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac>();
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

        #endregion

        //QuaTrinhDaoTao
        #region QuaTrinhDaoTao
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> ModuleThongTinNhanSu_QuaTrinhDaoTao(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> objList = null;//WebUser_Factory.New().Context.spd_Web_QuaTrinhDaoTao(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao>();
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

        #endregion

        //QuaTrinhDieuDong
        #region QuaTrinhDieuDong
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDieuDong> ModuleThongTinNhanSu_QuaTrinhDieuDong(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDieuDong> objList = WebUser_Factory.New().Context.spd_Web_QuaTrinhDieuDong(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhDieuDong>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        public String ModuleThongTinNhanSu_QuaTrinhDieuDong_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDieuDong> objList = ModuleThongTinNhanSu_QuaTrinhDieuDong(publicKey, token, idNhanVien);
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

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> objList = WebUser_Factory.New().Context.spd_Web_QuaTrinhBoNhiem(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        //QuaTrinhBoNhiemKiemNhiem
        #region QuaTrinhBoNhiemKiemNhiem
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiemKiemNhiem> ModuleThongTinNhanSu_QuaTrinhBoNhiemKiemNhiem(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiemKiemNhiem> objList = WebUser_Factory.New().Context.spd_Web_QuaTrinhBoNhiemKiemNhiem(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiemKiemNhiem>();
                return objList;
            }
            else
            {
                return null;
            }
        }
        #endregion

        public String ModuleThongTinNhanSu_QuaTrinhBoNhiemKiemNhiem_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiemKiemNhiem> objList = ModuleThongTinNhanSu_QuaTrinhBoNhiemKiemNhiem(publicKey, token, idNhanVien);
            String json = JsonConvert.SerializeObject(objList);
            return json;
        }

        public String ModuleThongTinNhanSu_QuaTrinhBoNhiem_Json(String publicKey, String token, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> objList = ModuleThongTinNhanSu_QuaTrinhBoNhiem(publicKey, token, idNhanVien);
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

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai> objList = null; // WebUser_Factory.New().Context.spd_Web_QuaTrinhDiNuocNgoai(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai>();
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
      
        #endregion

        //QuaTrinhKhenThuong
        #region QuaTrinhKhenThuong
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> ModuleThongTinNhanSu_QuaTrinhKhenThuong(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> objList = null;//WebUser_Factory.New().Context.spd_Web_QuaTrinhKhenThuong(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong>();
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
     
        #endregion

        //QuaTrinhKyLuat
        #region QuaTrinhKyLuat
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> ModuleThongTinNhanSu_QuaTrinhKyLuat(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> objList = null;// WebUser_Factory.New().Context.spd_Web_QuaTrinhKyLuat(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat>();
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
        #endregion

        //QuaTrinhNghienCuuKhoaHoc
        #region QuaTrinhNghienCuuKhoaHoc
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> objList = null; // WebUser_Factory.New().Context.spd_Web_QuaTrinhNghienCuuKhoaHoc(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc>();
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
     
        #endregion

        //QuaTrinhThamGiaHoatDongXaHoi
        #region QuaTrinhThamGiaHoatDongXaHoi
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> objList = null;// WebUser_Factory.New().Context.spd_Web_QuaTrinhThamGiaHoatDongXaHoi(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi>();
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
     
        #endregion

        //QuaTrinhThamGiaHoiThao
        #region QuaTrinhThamGiaHoiThao
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> objList = null;// WebUser_Factory.New().Context.spd_Web_QuaTrinhThamGiaHoiThao(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao>();
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
      
        #endregion

        //QuaTrinhThamGiaLucLuongVuTrang
        #region QuaTrinhThamGiaLucLuongVuTrang
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> objList = null;// WebUser_Factory.New().Context.spd_Web_QuaTrinhThamGiaLucLuongVuTrang(idNhanVien).Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang>();
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
        #endregion

        //DangVien
        #region DangVien
        public DTO_ModuleThongTinNhanSu_DangVien ModuleThongTinNhanSu_DangVien(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_DangVien obj = null; //WebUser_Factory.New().Context.spd_Web_DangVien(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DangVien>().SingleOrDefault();
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
        #endregion

        //DoanVien
        #region DoanVien
        public DTO_ModuleThongTinNhanSu_DoanVien ModuleThongTinNhanSu_DoanVien(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_DoanVien obj = null;//WebUser_Factory.New().Context.spd_Web_DoanVien(idNhanVien).Map<DTO_ModuleThongTinNhanSu_DoanVien>().SingleOrDefault();
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
        #endregion

        //CongDoan
        #region CongDoan
        public DTO_ModuleThongTinNhanSu_CongDoan ModuleThongTinNhanSu_CongDoan(String publicKey, String token, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                DTO_ModuleThongTinNhanSu_CongDoan obj = null;//WebUser_Factory.New().Context.spd_Web_CongDoan(idNhanVien).Map<DTO_ModuleThongTinNhanSu_CongDoan>().SingleOrDefault();
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
        #endregion

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
    }
}
