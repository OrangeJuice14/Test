using ERP_Core;
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
    public class dodulieudanhgiaApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<DoDuLieuDanhGiaDTO> GetListData()
        {
            var result = new List<DoDuLieuDanhGiaDTO>();
            SessionManager.DoWork(session =>
            {
                List<DoDuLieuDanhGia> list = session.Query<DoDuLieuDanhGia>().ToList();
                foreach (DoDuLieuDanhGia kpi in list)
                {
                    DoDuLieuDanhGiaDTO od = new DoDuLieuDanhGiaDTO();
                    od.Id = kpi.Id;
                    od.IdNhanVien = kpi.IdNhanVien != null ? kpi.IdNhanVien : kpi.IdNhanVien;
                    od.DepartmentName = kpi.DonVi != null ? kpi.DonVi.Name : "";
                    od.MaTieuChi = kpi.MaTieuChi;
                    od.GhiChu = kpi.GhiChu;
                    od.MaCanBo = kpi.MaCanBo;
                    od.TenCanBo = kpi.TenCanBo;
                    od.HocKy = kpi.HocKy;
                    od.NamHoc = kpi.NamHoc;
                    result.Add(od);
                }
                result = result.OrderBy(p => p.MaCanBo).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<DoDuLieuDanhGiaDTO> GetList()
        {
            var result = new List<DoDuLieuDanhGiaDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<DoDuLieuDanhGia>().OrderBy(d => d.MaCanBo).Map<DoDuLieuDanhGiaDTO>().ToList();

            });
            return result;
        }
        [Authorize]
        [Route("")]
        public DoDuLieuDanhGiaDTO GetObj(Guid id)
        {
            var result = new DoDuLieuDanhGiaDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<DoDuLieuDanhGia>().SingleOrDefault(a => a.Id == id).Map<DoDuLieuDanhGiaDTO>();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public DoDuLieuDanhGia Delete(DoDuLieuDanhGia obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }

    }
}
