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
    public class ABC_ChiTietKetQuaApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public bool PutNewChiTietKetQua(Guid ChiTietNhanVienDanhGiaId, List<ABC_TieuChiDanhGiaDTO> BoTieuChi)
        {

            foreach (ABC_TieuChiDanhGiaDTO item in BoTieuChi)
            {
                // kiểm tra xem chi tiết đánh giá đã tồn tại trong bảng chi tiết kết quả hay chưa nếu chưa thì true.
                if (IsNotExistTieuChiDanhGia(ChiTietNhanVienDanhGiaId, item.Id) == true)
                    SessionManager.DoWork(session =>
                    {
                        ABC_KetQuaChiTiet objsave = new ABC_KetQuaChiTiet();
                        objsave.Id = Guid.NewGuid();
                        objsave.TieuChiDanhGia = new ABC_TieuChiDanhGia() { Id = item.Id };
                        objsave.ChiTietNhanVienDanhGia = new ABC_ChiTietNhanVienDanhGia() { Id = ChiTietNhanVienDanhGiaId };
                        objsave.GhiChu = "";
                        session.Save(objsave);
                    });
            }
            return true;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_KetQuaChiTietDTO> GetListChiTietKetQua(Guid KyDanhGiaId, Guid NhanVienDuocDanhGiaId, Guid NhanVienDanhGiaId, int LoaiDanhGia)
        {
            List<ABC_KetQuaChiTietDTO> result = new List<ABC_KetQuaChiTietDTO>();

            ABC_DanhGiaDTO DanhGia = ABC_BoDanhGiaApiController.Instance.getBoDanhGiaByTimeNow(KyDanhGiaId, LoaiDanhGia);

            List<ABC_TieuChiDanhGiaDTO> ListTieuChi = ABC_TieuChiDanhGiaApiController.Instance.GetTieuChiDanhGiaById(DanhGia.Id).ToList();

            ABC_ChiTietNhanVienDanhGiaDTO ChiTietNhanVienDanhGia = ABC_ChiTietNhanVienDanhGiaApiController.Instance.getChiTietNhanVienDanhGia(DanhGia.Id, KyDanhGiaId, NhanVienDuocDanhGiaId, NhanVienDanhGiaId, LoaiDanhGia);


            result = getListByRef(ChiTietNhanVienDanhGia.Id);

            if (result.Count == 0)
            {
                PutNewChiTietKetQua(ChiTietNhanVienDanhGia.Id, ListTieuChi);
                result = getListByRef(ChiTietNhanVienDanhGia.Id);
            }

            return result;
        }

        [Authorize]
        [Route("")]
        public List<ABC_KetQuaChiTietDTO> getListByRef(Guid ChiTietNhanVienDanhGiaId)
        {
            List<ABC_KetQuaChiTietDTO> result = new List<ABC_KetQuaChiTietDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_KetQuaChiTiet>().Where(e => e.ChiTietNhanVienDanhGia.Id == ChiTietNhanVienDanhGiaId).OrderBy(e => e.TieuChiDanhGia.STT).Map<ABC_KetQuaChiTietDTO>().ToList();
            });
            return result;
        }
        public bool IsNotExistTieuChiDanhGia(Guid ChiTietNhanVienDanhGiaId, Guid TieuChiDanhGiaId)
        {// Get chitietketqua by ketquaId and ChiTietDanhGiaId
            int Dem = 0;
            SessionManager.DoWork(session =>
            {
                Dem = session.Query<ABC_KetQuaChiTiet>().Where(e => e.ChiTietNhanVienDanhGia.Id == ChiTietNhanVienDanhGiaId && e.TieuChiDanhGia.Id == TieuChiDanhGiaId).Count();
            });
            if (Dem == 0)
                return true; // true là chưa tồn tại
            return false; // flase đã tồn tại và không được insert vào

        }

        [Authorize]
        [Route("")]
        public bool PutSaveChiTietKetQua(List<ABC_KetQuaChiTietDTO> Obj)
        {
            foreach (ABC_KetQuaChiTietDTO item in Obj)
            {
                SessionManager.DoWork(session =>
                {
                    ABC_KetQuaChiTiet objsave = new ABC_KetQuaChiTiet();
                    objsave.Id = item.Id;
                    objsave.ChiTietNhanVienDanhGia = new ABC_ChiTietNhanVienDanhGia() { Id = item.ChiTietNhanVienDanhGiaId };
                    objsave.GhiChu = item.GhiChu;
                    objsave.Diem = item.Diem;
                    objsave.IsChecked = item.IsChecked;
                    objsave.YKienDanhGia = item.YKienDanhGia;
                    objsave.TieuChiDanhGia = new ABC_TieuChiDanhGia() { Id = item.TieuChiDanhGiaId };
                    //if (item.TieuChiDanhGiaParentId != Guid.Empty)
                    //{
                    //    objsave.Parent = getByRef(item.TieuChiDanhGiaParentId, item.ChiTietNhanVienDanhGiaId);
                    //    objsave.Parent.IsChecked = item.ParentIsChecked;
                    //}
                    session.Update(objsave);
                });
            }
            return true;
        }
        public void UpdateChiTietKetQua(ABC_KetQuaChiTiet obj)
        {
            SessionManager.DoWork(session =>
            {
                session.Update(obj);
            });
        }

        public ABC_KetQuaChiTiet getByRef(Guid TieuChiDanhGiaParentId, Guid ChiTietNhanVienDanhGiaId)
        {
            ABC_KetQuaChiTiet result = new ABC_KetQuaChiTiet();
            SessionManager.DoWork(session =>
            {
                result =  session.Query<ABC_KetQuaChiTiet>().Where(e => e.ChiTietNhanVienDanhGia.Id == ChiTietNhanVienDanhGiaId && e.TieuChiDanhGia.Id == TieuChiDanhGiaParentId).FirstOrDefault();
            });
            return result;
        }
    }
}
