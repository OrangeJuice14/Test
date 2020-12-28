using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class StaffLevel //Trình độ
    {
        public virtual Guid Id { get; set; }
        public virtual Qualification Qualification { get; set; }
        public virtual AcademicTitle AcademicTitle { get; set; }
    }
}
