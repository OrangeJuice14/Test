using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class EOfficeHRMDepartment
    {
        public virtual Guid EOfficeDepartmentId { get; set; }
        public virtual Guid DepartmentId { get; set; }
        public virtual string DepartmentName { get; set; }
    }
}
