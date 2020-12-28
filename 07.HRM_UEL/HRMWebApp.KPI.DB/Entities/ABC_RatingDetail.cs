using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_RatingDetail
    {
        public virtual Guid Id { get; set; }
        public virtual decimal StaffRecord { get; set; }
        public virtual decimal SupervisorRecord { get; set; }
        public virtual string SupervisorNote { get; set; }
        public virtual decimal AdminRecord { get; set; }
        public virtual string AdminNote { get; set; }
        public virtual ABC_CriterionDetail ABC_CriterionDetail { get; set; }
        public virtual ABC_Criterion ABC_Criterion { get; set; }
        public virtual ABC_Rating ABC_Rating { get; set; }
    }
}
