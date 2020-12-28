using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_DieuKienBoTieuChi
    {
        public virtual Guid Id { get; set; }
        public virtual ABC_BoTieuChi BoTieuChi { get; set; }
        public virtual ABC_BoTieuChi HoanThanhBoTieuChi { get; set; }
        public virtual int? LoaiHoanThanh { get; set; }
    }
}
