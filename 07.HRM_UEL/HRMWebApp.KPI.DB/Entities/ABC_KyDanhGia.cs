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
        public virtual Int64? GCRecord { get; set; }
        public virtual Guid? DeleteUserId { get; set; }
        public virtual Guid? LastEditUserId { get; set; }
        public virtual Guid? AddUserId { get; set; }
        public virtual DateTime? DeleteTime { get; set; }
        public virtual DateTime? LastEditTime { get; set; }
        public virtual DateTime? AddTime { get; set; }
        public virtual string NamHoc { get; set; }
        public virtual int? Loai { get; set; }// Loai 12: Nam
                                              // Loai 6: 6 tháng
                                              // Loai 3: Quy'
                                              // Loai 1: Thang'
        public virtual ABC_KyDanhGia Parent { get; set; }
        public virtual IList<ABC_KyDanhGia> Childrens { get; set; }
    }
}
