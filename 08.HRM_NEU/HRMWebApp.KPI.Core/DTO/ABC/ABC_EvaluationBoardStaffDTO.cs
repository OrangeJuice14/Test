using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_EvaluationBoardStaffDTO
    {
        public Guid EvaluationId { get; set; }
        public string StaffName { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public Guid StaffId { get; set; }
        public Guid DepartmentId { get; set; }
        public bool IsRated { get; set; }
        public bool IsSupervisorRated { get; set; }
        public string IsRatedString { get; set; }
        public string IsSupervisorRatedString { get; set; }
        public string Record { get; set; }
        public string StaffRecord { get; set; }
        public string Classification { get; set; }
        public int StaffType { get; set; }
        public int OrderNumber { get; set; }
        public int EvaluationBoardType { get; set; }     
        public Guid RatingId { get; set; }
        public string ClassificationSecond { get; set; }
        public string NoteSecond { get; set; }
        public string ClassificationThird { get; set; }
        public string NoteThird { get; set; }
    }
}
