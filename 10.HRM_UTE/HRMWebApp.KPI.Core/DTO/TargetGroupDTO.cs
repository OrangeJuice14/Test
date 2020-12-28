using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class TargetGroupDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AgentObject AgentObject { get; set; }
        public Guid AgentObjectId { get; set; }
    }
}
