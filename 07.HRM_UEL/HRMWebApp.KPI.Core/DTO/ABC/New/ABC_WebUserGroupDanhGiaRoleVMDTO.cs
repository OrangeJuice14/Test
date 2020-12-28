using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_WebUserGroupDanhGiaRoleVMDTO
    {
        public Guid Id { get; set; }
        public Guid? WebUserId { get; set; }
        public string WebUserUserName { get; set; }
        public string WebUserStaffInfoStaffProfileName { get; set; }
        public string WebUserStaffInfoSubjectName { get; set; }
        public string WebUserStaffInfoStaffDepartmentName { get; set; }
        public Guid? WebUserStaffInfoStaffDepartmentId { get; set; }
        public Guid? GroupDanhGiaId { get; set; }
        public string GroupDanhGiaName { get; set; }
        public bool? GroupDanhGiaHasQuanLyDonVi { get; set; }
    }
}
