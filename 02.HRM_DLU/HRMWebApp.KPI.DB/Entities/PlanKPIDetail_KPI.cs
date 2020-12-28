using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class PlanKPIDetail_KPI
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual PlanKPIDetail PlanKPIDetail { get; set; }
        public virtual MeasureUnit MeasureUnit { get; set; }

    }
}
