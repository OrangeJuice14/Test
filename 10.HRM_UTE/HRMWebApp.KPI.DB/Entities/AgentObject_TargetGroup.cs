using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class AgentObject_TargetGroup
    {
        public virtual Guid Id { get; set; }
        public virtual int TiTrong { get; set; }
        public virtual AgentObject AgentObjectId { get; set; }
        public virtual TargetGroupDetail TargetGroupDetailId { get; set; }
        public virtual string AgentObjectName { get; set; }
        public virtual string TargetGroupDetailName { get; set; }
    }
}
