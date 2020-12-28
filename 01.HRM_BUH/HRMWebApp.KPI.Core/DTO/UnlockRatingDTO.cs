using System;
using System.Collections.Generic;

namespace HRMWebApp.KPI.Core.DTO
{
    public class UnlockRatingDTO
    {
        public UnlockRatingDTO()
        {
            StaffIds = new List<Guid>();
        }
        public bool IsDepartment { get; set; }
        public Guid PlanKPIId { get; set; }
        public Guid DepartmentId { get; set; }
        public DateTime RatingStartTime { get; set; }
        public DateTime RatingEndTime { get; set; }
        public List<Guid> StaffIds { get; set; }

    }
}
