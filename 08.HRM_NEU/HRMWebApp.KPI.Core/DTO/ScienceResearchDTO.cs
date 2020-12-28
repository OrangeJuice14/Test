using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ScienceResearchDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PlanKPIDetailId { get; set; }
        public Guid CriterionDictionaryId { get; set; }
        public CriterionDictionaryDTO CriterionDictionary { get; set; }
        public int NumberOfResearch { get; set; }
        public int OrderNumber { get; set; }
        public int IsRating { get; set; }
    }
}
