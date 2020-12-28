using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.AdoDataClass
{
    public class Report_KetQuaDanhGiaKeHoachHoatDongCaNhanADO
    {
        public string SoHieuCongChuc { get; set; }
        public string HoTen { get; set; }
        public string BoPhan { get; set; }
        public double NMT1NV { get; set; }
        public double NMT2NV { get; set; }
        public double NMT3NV { get; set; }
        public double TongNV { get; set; }
        public double NMT1QL { get; set; }
        public double NMT2QL { get; set; }
        public double NMT3QL { get; set; }
        public double TongQL { get; set; }
        public double TongKPI { get; set; }
        public string XepLoai { get; set; }

    }
}
