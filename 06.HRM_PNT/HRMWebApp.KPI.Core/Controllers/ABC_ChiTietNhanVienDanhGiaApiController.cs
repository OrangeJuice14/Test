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
    public class ABC_ChiTietNhanVienDanhGiaApiController : ApiController
    {
        private static ABC_ChiTietNhanVienDanhGiaApiController instance;

        public static ABC_ChiTietNhanVienDanhGiaApiController Instance
        {
            get { if (instance == null) instance = new ABC_ChiTietNhanVienDanhGiaApiController(); return ABC_ChiTietNhanVienDanhGiaApiController.instance; }
            private set { ABC_ChiTietNhanVienDanhGiaApiController.instance = value; }
        }

        public ABC_ChiTietNhanVienDanhGiaDTO getChiTietNhanVienDanhGia(Guid DanhGiaId,Guid KyDanhGiaId, Guid NhanVienDuocDanhGiaId, Guid NhanVienDanhGiaId, int LoaiDanhGia)
        {
            ABC_ChiTietNhanVienDanhGiaDTO Obj = new ABC_ChiTietNhanVienDanhGiaDTO();
            ABC_KetQuaDTO ObjKetQua = ABC_KetQuaApiController.Instance.GetKetQua(KyDanhGiaId, NhanVienDuocDanhGiaId);
            SessionManager.DoWork(session =>
            {
                Obj = session.Query<ABC_ChiTietNhanVienDanhGia>()
                        .Where(e => e.KetQua.Id == ObjKetQua.Id
                                && e.NhanVienDanhGia.Id == NhanVienDanhGiaId
                                && e.LoaiDanhGia.Id == ABC_LoaiDanhGiaApiController.Instance.getLoaiDanhGia(LoaiDanhGia).Id
                                && e.DanhGia.Id == DanhGiaId)
                        .Map<ABC_ChiTietNhanVienDanhGiaDTO>()
                        .FirstOrDefault();
            });
            if (Obj == null)
                return saveChiTietNhanVienDanhGia(ObjKetQua, DanhGiaId, NhanVienDanhGiaId, LoaiDanhGia);
            return Obj;
        }
        public ABC_ChiTietNhanVienDanhGiaDTO saveChiTietNhanVienDanhGia(ABC_KetQuaDTO ObjKetQua,Guid DanhGiaId, Guid NhanVienDanhGiaId, int LoaiDanhGia)
        {
            ABC_ChiTietNhanVienDanhGia Obj = new ABC_ChiTietNhanVienDanhGia();
            Obj.Id = Guid.NewGuid();
            Obj.DanhGia = new ABC_DanhGia() { Id = DanhGiaId };
            Obj.KetQua = new ABC_KetQua() { Id = ObjKetQua.Id };
            Obj.NhanVienDanhGia = new Staff() { Id = NhanVienDanhGiaId };
            Obj.LoaiDanhGia = ABC_LoaiDanhGiaApiController.Instance.getLoaiDanhGia(LoaiDanhGia);
            SessionManager.DoWork(session =>
            {
                session.Save(Obj);
            });
            return getChiTietNhanVienDanhGia(DanhGiaId, ObjKetQua.KyDanhGiaId, ObjKetQua.NhanVienDuocDanhGiaId, NhanVienDanhGiaId, LoaiDanhGia);
        }

        public ABC_ChiTietNhanVienDanhGia GetChiTietNhanVienDanhGiaById(Guid id)
        {
            ABC_ChiTietNhanVienDanhGia result = new ABC_ChiTietNhanVienDanhGia();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_ChiTietNhanVienDanhGia>().Where(e => e.Id == id).FirstOrDefault();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public bool PutUpdateDTO(ABC_ChiTietNhanVienDanhGiaDTO Obj)
        {
            ABC_ChiTietNhanVienDanhGia result = new ABC_ChiTietNhanVienDanhGia();
            result.Id = Obj.Id;
            result.KetQua = new ABC_KetQua() { Id = Obj.KetQuaId };
            result.LoaiDanhGia = new ABC_LoaiDanhGia() { Id = Obj.LoaiDanhGiaId };
            result.NhanVienDanhGia = new Staff() { Id = Obj.NhanVienDanhGiaId };
            result.DanhGia = new ABC_DanhGia() { Id = Obj.DanhGiaId };
            result.TongDiem = Obj.TongDiem;
            result.YKienDongGop = Obj.YKienDongGop;
            result.isLock = Obj.isLock;
            result.TimeLock = Obj.TimeLock;
            SessionManager.DoWork(session => 
            {
                session.Update(result);
            });
            return true;
        }
        [Authorize]
        [Route("")]
        public bool PutUpdate(ABC_ChiTietNhanVienDanhGia Obj)
        {
            
            SessionManager.DoWork(session => 
            {
                session.Update(Obj);
            });
            return true;
        }
        [Authorize]
        [Route("")]
        public ABC_ChiTietNhanVienDanhGiaDTO getById(Guid Id)
        {
            ABC_ChiTietNhanVienDanhGiaDTO result = new ABC_ChiTietNhanVienDanhGiaDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_ChiTietNhanVienDanhGia>().Where(e => e.Id == Id).FirstOrDefault().Map<ABC_ChiTietNhanVienDanhGiaDTO>();
            });
            return result;
        }
        
        [Authorize]
        [Route("")]
        public ABC_ChiTietNhanVienDanhGiaDTO getByRef(Guid KetQuaId,Guid NhanVienDanhGiaId,Guid LoaiDanhGiaId)
        {
            ABC_ChiTietNhanVienDanhGiaDTO result = new ABC_ChiTietNhanVienDanhGiaDTO();
            ABC_ChiTietNhanVienDanhGia Obj = new ABC_ChiTietNhanVienDanhGia();
            SessionManager.DoWork(session =>
            {
                Obj = session.Query<ABC_ChiTietNhanVienDanhGia>().Where(e => e.KetQua.Id == KetQuaId && e.NhanVienDanhGia.Id == NhanVienDanhGiaId && e.LoaiDanhGia.Id == LoaiDanhGiaId).FirstOrDefault();
            });
            if (Obj == null)
                return null;
            return Obj.Map<ABC_ChiTietNhanVienDanhGiaDTO>();
        }
        
        [Authorize]
        [Route("")]
        public ABC_ChiTietNhanVienDanhGiaDTO getWithTruongPhong(Guid KetQuaId)
        {
            ABC_ChiTietNhanVienDanhGiaDTO result = new ABC_ChiTietNhanVienDanhGiaDTO();
            ABC_ChiTietNhanVienDanhGia Obj = new ABC_ChiTietNhanVienDanhGia();
            SessionManager.DoWork(session =>
            {
                Obj = session.Query<ABC_ChiTietNhanVienDanhGia>().Where(e => e.KetQua.Id == KetQuaId && e.LoaiDanhGia.MaLoai == 3).FirstOrDefault();
            });
            if (Obj == null)
                return null;
            return Obj.Map<ABC_ChiTietNhanVienDanhGiaDTO>();
        }
    }
}