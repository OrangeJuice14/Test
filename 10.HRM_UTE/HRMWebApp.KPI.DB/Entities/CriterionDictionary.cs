using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class CriterionDictionary
    {
        public CriterionDictionary()
        {
            StudyYears = new List<StudyYear>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double Record { get; set; }
        public virtual double MaxRecord { get; set; }
        public virtual double DataRecord { get; set; }
        public virtual double DataMaxRecord { get; set; }
        public virtual double NumberOfHour { get; set; }
        public virtual string Tooltip { get; set; }
        public virtual string ManageCode { get; set; }
        public virtual ProfessorCriterion ProfessorCriterion { get; set; }
        public virtual Criterion Criterion { get; set; }
        public virtual TargetGroupDetail TargetGroupDetail { get; set; }
        public virtual int OrderNumber { get; set; }
        public virtual int LevelIndex { get; set; }
        public virtual IList<StudyYear> StudyYears { get; set; }
    }
}
