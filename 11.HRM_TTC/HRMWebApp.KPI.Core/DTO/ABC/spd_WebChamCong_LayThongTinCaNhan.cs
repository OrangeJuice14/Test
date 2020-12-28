using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.ABC
{
    public class spd_WebChamCong_LayThongTinCaNhan
    {
        public string MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public string TenChucDanh { get; set; }
        public string TenCongTy { get; set; }
        public string TenBoPhan { get; set; }
        public DateTime? NgayVaoCongTy { get; set; }
        public DateTime? NgayBoNhiemChucVu { get; set; }
        public string QuanLyTrucTiep { get; set; }
        public string QuanLyTrucTiep_ChucDanh { get; set; }
        public string QuanLyTrenMotCap { get; set; }
        public string QuanLyTrenMotCap_ChucDanh { get; set; }
    }
}
