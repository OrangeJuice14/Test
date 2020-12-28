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
    public class ABC_XepLoaiDanhGiaApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_XepLoaiDanhGiaDTO> GetListByBoTieuChiId(Guid boTieuChiId)
        {
            List<ABC_XepLoaiDanhGiaDTO> result = new List<ABC_XepLoaiDanhGiaDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_XepLoaiDanhGia>().Where(e => e.BoTieuChi.Id == boTieuChiId && e.GCRecord == null).OrderByDescending(e => e.TuDiem).Map<ABC_XepLoaiDanhGiaDTO>().ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public ABC_XepLoaiDanhGiaDTO GetByXepLoaiDanhGiaId(Guid xepLoaiDanhGiaId)
        {
            try
            {
                ABC_XepLoaiDanhGiaDTO result = new ABC_XepLoaiDanhGiaDTO();
                SessionManager.DoWork(session =>
                {
                    result = session.Query<ABC_XepLoaiDanhGia>().Single(e => e.Id == xepLoaiDanhGiaId).Map<ABC_XepLoaiDanhGiaDTO>();
                });
                return result;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        [Authorize]
        [Route("")]
        public Guid? PutSaveOrUpdate(ABC_XepLoaiDanhGiaDTO obj)
        {
            ABC_XepLoaiDanhGia ObjSave = new ABC_XepLoaiDanhGia();
            try
            {
                SessionManager.DoWork(session =>
                {

                    ObjSave = obj.Id == Guid.Empty ? new ABC_XepLoaiDanhGia() { Id = Guid.NewGuid()} : session.Query<ABC_XepLoaiDanhGia>().SingleOrDefault(e => e.Id == obj.Id);
                    ObjSave.Name = obj.Name;
                    ObjSave.TuDiem = obj.TuDiem;
                    ObjSave.DenDiem = obj.DenDiem;
                    ObjSave.HasDieuKienPhu = obj.HasDieuKienPhu;
                    ObjSave.HasDieuKienTieuChi = obj.HasDieuKienTieuChi;
                    ObjSave.DiemDat = obj.DiemDat;
                    if(obj.BoTieuChiId.HasValue)
                    {
                        ObjSave.BoTieuChi = new ABC_BoTieuChi() { Id = obj.BoTieuChiId.Value };
                        session.SaveOrUpdate(ObjSave);
                    }
                    else
                    {
                        ObjSave.Id = Guid.Empty;
                    }
                });
            }
            catch (Exception ex)
            {
                ObjSave.Id = Guid.Empty;
                Helper.ErrorLog("ABC_XepLoaiDanhGiaApi/PutSaveOrUpdate ", ex); throw ex;
            }
            return ObjSave.Id;

        }
        [Authorize]
        [Route("")]
        public int Delete(Guid xepLoaiDanhGiaId, Guid userId)
        {
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_XepLoaiDanhGia ObjDel = new ABC_XepLoaiDanhGia();
                    ObjDel = session.Query<ABC_XepLoaiDanhGia>().Single(e => e.Id == xepLoaiDanhGiaId);
                    ObjDel.GCRecord = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    ObjDel.UserDeleteId = userId;
                    session.Update(ObjDel);
                });
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
    }
}
