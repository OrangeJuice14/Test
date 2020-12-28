using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class OtherActivityDataDTO
    {
        public Guid Id { get; set; }
        public string StaffCode { get; set; }
        public string StaffId { get; set; }
        //public List<Guid> StaffIds { get; set; }
        public string ManageCode { get; set; }
        public string ManageName { get; set; }
        public string ActivityManageCode { get; set; }
        public string StudyTerm { get; set; }
        public string StudyYear { get; set; }
        public double NumberOfTime { get; set; }
        public string Name { get; set; } 
        public string StaffName { get; set; }
        public string DepartmentName { get; set; }
        public Guid DepartmentId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public int TotalParticipants { get; set; }
        public decimal Coefficient { get; set; }
        public string ExcelManageCode { get; set; }
        public decimal NumberOfHour { get; set; }
        public string IdUserNhap { get; set; }
        public string UserNhap { get; set; }
        public Guid DonViCungCapId { get; set; }
        public string DonViCungCapName { get; set; }
        public Guid IdCanBo { get; set; }
        public int TargetGroupdetailType { get; set; }
        public Guid NhomMucTieuId { get; set; }
    }
}
