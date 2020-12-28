using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
   public class MethodStaffDTO
    {
        public MethodStaffDTO()
        {
            PlanDetailSubStaffs = new List<MethodSubStaffDTO>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public DateTime StartTime { get; set; }
        public string StartTimeString { get; set; }
        public DateTime EndTime { get; set; }
        public string EndTimeString { get; set; }
        public Guid PlanKPIDetailId { get; set; }
        public List<MethodSubStaffDTO> PlanDetailSubStaffs { get; set; }
        public PlanKPIDetailDTO PlanKPIDetail { get; set; }
        public Guid Planstaff { get; set; }
       // public List<Guid> LeadDepartment { get; set; }
        public Guid LeadDepartmentId { get; set; }
       // public List<DepartmentDTO> LeadDepartment { get; set; }
        public List<Method_DepartmentDTO> LeadDepartment { get; set; }
    }
}
