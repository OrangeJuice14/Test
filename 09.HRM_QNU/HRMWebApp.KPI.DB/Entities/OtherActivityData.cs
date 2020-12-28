using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
   public class OtherActivityData
    {
        public virtual Guid Id { get; set; }
        public virtual string StaffCode { get; set; }
        public virtual StaffProfile StaffProfile { get; set; }
        public virtual Department Department { get; set; }
        public virtual string StudyTerm { get; set; }
        public virtual string StudyYear { get; set; }
        public virtual double NumberOfTime { get; set; }
        public virtual string Name { get; set; }
        public virtual string ManageCode { get; set; }
        public virtual string ActivityManageCode { get; set; }
        public virtual int OrderNumber { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int TotalParticipants { get; set; }
        public virtual decimal Coefficient { get; set; }
        public virtual string ExcelManageCode { get; set; }
        public virtual decimal NumberOfHour { get; set; }
    }
}
