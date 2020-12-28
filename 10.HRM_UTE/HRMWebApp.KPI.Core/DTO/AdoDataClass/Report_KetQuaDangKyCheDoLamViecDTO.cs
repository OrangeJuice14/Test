using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.AdoDataClass
{
    public class Report_KetQuaDangKyCheDoLamViecDTO
    {
        public Guid PlanStaffId { get; set; }
        public Guid PlanKPIId { get; set; }
        public Guid StaffId { get; set; }
        public string MaCBVC { get; set; }
        public string HoTen { get; set; }
        public Guid BoMonId { get; set; }
        public string BoMon { get; set; }
        public string Khoa { get; set; }
        public Guid KhoaId { get; set; }
        public Guid CheDoDangKyId { get; set; }
        public string CheDoDangKy { get; set; }
        public string HocHam { get; set; }
        public string HocVi { get; set; }
        public Boolean Duyet { get; set; }
        public DateTime Time { get; set; }
    }
}
