using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_Criterion
    {   
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int CriterionType { get; set; }
        public virtual int OrderNumber { get; set; }
        public virtual int IsNotVisibleInEvaluationBoardType { get; set; }
    }
}
