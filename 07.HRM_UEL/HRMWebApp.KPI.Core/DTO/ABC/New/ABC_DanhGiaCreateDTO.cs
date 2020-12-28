using HRMWebApp.KPI.Core.DTO.ABC.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_DanhGiaCreateDTO
    {
        public Guid Id { get; set; }
        public bool? IsLock { get; set; }
        public float? TongDiem { get; set; }
        public Guid UserDanhGiaId { get; set; }
        public Guid UserDuocDanhGiaId { get; set; }
        public Guid? KyDanhGiaId { get; set; }
        public Guid? GroupUserDanhGiaId { get; set; }
        public Guid? BoTieuChiId { get; set; }
        public ABC_BoTieuChiCreateDTO BoTieuChi { get; set; }
        public List<ABC_DanhGiaChiTietCreateDTO> DanhGiaChiTiet { get; set; } 
        public string LoaiDanhGia { get; set; }
        public string LoaiTuDanhGia { get; set; }
    }
}
