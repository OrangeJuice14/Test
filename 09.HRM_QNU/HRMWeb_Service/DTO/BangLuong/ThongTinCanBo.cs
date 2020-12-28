using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWeb_Service.DTO.BangLuong
{
    public class ThongTinCanBo
    {
        public string HoTen { get; set; }
        public string MaSoThue { get; set; }
        public string SoNguoiPhuThuoc { get; set; }
        public string MaNhanSu { get; set; }
        public string NgachLuong { get; set; }
        public string BacLuong { get; set; }
        public decimal? HeSoLuong { get; set; }
        public decimal? HeSoPhuCapChucVu { get; set; }
        public string LuongCoBan { get; set; }
        public string TyLeHuongLuong { get; set; }
        public string XepLoai { get; set; }
        public string HeSoTNTT { get; set; }
        public string MucGiamTru { get; set; }
        public string DonVi { get; set; }
        public string NganHang { get; set; }
    }
}