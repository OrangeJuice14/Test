using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_KetQuaDTO
    {
        public Guid Id { get; set; }
        public Guid KyDanhGiaId { get; set; }
        public Guid NhanVienDuocDanhGiaId { get; set; } 
        public string KyDanhGiaTen { get; set; } 
        public string KyDanhGiaLoai { get; set; } 
    }
}