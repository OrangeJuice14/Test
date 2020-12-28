using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.AdoDataClass
{
    public class spd_PMS_XacNhanKeKhai
    {
        public Guid? Oid { get; set; }
        public string DonVi { get; set; }
        public string HoTen { get; set; }
        public string MaQuanLy { get; set; }
        public string TenHoatDong { get; set; }
        public decimal? SoGioThucHien { get; set; }
        public DateTime? NgayThucHien { get; set; }
        public string GhiChu { get; set; }
        public int? TrangThai { get; set; }
        public string TenHocKy { get; set; }
    }
}
