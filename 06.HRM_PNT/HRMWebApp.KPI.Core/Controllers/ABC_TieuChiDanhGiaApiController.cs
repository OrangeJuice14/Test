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
    public class ABC_TieuChiDanhGiaApiController : ApiController
    {
        private static ABC_TieuChiDanhGiaApiController instance;

        public static ABC_TieuChiDanhGiaApiController Instance
        {
            get { if (instance == null) instance = new ABC_TieuChiDanhGiaApiController(); return ABC_TieuChiDanhGiaApiController.instance; }
            private set { ABC_TieuChiDanhGiaApiController.instance = value; }
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_TieuChiDanhGiaDTO> GetTieuChiDanhGiaById(Guid BoDanhGiaId)
        {
            List<ABC_TieuChiDanhGiaDTO> result = new List<ABC_TieuChiDanhGiaDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_TieuChiDanhGia>().Where(e => e.DanhGia.Id == BoDanhGiaId).OrderBy(o => o.STT).Map<ABC_TieuChiDanhGiaDTO>().ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public Guid PutTieuChiDanhGia(ABC_TieuChiDanhGiaDTO obj)
        {

            Guid resulft = Guid.Empty;
            SessionManager.DoWork(session =>
            {
                ABC_TieuChiDanhGia objsave = new ABC_TieuChiDanhGia();
                objsave.Id = Guid.NewGuid();
                objsave.STT = obj.STT;
                objsave.TenTieuChi = obj.TenTieuChi;
                objsave.DiemToiDa = obj.DiemToiDa;
                objsave.ChildSelectOne = obj.ChildSelectOne;
                Guid? Parent = obj.ParentId;
                if (obj.ParentId != Guid.Empty && obj.ParentId != null)
                    objsave.Parent =  new ABC_TieuChiDanhGia() { Id = obj.ParentId.Value };
                objsave.DanhGia = new ABC_DanhGia() { Id = obj.DanhGiaId };
                session.Save(objsave);
            });
            return resulft;
        }

        [Authorize]
        [Route("")]
        public bool PutUpdate(ABC_TieuChiDanhGiaDTO obj)
        {
            ABC_TieuChiDanhGia objsave = new ABC_TieuChiDanhGia();
            objsave.Id = obj.Id;
            objsave.STT = obj.STT;
            objsave.TenTieuChi = obj.TenTieuChi;
            objsave.DiemToiDa = obj.DiemToiDa;
            objsave.ChildSelectOne = obj.ChildSelectOne;
            if (obj.ParentId != Guid.Empty && obj.ParentId != null)
                objsave.Parent = new ABC_TieuChiDanhGia() { Id = obj.ParentId.Value };
            objsave.DanhGia = new ABC_DanhGia() { Id = obj.DanhGiaId };

            SessionManager.DoWork(session =>
            {
                session.Update(objsave);
            });
            return true;
        }

        [Authorize]
        [Route("")]
        public bool PutDelete(ABC_TieuChiDanhGiaDTO obj)
        {
            SessionManager.DoWork(session =>
            {
                session.Delete(new ABC_TieuChiDanhGia() { Id = obj.Id });
            });
            return true;

        }

        public bool getDelete(Guid DanhGiaId)
        {
            List<ABC_TieuChiDanhGiaDTO> ListTieuChi = GetTieuChiDanhGiaById(DanhGiaId).ToList();

            foreach(ABC_TieuChiDanhGiaDTO item in ListTieuChi)
            {
                ABC_TieuChiDanhGia Obj = new ABC_TieuChiDanhGia() { Id = item.Id };

                SessionManager.DoWork(session =>
                {
                    session.Delete(Obj);
                });
            }
            return true;
        }
    }
}
