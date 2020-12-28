using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class MethodDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public DateTime StartTime { get; set; }
        public string StartTimeString { get; set; }
        public DateTime EndTime { get; set; }
        public string EndTimeString { get; set; }
        public Guid PlanKPIDetailId { get; set; }
    }
}
