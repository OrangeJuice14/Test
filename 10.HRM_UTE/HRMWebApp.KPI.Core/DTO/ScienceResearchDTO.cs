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
        public double NumberOfResearch { get; set; }
        public int OrderNumber { get; set; }
        public int IsRating { get; set; }
        public string ExecuteMethod { get; set; }
        public string BasicResource { get; set; }
        /// <summary>
        /// số tiết
        /// </summary>
        public double? NumberOfHour { get; set; }
    }
}
