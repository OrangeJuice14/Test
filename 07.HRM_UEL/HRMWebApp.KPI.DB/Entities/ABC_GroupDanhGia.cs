using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_GroupDanhGia
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual long? GCRecord { get; set; }
        public virtual DateTime? DeleteTime { get; set; }
        public virtual Guid? DeleteUserId { get; set; }
        public virtual DateTime? AddTime { get; set; }
        public virtual Guid? AddUserId { get; set; }
        public virtual DateTime? LastEditTime { get; set; }
        public virtual Guid? LastEditUserId { get; set; }
        public virtual int? STT { get; set; }
        public virtual bool? DaiDienDanhGia { get; set; }
        public virtual bool? TuDanhGia { get; set; }
        public virtual bool? DanhGiaCapDuoi { get; set; }
        public virtual bool? HasQuanLyDonVi { get; set; }
        public virtual bool? HasDieuKienPhu { get; set; }
        public virtual int? SoLuongGiangVien { get; set; }
        public virtual int? SoLuongSinhVien { get; set; }
        public virtual int? HeSoNgachDamNhiem { get; set; }
        public virtual int? HeSoQuanLy { get; set; }
        public virtual int? HeSoNgachDamNhiemHasDieuKien { get; set; }
        public virtual int? HeSoQuanLyHasDieuKien { get; set; }
    }
}
