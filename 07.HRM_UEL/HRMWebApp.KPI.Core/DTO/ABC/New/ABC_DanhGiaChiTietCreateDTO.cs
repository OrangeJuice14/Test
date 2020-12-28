using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_DanhGiaChiTietCreateDTO
    {
        public Guid Id { get; set; }
        public float? Diem { get; set; }
        public Guid? DanhGiaId { get; set; }
        public Guid? TieuChiId { get; set; }
    }
}
