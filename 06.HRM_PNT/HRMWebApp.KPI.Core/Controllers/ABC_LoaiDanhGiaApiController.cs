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
    public class ABC_LoaiDanhGiaApiController : ApiController
    {
        private static ABC_LoaiDanhGiaApiController instance;

        public static ABC_LoaiDanhGiaApiController Instance
        {
            get { if (instance == null) instance = new ABC_LoaiDanhGiaApiController(); return ABC_LoaiDanhGiaApiController.instance; }
            private set { ABC_LoaiDanhGiaApiController.instance = value; }
        }
        public ABC_LoaiDanhGia getLoaiDanhGia(int MaLoai)
        {
            ABC_LoaiDanhGia Obj = new ABC_LoaiDanhGia();
            SessionManager.DoWork(session =>
            {
                Obj = session.Query<ABC_LoaiDanhGia>().Where(e => e.MaLoai == MaLoai).FirstOrDefault();
            });
            return Obj;
        }
    }
}
