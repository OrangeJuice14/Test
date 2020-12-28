using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ResultDTO
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public Staff StaffRated { get; set; }
        public Guid StaffRatedId { get; set; }
        public Staff StaffRating { get; set; }
        public Guid StaffRatingId { get; set; }
        public PlanKPI PlanKPI { get; set; }
        public Guid PlanKPIId { get; set; }
        public string PlanName { get; set; }
        public string TotalRecord { get; set; }
        public double TotalRecordNumber { get; set; }
        public double TotalRecordSecondNumber { get; set; }
        public string TotalRecordSecond { get; set; }
        public bool IsLocked { get; set; }
        public bool IsUnlocked { get; set; }
        public bool IsCommitted { get; set; }
        public bool IsUnlockedForRating { get; set; }
        public string NumberOfEditing { get; set; }
    }
}
