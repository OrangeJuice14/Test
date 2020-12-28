using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_XepLoaiDanhGia
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual float? TuDiem { get; set; }
        public virtual float? DenDiem { get; set; }
        public virtual bool? HasDieuKienPhu { get; set; }
        public virtual bool? HasDieuKienTieuChi { get; set; }
        public virtual int? DiemDat { get; set; }
        public virtual long? GCRecord { get; set; }
        public virtual Guid? UserDeleteId { get; set; }
        public virtual ABC_BoTieuChi DieuKienBoTieuChi { get; set; }
        public virtual ABC_BoTieuChi BoTieuChi { get; set; }
    }
}
