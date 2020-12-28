using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_RatingDetailDTO
    {
        public ABC_RatingDetailDTO()
        {
            ABC_CriterionDetail = new ABC_CriterionDetailDTO();
            ABC_Rating = new ABC_RatingDTO();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OrderNumber { get; set; }
        public decimal StaffRecord { get; set; }
        public decimal MaxRecord { get; set; }
        public decimal SupervisorRecord { get; set; }
        public string SupervisorNote { get; set; }
        public decimal AdminRecord { get; set; }
        public string AdminNote { get; set; }
        public ABC_CriterionDetailDTO ABC_CriterionDetail { get; set; }
        public ABC_RatingDTO ABC_Rating { get; set; }
        public Guid ABC_CriterionDetailId { get; set; }
        public Guid ABC_RatingId { get; set; }
        public int ABC_CriterionDetailType { get; set; }
    }
}
