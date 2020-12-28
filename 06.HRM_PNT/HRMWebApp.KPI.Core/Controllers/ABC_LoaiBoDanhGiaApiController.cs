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
    public class ABC_LoaiBoDanhGiaApiController : ApiController
    {
        private static ABC_LoaiBoDanhGiaApiController instance;

        public static ABC_LoaiBoDanhGiaApiController Instance
        {
            get { if (instance == null) instance = new ABC_LoaiBoDanhGiaApiController(); return ABC_LoaiBoDanhGiaApiController.instance; }
            private set { ABC_LoaiBoDanhGiaApiController.instance = value; }
        }

        public List<ABC_LoaiBoDanhGiaDTO> GetAllDTO()
        {
            List<ABC_LoaiBoDanhGiaDTO> result = new List<ABC_LoaiBoDanhGiaDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_LoaiBoDanhGia>().Map<ABC_LoaiBoDanhGiaDTO>().ToList();
            });
            return result;
        }
        public List<ABC_LoaiBoDanhGia> GetAll()
        {
            List<ABC_LoaiBoDanhGia> result = new List<ABC_LoaiBoDanhGia>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_LoaiBoDanhGia>().ToList();
            });
            return result;
        }

        
    }
}
