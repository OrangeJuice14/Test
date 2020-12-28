using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_BoTieuChiDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public bool? Status { get; set; }
        public long? GCRecord { get; set; }
        public DateTime? TimeDelete { get; set; }
        public Guid? UserDeleteId { get; set; }
        public bool? HasDieuKienDanhGia { get; set; }
        public bool? ShowTen { get; set; }
        public bool? ShowDonVi { get; set; }
        public bool? ShowBoPhan { get; set; }
        public bool? ShowDay { get; set; }
        public bool? ShowMonth { get; set; }
        public bool? ShowYear { get; set; }
        public Guid? LastEditUserId { get; set; }
        public Guid? AddUserId { get; set; }
        public DateTime? EditLastTime { get; set; }
        public DateTime? AddTime { get; set; }
        public int? LoaiBoTieuChi { get; set; } // Loai 0: Nam
                                                // Loai 1: 6 thang
                                                // Loai 2: Quy'
                                                // Loai 3: Thang'
    }
}
