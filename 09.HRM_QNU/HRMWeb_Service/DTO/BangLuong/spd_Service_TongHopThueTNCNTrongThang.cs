using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWeb_Service.DTO.BangLuong
{
    public class spd_Service_TongHopThueTNCNTrongThang
    {
        public string HoTen { get; set; }
        public string MaSoThue { get; set; }
        public decimal TongThuNhap { get; set; }
        public decimal TongThuNhapChiuThue { get; set; }
        public decimal? MienThue { get; set; }
        public decimal? TongGiamTru { get; set; }
        public decimal GiamTruGiaCanh { get; set; }
        public decimal GiamTruBanThan { get; set; }
        public decimal GiamTruBaoHiem { get; set; }
        public decimal SoNguoiPhuThuoc { get; set; }
        public decimal ThuNhapTinhThue { get; set; }
        public decimal ThueTNCNPhaiNop { get; set; }
    }
}