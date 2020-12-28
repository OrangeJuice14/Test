using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class AgentObject_Salary
    {
        public virtual Guid Id { get; set; }
        public virtual AgentObject AgentObject { get; set; }
        public virtual ScaleSalary ScaleSalary { get; set; } //Ngạch lương
        public virtual Qualification Qualification { get; set; } //Trình độ chuyên môn
        public virtual AcademicTitle AcademicTitle { get; set; } //Học hàm
    }
}
