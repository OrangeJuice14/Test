using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using System;
using System.Net.Http;
using System.Net;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class WebUserApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<WebUserDTO> GetListHaveStaffInfoByDepartmentId(Guid departmentId)
        {
            List<WebUserDTO> result = new List<WebUserDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<WebUser>()
                                .Where(e => e.StaffInfo != null && e.StaffInfo.Staff.Department.Id == departmentId)
                                .OrderBy(e => e.Department.Name)
                                .ThenBy(e => e.StaffInfo.StaffProfile.FirstName)
                                .ThenBy(e => e.StaffInfo.StaffProfile.Name)
                                .Map<WebUserDTO>().ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public WebUserDTO GetUserDTO(Guid userId)
        {
            WebUserDTO result = new WebUserDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<WebUser>()
                                .Single(e => e.Id == userId)
                                .Map<WebUserDTO>();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public ABC_WebUserVMDTO GetABCWebUserVMDTOByUserId(Guid userId)
        {
            ABC_WebUserVMDTO result = new ABC_WebUserVMDTO();
            SessionManager.DoWork(session =>
            {
                result = new ABC_WebUserVMDTO(session.Query<WebUser>().Single(e => e.Id == userId));
            });
            return result;
        }

        public HttpResponseMessage GetAllUser([FromUri]PagingParams param, Guid? maBoPhan)
        {
            List<ABC_WebUserVMDTO> result = new List<ABC_WebUserVMDTO>();
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
                    result.Add(new ABC_WebUserVMDTO(listUser[i]));
                    List<ABC_WebUserGroupDanhGiaRole> ListObj = session.Query<ABC_WebUserGroupDanhGiaRole>().Where(e => e.WebUser.Id == listUser[i].Id && e.DeleteTime == null).ToList();
                    result[i].GroupDanhGiaName = "";
                    foreach (ABC_WebUserGroupDanhGiaRole obj in ListObj)
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
    }
}
