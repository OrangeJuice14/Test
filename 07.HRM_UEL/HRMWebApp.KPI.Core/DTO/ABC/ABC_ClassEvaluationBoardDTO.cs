using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_ClassEvaluationBoardDTO
    {
        public Guid Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime StartRating { get; set; }
        public DateTime EndRating { get; set; }
        public virtual ABC_ClassEvaluationBoardDTO ABC_ParentClassEvaluationBoard { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? SelectedId { get; set; }
        public Guid? SelectedParentId { get; set; }
        public string IsRatedSecond { get; set; }
        public string IsRatedThird { get; set; }
    }
}
