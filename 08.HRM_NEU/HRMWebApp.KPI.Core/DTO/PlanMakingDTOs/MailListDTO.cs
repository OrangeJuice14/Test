using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.PlanMakingDTOs
{
    public class MailListDTO
    {
        public Guid StaffId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid CriterionId { get; set; }
    }
}
