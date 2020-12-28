using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_DieuKienXepLoaiPhu
    {
        public virtual Guid Id { get; set; }
        public virtual int? DiemDat { get; set; }
        public virtual ABC_XepLoaiDanhGia XepLoaiDanhGia { get; set; }
        public virtual ABC_TieuChi TieuChi { get; set; }
    }
}
