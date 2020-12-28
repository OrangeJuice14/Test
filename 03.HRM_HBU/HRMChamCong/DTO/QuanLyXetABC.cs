using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMChamCong.DTO
{
    public class QuanLyXetABC
    {
        public class QuanLyXetABCFieldsForSave
        {
            public Guid Oid { get; set; }
            public string DanhGia { get; set; }
            public string DanhGiaTruocDieuChinh { get; set; }
            public string DienGiai { get; set; }
            public bool TrangThai { get; set; }
        }
    }
}