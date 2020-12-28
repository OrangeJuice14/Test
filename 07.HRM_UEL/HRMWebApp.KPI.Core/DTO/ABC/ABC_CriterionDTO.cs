using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_CriterionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CriterionType { get; set; }
        public int OrderNumber { get; set; }
        public int IsNotVisibleInEvaluationBoardType { get; set; }
        public ABC_RatingTypeDTO ABC_RatingTypeDTO { get; set; }
        public List<Guid> SelectRatingTypes { get; set; }
        public Guid CopyFromCriterion { get; set; }
        public ABC_RatingTypeDTO CopyFromRatingType { get; set; }
        public List<Guid> CopyToRatingTypes { get; set; }
        public List<Guid> CriterionCopyIds { get; set; }
        public bool IsGroupByRatingTypeOnEvaluationBoard { get; set; }
        public bool @checked { get; set; }
    }
}
