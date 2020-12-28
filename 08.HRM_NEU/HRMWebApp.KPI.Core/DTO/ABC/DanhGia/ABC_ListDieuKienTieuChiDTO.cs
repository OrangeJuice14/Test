using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_ListDieuKienTieuChiDTO
    {
        public ABC_TieuChiDTO TieuChi { get; set; }
        public ABC_BoTieuChiDTO DieuKienDiemBoTieuChi { get; set; }
        public List<Guid> ListDieuKienDiemTieuChiId { get; set; }
    }
}
