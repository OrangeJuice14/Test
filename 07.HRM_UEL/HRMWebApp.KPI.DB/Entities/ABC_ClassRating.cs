using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_ClassRating
    {
        public virtual Guid Id { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual ABC_ClassEvaluationBoard ABC_ClassEvaluationBoard { get; set; }
        public virtual string Classification { get; set; }
        public virtual string ClassificationSecond { get; set; }
        public virtual string NoteSecond { get; set; }
        public virtual string ClassificationThird { get; set; }
        public virtual string NoteThird { get; set; }
        public virtual bool IsRated { get; set; }
        public virtual bool IsRatedSecond { get; set; }
        public virtual bool IsRatedThird { get; set; }
        public virtual bool IsRatingLocked { get; set; }
        public virtual Department Department { get; set; }
        public virtual DateTime? DateRated { get; set; }
        public virtual DateTime? DateRatedSecond { get; set; }
        public virtual DateTime? DateRatedThird { get; set; }
        public virtual Staff StaffRatingSecond { get; set; }
        public virtual Staff StaffRatingThird { get; set; }
    }
}
