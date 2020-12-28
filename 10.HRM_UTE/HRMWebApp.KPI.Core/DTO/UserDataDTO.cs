using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class UserDataDTO
    {

        public DateTime CreateDate { get; set; }
        public DateTime BirthDate { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ThongTinNhanVien { get; set; }
        public string AgentObjectTypeId { get; set; }
        public string DepartmentId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HoVaTen { get; set; }
        public string WebGroupId { get; set; }
        public bool IsKPIs { get; set; }
        public string SubPositionId { get; set; }
        public string DepartmentName { get; set; }
    }
}
