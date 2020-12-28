using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_Classifications
    {
        public virtual Guid Id { get; set; }
        public virtual decimal? MinRecord { get; set; }
        public virtual decimal? MaxRecord { get; set; }
        public virtual string Rank { get; set; }
        public virtual bool? IsEligible { get; set; }
        public virtual ABC_ClassificationSet ABC_ClassificationSet { get; set; }
    }
}
