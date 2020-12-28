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
                WebUser webUser = session.Query<WebUser>().SingleOrDefault(w => w.Id == webUserId);
                if (webUser != null)
                {
                    if (webUser.StaffInfo != null)
                    {
                        staff = session.Query<Staff>().Where(s => s.Id == webUser.StaffInfo.Id).FirstOrDefault();
                        if (staff != null)
                        {
                            result = new ApplicationUser() { Id = staff.Id.ToString(), UserId = webUser.Id.ToString(), ThongTinNhanVien = webUser.Id.ToString(), DepartmentId = staff.Department.Id.ToString(), UserName = UserName ?? webUser.UserName, HoVaTen = staff.StaffProfile.Name, WebGroupId = webUser.WebGroupId.ToString() };
                            if (staff.StaffInfo.Position == null && staff.StaffInfo.StaffType?.ManageCode == "3")
                                result.AgentObjectTypeId = "2";
                        }
                    }
                    else if (webUser.GiangVienThinhGiang != null)
                    {
                        staff = session.Query<Staff>().Where(s => s.Id == webUser.GiangVienThinhGiang.Id).FirstOrDefault();
                        if (staff != null)
                        {
                            result = new ApplicationUser() { Id = staff.Id.ToString(), UserId = webUser.Id.ToString(), ThongTinNhanVien = webUser.Id.ToString(), DepartmentId = staff.Department?.Id.ToString(), UserName = UserName ?? webUser.UserName, HoVaTen = staff.StaffProfile.Name, WebGroupId = webUser.WebGroupId.ToString() };
                        }
                    }
                    else
                    {
                        if (webUser.AgentObjectType == null)
                        {
                            webUser.AgentObjectType = new AgentObjectType() { Id = 0 };
                        }
                        if (webUser.Department != null)
                        {
                            result = new ApplicationUser() { UserId = webUser.Id.ToString(), UserName = UserName ?? webUser.UserName, WebGroupId = webUser.WebGroupId.ToString(), AgentObjectTypeId = webUser.AgentObjectType.Id.ToString(), DepartmentId = webUser.Department.Id.ToString() };
                        }
                        else
                        {
                            result = new ApplicationUser() { UserId = webUser.Id.ToString(), UserName = UserName ?? webUser.UserName, WebGroupId = webUser.WebGroupId.ToString(), AgentObjectTypeId = webUser.AgentObjectType.Id.ToString() };
                        }
                    }
                }
            });
            return result;
        }

        public static bool IsAllowPlanMaking(Guid currentUserId, Guid normalstaffId, NHibernate.ISession session)
        {
            bool result = false;


            //SessionManager.DoWork(session =>
            //{
            Staff supervisor = ControllerHelpers.GetCurrentStaff(session);
            Staff normalStaff = session.Query<Staff>().SingleOrDefault(s => s.Id == normalstaffId);
            if (supervisor.Id != normalStaff.Id)
            {
                if (supervisor != null)
                {
                    if (supervisor.StaffInfo.Position != null)
                    {
                        switch (supervisor.StaffInfo.Position.AgentObjectType.Id)
                        {
                            case (int)AgentObjectTypes.PhongBan:
                                {
                                    if ((normalStaff.StaffInfo.Position == null || normalStaff.StaffInfo.Position.AgentObjectType.Id == 7) && normalStaff.Department.Id == supervisor.Department.Id)
                                        result = true;
                                }
                                break;
                            case (int)AgentObjectTypes.Khoa:
                                {
                                    if ((normalStaff.StaffInfo.Position == null || normalStaff.StaffInfo.Position.AgentObjectType.Id == 8) && normalStaff.Department.Id == supervisor.Department.Id)
                                        result = true;
                                }
                                break;
                            case (int)AgentObjectTypes.HieuTruong:
                                {
                                    if (normalStaff.StaffInfo.Position != null && normalStaff.StaffInfo.Position.AgentObjectType.Id == 11 && normalStaff.Department.Id == supervisor.Department.Id)
                                        result = true;
                                    else if (normalStaff.StaffInfo.Position != null)
                                        result = true;
                                }
                                break;
                        }
                    }
                }
            }
            else
                return true;

            return result;
        }


        public static bool IsAllowPlanMaking(Guid currentUserId, Guid normalstaffId)
        {
            bool result = false;

            SessionManager.DoWork(session =>
            {

                Staff supervisor = ControllerHelpers.GetCurrentStaff(session);
                Staff normalStaff = session.Query<Staff>().SingleOrDefault(s => s.Id == normalstaffId);
                if (supervisor != null)
                {
                    if (normalStaff !=null && supervisor.Id != normalStaff.Id )
                    {
                        if (supervisor.StaffInfo.Position != null)
                        {
                            switch (supervisor.StaffInfo.Position.AgentObjectType.Id)
                            {
                                case (int)AgentObjectTypes.PhoHieuTruong:
                                    {
                                        if ((normalStaff.StaffInfo.Position.AgentObjectType.Id == 3 || normalStaff.StaffInfo.Position.AgentObjectType.Id == 5))
                                            result = true;
                                    }
                                    break;
                                case (int)AgentObjectTypes.BoMon:
                                    {
                                        if ((normalStaff.StaffInfo.Position == null || normalStaff.StaffInfo.Position.AgentObjectType.Id == 9) && normalStaff.Department.Id == supervisor.Department.Id)
                                            result = true;
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhongBan:
                                    {
                                        if ((normalStaff.StaffInfo.Position == null || normalStaff.StaffInfo.Position.AgentObjectType.Id == 7) && normalStaff.Department.Id == supervisor.Department.Id)
                                            result = true;
                                    }
                                    break;
                                case (int)AgentObjectTypes.Khoa:
                                    {
                                        if ((normalStaff.StaffInfo.Position == null || normalStaff.StaffInfo.Position.AgentObjectType.Id == 8) && normalStaff.Department.Id == supervisor.Department.Id)
                                            result = true;
                                    }
                                    break;
                                case (int)AgentObjectTypes.HieuTruong:
                                    {
                                        if (normalStaff.StaffInfo.Position != null && (normalStaff.StaffInfo.Position.AgentObjectType.Id == 11 || normalStaff.StaffInfo.Position.AgentObjectType.Id == 5 || normalStaff.StaffInfo.Position.AgentObjectType.Id == 3))
                                            result = true;
                                        else if (normalStaff.StaffInfo.Position != null)
                                            result = true;
                                    }
                                    break;
                            }
                        }
                    }
                    else
                        result= true;

                }
            });


            return result;
        }
    }
}
