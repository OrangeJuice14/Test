using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_RatingGroupDTO
    {
        public ABC_RatingGroupDTO(){
            ABC_RatingDetailDTOs = new List<ABC_RatingDetailDTO>();
            ABC_Criterion = new ABC_CriterionDTO();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public int ABC_RatingGroupType { get; set; }

        /// <summary>
        /// Loại kỳ đánh giá nào sẽ ko hiện nhóm tiêu chí này
        /// </summary>
        public int IsNotVisibleInEvaluationBoardType { get; set; }
        public ABC_CriterionDTO ABC_Criterion { get; set; }
        public Guid ABC_CriterionId { get; set; }
        public List<ABC_RatingDetailDTO> ABC_RatingDetailDTOs { get; set; }
        public bool IsCheckBoxGroup { get; set; }

    }
}
