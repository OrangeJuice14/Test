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
        [Authorize]
        [Route("")]
        public IEnumerable<ProfessorOtherActivityDTO> GetListByPlanDetail(Guid Id)
        {
            var result = new List<ProfessorOtherActivityDTO>();
            SessionManager.DoWork(session =>
            {
                List<ProfessorOtherActivity> list = session.Query<ProfessorOtherActivity>().Where(d => d.PlanKPIDetail.Id == Id).ToList();
                foreach (ProfessorOtherActivity kpi in list)
                {
                    ProfessorOtherActivityDTO k = new ProfessorOtherActivityDTO();
                    k.Id = kpi.Id;
                    k.PlanKPIDetailId = kpi.PlanKPIDetail != null ? kpi.PlanKPIDetail.Id : Guid.Empty;
                    if (kpi.CriterionDictionary != null) // hoạt động 1,2,3,4 (có từ điển)
                    {
                        k.Name = kpi.CriterionDictionary.Name;
                        k.CriterionDictionary = new CriterionDictionaryDTO() { Id = kpi.CriterionDictionary.Id, Name = kpi.CriterionDictionary.Name };
                        k.CriterionDictionaryId = kpi.CriterionDictionary.Id;
                        k.NumberOfHour = kpi.NumberOfHour;
                        k.OrderNumber = kpi.CriterionDictionary.OrderNumber;
                    }
                    else // hoạt động khác
                    {
                        k.Name = kpi.Name;
                        k.NumberOfHour = kpi.NumberOfHour;
                        k.OrderNumber = kpi.OrderNumber;
                    }
                    k.NumberOfTime = kpi.NumberOfTime;
                    k.IsRating = kpi.IsRating;
                    k.ExecuteMethod = kpi.ExecuteMethod;
                    k.BasicResource = kpi.BasicResource;
                    k.IsAssign = kpi.IsAssign;
                    result.Add(k);
                }
                result = result.OrderBy(p => p.OrderNumber).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
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
                    result.CriterionDictionaryId = kpi.CriterionDictionary != null ? kpi.CriterionDictionary.Id : Guid.Empty;
                    result.PlanKPIDetailId = kpi.PlanKPIDetail != null ? kpi.PlanKPIDetail.Id : Guid.Empty;
                    result.Name = kpi.Name;
                    result.OrderNumber = kpi.OrderNumber;
                    result.NumberOfHour = kpi.NumberOfHour;
                    result.IsRating = kpi.IsRating;
                    result.ExecuteMethod = kpi.ExecuteMethod;
                    result.BasicResource = kpi.BasicResource;
                    result.IsAssign = kpi.IsAssign;
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int GetMaxOrderNumberActivity(Guid planKPIDetailId) 
        {
            int result = 0;
            List<ProfessorOtherActivity> list = new List<ProfessorOtherActivity>();
            SessionManager.DoWork(session =>
            {
                list = session.Query<ProfessorOtherActivity>().Where(p => p.PlanKPIDetail.Id == planKPIDetailId).ToList();
                if (list.Count > 0)
                    result = session.Query<ProfessorOtherActivity>().Where(p => p.PlanKPIDetail.Id == planKPIDetailId).Max(p => p.OrderNumber);
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int Put(ProfessorOtherActivityDTO obj)
        {
            int result = 0;
            //try
            //{
                SessionManager.DoWork(session =>
                {
                    if (obj.CriterionDictionaryId == Guid.Empty) //các hoạt động khác (ko có CriterionDictionary)
                    {
                        ProfessorOtherActivity pa = new ProfessorOtherActivity();
                        pa.NumberOfTime = obj.NumberOfTime;
                        pa.NumberOfHour = obj.NumberOfHour;
                        pa.Name = obj.Name;
                        pa.OrderNumber = obj.OrderNumber;
                        pa.IsRating = obj.IsRating;
                        pa.PlanKPIDetail = new PlanKPIDetail() { Id = obj.PlanKPIDetailId };
                        pa.CriterionDictionary = null;
                        pa.ExecuteMethod = obj.ExecuteMethod;
                        pa.BasicResource = obj.BasicResource;
                        pa.IsAssign = obj.IsAssign;

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
                    else //có CriterionDictionary
                    {
                        ProfessorOtherActivity pa = new ProfessorOtherActivity();
                        pa.Id = obj.Id;
                        pa.NumberOfTime = obj.NumberOfTime;
                        pa.IsRating = obj.IsRating;
                        pa.ExecuteMethod = obj.ExecuteMethod;
                        pa.BasicResource = obj.BasicResource;
                        pa.PlanKPIDetail = new PlanKPIDetail() { Id = obj.PlanKPIDetailId };
                        pa.CriterionDictionary = new CriterionDictionary() { Id = obj.CriterionDictionaryId };
                        pa.NumberOfHour = obj.NumberOfHour;
                        pa.IsAssign = obj.IsAssign;
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
        public ProfessorOtherActivity Delete(ProfessorOtherActivity obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }
    }
}
