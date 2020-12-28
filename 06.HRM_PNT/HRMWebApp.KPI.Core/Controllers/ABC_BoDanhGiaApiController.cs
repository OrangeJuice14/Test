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
    public class ABC_BoDanhGiaApiController : ApiController
    {
        private static ABC_BoDanhGiaApiController instance;

        public static ABC_BoDanhGiaApiController Instance
        {
            get { if (instance == null) instance = new ABC_BoDanhGiaApiController(); return ABC_BoDanhGiaApiController.instance; }
            private set { ABC_BoDanhGiaApiController.instance = value; }
        }

        [Authorize]
        [Route("")]
        public ABC_DanhGiaDTO PutNewBoDanhGia(ABC_DanhGiaDTO obj)
        {

            ABC_DanhGia objsave = new ABC_DanhGia();
            objsave.Id = Guid.NewGuid();
            objsave.MoTa = obj.MoTa;
            objsave.LoaiBoDanhGia = new ABC_LoaiBoDanhGia() { Id = obj.LoaiBoDanhGiaId };
            objsave.TuNgay = obj.TuNgay;
            objsave.DenNgay = obj.DenNgay;
            SessionManager.DoWork(session =>
            {
                session.Save(objsave);
            });
            return objsave.Map<ABC_DanhGiaDTO>();
        }
        
        [Authorize]
        [Route("")]
        public bool PutUpdate(ABC_DanhGiaDTO obj)
        {

            ABC_DanhGia objsave = new ABC_DanhGia();
            objsave.Id = obj.Id;
            objsave.MoTa = obj.MoTa;
            if(obj.LoaiBoDanhGiaId != Guid.Empty)
                objsave.LoaiBoDanhGia = new ABC_LoaiBoDanhGia() { Id = obj.LoaiBoDanhGiaId };
            objsave.IsLock = obj.IsLock;
            objsave.TuNgay = obj.TuNgay;
            objsave.DenNgay = obj.DenNgay;
            SessionManager.DoWork(session =>
            {
                session.Update(objsave);
            });
            return true;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_DanhGiaDTO> GetListBoDanhGiaByNam(int Nam)
        {
            List<ABC_DanhGiaDTO> result = new List<ABC_DanhGiaDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_DanhGia>().Where(e => e.TuNgay.Year == Nam || e.DenNgay.Year == Nam).Map<ABC_DanhGiaDTO>().OrderBy(e => e.TuNgay).ToList();
            });
            return result;
        }
        
        [Authorize]
        [Route("")]
        public ABC_DanhGiaDTO GetBoDanhGiaById(Guid Id)
        {
            ABC_DanhGiaDTO result = new ABC_DanhGiaDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_DanhGia>().Where(e => e.Id == Id).Map<ABC_DanhGiaDTO>().SingleOrDefault();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public ABC_DanhGiaDTO getBoDanhGiaByTimeNow(Guid KyDanhGiaId, int LoaiDanhGia)
        {
            ABC_KyDanhGia KyDanhGia = ABC_KyDanhGiaApiController.Instance.getKyDanhGiaById(KyDanhGiaId);

            DateTime DateNow = DateTime.Now;
            Guid LoaiBoDanhGiaId = Guid.Empty;
            ABC_DanhGiaDTO result = new ABC_DanhGiaDTO();
            if (KyDanhGia.Loai == 2)
            {
                switch (LoaiDanhGia)
                {
                    case 2:
                        LoaiBoDanhGiaId = new Guid("1644176D-CEAC-4E24-802C-C07718BA98C8");
                        break;
                    default:
                        LoaiBoDanhGiaId = new Guid("1E02E571-F771-4741-AC51-28207756C0EB");
                        break;
                }
            }
            else
            {
                LoaiBoDanhGiaId = new Guid("7B83ED33-DBA6-4378-8BCC-20B108092CDD");
            }
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_DanhGia>().Where(e => e.TuNgay < DateNow && e.DenNgay > DateNow && e.IsLock == true && e.LoaiBoDanhGia.Id == LoaiBoDanhGiaId).Map<ABC_DanhGiaDTO>().SingleOrDefault();
            });
            return result;
        }
        public bool getDelete(Guid Id)
        {
            bool isDelete = ABC_TieuChiDanhGiaApiController.Instance.getDelete(Id);
            if(isDelete == true)
            {
                ABC_DanhGia obj = new ABC_DanhGia() { Id = Id };
                SessionManager.DoWork(session =>
                {
                    session.Delete(obj);
                });
                return true;
            }
            return false;
        }
    }
}
