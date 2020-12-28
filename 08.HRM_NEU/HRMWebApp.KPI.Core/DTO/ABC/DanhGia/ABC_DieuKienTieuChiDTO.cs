using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_DieuKienTieuChiDTO
    {
        public Guid Id { get; set; }
        public Guid TieuChiId { get; set; }
        public Guid DieuKienDiemTieuChiId { get; set; }
        public Guid DieuKienDiemBoTieuChiId { get; set; }
    }
}
