using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_KyDanhGiaDTO
    {
        
        public  Guid Id { get; set; }
        public  string Ten { get; set; }
        public  int? Nam { get; set; }
        public  DateTime? TuNgay { get; set; }
        public  DateTime? DenNgay { get; set; }
        public Guid? ParentId { get; set; }
        public  int? Loai { get; set; }
        public DateTime? NgayTao { get; set; }
    }
}
