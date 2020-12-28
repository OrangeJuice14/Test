using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class AgentObjectType
    {
        public AgentObjectType()
        {
            AgentObjects = new List<AgentObject>();
            Positions = new List<Position>();
            AgentObjectTypeRate = new AgentObjectTypeRate();
            PlanTypes = new List<PlanType>();
            Roles = new List<Role>();
        }

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual AgentObjectType ParentAgentObjectType { get; set; }
        public virtual IList<AgentObject> AgentObjects { get; set; }
        public virtual IList<Position> Positions { get; set; }
        public virtual AgentObjectTypeRate AgentObjectTypeRate { get; set; }
        public virtual IList<PlanType> PlanTypes { get; set; }
        public virtual IList<Role> Roles { get; set; }
    }
}
