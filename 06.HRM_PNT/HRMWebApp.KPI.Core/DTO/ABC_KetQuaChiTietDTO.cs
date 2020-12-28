using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_KetQuaChiTietDTO
    {
        public Guid Id { get; set; }
        public int? Diem { get; set; }
        public string GhiChu { get; set; }
        public string YKienDanhGia { get; set; }
        public bool? IsChecked { get; set; }
        public Guid ChiTietNhanVienDanhGiaId { get; set; }
        public Guid TieuChiDanhGiaId { get; set; }
        public string TieuChiDanhGiaTenTieuChi { get; set; }
        public string TieuChiDanhGiaSTT { get; set; }
        public int TieuChiDanhGiaDiemToiDa { get; set; }
        public bool TieuChiDanhGiaChildSelectOne { get; set; }
        public Guid TieuChiDanhGiaParentId { get; set; }
        public string TieuChiDanhGiaParentSTT { get; set; }
        public bool TieuChiDanhGiaParentChildSelectOne { get; set; }
        //public Guid ParentId { get; set; }
        //public Guid? ParentIsChecked { get; set; }
    }
}
