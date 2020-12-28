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

namespace HRMWebApp.KPI.Core.Controllers
{
    public class PhanQuyenBoPhanDuToanApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<DepartmentDTO> GetPhanQuyenBoPhanDuToanByUserId(Guid userId)
        {
            List<DepartmentDTO> result = new List<DepartmentDTO>();
            SessionManager.DoWork(session =>
            {
                if(userId != Guid.Empty)
                {
                    List<DuToanNoiBo_BoPhanUserRole> List = session.Query<DuToanNoiBo_BoPhanUserRole>()
                                    .Where(e => e.WebUser.Id == userId).ToList();
                    if(List != null)
                        result = List.Select(e => e.Department)
                                     .Map<DepartmentDTO>().ToList();
                }
            });
            return result;
        }
        
        [Authorize]
        [Route("")]
        public int PostSave(List<Guid> listDonViSelected,Guid userId)
        {
            int result = 0;
            try
            {
                SessionManager.DoWork(session =>
                {
                    List<DuToanNoiBo_BoPhanUserRole> ListDelete = session.Query<DuToanNoiBo_BoPhanUserRole>().Where(e => e.WebUser.Id == userId).ToList();
                    foreach (DuToanNoiBo_BoPhanUserRole ItemDelete in ListDelete)
                    {
                        session.Delete(ItemDelete);
                    }

                    foreach (Guid DepartmentId in listDonViSelected)
                    {
                        DuToanNoiBo_BoPhanUserRole ObjSave = new DuToanNoiBo_BoPhanUserRole()
                        {
                            Id = Guid.NewGuid(),
                            Department = new Department() { Id = DepartmentId },
                            WebUser = new WebUser() { Id = userId }
                        };
                        session.Save(ObjSave);
                    }
                });
                result = 1;
            }
            catch(Exception ex)
            {
                result = 0;
                Helper.ErrorLog("PhanQuyenBoPhanDuToanApi/PostSave", ex);
                throw ex;
            }

            return result;
        }
    }
}
