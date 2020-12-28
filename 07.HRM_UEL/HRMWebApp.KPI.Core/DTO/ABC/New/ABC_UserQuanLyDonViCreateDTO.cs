using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_UserQuanLyDonViCreateDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
