using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class PlanKPI
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        //public virtual AgentObject AgentObject { get; set; }
        public virtual IList<AgentObject> AgentObjects { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime EndTime { get; set; }
        public virtual DateTime RatingStartTime { get; set; }
        public virtual DateTime RatingEndTime { get; set; }
        public virtual PlanKPI ParentPlan { get; set; }
        public virtual string StudyYear { get; set; }
        public virtual string StudyTerm { get; set; }
        public virtual PlanType PlanType { get; set; }
    }
}
