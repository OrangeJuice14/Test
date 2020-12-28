using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class FileAttachment
    {
        public virtual Guid Id { get; set; }
        public virtual string Path { get; set; }
        public virtual PlanKPIDetail PlanKPIDetail { get; set; }
        public virtual ResultDetail ResultDetail { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual string Name { get; set; }
        public virtual string Extension { get; set; }
    }
}
