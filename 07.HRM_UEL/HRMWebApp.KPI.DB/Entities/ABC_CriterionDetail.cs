using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_CriterionDetail
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal MaxRecord { get; set; }
        public virtual int OrderNumber { get; set; }
        public virtual ABC_Criterion ABC_Criterion { get; set; }
        public virtual ABC_CriterionDetailType ABC_CriterionDetailType { get; set; }
    }
}
