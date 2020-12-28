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
    public class ABC_KetQuaXepLoaiApiController : ApiController
    {
        private static ABC_KetQuaXepLoaiApiController instance;

        public static ABC_KetQuaXepLoaiApiController Instance
        {
            get { if (instance == null) instance = new ABC_KetQuaXepLoaiApiController(); return ABC_KetQuaXepLoaiApiController.instance; }
            private set { ABC_KetQuaXepLoaiApiController.instance = value; }
        }

        public IEnumerable<ABC_KetQuaXepLoaiDTO> getListDTOByDanhGiaId(Guid DanhGiaId)
        {
            List<ABC_KetQuaXepLoaiDTO> result = new List<ABC_KetQuaXepLoaiDTO>();

            SessionManager.DoWork(session => 
            {
                result = session.Query<ABC_KetQuaXepLoai>().Where(e => e.DanhGia.Id == DanhGiaId).OrderBy(e => e.FromScore).Map<ABC_KetQuaXepLoaiDTO>().ToList();
            }); 

            return result;
        }
        [Authorize]
        [Route("")]
        public bool PutNew(ABC_KetQuaXepLoaiDTO obj)
        {
            ABC_KetQuaXepLoai objSave = new ABC_KetQuaXepLoai()
            {
                Id = Guid.NewGuid(),
                DanhGia = new ABC_DanhGia() { Id = obj.DanhGiaId },
                FromScore = obj.FromScore,
                ToScore = obj.ToScore,
                TenXepLoai = obj.TenXepLoai
            };
            SessionManager.DoWork(session => { session.Save(objSave); }); 
            return true;
        }
        [Authorize]
        [Route("")]
        public bool getDelete(Guid IdDelete)
        {
            SessionManager.DoWork(session => { session.Delete(new ABC_KetQuaXepLoai() { Id = IdDelete }); }); 
            return true;
        }
    }
}
