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
    public class ABC_BoTieuChiApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_BoTieuChiDTO> GetAll()
        {
            List<ABC_BoTieuChiDTO> result = new List<ABC_BoTieuChiDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    result = session.Query<ABC_BoTieuChi>().Where(e => e.GCRecord == null).OrderByDescending(e => e.LoaiBoTieuChi).ThenByDescending(e => e.DenNgay).Map<ABC_BoTieuChiDTO>().ToList();
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_BoTieuChiApi/GetAll ", ex);
                throw ex;
            }

            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_BoTieuChiDTO> GetBoTieuChiTuDanhGiaByUserId(Guid userId, Guid groupDanhGiaId, Guid kyDanhGiaId)
        {
            List<ABC_BoTieuChiDTO> result = new List<ABC_BoTieuChiDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_KyDanhGia KyDanhGia = session.Query<ABC_KyDanhGia>().SingleOrDefault(e => e.Id == kyDanhGiaId);
                    List<ABC_BoTieuChiDTO> ListBoTieuChi = session.Query<ABC_BoTieuChi_Role>().
                                                                    Where(e => e.GroupTuDanhGia.Id == groupDanhGiaId &&
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
                    result = ListBoTieuChi;
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_BoTieuChiApi/GetBoTieuChiTuDanhGiaByUserId ", ex);
                throw ex;
            }
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_BoTieuChiDTO> GetBoTieuChiDanhGiaDongNghiep(Guid userId, Guid groupDanhGiaId, Guid kyDanhGiaId)
        {
            List<ABC_BoTieuChiDTO> result = new List<ABC_BoTieuChiDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_KyDanhGia KyDanhGia = session.Query<ABC_KyDanhGia>().SingleOrDefault(e => e.Id == kyDanhGiaId);
                    List<ABC_BoTieuChiDTO> ListBoTieuChi = session.Query<ABC_BoTieuChi_Role>().
                                                                    Where(e => e.GroupDanhGia.Id == groupDanhGiaId &&
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
                    result = ListBoTieuChi;
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_BoTieuChiApi/GetBoTieuChiDanhGiaDongNghiep ", ex); throw ex;
            }

            return result;
        }
        [Authorize]
        [Route("")]
        public ABC_BoTieuChiDTO GetById(Guid id)
        {
            ABC_BoTieuChiDTO result = new ABC_BoTieuChiDTO();
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_BoTieuChi a = session.Query<ABC_BoTieuChi>().SingleOrDefault(e => e.Id == id);
                    result = session.Query<ABC_BoTieuChi>().SingleOrDefault(e => e.Id == id)?.Map<ABC_BoTieuChiDTO>();
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_BoTieuChiApi/GetById ", ex); throw ex;
            }
            return result;
        }
        [Authorize]
        [Route("")]
        public Guid PutSaveOrUpdate(ABC_BoTieuChiDTO obj)
        {
            ABC_BoTieuChi result = new ABC_BoTieuChi();
            try
            {
                SessionManager.DoWork(session =>
                {
                    DateTime TimeNow = DateTime.Now;
                    if (obj.Id == Guid.Empty)
                    {
                        result.Id = Guid.NewGuid();
                        result.AddUserId = obj.AddUserId;
                        result.AddTime = TimeNow;
                    }
                    else
                    {
                        result = session.Query<ABC_BoTieuChi>().Single(e => e.Id == obj.Id);
                        result.LastEditUserId = obj.LastEditUserId;
                        result.EditLastTime = TimeNow;
                    }
                    result.TuNgay = obj.TuNgay;
                    result.DenNgay = obj.DenNgay;
                    result.Name = obj.Name;
                    result.HasDieuKienDanhGia = obj.HasDieuKienDanhGia;
                    result.ShowTen = obj.ShowTen;
                    result.ShowBoPhan = obj.ShowBoPhan;
                    result.ShowDonVi = obj.ShowDonVi;
                    result.ShowDay = obj.ShowDay;
                    result.ShowMonth = obj.ShowMonth;
                    result.ShowYear = obj.ShowYear;
                    result.LoaiBoTieuChi = obj.LoaiBoTieuChi.GetValueOrDefault();
                    session.SaveOrUpdate(result);

                    ABC_LogBoTieuChi LogBoTieuChi = new ABC_LogBoTieuChi(result, TimeNow);
                    session.Save(LogBoTieuChi);
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_BoTieuChiApi/PutSaveOrUpdate ", ex); throw ex;
            }

            return result.Id;
        }
        [Authorize]
        [Route("")]
        public int GetDeleteById(Guid Id, Guid userId)
        {
            int result = 2;
            try
            {
                SessionManager.DoWork(session =>
                {
                    List<ABC_DanhGia> list = session.Query<ABC_DanhGia>().Where(e => e.BoTieuChi.Id == Id).ToList();
                    if (list.Count == 0) // kiểm tra bộ tiêu chí đã được đánh giá hay chưa
                    {
                        DateTime TimeNow = DateTime.Now;
                        ABC_BoTieuChi objDelete = session.Query<ABC_BoTieuChi>().SingleOrDefault(e => e.Id == Id);
                        objDelete.GCRecord = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                        objDelete.TimeDelete = TimeNow;
                        objDelete.UserDeleteId = userId;
                        session.Update(objDelete);
                        result = 1;

                        //Lưu log
                        ABC_LogBoTieuChi LogBoTieuChi = new ABC_LogBoTieuChi(objDelete, TimeNow);
                        session.Save(LogBoTieuChi);
                    }
                });
            }
            catch (Exception ex)
            {
                result = 0;
                Helper.ErrorLog("ABC_BoTieuChiApi/GetDeleteById ", ex); throw ex;
            }
            return result;
        }
    }
}
