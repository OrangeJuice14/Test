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
    public class ABC_WebUserGroupDanhGiaRoleApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_WebUserGroupDanhGiaRoleVMDTO> GetListUserQuanLyDonVi()
        {
            List<ABC_WebUserGroupDanhGiaRoleVMDTO> result = new List<ABC_WebUserGroupDanhGiaRoleVMDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_WebUserGroupDanhGiaRole>().
                                Where(e => e.GroupDanhGia.HasQuanLyDonVi == true).Map<ABC_WebUserGroupDanhGiaRoleVMDTO>().ToList();
            });
            return result;
        }

        public IEnumerable<ABC_WebUserGroupDanhGiaRole> GetAll(ISession session)
        {
            List<ABC_WebUserGroupDanhGiaRole> result = new List<ABC_WebUserGroupDanhGiaRole>();
            result = session.Query<ABC_WebUserGroupDanhGiaRole>().Where(e => e.GCRecord == null).ToList();
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_WebUserGroupDanhGiaRoleVMDTO> GetAllDTO()
        {
            List<ABC_WebUserGroupDanhGiaRoleVMDTO> result = new List<ABC_WebUserGroupDanhGiaRoleVMDTO>();
            SessionManager.DoWork(session =>
            {
                result = GetAll(session).Map<ABC_WebUserGroupDanhGiaRoleVMDTO>().ToList();
            });
            return result;
        }
        //[Authorize]
        //[Route("")]
        //public IEnumerable<ABC_UserDTO> GetListUserDanhGia(Guid boTieuChiId)
        //{
        //    List<ABC_UserDTO> result = new List<ABC_UserDTO>();
        //    ABC_BoTieuChi_Role ObjBoTieuChiRole = new ABC_BoTieuChi_Role();
        //    SessionManager.DoWork(session =>
        //    {
        //        ABC_GroupDanhGia GroupDanhGia = session.Query<ABC_BoTieuChi_Role>().
        //                                                Where(e => e.BoTieuChi.Id == boTieuChiId).
        //                                                Select(e => e.GroupTuDanhGia).
        //                                                Distinct().
        //                                                SingleOrDefault();

        //        result = session.Query<ABC_User>().
        //                        Where(e => e.GroupDanhGia.Id == GroupDanhGia.Id && e.DeleteTime == null).
        //                        Map<ABC_UserDTO>().
        //                        ToList();

        //    });
        //    return result;
        //}
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
        public IEnumerable<ABC_WebUserGroupDanhGiaRoleVMDTO> GetByUserId(Guid userId)
        {
            List<ABC_WebUserGroupDanhGiaRoleVMDTO> result = new List<ABC_WebUserGroupDanhGiaRoleVMDTO>();

            SessionManager.DoWork(session =>
            {
                List<ABC_WebUserGroupDanhGiaRole> list = session.Query<ABC_WebUserGroupDanhGiaRole>().
                                                                Where(e => e.WebUser.Id == userId && e.DeleteTime == null).
                                                                ToList();
                result = list.Map<ABC_WebUserGroupDanhGiaRoleVMDTO>();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public string Put(List<ABC_WebUserGroupDanhGiaRoleCreateDTO> list, Guid userId)
        {
            string result = "";
            try
            {
                SessionManager.DoWork(session =>
                {
                    DateTime DateTimeNow = DateTime.Now;
                    List<ABC_WebUserGroupDanhGiaRole> ListDelete = session.Query<ABC_WebUserGroupDanhGiaRole>().
                                                                        Where(e => e.WebUser.Id == list[0].WebUserId &&
                                                                                    e.WebUser.StaffInfo.StaffProfile.GCRecord == null && e.DeleteTime == null).
                                                                        ToList();
                    Delete(ListDelete, userId, DateTimeNow, session);
                    foreach (ABC_WebUserGroupDanhGiaRoleCreateDTO UserGroupDanhGiaRole in list)
                    {
                        ABC_GroupDanhGia ObjGroupDanhGia = session.Query<ABC_GroupDanhGia>().Single(e => e.Id == UserGroupDanhGiaRole.GroupDanhGiaId && e.GCRecord == null);
                        if (ObjGroupDanhGia.DaiDienDanhGia == true)
                        {
                            WebUser UserNow = session.Query<WebUser>().Single(e => e.Id == UserGroupDanhGiaRole.WebUserId);
                            ABC_WebUserGroupDanhGiaRole UserRoleDaiDienDanhGia = session.Query<ABC_WebUserGroupDanhGiaRole>().
                                                                                        SingleOrDefault(e => e.GroupDanhGia.Id == ObjGroupDanhGia.Id &&
                                                                                                            e.WebUser.StaffInfo.Staff.Department.Id == UserNow.StaffInfo.Staff.Department.Id &&
                                                                                                            e.DeleteTime == null);
                            if (UserRoleDaiDienDanhGia != null)
                            {
                                result += "Đã có nhân viên đại diện cho đơn vị " + UserNow.StaffInfo.Staff.Department.Name + ": " + UserRoleDaiDienDanhGia.WebUser.UserName + "-" + UserRoleDaiDienDanhGia.WebUser.StaffInfo.StaffProfile.Name + "\n";
                            }
                            else
                            {
                                ABC_WebUserGroupDanhGiaRole ObjSave = new ABC_WebUserGroupDanhGiaRole();
                                ObjSave.Id = Guid.NewGuid();
                                ObjSave.GroupDanhGia = new ABC_GroupDanhGia() { Id = UserGroupDanhGiaRole.GroupDanhGiaId.Value };
                                ObjSave.WebUser = new WebUser() { Id = UserGroupDanhGiaRole.WebUserId.Value };
                                ObjSave.AddTime = DateTimeNow;
                                ObjSave.AddUserId = userId;
                                session.SaveOrUpdate(ObjSave);
                            }
                        }
                        else
                        {
                            ABC_WebUserGroupDanhGiaRole ObjSave = new ABC_WebUserGroupDanhGiaRole();
                            ObjSave.Id = Guid.NewGuid();
                            ObjSave.GroupDanhGia = new ABC_GroupDanhGia() { Id = UserGroupDanhGiaRole.GroupDanhGiaId.Value };
                            ObjSave.WebUser = new WebUser() { Id = UserGroupDanhGiaRole.WebUserId.Value };
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
                Helper.ErrorLog("ABC_UserGroupDanhGiaRoleApi/Put", ex); throw ex;
            }
            return result;
        }
        //[Authorize]
        //[Route("")]
        //public string Puts(List<ABC_WebUserVMDTO> listUser, Guid? groupDanhGiaId, Guid userId)
        //{
        //    string result = "";
        //    try
        //    {
        //        SessionManager.DoWork(session =>
        //        {
        //            DateTime DateTimeNow = DateTime.Now;
        //            ABC_GroupDanhGia ObjGroupDanhGia = new ABC_GroupDanhGia();
        //            if (groupDanhGiaId.HasValue)
        //                ObjGroupDanhGia = session.Query<ABC_GroupDanhGia>().SingleOrDefault(e => e.Id == groupDanhGiaId.Value);
        //            foreach (ABC_WebUserVMDTO User in listUser)
        //            {
        //                List<ABC_WebUserGroupDanhGiaRole> ListDelete = session.Query<ABC_WebUserGroupDanhGiaRole>().Where(e => e.WebUser.Id == User.WebUserId.Value && e.DeleteTime == null).ToList();
        //                Delete(ListDelete, userId, DateTimeNow, session);
        //                if (groupDanhGiaId.HasValue)
        //                    if (ObjGroupDanhGia.DaiDienDanhGia == true)
        //                    {
        //                        ABC_User UserNow = session.Query<ABC_User>().Single(e => e.Id == User.Id && e.WebUser.StaffInfo.StaffProfile.GCRecord == null);
        //                        ABC_WebUserGroupDanhGiaRole UserRoleDaiDienDanhGia = session.Query<ABC_WebUserGroupDanhGiaRole>().
        //                                                                                    SingleOrDefault(e => e.GroupDanhGia.Id == ObjGroupDanhGia.Id &&
        //                                                                                                        e.WebUser.Department.Id == UserNow.Department.Id &&
        //                                                                                                        e.DeleteTime == null);
        //                        if (UserRoleDaiDienDanhGia != null)
        //                        {
        //                            result = "Đã có nhân viên đại diện cho đơn vị " + UserNow.Department.Name + ": " + UserRoleDaiDienDanhGia.WebUser.UserName + "-" + UserRoleDaiDienDanhGia.WebUser.StaffInfo.StaffProfile.Name;
        //                            break;
        //                        }
        //                        else
        //                        {
        //                            ABC_WebUserGroupDanhGiaRole ObjSave = new ABC_WebUserGroupDanhGiaRole();
        //                            ObjSave.Id = Guid.NewGuid();
        //                            ObjSave.GroupDanhGia = new ABC_GroupDanhGia() { Id = groupDanhGiaId.Value };
        //                            ObjSave.WebUser = new WebUser() { Id = User.WebUserId.Value };
        //                            ObjSave.AddTime = DateTimeNow;
        //                            ObjSave.AddUserId = userId;
        //                            session.SaveOrUpdate(ObjSave);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ABC_WebUserGroupDanhGiaRole ObjSave = new ABC_WebUserGroupDanhGiaRole();
        //                        ObjSave.Id = Guid.NewGuid();
        //                        ObjSave.GroupDanhGia = new ABC_GroupDanhGia() { Id = groupDanhGiaId.Value };
        //                        ObjSave.WebUser = new WebUser() { Id = User.WebUserId.Value };
        //                        ObjSave.AddTime = DateTimeNow;
        //                        ObjSave.AddUserId = userId;
        //                        session.SaveOrUpdate(ObjSave);
        //                    }
        //            }
        //        });
        //        if (result == "")
        //            result = "SUCCESS";
        //    }
        //    catch (Exception ex)
        //    {
        //        result = "ERRORS";
        //        Helper.ErrorLog("ABC_UserGroupDanhGiaRoleApi/PutSaveAdds ", ex); throw ex;
        //    }
        //    return result;
        //}
        public void Delete(List<ABC_WebUserGroupDanhGiaRole> ListDelete, Guid userId, DateTime dateTimeNow, ISession session)
        {
            foreach (ABC_WebUserGroupDanhGiaRole ObjDelete in ListDelete)
            {
                Delete(ObjDelete, userId, dateTimeNow, session);
            }
        }
        public void Delete(ABC_WebUserGroupDanhGiaRole ObjDelete, Guid userId, DateTime dateTimeNow, ISession session)
        {
            ObjDelete.DeleteTime = dateTimeNow;
            ObjDelete.DeleteUserId = userId;
            session.Update(ObjDelete);
        }
    }
}
