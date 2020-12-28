using System;
using System.Collections.Generic;
using HRMWebApp.KPI.DB;

namespace HRMWebApp.KPI.Core.DTO
{
    public class RoleHierarchyDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<RoleDTO> items { get; set; }

    }
}
