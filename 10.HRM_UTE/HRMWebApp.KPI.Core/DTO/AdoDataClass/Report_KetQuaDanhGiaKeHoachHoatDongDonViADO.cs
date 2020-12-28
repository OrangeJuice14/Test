using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.AdoDataClass
{
    public class Report_KetQuaDanhGiaKeHoachHoatDongDonViADO
    {
        public string TenBoPhan { get; set; }
        public double NMT1QL { get; set; }
        public double NMT2QL { get; set; }
        public double NMT3QL { get; set; }
        public double NMT1BGH { get; set; }
        public double NMT2BGH { get; set; }
        public double NMT3BGH { get; set; }
        public string TongDiem_XepLoai { get; set; }
        public int SoNV { get; set; }
        public int NV_A { get; set; }
        public double PhanTram_A { get; set; }
        public int NV_B { get; set; }
        public double PhanTram_B { get; set; }
        public int NV_C { get; set; }
        public double PhanTram_C { get; set; }
        public int NV_D { get; set; }
        public double PhanTram_D { get; set; }
        public int NV_E { get; set; }
        public double PhanTram_E { get; set; }
        public int NV_F { get; set; }
        public double PhanTram_F { get; set; }
    }
}
