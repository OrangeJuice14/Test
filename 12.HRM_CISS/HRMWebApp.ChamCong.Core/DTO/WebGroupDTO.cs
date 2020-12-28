using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.ChamCong.Core.DTO
{
    public class WebGroupDTO
    {
        public WebGroupDTO()
        {
            WebMenuIds = new List<Guid>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> WebMenuIds { get; set; }       
    }
}
