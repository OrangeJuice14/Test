using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
   public class MethodSubStaff
    {
        public virtual Staff Staff { get; set; }
        public virtual Method Method { get; set; }
    }
}
