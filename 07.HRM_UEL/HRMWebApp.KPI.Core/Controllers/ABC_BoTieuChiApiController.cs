using System;
using System.Collections.Generic;
using System.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.Core.DTO.ABC.New;
using NHibernate;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_BoTieuChiApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_BoTieuChiVMDTO> GetList()
        {
            List<ABC_BoTieuChiVMDTO> ListReusult = new List<ABC_BoTieuChiVMDTO>();
            SessionManager.DoWorkNoTransaction(session =>
            {
                ListReusult = session.Query<ABC_BoTieuChi>().Where(e => e.GCRecord == null).OrderBy(e => e.LoaiBoTieuChi).ThenBy(e => e.TuNgay).ThenBy(e => e.DenNgay).ToList().Map<ABC_BoTieuChiVMDTO>();
            });
            return ListReusult;
        }

        [Authorize]
        [Route("")]
        public ABC_BoTieuChiVMDTO GetById(Guid id)
        {
            ABC_BoTieuChiVMDTO Reusult = new ABC_BoTieuChiVMDTO();
            SessionManager.DoWorkNoTransaction(session =>
            {
                Reusult = session.Query<ABC_BoTieuChi>().Single(e => e.Id == id).Map<ABC_BoTieuChiVMDTO>();
            });
            return Reusult;
        }

        [Authorize]
        [Route("")]
        public int Put(Guid? id, Guid userId, [FromBody]ABC_BoTieuChiCreateDTO obj)
        {
            int result = 0;
            DateTime DateTimeNow = DateTime.Now;
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_BoTieuChi BoTieuChi = new ABC_BoTieuChi();
                    if (id == Guid.Empty || id == null)
                    {
                        BoTieuChi.Id = Guid.NewGuid();
                        BoTieuChi.AddTime = DateTimeNow;
                        BoTieuChi.AddUserId = userId;
                    }
                    else
                    {
                        BoTieuChi.Id = id.Value;
                        BoTieuChi.LastEditTime = DateTimeNow;
                        BoTieuChi.LastEditUserId = userId;
                    }
                    BoTieuChi.Name = obj.Name;
                    BoTieuChi.ShowDonVi = obj.ShowDonVi;
                    BoTieuChi.ShowTen = obj.ShowTen;
                    BoTieuChi.Status = obj.Status;
                    BoTieuChi.TuNgay = obj.TuNgay;
                    BoTieuChi.DenNgay = obj.DenNgay;
                    BoTieuChi.LoaiBoTieuChi = obj.LoaiBoTieuChi;
                    session.SaveOrUpdate(BoTieuChi);

                    Log(BoTieuChi, DateTimeNow, session);

                });
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                Helper.ErrorLog("ABC_BoTieuChiApi/Put?id=" + id + "&userId=" + userId, ex); throw ex;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public int DeleteById(Guid id, Guid userId)
        {
            int result = 0;
            DateTime DateTimeNow = DateTime.Now;
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_BoTieuChi BoTieuChi = new ABC_BoTieuChi();
                    BoTieuChi = session.Query<ABC_BoTieuChi>().Single(e => e.Id == id);
                    BoTieuChi.GCRecord = Convert.ToInt64(DateTimeNow.ToString("yyyyMMddHHmmss"));
                    BoTieuChi.DeleteTime = DateTimeNow;
                    BoTieuChi.DeleteUserId = userId;
                    session.Update(BoTieuChi);

                    Log(BoTieuChi, DateTimeNow, session);
                });
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                Helper.ErrorLog("ABC_BoTieuChiApi/DeleteById?id=" + id + "&userId=" + userId, ex); throw ex;
            }
            return result;
        }
        public void Log(ABC_BoTieuChi boTieuChi, DateTime dateTimeNow, ISession session)
        {
            ABC_LogBoTieuChi LogBoTieuChi = new ABC_LogBoTieuChi()
            {
                Id = Guid.NewGuid(),
                Name = boTieuChi.Name,
                ShowDonVi = boTieuChi.ShowDonVi,
                ShowTen = boTieuChi.ShowTen,
                Status = boTieuChi.Status,
                TuNgay = boTieuChi.TuNgay,
                DenNgay = boTieuChi.DenNgay,
                BoTieuChiId = boTieuChi.Id,
                LoaiBoTieuChi = boTieuChi.LoaiBoTieuChi,
                AddTime = boTieuChi.AddTime,
                AddUserId = boTieuChi.AddUserId,
                DeleteTime = boTieuChi.DeleteTime,
                DeleteUserId = boTieuChi.DeleteUserId,
                GCRecord = boTieuChi.GCRecord,
                LastEditTime = boTieuChi.LastEditTime,
                LastEditUserId = boTieuChi.LastEditUserId,
                TimeLog = dateTimeNow
            };
            session.Save(LogBoTieuChi);
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_BoTieuChiVMDTO> GetBoTieuChiTuDanhGia(Guid userId, Guid groupDanhGiaId, Guid kyDanhGiaId)
        {
            List<ABC_BoTieuChiVMDTO> result = new List<ABC_BoTieuChiVMDTO>();
            try
            {
                SessionManager.DoWorkNoTransaction(session =>
                {
                    ABC_KyDanhGia KyDanhGia = session.Query<ABC_KyDanhGia>().SingleOrDefault(e => e.Id == kyDanhGiaId);
                    List<ABC_BoTieuChiVMDTO> ListBoTieuChi = session.Query<ABC_Role_BoTieuChi>().
                                                                    Where(e => e.GroupTuDanhGia.Id == groupDanhGiaId &&
                                                                                e.BoTieuChi.TuNgay <= DateTime.Now.Date &&
                                                                                e.BoTieuChi.DenNgay >= DateTime.Now.Date &&
                                                                                ((e.BoTieuChi.TuNgay <= KyDanhGia.TuNgay &&
                                                                                (e.BoTieuChi.DenNgay >= KyDanhGia.DenNgay || e.BoTieuChi.DenNgay >= KyDanhGia.TuNgay)) ||
                                                                                (e.BoTieuChi.TuNgay > KyDanhGia.TuNgay && e.BoTieuChi.TuNgay <= KyDanhGia.DenNgay)) &&
                                                                                e.BoTieuChi.LoaiBoTieuChi == KyDanhGia.Loai).
                                                                    Select(e => e.BoTieuChi).
                                                                    Distinct().
                                                                    Map<ABC_BoTieuChiVMDTO>().
                                                                    ToList();
                    result = ListBoTieuChi;
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_BoTieuChiApi/GetBoTieuChiTuDanhGia ", ex);
                throw ex;
            }
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_BoTieuChiVMDTO> GetBoTieuChiDanhGiaDongNghiep(Guid userId, Guid groupDanhGiaId, Guid kyDanhGiaId)
        {
            List<ABC_BoTieuChiVMDTO> result = new List<ABC_BoTieuChiVMDTO>();
            DateTime DateTimeNow = DateTime.Now;
            try
            {
                SessionManager.DoWorkNoTransaction(session =>
                {
                    ABC_KyDanhGia KyDanhGia = session.Query<ABC_KyDanhGia>().SingleOrDefault(e => e.Id == kyDanhGiaId);
                    List<ABC_BoTieuChiVMDTO> ListBoTieuChi = session.Query<ABC_Role_BoTieuChi>().
                                                                    Where(e => e.GroupDanhGia.Id == groupDanhGiaId &&
                                                                                e.BoTieuChi.TuNgay <= DateTimeNow.Date &&
                                                                                e.BoTieuChi.DenNgay >= DateTimeNow.Date &&
                                                                                ((e.BoTieuChi.TuNgay <= KyDanhGia.TuNgay &&
                                                                                (e.BoTieuChi.DenNgay >= KyDanhGia.DenNgay || e.BoTieuChi.DenNgay >= KyDanhGia.TuNgay)) ||
                                                                                (e.BoTieuChi.TuNgay > KyDanhGia.TuNgay && e.BoTieuChi.TuNgay <= KyDanhGia.DenNgay)) &&
                                                                                e.BoTieuChi.LoaiBoTieuChi == KyDanhGia.Loai).
                                                                    Select(e => e.BoTieuChi).
                                                                    Distinct().
                                                                    Map<ABC_BoTieuChiVMDTO>().
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
    }
}
