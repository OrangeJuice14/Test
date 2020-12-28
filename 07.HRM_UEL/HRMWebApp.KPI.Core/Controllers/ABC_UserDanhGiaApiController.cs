using AutoMapper;
using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.Core.DTO.ABC.New;
using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_UserDanhGiaApiController : ApiController
    {
        public ABC_UserVMDTO GetUserByKyDanhGia(ISession session, Guid userId, ABC_KyDanhGia ojKyDanhGia)
        {

            WebUser ObjUser = session.Query<WebUser>().Single(e => e.Id == userId);

            // Get Thong Tin Nhân Viên mới nhất trước kì đánh giá đã chọn
            HistoryThongTinHoSoNhanVien ObjHistoryThongTinHoSoNhanVien = session.Query<HistoryThongTinHoSoNhanVien>()
                                                                                .Where(e => e.StaffInfo.Id == ObjUser.StaffInfo.Id &&
                                                                                            (e.TimeLog == null || e.TimeLog <= ojKyDanhGia.DenNgay))
                                                                                .OrderBy(e => e.TimeLog)
                                                                                .First();

            //Lấy thời gian update UserGroupDanhGiaRole lớn nhất tính đến kỳ đánh giá đã chọn
            ABC_WebUserGroupDanhGiaRole UserGroupDanhGiaRole = session.Query<ABC_WebUserGroupDanhGiaRole>()
                                                                        .Where(e => (e.AddTime == null || e.AddTime <= ojKyDanhGia.DenNgay) && e.WebUser.Id == ObjUser.Id)
                                                                        .OrderBy(e => e.AddTime)
                                                                        .First();
            DateTime? TimeModifyInKyDanhGia = UserGroupDanhGiaRole.AddTime;

            //Get GroupDanhGia Của user mới nhất tính đến kì đánh giá đã chọn
            List<ABC_WebUserGroupDanhGiaRole> ListUserGroupDanhGiaRole = session.Query<ABC_WebUserGroupDanhGiaRole>()
                                                                                .Where(e => e.AddTime == TimeModifyInKyDanhGia && e.WebUser.Id == ObjUser.Id)
                                                                                .ToList();

            return new ABC_UserVMDTO(ObjUser, ObjHistoryThongTinHoSoNhanVien, ListUserGroupDanhGiaRole);
        }
        public ABC_UserVMDTO GetUserByKyDanhGiaWithGroupDanhGia(ISession session, Guid userId, ABC_KyDanhGia ojKyDanhGia, Guid groupDanhGiaId)
        {

            WebUser ObjUser = session.Query<WebUser>().Single(e => e.Id == userId);

            // Get Thong Tin Nhân Viên mới nhất trước kì đánh giá đã chọn
            HistoryThongTinHoSoNhanVien ObjHistoryThongTinHoSoNhanVien = session.Query<HistoryThongTinHoSoNhanVien>()
                                                                                .Where(e => e.StaffInfo.Id == ObjUser.StaffInfo.Id &&
                                                                                            (e.TimeLog.HasValue == false || e.TimeLog <= ojKyDanhGia.DenNgay))
                                                                                .OrderBy(e => e.TimeLog)
                                                                                .First();

            //Lấy thời gian update UserGroupDanhGiaRole lớn nhất tính đến kỳ đánh giá đã chọn
            DateTime? TimeModifyInKyDanhGia = session.Query<ABC_WebUserGroupDanhGiaRole>()
                                                        .Where(e => (e.AddTime == null || e.AddTime <= ojKyDanhGia.DenNgay) && e.WebUser.Id == ObjUser.Id)
                                                        .OrderBy(e => e.AddTime)
                                                        .Select(e => e.AddTime)
                                                        .First();

            //Get GroupDanhGia Của user mới nhất tính đến kì đánh giá đã chọn
            List<ABC_WebUserGroupDanhGiaRole> ListUserGroupDanhGiaRole = session.Query<ABC_WebUserGroupDanhGiaRole>()
                                                                                .Where(e => e.AddTime == TimeModifyInKyDanhGia && e.WebUser.Id == ObjUser.Id)
                                                                                .ToList();
            ABC_GroupDanhGia ObjGroupDanhGia = session.Query<ABC_GroupDanhGia>().Single(e => e.Id == groupDanhGiaId);
            ABC_UserVMDTO result = new ABC_UserVMDTO(ObjUser, ObjHistoryThongTinHoSoNhanVien, ObjGroupDanhGia);
            
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_UserVMDTO> GetListUserDanhGiaInDonVi(Guid boTieuChiId, Guid kyDanhGiaId, Guid userId, Guid groupDanhGiaId, Guid donViId)
        {
            Mapper.CreateMap<ABC_DanhGia, ABC_DanhGiaVMDTO>()
                .ForMember(dest => dest.GroupUserDanhGiaId, opt => opt.MapFrom(src => src.GroupUserDanhGia.Id))
                .ForMember(dest => dest.GroupUserDanhGiaName, opt => opt.MapFrom(src => src.GroupUserDanhGia.Name))
                .ForMember(dest => dest.BoTieuChi, opt => opt.MapFrom(src => src.BoTieuChi));
            Mapper.CreateMap<ABC_BoTieuChi, ABC_BoTieuChiVMDTO>();
            List<ABC_UserVMDTO> result = new List<ABC_UserVMDTO>();
            ABC_Role_BoTieuChi ObjBoTieuChiRole = new ABC_Role_BoTieuChi();
            try
            {
                // Get ListUser trong đơn vị
                List<ABC_UserVMDTO> ListUserVM = DataClassHelper.spd_DanhGiaABC_GetListThongTinUserInDonVi(kyDanhGiaId, donViId);

                SessionManager.DoWork(session =>
                {
                    ABC_KyDanhGia KyDanhGia = session.Query<ABC_KyDanhGia>().Single(e => e.Id == kyDanhGiaId);
                    // Get GroupTuDanhGia mà BoTieuChi được phân quyền
                    ABC_GroupDanhGia GroupTuDanhGia = session.Query<ABC_Role_BoTieuChi>().
                                                            Where(e => e.BoTieuChi.Id == boTieuChiId && e.GroupDanhGia.Id == groupDanhGiaId).
                                                            Select(e => e.GroupTuDanhGia).
                                                            Distinct().SingleOrDefault();

                    ABC_GroupDanhGia GroupDanhGia = session.Query<ABC_GroupDanhGia>().Single(e => e.Id == groupDanhGiaId);

                    ABC_User UserNow = session.Query<ABC_User>().SingleOrDefault(e => e.WebUser.Id == userId && e.KyDanhGia.Id == KyDanhGia.Id && e.DeleteTime == null && e.GroupDanhGia.Id == GroupDanhGia.Id);

                    List<ABC_WebUserGroupDanhGiaRole> ListUserRole = new List<ABC_WebUserGroupDanhGiaRole>();

                    int TotalUser = ListUserVM.Count;
                    for (int i = 0; i < TotalUser; i++)
                    {
                        List<ABC_DanhGia> ListDanhGia = session.Query<ABC_DanhGia>().
                                                                Where(e => e.BoTieuChi.Id == boTieuChiId &&
                                                                            e.KyDanhGia.Id == kyDanhGiaId &&
                                                                            e.UserDuocDanhGia.Id == ListUserVM[i].Id).
                                                                OrderBy(e => e.GroupUserDanhGia.STT).ToList();
                        List<ABC_DanhGiaVMDTO> ListDanhGiaDTO = ListDanhGia.Map<ABC_DanhGiaVMDTO>();
                        ListUserVM[i].ListDanhGia  = ListDanhGiaDTO;
                        ListUserVM[i].GroupDanhGiaId = GroupTuDanhGia.Id;
                        ListUserVM[i].GroupDanhGiaName = GroupTuDanhGia.Name;
                    }
                });
                return ListUserVM;
            }
            catch (Exception ex)
            {
                result = new List<ABC_UserVMDTO>();
                Helper.ErrorLog("ABC_UserDanhGiaApi/GetListUserDanhGia ", ex); throw ex;
            }
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
                Helper.ErrorLog("ABC_UserDanhGiaApi/GetCheckIsTeacher ", ex); throw ex;
            }
            return result;
        }
        [Authorize]
        [Route("")]
        public ABC_UserVMDTO GetUserAndListBoTieuChiPhanQuyen(Guid userId, Guid kyDanhGiaId)
        {
            ABC_UserVMDTO result = new ABC_UserVMDTO();
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_KyDanhGia ObjKyDanhGia = session.Query<ABC_KyDanhGia>().Single(e => e.Id == kyDanhGiaId);

                    result = GetUserByKyDanhGia(session, userId, ObjKyDanhGia);

                    int CountListGroupDanhGia = result.ListGroupDanhGia.Count;
                    for (int i = 0; i < CountListGroupDanhGia; i++)
                    {
                        if (result.ListGroupDanhGia[i].TuDanhGia == true)
                        {
                            // Get List Bộ tiêu chí Tự đánh giá được phân quyền
                            List<ABC_BoTieuChiVMDTO> ListBoTieuChi = session.Query<ABC_Role_BoTieuChi>()
                                                                            .Where(e => e.GroupTuDanhGia.Id == result.ListGroupDanhGia[i].Id &&
                                                                                        e.BoTieuChi.TuNgay <= DateTime.Now.Date &&
                                                                                        e.BoTieuChi.DenNgay >= DateTime.Now.Date &&
                                                                                        ((e.BoTieuChi.TuNgay <= ObjKyDanhGia.TuNgay &&
                                                                                        (e.BoTieuChi.DenNgay >= ObjKyDanhGia.DenNgay || e.BoTieuChi.DenNgay >= ObjKyDanhGia.TuNgay)) ||
                                                                                        (e.BoTieuChi.TuNgay > ObjKyDanhGia.TuNgay && e.BoTieuChi.TuNgay <= ObjKyDanhGia.DenNgay)) &&
                                                                                        e.BoTieuChi.LoaiBoTieuChi == ObjKyDanhGia.Loai)
                                                                            .Select(e => e.BoTieuChi)
                                                                            .Distinct()
                                                                            .Map<ABC_BoTieuChiVMDTO>()
                                                                            .ToList();

                            result.ListGroupDanhGia[i].ListBoTieuChiTuDanhGia = ListBoTieuChi == null ? new List<ABC_BoTieuChiVMDTO>() : ListBoTieuChi;
                        }

                        if (result.ListGroupDanhGia[i].DanhGiaCapDuoi == true)
                        {
                            // Get List Bộ tiêu chí được phân quyền đánh giá cấp dưới
                            List<ABC_BoTieuChiVMDTO> ListBoTieuChi = session.Query<ABC_Role_BoTieuChi>()
                                                                            .Where(e => e.GroupDanhGia.Id == result.ListGroupDanhGia[i].Id &&
                                                                                        e.BoTieuChi.TuNgay <= DateTime.Now.Date &&
                                                                                        e.BoTieuChi.DenNgay >= DateTime.Now.Date &&
                                                                                        ((e.BoTieuChi.TuNgay <= ObjKyDanhGia.TuNgay &&
                                                                                        (e.BoTieuChi.DenNgay >= ObjKyDanhGia.DenNgay || e.BoTieuChi.DenNgay >= ObjKyDanhGia.TuNgay)) ||
                                                                                        (e.BoTieuChi.TuNgay > ObjKyDanhGia.TuNgay && e.BoTieuChi.TuNgay <= ObjKyDanhGia.DenNgay)) &&
                                                                                        e.BoTieuChi.LoaiBoTieuChi == ObjKyDanhGia.Loai)
                                                                            .Select(e => e.BoTieuChi)
                                                                            .Distinct()
                                                                            .Map<ABC_BoTieuChiVMDTO>()
                                                                            .ToList();

                            result.ListGroupDanhGia[i].ListBoTieuChiDanhGiaCapDuoi = ListBoTieuChi == null ? new List<ABC_BoTieuChiVMDTO>() : ListBoTieuChi;
                        }
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
        public ABC_UserVMDTO GetUserWithGroupDanhGiaId(Guid userId, Guid kyDanhGiaId, Guid groupDanhGiaId)
        {
            ABC_UserVMDTO result = new ABC_UserVMDTO();
            try
            {
                SessionManager.DoWorkNoTransaction(session =>
                {
                    ABC_KyDanhGia ObjKyDanhGia = session.Query<ABC_KyDanhGia>().Single(e => e.Id == kyDanhGiaId);

                    result = GetUserByKyDanhGiaWithGroupDanhGia(session, userId, ObjKyDanhGia, groupDanhGiaId);
                });
            }
            catch (Exception ex)
            {
                result = null;
                Helper.ErrorLog("ABC_UserDanhGiaApi/GetUserWithGroupDanhGiaId ", ex); throw ex;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public ABC_UserVMDTO GetUserNow(Guid userId)
        {
            ABC_UserVMDTO result = new ABC_UserVMDTO();
            try
            {
                SessionManager.DoWorkNoTransaction(session =>
                {
                    WebUser WebUser = session.Query<WebUser>().Single(e => e.Id == userId);
                    result = new ABC_UserVMDTO(WebUser);
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
        public ABC_UserVMDTO GetUser(Guid userId, Guid kyDanhGiaId)
        {
            ABC_UserVMDTO result = new ABC_UserVMDTO();
            try
            {
                SessionManager.DoWorkNoTransaction(session =>
                {

                    ABC_KyDanhGia ObjKyDanhGia = session.Query<ABC_KyDanhGia>().Single(e => e.Id == kyDanhGiaId);

                    result = GetUserByKyDanhGia(session, userId, ObjKyDanhGia);
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
