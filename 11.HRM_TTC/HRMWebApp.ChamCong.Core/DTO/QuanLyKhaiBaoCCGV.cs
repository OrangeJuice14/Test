using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWebApp.ChamCong.Core.DTO
{
    public class DTO_Thu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Chon { get; set; }
    }

    public class QuanLyKhaiBaoCCGV
    {
        public byte Buoi { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public DTO_Thu[] DanhSachDTO_Thu { get; set; }
        public string ThongTinNhanVien { get; set; }
    }
}