using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class OtherActivityDataDTO
    {
        public Guid Id { get; set; }
        public string StaffCode { get; set; }
        public Guid StaffId { get; set; }
        public string ManageCode { get; set; }
        public string ActivityManageCode { get; set; }
        public string StudyTerm { get; set; }
        public string StudyYear { get; set; }
        public double NumberOfTime { get; set; }
        public string Name { get; set; }
    }
}
