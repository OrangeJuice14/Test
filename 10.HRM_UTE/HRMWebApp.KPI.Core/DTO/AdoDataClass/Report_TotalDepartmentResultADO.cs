using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.AdoDataClass
{
    public class Report_TotalDepartmentResultADO
    {
        public string TenBoPhan { get; set; }
        public string TenBoMon { get; set; }
        public int SoNV { get; set; }
        public bool BoPhanKhoa { get; set; }
        public bool BoMonKhoa { get; set; }
        public bool TruongDVDaDanhGia { get; set; }
        public bool NVDaDanhGia { get; set; }
    }
}
