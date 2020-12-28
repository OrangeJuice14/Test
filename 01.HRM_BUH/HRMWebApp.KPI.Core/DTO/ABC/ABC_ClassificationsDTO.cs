using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_ClassificationsDTO
    {
        public Guid Id { get; set; }
        public decimal? MinRecord { get; set; }
        public decimal? MaxRecord { get; set; }
        public string Rank { get; set; }
        public bool? IsEligible { get; set; }
        public Guid ABC_ClassificationSetId { get; set; }
    }
}
