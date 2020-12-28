using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.PlanMakingDTOs
{
    public class UserPlanKPIDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StaffName { get; set; }
        public Guid StaffId { get; set; }
        public AgentObjectDTO AgentObject { get; set; }
        public Guid AgentObjectId { get; set; }
        public int AgentObjectTypeId { get; set; }
        public string AgentObjectName { get; set; }
        public bool IsLocked { get; set; }
        public List<Guid> AgentObjectIds { get; set; }
    }
}
