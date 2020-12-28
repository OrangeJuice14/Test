using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class AgentObject_TargetGroupDTO
    {
        public  Guid Id { get; set; }
        public int TiTrong { get; set; }
        public  AgentObjectDTO AgentObjectId { get; set; }
        public  TargetGroupDetailDTO TargetGroupDetailId { get; set; }
        public string TargetGroupDetailName { get; set; }
        public string AgentObjectName { get; set; }
        public string Name { get; set; }
        public int AgentObjectTypeId { get; set; }
        public int TargetGroupDetailTypeId { get; set; }
    }
}
