using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ProfessorOtherActivityDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PlanKPIDetailId { get; set; }
        public Guid CriterionDictionaryId { get; set; }
        public CriterionDictionaryDTO CriterionDictionary { get; set; }
        public double NumberOfTime { get; set; }
        public double NumberOfTimeDouble { get; set; }
        public double? NumberOfHour { get; set; }
        public int OrderNumber { get; set; }
        public int IsRating { get; set; }
        public bool IsAssign { get; set; }
        public string ExecuteMethod { get; set; }
        public string BasicResource { get; set; }
        public string ManageCode { get; set; }
        public double SupervisorRecord { get; set; }
    }
}
