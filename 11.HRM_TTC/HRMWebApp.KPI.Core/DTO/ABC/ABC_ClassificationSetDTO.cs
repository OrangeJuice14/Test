using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_ClassificationSetDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? ThoiGianApDung { get; set; }
    }
}
