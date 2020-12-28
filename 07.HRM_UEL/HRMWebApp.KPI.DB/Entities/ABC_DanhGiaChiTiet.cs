using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_DanhGiaChiTiet
    {
        public virtual Guid Id{ get; set; } 
        public virtual float? Diem { get; set; } 
        public virtual ABC_DanhGia DanhGia { get; set; } 
        public virtual ABC_TieuChi TieuChi { get; set; } 
    }
}
