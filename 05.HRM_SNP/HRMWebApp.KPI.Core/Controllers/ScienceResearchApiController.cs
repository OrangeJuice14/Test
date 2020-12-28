using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using System.Web.Http;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ScienceResearchApiController : ApiController
    {
        public IEnumerable<ScienceResearchDTO> GetListByPlanDetail(Guid Id)
        {
            var result = new List<ScienceResearchDTO>();
            SessionManager.DoWork(session =>
            {
                List<ScienceResearch> list = session.Query<ScienceResearch>().Where(d => d.PlanKPIDetail.Id == Id).OrderBy(d => d.CriterionDictionary.NumberOfHour).ToList();
                foreach (ScienceResearch kpi in list)
                {
                    ScienceResearchDTO k = new ScienceResearchDTO();
                    k.Id = kpi.Id;
                    k.Name = kpi.CriterionDictionary !=null? kpi.CriterionDictionary.Name:"";
                    k.PlanKPIDetailId = kpi.PlanKPIDetail != null ? kpi.PlanKPIDetail.Id : Guid.Empty;
                    k.CriterionDictionary = kpi.CriterionDictionary != null ? new CriterionDictionaryDTO() { Id = kpi.CriterionDictionary.Id, Name = kpi.CriterionDictionary.Name } : null;
                    k.CriterionDictionaryId = kpi.CriterionDictionary != null ? kpi.CriterionDictionary.Id : Guid.Empty;
                    k.NumberOfResearch = kpi.NumberOfResearch;
                    result.Add(k);
                }
            });
            return result;
        }

        public ScienceResearchDTO GetObj(Guid id)
        {
            var result = new ScienceResearchDTO();
            SessionManager.DoWork(session =>
            {
                ScienceResearch sr = session.Query<ScienceResearch>().SingleOrDefault(a => a.Id == id);
                if (sr != null)
                {
                    result.Id = sr.Id;
                    result.Name = sr.CriterionDictionary.Name;
                    result.PlanKPIDetailId = sr.PlanKPIDetail != null ? sr.PlanKPIDetail.Id : Guid.Empty;
                    result.CriterionDictionaryId = sr.CriterionDictionary.Id;
                    result.NumberOfResearch = sr.NumberOfResearch;
                    
                }
            });
            return result;
        }

        public int Put(ScienceResearchDTO obj)
        {
            int result = 0;
            //try
            //{
            SessionManager.DoWork(session =>
            {
                ScienceResearch pa = new ScienceResearch();
                pa.Id = obj.Id;
                pa.Name = obj.Name;
                pa.NumberOfResearch = obj.NumberOfResearch;
                pa.PlanKPIDetail = new PlanKPIDetail() { Id = obj.PlanKPIDetailId };
                pa.CriterionDictionary = new CriterionDictionary() { Id = obj.CriterionDictionaryId };
                session.Update(pa);
                result = 1;
            });
            //}
            //catch (Exception e)
            //{
            //    result = 0;
            //}
            return result;
        }

        public ScienceResearch Delete(ScienceResearch obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }
    }
}
