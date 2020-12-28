using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.Core.DTO;

namespace HRMWebApp.KPI.Core.DTO.RatingKPIDTOs
{
    public class TargetGroupRatingDTO
    {
        public TargetGroupRatingDTO() {
            ResultDetailDTOs = new List<ResultDetailRatingDTO>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Density { get; set; }
        public int TargetGroupDetailTypeId { get; set; }
        public List<CriterionDictionaryDTO> CriterionDictionaries { get; set; }
        public List<ResultDetailRatingDTO> ResultDetailDTOs { get; set; }
    }
}
