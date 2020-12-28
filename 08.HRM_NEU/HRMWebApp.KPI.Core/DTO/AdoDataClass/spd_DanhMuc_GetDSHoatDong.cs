using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.AdoDataClass
{
    public class spd_DanhMuc_GetDSHoatDong
    {
        public Guid? Oid { get; set; }
        public string MaQuanLy { get; set; }
        public string TenHoatDong { get; set; }
    }
}
