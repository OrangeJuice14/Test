using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_LockedRatingDepartment
    {   
        public virtual Guid Id { get; set; }
        public virtual bool Status { get; set; }
        public virtual Department Department { get; set; }
        public virtual ABC_EvaluationBoard ABC_EvaluationBoard { get; set; }
    }
}
