using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.PlanMakingDTOs
{
    public class PlanDetailMakingDTO
    {
        public Guid Id { get; set; }
        public int PlanTypeId { get; set; }
        public StaffDTO StaffDTO { get; set; }
        public StaffDTO Supervisor { get; set; }
        public List<TargetGroupPlanMakingDTO> TargetGroupPlanMakings { get; set; }
        public List<PlanKPIMakingDetailDTO> AdditionalPlanDetails { get; set; }
        public Guid PlanId { get; set; }
        public DateTime StartPlanTime { get; set; }
        public DateTime EndPlanTime { get; set; }
        public bool IsSupervisor { get; set; }
        public Guid PlanStaffId { get; set; }
        public bool IsLocked { get; set; }
        public Guid AgentObjectId { get; set; }
        public string AgentObjectName { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public bool IsDisable { get; set; }
        public bool IsFromEoffice { get; set; }
        public bool IsViewer { get; set; }        
    }
}
