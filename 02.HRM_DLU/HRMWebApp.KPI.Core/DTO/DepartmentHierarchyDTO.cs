using System;
using System.Collections.Generic;

namespace HRMWebApp.KPI.Core.DTO
{
    public class DepartmentHierarchyDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<DepartmentDTO> items { get; set; }
        public int DepartmentType { get; set; }
        public string DepartmentTypeName { get; set; }
        public string text { get; set; }

    }
}
