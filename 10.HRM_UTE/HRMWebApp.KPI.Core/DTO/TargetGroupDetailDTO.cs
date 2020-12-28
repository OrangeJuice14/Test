using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class TargetGroupDetailDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Density { get; set; }
        public int OrderNumber { get; set; }
        //public TargetGroup TargetGroup { get; set; }
        public Guid TargetGroupId { get; set; }
        public int TargetGroupDetailTypeId { get; set; }
        public List<Guid> AgentObjectIds { get; set; }
        public List<Guid> StudyYearIds { get; set; }
        public Boolean IsCheck { get; set; }
        //public List<Guid> TargetGroupDetailIds { get; set; }
    }
}
