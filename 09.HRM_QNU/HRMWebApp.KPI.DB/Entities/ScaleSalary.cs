using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ScaleSalary //Ngạch lương
    {
        public virtual Guid Id { get; set; }
        public virtual string ManageCode { get; set; }
        public virtual string Name { get; set; }
    }
}
