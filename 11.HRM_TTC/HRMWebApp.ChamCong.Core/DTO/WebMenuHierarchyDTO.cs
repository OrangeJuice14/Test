using System;
using System.Collections.Generic;

namespace HRMWebApp.ChamCong.Core.DTO
{
    public class WebMenuHierarchyDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<WebMenuDTO> items { get; set; }
        public string text { get; set; }
        public Guid ParentId { get; set; }
        public bool @checked { get; set; }
    }
}
