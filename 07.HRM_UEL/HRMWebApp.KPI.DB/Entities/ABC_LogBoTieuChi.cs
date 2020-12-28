using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_LogBoTieuChi
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime? TuNgay { get; set; }
        public virtual DateTime? DenNgay { get; set; }
        public virtual bool? Status { get; set; }
        public virtual bool? ShowTen { get; set; }
        public virtual bool? ShowDonVi { get; set; }
        public virtual long? GCRecord { get; set; }
        public virtual DateTime? DeleteTime { get; set; }
        public virtual Guid? DeleteUserId { get; set; }
        public virtual Guid? LastEditUserId { get; set; }
        public virtual DateTime? LastEditTime { get; set; }
        public virtual Guid? AddUserId { get; set; }
        public virtual DateTime? AddTime { get; set; }
        public virtual DateTime? TimeLog { get; set; }
        public virtual Guid? BoTieuChiId { get; set; }
        public virtual int? LoaiBoTieuChi { get; set; }// Loai 12: Nam
                                                       // Loai 6: 6 tháng
                                                       // Loai 3: Quy'
                                                       // Loai 1: Thang'
    }
}
