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
    public class ABC_DieuKienBoTieuChiApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_DieuKienBoTieuChiDTO> GetByBoTieuChiId(Guid boTieuChiId)
        {
            List<ABC_DieuKienBoTieuChiDTO> result = new List<ABC_DieuKienBoTieuChiDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    result = session.Query<ABC_DieuKienBoTieuChi>().
                                    Where(e => e.BoTieuChi.Id == boTieuChiId).
                                    Map<ABC_DieuKienBoTieuChiDTO>().
                                    ToList();
                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_DieuKienBoTieuChiApi/GetByBoTieuChiId ", ex); throw ex; }

            return result;
        }
        [Authorize]
        [Route("")]
        public string GetCheckDieuKienBoTieuChi(Guid boTieuChiId, Guid kyDanhGiaId, Guid aBC_UserId, Guid groupDanhGiaId)
        {
            string result = "";
            try
            {
                SessionManager.DoWorkNoTransaction(session =>
                {
                    List<ABC_DieuKienBoTieuChi> ListDieuKienBoTieuChi = session.Query<ABC_DieuKienBoTieuChi>().
                                                                                    Where(e => e.BoTieuChi.Id == boTieuChiId).
                                                                                    ToList();
                    ABC_KyDanhGia KyDanhGia = session.Query<ABC_KyDanhGia>().SingleOrDefault(e => e.Id == kyDanhGiaId);
                    ABC_BoTieuChi BoTieuChi = session.Query<ABC_BoTieuChi>().SingleOrDefault(e => e.Id == boTieuChiId);
                    ABC_User UserNow = session.Query<ABC_User>().SingleOrDefault(e => e.Id == aBC_UserId);
                    foreach (ABC_DieuKienBoTieuChi ObjDieuKienBoTieuChi in ListDieuKienBoTieuChi)
                    {
                        ABC_BoTieuChi_Role ObjDieuKienHoanThanhBoTieuChiRole = session.Query<ABC_BoTieuChi_Role>().Where(e => e.BoTieuChi.Id == ObjDieuKienBoTieuChi.HoanThanhBoTieuChi.Id && e.GroupDanhGia.Id == groupDanhGiaId).OrderBy(e => e.GroupDanhGia.STT).FirstOrDefault();
                        if(ObjDieuKienHoanThanhBoTieuChiRole == null)
                        {
                            ObjDieuKienHoanThanhBoTieuChiRole = session.Query<ABC_BoTieuChi_Role>().OrderBy(e => e.GroupDanhGia.STT).FirstOrDefault(e => e.BoTieuChi.Id == ObjDieuKienBoTieuChi.HoanThanhBoTieuChi.Id);
                        }
                        switch (ObjDieuKienBoTieuChi.LoaiHoanThanh)
                        {
                            case 1: // 1: Đúng người tự đánh giá
                                {
                                    switch (KyDanhGia.Loai)
                                    {
                                        case 1: // 1: 6 tháng
                                            break;
                                        case 2: // 2: Quý
                                            break;
                                        case 3: // 3: Tháng
                                            break;
                                        default: // 0: Năm
                                            {
                                                switch (ObjDieuKienBoTieuChi.HoanThanhBoTieuChi.LoaiBoTieuChi)
                                                {
                                                    case 1: // 1: 6 tháng
                                                        {
                                                            break;
                                                        }
                                                    case 2: // 2: Quý
                                                        break;
                                                    case 3: // 3: Tháng
                                                        {
                                                            List<int> ListThangChuaHoanThanh = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                                                            List<ABC_DanhGia> ListDanhGia = session.Query<ABC_DanhGia>().
                                                                                                    Where(e => e.BoTieuChi.Id == ObjDieuKienBoTieuChi.HoanThanhBoTieuChi.Id &&
                                                                                                                e.KyDanhGia.Nam == KyDanhGia.Nam &&
                                                                                                                e.UserDuocDanhGia.WebUser.Id == UserNow.WebUser.Id &&
                                                                                                                e.UserDanhGia_Group.Id == groupDanhGiaId &&
                                                                                                                e.IsLock == true).
                                                                                                    ToList();

                                                            foreach (ABC_DanhGia ObjDanhGia in ListDanhGia)
                                                            {
                                                                ListThangChuaHoanThanh.Remove(ObjDanhGia.KyDanhGia.TuNgay.Value.Month);
                                                            }

                                                            if (ListThangChuaHoanThanh.Count > 0)
                                                            {
                                                                result += ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia.Name + " chưa hoàn thành bộ tiêu chí " + ObjDieuKienBoTieuChi.HoanThanhBoTieuChi.Name + " chưa hoàn thành tháng: ";
                                                                for (int i = 0; i < ListThangChuaHoanThanh.Count; i++)
                                                                {
                                                                    if (i != ListThangChuaHoanThanh.Count - 1) // kiểm tra có phải phần tử cuối cùng hay không
                                                                    {
                                                                        result += ListThangChuaHoanThanh[i] + ", ";
                                                                    }
                                                                    else
                                                                    {
                                                                        result += ListThangChuaHoanThanh[i] + ".\n";
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        }
                                                    default: // 0: Năm
                                                        {
                                                            break;
                                                        }
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case 2: // 2: Tất cả thành viên trong nhóm
                                {
                                    List<ABC_User> ListAllUserInDonVi = session.Query<ABC_User>().
                                                                                Where(e => e.Department.Id == UserNow.Department.Id &&
                                                                                            e.GroupDanhGia.Id == ObjDieuKienHoanThanhBoTieuChiRole.GroupTuDanhGia.Id &&
                                                                                            e.KyDanhGia.Id == kyDanhGiaId &&
                                                                                            e.DeleteTime == null).
                                                                                OrderBy(e => e.WebUser.UserName).ToList();

                                    List<ABC_User> ListUserIsRatingInDonVi = session.Query<ABC_DanhGia>().
                                                                                    Where(e => e.UserDanhGia_Group.Id == ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia.Id &&
                                                                                                e.UserDuocDanhGia.Department.Id == UserNow.Department.Id &&
                                                                                                e.BoTieuChi.Id == ObjDieuKienBoTieuChi.HoanThanhBoTieuChi.Id &&
                                                                                                e.IsLock == true && e.KyDanhGia.Id == kyDanhGiaId).
                                                                                    Select(e => e.UserDuocDanhGia).ToList();

                                    foreach (ABC_User UserIsRating in ListUserIsRatingInDonVi)
                                    {
                                        ListAllUserInDonVi.Remove(UserIsRating);// lọc ra những User chưa đánh giá
                                    }

                                    switch (KyDanhGia.Loai)
                                    {
                                        case 1: // 1: 6 tháng
                                            break;
                                        case 2: // 2: Quý
                                            break;
                                        case 3: // 3: Tháng
                                            {
                                                if (ObjDieuKienBoTieuChi.HoanThanhBoTieuChi.LoaiBoTieuChi == 3) // Check loại của bộ tiêu chí cần hoàn thành trước
                                                {
                                                    if (ListAllUserInDonVi.Count > 0)
                                                    {
                                                        result += ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia.Name + " chưa hoàn thành bộ tiêu chí '" + ObjDieuKienBoTieuChi.HoanThanhBoTieuChi.Name + "' của các nhân viên: \n";
                                                        for (int i = 0; i < ListAllUserInDonVi.Count; i++)
                                                        {
                                                            List<ABC_DanhGia> ListIsDanhGia = session.Query<ABC_DanhGia>().
                                                                                                        Where(e => e.BoTieuChi.Id == ObjDieuKienBoTieuChi.HoanThanhBoTieuChi.Id &&
                                                                                                                    e.IsLock == true &&
                                                                                                                    e.KyDanhGia.Id == kyDanhGiaId &&
                                                                                                                    e.UserDuocDanhGia.Id == ListAllUserInDonVi[i].Id).
                                                                                                        OrderBy(e => e.UserDanhGia_Group.STT).
                                                                                                        ToList();
                                                            if (ListIsDanhGia.Count == 0 || ListIsDanhGia[0].UserDanhGia_Group.Id != ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia.Id) // người đánh giá có cấp bậc cao nhất khác với nhóm đã đánh giá có cấp bậc cao nhất
                                                                result += ListAllUserInDonVi[i].WebUser.UserName + ": " + ListAllUserInDonVi[i].WebUser.StaffInfo.StaffProfile.Name + ".\n";
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        default: // 0: Năm
                                            {
                                                switch (ObjDieuKienBoTieuChi.HoanThanhBoTieuChi.LoaiBoTieuChi)
                                                {
                                                    case 1: // 6 Tháng
                                                        break;
                                                    case 2: // Quý
                                                        break;
                                                    case 3: // Tháng
                                                        break;
                                                    default: // 0: Năm
                                                        {
                                                            if (ListAllUserInDonVi.Count > 0)
                                                            {
                                                                result += ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia.Name + " chưa hoàn thành bộ tiêu chí '" + ObjDieuKienBoTieuChi.HoanThanhBoTieuChi.Name + "': \n";
                                                                for (int i = 0; i < ListAllUserInDonVi.Count; i++)
                                                                {
                                                                    List<ABC_DanhGia> ListIsDanhGia = session.Query<ABC_DanhGia>().
                                                                                                                Where(e => e.BoTieuChi.Id == ObjDieuKienBoTieuChi.HoanThanhBoTieuChi.Id &&
                                                                                                                            e.IsLock == true &&
                                                                                                                            e.KyDanhGia.Id == kyDanhGiaId &&
                                                                                                                            e.UserDuocDanhGia.WebUser.Id == ListAllUserInDonVi[i].WebUser.Id).
                                                                                                                OrderBy(e => e.UserDanhGia_Group.STT).
                                                                                                                ToList();
                                                                    if (ListIsDanhGia.Count == 0 || ListIsDanhGia[0].UserDanhGia_Group.Id != ObjDieuKienHoanThanhBoTieuChiRole.GroupDanhGia.Id) // người đánh giá có cấp bậc cao nhất khác với nhóm đã đánh giá có cấp bậc cao nhất
                                                                        result += ListAllUserInDonVi[i].WebUser.UserName + ": " + ListAllUserInDonVi[i].WebUser.StaffInfo.StaffProfile.Name + ".\n";
                                                                }
                                                            }
                                                            break;
                                                        }
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_DieuKienBoTieuChiApi/GetCheckDieuKienBoTieuChi", ex); throw ex; }

            return result;
        }
        [Authorize]
        [Route("")]
        public int PutSave(List<ABC_DieuKienBoTieuChiDTO> list)
        {
            int result = 0;
            try
            {
                SessionManager.DoWork(session =>
                {
                    // Xóa điều kiện cũ
                    List<ABC_DieuKienBoTieuChi> ListDel = session.Query<ABC_DieuKienBoTieuChi>().Where(e => e.BoTieuChi.Id == list[0].BoTieuChiId).ToList();
                    foreach (ABC_DieuKienBoTieuChi ObjDel in ListDel)
                    {
                        session.Delete(ObjDel);
                    }

                    // Thêm lại điều kiện mới
                    if (list != null && list.Count != 0)
                    {
                        foreach (ABC_DieuKienBoTieuChiDTO Obj in list)
                        {
                            if (Obj.HoanThanhBoTieuChiId != null)
                            {
                                ABC_DieuKienBoTieuChi ObjSave = new ABC_DieuKienBoTieuChi();
                                ObjSave.Id = Guid.NewGuid();
                                ObjSave.BoTieuChi = new ABC_BoTieuChi() { Id = Obj.BoTieuChiId.Value };
                                ObjSave.HoanThanhBoTieuChi = new ABC_BoTieuChi() { Id = Obj.HoanThanhBoTieuChiId.Value };
                                ObjSave.LoaiHoanThanh = Obj.LoaiHoanThanh;
                                session.Save(ObjSave);
                            }
                        }
                    }
                });
                result = 1;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_DieuKienBoTieuChiApi/PutSave", ex); throw ex;
            }
            return result;

        }
    }
}
