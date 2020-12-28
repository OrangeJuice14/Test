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
        public int Month { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public virtual ABC_EvaluationBoardDTO ABC_ParentEvaluationBoard { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? SelectedId { get; set; }
        public Guid? SelectedParentId { get; set; }
        public int EvaluationBoardType { get; set; }
        public string IsSupervisorRated { get; set; }

    }
}
