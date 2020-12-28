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
                    //obj.ChiTietThuNhapCaNhan =
                    //    factory.Context.spd_Report_Luong_ChiTietTongThuNhapTungNhanVien(kyTinhLuong, idNhanVien, 1)
                    //        .Map<DTO_ModuleThongTinNhanSu_ChiTietThuNhapCaNhan>();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }
        public DTO_BangLuongBUH_LuongNganSach BangLuong_LuongNganSach(String publicKey, String token, Guid idNhanVien, int nam, int thang)
        {

            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_BangLuongBUH_LuongNganSach obj = new DTO_BangLuongBUH_LuongNganSach();
                try
                {
                    obj =factory.Context.spd_Web1_BangLuong_LuongNganSach(nam, thang, idNhanVien)
                        .Map<DTO_BangLuongBUH_LuongNganSach>().SingleOrDefault();
                }
                catch (Exception) { }
                return obj;
            }
            else
            {
                return null;
            }
        }
        public DTO_BangLuongBUH_LuongTamUng BangLuong_LuongTamUng(String publicKey, String token, Guid idNhanVien, int nam, int thang)
        {

            if (Helper.TrustTest(publicKey, token))
            {
                var factory = WebUser_Factory.New();
                DTO_BangLuongBUH_LuongTamUng obj = new DTO_BangLuongBUH_LuongTamUng();
                try
                {
                    obj = factory.Context.spd_Web1_BangLuong_LuongTamUng(nam, thang, idNhanVien)
                        .Map<DTO_BangLuongBUH_LuongTamUng>().SingleOrDefault();
                }
                catch (Exception) { }
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
        public String BangLuong_LuongNganSach_Json(String publicKey, String token, Guid idNhanVien, int nam, int thang)
        {//DANG SD
            DTO_BangLuongBUH_LuongNganSach obj = BangLuong_LuongNganSach(publicKey, token, idNhanVien, nam, thang);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public String BangLuong_LuongTamUng_Json(String publicKey, String token, Guid idNhanVien, int nam, int thang)
        {//DANG SD
            DTO_BangLuongBUH_LuongTamUng obj = BangLuong_LuongTamUng(publicKey, token, idNhanVien, nam, thang);
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

                DTO_ModuleThongTinNhanSu_SoYeuLyLich obj = WebUser_Factory.New().Context.spd_Web1_SoYeuLyLich(idNhanVien).SingleOrDefault()?.Map<DTO_ModuleThongTinNhanSu_SoYeuLyLich>();
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
        #endregion

        //QuaTrinhDiNuocNgoai
        #region QuaTrinhDiNuocNgoa
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
                    //obj.HoSoBaoHiem =
                    //    factory.Context.spd_Web1_HoSoBaoHiem(idNhanVien)
                    //        .Map<DTO_ModuleThongTinNhanSu_HoSoBaoHiem>()
                    //        .SingleOrDefault();
                }
                catch (Exception) { }
                try
                {
                    //obj.DanhSach_QuaTrinhThamGiaBHXH =
                    //    factory.Context.spd_Web1_QuaTrinhThamGiaBHXH(idNhanVien)
                    //        .Map<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaBHXH>();
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
        #endregion
    }
}
