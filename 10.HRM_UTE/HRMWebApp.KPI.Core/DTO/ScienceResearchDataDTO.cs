using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ScienceResearchDataDTO
    {
        public Guid Id { get; set; }
        public string StaffCode { get; set; }
        public string ManageCode { get; set; }
        public string StudyTerm { get; set; }
        public string StudyYear { get; set; }
        public double Record { get; set; }
        public string Name { get; set; }
        public double NumberOfTime { get; set; }
    }
}
