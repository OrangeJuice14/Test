using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.ABC
{
    public class spd_Web_DanhGia_ThanhTichCaNhan_SangKien
    {
        public string TenSangKien { get; set; }
        public string CapDoThamGia { get; set; }
        public DateTime? ThoiGianHoanThanh { get; set; }
        public string Diem { get; set; }
    }

    public class spd_Web_DanhGia_ThanhTichCaNhan_NCKH
    {
        public string TenHoatDong { get; set; }
        public string TenNCKH { get; set; }
        public string CapDoThamGia { get; set; }
        public DateTime? ThoiGianHoanThanh { get; set; }
        public decimal SoTietQuyDoi { get; set; }
        public string Diem { get; set; }
    }
    public class spd_Web_DanhGia_ThanhTichCaNhan_KhenThuong
    {
        public string TenHinhThucKhenThuong { get; set; }
        public string LyDo { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime? NgayQuyetDinh { get; set; }
        public string CoQuanRaQuyetDinh { get; set; }
    }
}
