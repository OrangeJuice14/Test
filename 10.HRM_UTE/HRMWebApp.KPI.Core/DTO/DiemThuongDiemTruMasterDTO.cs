using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
   public class DiemThuongDiemTruMasterDTO
    {
        public  Guid Id { get; set; }
        public  string NoiDung { get; set; }
        public  double SoDiem { get; set; }
        public  string MaCanBo { get; set; }
        public  string TenCanBo { get; set; }
        public  Department Khoa { get; set; }
        public string KhoaName { get; set; }
    }
}
