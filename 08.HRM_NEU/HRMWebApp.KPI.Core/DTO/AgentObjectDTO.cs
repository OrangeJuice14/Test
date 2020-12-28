using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class AgentObjectDTO
    {
        public AgentObjectDTO()
        {
            TargetGroupDetailIds = new List<Guid>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> TargetGroupDetailIds { get; set; }
        public int NumberOfSection { get; set; }
        public int ScienceResearch { get; set; }
        public int AgentObjectTypeId { get; set; }
       
    }
}
