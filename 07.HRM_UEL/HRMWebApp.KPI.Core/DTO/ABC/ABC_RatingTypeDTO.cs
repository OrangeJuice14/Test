using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_RatingTypeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public bool IsGroupOnEvaluationBoard { get; set; }
        public List<Guid> RatingTypeIncludedIds { get; set; }
        public List<Guid> CriterionIds { get; set; }
    }
}
