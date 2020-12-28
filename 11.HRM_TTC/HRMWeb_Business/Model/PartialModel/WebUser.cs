using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWeb_Business.Model
{
    public partial class WebUser
    {
        public string SoHieuCongChuc { get; set; }
        public string HoVaTen { get; set; }
        public string Email { get; set; }
        public string TenBoPhan { get; set; }
        public IEnumerable<DTO_BoPhan> DanhSachDTO_BoPhan { get; set; }
    }
}
