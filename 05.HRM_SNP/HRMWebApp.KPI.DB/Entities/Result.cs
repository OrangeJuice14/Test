using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class Result
    {
        public virtual Guid Id { get; set; }
        public virtual Staff StaffRated { get; set; }
        public virtual Staff StaffRating { get; set; }
        public virtual double TotalRecord { get; set; }
        public virtual double TotalRecordSecond { get; set; }
        public virtual bool IsLocked { get; set; }
        public virtual bool IsUnlocked { get; set; }
        public virtual DateTime Time { get; set; }
        public virtual bool IsCommitted { get; set; }
        public virtual bool IsUnlockedForRating { get; set; }
        public virtual PlanStaff PlanStaff { get; set; }
        public virtual int NumberOfEditing { get; set; }
    }
}
