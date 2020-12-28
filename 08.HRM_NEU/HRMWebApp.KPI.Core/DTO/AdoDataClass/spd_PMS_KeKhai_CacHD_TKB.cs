using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.AdoDataClass
{
    public class spd_PMS_KeKhai_CacHD_TKB
    {
        public string HoTen { get; set; }
        public string MaNhanVien { get; set; }
        public string CMND { get; set; }
        public string TenBoPhan { get; set; }
        public string BoMonQuanLy { get; set; }
        public string TenMonHoc { get; set; }
        public string LopMonHoc { get; set; }
        public int? SoBaiKiemTra { get; set; }
        public int? SoBaiThi { get; set; }
        public int? SoBaiTapLon { get; set; }
        public int? SoBaiTieuLuan { get; set; }
        public int? SoDeAnTotNghiep { get; set; }
        public int? SoChuyenDeTotNghiep { get; set; }
        public int? SoHDKhac { get; set; }
        public int? SoSlotHoc { get; set; }
        public int? SoTraLoiCauHoiTrenHeThongHocTap { get; set; }
        public int? SoTruyCapLopHoc { get; set; }
        public int? SoDeRaDe { get; set; }
        public Guid? Oid { get; set; }
        public Guid? OidHuongDan { get; set; }
        public string TenHuongDan { get; set; }
        public int? SoLuongHuongDan { get; set; }
        public bool? HieuLucXacNhan { get; set; }
    }
}
