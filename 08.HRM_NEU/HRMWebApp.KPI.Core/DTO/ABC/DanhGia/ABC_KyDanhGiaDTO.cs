using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_KyDanhGiaDTO
    {
        public Guid Id { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public DateTime? NgayBatDauDanhGia { get; set; }
        public DateTime? NgayKetThucDanhGia { get; set; }
        public int? Nam { get; set; }
        public string Name { get; set; }
        public int? Loai { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? DelUserId { get; set; }
        public Guid? LastEditUserId { get; set; }
        public Guid? AddUserId { get; set; }
        public DateTime? DelTime { get; set; }
        public DateTime? EditLastTime { get; set; }
        public DateTime? AddTime { get; set; }
        public IList<ABC_KyDanhGiaDTO> Childrens { get; set; }
    }
}
