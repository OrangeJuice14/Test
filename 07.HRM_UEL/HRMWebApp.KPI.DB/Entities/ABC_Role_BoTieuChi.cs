using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_Role_BoTieuChi
    {
        public virtual Guid Id { get; set; }
        public virtual ABC_GroupDanhGia GroupTuDanhGia { get; set; }
        public virtual ABC_GroupDanhGia GroupDanhGia { get; set; }
        public virtual ABC_BoTieuChi BoTieuChi { get; set; }
        public virtual bool? UserDanhGiaNgangHang { get; set; }

    }
}
