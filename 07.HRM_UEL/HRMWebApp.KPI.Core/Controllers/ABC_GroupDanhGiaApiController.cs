using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;


namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_GroupDanhGiaApiController : ApiController
    {
        public IEnumerable<ABC_GroupDanhGia> GetAll(ISession session)
        {
            List<ABC_GroupDanhGia> result = new List<ABC_GroupDanhGia>();
            result = session.Query<ABC_GroupDanhGia>().
                            Where(e => e.GCRecord == null).
                            OrderBy(e => e.STT).ThenBy(e => e.Name).ToList();
            return result;
        }
        public ABC_GroupDanhGia GetById(ISession session, Guid Id)
        {
            ABC_GroupDanhGia result = new ABC_GroupDanhGia();
            result = session.Query<ABC_GroupDanhGia>().Single(e => e.GCRecord == null && e.Id == Id);
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_GroupDanhGiaVMDTO> GetAllDTO()
        {
            List<ABC_GroupDanhGiaVMDTO> result = new List<ABC_GroupDanhGiaVMDTO>();
            try
            {
                SessionManager.DoWorkNoTransaction(session =>
                {
                    result = GetAll(session).Map<ABC_GroupDanhGiaVMDTO>().ToList();
                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_GroupDanhGiaApi/GetAllDTO", ex); throw ex; }
            return result;
        }
        [Authorize]
        [Route("")]
        public ABC_GroupDanhGiaVMDTO GetDTOById(Guid id)
        {
            ABC_GroupDanhGiaVMDTO result = new ABC_GroupDanhGiaVMDTO();
            try
            {
                SessionManager.DoWorkNoTransaction(session =>
                {
                    result = GetById(session, id).Map<ABC_GroupDanhGiaVMDTO>();
                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_GroupDanhGiaApi/GetDTOById", ex); throw ex; }
            return result;
        }

        [Authorize]
        [Route("")]
        public int Put(Guid? id, Guid userId, [FromBody]ABC_GroupDanhGiaCreateDTO obj)
        {
            int result = 0;
            DateTime DateTimeNow = DateTime.Now;
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_GroupDanhGia objSave = new ABC_GroupDanhGia();
                    if (id == Guid.Empty || id == null || id != obj.Id)
                    {
                        objSave.Id = Guid.NewGuid();
                        objSave.AddTime = DateTimeNow;
                        objSave.AddUserId = userId;
                    }
                    else
                    {
                        objSave = GetById(session, id.Value);
                        objSave.LastEditTime = DateTimeNow;
                        objSave.LastEditUserId = userId;
                    }
                    objSave.Name = obj.Name;
                    objSave.STT = obj.STT;
                    objSave.DaiDienDanhGia = obj.DaiDienDanhGia;
                    objSave.TuDanhGia = obj.TuDanhGia;
                    if (obj.DaiDienDanhGia.HasValue && obj.DaiDienDanhGia.Value)
                        objSave.TuDanhGia = true;
                    objSave.SoLuongSinhVien = obj.SoLuongSinhVien;
                    objSave.SoLuongGiangVien = obj.SoLuongGiangVien;
                    objSave.DanhGiaCapDuoi = obj.DanhGiaCapDuoi;
                    objSave.HasQuanLyDonVi = obj.HasQuanLyDonVi;
                    objSave.HasDieuKienPhu = obj.HasDieuKienPhu;
                    objSave.HeSoNgachDamNhiem = obj.HeSoNgachDamNhiem;
                    objSave.HeSoQuanLy = obj.HeSoQuanLy;
                    objSave.HeSoNgachDamNhiemHasDieuKien = obj.HeSoNgachDamNhiemHasDieuKien;
                    objSave.HeSoQuanLyHasDieuKien = obj.HeSoQuanLyHasDieuKien;
                    session.SaveOrUpdate(objSave);
                    result = 1;
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_GroupDanhGiaApi/Put", ex); throw ex;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public int Delete(ABC_GroupDanhGiaVMDTO obj, Guid userId)
        {
            int result = 0;
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_GroupDanhGia ObjDelete = GetById(session, obj.Id);
                    ObjDelete.GCRecord = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    ObjDelete.DeleteTime = DateTime.Now;
                    ObjDelete.DeleteUserId = userId;
                    session.Update(ObjDelete);
                });
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                Helper.ErrorLog("ABC_GroupDanhGiaApi/Delete", ex); throw ex;
            }
            return result;
        }
    }
}
