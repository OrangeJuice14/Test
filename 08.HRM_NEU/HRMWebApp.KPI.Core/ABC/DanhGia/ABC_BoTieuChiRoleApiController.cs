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
    public class ABC_BoTieuChiRoleApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_BoTieuChi_RoleDTO> GetByBoTieuId(Guid boTieuChiId)
        {
            List<ABC_BoTieuChi_RoleDTO> result = new List<ABC_BoTieuChi_RoleDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    result = session.Query<ABC_BoTieuChi_Role>().Where(e => e.BoTieuChi.Id == boTieuChiId)?.OrderByDescending(e => e.GroupDanhGia.STT).ThenBy(e => e.GroupDanhGia.Name).Map<ABC_BoTieuChi_RoleDTO>();
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
        public IEnumerable<ABC_BoTieuChi_RoleDTO> GetByBoTieuIdAndGroupDanhGiaId(Guid boTieuChiId,Guid groupDanhGiaId)
        {
            List<ABC_BoTieuChi_RoleDTO> result = new List<ABC_BoTieuChi_RoleDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_GroupDanhGia groupTuDanhGia = session.Query<ABC_BoTieuChi_Role>().Where(e => e.BoTieuChi.Id == boTieuChiId && e.GroupDanhGia.Id == groupDanhGiaId)?.Select(e => e.GroupTuDanhGia).SingleOrDefault();
                    result = session.Query<ABC_BoTieuChi_Role>().Where(e => e.BoTieuChi.Id == boTieuChiId && e.GroupTuDanhGia.Id == groupTuDanhGia.Id)?.OrderByDescending(e => e.GroupDanhGia.STT).ThenBy(e => e.GroupDanhGia.Name).Map<ABC_BoTieuChi_RoleDTO>();
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
        public int PutSaveOrUpdate(List<ABC_BoTieuChi_RoleDTO> listObj)
        {
            int result = 1;
            try
            {
                if (listObj.Count > 0)
                {
                    SessionManager.DoWork(session =>
                    {
                        List<ABC_BoTieuChi_Role> listDelete = session.Query<ABC_BoTieuChi_Role>().Where(e => e.BoTieuChi.Id == listObj[0].BoTieuChiId).ToList();
                        foreach (ABC_BoTieuChi_Role obj in listDelete)
                        {
                            session.Delete(obj);
                        }
                    });

                    SessionManager.DoWork(session =>
                    {
                        foreach (ABC_BoTieuChi_RoleDTO obj in listObj)
                        {
                            ABC_BoTieuChi_Role objSave = new ABC_BoTieuChi_Role();
                            objSave.Id = Guid.NewGuid();
                            if (obj.GroupTuDanhGiaId != null)
                                objSave.GroupTuDanhGia = new ABC_GroupDanhGia() { Id = obj.GroupTuDanhGiaId.Value };
                            objSave.GroupDanhGia = new ABC_GroupDanhGia() { Id = obj.GroupDanhGiaId.Value };
                            objSave.BoTieuChi = new ABC_BoTieuChi() { Id = obj.BoTieuChiId.Value };
                            objSave.UserDanhGiaNgangHang = obj.UserDanhGiaNgangHang == true ? true : false;
                            session.SaveOrUpdate(objSave);
                        }
                        result = 1;
                    });
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
