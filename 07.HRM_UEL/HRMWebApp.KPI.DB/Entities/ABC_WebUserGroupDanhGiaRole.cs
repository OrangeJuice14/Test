using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_WebUserGroupDanhGiaRole
    {
        public virtual Guid Id { get; set; }
        public virtual long? GCRecord { get; set; }
        public virtual DateTime? DeleteTime { get; set; }
        public virtual Guid? DeleteUserId { get; set; }
        public virtual DateTime? AddTime { get; set; }
        public virtual Guid? AddUserId { get; set; }
        public virtual WebUser WebUser { get; set; }
        public virtual ABC_GroupDanhGia GroupDanhGia { get; set; }
    }
}
