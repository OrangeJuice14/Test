using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class WebUserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid WebGroupId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid StaffInfoId { get; set; }
        public Guid StaffInfoPositionId { get; set; } 
        public string passWord { get; set; } 
    }
}
