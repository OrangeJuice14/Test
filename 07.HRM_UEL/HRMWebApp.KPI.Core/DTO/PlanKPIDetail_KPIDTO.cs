using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class PlanKPIDetail_KPIDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PlanKPIDetailId { get; set; }
        public MeasureUnitDTO MeasureUnit { get; set; }
        public int MeasureUnitId { get; set; }
        public string MeasureUnitName { get; set; }
        public int OrderNumber { get; set; }
    }
}
