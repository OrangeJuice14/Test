using System;
using System.Collections.Generic;
using System.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.Helpers;
using System.Web.Http;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class MethodApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<DepartmentDTO> GetList()
        {
            var result = new List<DepartmentDTO>();
            SessionManager.DoWork(session =>
            {
                Guid spkt = new Guid("E054A602-E077-444C-B843-E856D643CA7F");
                result = session.Query<Department>().Where(d => d.ParentDepartment.Id == spkt && d.GCRecord == null).OrderBy(d => d.Name).ToList().Map<DepartmentDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int GetMaxOrderNumberMethods(Guid planKPIDetailId)
        {
            int result = 0;
            List<Method> list = new List<Method>();
            SessionManager.DoWork(session =>
            {
                list = session.Query<Method>().Where(p => p.PlanKPIDetail.Id == planKPIDetailId).ToList();
                if (list.Count > 0)
                    result = session.Query<Method>().Where(p => p.PlanKPIDetail.Id == planKPIDetailId).Max(p => p.OrderNumber);
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public bool GetCheckPlanDetailMethod(Guid planDetailId)
        {
            bool result = false;
            SessionManager.DoWork(session =>
            {
                List<MethodDTO> originalMethods = ControllerHelpers.GetOriginalMethods(planDetailId, session);
                result = originalMethods.Count() > 0 ? true : false;
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<MethodDTO> GetListByPlanDetail(Guid Id)
        {
            var result = new List<MethodDTO>();
            SessionManager.DoWork(session =>
            {
                List<Method> list = session.Query<Method>().Where(d =>d.PlanKPIDetail.Id==Id).OrderBy(d => d.OrderNumber).ToList();
                foreach(Method m in list)
                {
                    MethodDTO mdto = new MethodDTO()
                    {
                        Id = m.Id,
                        Name = m.Name,
                        OrderNumber = m.OrderNumber,
                        PlanKPIDetailId = m.PlanKPIDetail.Id,
                        StartTime = m.StartTime,
                        EndTime = m.EndTime,
                        StartTimeString = m.StartTime.ToShortDateString(),
                        EndTimeString = m.EndTime.ToShortDateString()
                    };
                    result.Add(mdto);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<MethodDTO> GetMethodDetail(Guid Id)
        {
            var result = new List<MethodDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<MethodDetail>().Where(d => d.Method.Id == Id).OrderBy(d => d.OrderNumber).ToList().Map<MethodDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public MethodDTO GetObj(Guid id)
        {
            var result = new MethodDTO();
            SessionManager.DoWork(session =>
            {
                Method method = session.Query<Method>().SingleOrDefault(a => a.Id == id);
                if(method!=null)
                {
                    result = method.Map<MethodDTO>();
                    result.PlanKPIDetailId = method.PlanKPIDetail.Id;
                }                                  
            });
            return result;
        }

        //[Authorize]
        //[Route("")]
        //public bool GetUpdatePlanDetailDic(Guid planDetailId)
        //{
        //    bool result = false;
        //    SessionManager.DoWork(session =>
        //    {
        //        PlanKPIDetail planDetail = session.Query<PlanKPIDetail>().Where(p => p.Id == planDetailId).SingleOrDefault();
        //        ControllerHelpers.UpdatePlanDetailDic(planDetail, 1, session);
        //        result = true;
        //    });
        //    return result;
        //}

        [Authorize]
        [Route("")]
        public Guid Put(Method obj)
        {
            Guid result = Guid.Empty;
            //try { 
            SessionManager.DoWork(session =>
            {
                if (obj.Id == Guid.Empty)
                {
                    obj.Id = Guid.NewGuid();
                    if (obj.PlanKPIDetail.Id == Guid.Empty)
                    {
                        //Thêm vào plandetail mới (parent)
                        PlanKPIDetail pld = new PlanKPIDetail();
                        pld.Id = Guid.NewGuid();
                        pld.PlanStaff = new PlanStaff() { Id = obj.PlanKPIDetail.PlanStaff.Id };
                        pld.TargetGroupDetail = obj.PlanKPIDetail.TargetGroupDetail;
                        pld.StartTime = DateTime.Now;
                        pld.EndTime = DateTime.Now;
                        pld.CreateTime = DateTime.Now;

                        obj.StartTime = obj.StartTime.ToLocalTime();
                        obj.EndTime = obj.EndTime.ToLocalTime();
                        obj.PlanKPIDetail = pld;
                        pld.Methods = new List<Method>();
                        pld.Methods.Add(obj);
                        session.Save(pld);
                        //obj.PlanKPIDetail = pld;
                    }
                    else
                    {                      
                        obj.StartTime = obj.StartTime.ToLocalTime();
                        obj.EndTime = obj.EndTime.ToLocalTime();
                        session.Save(obj);
                    }
                }
                else
                {
                    obj.StartTime = obj.StartTime.ToLocalTime();
                    obj.EndTime = obj.EndTime.ToLocalTime();
                    session.SaveOrUpdate(obj);
                }
                result = obj.PlanKPIDetail.Id;

                //Update Cache
                //PlanKPIDetail temp = session.Query<PlanKPIDetail>().Where(p => p.Id == result).SingleOrDefault();
                //ControllerHelpers.UpdatePlanDetailDic(temp, 1, session);
            });
            //}
            //catch (Exception e)
            //{
            //    result = Guid.Empty;
            //}
            return result;
        }

        [Authorize]
        [Route("")]
        public Method Delete(Method obj)
        {
            SessionManager.DoWork(session =>
            {
                obj = session.Query<Method>().Where(m => m.Id == obj.Id).SingleOrDefault();
                session.Delete(obj);
                //Update cache
                //ControllerHelpers.UpdatePlanDetailDic(obj.PlanKPIDetail, 2, session);
            });
            return obj;
        }
    }
}
