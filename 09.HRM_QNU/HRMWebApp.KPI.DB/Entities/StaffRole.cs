using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class StaffRole
    {       
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double ManagementDensity { get; set; }
        public virtual AgentObject AgentObject { get; set; }
        public virtual IList<Staff> Staffs { get; set; }

    }
}
