using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
   public class Method_Department
    {
        public virtual Guid Id { get; set; }
        public virtual Method MethodId { get; set; }
        public virtual Department DepartmentId { get; set; }
        public virtual int DiemSo { get; set; }
        public virtual int Diem1 { get; set; }
        public virtual int Diem2 { get; set; }
        public virtual int Diem3 { get; set; }
        public virtual int Diem4 { get; set; }
    }
}
