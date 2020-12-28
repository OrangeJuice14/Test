using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
   public class PlanKPIDetail
    {
       public PlanKPIDetail()
       {
           SubDepartments = new List<Department>();
           SubStaffs = new List<Staff>();
           Criterions = new List<Criterion>();
           PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
           ScienceResearches = new List<ScienceResearch>();
           ProfessorOtherActivities = new List<ProfessorOtherActivity>();
           Methods = new List<Method>();
           FileAttachments = new List<FileAttachment>();
       }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string ExecuteMethod { get; set; }
        public virtual string BasicResource { get; set; }
        public virtual string PreviousKPI { get; set; }
        public virtual Department LeadDepartment { get; set; }
        public virtual Staff StaffLeader { get; set; }
        public virtual MeasureUnit MeasureUnit { get; set; }
        public virtual ManageCode ManageCode { get; set; }
        public virtual string CurrentKPI { get; set; }
        public virtual string TargetDetail { get; set; }
        public virtual PlanStaff PlanStaff { get; set; }
        public virtual double MaxRecord { get; set; }
        public virtual IList<Criterion> Criterions { get; set; }
        public virtual Criterion  FromCriterion { get; set; }
        public virtual ProfessorCriterion FromProfessorCriterion { get; set; }
        public virtual TargetGroupDetail TargetGroupDetail { get; set; }
        public virtual bool IsDisable { get; set; }
        public virtual PlanKPIDetail ParentPlanKPIDetail { get; set; }
        public virtual IList<Method> Methods { get; set; }
        public virtual IList<PlanKPIDetail_KPI> PlanKPIDetail_KPIs { get; set; }
        public virtual IList<ProfessorOtherActivity> ProfessorOtherActivities { get; set; }
        public virtual IList<ScienceResearch> ScienceResearches { get; set; }
        public virtual IList<FileAttachment> FileAttachments { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual IList<Department> SubDepartments { get; set; }
        public virtual IList<Staff> SubStaffs { get; set; }
        public virtual bool IsAddition { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime EndTime { get; set; }
        public virtual bool IsFromEoffice { get; set; }
        public virtual bool IsDelete { get; set; }
        public virtual bool IsLocked { get; set; }
        public virtual int OrderNumber { get; set; }
    }
}
