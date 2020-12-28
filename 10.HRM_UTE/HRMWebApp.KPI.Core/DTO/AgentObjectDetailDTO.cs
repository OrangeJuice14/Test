using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class AgentObjectDetailDTO
    {
        public Guid Id { get; set; }
        public Guid AgentObjectId { get; set; }
        public Guid WorkingModeId { get; set; }
        public string WorkingModeName { get; set; }
        //public PlanKPI Plan { get; set; }
        public double NumberOfSection { get; set; }
        public double ScienceResearch { get; set; }
        public double OtherActivity { get; set; }
        public double NumberOfSectionDensity { get; set; }
        public double ScienceResearchDensity { get; set; }
        public double OtherActivityDensity { get; set; }
    }
}
