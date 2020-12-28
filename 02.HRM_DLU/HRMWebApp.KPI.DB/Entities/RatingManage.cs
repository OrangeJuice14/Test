using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class RatingManage
    {
        public virtual Guid Id { get; set; }
        public virtual PlanKPI PlanKPI { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Department Department { get; set; }
        public virtual DateTime RatingStartTime { get; set; }
        public virtual DateTime RatingEndTime { get; set; }
    }
}
