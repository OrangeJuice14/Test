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
        public virtual DateTime? TimeDelete { get; set; }
        public virtual Guid? UserDeleteId { get; set; }
        public virtual DateTime? AddTime { get; set; }
        public virtual Guid? AddUserId { get; set; }
        public virtual DateTime? LastEditTime { get; set; }
        public virtual Guid? LastEditUserId { get; set; }
        public virtual int? STT { get; set; }
        public virtual bool? DaiDienDanhGia { get; set; }
        public virtual bool? TuDanhGia { get; set; }
        public virtual bool? DanhGiaCapDuoi { get; set; }
        public virtual bool? HasQuanLyDonVi { get; set; }
    }
}
