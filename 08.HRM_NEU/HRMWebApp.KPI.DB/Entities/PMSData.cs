using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
   public class PMSData
    {
        public virtual Guid Id { get; set; }
        public virtual Guid StaffId { get; set; }
        public virtual double NumberOfLesson { get; set; }
        public virtual double PercentShortage { get; set; }
        public virtual string StudyTerm { get; set; }
        public virtual string StudyYear { get; set; }


    }
}
