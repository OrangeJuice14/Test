using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.AdoDataClass
{
    public class spd_PMS_ThoiKhoaBieu_DanhSachPhanCongGiangDay
    {
        public string TenBoPhan { get; set; } 
        public string TenMonHoc { get; set; } 
        public string MaMonHoc { get; set; } 
        public string LopHocPhan { get; set; }
        public string SoHieuCongChuc { get; set; }
        public string HoTen { get; set; }
        public Guid OidChiTiet { get; set; } 
    }
}
