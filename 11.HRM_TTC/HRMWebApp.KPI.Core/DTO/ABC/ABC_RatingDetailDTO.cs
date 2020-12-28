using HRMWebApp.KPI.Core.DTO.ABC;
using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_RatingDetailDTO
    {
        public ABC_RatingDetailDTO()
        {
            ABC_CriterionDTO = new ABC_CriterionDTO();
        }
        public Guid Id { get; set; }
        public Guid StaffRecord { get; set; }
        public Guid SupervisorRecord { get; set; }
        public int IsNotVisibleInEvaluationBoard { get; set; }
        public ABC_RatingDTO ABC_RatingDTO { get; set; }
        public ABC_CriterionDTO ABC_CriterionDTO { get; set; }
        public List<ABC_RatingLevelDTO> RatingLevelDTOs { get; set; }
    }   
}
