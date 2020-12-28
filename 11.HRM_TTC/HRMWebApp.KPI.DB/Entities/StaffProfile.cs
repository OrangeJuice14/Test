using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class StaffProfile
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Email { get; set; }
        public virtual int? GCRecord { get; set; }
    }
}
