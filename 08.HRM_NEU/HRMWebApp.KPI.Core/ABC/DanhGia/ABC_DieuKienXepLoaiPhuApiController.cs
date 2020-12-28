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
    public class ABC_DieuKienXepLoaiPhuApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public ABC_DieuKienXepLoaiPhuDTO GetByXepLoaiDanhGiaId(Guid xepLoaiDanhGiaId)
        {
            ABC_DieuKienXepLoaiPhuDTO result = new ABC_DieuKienXepLoaiPhuDTO();
            try
            {
                SessionManager.DoWork(session =>
                {
                    result = session.Query<ABC_DieuKienXepLoaiPhu>().SingleOrDefault(e => e.XepLoaiDanhGia.Id == xepLoaiDanhGiaId).Map<ABC_DieuKienXepLoaiPhuDTO>();
                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_DieuKienXepLoaiPhuApi/GetByXepLoaiDanhGiaId", ex); throw ex; }
            return result;
        }
        [Authorize]
        [Route("")]
        public List<ABC_DieuKienXepLoaiPhuDTO> GetListDieuKienXepLoaiPhuByBoTieuChiId(Guid boTieuChiId)
        {
            List<ABC_DieuKienXepLoaiPhuDTO> result = new List<ABC_DieuKienXepLoaiPhuDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    result = session.Query<ABC_DieuKienXepLoaiPhu>().Where(e => e.XepLoaiDanhGia.BoTieuChi.Id == boTieuChiId).Map<ABC_DieuKienXepLoaiPhuDTO>().ToList();
                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_DieuKienXepLoaiPhuApi/GetListDieuKienXepLoaiPhuByBoTieuChiId", ex); throw ex; }
            return result;
        }

        [Authorize]
        [Route("")]
        public Guid? PutSaveOrUpdate(ABC_DieuKienXepLoaiPhuDTO obj)
        {
            ABC_DieuKienXepLoaiPhu ObjSave = new ABC_DieuKienXepLoaiPhu();
            try
            {
                SessionManager.DoWork(session =>
                {
                    ObjSave = obj.Id == Guid.Empty ? new ABC_DieuKienXepLoaiPhu() { Id = Guid.NewGuid() } : session.Query<ABC_DieuKienXepLoaiPhu>().SingleOrDefault(e => e.Id == obj.Id);
                    if (obj.XepLoaiDanhGiaId.HasValue && obj.TieuChiId.HasValue)
                    {
                        ObjSave.XepLoaiDanhGia = new ABC_XepLoaiDanhGia() { Id = obj.XepLoaiDanhGiaId.Value };
                        ObjSave.TieuChi = new ABC_TieuChi() { Id = obj.TieuChiId.Value };
                        ObjSave.DiemDat = obj.DiemDat;
                        session.SaveOrUpdate(ObjSave);
                    }
                    else // lỗi
                    {
                        ObjSave.Id = Guid.Empty;
                    }
                });
            }
            catch (Exception ex)
            {
                ObjSave.Id = Guid.Empty;
                Helper.ErrorLog("ABC_DieuKienXepLoaiPhuApi/PutSaveOrUpdate", ex); throw ex;
            }

            return ObjSave.Id;
        }
    }
}
