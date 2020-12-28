using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_DieuKienXepLoaiPhuDTO
    {
        public Guid Id { get; set; }
        public int? DiemDat { get; set; }
        public Guid? XepLoaiDanhGiaId { get; set; }
        public Guid? TieuChiId { get; set; }
        public string TieuChiNoiDung { get; set; }
        //public 
    }
}
