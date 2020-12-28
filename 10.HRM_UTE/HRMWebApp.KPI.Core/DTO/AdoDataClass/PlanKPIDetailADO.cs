using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.AdoDataClass
{
   public class PlanKPIDetailADO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //public string CreateTime { get; set; }
        public Guid ParentPlanKPIDetailId { get; set; }
    }
}
