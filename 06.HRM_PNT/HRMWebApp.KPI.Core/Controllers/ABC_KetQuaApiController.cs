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
    public class ABC_KetQuaApiController : ApiController
    {
        private static ABC_KetQuaApiController instance;

        public static ABC_KetQuaApiController Instance
        {
            get { if (instance == null) instance = new ABC_KetQuaApiController(); return ABC_KetQuaApiController.instance; }
            private set { ABC_KetQuaApiController.instance = value; }
        }
        [Authorize]
        [Route("")]
        public ABC_KetQuaDTO getKetQuaDanhGia(Guid StaffId, Guid KyDanhGiaId)
        {
            ABC_KetQuaDTO result = new ABC_KetQuaDTO();
            SessionManager.DoWork(sesision =>
            {
                result = sesision.Query<ABC_KetQua>().Where(e => e.KyDanhGia.Id == KyDanhGiaId && e.NhanVienDuocDanhGia.Id == StaffId).FirstOrDefault().Map<ABC_KetQuaDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public ABC_KetQuaDTO getByIdDTO(Guid Id)
        {
            ABC_KetQuaDTO result = new ABC_KetQuaDTO();
            SessionManager.DoWork(sesision =>
            {
                result = sesision.Query<ABC_KetQua>().Where(e => e.Id == Id).FirstOrDefault().Map<ABC_KetQuaDTO>();
            });
            return result;
        }

        public ABC_KetQuaDTO GetKetQua(Guid KyDanhGiaId, Guid StaffId)
        {
            ABC_KetQuaDTO obj = new ABC_KetQuaDTO();
            SessionManager.DoWork(session =>
            {
                obj = session.Query<ABC_KetQua>().Where(e => e.NhanVienDuocDanhGia.Id == StaffId&& e.KyDanhGia.Id == KyDanhGiaId).Map<ABC_KetQuaDTO>().FirstOrDefault();
            });
            if (obj == null)
                return SaveKetQua(KyDanhGiaId, StaffId);
            return obj;
        }

        public ABC_KetQuaDTO SaveKetQua(Guid KyDanhGiaId, Guid StaffId)
        {
            ABC_KetQua Obj = new ABC_KetQua();
            Obj.Id = Guid.NewGuid();
            Obj.KyDanhGia = new ABC_KyDanhGia() { Id = KyDanhGiaId };
            Obj.NhanVienDuocDanhGia = new Staff() { Id = StaffId };
            SessionManager.DoWork(session =>
            {
                session.SaveOrUpdate(Obj);
            });
            return GetKetQua(KyDanhGiaId, StaffId);
        }

        public void UpdateKetQua(ABC_KetQua obj)
        {
            SessionManager.DoWork(session =>
            {
                session.Update(obj);
            });
        }
        public ABC_KetQua GetKetQuaByid(Guid id)
        {
            ABC_KetQua result = new ABC_KetQua();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_KetQua>().Where(e => e.Id == id).FirstOrDefault();
            });
            return result;
        }
    }
}
