using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWebApp.ChamCong.Core.DTO
{
    public class QuanLyChamCong
    {
    }
    public class UserSave
    {
        public Guid Oid { get; set; }
        public string IDHinhThucNghi { get; set; }
        public bool DaChamCong { get; set; }
    }
    public class ChamCongNgay
    {
        public string CC_ChamCongTheoNgayOid { get; set; }
        public string MaHinhThucNghi { get; set; }
    }
    public class ChamCongThang
    {
        public List<ChamCongNgay> ChiTietChamCong { get; set; }
    }
}