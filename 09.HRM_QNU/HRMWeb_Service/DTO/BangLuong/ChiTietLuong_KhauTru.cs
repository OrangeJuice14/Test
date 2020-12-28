using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWeb_Service.DTO.BangLuong
{
    public class ChiTietLuong_KhauTru
    {
        public string DienGiai { get; set; }
        public string TyLeTru { get; set; }
        public decimal? SoTien { get; set; }
        public decimal? SoTienChiuThue { get; set; }
    }
}