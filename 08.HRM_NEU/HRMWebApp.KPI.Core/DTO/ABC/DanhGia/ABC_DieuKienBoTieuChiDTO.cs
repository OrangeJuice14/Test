using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_DieuKienBoTieuChiDTO
    {
        public Guid? Id { get; set; }
        public Guid? BoTieuChiId { get; set; }
        public int? BoTieuChiLoaiBoTieuChi { get; set; }
        public Guid? HoanThanhBoTieuChiId { get; set; }
        public int? HoanThanhBoTieuChiLoaiBoTieuChi { get; set; }
        public int? LoaiHoanThanh { get; set; }
    }
}
