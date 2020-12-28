using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class CriterionDTO
    {
        
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Tooltip { get; set; }
            public double MaxRecord { get; set; }
            public TargetGroupDetail TargetGroupDetail { get; set; }
            public Guid TargetGroupDetailId { get; set; }
            public AgentObject AgentObject { get; set; }
            public Guid AgentObjectId { get; set; }
            public DepartmentDTO Department { get; set; }
            public Guid DepartmentId { get; set; }
            public int OrderNumber { get; set; }
            public int CriterionTypeId { get; set; }
            public CriterionType CriterionType { get; set; }
        
    }
}
