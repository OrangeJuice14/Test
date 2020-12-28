using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class AgentObject
    {
        public AgentObject()
        {
           // TargetGroupDetails = new List<TargetGroupDetail>();
            AgentObjectDetails = new List<AgentObjectDetail>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual AgentObjectType AgentObjectType { get; set; }
       // public virtual IList<TargetGroupDetail> TargetGroupDetails { get; set; }
        public virtual IList<PlanKPI> PlanKPIs { get; set; }
        public virtual IList<AgentObjectDetail> AgentObjectDetails { get; set; }
    }
}
