using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_TieuChiDTO
    {
        public  Guid Id { get; set; }
        public string STT { get; set; }
        public float? STTSapXep { get; set; }
        public string NoiDung { get; set; }
        public float? DiemToiDa { get; set; }
        public int? HeSoTieuChiCon { get; set; }
        public bool? DiemTru { get; set; }
        public bool? IsAutoScore { get; set; }
        public bool? IsTeacher { get; set; }
        public string CongThucTinhDiemTeacher { get; set; }
        public int? DieuKienDiemNhanVien { get; set; }
        public int? DieuKienLoaiDiem { get; set; }
        public int? DieuKienThoiGian { get; set; }
        public string DieuKienListThoiGian { get; set; }
        public string CongThucTinhDiem { get; set; }
        public int? GCRecord { get; set; }
        public DateTime? TimeDelete { get; set; }
        public Guid? UserDeleteId { get; set; }
        public Guid? BoTieuChiId { get; set; }
        public Guid? ParentId { get; set; }
        public DateTime? AddTime { get; set; }
        public Guid? AddUserId { get; set; }
        public DateTime? LastEditTime { get; set; }
        public Guid? LastEditUserId { get; set; }
        //public IList<ABC_TieuChiDTO> Childrens { get; set; }
    }
}
