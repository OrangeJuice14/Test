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
    public class ABC_UserGroupDanhGiaRoleApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_UserGroupDanhGiaRoleDTO> GetAll()
        {
            List<ABC_UserGroupDanhGiaRoleDTO> result = new List<ABC_UserGroupDanhGiaRoleDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_UserGroupDanhGiaRole>().Where(e => e.DeleteTime == null).Map<ABC_UserGroupDanhGiaRoleDTO>().ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_UserDTO> GetListUserDanhGia(Guid boTieuChiId)
        {
            List<ABC_UserDTO> result = new List<ABC_UserDTO>();
            ABC_BoTieuChi_Role ObjBoTieuChiRole = new ABC_BoTieuChi_Role();
            SessionManager.DoWork(session =>
            {
                ABC_GroupDanhGia GroupDanhGia = session.Query<ABC_BoTieuChi_Role>().
                                                        Where(e => e.BoTieuChi.Id == boTieuChiId).
                                                        Select(e => e.GroupTuDanhGia).
                                                        Distinct().
                                                        SingleOrDefault();

                result = session.Query<ABC_User>().
                                Where(e => e.GroupDanhGia.Id == GroupDanhGia.Id && e.DeleteTime == null).
                                Map<ABC_UserDTO>().
                                ToList();

            });
            return result;
        }
        //[Authorize]
        //[Route("")]
        //public ABC_UserDTO GetUserDanhGiaByUserId(Guid userId)
        //{
        //    ABC_UserDTO result = new ABC_UserDTO();
        //    SessionManager.DoWork(session =>
        //    {
        //        result = session.Query<ABC_User>().SingleOrDefault(e => e.Id == userId).Map<ABC_UserDTO>();
        //    });
        //    return result;
        //}
        [Authorize]
        [Route("")]
        public ABC_UserGroupDanhGiaRoleDTO GetByUserId(Guid userId)
        {
            ABC_UserGroupDanhGiaRoleDTO result = new ABC_UserGroupDanhGiaRoleDTO();
            
            SessionManager.DoWork(session =>
            {
                List<ABC_UserGroupDanhGiaRole> list = session.Query<ABC_UserGroupDanhGiaRole>().
                                                                Where(e => e.WebUser.Id == userId && e.DeleteTime == null).
                                                                ToList();
                if (list.Count != 0)
                {
                    result = list[0].Map<ABC_UserGroupDanhGiaRoleDTO>();
                    result.ListGroupDanhGiaId = new List<Guid>();
                    result.ListGroupDanhGia = new List<ABC_GroupDanhGiaDTO>();
                    foreach (ABC_UserGroupDanhGiaRole item in list)
                    {
                        result.ListGroupDanhGiaId.Add(item.GroupDanhGia.Id);
                        result.ListGroupDanhGia.Add(item.GroupDanhGia.Map<ABC_GroupDanhGiaDTO>());
                    }
                }
                else
                {
                    result = null;
                }
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public string PutSaveOrUpdate(ABC_UserGroupDanhGiaRoleDTO obj,Guid userId)
        {
            string result = "";
            try
            {
                SessionManager.DoWork(session =>
                {
                     DateTime DateTimeNow = DateTime.Now;
                    List<ABC_UserGroupDanhGiaRole> ListDelete = session.Query<ABC_UserGroupDanhGiaRole>().
                                                                        Where(e => e.WebUser.Id == obj.WebUserId && 
                                                                                    e.WebUser.StaffInfo.StaffProfile.GCRecord == null && e.DeleteTime == null).
                                                                        ToList();
                    Delete(ListDelete, userId, session);
                    if(obj.ListGroupDanhGiaId != null)
                    {
                        obj.ListGroupDanhGiaId = obj.ListGroupDanhGiaId.Distinct().ToList();
                        foreach (Guid GroupDanhGiaId in obj.ListGroupDanhGiaId)
                        {
                            ABC_GroupDanhGia ObjGroupDanhGia = session.Query<ABC_GroupDanhGia>().Single(e => e.Id == GroupDanhGiaId && e.TimeDelete == null);
                            if (ObjGroupDanhGia.DaiDienDanhGia == true)
                            {
                                WebUser UserNow = session.Query<WebUser>().Single(e => e.Id == obj.WebUserId);
                                ABC_UserGroupDanhGiaRole UserRoleDaiDienDanhGia = session.Query<ABC_UserGroupDanhGiaRole>().
                                                                                            SingleOrDefault(e => e.GroupDanhGia.Id == ObjGroupDanhGia.Id &&
                                                                                                                e.WebUser.StaffInfo.Staff.Department.Id == UserNow.StaffInfo.Staff.Department.Id &&
                                                                                                                e.DeleteTime == null);
                                if (UserRoleDaiDienDanhGia != null)
                                {
                                    result = "Đã có nhân viên đại diện cho đơn vị " + UserNow.StaffInfo.Staff.Department.Name + ": " + UserRoleDaiDienDanhGia.WebUser.UserName + "-" + UserRoleDaiDienDanhGia.WebUser.StaffInfo.StaffProfile.Name;
                                }
                                else
                                {
                                    ABC_UserGroupDanhGiaRole ObjSave = new ABC_UserGroupDanhGiaRole();
                                    ObjSave.Id = Guid.NewGuid();
                                    ObjSave.GroupDanhGia = new ABC_GroupDanhGia() { Id = GroupDanhGiaId };
                                    ObjSave.WebUser = new WebUser() { Id = obj.WebUserId.Value };
                                    ObjSave.AddTime = DateTimeNow;
                                    ObjSave.AddUserId = userId;
                                    session.SaveOrUpdate(ObjSave);
                                }
                            }
                            else
                            {
                                ABC_UserGroupDanhGiaRole ObjSave = new ABC_UserGroupDanhGiaRole();
                                ObjSave.Id = Guid.NewGuid();
                                ObjSave.GroupDanhGia = new ABC_GroupDanhGia() { Id = GroupDanhGiaId };
                                ObjSave.WebUser = new WebUser() { Id = obj.WebUserId.Value };
                                ObjSave.AddTime = DateTimeNow;
                                ObjSave.AddUserId = userId;
                                session.SaveOrUpdate(ObjSave);
                            }
                        }
                    }
                    
                });
                if (result == "")
                    result = "SUCCESS";
            }
            catch (Exception ex)
            {
                result = "ERRORS";
                Helper.ErrorLog("ABC_UserGroupDanhGiaRoleApi/PutSaveOrUpdate ", ex); throw ex;
            }
            return result;
        }
        [Authorize]
        [Route("")]
        public string PutSaveAdds(List<ABC_UserDTO> listUser, Guid? groupDanhGiaId, Guid userId)
        {
            string result = "";
            try
            {
                SessionManager.DoWork(session =>
                {
                    DateTime DateTimeNow = DateTime.Now;
                    ABC_GroupDanhGia ObjGroupDanhGia = new ABC_GroupDanhGia();
                    if (groupDanhGiaId.HasValue)
                        ObjGroupDanhGia = session.Query<ABC_GroupDanhGia>().SingleOrDefault(e => e.Id == groupDanhGiaId.Value);
                    foreach (ABC_UserDTO User in listUser)
                    {
                        List<ABC_UserGroupDanhGiaRole> ListDelete = session.Query<ABC_UserGroupDanhGiaRole>().Where(e => e.WebUser.Id == User.WebUserId.Value && e.DeleteTime == null).ToList();
                        Delete(ListDelete, userId, session);

                        if (groupDanhGiaId.HasValue)
                            if (ObjGroupDanhGia.DaiDienDanhGia == true)
                            {
                                ABC_User UserNow = session.Query<ABC_User>().Single(e => e.Id == User.Id && e.WebUser.StaffInfo.StaffProfile.GCRecord == null);
                                ABC_UserGroupDanhGiaRole UserRoleDaiDienDanhGia = session.Query<ABC_UserGroupDanhGiaRole>().
                                                                                            SingleOrDefault(e => e.GroupDanhGia.Id == ObjGroupDanhGia.Id &&
                                                                                                                e.WebUser.Department.Id == UserNow.Department.Id &&
                                                                                                                e.DeleteTime == null);
                                if (UserRoleDaiDienDanhGia != null)
                                {
                                    result = "Đã có nhân viên đại diện cho đơn vị " + UserNow.Department.Name + ": " + UserRoleDaiDienDanhGia.WebUser.UserName + "-" + UserRoleDaiDienDanhGia.WebUser.StaffInfo.StaffProfile.Name;
                                    break;
                                }
                                else
                                {
                                    ABC_UserGroupDanhGiaRole ObjSave = new ABC_UserGroupDanhGiaRole();
                                    ObjSave.Id = Guid.NewGuid();
                                    ObjSave.GroupDanhGia = new ABC_GroupDanhGia() { Id = groupDanhGiaId.Value };
                                    ObjSave.WebUser = new WebUser() { Id = User.WebUserId.Value };
                                    ObjSave.AddTime = DateTimeNow;
                                    ObjSave.AddUserId = userId;
                                    session.SaveOrUpdate(ObjSave);
                                }
                            }
                            else
                            {
                                ABC_UserGroupDanhGiaRole ObjSave = new ABC_UserGroupDanhGiaRole();
                                ObjSave.Id = Guid.NewGuid();
                                ObjSave.GroupDanhGia = new ABC_GroupDanhGia() { Id = groupDanhGiaId.Value };
                                ObjSave.WebUser = new WebUser() { Id = User.WebUserId.Value };
                                ObjSave.AddTime = DateTimeNow;
                                ObjSave.AddUserId = userId;
                                session.SaveOrUpdate(ObjSave);
                            }
                    }
                });
                if (result == "")
                    result = "SUCCESS";
            }
            catch (Exception ex)
            {
                result = "ERRORS";
                Helper.ErrorLog("ABC_UserGroupDanhGiaRoleApi/PutSaveAdds ", ex); throw ex;
            }
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<DepartmentDTO> GetList()
        {
            var result = new List<DepartmentDTO>();
            SessionManager.DoWork(session =>
            {
                Guid truong = new Guid(System.Configuration.ConfigurationManager.AppSettings["BoPhanCha"]); //Trường
                result = session.Query<Department>().Where(d => d.ParentDepartment.Id == truong && d.GCRecord == null && d.IsDisable == false).OrderBy(d => d.Name).ToList().Map<DepartmentDTO>();
            });
            return result;
        }
        public void Delete(List<ABC_UserGroupDanhGiaRole> ListDelete, Guid userId, ISession session)
        {
            foreach(ABC_UserGroupDanhGiaRole ObjDelete in ListDelete)
            {
                Delete(ObjDelete, userId, session);
            }
        }
        public void Delete(ABC_UserGroupDanhGiaRole ObjDelete, Guid userId, ISession session)
        {
            ObjDelete.DeleteTime = DateTime.Now;
            ObjDelete.DeleteUserId = userId;
            session.Update(ObjDelete);
        }
    }
}
