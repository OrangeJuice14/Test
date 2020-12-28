using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.AdoDataClass
{
    public class spd_PMS_GetLoaiHoatDongAll
    {
        public Guid? Oid { get; set; }
        public string MaQuanLy { get; set; }
        public string TenLoaiHoatDong { get; set; }
    }
}
