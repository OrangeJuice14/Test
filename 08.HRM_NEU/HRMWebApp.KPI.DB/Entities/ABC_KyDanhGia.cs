using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_KyDanhGia
    {
        public virtual Guid Id{ get; set; } 
        public virtual string Name { get; set; } 
        public virtual DateTime? TuNgay { get; set; } 
        public virtual DateTime? DenNgay { get; set; } 
        public virtual DateTime? NgayBatDauDanhGia { get; set; } 
        public virtual DateTime? NgayKetThucDanhGia { get; set; } 
        public virtual int? Nam { get; set; }
        public virtual Guid? DelUserId { get; set; }
        public virtual Guid? LastEditUserId { get; set; }
        public virtual Guid? AddUserId { get; set; }
        public virtual DateTime? DelTime { get; set; }
        public virtual DateTime? EditLastTime { get; set; }
        public virtual DateTime? AddTime { get; set; }
        public virtual int? Loai { get; set; }// Loai 0: Nam
                                              // Loai 1: 6 thang
                                              // Loai 2: Quy'
                                              // Loai 3: Thang'
        public virtual ABC_KyDanhGia Parent { get; set; }
        public virtual IList<ABC_KyDanhGia> Childrens { get; set; }
    }
}
