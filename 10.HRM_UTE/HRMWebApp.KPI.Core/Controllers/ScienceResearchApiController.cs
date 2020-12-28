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
        [Authorize]
        [Route("")]
        public IEnumerable<ScienceResearchDTO> GetListByPlanDetail(Guid Id)
        {
            var result = new List<ScienceResearchDTO>();
            SessionManager.DoWork(session =>
            {
                List<ScienceResearch> list = session.Query<ScienceResearch>().Where(d => d.PlanKPIDetail.Id == Id).ToList();
                foreach (ScienceResearch kpi in list)
                {
                    ScienceResearchDTO k = new ScienceResearchDTO();
                    k.Id = kpi.Id;
                    k.Name = kpi.CriterionDictionary != null ? kpi.CriterionDictionary.Name : kpi.Name;
                    k.PlanKPIDetailId = kpi.PlanKPIDetail != null ? kpi.PlanKPIDetail.Id : Guid.Empty;
                    k.CriterionDictionary = kpi.CriterionDictionary != null ? new CriterionDictionaryDTO() { Id = kpi.CriterionDictionary.Id, Name = kpi.CriterionDictionary.Name } : null;
                    k.CriterionDictionaryId = kpi.CriterionDictionary != null ? kpi.CriterionDictionary.Id : Guid.Empty;
                    k.NumberOfResearch = kpi.NumberOfResearch;
                    k.NumberOfHour = kpi.CriterionDictionary != null ? kpi.CriterionDictionary.Record : kpi.NumberOfHour;
                    k.OrderNumber = kpi.CriterionDictionary != null ? kpi.CriterionDictionary.OrderNumber : 0;
                    k.IsRating = kpi.IsRating;
                    k.ExecuteMethod = kpi.ExecuteMethod;
                    k.BasicResource = kpi.BasicResource;
                    result.Add(k);
                }
                result = result.OrderBy(p => p.OrderNumber).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public ScienceResearchDTO GetObj(Guid id)
        {
            var result = new ScienceResearchDTO();
            SessionManager.DoWork(session =>
            {
                ScienceResearch sr = session.Query<ScienceResearch>().SingleOrDefault(a => a.Id == id);
                if (sr != null)
                {
                    result.Id = sr.Id;
                    result.Name = sr.Name;
                    result.PlanKPIDetailId = sr.PlanKPIDetail != null ? sr.PlanKPIDetail.Id : Guid.Empty;
                    result.CriterionDictionaryId = sr.CriterionDictionary != null ? sr.CriterionDictionary.Id : Guid.Empty;
                    result.NumberOfResearch = sr.NumberOfResearch;
                    result.NumberOfHour = sr.CriterionDictionary != null ? sr.CriterionDictionary.Record : sr.NumberOfHour;
                    result.OrderNumber = sr.OrderNumber;
                    result.IsRating = sr.IsRating;
                    result.ExecuteMethod = sr.ExecuteMethod;
                    result.BasicResource = sr.BasicResource;
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int GetMaxOrderNumberResearch(Guid planKPIDetailId)
        {
            int result = 0;
            List<ScienceResearch> list = new List<ScienceResearch>();
            SessionManager.DoWork(session =>
            {
                list = session.Query<ScienceResearch>().Where(p => p.PlanKPIDetail.Id == planKPIDetailId).ToList();
                if (list.Count > 0)
                    result = session.Query<ScienceResearch>().Where(p => p.PlanKPIDetail.Id == planKPIDetailId).Max(p => p.OrderNumber);
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int Put(ScienceResearchDTO obj)
        {
            int result = 0;
            //try
            //{
                SessionManager.DoWork(session =>
                {
                    if (obj.CriterionDictionaryId == Guid.Empty) //các hoạt động khác (ko có CriterionDictionary)
                    {
                        ScienceResearch pa = new ScienceResearch();
                        pa.Id = Guid.NewGuid();
                        pa.Name = obj.Name;
                        pa.NumberOfResearch = obj.NumberOfResearch;
                        pa.NumberOfHour = obj.NumberOfHour;
                        pa.OrderNumber = obj.OrderNumber;
                        pa.IsRating = obj.IsRating;
                        pa.PlanKPIDetail = new PlanKPIDetail() { Id = obj.PlanKPIDetailId };
                        pa.CriterionDictionary = null;
                        pa.ExecuteMethod = obj.ExecuteMethod;
                        pa.BasicResource = obj.BasicResource;

                        if (obj.Id == Guid.Empty) //thêm mới
                        {
                            pa.Id = Guid.NewGuid();
                            session.Save(pa);
                        }
                        else //cập nhật
                        {
                            pa.Id = obj.Id;
                            session.Update(pa);
                        }
                    }
                    else
                    {
                        ScienceResearch pa = new ScienceResearch();
                        pa.Id = obj.Id;
                        pa.Name = obj.Name;
                        pa.NumberOfResearch = obj.NumberOfResearch;
                        pa.IsRating = obj.IsRating;
                        pa.PlanKPIDetail = new PlanKPIDetail() { Id = obj.PlanKPIDetailId };
                        pa.CriterionDictionary = new CriterionDictionary() { Id = obj.CriterionDictionaryId };
                        pa.ExecuteMethod = obj.ExecuteMethod;
                        pa.BasicResource = obj.BasicResource;
                        session.Update(pa);
                    }
                    result = 1;
                });
            //}
            //catch (Exception e)
            //{
            //    result = 0;
            //}
            return result;
        }

        [Authorize]
        [Route("")]
        public ScienceResearch Delete(ScienceResearch obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }
    }
}
