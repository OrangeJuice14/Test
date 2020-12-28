using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_GroupDanhGiaDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long? GCRecord { get; set; }
        public DateTime? TimeDelete { get; set; }
        public Guid? UserDeleteId { get; set; }
        public DateTime? AddTime { get; set; }
        public Guid? AddUserId { get; set; }
        public DateTime? LastEditTime { get; set; }
        public Guid? LastEditUserId { get; set; }
        public int? STT { get; set; }
        public bool? DaiDienDanhGia { get; set; }
        public bool? TuDanhGia { get; set; }
        public bool? DanhGiaCapDuoi { get; set; }
        public bool? HasQuanLyDonVi { get; set; }
        public List<ABC_BoTieuChiDTO> ListBoTieuChiTuDanhGia { get; set; } 
        public List<ABC_BoTieuChiDTO> ListBoTieuChiDanhGiaCapDuoi { get; set; } 
    }
}
