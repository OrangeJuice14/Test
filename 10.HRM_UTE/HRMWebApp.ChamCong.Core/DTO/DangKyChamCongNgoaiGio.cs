using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWebApp.ChamCong.Core.DTO
{
    public class DangKyChamCongNgoaiGio
    {
        public string LyDo { get; set; }
        public decimal SoGio { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public DTO_Thu[] DanhSachDTO_Thu { get; set; }
        public string ThongTinNhanVien { get; set; }
        public int GioBatDau { get; set; }
        public int PhutBatDau { get; set; }
        public int GioKetThuc { get; set; }
        public int PhutKetThuc { get; set; }
    }
}