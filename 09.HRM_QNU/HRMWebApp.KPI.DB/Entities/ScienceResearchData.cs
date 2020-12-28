using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
   public class ScienceResearchData
    {
        public virtual Guid Id { get; set; }
        public virtual string StaffCode { get; set; }
        public virtual string StudyTerm { get; set; }
        public virtual string StudyYear { get; set; }
        public virtual double Record { get; set; }
        public virtual string Name { get; set; }
        public virtual string ManageCode { get; set; }
    }
}
