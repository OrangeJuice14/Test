using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
   public class OtherActivityDataDetailDTO
    {
        public Guid Id { get; set; }
        public string StaffCode { get; set; }
        public string StaffName { get; set; }
        public double NumberOfTime { get; set; }
        public Guid IdCanBo { get; set; }
    }
}
