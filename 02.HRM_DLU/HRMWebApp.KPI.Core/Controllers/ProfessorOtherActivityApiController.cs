using System;
using System.Collections.Generic;
using System.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ProfessorOtherActivityApiController : ApiController
    {
        public IEnumerable<ProfessorOtherActivityDTO> GetListByPlanDetail(Guid Id)
        {
            var result = new List<ProfessorOtherActivityDTO>();
            SessionManager.DoWork(session =>
            {
                List<ProfessorOtherActivity> list = session.Query<ProfessorOtherActivity>().Where(d => d.PlanKPIDetail.Id == Id).OrderBy(d => d.CriterionDictionary.NumberOfHour).ToList();
                foreach (ProfessorOtherActivity kpi in list)
                {
                    ProfessorOtherActivityDTO k = new ProfessorOtherActivityDTO();
                    k.Id = kpi.Id;
                    k.PlanKPIDetailId = kpi.PlanKPIDetail != null ? kpi.PlanKPIDetail.Id : Guid.Empty;
                    if (kpi.CriterionDictionary != null)
                    {
                        k.Name = kpi.CriterionDictionary.Name;
                        k.CriterionDictionary = new CriterionDictionaryDTO() { Id = kpi.CriterionDictionary.Id, Name = kpi.CriterionDictionary.Name };
                        k.CriterionDictionaryId = kpi.CriterionDictionary.Id;
                        k.NumberOfHour = kpi.CriterionDictionary.NumberOfHour;
                    }
                    k.NumberOfTime = kpi.NumberOfTime;
                    result.Add(k);
                }
            });
            return result;
        }

        public ProfessorOtherActivityDTO GetObj(Guid id)
        {
            var result = new ProfessorOtherActivityDTO();
            SessionManager.DoWork(session =>
            {
                ProfessorOtherActivity kpi = session.Query<ProfessorOtherActivity>().SingleOrDefault(a => a.Id == id);
                if (kpi != null)
                {
                    result.Id = kpi.Id;
                    result.NumberOfTime = kpi.NumberOfTime;
                    result.CriterionDictionaryId = kpi.CriterionDictionary.Id;
                    result.PlanKPIDetailId = kpi.PlanKPIDetail.Id;
                }
            });
            return result;
        }

        public int Put(ProfessorOtherActivityDTO obj)
        {
            int result = 0;
            //try
            //{
                SessionManager.DoWork(session =>
                {
                    ProfessorOtherActivity pa = new ProfessorOtherActivity();
                    pa.Id = obj.Id;
                    pa.NumberOfTime = obj.NumberOfTime;
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

        public ProfessorOtherActivity Delete(ProfessorOtherActivity obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }
    }
}
