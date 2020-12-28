using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_ClassRatingDTO
    {
        public Guid Id { get; set; }
        public bool IsRated { get; set; }
        public bool IsRatedSecond { get; set; }
        public bool IsRatedThird { get; set; }
        public bool IsRatingLocked { get; set; }
        public bool IsSupervisor { get; set; }
        public byte SupervisorType { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public Guid ABC_ClassEvaluationBoardId { get; set; }
        public string ABC_ClassEvaluationBoardName { get; set; }
        public Guid StaffId { get; set; }
        public string StaffName { get; set; }
        public string StaffPosition { get; set; }
        public Guid WebGroupId { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool IsValid { get; set; }
        public int StaffType { get; set; }
        public string Classification { get; set; }
        public string ClassificationSecond { get; set; }
        public string NoteSecond { get; set; }
        public string ClassificationThird { get; set; }
        public string NoteThird { get; set; }
    }
}
