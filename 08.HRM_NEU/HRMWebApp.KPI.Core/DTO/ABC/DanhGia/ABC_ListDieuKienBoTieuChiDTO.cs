using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_ListDieuKienBoTieuChiDTO
    {
        public Guid BoTieuChiId { get; set; }
        public List<Guid> ListHoanThanhBoTieuChiId { get; set; }
    }
}
