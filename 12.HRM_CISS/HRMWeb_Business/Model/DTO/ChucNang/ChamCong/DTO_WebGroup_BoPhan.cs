using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWeb_Business.Model.DTO.ChucNang.ChamCong
{
    public class DTO_WebGroup_BoPhan
    {
        public Guid WebGroupId { get; set; }
        public List<Guid> BoPhanIds { get; set; }
    }
}
