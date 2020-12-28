using HRMWebApp.KPI.Core.DTO.ABC.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_DanhGiaVMDTO
    {
        public Guid Id { get; set; }
        public DateTime? ThoiGianDanhGia { get; set; }
        public bool? IsLock { get; set; }
        public float? TongDiem { get; set; }
        public Guid UserDanhGiaId { get; set; }
        public Guid UserDuocDanhGiaId { get; set; }
        public Guid? KyDanhGiaId { get; set; }
        public Guid? GroupUserDanhGiaId { get; set; }
        public Guid? BoTieuChiId { get; set; }
        public string GroupUserDanhGiaName { get; set; }
        public ABC_BoTieuChiVMDTO BoTieuChi { get; set; }
        public List<ABC_DanhGiaChiTietVMDTO> DanhGiaChiTiet { get; set; } 
        public string LoaiDanhGia { get; set; }
        public string LoaiTuDanhGia { get; set; }
    }
}
