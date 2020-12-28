using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_LogTieuChi
    {
        public virtual Guid Id { get; set; }
        public virtual Guid? ParentId { get; set; }
        public virtual Guid? BoTieuChiId { get; set; }
        public virtual Guid? TieuChiId { get; set; }
        public virtual string ChiMuc { get; set; }
        public virtual float? STT { get; set; }
        public virtual string NoiDung { get; set; }
        public virtual string ListDiem { get; set; }
        public virtual float? DiemToiDa { get; set; }
        public virtual bool? DiemTru { get; set; }
        public virtual bool? ChildSelectOne { get; set; }
        public virtual long? GCRecord { get; set; }
        public virtual bool? IsDiemDanhGiaCongTac { get; set; }
        public virtual bool? IsDiemThuong { get; set; }
        public virtual DateTime? DeleteTime { get; set; }
        public virtual DateTime? TimeLog { get; set; }
        public virtual Guid? DeleteUserId  { get; set; }
        public virtual DateTime? LastEditTime  { get; set; }
        public virtual Guid? LastEditUserId { get; set; }
        public virtual DateTime? AddTime { get; set; }
        public virtual Guid? AddUserId { get; set; }
    }
}
