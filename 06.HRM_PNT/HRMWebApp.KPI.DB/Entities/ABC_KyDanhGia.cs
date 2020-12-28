using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_KyDanhGia
    {
        public virtual Guid Id { get; set; } 
        public virtual string Ten { get; set; } 
        public virtual int? Nam { get; set; } 
        public virtual DateTime? TuNgay { get; set; } 
        public virtual DateTime? DenNgay { get; set; }
        public virtual ABC_KyDanhGia Parent { get; set; } 
        public virtual int? Loai { get; set; }
        public virtual DateTime? NgayTao { get; set; }
    }
}
