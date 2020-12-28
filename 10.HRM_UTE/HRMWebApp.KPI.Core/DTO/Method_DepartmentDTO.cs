using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
   public class Method_DepartmentDTO
    {
        public Guid Id { get; set; }
        public DepartmentDTO DepartmentId { get; set; }
        public MethodDTO MethodId { get; set; }
        public int DiemSo { get; set; }
        public int Diem1 { get; set; }
        public int Diem2 { get; set; }
        public int Diem3 { get; set; }
        public int Diem4 { get; set; }
    }
}
