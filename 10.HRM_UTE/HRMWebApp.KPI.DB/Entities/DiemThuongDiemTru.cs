using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
   public class DiemThuongDiemTru
    {
        public virtual Guid Id { get; set; }
        public virtual string NoiDung { get; set; }
        public virtual Department DonViCungCap { get; set; }
        public virtual string MaHoatDong { get; set; }
        public virtual string MaNhomHoatDong { get; set; }
        public virtual string NamHoc { get; set; }
        public virtual string HocKy { get; set; }
        public virtual DateTime? Date { get; set; }
        public virtual string UserNhap { get; set; }
        public virtual string NoiDungHoatDong { get; set; }
        public virtual double SoDiem { get; set; }
        public virtual string MaCanBo { get; set; }
        public virtual string TenCanBo { get; set; }
        public virtual Department Khoa { get; set; }
        public virtual string DonViCungCapName { get; set; }
    }
}
