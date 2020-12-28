using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_QuanLyDonViApiController : ApiController
    {
        public IEnumerable<ABC_QuanLyDonVi> GetListByUserId(ISession session,Guid userId)
        {
            return session.Query<ABC_QuanLyDonVi>().Where(e => e.User.Id == userId);
        }

        [Authorize]
        [Route("")]
        public IEnumerable<Guid> GetListQuanLyDonViIdByUserId(Guid userId)
        {
            List<Guid> result = new List<Guid>();
            SessionManager.DoWork(session =>
            {
                result = GetListByUserId(session,userId).Select(e => e.Department.Id).ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public bool GetCheckQLDV(Guid userId, Guid departmentId)
        {
            bool result = false;
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_QuanLyDonVi QLDV = session.Query<ABC_QuanLyDonVi>().SingleOrDefault(e => e.User.Id == userId && e.Department.Id == departmentId);
                    if (QLDV != null)
                    {
                        result = true;
                    }
                });
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                Helper.ErrorLog("ABC_QuanLyDonViApi/GetCheckQLDV ", ex); throw ex;
            }
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_UserQuanLyDonViVMDTO> GetListQuanLyDonViByUserId(Guid userId)
        {
            List<ABC_UserQuanLyDonViVMDTO> result = new List<ABC_UserQuanLyDonViVMDTO>();
            SessionManager.DoWork(session =>
            {
                result = GetListByUserId(session, userId).Map<ABC_UserQuanLyDonViVMDTO>().ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public string Put(List<Guid> listDepartment, Guid userId)
        {
            string result = "";
            try
            {
                SessionManager.DoWork(session =>
                {
                    List<ABC_QuanLyDonVi> ListDel = GetListByUserId(session, userId).ToList();
                    foreach (ABC_QuanLyDonVi ObjDel in ListDel)
                    {
                        session.Delete(ObjDel);
                    }

                    foreach (Guid DepartmentId in listDepartment)
                    {
                        ABC_QuanLyDonVi ObjSave = new ABC_QuanLyDonVi();
                        ObjSave.Id = Guid.NewGuid();
                        ObjSave.Department = new Department() { Id = DepartmentId };
                        ObjSave.User = new WebUser() { Id = userId };
                        session.Save(ObjSave);
                    }
                });

            }
            catch (Exception ex)
            {
                result = "ERRORS";
                Helper.ErrorLog("ABC_QuanLyDonViApi/Put ", ex); throw ex;
            }
            return result;

        }
    }
}
