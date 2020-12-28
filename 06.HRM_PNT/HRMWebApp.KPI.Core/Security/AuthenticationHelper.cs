using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.Helpers;


namespace HRMWebApp.KPI.Core.Security
{
    public static class AuthenticationHelper
    {
        public static ApplicationUser GetUserById(Guid webUserId, string UserName)
        {
            ApplicationUser result = new ApplicationUser();
            Staff staff = new Staff();
            SessionManager.DoWork(session =>
            {

                HRMWebApp.KPI.DB.Entities.WebUser webUser = session.Query<HRMWebApp.KPI.DB.Entities.WebUser>().SingleOrDefault(w => w.Id == webUserId);
                if (webUser != null)
                {
                    if (webUser.StaffInfo != null)
                    {
                        staff = session.Query<Staff>().Where(s => s.Id == webUser.StaffInfo.Id).FirstOrDefault();
                        if (staff != null)
                        {
                            result = new ApplicationUser() { Id = staff.Id.ToString(), UserId = webUser.Id.ToString(), ThongTinNhanVien = webUser.Id.ToString(), DepartmentId = staff.Department.Id.ToString(), UserName = UserName, HoVaTen = staff.StaffProfile.Name, WebGroupId = webUser.WebGroupId.ToString() };
                            if (staff.StaffInfo.Position == null && staff.StaffInfo.StaffType.ManageCode == "3")
                                result.AgentObjectTypeId = "2";
                        }
                    }
                    else
                    {
                        if (webUser.Department != null)
                        {
                            result = new ApplicationUser() { UserId = webUser.Id.ToString(), UserName = UserName, WebGroupId = webUser.WebGroupId.ToString(), DepartmentId = webUser.Department.Id.ToString() };
                        }
                        else
                        {
                            result = new ApplicationUser() { UserId = webUser.Id.ToString(), UserName = UserName, WebGroupId = webUser.WebGroupId.ToString() };
                        }
                    }
                }
            });
            return result;
        }


    }
}
