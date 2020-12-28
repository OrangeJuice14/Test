using System;
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
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;
using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.Core.DTO.AdoDataClass;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class PhanCongGiangDayApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<spd_PMS_ThoiKhoaBieu_DanhSachPhanCongGiangDay> GetThoiKhoaBieu_DanhSachPhanCongGiangDay(Guid NamHoc, Guid HocKy, Guid BoMonQuanLy)
        {  
            return DataClassHelper.spd_PMS_ThoiKhoaBieu_DanhSachPhanCongGiangDay_Web(NamHoc, HocKy, BoMonQuanLy);
        }

        [Authorize]
        [Route("")]
        public IEnumerable<DepartmentDTO> GetBoPhanQuanLy(Guid UserId)
        {
            List<DepartmentDTO> result = new List<DepartmentDTO>();
            SessionManager.DoWorkNoTransaction(session =>
            {
                WebUser User = session.Query<WebUser>().Single(e => e.Id == UserId);
                result = session.Query<Department>().Where(e => e.ParentDepartment.Id == User.StaffInfo.Staff.Department.Id).Map<DepartmentDTO>().ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<WebUserDTO> GetListUserInBoMonId(Guid boMonId)
        {
            List<WebUserDTO> result = new List<WebUserDTO>();
            SessionManager.DoWorkNoTransaction(session =>
            {
                result = session.Query<WebUser>().Where(e => e.StaffInfo.Subject.Id == boMonId).Map<WebUserDTO>().ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public int PutPhanCongGiangDay(Guid id_Chitiet, Guid giangVienId)
        {
            return DataClassHelper.spd_PMS_ThoiKhoaBieu_PhanCongGiangDay_Web(id_Chitiet, giangVienId);
        }

    }
}
