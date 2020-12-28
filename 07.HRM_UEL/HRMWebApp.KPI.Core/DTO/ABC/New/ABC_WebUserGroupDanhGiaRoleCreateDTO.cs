using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_WebUserGroupDanhGiaRoleCreateDTO
    {
        public Guid? WebUserId { get; set; }
        public Guid? GroupDanhGiaId { get; set; }
    }
}
