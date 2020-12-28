using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class KPI_WebUser
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Password { get; set; }
        public virtual Guid WebGroupId { get; set; }
        public virtual StaffInfo StaffInfo { get; set; }
        public virtual int AgentObjectTypeId { get; set; }
        public virtual Department Department { get; set; }
        public virtual SubPosition SubPosition { get; set; }
    }
}
