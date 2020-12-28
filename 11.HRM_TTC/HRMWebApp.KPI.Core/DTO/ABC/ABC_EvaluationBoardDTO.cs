using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_EvaluationBoardDTO
    {
        public Guid Id { get; set; }
        public int Quater { get; set; }
        public string Year { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime? StartRating { get; set; }
        public DateTime? EndRating { get; set; }
        public string IsSupervisorRated { get; set; }
    }
}
