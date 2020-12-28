using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_EvaluationBoard
    {   
        public virtual Guid Id { get; set; }
        public virtual int Quater { get; set; }
        public virtual string Year { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime ToDate { get; set; }
        public virtual DateTime? StartRating { get; set; }
        public virtual DateTime? EndRating { get; set; }
    }
}
