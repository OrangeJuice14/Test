using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.AdoDataClass
{
    public class spd_PMS_ThongTinThoiKhoaBieu_GiangVien
    {
        public string HoTen { get; set; }
        public string MaNhanVien { get; set; }
        public string CMND { get; set; }
        public string TenDonVi { get; set; }
        public string KhoaVien { get; set; }
        public string BoMon { get; set; }
        public string TenTrinhDoChuyenMon { get; set; }
        public string TenChucDanh { get; set; }
        public string MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }
        public string MaLopHocPhan { get; set; }
        public string TenLopHocPhan { get; set; }
        public string HinhThucGiangDay { get; set; }
        public string LoaiHocPhan { get; set; }
        public string TenBacDaoTao { get; set; }
        public string TenHeDaoTao { get; set; }
        public double? SoTinChi { get; set; }
        public double? SoTietDungLop { get; set; }
        public double? SoTietHeThong { get; set; }
        public double? SoGioChuanDungLop { get; set; }
        public double? SoSinhVienDK { get; set; }
        public string ThoiGianGiangDay { get; set; }
        public string KhoaDaoTao { get; set; }
        public string GhiChu { get; set; }
        public Guid? Oid { get; set; }
        public bool? XacNhan { get; set; }
    }
}
