using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_DanhGia
    {
        public virtual Guid Id { get; set; }
        public virtual string MoTa { get; set; }
        public virtual DateTime TuNgay { get; set; }
        public virtual DateTime DenNgay { get; set; }
        public virtual bool? IsLock { get; set; }
        public virtual ABC_LoaiBoDanhGia LoaiBoDanhGia { get; set; }

    }
}
