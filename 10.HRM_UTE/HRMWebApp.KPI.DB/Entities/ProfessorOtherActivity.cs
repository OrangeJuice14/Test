using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ProfessorOtherActivity
    {
        public ProfessorOtherActivity()
        {
            CriterionDictionary = new CriterionDictionary();
        }
        public virtual Guid Id { get; set; }
        public virtual double NumberOfTime { get; set; }
        public virtual PlanKPIDetail PlanKPIDetail { get; set; }
        public virtual CriterionDictionary CriterionDictionary { get; set; }
        public virtual string Name { get; set; }
        public virtual double? NumberOfHour { get; set; }
        public virtual int OrderNumber { get; set; }

        /// <summary>
        /// 1: Được thêm từ biểu đánh giá
        /// </summary>
        public virtual int IsRating { get; set; }

        /// <summary>
        /// Kế thừa từ cấp trên
        /// </summary>
        public virtual bool IsAssign { get; set; }
        public virtual string ExecuteMethod { get; set; }
        public virtual string BasicResource { get; set; }
    }
}
