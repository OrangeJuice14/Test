using System;
using System.Collections.Generic;
using System.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;
using HRMWebApp.KPI.Core.Helpers;
using System.Web;
using HRMWeb_Business.Predefined;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class StaffApiController : ApiController
    {
        public StaffDTO ParseStaff(Staff staff)
        {
            StaffDTO sd = new StaffDTO();
            sd.Id = staff.Id;
            sd.Name = staff.StaffProfile.Name;
            //sd.ManageCode = staff.StaffInfo.ManageCode;
            sd.DepartmentName = staff.Department.Name;
            sd.PositionName = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.Name : "";
            return sd;
        }

        [Authorize]
        [Route("")]
        public StaffDTO GetObj(Guid id)
        {
            var result = new StaffDTO();
            SessionManager.DoWork(session =>
            {
                Staff staff = session.Query<Staff>().SingleOrDefault(a => a.Id == id);
                result = staff.Map<StaffDTO>();
                result.DepartmentId = result.Department.Id;
                result.DepartmentIds = new List<Guid>();
                foreach (var a in staff.Departments)
                {
                    result.DepartmentIds.Add(a.Id);
                }
                //if (staff != null)
                //    foreach (var agentObject in staff.StaffInfo.AgentObjects)
                //    {
                //        result.AgentObjectIds.Add(agentObject.Id);
                //    }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<StaffDTO> GetListAdminLeader(Guid congTyId)
        {
            List<StaffDTO> result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                List<Staff> templist = new List<Staff>();
                templist = session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.Company.Id == congTyId && s.StaffInfo.Position.WebGroup.Id == WebGroupConst.HieuTruongID).ToList();
                foreach (Staff s in templist)
                {
                    StaffDTO sd = new StaffDTO();
                    sd = ParseStaff(s);
                    result.Add(sd);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public Guid GetCurrentUserGroupId()
        {
            Guid groupId = Guid.Empty;
            SessionManager.DoWork(session =>
            {
                groupId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId);
            });
            return groupId;
        }
        
        [Authorize]
        [Route("")]
        public int PutDepartmentManage(StaffDTO obj)
        {
            Staff staff = new Staff();
            int result = 0;
            try
            {
                SessionManager.DoWork(session =>
                {
                    staff = session.Query<Staff>().SingleOrDefault(a => a.Id == obj.Id);
                    staff.Departments = new List<Department>();
                    obj.DepartmentIds.Remove(new Guid("00000000-0000-0000-0000-000000000001"));
                    if (obj.DepartmentIds.Count == 0)
                    {
                        result = 1;
                    }
                    else
                    {
                        foreach (var id in obj.DepartmentIds)
                        {
                            Department dept = new Department { Id = id };
                            //bool check = session.Query<Staff>().Any(s => s.Id != obj.Id && s.Departments.Contains(dept));
                            //if (check)
                            //{
                            //    result = 2;
                            //    break;
                            //}
                            //else
                            //{
                                staff.Departments.Add(dept);
                                session.Update(staff);
                                result = 1;
                            //}

                        }
                    }
                });
            }
            catch (Exception e)
            {
                result = 0;
            }         
            return result;
        }
    }
}
