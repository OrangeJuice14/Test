using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
   public class DoDuLieuDanhGia
    {
        public virtual Guid Id { get; set; }
        public virtual StaffInfo IdNhanVien { get; set; }
        public virtual string MaCanBo { get; set; }
        public virtual string TenCanBo { get; set; }
        public virtual string NamHoc { get; set; }
        public virtual string HocKy { get; set; }
        public virtual DateTime? CreationTime { get; set; }
        public virtual string MaTieuChi { get; set; }
        public virtual string LoaiGiaTri { get; set; }
        public virtual string DatMuc { get; set; }
        public virtual double GiaTriThuc { get; set; }
        public virtual string DonViCungCap { get; set; }
        public virtual CriterionDictionary CriterionDictionaryId { get; set; }
        public virtual FileGiangDay Path { get; set; }
        public virtual string DuLieuMinhChung { get; set; }
        public virtual string GhiChu { get; set; }
        public virtual Department DonVi { get; set; }

    }
}
