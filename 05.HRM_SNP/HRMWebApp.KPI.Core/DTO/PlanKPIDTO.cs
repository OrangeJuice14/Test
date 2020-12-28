using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class PlanKPIDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime RatingStartTime { get; set; }
        public DateTime RatingEndTime { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? SelectedId { get; set; }
        public Guid? SelectedParentId { get; set; }
        public AgentObject AgentObject { get; set; }
        public Guid AgentObjectId { get; set; }
        public string AgentObjectName { get; set; }
      
        public int PlanTypeId { get; set; }
        public string StudyYear { get; set; }
        public List<Guid> AgentObjectIds { get; set; }

    }
}
