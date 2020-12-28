using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
   public class DiemThuongDiemTruDTO
    {
        public  Guid Id { get; set; }
        public string NoiDung { get; set; }
        public Department DonViCungCap { get; set; }
        public string MaHoatDong { get; set; }
        public string MaNhomHoatDong { get; set; }
        public string NamHoc { get; set; }
        public string HocKy { get; set; }
        public DateTime? Date { get; set; }
        public string UserNhap { get; set; }
        public string NoiDungHoatDong { get; set; }
        public double SoDiem { get; set; }
        public string MaCanBo { get; set; }
        public string TenCanBo { get; set; }
        public Department Khoa { get; set; }
        public string DonViCungCapName { get; set; }
        public ProfessorCriterion ProfessorCriterion { get; set; }
    }
}
