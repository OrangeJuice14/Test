using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class KyTinhLuongDTO
    {
        public Guid Oid { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int? GCRecord { get; set; }
        public string Name { get; set; }

    }
}
