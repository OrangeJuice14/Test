using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class RoleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool @checked { get; set; }
        public string ParentId { get; set; }
    }
}
