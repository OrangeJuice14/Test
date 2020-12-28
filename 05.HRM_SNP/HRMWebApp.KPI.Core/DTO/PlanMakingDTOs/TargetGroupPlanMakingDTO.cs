using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB.Entities;

namespace HRMWebApp.KPI.Core.DTO.PlanMakingDTOs
{
    public class TargetGroupPlanMakingDTO
    {
        public TargetGroupPlanMakingDTO()
        {
            PlanKPIDetails = new List<PlanKPIMakingDetailDTO>();
        }
        public Guid TargetGroupId { get; set; }
        public string TargetGroupName { get; set; }
        public double Density { get; set; }
        public int TargetGroupDetailTypeId { get; set; }
        public List<PlanKPIMakingDetailDTO> PlanKPIDetails { get; set; }
        public List<CriterionPlanDTO> Criterions { get; set; }
        public List<ProfessorCriterionPlanDTO> ProfessorCriterions { get; set; }
        public List<CriterionDictionaryDTO> CriterionDictionaries { get; set; }
    }

    public class CriterionPlanDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Density { get; set; }
        public int OrderNumber { get; set; }
        public CriterionType CriterionType { get; set; }
        public string ServiceUrl { get; set; }
        public string Tooltip { get; set; }
    }
    public class ProfessorCriterionPlanDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Density { get; set; }
        public double Record { get; set; }
        public int OrderNumber { get; set; }
        public CriterionType CriterionType { get; set; }
        public string Tooltip { get; set; }
    }

    public class PlanKPIMakingDetailDTO
    {
        public PlanKPIMakingDetailDTO()
        {
            SubDepartmentIds = new List<Guid>();
            SubjectIds= new List<Guid>();
            SubjectNames = new List<String>();
            SubStaffIds = new List<Guid>();
            SubDepartmentNames = new List<String>();
            SubStaffNames = new List<String>();
            Methods = new List<MethodDTO>();
            PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
            ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
            PlanFileDTOs = new List<FileAttachmentDTO>();
            ProfessorOtherActivities = new List<ProfessorOtherActivityDTO>();
            ScienceResearches = new List<ScienceResearchDTO>();
            ScienceResearchIds = new List<Guid>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<MethodDTO> Methods { get; set; }
        public List<ProfessorOtherActivityDTO> ProfessorOtherActivities { get; set; }
        public List<ScienceResearchDTO> ScienceResearches { get; set; }
        public List<FileAttachmentDTO> PlanFileDTOs { get; set; }
        public List<PlanKPIDetail_KPIDTO> PlanKPIDetail_KPIs { get; set; }
        public List<PlanKPIDetail_KPIDTO> ParentPlanKPIDetail_KPIs { get; set; }
        public string BasicResource { get; set; }
        public string PreviousKPI { get; set; }
        public string CurrentKPI { get; set; }
        public string CurrentKPISecond { get; set; }
        public string PreviousKPISecond { get; set; }
        public DateTime StartTime { get; set; }
        public string StartDateString { get; set; }
        public DateTime EndTime { get; set; }
        public string EndTimeString { get; set; }
        public Guid CriterionId { get; set; }
        public CriterionDTO FromCriterion { get; set; }
        public ProfessorCriterionDTO FromProfessorCriterion { get; set; }
        public Guid FromCriterionId { get; set; }
        public Guid FromProfessorCriterionId { get; set; }
        public DepartmentDTO LeadDepartment { get; set; }
        public Guid LeadDepartmentId { get; set; }
        public string LeadDepartmentName { get; set; }
        public Guid AdminLeaderId { get; set; }
        public string AdminLeaderName { get; set; }
        public StaffDTO StaffLeader { get; set; }
        public Guid StaffLeaderId { get; set; }
        public string CriterionName { get; set; }
        public int OrderNumber { get; set; }
        public string ExecuteMethod { get; set; }
        public string TargetDetail { get; set; }
        public string TargetDetailName { get; set; }
        public TargetGroupDetailDTO TargetGroupDetail { get; set; }
        public double MaxRecord { get; set; }
        public double Record { get; set; }
        public int CriterionTypeId { get; set; }
        public MeasureUnitDTO  MeasureUnitDTO{ get; set; }
        public List<Guid> SubDepartmentIds { get; set; }
        public List<String> SubDepartmentNames { get; set; }
        public List<Guid> SubjectIds { get; set; }
        public List<String> SubjectNames { get; set; }
        public List<Guid> ActivityIds { get; set; }
        public List<Guid> ScienceResearchIds { get; set; }
        public List<String> ActivityNames { get; set; }
        public List<Guid> SubStaffIds { get; set; }
        public List<String> SubStaffNames { get; set; }
        public string CriterionDictionaryName { get; set; }
        public List<CriterionDictionaryDTO> CriterionDictionaries { get; set; }
        public string Tooltip { get; set; }
        public bool CanDelete { get; set; }
        public bool IsAddition { get; set; }
        public bool IsDisable { get; set; }
        public bool IsLockable { get; set; }
        public Guid ParentPlanKPIDetailId { get; set; }
        public Guid PlanStaffId { get; set; }
        public Guid StaffId { get; set; }
        public Guid PlanId { get; set; }
        public DateTime CreateTime { get; set; }
        public ManageCode ManageCode { get; set; }
        public string ManageId { get; set; }
    }
}
