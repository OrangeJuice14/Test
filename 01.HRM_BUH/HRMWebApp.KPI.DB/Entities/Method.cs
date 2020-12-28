using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class Method
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual PlanKPIDetail PlanKPIDetail { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime EndTime { get; set; }
        public virtual int OrderNumber { get; set; }
       
    }
}
