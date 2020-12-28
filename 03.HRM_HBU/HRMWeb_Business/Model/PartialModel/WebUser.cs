using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWeb_Business.Model
{
    public partial class WebUser
    {
        //Them vao
        //private DTO_WebGroup _DTOWebGroup = null;
        //public DTO_WebGroup DTOWebGroup
        //{
        //    get { return _DTOWebGroup; }
        //    set
        //    {
        //        _DTOWebGroup = value;
        //        if (_DTOWebGroup != null)
        //        {
        //            this.WebGroupID = _DTOWebGroup.ID;
        //        }
        //        else
        //        {
        //            this.WebGroupID = null;
        //        }
        //    }
        //}

        public string MaNhanSu { get; set; }
        public string HoVaTen { get; set; }
        public string Email { get; set; }
        public string TenBoPhan { get; set; }
        public IEnumerable<DTO_BoPhan> DanhSachDTO_BoPhan { get; set; }
    }
}
