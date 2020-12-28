using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_TieuChi
    {
        public virtual Guid Id { get; set; }
        public virtual string STT { get; set; }
        public virtual float? STTSapXep { get; set; }
        public virtual string NoiDung { get; set; }
        public virtual float? DiemToiDa { get; set; }
        public virtual int? HeSoTieuChiCon { get; set; }
        public virtual bool? DiemTru { get; set; }
        public virtual bool? IsAutoScore { get; set; }
        public virtual bool? IsTeacher { get; set; }
        public virtual int? DieuKienDiemNhanVien { get; set; }
        public virtual int? DieuKienThoiGian { get; set; }
        public virtual int? DieuKienLoaiDiem { get; set; }
        public virtual string DieuKienListThoiGian { get; set; }
        public virtual string CongThucTinhDiem { get; set; }
        public virtual string CongThucTinhDiemTeacher { get; set; }
        public virtual int? GCRecord { get; set; }  
        public virtual DateTime? TimeDelete { get; set; }
        public virtual Guid? UserDeleteId { get; set; }
        public virtual DateTime? AddTime { get; set; }
        public virtual Guid? AddUserId { get; set; }
        public virtual DateTime? LastEditTime { get; set; }
        public virtual Guid? LastEditUserId { get; set; }
        public virtual ABC_BoTieuChi BoTieuChi { get; set; }
        public virtual ABC_TieuChi Parent { get; set; } 
        public virtual IList<ABC_TieuChi> Childrens { get; set; } 
    }
}
