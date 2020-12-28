using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMWebApp.ChamCong.Core.DTO
{
    public class DTO_BoPhan
    {
        public string Oid { get; set; }
        public string TenBoPhan { get; set; }
        public bool Chon { get; set; }
    }

    public class QuanLyUser
    {
        public Guid Oid { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HoVaTen { get; set; }
        public string Email { get; set; }
        public bool HoatDong { get; set; }
        public DTO_BoPhan[] DanhSachDTO_BoPhan { get; set; }
        public string WebGroupID { get; set; }
        public string BoPhanId { get; set; }
        public string ThongTinNhanVien { get; set; }
        public string EmailHDQT { get; set; }
        public string EmailHT { get; set; }
        public string EmailTP { get; set; }
    }
}