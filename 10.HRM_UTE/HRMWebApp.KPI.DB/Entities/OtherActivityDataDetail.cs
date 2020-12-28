using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class OtherActivityDataDetail
    {
        public virtual Guid Id { get; set; }
        public virtual string StaffCode { get; set; }
        public virtual string StaffName { get; set; }
        public virtual double NumberOfTime { get; set; }
        public virtual Department Khoa { get; set; }
        public virtual Guid IdCanBo { get; set; }
    }
}
