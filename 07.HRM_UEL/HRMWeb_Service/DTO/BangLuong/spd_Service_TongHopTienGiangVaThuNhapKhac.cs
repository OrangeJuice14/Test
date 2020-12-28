using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWeb_Service.DTO.BangLuong
{
    public class spd_Service_TongHopTienGiangVaThuNhapKhac
    {
        public ThongTinCanBo ThongTinCanBo { get; set; }
        public TongHopLuong TongHopLuong { get; set; }
        public IList<ChiTietLuong_PhuCap> ChiTietLuong_PhuCap { get; set; }
        public IList<ChiTietLuong_KhenThuongPhucLoi> ChiTietLuong_KhenThuongPhucLoi { get; set; }
        public IList<ChiTietLuong_ThuNhapKhac> ChiTietLuong_ThuNhapKhac { get; set; }
        public IList<ChiTietLuong_KhauTru> ChiTietLuong_KhauTru { get; set; }
    }
}