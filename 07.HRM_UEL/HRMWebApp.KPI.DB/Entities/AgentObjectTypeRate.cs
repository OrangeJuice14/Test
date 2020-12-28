using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class AgentObjectTypeRate
    {
        public virtual int AgentObjectTypeId { get; set; }
        public virtual double ResultRate { get; set; }
    }
}
