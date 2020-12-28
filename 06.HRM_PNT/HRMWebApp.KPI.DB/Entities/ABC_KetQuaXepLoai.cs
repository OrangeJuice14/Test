using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_KetQuaXepLoai
    {
        public virtual Guid Id { get; set; }
        public virtual string TenXepLoai { get; set; }
        public virtual int FromScore { get; set; }
        public virtual int ToScore { get; set; }
        public virtual ABC_DanhGia DanhGia { get; set; }
    }
}
