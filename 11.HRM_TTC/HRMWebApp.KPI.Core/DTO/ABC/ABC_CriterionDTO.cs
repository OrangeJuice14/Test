using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO.ABC;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_CriterionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CriterionDetail { get; set; }
        public string Methods { get; set; }
        public string Percents { get; set; }
        public int OrderNumber { get; set; }
        public int IsNotVisibleInEvaluationBoardType { get; set; }
        public bool IsTemp { get; set; }
        public List<Guid> RatingTypeIds { get; set; }
        public bool @checked { get; set; }
    }
}
