using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class staffApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public List<NhanVienDTO> GetListStaff(Guid StaffId)
        {
            List<NhanVienDTO> result = new List<NhanVienDTO>();
            Staff staff = new Staff();
            SessionManager.DoWork(sesision =>
            {
                staff = sesision.Query<Staff>().Where(e => e.Id == StaffId).FirstOrDefault();
            });

            SessionManager.DoWork(sesision =>
            {
                result = sesision.Query<Staff>().Where(e => e.Department.Id == staff.Department.Id && e.Id != StaffId).OrderBy(e => e.StaffProfile.FirstName).OrderBy(e => e.StaffProfile.Name).Map<NhanVienDTO>().ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public NhanVienDTO getStaffById(Guid StaffId)
        {
            NhanVienDTO result = new NhanVienDTO();
            Staff staff = new Staff();
            SessionManager.DoWork(sesision =>
            {
                result = sesision.Query<Staff>().Where(e => e.Id == StaffId).Map<NhanVienDTO>().FirstOrDefault();
            });
            return result;
        }

    }
}
