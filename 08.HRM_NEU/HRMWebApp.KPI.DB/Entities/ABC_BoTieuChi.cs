using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_BoTieuChi
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime? TuNgay { get; set; }
        public virtual DateTime? DenNgay { get; set; }
        public virtual bool? Status { get; set; }
        public virtual long? GCRecord { get; set; }
        public virtual DateTime? TimeDelete { get; set; }
        public virtual Guid? UserDeleteId { get; set; }
        public virtual bool? HasDieuKienDanhGia { get; set; }
        public virtual bool? ShowTen { get; set; }
        public virtual bool? ShowDonVi { get; set; }
        public virtual bool? ShowBoPhan { get; set; }
        public virtual bool? ShowDay { get; set; }
        public virtual bool? ShowMonth { get; set; }
        public virtual bool? ShowYear { get; set; }
        public virtual Guid? LastEditUserId { get; set; }
        public virtual Guid? AddUserId { get; set; }
        public virtual DateTime? EditLastTime { get; set; }
        public virtual DateTime? AddTime { get; set; }
        public virtual int? LoaiBoTieuChi { get; set; }// Loai 0: Nam
                                                       // Loai 1: 6 thang
                                                       // Loai 2: Quy'
                                                       // Loai 3: Thang'
        public virtual IList<ABC_TieuChi> TieuChis { get; set; } 

    }
}
