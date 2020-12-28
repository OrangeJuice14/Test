using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.AdoDataClass
{
    public class MethodADO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PlanKPIDetailId { get; set; }
    }
}
