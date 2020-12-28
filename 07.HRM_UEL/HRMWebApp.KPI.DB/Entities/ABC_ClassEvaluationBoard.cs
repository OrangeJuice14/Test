using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_ClassEvaluationBoard
    {
        public virtual Guid Id { get; set; }
        public virtual int Month { get; set; }
        public virtual int Year { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime ToDate { get; set; }
        public virtual ABC_ClassEvaluationBoard ABC_ParentClassEvaluationBoard { get; set; }
    }
}
