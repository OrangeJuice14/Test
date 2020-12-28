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
    public class ABC_GroupDanhGiaApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_GroupDanhGiaDTO> GetAll()
        {
            List<ABC_GroupDanhGiaDTO> result = new List<ABC_GroupDanhGiaDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    result = session.Query<ABC_GroupDanhGia>().
                                    Where(e => e.GCRecord == null).
                                    OrderBy(e => e.STT).ThenBy(e => e.Name).
                                    Map<ABC_GroupDanhGiaDTO>().ToList();
                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_GroupDanhGiaApi/GetAll", ex); throw ex; }
            return result;
        }

        [Authorize]
        [Route("")]
        public int PutSaveOrUpdate(ABC_GroupDanhGiaDTO obj)
        {
            int result = 0;
            try
            {
                ABC_GroupDanhGia objSave = new ABC_GroupDanhGia();
                if (obj.Id == Guid.Empty)
                {
                    objSave.Id = Guid.NewGuid();
                    objSave.AddUserId = obj.AddUserId;
                    objSave.AddTime = DateTime.Now;
                }
                else
                {
                    objSave.Id = obj.Id;
                    objSave.LastEditUserId = obj.LastEditUserId;
                    objSave.LastEditTime = DateTime.Now;
                }
                objSave.Name = obj.Name;
                objSave.STT = obj.STT;
                objSave.DaiDienDanhGia = obj.DaiDienDanhGia;
                objSave.TuDanhGia = obj.TuDanhGia;
                if (obj.DaiDienDanhGia.HasValue && obj.DaiDienDanhGia.Value)
                    objSave.TuDanhGia = true;
                objSave.DanhGiaCapDuoi = obj.DanhGiaCapDuoi;
                objSave.HasQuanLyDonVi = obj.HasQuanLyDonVi;
                SessionManager.DoWork(session =>
                {
                    session.SaveOrUpdate(objSave);
                    result = 1;
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_GroupDanhGiaApi/PutSaveOrUpdate", ex); throw ex;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public int Delete(ABC_GroupDanhGiaDTO obj, Guid userId)
        {
            int result = 0;
            try
            {
                SessionManager.DoWork(session =>
                {
                    //obj.GCRecord = ;
                    ABC_GroupDanhGia ObjDelete = session.Query<ABC_GroupDanhGia>().SingleOrDefault(e => e.Id == obj.Id);
                    ObjDelete.GCRecord = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    ObjDelete.TimeDelete = DateTime.Now;
                    ObjDelete.UserDeleteId = userId;
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
