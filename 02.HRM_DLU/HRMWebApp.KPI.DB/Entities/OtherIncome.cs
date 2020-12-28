using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class OtherIncome
    {
        public virtual Guid Oid { get; set; }
        public virtual string MaQuanLy { get; set; }
        public virtual string TenLoaiThuNhapKhac { get; set; }
    }
}
