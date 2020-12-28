using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class Criterion
    {
        public Criterion()
        {
            CriterionDictionaries = new List<CriterionDictionary>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double MaxRecord { get; set; }
        public virtual string Tooltip { get; set; }
        public virtual TargetGroupDetail TargetGroupDetail { get; set; }
        public virtual Department Department { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Staff StaffLeader { get; set; }
        public virtual PlanKPI PlanKPI { get; set; }
        public virtual int OrderNumber { get; set; }
        public virtual PlanKPIDetail  FromPlanKPIDetail { get; set; }
        public virtual CriterionType CriterionType { get; set; }
        public virtual IList<PlanKPIDetail> ToPlanKPIDetails { get; set; }
        public virtual IList<CriterionDictionary> CriterionDictionaries { get; set; }
        public virtual string ServiceUrl { get; set; }
    }
}
