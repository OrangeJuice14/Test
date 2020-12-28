using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ProfessorCriterion
    {
        public ProfessorCriterion()
        {
            CriterionDictionaries = new List<CriterionDictionary>();
            StudyYears = new List<StudyYear>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string ManageCode { get; set; }
        public virtual double Record { get; set; }
        public virtual int NumberOfHour { get; set; }
        public virtual string Tooltip { get; set; }
        public virtual TargetGroupDetail TargetGroupDetail { get; set; }
        public virtual int OrderNumber { get; set; }
        public virtual CriterionType CriterionType { get; set; }
        public virtual IList<CriterionDictionary> CriterionDictionaries { get; set; } 
        public virtual IList<StudyYear> StudyYears { get; set; }
        public virtual DonViTinh DonViTinh { get; set; }
        public virtual DanhGiaChiTiet DanhGiaChiTiet { get; set; }
        public virtual IList<Department> DonViCungCap { get; set; }
        public virtual IList<ChucDanh> ChucDanh { get; set; }
    }
}
