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
        public virtual ABC_RatingType ABC_RatingType { get; set; }
        public virtual Guid CopyFromCriterion { get; set; }
        public virtual ABC_RatingType CopyFromRatingType { get; set; }
        public virtual bool IsGroupByRatingTypeOnEvaluationBoard { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
