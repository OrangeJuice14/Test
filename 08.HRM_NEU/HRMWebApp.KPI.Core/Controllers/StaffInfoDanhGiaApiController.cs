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
using System.Net.Http;
using System.Net;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class StaffInfoDanhGiaApiController : ApiController
    {

        public IEnumerable<ABC_UserDTO> GetListUserDanhGia(Guid UserId)
        {
            List<ABC_UserDTO> result = new List<ABC_UserDTO>();
            SessionManager.DoWork(session =>
            {
                WebUser user = new WebUser();
                user = session.Query<WebUser>().SingleOrDefault(e => e.Id == UserId);

                List<WebUser> list = new List<WebUser>();
                if (user.StaffInfo != null)
                    list = session.Query<WebUser>().
                                    Where(e => e.StaffInfo.Subject.Id == user.StaffInfo.Subject.Id).
                                    OrderBy(e => e.StaffInfo.StaffProfile.FirstName).
                                    OrderBy(e => e.StaffInfo.StaffProfile.LastName).
                                    ToList();
                result = list.Map<ABC_UserDTO>();
            });
            return result;
        }

        public HttpResponseMessage GetAllUser([FromUri]PagingParams param, Guid? maBoPhan)
        {
            List<ABC_UserDTO> result = new List<ABC_UserDTO>();
            List<WebUser> listUser = new List<WebUser>();
            List<WebUser> listTotalUser = new List<WebUser>();
            SessionManager.DoWork(session =>
            {
                listTotalUser = session.Query<WebUser>().Where(e => e.StaffInfo != null &&
                                                                    e.StaffInfo.StaffProfile.GCRecord == null &&
                                                                    e.StaffInfo.Staff.Department.Id == maBoPhan.Value).ToList();

                listUser = listTotalUser.OrderBy(e => e.StaffInfo.StaffProfile.FirstName).ThenBy(e => e.StaffInfo.StaffProfile.LastName).Skip(param.Skip).Take(param.Take).ToList();
                int listUserCount = listUser.Count;
                for (int i = 0; i < listUserCount; i++)
                {
                    result.Add(new ABC_UserDTO(listUser[i]));
                    List<ABC_UserGroupDanhGiaRole> ListObj = session.Query<ABC_UserGroupDanhGiaRole>().Where(e => e.WebUser.Id == listUser[i].Id && e.DeleteTime == null).ToList();
                    result[i].GroupDanhGiaName = "";
                    foreach (ABC_UserGroupDanhGiaRole obj in ListObj)
                    {
                        if (result[i].GroupDanhGiaName == "")
                            result[i].GroupDanhGiaName += obj.GroupDanhGia.Name;
                        else
                            result[i].GroupDanhGiaName += ", " + obj.GroupDanhGia.Name;
                    }
                }
            });

            var Userresult = new
            {
                Users = result,
                Total = listTotalUser.Count(),
                IndexPage = param.Page
            };
            return Request.CreateResponse(HttpStatusCode.OK, Userresult, Configuration.Formatters.JsonFormatter);
        }
        public ABC_UserDTO GetUserById(Guid userId)
        {
            ABC_UserDTO result = new ABC_UserDTO();
            SessionManager.DoWork(session =>
            {
                WebUser WebUser = session.Query<WebUser>().SingleOrDefault(e => e.Id == userId);
                result = new ABC_UserDTO(WebUser);
                
            });
            return result;
        }
    }
}
