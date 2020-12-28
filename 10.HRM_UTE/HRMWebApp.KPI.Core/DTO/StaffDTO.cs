using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class StaffDTO
    {
        public StaffDTO()
        {
            Department = new DepartmentDTO();
            Subject = new DepartmentDTO();
            Position = new PositionDTO();
            StaffProfile = new StaffProfile();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ManageCode { get; set; }
        public DepartmentDTO Department  { get; set; }
        public DepartmentDTO Subject { get; set; }
        public PositionDTO Position { get; set; }
        public StaffProfile StaffProfile { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public List<Guid> AgentObjectIds { get; set; }
        public List<Guid> DepartmentIds { get; set; }
        public int AgentObjectTypeId { get; set; }
        public Guid AgentObjectId { get; set; }
        public Guid StaffStatusId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsStaffRated { get; set; }
        public bool IsSupervisorRated { get; set; }
        public bool Checked { get; set; }
        public Guid PlanStaffId { get; set; }
        public string WorkingModeName { get; set; }
        public bool IsWorkingModeLocked { get; set; }
    }
}
