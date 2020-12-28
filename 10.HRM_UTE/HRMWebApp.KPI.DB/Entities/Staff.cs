using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class Staff
    {
        public Staff()
        {
            StaffRoles = new List<StaffRole>();
        }
        public virtual Guid Id { get; set; }
        public virtual Department Department { get; set; }
        public virtual StaffInfo StaffInfo { get; set; }
        public virtual StaffProfile StaffProfile { get; set; }
        public virtual StaffStatus StaffStatus { get; set; }
        public virtual IList<StaffRole> StaffRoles { get; set; }
        public virtual IList<Department> Departments { get; set; }
        public virtual StaffLevel StaffLevel { get; set; }
        public virtual StaffSalaryInfo StaffSalaryInfo { get; set; }
        public virtual Jobs Jobs { get; set; }
    }
}
