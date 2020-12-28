using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWebApp.ChamCong.Core.DTO
{
    public class QuanLyNghiPhepNam
    {      
        public Guid Oid { get; set; }
        public decimal TongSoNgayPhep { get; set; }
        public decimal SoNgayPhepDaNghi { get; set; }
        public decimal SoNgayPhepConLai { get; set; }
        public decimal SoNgayPhepCongThem { get; set; }
        public decimal SoNgayPhepNamTruoc { get; set; }
        public decimal SoNgayPhepNamHienTai { get; set; }
    }
}