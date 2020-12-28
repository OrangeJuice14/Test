using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class PlanDetailSubStaffDTO
    {
        public Guid StaffId { get; set; }
        public string StaffName { get; set; }
        public Guid PlanKPIDetailId { get; set; }
        public double Density { get; set; }
        public double NumberOfHour { get; set; }
    }
}
