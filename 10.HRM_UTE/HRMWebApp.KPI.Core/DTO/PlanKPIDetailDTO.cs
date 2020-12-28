using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HRMWebApp.KPI.Core.DTO
{
    public class PlanKPIDetailDTO
    {
        public PlanKPIDetailDTO()
        {
            PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
        }
        public  Guid Id { get; set; }
        public  string Name { get; set; }
        public  string ExcecuteMethod { get; set; }
        public  string BasicResource { get; set; }
        public string PrevisousKPI { get; set; }
        public string CurrentKPI { get; set; }
        public string StartTime { get; set; }
        public PlanKPI PlanKPI { get; set; }
        public Guid PlanKPIId { get; set; }
        public  Criterion Criterion { get; set; }
        public Guid CriterionId { get; set; }
        public string CriterionName { get; set; }
        public  Staff Staff { get; set; }
        public Staff StaffLeader { get; set; }
        public Guid StaffId { get; set; }
        public TargetGroupDetail TargetGroupDetail { get; set; }
        public Guid TargetGroupDetailId { get; set; }
        public bool IsDisable { get; set; }
        public bool IsLocked { get; set; }
        public List<Guid> DepartmentIds { get; set; }
        public List<PlanKPIDetail_KPIDTO> PlanKPIDetail_KPIs { get; set; }
        public bool IsMoved { get; set; }
        public int CapMucTieu { get; set; }
        public DanhMucMTCLDTO DanhMucMTCL { get; set; }

    }
}
