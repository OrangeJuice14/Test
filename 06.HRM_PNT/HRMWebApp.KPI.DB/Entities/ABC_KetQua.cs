using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_KetQua
    {
        public virtual Guid Id { get; set; }
        public virtual ABC_KyDanhGia KyDanhGia { get; set; }
        public virtual Staff NhanVienDuocDanhGia { get; set; }
    }
}
