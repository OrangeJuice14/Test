using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class SubPosition
    {
        public virtual Guid Id { get; set; }
        public virtual StaffInfo StaffInfo { get; set; }
        public virtual Position Position { get; set; }
        public virtual Department Department { get; set; }
        public virtual int? GCRecord { get; set; }
        public virtual DateTime? NgayBoNhiem { get; set; }
    }
}
