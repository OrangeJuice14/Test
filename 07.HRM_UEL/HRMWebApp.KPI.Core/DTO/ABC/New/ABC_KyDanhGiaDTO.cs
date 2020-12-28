using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.ABC.New
{
    public class ABC_KyDanhGiaDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public DateTime? NgayBatDauDanhGia { get; set; }
        public DateTime? NgayKetThucDanhGia { get; set; }
        public int? Nam { get; set; }
        public Int64? GCRecord { get; set; }
        public Guid? DeleteUserId { get; set; }
        public Guid? LastEditUserId { get; set; }
        public Guid? AddUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public DateTime? EditLastTime { get; set; }
        public DateTime? AddTime { get; set; }
        public string NamHoc { get; set; }
        public int? Loai { get; set; }// Loai 12: Nam
                                      // Loai 6: 6 tháng
                                      // Loai 3: Quy'
                                      // Loai 1: Thang'
        public Guid? ParentId { get; set; }
    }
}
