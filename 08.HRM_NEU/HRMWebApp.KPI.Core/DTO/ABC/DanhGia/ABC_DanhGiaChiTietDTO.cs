using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_DanhGiaChiTietDTO
    {
        public Guid Id { get; set; }
        public float? Diem { get; set; }
        public Guid? DanhGiaId { get; set; }
        public Guid? TieuChiId { get; set; }
        public string TieuChiSTT { get; set; }
        public string TieuChiNoiDung { get; set; }
        public string TieuChiCongThucTinhDiem { get; set; }
        public string TieuChiCongThucTinhDiemTeacher { get; set; }
        public bool? TieuChiIsAutoScore { get; set; }
        public bool? TieuChiIsTeacher { get; set; }
        public int? TieuChiDieuKienDiemNhanVien { get; set; }
        public int? TieuChiDieuKienThoiGian { get; set; }
        public int? TieuChiDieuKienLoaiDiem { get; set; }
        public string TieuChiDieuKienListThoiGian { get; set; }
        public int? TieuChiDiemToiDa { get; set; }
        public int? TieuChiHeSoTieuChiCon { get; set; }
        public bool? TieuChiDiemTru { get; set; }
        public Guid? TieuChiParentId { get; set; }
        public int? TieuChiParentDiemToiDa { get; set; }
        public bool NoChild { get; set; } 
    }
}
