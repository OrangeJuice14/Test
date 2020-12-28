using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ResultDetailDTO
    {
        public Guid Id { get; set; }
        public double Record { get; set; }
        public string RegisterTarget { get; set; }      
        public double SupervisorRecord { get; set; }
        public Guid CriterionId { get; set; }
        public string CriterionName { get; set; }
        public PlanKPIDetail PlanKPIDetail { get; set; }
        public Guid PlanKPIDetailId { get; set; }
        public string PlanKPIDetailName { get; set; }
    }
}
