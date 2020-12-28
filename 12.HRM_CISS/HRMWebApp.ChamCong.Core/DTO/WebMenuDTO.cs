using System;

namespace HRMWebApp.ChamCong.Core.DTO
{
    public class WebMenuDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public int? Index { get; set; }
        public bool @checked { get; set; }
    }
}
