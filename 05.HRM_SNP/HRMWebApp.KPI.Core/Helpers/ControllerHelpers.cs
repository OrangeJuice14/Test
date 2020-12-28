using System;
using System.Collections.Generic;
using System.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;
using System.Web;

namespace HRMWebApp.KPI.Core.Helpers
{
    public static class ControllerHelpers
    {
        public static PlanKPIDetail GetOriginalParentPlanKPIDetail(Guid pldId)
        {
            PlanKPIDetail pld = null;
            SessionManager.DoWork(session =>
            {
                pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId);
                if (pld != null && pld.ParentPlanKPIDetail != null)
                {
                    while (pld.ParentPlanKPIDetail != null)
                    {
                        pld = pld.ParentPlanKPIDetail;
                    }
                }
            });
            return pld;
        }

        public static PlanKPIDetail GetOriginalParentPlanKPIDetail(Guid pldId,NHibernate.ISession session)
        {
            PlanKPIDetail pld = null;

                pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId);
                if (pld != null && pld.ParentPlanKPIDetail != null)
                {
                    while (pld.ParentPlanKPIDetail != null)
                    {
                        pld = pld.ParentPlanKPIDetail;
                    }
                }
         
            return pld;
        }

        public static PlanKPIDetail GetOriginalParentPlanKPIDetailTest(Guid pldId)
        {
            PlanKPIDetail pld = null;
            SessionManager.DoWork(session =>
            {
                pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId);
                if (pld!=null && pld.ParentPlanKPIDetail != null)
                    pld = GetOriginalParentPlanKPIDetailTest(pld.ParentPlanKPIDetail.Id);                
            });
            return pld;               
        }
        public static PlanKPIDetailDTO GetParentPlanKPIDetail(Guid pldId)
        {
            PlanKPIDetailDTO parentPlanDTO = new PlanKPIDetailDTO(); 
            SessionManager.DoWork(session =>
            {
                PlanKPIDetail parentPlan = null;
                PlanKPIDetail planpld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId);
                if (planpld.ParentPlanKPIDetail!=null)
                {
                    parentPlan = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == planpld.ParentPlanKPIDetail.Id);
                    parentPlanDTO.Id = parentPlan.Id;
                    parentPlanDTO.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                    foreach (PlanKPIDetail_KPI kpi in parentPlan.PlanKPIDetail_KPIs)
                    {
                        PlanKPIDetail_KPIDTO k = new PlanKPIDetail_KPIDTO();
                        k.Id = kpi.Id;
                        k.Name = kpi.Name;
                        parentPlanDTO.PlanKPIDetail_KPIs.Add(k);
                    }
                }              
            });
            return parentPlanDTO;
        }

        public static List<MethodDTO> GetOriginalMethods(Guid pldId)
        {
            List<MethodDTO> result = new List<MethodDTO>();
            PlanKPIDetail originalPld = GetOriginalParentPlanKPIDetail(pldId);
            SessionManager.DoWork(session =>
            {
                PlanKPIDetail pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == originalPld.Id);
                if (pld != null)
                {
                    foreach(Method me in pld.Methods)
                    {
                        result.Add(me.Map<MethodDTO>());
                    }
                }
            });
            return result;
        }

        public static List<MethodDTO> GetOriginalMethods(Guid pldId, NHibernate.ISession session)
        {
            List<MethodDTO> result = new List<MethodDTO>();
            PlanKPIDetail originalPld = GetOriginalParentPlanKPIDetail(pldId,session);
          
            PlanKPIDetail pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == originalPld.Id);
            if (pld != null)
            {
                foreach (Method me in pld.Methods)
                {
                    result.Add(me.Map<MethodDTO>());
                }
            }
            
            return result;
        }

        public static List<DateTime> GetOriginalMethodsStartTime(Guid pldId, NHibernate.ISession session)
        {
            List<DateTime> result = new List<DateTime>();
            PlanKPIDetail originalPld = GetOriginalParentPlanKPIDetail(pldId, session);

            PlanKPIDetail pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == originalPld.Id);
            if (pld != null)
            {
                foreach (Method me in pld.Methods)
                {
                    result.Add(me.StartTime);
                }
            }

            return result;
        }

        public static List<DateTime> GetOriginalMethodsEndTime(Guid pldId, NHibernate.ISession session)
        {
            List<DateTime> result = new List<DateTime>();
            PlanKPIDetail originalPld = GetOriginalParentPlanKPIDetail(pldId, session);

            PlanKPIDetail pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == originalPld.Id);
            if (pld != null)
            {
                foreach (Method me in pld.Methods)
                {
                    result.Add(me.EndTime);
                }
            }

            return result;
        }
        public static Staff GetCurrentStaff(NHibernate.ISession session)
        {
            Staff result = new Staff();
            // Nếu user có thông tin nhân viên
            if (AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name).Id != null)
            {
                Guid staffId = new Guid(AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name).Id);
                result = session.Query<Staff>().SingleOrDefault(s => s.Id == staffId);
            }
            //Nếu user ủy quyền không có thông tin nhân viên, mặc định gán cho trưởng đơn vị
            else
            {
                Guid departmentId = Guid.Empty;
                int agentObjectTypeId = 0;
                if (AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name).DepartmentId != null)
                {
                    departmentId = new Guid(AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name).DepartmentId);
                }
                if (AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name).AgentObjectTypeId != null)
                {
                    agentObjectTypeId = Convert.ToInt16(AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name).AgentObjectTypeId) ;
                    
                }
                result = session.Query<Staff>().SingleOrDefault(a => a.Department.Id == departmentId && a.StaffInfo.Position.AgentObjectType != null && a.StaffInfo.Position.AgentObjectType.Id == agentObjectTypeId);
            }
            //Nếu không có staff, trường hợp là admin KPIs, gắn mặc định cho 1 ng trong BGH
            if (result==null)
            {
                result = session.Query<Staff>().Where(s => s.StaffInfo.Position.AgentObjectType.Id == 4 && s.StaffProfile.GCRecord == null && s.StaffStatus.NoLongerWork == 0).FirstOrDefault();
            }
            return result;
        }
       
    }
}
