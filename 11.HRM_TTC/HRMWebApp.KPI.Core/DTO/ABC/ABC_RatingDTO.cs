using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.Core.DTO.ABC;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_RatingDTO
    {
        public ABC_RatingDTO(Guid staffId)
        {
            ABC_EvaluationBoardDTO = new ABC_EvaluationBoardDTO();
            ABC_RatingDetailDTOs = new List<ABC_RatingDetailDTO>();
            StaffInfo = DataClassHelper.GetThongTinCaNhan(staffId);
        }

        public ABC_RatingDTO()
        {
            ABC_EvaluationBoardDTO = new ABC_EvaluationBoardDTO();
            ABC_RatingDetailDTOs = new List<ABC_RatingDetailDTO>();
        }

        public Guid Id { get; set; }
        public bool IsRated { get; set; }
        public bool IsSupervisorRated { get; set; }
        public bool IsRatingLocked { get; set; }
        public bool IsSupervisor { get; set; }
        public byte IsAdmin { get; set; }
        public int Quater { get; set; }
        public string Year { get; set; }
        public Staff Staff { get; set; }
        public ABC_EvaluationBoardDTO ABC_EvaluationBoardDTO { get; set; }
        public Guid StaffId { get; set; }
        public Guid StaffRatingId { get; set; }
        public Guid WebGroupId { get; set; }
        public bool IsEligible { get; set; }
        public bool IsValid { get; set; }
        public int StaffType { get; set; }
        public Dictionary<string, object> StaffInfo { get; set; }
        public string StaffNote { get; set; }
        public string SupervisorNote { get; set; }
        public List<ABC_RatingDetailDTO> ABC_RatingDetailDTOs { get; set; }
        public List<ABC_RatingLevelDTO> ABC_RatingLevelDTOs { get; set; }
    }
}
