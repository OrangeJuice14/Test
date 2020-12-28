using AutoMapper;
using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_UserDanhGiaApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_UserGroupDanhGiaRoleDTO> GetListUserDanhGia(Guid boTieuChiId, Guid kyDanhGiaId, Guid userId, Guid groupDanhGiaId)
        {
            Mapper.CreateMap<ABC_DanhGia, ABC_DanhGiaDTO>()
                .ForMember(dest => dest.UserDanhGia_GroupId, opt => opt.MapFrom(src => src.UserDanhGia_Group.Id))
                .ForMember(dest => dest.UserDanhGia_GroupName, opt => opt.MapFrom(src => src.UserDanhGia_Group.Name));
            //.ForMember(dest => dest.BoTieuChi, opt => opt.MapFrom(src => src.BoTieuChi));
            List<ABC_UserGroupDanhGiaRoleDTO> result = new List<ABC_UserGroupDanhGiaRoleDTO>();
            ABC_BoTieuChi_Role ObjBoTieuChiRole = new ABC_BoTieuChi_Role();
            //try
            //{
            //    SessionManager.DoWork(session =>
            //    {
            //        ABC_KyDanhGia KyDanhGia = session.Query<ABC_KyDanhGia>().Single(e => e.Id == kyDanhGiaId);
            //        // Get GroupTuDanhGia mà BoTieuChi được phân quyền
            //        ABC_GroupDanhGia GroupTuDanhGia = session.Query<ABC_BoTieuChi_Role>().
            //                                                Where(e => e.BoTieuChi.Id == boTieuChiId).
            //                                                Select(e => e.GroupTuDanhGia).
            //                                                Distinct().SingleOrDefault();

            //        ABC_GroupDanhGia GroupDanhGia = session.Query<ABC_GroupDanhGia>().Single(e => e.Id == groupDanhGiaId);

            //        ABC_User UserNow = session.Query<ABC_User>().
            //                                    Where(e => e.Id == userId &&
            //                                                e.WebUser.StaffInfo.StaffProfile.GCRecord == null &&
            //                                                e.LastChange < KyDanhGia.DenNgay.Value.AddDays(1)).
            //                                    FirstOrDefault();

            //        List<ABC_UserGroupDanhGiaRole> ListUserRole = new List<ABC_UserGroupDanhGiaRole>();

            //        if (GroupDanhGia.HasQuanLyDonVi.HasValue && GroupDanhGia.HasQuanLyDonVi.Value)
            //        {
            //        }
            //        else
            //        {
            //            // Get ListUser được phân quyền vào nhóm
            //            List<ABC_User> ListUser = session.Query<ABC_UserGroupDanhGiaRole>().
            //                                                Where(e => e.GroupDanhGia.Id == GroupTuDanhGia.Id &&
            //                                                            e.User.Id != UserNow.Id && e.AddTime < KyDanhGia.DenNgay.Value.AddDays(1)).
            //                                                Select(e => e.User).
            //                                                Distinct().
            //                                                OrderBy(e => e.WebUser.StaffInfo.StaffProfile.FirstName).
            //                                                ThenBy(e => e.WebUser.StaffInfo.StaffProfile.LastName).ToList();
            //            foreach (ABC_User User in ListUser)
            //            {
            //                ABC_UserGroupDanhGiaRole UserGroupRole = session.Query<ABC_UserGroupDanhGiaRole>().
            //                                                            Where(e => e.User.Id != UserNow.Id &&
            //                                                                        e.AddTime < KyDanhGia.DenNgay.Value.AddDays(1)).
            //                                                            OrderByDescending(e => e.AddTime).
            //                                                            FirstOrDefault();
            //                if (UserGroupRole.GroupDanhGia.Id == GroupTuDanhGia.Id)
            //                {
            //                    ListUserRole.Add(UserGroupRole);
            //                    //------------------------------------------
            //                    if (UserGroupRole.User.LastChange.HasValue && UserGroupRole.User.LastChange.Value > KyDanhGia.DenNgay.Value.AddDays(1))
            //                    {
            //                        ABC_HistoryChangeUser OldUser = session.Query<ABC_HistoryChangeUser>().
            //                                                                    Where(e => e.LastChange <= KyDanhGia.DenNgay &&
            //                                                                                e.Id == UserGroupRole.User.Id).
            //                                                                    OrderByDescending(e => e.LastChange).
            //                                                                    FirstOrDefault();

            //                        UserGroupRole.User = new ABC_User(OldUser);
            //                    }

            //                    if (UserGroupRole.User.Department.Id == UserNow.Department.Id)
            //                    {
            //                        ABC_UserGroupDanhGiaRoleDTO Obj = UserGroupRole.Map<ABC_UserGroupDanhGiaRoleDTO>();
            //                        Obj.UserDanhGia = UserGroupRole.User.Map<ABC_UserDanhGiaDTO>();
            //                        List<ABC_DanhGia> ListDanhGia = session.Query<ABC_DanhGia>().
            //                                                                Where(e => e.BoTieuChi.Id == boTieuChiId &&
            //                                                                            e.KyDanhGia.Id == kyDanhGiaId &&
            //                                                                            e.UserDuocDanhGia.Id == UserGroupRole.User.Id).
            //                                                                OrderBy(e => e.UserDanhGia_Group.STT).ToList();
            //                        List<ABC_DanhGiaDTO> ListDanhGiaDTO = ListDanhGia.Map<ABC_DanhGiaDTO>();
            //                        Obj.UserDanhGia.ListDanhGia = ListDanhGiaDTO;
            //                        //for(int i = 0; i< Obj.UserDanhGia.ListDanhGia.Count; i++)
            //                        //{
            //                        //    Obj.UserDanhGia.ListDanhGia[i].LoaiDanhGia = Obj.UserDanhGia.ListDanhGia[i].UserDuocDanhGiaId == Obj.UserDanhGia.ListDanhGia[i].UserDanhGiaId ? "Tự" : UserRole.GroupDanhGia.Name;
            //                        //    //objDanhGia.LoaiTuDanhGia = ListBoTieuChiRole[0].GroupTuDanhGia.Name;
            //                        //}
            //                        result.Add(Obj);
            //                    }

            //                }
            //            }
            //        }
            //    });
            //}
            //catch (Exception ex)
            //{
            //    result = new List<ABC_UserGroupDanhGiaRoleDTO>();
            //    Helper.ErrorLog("ABC_UserDanhGiaApi/GetListUserDanhGia ", ex); throw ex;
            //}
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_UserDTO> GetListUserDanhGia(Guid boTieuChiId, Guid kyDanhGiaId, Guid userId, Guid groupDanhGiaId, Guid donViId)
        {
            Mapper.CreateMap<ABC_DanhGia, ABC_DanhGiaDTO>()
                .ForMember(dest => dest.UserDanhGia_GroupId, opt => opt.MapFrom(src => src.UserDanhGia_Group.Id))
                .ForMember(dest => dest.UserDanhGia_GroupName, opt => opt.MapFrom(src => src.UserDanhGia_Group.Name))
                .ForMember(dest => dest.BoTieuChi, opt => opt.MapFrom(src => src.BoTieuChi));
            Mapper.CreateMap<ABC_BoTieuChi, ABC_BoTieuChiDTO>();
            List<ABC_UserDTO> result = new List<ABC_UserDTO>();
            ABC_BoTieuChi_Role ObjBoTieuChiRole = new ABC_BoTieuChi_Role();
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_KyDanhGia KyDanhGia = session.Query<ABC_KyDanhGia>().Single(e => e.Id == kyDanhGiaId);
                    // Get GroupTuDanhGia mà BoTieuChi được phân quyền
                    ABC_GroupDanhGia GroupTuDanhGia = session.Query<ABC_BoTieuChi_Role>().
                                                            Where(e => e.BoTieuChi.Id == boTieuChiId && e.GroupDanhGia.Id == groupDanhGiaId).
                                                            Select(e => e.GroupTuDanhGia).
                                                            Distinct().SingleOrDefault();

                    ABC_GroupDanhGia GroupDanhGia = session.Query<ABC_GroupDanhGia>().Single(e => e.Id == groupDanhGiaId);

                    ABC_User UserNow = session.Query<ABC_User>().SingleOrDefault(e => e.WebUser.Id == userId && e.KyDanhGia.Id == KyDanhGia.Id && e.DeleteTime == null && e.GroupDanhGia.Id == GroupDanhGia.Id);

                    List<ABC_UserGroupDanhGiaRole> ListUserRole = new List<ABC_UserGroupDanhGiaRole>();
                    // Get ListUser trong đơn vị
                    List<ABC_User> ListUser = session.Query<ABC_User>().
                                                        Where(e => e.KyDanhGia.Id == kyDanhGiaId &&
                                                                    e.Department.Id == donViId &&
                                                                    e.WebUser.StaffInfo.StaffProfile.GCRecord == null &&
                                                                    e.Status == false &&
                                                                    e.GroupDanhGia.Id == GroupTuDanhGia.Id && e.DeleteTime == null && e.GroupDanhGia.TimeDelete == null).
                                                        OrderBy(e => e.WebUser.StaffInfo.StaffProfile.FirstName).
                                                        ThenBy(e => e.WebUser.StaffInfo.StaffProfile.LastName).ToList();

                    int TotalUser = ListUser.Count;
                    for (int i = 0; i < TotalUser; i++)
                    {
                        //ABC_UserGroupDanhGiaRole userGroupDanhGiaRole = session.Query<ABC_UserGroupDanhGiaRole>().
                        //                                                        SingleOrDefault(e => e.WebUser.Id == ListUser[i].WebUser.Id &&
                        //                                                                            e.GroupDanhGia.Id == ListUser[i].GroupDanhGia.Id && e.DeleteTime == null);
                        //if (userGroupDanhGiaRole != null)
                        //{
                            ABC_UserDTO Obj = new ABC_UserDTO();
                            Obj = ListUser[i].Map<ABC_UserDTO>();
                            List<ABC_DanhGia> ListDanhGia = session.Query<ABC_DanhGia>().
                                                                    Where(e => e.BoTieuChi.Id == boTieuChiId &&
                                                                                e.KyDanhGia.Id == kyDanhGiaId &&
                                                                                e.UserDuocDanhGia.Id == ListUser[i].Id).
                                                                    OrderBy(e => e.UserDanhGia_Group.STT).ToList();
                            List<ABC_DanhGiaDTO> ListDanhGiaDTO = ListDanhGia.Map<ABC_DanhGiaDTO>();
                            Obj.ListDanhGia = ListDanhGiaDTO;
                            result.Add(Obj);
                        //}
                    }
                });
            }
            catch (Exception ex)
            {
                result = new List<ABC_UserDTO>();
                Helper.ErrorLog("ABC_UserDanhGiaApi/GetListUserDanhGia ", ex); throw ex;
            }
            return result;
        }
        [Authorize]
        [Route("")]
        public bool GetCheckIsTeacher(Guid userId, Guid kyDanhGiaId, Guid groupDanhGiaId)
        {
            bool result = false;
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_User User = session.Query<ABC_User>().Where(e => e.WebUser.Id == userId && e.KyDanhGia.Id == kyDanhGiaId && e.GroupDanhGia.Id == groupDanhGiaId).OrderByDescending(e => e.AddTime).FirstOrDefault();
                    result = (User.StaffType.ManageCode != "GV" && User.ThamGiaGiangDay.Value) ? User.ThamGiaGiangDay.Value : false;

                });
            }
            catch (Exception ex)
            {
                result = false;
                Helper.ErrorLog("ABC_UserDanhGiaApi/GetListUserDanhGia ", ex); throw ex;
            }
            return result;
        }
        [Authorize]
        [Route("")]
        public ABC_UserDTO GetUserByKyDanhGiaId(Guid userId, Guid kyDanhGiaId)
        {
            ABC_UserDTO result = new ABC_UserDTO();
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_KyDanhGia KyDanhGia = session.Query<ABC_KyDanhGia>().Single(e => e.Id == kyDanhGiaId);
                    List<ABC_User> User = session.Query<ABC_User>().Where(e => e.WebUser.Id == userId && e.KyDanhGia.Id == kyDanhGiaId && e.DeleteTime == null).OrderBy(e => e.GroupDanhGia.STT).ToList();
                    if (User.Count != 0)
                    {
                        result = new ABC_UserDTO(User);
                        int CountListGroupDanhGia = result.ListGroupDanhGia.Count;
                        for (int i = 0; i < CountListGroupDanhGia; i++)
                        {
                            if (result.ListGroupDanhGia[i].TuDanhGia == true)
                            {
                                List<ABC_BoTieuChiDTO> ListBoTieuChi = session.Query<ABC_BoTieuChi_Role>().
                                                                                Where(e => e.GroupTuDanhGia.Id == result.ListGroupDanhGia[i].Id &&
                                                                                            e.BoTieuChi.TuNgay <= DateTime.Now.Date &&
                                                                                            e.BoTieuChi.DenNgay >= DateTime.Now.Date &&
                                                                                            ((e.BoTieuChi.TuNgay <= KyDanhGia.TuNgay &&
                                                                                            (e.BoTieuChi.DenNgay >= KyDanhGia.DenNgay || e.BoTieuChi.DenNgay >= KyDanhGia.TuNgay)) ||
                                                                                            (e.BoTieuChi.TuNgay > KyDanhGia.TuNgay && e.BoTieuChi.TuNgay <= KyDanhGia.DenNgay)) &&
                                                                                            e.BoTieuChi.LoaiBoTieuChi == KyDanhGia.Loai).
                                                                                Select(e => e.BoTieuChi).
                                                                                Distinct().
                                                                                Map<ABC_BoTieuChiDTO>().
                                                                                ToList();

                                result.ListGroupDanhGia[i].ListBoTieuChiTuDanhGia = ListBoTieuChi == null ? new List<ABC_BoTieuChiDTO>() : ListBoTieuChi;
                            }

                            if (result.ListGroupDanhGia[i].DanhGiaCapDuoi == true)
                            {

                                List<ABC_BoTieuChiDTO> ListBoTieuChi = session.Query<ABC_BoTieuChi_Role>().
                                                                                Where(e => e.GroupDanhGia.Id == result.ListGroupDanhGia[i].Id &&
                                                                                            e.BoTieuChi.TuNgay <= DateTime.Now.Date &&
                                                                                            e.BoTieuChi.DenNgay >= DateTime.Now.Date &&
                                                                                            ((e.BoTieuChi.TuNgay <= KyDanhGia.TuNgay &&
                                                                                            (e.BoTieuChi.DenNgay >= KyDanhGia.DenNgay || e.BoTieuChi.DenNgay >= KyDanhGia.TuNgay)) ||
                                                                                            (e.BoTieuChi.TuNgay > KyDanhGia.TuNgay && e.BoTieuChi.TuNgay <= KyDanhGia.DenNgay)) &&
                                                                                            e.BoTieuChi.LoaiBoTieuChi == KyDanhGia.Loai).
                                                                                Select(e => e.BoTieuChi).
                                                                                Distinct().
                                                                                Map<ABC_BoTieuChiDTO>().
                                                                                ToList();

                                result.ListGroupDanhGia[i].ListBoTieuChiDanhGiaCapDuoi = ListBoTieuChi == null ? new List<ABC_BoTieuChiDTO>() : ListBoTieuChi;
                            }
                        }
                    }
                    else
                    {
                        result = null;
                    }
                });
            }
            catch (Exception ex)
            {
                result = null;
                Helper.ErrorLog("ABC_UserDanhGiaApi/GetUserByKyDanhGiaId ", ex); throw ex;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public ABC_UserDTO GetUserByKyDanhGiaId(Guid userId, Guid kyDanhGiaId, Guid groupDanhGiaId)
        {
            ABC_UserDTO result = new ABC_UserDTO();
            try
            {
                SessionManager.DoWorkNoTransaction(session =>
                {
                    result = session.Query<ABC_User>().Where(e => e.WebUser.Id == userId && e.KyDanhGia.Id == kyDanhGiaId && e.GroupDanhGia.Id == groupDanhGiaId && e.DeleteTime == null).Distinct().SingleOrDefault().Map<ABC_UserDTO>();
                });
            }
            catch (Exception ex)
            {
                result = null;
                Helper.ErrorLog("ABC_UserDanhGiaApi/GetUserByKyDanhGiaId ", ex); throw ex;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public ABC_UserDTO GetUserNow(Guid userId)
        {
            ABC_UserDTO result = new ABC_UserDTO();
            try
            {
                SessionManager.DoWorkNoTransaction(session =>
                {
                    WebUser WebUser = session.Query<WebUser>().Single(e => e.Id == userId);
                    result = new ABC_UserDTO(WebUser);
                });
            }
            catch (Exception ex)
            {
                result = null;
                Helper.ErrorLog("ABC_UserDanhGiaApi/GetUserNow ", ex); throw ex;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public ABC_UserDTO GetById(Guid ABC_UserId)
        {
            ABC_UserDTO result = new ABC_UserDTO();
            try
            {
                SessionManager.DoWorkNoTransaction(session =>
                {
                    ABC_User aBC_User = session.Query<ABC_User>().Single(e => e.Id == ABC_UserId && e.DeleteTime == null);
                    result = aBC_User.Map<ABC_UserDTO>();
                });
            }
            catch (Exception ex)
            {
                result = null;
                Helper.ErrorLog("ABC_UserDanhGiaApi/GetById ", ex); throw ex;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public bool PutChotTTTKTheoKyDanhGiaId(Guid userId, Guid kyDanhGiaId) // Chốt thông tin tài khoản theo từng kì
        {
            bool result = false;
            try
            {
                result = DataClassHelper.spd_DanhGiaABC_ThongThongTinUserTheoKy(kyDanhGiaId, userId) > 0;
            }
            catch (Exception ex)
            {
                result = false;
                Helper.ErrorLog("ABC_UserDanhGiaApi/PutChotTTTKTheoKyDanhGiaId ", ex); throw ex;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public string GetCheckChotTTTK(Guid kyDanhGiaId) // Kiểm tra xem đã chốt thông tin đánh giá của kì chưa
        {
            string result = "";
            try
            {
                SessionManager.DoWorkNoTransaction(session =>
                {
                    List<ABC_User> ListUser = session.Query<ABC_User>().Where(e => e.KyDanhGia.Id == kyDanhGiaId && e.DeleteTime == null).ToList();
                    result = ListUser.Count > 0 ? ListUser[0].AddTime.Value.ToString("dd/MM/yyyy") : "";
                });
            }
            catch (Exception ex)
            {
                result = null;
                Helper.ErrorLog("ABC_UserDanhGiaApi/GetCheckTTTK ", ex); throw ex;
            }
            return result;
        }
    }
}
