using System;

namespace HRMWebApp.KPI.Core.DTO
{
    public class DepartmentDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? ParentDepartmentId { get; set; }
        public int DepartmentType { get; set; }
        public string DepartmentTypeName { get; set; }
        public Guid StaffId { get; set; }
        public int StaffRoleId { get; set; }    
        public bool IsManaged { get; set; }
        public bool @checked { get; set; }
    }
}
