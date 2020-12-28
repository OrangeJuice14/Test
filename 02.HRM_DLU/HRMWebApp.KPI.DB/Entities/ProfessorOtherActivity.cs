using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ProfessorOtherActivity
    {
        public ProfessorOtherActivity()
        {
            CriterionDictionary = new CriterionDictionary();
        }
        public virtual Guid Id { get; set; }
        public virtual int NumberOfTime { get; set; }
        public virtual PlanKPIDetail PlanKPIDetail { get; set; }
        public virtual CriterionDictionary CriterionDictionary { get; set; }

    }
}
