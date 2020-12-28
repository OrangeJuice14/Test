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
        public virtual Guid StaffRecord { get; set; }
        public virtual Guid SupervisorRecord { get; set; }
        public virtual ABC_Rating ABC_Rating { get; set; }
        public virtual ABC_Criterion ABC_Criterion { get; set; }
    }
}
