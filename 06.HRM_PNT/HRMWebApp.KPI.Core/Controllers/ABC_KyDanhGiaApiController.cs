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
    public class ABC_KyDanhGiaApiController : ApiController
    {
        private static ABC_KyDanhGiaApiController instance;

        public static ABC_KyDanhGiaApiController Instance
        {
            get { if (instance == null) instance = new ABC_KyDanhGiaApiController(); return ABC_KyDanhGiaApiController.instance; }
            private set { ABC_KyDanhGiaApiController.instance = value; }
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_KyDanhGia> GetListAll()
        {
            List<ABC_KyDanhGia> result = new List<ABC_KyDanhGia>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_KyDanhGia>().ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public ABC_KyDanhGiaDTO getKyDanhGiaDTOById(Guid Id)
        {
            ABC_KyDanhGiaDTO result = new ABC_KyDanhGiaDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_KyDanhGia>().Where(e => e.Id == Id).Map<ABC_KyDanhGiaDTO>().FirstOrDefault();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public ABC_KyDanhGia getKyDanhGiaById(Guid Id)
        {
            ABC_KyDanhGia result = new ABC_KyDanhGia();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_KyDanhGia>().Where(e => e.Id == Id).FirstOrDefault();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_KyDanhGiaDTO> GetListKyDanhGia(int nam) //Quản lý đánh giá ABC
        {
            List<ABC_KyDanhGiaDTO> result = new List<ABC_KyDanhGiaDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_KyDanhGia>().Where(e => e.Nam == nam).Map<ABC_KyDanhGiaDTO>().ToList();
            });
            result = result.OrderBy(r => r.Loai).OrderBy(r => r.TuNgay).ToList();
            return result;
        }
        [Authorize]
        [Route("")]
        public bool GetTaoKyDanhGiaNam(int Nam)
        {

            bool resulft = false;
            if(GetListKyDanhGia(Nam).Count() == 0)
            SessionManager.DoWork(session =>
            {
                ABC_KyDanhGia objsave = new ABC_KyDanhGia();

                DateTime Ngay = new DateTime(Nam - 1, 12, 31);
                objsave.Id = Guid.NewGuid();
                objsave.TuNgay = new DateTime(Nam, 1, 1);
                objsave.DenNgay = new DateTime(Nam, 12, 31);
                objsave.Nam = Nam;
                objsave.Loai = 1;
                objsave.Ten = "Năm " + Nam;
                objsave.NgayTao = DateTime.Now;
                Guid Parent = objsave.Id;
                session.Save(objsave);

                string[] arr = { "Quý I Năm ", "Quý II Năm ", "Quý III Năm ", "Quý IV Năm " };

                for (int i = 0; i < 4; i++)
                {
                    ABC_KyDanhGia objsaveq = new ABC_KyDanhGia();
                    objsaveq.Loai = 2;
                    objsaveq.Parent = new ABC_KyDanhGia() { Id = Parent };
                    objsaveq.Nam = Nam;
                    objsaveq.Id = Guid.NewGuid();
                    Ngay = Ngay.AddDays(1);
                    objsaveq.TuNgay = Ngay;
                    Ngay = Ngay.AddMonths(3).AddDays(-1);
                    objsaveq.DenNgay = Ngay;
                    objsaveq.NgayTao = DateTime.Now;
                    objsaveq.Ten = arr[i] + Nam;
                    session.Save(objsaveq);
                }
                resulft = true;
            });
            return resulft;
        }

        public List<int> GetNam()
        {
            List<int> ListNam = new List<int>();
            SessionManager.DoWork(session => 
            {
                ListNam = session.Query<ABC_KyDanhGia>().Where(e => e.Nam != null).Select(e => e.Nam.Value).Distinct().ToList();
                }); 
            return ListNam;
        }
        public ABC_KyDanhGia DeleteKyDanhGia(ABC_KyDanhGia obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }
    }
}
