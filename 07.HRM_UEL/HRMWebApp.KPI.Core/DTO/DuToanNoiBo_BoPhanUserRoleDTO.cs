using System;

namespace HRMWebApp.KPI.Core.DTO
{
    public class DuToanNoiBo_BoPhanUserRoleDTO
    {
        public virtual Guid Id { get; set; }
        public virtual Guid DepartmentId { get; set; }
        public virtual string DepartmentName { get; set; }
        public virtual Guid WebUserId { get; set; }
    }
}
