using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class WebUserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string StaffInfoStaffProfileName { get; set; }
        public Guid StaffInfoId { get; set; }
        public string StaffInfoStaffProfileLastName { get; set; }
        public string StaffInfoStaffProfileFirstName { get; set; }
        public Guid StaffInfoSubjectId { get; set; }
        public string StaffInfoSubjectName { get; set; }
        public Guid StaffInfoDepartmentId { get; set; }
        public string StaffInfoDepartmentName { get; set; }
    }
}
