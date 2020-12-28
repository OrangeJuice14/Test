using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class MethodDetailDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Method Method { get; set; }
        public Guid MethodId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
