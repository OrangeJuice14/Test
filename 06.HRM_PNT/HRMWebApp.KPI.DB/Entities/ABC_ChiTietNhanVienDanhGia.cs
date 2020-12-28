using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_ChiTietNhanVienDanhGia
    {
        public virtual Guid Id { get; set; }
        public virtual ABC_KetQua KetQua { get; set; }
        public virtual Staff NhanVienDanhGia { get; set; }
        public virtual ABC_LoaiDanhGia LoaiDanhGia { get; set; }
        public virtual ABC_DanhGia DanhGia { get; set; }
        public virtual bool? isLock { get; set; }
        public virtual string YKienDongGop { get; set; } 
        public virtual int? TongDiem { get; set; }
        public virtual DateTime? TimeLock { get; set; }
    }
}
