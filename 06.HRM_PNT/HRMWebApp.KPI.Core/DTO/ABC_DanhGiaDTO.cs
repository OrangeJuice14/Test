using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_DanhGiaDTO
    {
        public Guid Id { get; set; }
        public string MoTa { get; set; }
        public DateTime TuNgay { get; set; } 
        public DateTime DenNgay { get; set; }
        public bool? IsLock { get; set; }
        public Guid LoaiBoDanhGiaId { get; set; }
        public string LoaiBoDanhGiaTenLoai { get; set; }
    }
}
