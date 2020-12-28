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
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_KyDanhGiaDTO> GetByYear(int nam)
        {
            List<ABC_KyDanhGiaDTO> result = new List<ABC_KyDanhGiaDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    result = session.Query<ABC_KyDanhGia>().Where(e => e.Nam == nam).OrderBy(e => e.Loai).OrderBy(e => e.TuNgay).Map<ABC_KyDanhGiaDTO>().ToList();
                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_KyDanhGiaApi/GetByYear ", ex); throw ex; }
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_KyDanhGiaDTO> GetAll(int nam)
        {
            List<ABC_KyDanhGiaDTO> result = new List<ABC_KyDanhGiaDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    result = session.Query<ABC_KyDanhGia>().Map<ABC_KyDanhGiaDTO>().ToList();
                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_KyDanhGiaApi/GetAll ", ex); throw ex; }
            return result;
        }
        [Authorize]
        [Route("")]
        public ABC_KyDanhGiaDTO GetById(Guid id)
        {
            ABC_KyDanhGiaDTO result = new ABC_KyDanhGiaDTO();
            try
            {
                SessionManager.DoWork(session =>
                {
                    result = session.Query<ABC_KyDanhGia>().SingleOrDefault(e => e.Id == id).Map<ABC_KyDanhGiaDTO>();
                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_KyDanhGiaApi/GetById ", ex); throw ex; }
            return result;
        }
        [Authorize]
        [Route("")]
        public List<int> GetListNam(int nam)
        {
            List<int> result = new List<int>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    result = session.Query<ABC_KyDanhGia>().Where(e => e.Nam != null).Select(e => e.Nam.Value).Distinct().ToList();
                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_KyDanhGiaApi/GetListNam", ex); throw ex; }
            return result;
        }
        [Authorize]
        [Route("")]
        public bool PutNew(int nam, Guid userId)
        {
            bool result = false;
            List<ABC_KyDanhGia> objOld = new List<ABC_KyDanhGia>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    objOld = session.Query<ABC_KyDanhGia>().Where(e => e.Nam == nam).ToList();
                });

                if (objOld.Count != 0) // Nếu đã có thì không thêm nữa
                    return false;

                SessionManager.DoWork(session =>
                {
                    ABC_KyDanhGia objY = new ABC_KyDanhGia();
                    objY.Id = Guid.NewGuid();
                    Guid ParentId = objY.Id;
                    objY.TuNgay = new DateTime(nam, 1, 1);
                    objY.DenNgay = new DateTime(nam, 12, 31);
                    objY.Nam = nam;
                    objY.Name = "Năm " + nam;
                    objY.AddUserId = userId;
                    objY.AddTime = DateTime.Now;
                    objY.Loai = 0;  // Loai 0: Nam
                                    // Loai 1: 6 thang
                                    // Loai 2: Quy'
                                    // Loai 3: Thang'
                    session.Save(objY);
                    for (int i = 1; i <= 12; i++)
                    {
                        ABC_KyDanhGia objM = new ABC_KyDanhGia();
                        objM.Id = Guid.NewGuid();
                        objM.Parent = new ABC_KyDanhGia() { Id = ParentId };
                        objM.Nam = nam;
                        objM.Loai = 3;
                        objM.Name = "Tháng " + i + " Năm " + nam;
                        objM.TuNgay = new DateTime(nam, i, 1);
                        objM.DenNgay = objM.TuNgay.Value.AddMonths(1).AddDays(-1);
                        objM.AddUserId = userId;
                        objM.AddTime = DateTime.Now;
                        session.Save(objM);
                    }
                    result = true;
                });
            }
            catch (Exception ex)
            {
                result = false;
                Helper.ErrorLog("ABC_KyDanhGiaApi/PutNew", ex); throw ex;
            }
            return result;
        }
        [Authorize]
        [Route("")]
        public int PutSaveOrUpdate(ABC_KyDanhGiaDTO obj, Guid userId)
        {
            int result = 0;
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_KyDanhGia ObjSave = new ABC_KyDanhGia()
                    {
                        Id = obj.Id,
                        DenNgay = obj.DenNgay,
                        TuNgay = obj.TuNgay,
                        NgayBatDauDanhGia = obj.NgayBatDauDanhGia,
                        NgayKetThucDanhGia = obj.NgayKetThucDanhGia,
                        Loai = obj.Loai,
                        Nam = obj.Nam,
                        Name = obj.Name,
                        Parent = obj.ParentId != null ? new ABC_KyDanhGia() { Id = obj.ParentId.Value } : null,
                        AddUserId = userId,
                        AddTime = DateTime.Now,
                    };
                    session.Update(ObjSave);
                });
                result = 1;
            }
            catch (Exception ex) { result = 0; Helper.ErrorLog("", ex); throw ex; }
            return result;
        }
    }
}
