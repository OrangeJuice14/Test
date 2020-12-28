using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using System.Web.Http;
using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.DTO.AdoDataClass;
using HRMWebApp.KPI.Core.Helpers;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class PMSApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<NamHoc> GetNamHoc()
        {
            return DataClassHelper.GetNamHoc();
        }

        [Authorize]
        [Route("")]
        public IEnumerable<HocKy> GetHocKy(Guid NamHoc)
        {
            return DataClassHelper.GetHocKy(NamHoc);
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public IEnumerable<spd_PMS_ThongTinThoiKhoaBieu_GiangVien> DanhSachThoiKhoaBieu(Guid NamHoc, Guid HocKy, Guid GiangVien)
        {
            return DataClassHelper.spd_PMS_ThongTinThoiKhoaBieu_GiangVien(NamHoc, HocKy, GiangVien);
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public int XacNhanThoiKhoaBieu(Guid? OidChiTiet, string User, string GhiChu)
        {
            return DataClassHelper.spd_PMS_ThoiKhoaBiet_XacNhan_Web(OidChiTiet, User, GhiChu);
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public IEnumerable<spd_PMS_GetLoaiHoatDongAll> GetLoaiHoatDongAll()
        {
            return DataClassHelper.spd_PMS_GetLoaiHoatDongAll();
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public IEnumerable<spd_PMS_KeKhai_CacHD_TKB> KeKhaiHDTKB(Guid NamHoc, Guid HocKy, Guid GiangVien)
        {
            return DataClassHelper.spd_PMS_KeKhai_CacHD_TKB(NamHoc, HocKy, GiangVien);
        }

        [Authorize]
        [Route("")]
        [HttpPut]
        public int KeKhaiHDTKBCapNhat(spd_PMS_KeKhai_CacHD_TKB_CapNhat obj)
        {
            return DataClassHelper.spd_PMS_KeKhai_CacHD_TKB_CapNhat(obj.OidChiTiet, obj.User, obj.SoBaiKT, obj.SoBaiThi, obj.SoBaiTapLon, obj.SoBaiTieuLuan, obj.SoDeAnMonHoc, obj.SoChuyenDeTN, obj.SoHDKhac, obj.SoSlotHoc, obj.SoTraLoiCauHoi, obj.SoTruyCapLopHoc, obj.SoDeRaDe, obj.SoLuongHuongDan);
        }

        [Authorize]
        [Route("")]
        [HttpPut]
        public int KeKhaiHDTKBThem(spd_PMS_KeKhai_CacHD_TKB_Them obj)
        {
            return DataClassHelper.spd_PMS_KeKhai_CacHD_TKB_Them(obj.Oid_NhanVien, obj.Oid_LoaiHuongDan, obj.NamHoc, obj.HocKy, obj.BacDaoTao, obj.HeDaoTao, obj.TenMonHoc, obj.LopHocPhan, obj.BoMon, obj.SoBaiKiemTra, obj.SoBaiThi, obj.SoBaiTapLon, obj.SoBaiTieuLuan, obj.SoDeAnMonHoc, obj.SoChuyenDeTN, obj.SoHDKhac, obj.SoSlotHoc, obj.SoTraLoiCauHoi, obj.SoTruyCapLopHoc, obj.SoDeRaDe, obj.SoLuongHuongDan);
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public IEnumerable<spd_DanhMuc_GetDSHoatDong> DanhSachHoatDong(Guid? NhomHoatDong)
        {
            return DataClassHelper.spd_DanhMuc_GetDSHoatDong(NhomHoatDong);
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public IEnumerable<spd_KeKhaiHDKhac_LayDanhSachKeKhai> HDKhac_LayDanhSachKeKhai(Guid NamHoc, Guid HocKy, Guid GiangVien)
        {
            return DataClassHelper.spd_KeKhaiHDKhac_LayDanhSachKeKhai(NamHoc, HocKy, GiangVien);
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public spd_PMS_KeKhai_CacHoatDongKhac HDKhac_KeKhai(Guid NamHoc, Guid HocKy, Guid GiangVien, Guid HoatDong, Guid BoMon, decimal SoGioThucHien, string GhiChu, DateTime? NgayThucHien)
        {
            return DataClassHelper.spd_PMS_KeKhai_CacHoatDongKhac(NamHoc, HocKy, GiangVien, HoatDong, BoMon, SoGioThucHien, GhiChu, NgayThucHien);
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public int HDKhac_CapNhatGioKeKhai(Guid OidChiTiet, decimal SoGio, string GhiChu, string User)
        {
            return DataClassHelper.spd_KeKhaiHDKhac_CapNhatGioKeKhai(OidChiTiet, SoGio, GhiChu, User);
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public IEnumerable<spd_PMS_XacNhanKeKhai> HDKhac_LayDanhSachKeKhai_Duyet(Guid NamHoc, Guid HocKy, Guid BoPhan, Guid NhanVien)
        {
            return DataClassHelper.spd_PMS_XacNhanKeKhai(NamHoc, HocKy, BoPhan, NhanVien);
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public int HDKhac_DonVi_Duyet(Guid OidChiTiet, int TrangThai, string User)
        {
            return DataClassHelper.spd_KeKhaiHDKhac_DonVi_Duyet(OidChiTiet, TrangThai, User);
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public spd_PMS_KiemTraPhanQuyenImport KiemTraKhoaImport(Guid? NamHoc, Guid? HocKy, Guid? NhanVien)
        {
            return DataClassHelper.spd_PMS_KiemTraPhanQuyenImport(NamHoc, HocKy, NhanVien);
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public IEnumerable<BacDaoTao> GetBacDaoTao()
        {
            return DataClassHelper.GetBacDaoTao();
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public IEnumerable<HeDaoTao> GetHeDaoTao()
        {
            return DataClassHelper.GetHeDaoTao();
        }

        [Authorize]
        [Route("")]
        [HttpGet]
        public IEnumerable<BoPhan> GetBoPhan()
        {
            return DataClassHelper.GetBoPhan();
        }
    }

    public class spd_PMS_KeKhai_CacHD_TKB_CapNhat
    {
        public Guid? OidChiTiet { get; set; }
        public string User { get; set; }
        public int SoBaiKT { get; set; }
        public int SoBaiThi { get; set; }
        public int SoBaiTapLon { get; set; }
        public int SoBaiTieuLuan { get; set; }
        public int SoDeAnMonHoc { get; set; }
        public int SoChuyenDeTN { get; set; }
        public int SoHDKhac { get; set; }
        public int SoSlotHoc { get; set; }
        public int SoTraLoiCauHoi { get; set; }
        public int SoTruyCapLopHoc { get; set; }
        public int SoDeRaDe { get; set; }
        public int SoLuongHuongDan { get; set; }
    }

    public class spd_PMS_KeKhai_CacHD_TKB_Them
    {
        public Guid? Oid_NhanVien { get; set; }
        public Guid? Oid_LoaiHuongDan { get; set; }
        public Guid? NamHoc { get; set; }
        public Guid? HocKy { get; set; }
        public Guid? BacDaoTao { get; set; }
        public Guid? HeDaoTao { get; set; }
        public string TenMonHoc { get; set; }
        public string LopHocPhan { get; set; }
        public Guid? BoMon { get; set; }
        public int? SoBaiKiemTra { get; set; }
        public int? SoBaiThi { get; set; }
        public int? SoBaiTapLon { get; set; }
        public int? SoBaiTieuLuan { get; set; }
        public int? SoDeAnMonHoc { get; set; }
        public int? SoChuyenDeTN { get; set; }
        public int? SoHDKhac { get; set; }
        public int? SoSlotHoc { get; set; }
        public int? SoTraLoiCauHoi { get; set; }
        public int? SoTruyCapLopHoc { get; set; }
        public int? SoDeRaDe { get; set; }
        public int? SoLuongHuongDan { get; set; }
    }
}
