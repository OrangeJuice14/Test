using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_RatingDTO
    {
        public ABC_RatingDTO(){
            ABC_RatingGroupDTOs = new List<ABC_RatingGroupDTO>();
            ABC_RatingGroupPropertyDTOs = new List<ABC_RatingGroupDTO>();
            ABC_RatingGroupSpecialDTOs = new List<ABC_RatingGroupDTO>();
            ABC_EvaluationBoard = new ABC_EvaluationBoardDTO();
        }
        public Guid Id { get; set; }
        public bool IsRated { get; set; }
        public bool IsSupervisorRated { get; set; }
        public bool IsRatingLocked { get; set; }
        public bool IsSupervisor { get; set; }
        public byte IsAdmin { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public Staff Staff { get; set; }
        public ABC_EvaluationBoardDTO ABC_EvaluationBoard { get; set; }
        public List<ABC_RatingGroupDTO> ABC_RatingGroupDTOs { get; set; }
        /// <summary>
        /// Điểm phạt, điểm thưởng
        /// </summary>
        public List<ABC_RatingGroupDTO> ABC_RatingGroupPropertyDTOs { get; set; }
        /// <summary>
        /// ĐÁNH GIÁ ĐỐI VỚI VIÊN CHỨC QUẢN LÝ, NGƯỜI LAO ĐỘNG, GIẢNG VIÊN
        /// </summary>
        public List<ABC_RatingGroupDTO> ABC_RatingGroupSpecialDTOs { get; set; }
        public Guid StaffId { get; set; }
        public string StaffName { get; set; }
        public string StaffPosition { get; set; }
        public Guid WebGroupId { get; set; }
        public string DepartmentName { get; set; }
        public decimal TotalStaffRecord { get; set; }
        public decimal TotalSupervisorRecord { get; set; }
        public bool IsEligible { get; set; }
        public bool IsValid { get; set; }
        public int EvaluationBoardType { get; set; }
        public int StaffType { get; set; }
        public List<ABC_RatingTypeDTO> ABC_RatingTypeDTOs { get; set; }
        public string Classification { get; set; }
        public string ClassificationSecond { get; set; }
        public string NoteSecond { get; set; }
        public string ClassificationThird { get; set; }
        public string NoteThird { get; set; }
        public Guid? ABC_TitleId { get; set; }
        public string ABC_TitleName { get; set; }
        public Guid? ABC_TitleSecondId { get; set; }
        public string ABC_TitleSecondName { get; set; }
    }
}
