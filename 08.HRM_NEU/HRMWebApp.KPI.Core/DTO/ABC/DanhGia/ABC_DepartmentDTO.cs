using System;
using System.Collections.Generic;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_DepartmentDTO
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public bool @checked { get; set; }
        public bool? expanded { get; set; }
        public List<ABC_DepartmentDTO> items { get; set; }
    }
}
