using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class DuToanNoiBo_BoPhanUserRole
    {
        public virtual Guid Id { get; set; }
        public virtual Department Department { get; set; }
        public virtual WebUser WebUser { get; set; }
    }
}
