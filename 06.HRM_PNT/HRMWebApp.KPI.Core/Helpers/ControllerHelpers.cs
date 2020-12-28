using System;
using System.Collections.Generic;
using System.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Mail;
using System.Web.Configuration;

namespace HRMWebApp.KPI.Core.Helpers
{

    public static class ControllerHelpers
    {
       public static Staff GetCurrentStaff(NHibernate.ISession session)
        {
            Staff result = new Staff();
            // Nếu user có thông tin nhân viên
            ApplicationUser currentUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
            if (currentUser.Id != null)
            {
                Guid staffId = new Guid(AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name).Id);
                result = session.Query<Staff>().SingleOrDefault(s => s.Id == staffId);
                //if (currentUser.AgentObjectTypeId == "2")
                //{

                //    result.StaffInfo.Position = new Position() { AgentObjectType = new AgentObjectType() { Id = 2 } };
                //}
                //if (currentUser.IsKPIs)
                //    result.StaffInfo.Position.AgentObjectType = new AgentObjectType() { Id = Convert.ToInt32(currentUser.AgentObjectTypeId) };
                //SessionHelper.Data(SessionKey.IsKPIs, true);
                //else
                //    SessionHelper.Data(SessionKey.IsKPIs, false);
            }
            //Nếu user ủy quyền không có thông tin nhân viên, mặc định gán cho trưởng đơn vị
            else
            {
                Guid departmentId = Guid.Empty;
                int agentObjectTypeId = 0;
                if (currentUser.DepartmentId != null)
                {
                    departmentId = new Guid(AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name).DepartmentId);
                }
                if (currentUser.AgentObjectTypeId != null)
                {
                    agentObjectTypeId = Convert.ToInt16(AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name).AgentObjectTypeId);

                }
                if (!currentUser.IsKPIs)
                    result = session.Query<Staff>().SingleOrDefault(a => a.Department.Id == departmentId);
                else
                    result = new Staff()
                    {
                        Id = Guid.Empty,
                        Department = new Department() { Id = departmentId }
                    };
            }
            return result;
        }
    } 
}
