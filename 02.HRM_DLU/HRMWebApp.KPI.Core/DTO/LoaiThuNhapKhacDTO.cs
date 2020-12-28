using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class LoaiThuNhapKhacDTO
    {
        public Guid Oid { get; set; }
        public string MaQuanLy { get; set; }
        public string TenLoaiThuNhapKhac { get; set; }
        public int? GCRecord { get; set; }
    }
}
