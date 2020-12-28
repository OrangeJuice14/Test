using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_KetQuaChiTiet
    {
        public virtual Guid Id { get; set; }
        public virtual int? Diem { get; set; }
        public virtual string GhiChu { get; set; }
        public virtual string YKienDanhGia { get; set; }
        public virtual bool? IsChecked { get; set; }
        public virtual ABC_TieuChiDanhGia TieuChiDanhGia { get; set; }
        public virtual ABC_ChiTietNhanVienDanhGia ChiTietNhanVienDanhGia { get; set; }
        //public virtual ABC_KetQuaChiTiet Parent { get; set; } 
    }
}
