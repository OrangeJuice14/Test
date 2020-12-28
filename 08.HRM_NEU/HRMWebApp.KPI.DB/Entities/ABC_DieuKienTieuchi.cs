using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_DieuKienTieuChi
    {
        public virtual Guid Id { get; set; }
        public virtual ABC_TieuChi TieuChi { get; set; }
        public virtual ABC_TieuChi DieuKienDiemTieuChi { get; set; }
        public virtual ABC_BoTieuChi DieuKienDiemBoTieuChi { get; set; }
    }
}
