using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWeb_Service.DTO.BangLuong
{
    public class spd_BangThuLaoNhanVien_ChiTietThanhToanPMS
    {
        public IList<spd_BangThuLaoNhanVien_ChiTietThanhToanPMS1> PMS1 { get; set; }
        public IList<spd_BangThuLaoNhanVien_ChiTietThanhToanPMS2> PMS2 { get; set; }
        public decimal? SoTien { get; set; }
    }
}