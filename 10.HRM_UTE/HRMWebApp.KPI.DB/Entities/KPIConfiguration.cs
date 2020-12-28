using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class KPIConfiguration
    {
        public virtual int Id { get; set; }
        public virtual int MaxBonusRecord { get; set; }
        public virtual int TotalHourDefault { get; set; }
        public virtual double ScienceResearchConfig { get; set; }
        public virtual double OtherActivityConfig { get; set; }
        public virtual double GiangdayConfig { get; set; }
    }
}
