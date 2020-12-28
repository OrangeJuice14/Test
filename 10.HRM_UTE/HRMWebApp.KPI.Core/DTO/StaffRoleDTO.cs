using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class StaffRoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double ManagementDensity { get; set; }
        public Guid AgentObjectId { get; set; }
        public Staff Staff { get; set; }
        public Guid StaffId { get; set; }
        public List<Guid> StaffIds { get; set; }
    }
}
