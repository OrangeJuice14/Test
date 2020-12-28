using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ResultDetail
    {
        public ResultDetail()
        {
            FileAttachments = new List<FileAttachment>();
        }
        public virtual Guid Id { get; set; }
        public virtual Result Result { get; set; }
        public virtual PlanKPIDetail PlanKPIDetail { get; set; }
        public virtual TargetGroupDetail TargetGroupDetail { get; set; }
        public virtual double Record { get; set; }
        public virtual double RecordSecond { get; set; }
        public virtual bool IsTargetGroupRating { get; set; }
        public virtual bool IsConfirmed { get; set; }
        public virtual double SupervisorRecord { get; set; }
        public virtual string PreviousResult { get; set; }
        public virtual string CurrentResult { get; set; }
        public virtual string RegisterTarget { get; set; }
        public virtual IList<FileAttachment> FileAttachments { get; set; }
        public virtual string Note { get; set; }
        public virtual string SupervisorNote { get; set; }

    }
}
