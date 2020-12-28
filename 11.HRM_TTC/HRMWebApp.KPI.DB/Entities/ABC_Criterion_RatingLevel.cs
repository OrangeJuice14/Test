using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_Criterion_RatingLevel
    {
        public virtual ABC_Criterion ABC_Criterion { get; set; }
        public virtual ABC_RatingLevel ABC_RatingLevel { get; set; }
        //public virtual Guid CriterionId { get; set; }
        //public virtual Guid RatingLevelId { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsTemp { get; set; }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
