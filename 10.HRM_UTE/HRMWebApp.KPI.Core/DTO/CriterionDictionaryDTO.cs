using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO
{
    public class CriterionDictionaryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public double Record { get; set; }
        public double MaxRecord { get; set; }
        public double DataRecord { get; set; }
        public double DataMaxRecord { get; set; }
        public string Tooltip { get; set; }
        public Guid CriterionId { get; set; }
        public int Exception { get; set; }
        public double NumberOfHour { get; set; }
        public string ManageCode { get; set; }
        public int LevelIndex { get; set; }
        public List<Guid> StudyYearIds { get; set; }
        public IList<StudyYear> StudyYears { get; set; }
        public Guid targetGroupDetailId { get; set; }
        //  public ProfessorCriterionDTO ProfessorCriterion { get; set; }
        public Guid ProfessorCriterionId { get; set; }
    }
}
