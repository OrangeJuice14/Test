using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class KyTinhLuong_Nh
    {
        public virtual Guid Oid { get; set; }
        public virtual int Thang { get; set; }
        public virtual int Nam { get; set; }
        public virtual int? GCRecord { get; set; }
    }
}
