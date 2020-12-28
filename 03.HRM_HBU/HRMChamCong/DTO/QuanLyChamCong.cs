using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMChamCong.DTO
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
    public class ChamCongNgay_DoiCa
    {
        public string CC_ChamCongTheoNgayOid { get; set; }
        public Guid CC_CaChamCong { get; set; }
    }
    public class ChamCongThang_DoiCa
    {
        public List<ChamCongNgay_DoiCa> ChiTietChamCong { get; set; }
    }

    public class ChamCongNgoaiGio
    {
        public Guid Oid { get; set; }
        public String SoCongNgoaiGio { get; set; }
        public String SoCongNgoaiGioSau23Gio { get; set; }
        public String SoCongNgoaiGioT7CN { get; set; }
        public String SoCongNgoaiGioT7CNSau23Gio { get; set; }
        public String SoCongNgoaiGioLe { get; set; }
        public String SoCongNgoaiGioLeSau23Gio { get; set; }
    }
}