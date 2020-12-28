using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class FileAttachmentDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public Guid PlanKPIDetailId { get; set; }
        public Guid ResultDetailId { get; set; }
        public string FileName
        {
            get
            {
                return this.Name + this.Extension;
            }
        }
    }
}
