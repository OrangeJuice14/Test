using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using NHibernate.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.Core.Security;
using System.Web;
using Microsoft.AspNet.Identity;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class DiemthuongdiemtruApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<PlanKPIDTO> GetListStudyYear()
        {
            List<PlanKPIDTO> result = new List<PlanKPIDTO>();
            SessionManager.DoWork(session =>
            {
                List<PlanKPI> temp = session.Query<PlanKPI>().Where(p => p.StudyYear != null).ToList();
                List<string> tempstring = new List<string>();
                foreach (PlanKPI pl in temp)
                {
                    tempstring.Add(pl.StudyYear);
                }
                tempstring = tempstring.Distinct().ToList();
                foreach (string year in tempstring)
                {
                    PlanKPIDTO pld = new PlanKPIDTO();
                    pld.StudyYear = year;
                    result.Add(pld);
                }

            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<DiemThuongDiemTruDTO> GetSearchdiemthuongdiemtru(string studyYear, string studyTerm)
        {
            var result = new List<DiemThuongDiemTruDTO>();
            SessionManager.DoWork(session =>
            {
                bool allYear = (studyYear == null || studyYear == "" || studyYear == "null") ? true : false;
                bool allTerm = (studyTerm == null || studyTerm == "" || studyTerm == "null") ? true : false;
                var temp = session.Query<DiemThuongDiemTru>().Where(d =>
                 (allYear || d.NamHoc == studyYear) && (allTerm || d.HocKy == studyTerm)).Select(r => new { r.MaHoatDong, r.NoiDung, r.MaNhomHoatDong }).Distinct().ToList();

                List<DiemThuongDiemTru> mylist = new List<DiemThuongDiemTru>();
                foreach (var od in temp)
                {
                    DiemThuongDiemTru oad = new DiemThuongDiemTru();
                    oad = session.Query<DiemThuongDiemTru>().Where(r => r.MaHoatDong == od.MaHoatDong && r.MaNhomHoatDong == od.MaNhomHoatDong && r.NoiDung == od.NoiDung).FirstOrDefault();
                    mylist.Add(oad);
                }

                foreach (DiemThuongDiemTru o in mylist)
                {
                    DiemThuongDiemTruDTO od = new DiemThuongDiemTruDTO();
                    od.Id = o.Id;
                    od.MaNhomHoatDong = o.MaNhomHoatDong;
                    od.MaHoatDong = o.MaHoatDong;
                    od.NoiDung = o.NoiDung;
                    od.NoiDungHoatDong = o.NoiDungHoatDong;
                    od.SoDiem = o.SoDiem;
                    od.MaCanBo = o.MaCanBo;
                    od.MaCanBo = o.TenCanBo;
                    od.NamHoc = o.NamHoc;
                    od.HocKy = o.HocKy;
                    od.Date = o.Date;
                    od.DonViCungCapName = o.DonViCungCapName;
                    od.UserNhap = o.UserNhap;
                    result.Add(od);
                }

            });
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<DiemThuongDiemTruDTO> GetSearchdiemthuongdiemtruUser(string studyYear, string studyTerm)
        {
            var result = new List<DiemThuongDiemTruDTO>();
            SessionManager.DoWork(session =>
            {
                bool allYear = (studyYear == null || studyYear == "" || studyYear == "null") ? true : false;
                bool allTerm = (studyTerm == null || studyTerm == "" || studyTerm == "null") ? true : false;
                List<DiemThuongDiemTru> temp = session.Query<DiemThuongDiemTru>().Where(d =>
                 (allYear || d.NamHoc == studyYear) && (allTerm || d.HocKy == studyTerm)).OrderBy(r => r.Date).ToList();
                foreach (DiemThuongDiemTru o in temp)
                {
                    DiemThuongDiemTruDTO od = new DiemThuongDiemTruDTO();
                    od.Id = o.Id;
                    od.MaNhomHoatDong = o.MaNhomHoatDong;
                    od.MaHoatDong = o.MaHoatDong;
                    od.NoiDung = o.NoiDung;
                    od.SoDiem = o.SoDiem;
                    od.MaCanBo = o.MaCanBo;
                    od.TenCanBo = o.TenCanBo;
                    od.NamHoc = o.NamHoc;
                    od.HocKy = o.HocKy;
                    od.Date = o.Date;
                    od.UserNhap = o.UserNhap;
                    od.NoiDungHoatDong = o.NoiDungHoatDong;
                    od.DonViCungCapName = o.DonViCungCapName;
                    result.Add(od);
                }

            });
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ProfessorCriterionDTO> GetListProfessorCriterion()
        {
            List<ProfessorCriterionDTO> result = new List<ProfessorCriterionDTO>();
            SessionManager.DoWork(session =>
            {
                List<ProfessorCriterion> temp = session.Query<ProfessorCriterion>().ToList();
                foreach (ProfessorCriterion p in temp)
                {
                    ProfessorCriterionDTO pd = new ProfessorCriterionDTO();
                    pd.Id = p.Id;
                    pd.Name = p.Name;
                    pd.ManageCode = p.ManageCode;
                    result.Add(pd);
                }
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<CriterionDictionaryDTO> GetListDictionaryByManageCode(string MaNhomHoatDong)
        {
            List<CriterionDictionaryDTO> result = new List<CriterionDictionaryDTO>();
            SessionManager.DoWork(session =>
            {
                List<CriterionDictionary> temp = session.Query<CriterionDictionary>().Where(p => p.ProfessorCriterion.ManageCode == MaNhomHoatDong && MaNhomHoatDong != null && MaNhomHoatDong != "").OrderBy(q => q.OrderNumber).ToList();
                foreach (CriterionDictionary p in temp)
                {
                    CriterionDictionaryDTO pd = new CriterionDictionaryDTO();
                    pd.Id = p.Id;
                    pd.Name = p.Name;
                    pd.ManageCode = p.ManageCode;
                    result.Add(pd);
                }
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public UserDataDTO GetUserNhap()
        {

            var result = new UserDataDTO();
            SessionManager.DoWork(session =>
            {
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                if (applicationUser.UserName == "admin" ||  applicationUser.UserName == "ad")
                {
                    applicationUser.HoVaTen = applicationUser.UserName;
                    applicationUser.DepartmentName = applicationUser.UserName;
                }
                else
                {
                    applicationUser.HoVaTen = applicationUser.HoVaTen;
                    applicationUser.DepartmentName = session.Query<Department>().Where(r => r.Id == new Guid(applicationUser.DepartmentId)).Select(r => r.Name).FirstOrDefault();
                }
                result = applicationUser.Map<UserDataDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public ProfessorCriterionDTO GetDVCC( string MaNhomHoatDong)
        {
            ProfessorCriterionDTO result = new ProfessorCriterionDTO();
            SessionManager.DoWork(session =>
            {
                ProfessorCriterion criterion = session.Query<ProfessorCriterion>().SingleOrDefault(a => a.ManageCode == MaNhomHoatDong);
                result = criterion.Map<ProfessorCriterionDTO>();
                result.TargetGroupDetail = new TargetGroupDetail() { Id = criterion.TargetGroupDetail.Id };
                result.CriterionType = new CriterionType() { Id = result.CriterionType != null ? result.CriterionType.Id : 0 };
                result.StudyYearIds = new List<Guid>();
                foreach (StudyYear y in criterion.StudyYears)
                {
                    result.StudyYearIds.Add(y.Id);
                }
                result.DonViTinh = new DonViTinh() { Id = result.DonViTinh != null ? result.DonViTinh.Id : 0 };
                result.DanhGiaChiTiet = new DanhGiaChiTiet() { Id = result.DanhGiaChiTiet != null ? result.DanhGiaChiTiet.Id : 0 };
                result.DepartmentIds = new List<Guid>();
                foreach (Department d in criterion.DonViCungCap)
                {
                    result.DepartmentIds.Add(d.Id);
                }
                result.ChucDanhIds = new List<Guid>();
                foreach (ChucDanh c in criterion.ChucDanh)
                {
                    result.ChucDanhIds.Add(c.Id);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<DiemThuongDiemTruDTO> GetListOtherActivityUserDetail(Guid id)
        {
            var result = new List<DiemThuongDiemTruDTO>();
            SessionManager.DoWork(session =>
            {
                List<DiemThuongDiemTru> temp = session.Query<DiemThuongDiemTru>().Where(d => d.Id == id).ToList();
                Guid deptId = Guid.Empty;
                string studyYear = temp.Select(r => r.NamHoc).FirstOrDefault();
                string activityManageCode = temp.Select(r => r.MaNhomHoatDong).FirstOrDefault();
                string manageCode = temp.Select(r => r.MaHoatDong).FirstOrDefault();
                string studyTerm = temp.Select(r => r.HocKy).FirstOrDefault();

                List<DiemThuongDiemTru> templist = session.Query<DiemThuongDiemTru>().Where(d => d.MaNhomHoatDong == activityManageCode.Trim() && d.MaHoatDong == manageCode.Trim()).ToList();

                foreach (DiemThuongDiemTru o in templist)
                {
                    DiemThuongDiemTruDTO od = new DiemThuongDiemTruDTO();
                    od.Id = o.Id;
                    od.MaNhomHoatDong = o.MaNhomHoatDong;
                    od.MaHoatDong = o.MaHoatDong;
                    od.NoiDungHoatDong = o.NoiDungHoatDong;
                    od.NoiDung = o.NoiDung;
                    od.SoDiem = o.SoDiem;
                    od.MaCanBo = o.MaCanBo;
                    od.TenCanBo = o.TenCanBo;
                    od.NamHoc = o.NamHoc;
                    od.HocKy = o.HocKy;
                    od.Date = o.Date;
                    od.DonViCungCapName = o.DonViCungCapName;
                    od.UserNhap = o.UserNhap;
                    od.DonViCungCapName = o.DonViCungCapName;
                    result.Add(od);
                }

            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<DiemThuongDiemTruMasterDTO> GetListUserOtherActivity()
        {
            List<DiemThuongDiemTruMasterDTO> result = new List<DiemThuongDiemTruMasterDTO>();
            SessionManager.DoWork(session =>
            {
                List<DiemThuongDiemTruMaster> temp = session.Query<DiemThuongDiemTruMaster>().ToList();
                foreach (DiemThuongDiemTruMaster p in temp)
                {
                    DiemThuongDiemTruMasterDTO pd = new DiemThuongDiemTruMasterDTO();
                    pd.Id = p.Id;
                    pd.MaCanBo = p.MaCanBo;
                    pd.TenCanBo = p.TenCanBo;
                    pd.SoDiem = p.SoDiem;
                    result.Add(pd);
                }
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public DiemThuongDiemTruDTO GetObj(Guid id)
        {
            var result = new DiemThuongDiemTruDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<DiemThuongDiemTru>().SingleOrDefault(a => a.Id == id).Map<DiemThuongDiemTruDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public DiemThuongDiemTruDTO Put(DiemThuongDiemTruDTO obj)
        {
            List<DiemThuongDiemTru> result = new List<DiemThuongDiemTru>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    //List<DiemThuongDiemTru> list = session.Query<DiemThuongDiemTru>().Where(r => r.MaNhomHoatDong == obj.MaNhomHoatDong && r.MaHoatDong == obj.MaHoatDong).ToList();
                    //foreach (DiemThuongDiemTru i in list)
                    //{
                    //    session.Delete(i);
                    //}

                    List<DiemThuongDiemTruMaster> ListOtherData = session.Query<DiemThuongDiemTruMaster>().ToList();
                    foreach (DiemThuongDiemTruMaster item in ListOtherData)
                    {
                        DiemThuongDiemTru oad = new DiemThuongDiemTru();
                        oad.Id = item.Id;
                        //if (obj.Date.Year == 1)
                        //    oad.Date = null;
                        oad.Date = obj.Date;
                        oad.MaHoatDong = obj.MaHoatDong;
                        oad.MaNhomHoatDong = obj.MaNhomHoatDong;
                        oad.NoiDung = obj.NoiDung;
                        oad.NamHoc = obj.NamHoc;
                        oad.HocKy = obj.HocKy;
                        oad.MaCanBo = item.MaCanBo;
                        oad.TenCanBo = item.TenCanBo;
                        oad.SoDiem = item.SoDiem;
                        oad.UserNhap = obj.UserNhap;
                        oad.Khoa = new Department() { Id = item.Khoa.Id };
                        oad.NoiDungHoatDong = item.NoiDung;
                        oad.DonViCungCapName = obj.DonViCungCapName;
                        result.Add(oad);
                    }
                    foreach (DiemThuongDiemTru p in result)
                    {
                        session.SaveOrUpdate(p);
                    }
                    foreach(DiemThuongDiemTruMaster it in ListOtherData)
                    {
                        session.Delete(it);
                    }
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DiemthuongdiemtruApi/Put", ex);
                throw ex;
            }
            return obj;
        }
        public IEnumerable<DiemThuongDiemTruDTO> GetList()
        {
            var result = new List<DiemThuongDiemTruDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<DiemThuongDiemTru>().OrderBy(d => d.MaCanBo).Map<DiemThuongDiemTruDTO>().ToList();

            });
            return result;
        }

        [Authorize]
        [Route("")]
        public DiemThuongDiemTru Delete(DiemThuongDiemTru obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }

    }
}
