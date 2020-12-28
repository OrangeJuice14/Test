using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class PlanStaff
    {
        public PlanStaff()
        {
            PlanKPIDetails = new List<PlanKPIDetail>();
            
        }
        public virtual Guid Id { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual DateTime ModifiedTime { get; set; }
        public virtual bool IsLocked { get; set; }
        public virtual PlanKPI PlanKPI { get; set; }
        public virtual string Vision { get; set; }
        public virtual string Mission { get; set; }
        public virtual IList<PlanKPIDetail> PlanKPIDetails { get; set; }
        public virtual Guid WebGroupId { get; set; }
        public virtual Department Department { get; set; }
    }
}
