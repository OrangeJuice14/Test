using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class TargetGroupDetail
    {
        public TargetGroupDetail()
        {
            AgentObjects = new List<AgentObject>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double Density { get; set; }
        public virtual int OrderNumber { get; set; }
        public virtual TargetGroupDetail ParentTargetGroupDetail { get; set; }
        public virtual IList<AgentObject> AgentObjects { get; set; }
        public virtual TargetGroupDetailType TargetGroupDetailType { get; set; }
    }
}
