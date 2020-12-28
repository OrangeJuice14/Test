using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.RatingKPIDTOs
{
    public class ResultDetailRatingDTO
    {
        public ResultDetailRatingDTO()
        {
            PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
            FileAttachments = new List<FileAttachmentDTO>();
            ProfessorOtherActivities = new List<ProfessorOtherActivityDTO>();
            ScienceResearches = new List<ScienceResearchDTO>();
            ProfessorOtherActivitiesResult = new List<ProfessorOtherActivityDTO>();
        }
        public Guid Id { get; set; }
        public string RegisterTarget { get; set; }        
        public double Record { get; set; }
        public double RecordOld { get; set; }
        public double RecordSecond { get; set; }
        public double SupervisorRecord { get; set; }
        public double ActivityHour { get; set; }
        public List<CriterionDictionaryDTO> CriterionDictionaries { get; set; }
        public List<ProfessorOtherActivityDTO> ProfessorOtherActivities { get; set; }
        public List<ScienceResearchDTO> ScienceResearches { get; set; }
        public List<ProfessorOtherActivityDTO> ProfessorOtherActivitiesResult { get; set; }
        public Guid CriterionId { get; set; }
        public string CriterionName { get; set; }
        public int CriterionTypeId { get; set; }
        public int FileAttachmentCount { get; set; }
        public string CurrentKPI { get; set; }
        public string CurrentKPIName { get; set; }
        public PlanKPIDetail PlanKPIDetail { get; set; }
        public TargetGroupDetail TargetGroupDetail { get; set; }
        public string TargetGroupDetailString { get; set; }
        public bool IsTargetGroupRating { get; set; }
        public bool IsConfirmed { get; set; }
        public Guid PlanKPIDetailId { get; set; }
        public Guid TargetGroupDetailId { get; set; }
        public string PlanKPIDetailName { get; set; }
        public string PlanKPIDetailNameString { get; set; }
        public string PreviousResult { get; set; }
        public string CurrentResult { get; set; }
        public double MaxRecord { get; set; }
        public double Density { get; set; }
        public double DensityPercent { get; set; }
        public string Tooltip { get; set; }
        public string Note { get; set; }
        public string SupervisorNote { get; set; }
        public List<PlanKPIDetail_KPIDTO> PlanKPIDetail_KPIs { get; set; }

        public List<FileAttachmentDTO> FileAttachments { get; set; }
    }

   
}
