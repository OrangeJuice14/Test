using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class AgentObjectTypeRateDTO
    {
        public int AgentObjectTypeId { get; set; }
        public string AgentObjectTypeName { get; set; }
        public double ResultRate { get; set; }
    }
}
