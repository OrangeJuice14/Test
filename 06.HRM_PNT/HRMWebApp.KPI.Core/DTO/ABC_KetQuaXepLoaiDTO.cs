using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_KetQuaXepLoaiDTO
    {
        public Guid Id { get; set; }
        public string TenXepLoai { get; set; }
        public int FromScore { get; set; }
        public int ToScore { get; set; }
        public Guid DanhGiaId { get; set; }
    }
}
