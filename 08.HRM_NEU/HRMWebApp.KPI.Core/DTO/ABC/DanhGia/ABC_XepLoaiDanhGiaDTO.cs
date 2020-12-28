using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_XepLoaiDanhGiaDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float? TuDiem { get; set; }
        public float? DenDiem { get; set; }
        public bool? HasDieuKienPhu { get; set; }
        public bool? HasDieuKienTieuChi { get; set; }
        public int? DiemDat { get; set; }
        public long? GCRecord { get; set; }
        public Guid? UserDeleteId { get; set; }
        public Guid? DieuKienBoTieuChiId { get; set; }
        public Guid? BoTieuChiId { get; set; }
    }
}
