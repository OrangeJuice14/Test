using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.AdoDataClass
{
    public class Report_DepartmentStaffResultADO
    {
        public string TenBoPhan { get; set; }
        public string HoTen { get; set; }
        public bool TruongDonViDuyet { get; set; }
        public bool CaNhanDanhGia { get; set; }
        public bool TruongDonViDanhGia { get; set; }
    }
}
