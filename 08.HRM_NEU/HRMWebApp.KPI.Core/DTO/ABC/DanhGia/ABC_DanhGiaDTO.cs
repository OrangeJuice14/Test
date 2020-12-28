using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_DanhGiaDTO
    {
        public Guid Id { get; set; }
        public DateTime? ThoiGianDanhGia { get; set; }
        public bool? IsLock { get; set; }
        public float? TongDiem { get; set; }
        public Guid? UserDanhGiaId { get; set; }
        public Guid? UserDuocDanhGiaId { get; set; }
        public Guid? KyDanhGiaId { get; set; }
        public Guid? UserDanhGia_GroupId { get; set; }
        public Guid? BoTieuChiId { get; set; }
        public string UserDanhGia_GroupName { get; set; }
        public ABC_BoTieuChiDTO BoTieuChi { get; set; }
        public List<ABC_DanhGiaChiTietDTO> DanhGiaChiTiet { get; set; } 
        public string LoaiDanhGia { get; set; }
        public string LoaiTuDanhGia { get; set; }
    }
}
