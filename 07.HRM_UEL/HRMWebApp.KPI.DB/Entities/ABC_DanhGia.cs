using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_DanhGia
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime? ThoiGianDanhGia { get; set; }
        public virtual bool? IsLock { get; set; }
        public virtual float? TongDiem { get; set; }
        public virtual WebUser UserDanhGia { get; set; }
        public virtual WebUser UserDuocDanhGia { get; set; }
        public virtual ABC_KyDanhGia KyDanhGia { get; set; }
        public virtual ABC_BoTieuChi BoTieuChi { get; set; }
        public virtual ABC_GroupDanhGia GroupUserDanhGia { get; set; }
    }
}
