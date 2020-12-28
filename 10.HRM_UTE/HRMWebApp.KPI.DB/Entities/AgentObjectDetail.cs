using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class AgentObjectDetail
    {
        public virtual Guid Id { get; set; }
        public virtual AgentObject AgentObject { get; set; }
        public virtual WorkingMode WorkingMode { get; set; }
        //public virtual PlanKPI Plan { get; set; }
        public virtual double NumberOfSection { get; set; }
        public virtual double ScienceResearch { get; set; }
        public virtual double OtherActivity { get; set; }
        public virtual double NumberOfSectionDensity { get; set; }
        public virtual double ScienceResearchDensity { get; set; }
        public virtual double OtherActivityDensity { get; set; }
    }
}
