using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class PlanDetailSubStaff
    {
        public virtual Staff Staff { get; set; }
        public virtual PlanKPIDetail PlanKPIDetail { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = obj as PlanDetailSubStaff;
            if (t == null)
                return false;
            if (Staff == t.Staff && PlanKPIDetail == t.PlanKPIDetail)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return ((Staff!=null? Staff.Id: Guid.Empty) + "|" + (PlanKPIDetail!=null? PlanKPIDetail.Id:Guid.Empty)).GetHashCode();
        }
        public virtual double Density { get; set; }
        public virtual double NumberOfHour { get; set; }
    }
}
