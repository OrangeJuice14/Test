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
    public class ABC_DieuKienTieuChiApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_DieuKienTieuChiDTO> GetByTieuChiId(Guid tieuChiId)
        {
            List<ABC_DieuKienTieuChiDTO> result = new List<ABC_DieuKienTieuChiDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    result = session.Query<ABC_DieuKienTieuChi>().Where(e => e.TieuChi.Id == tieuChiId).Map<ABC_DieuKienTieuChiDTO>().ToList();
                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_DieuKienTieuChiApi/GetByTieuChiId", ex); throw ex; }
            return result;
        }
        [Authorize]
        [Route("")]
        public int PutSave(List<ABC_DieuKienTieuChiDTO> list)
        {
            int result = 0;
            try
            {
                if (list != null)
                SessionManager.DoWork(session =>
                {
                    // Xóa điều kiện cũ
                    List<ABC_DieuKienTieuChi> ListDel = session.Query<ABC_DieuKienTieuChi>().Where(e => e.TieuChi.Id == list[0].TieuChiId).ToList();
                    foreach (ABC_DieuKienTieuChi ObjDel in ListDel)
                    {
                        session.Delete(ObjDel);
                    }
                    ABC_TieuChi ObjTieuChi = session.Query<ABC_TieuChi>().SingleOrDefault(e => e.Id == list[0].TieuChiId);

                    if (ObjTieuChi.IsAutoScore == true && ObjTieuChi.DieuKienLoaiDiem == 1)// DieuKienLoaiDiem = 1: Công thức tính điểm được tính đến Tiêu chí
                    {
                        foreach (ABC_DieuKienTieuChiDTO DieuKienTieuChi in list)
                        {
                            ABC_DieuKienTieuChi ObjSave = new ABC_DieuKienTieuChi();
                            ObjSave.Id = Guid.NewGuid();
                            ObjSave.TieuChi = new ABC_TieuChi() { Id = DieuKienTieuChi.TieuChiId };
                            ObjSave.DieuKienDiemBoTieuChi = new ABC_BoTieuChi() { Id = DieuKienTieuChi.DieuKienDiemBoTieuChiId };
                            ObjSave.DieuKienDiemTieuChi = new ABC_TieuChi() { Id = DieuKienTieuChi.DieuKienDiemTieuChiId };
                            session.Save(ObjSave);
                        }
                    }

                    if (ObjTieuChi.IsAutoScore == true && ObjTieuChi.DieuKienLoaiDiem == 0) //DieuKienLoaiDiem = 0: Tính theo điểm tổng của bộ tiêu chí đã chọn
                    {
                        ABC_DieuKienTieuChi ObjSave = new ABC_DieuKienTieuChi();
                        ObjSave.Id = Guid.NewGuid();
                        ObjSave.TieuChi = new ABC_TieuChi() { Id = list[0].TieuChiId };
                        ObjSave.DieuKienDiemBoTieuChi = new ABC_BoTieuChi() { Id = list[0].DieuKienDiemBoTieuChiId };
                        session.Save(ObjSave);
                    }
                });
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                Helper.ErrorLog("ABC_DieuKienTieuChiApi/PutSave", ex); throw ex;
            }
            return result;
        }
    }
}
