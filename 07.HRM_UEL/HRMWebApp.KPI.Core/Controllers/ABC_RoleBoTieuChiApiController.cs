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
    public class ABC_RoleBoTieuChiApiController : ApiController
    {
        public List<ABC_Role_BoTieuChi> GetByBoTieuChiId(ISession session, Guid boTieuChiId)
        {
            return session.Query<ABC_Role_BoTieuChi>().Where(e => e.BoTieuChi.Id == boTieuChiId)?.OrderByDescending(e => e.GroupDanhGia.STT).ThenBy(e => e.GroupDanhGia.Name).ToList();
        }

        public List<ABC_Role_BoTieuChi> GetByBoTieuChiIdAndGroupDanhGiaId(ISession session, Guid boTieuChiId, Guid groupTuDanhGiaId)
        {
            return session.Query<ABC_Role_BoTieuChi>().Where(e => e.BoTieuChi.Id == boTieuChiId && e.GroupTuDanhGia.Id == groupTuDanhGiaId)?.ToList();
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_Role_BoTieuChiVMDTO> GetByBoTieuId(Guid boTieuChiId)
        {
            List<ABC_Role_BoTieuChiVMDTO> result = new List<ABC_Role_BoTieuChiVMDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    result = GetByBoTieuChiId(session, boTieuChiId).Map<ABC_Role_BoTieuChiVMDTO>();
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_BoTieuChiRoleApi/GetByBoTieuId", ex); throw ex;
            }

            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_Role_BoTieuChiVMDTO> GetByBoTieuIdAndGroupDanhGiaId(Guid boTieuChiId, Guid groupDanhGiaId)
        {
            List<ABC_Role_BoTieuChiVMDTO> result = new List<ABC_Role_BoTieuChiVMDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_GroupDanhGia groupTuDanhGia = session.Query<ABC_Role_BoTieuChi>().Where(e => e.BoTieuChi.Id == boTieuChiId && e.GroupDanhGia.Id == groupDanhGiaId)?.Select(e => e.GroupTuDanhGia).SingleOrDefault();
                    result = GetByBoTieuChiIdAndGroupDanhGiaId(session, boTieuChiId, groupTuDanhGia.Id).OrderByDescending(e => e.GroupDanhGia.STT).ThenBy(e => e.GroupDanhGia.Name).Map<ABC_Role_BoTieuChiVMDTO>();
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_BoTieuChiRoleApi/GetByBoTieuIdAndGroupDanhGiaId", ex); throw ex;
            }

            return result;
        }

        [Authorize]
        [Route("")]
        public int Put(List<ABC_Role_BoTieuChiCreateDTO> listObj, Guid userId)
        {
            int result = 1;
            try
            {
                if (listObj.Count > 0)
                {
                    SessionManager.DoWork(session =>
                    {
                        List<ABC_Role_BoTieuChi> listDelete = GetByBoTieuChiId(session, listObj[0].BoTieuChiId.Value);
                        foreach (ABC_Role_BoTieuChi obj in listDelete)
                        {
                            session.Delete(obj);
                        }

                        foreach (ABC_Role_BoTieuChiCreateDTO obj in listObj)
                        {
                            ABC_Role_BoTieuChi objSave = new ABC_Role_BoTieuChi();
                            objSave.Id = Guid.NewGuid();
                            if (obj.GroupTuDanhGiaId.HasValue)
                                objSave.GroupTuDanhGia = new ABC_GroupDanhGia() { Id = obj.GroupTuDanhGiaId.Value };
                            if (obj.GroupDanhGiaId.HasValue)
                                objSave.GroupDanhGia = new ABC_GroupDanhGia() { Id = obj.GroupDanhGiaId.Value };
                            objSave.BoTieuChi = new ABC_BoTieuChi() { Id = obj.BoTieuChiId.Value };
                            objSave.UserDanhGiaNgangHang = obj.UserDanhGiaNgangHang == true ? true : false;
                            session.SaveOrUpdate(objSave);
                        }
                    });
                    return 1;
                }
            }
            catch (Exception ex)
            {
                result = 0;
                Helper.ErrorLog("ABC_BoTieuChiRoleApi/PutSaveOrUpdate", ex); throw ex;
            }
            return result;
        }
    }
}
