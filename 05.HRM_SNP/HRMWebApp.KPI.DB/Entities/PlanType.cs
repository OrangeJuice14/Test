using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class PlanType
    {
        public PlanType()
        {
            AgentObjectTypes = new List<AgentObjectType>();
        }
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<AgentObjectType> AgentObjectTypes { get; set; }
    }
}
