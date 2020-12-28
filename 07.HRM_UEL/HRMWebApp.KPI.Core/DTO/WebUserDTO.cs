using System;

namespace HRMWebApp.KPI.Core.DTO
{
    public class WebUserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public Guid StaffInfoStaffDepartmentId { get; set; }
        public string StaffInfoStaffDepartmentName { get; set; }
        public string StaffInfoStaffProfileName { get; set; }
    }
}
