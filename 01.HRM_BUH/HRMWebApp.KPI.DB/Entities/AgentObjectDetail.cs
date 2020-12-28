using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class AgentObjectDetail
    {
        public virtual Guid Id { get; set; }
        public virtual int NumberOfSection { get; set; }
        public virtual int ScienceResearch { get; set; }

    }
}
