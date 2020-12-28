using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_ChiTietNhanVienDanhGiaDTO
    {
        public Guid Id { get; set; }
        public Guid KetQuaId { get; set; }
        public Guid NhanVienDanhGiaId { get; set; }
        public Guid LoaiDanhGiaId { get; set; }
        public Guid DanhGiaId { get; set; }
        public bool? isLock { get; set; }
        public string YKienDongGop { get; set; }
        public int? TongDiem { get; set; }
        public DateTime? TimeLock { get; set; }
    }
}
