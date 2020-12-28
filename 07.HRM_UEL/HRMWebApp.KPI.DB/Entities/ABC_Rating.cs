using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_Rating
    {   
        public virtual Guid Id { get; set; }
        public virtual bool IsRated { get; set; }
        public virtual bool IsSupervisorRated { get; set; }
        public virtual bool IsRatingLocked { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Staff StaffRating { get; set; }
        public virtual Department Department { get; set; }
        public virtual ABC_EvaluationBoard ABC_EvaluationBoard { get; set; }
        public virtual decimal TotalStaffRecord { get; set; }
        public virtual decimal TotalSupervisorRecord { get; set; }
        public virtual string Classification { get; set; }
        public virtual string ClassificationSecond { get; set; }
        public virtual string NoteSecond { get; set; }
        public virtual string ClassificationThird { get; set; }
        public virtual string NoteThird { get; set; }
        public virtual DateTime? DateRated { get; set; }
        public virtual DateTime? DateSupervisorRated { get; set; }
        public virtual ABC_Title ABC_Title { get; set; }
        public virtual ABC_Title ABC_TitleSecond { get; set; }
    }
}
