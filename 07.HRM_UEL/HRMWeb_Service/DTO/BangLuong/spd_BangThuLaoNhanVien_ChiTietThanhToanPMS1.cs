using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWeb_Service.DTO.BangLuong
{
    public class spd_BangThuLaoNhanVien_ChiTietThanhToanPMS1
    {
        public string KhoanChi { get; set; }
        public string TenMonHoc { get; set; }
        public string LopHocPhan { get; set; }
        public decimal? SoBaiQuaTrinh { get; set; }
        public decimal? SoBaiGiuaKy { get; set; }
        public decimal? SoBaiCuoiKy { get; set; }
        public decimal? DonGiaQuaTrinh { get; set; }
        public decimal? DonGiaGiuaKy { get; set; }
        public decimal? DonGiaCuoiKy { get; set; }
        public decimal? TongTien { get; set; }
    }
}