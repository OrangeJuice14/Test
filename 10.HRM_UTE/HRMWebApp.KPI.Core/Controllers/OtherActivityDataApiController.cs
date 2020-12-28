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
    public class OtherActivityDataApiController : ApiController
    {

        [Authorize]
        [Route("")]
        public IEnumerable<OtherActivityDataDTO> GetList()
        {
            var result = new List<OtherActivityDataDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<OtherActivityData>().OrderBy(d => d.StaffCode).Map<OtherActivityDataDTO>().ToList();

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
                // result = session.Query<OtherActivityData>().OrderBy(d => d.StaffCode).Map<OtherActivityDataDTO>().ToList();
                if (applicationUser.UserName == "admin" || applicationUser.UserName == "ad")
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

        //[Authorize]
        //[Route("")]
        //public IEnumerable<OtherActivityDataDTO> GetListByStaffId(Guid staffId)
        //{
        //    var result = new List<OtherActivityDataDTO>();
        //    SessionManager.DoWork(session =>
        //    {
        //        result = session.Query<OtherActivityData>().Where(d => d.StaffProfile.Id == staffId).OrderByDescending(d => d.StudyYear).Map<OtherActivityDataDTO>().ToList();

        //    });
        //    return result;
        //}
        [Authorize]
        [Route("")]
        public IEnumerable<OtherActivityDataDTO> GetSearchOtherActivity(Guid deptId, string studyYear, string activityManageCode, string manageCode, string studyTerm)
        {
            var result = new List<OtherActivityDataDTO>();
            SessionManager.DoWork(session =>
            {
                bool allDept = (deptId == Guid.Empty) ? true : false;
                bool allYear = (studyYear == null || studyYear == "" || studyYear == "null") ? true : false;
                bool allActivity = (activityManageCode == null || activityManageCode == "" || activityManageCode == "null") ? true : false;
                bool allManage = (manageCode == null || manageCode == "" || manageCode == "null") ? true : false;
                bool allTerm = (studyTerm == null || studyTerm == "" || studyTerm == "null") ? true : false;
                //List<OtherActivityData> temp = new List<OtherActivityData>();
                var temp = session.Query<OtherActivityData>().Where(d =>
                (allDept || d.Department.Id == deptId)
                && (allYear || d.StudyYear == studyYear)
                && (allActivity || d.ActivityManageCode == activityManageCode)
                && (allManage || d.ManageCode == manageCode)
                && (allTerm || d.StudyTerm == studyTerm)
                ).Select(r => new { r.ManageCode, r.Name, r.ActivityManageCode }).Distinct().ToList();

                List<OtherActivityData> mylist = new List<OtherActivityData>();
                foreach (var od in temp)
                {
                    OtherActivityData oad = new OtherActivityData();
                    oad = session.Query<OtherActivityData>().Where(r => r.ManageCode == od.ManageCode && r.Name == od.Name && r.ActivityManageCode == od.ActivityManageCode).FirstOrDefault();
                    mylist.Add(oad);
                }

                foreach (OtherActivityData o in mylist)
                {

                    OtherActivityDataDTO od = new OtherActivityDataDTO();
                    od.Id = o.Id;
                    od.ActivityManageCode = o.ActivityManageCode;
                    od.DepartmentName = o.Department != null ? o.Department.Name : "";
                    od.ManageCode = o.ManageCode;
                    od.Name = o.Name;
                    od.NumberOfTime = o.NumberOfTime;
                    od.StaffCode = o.StaffCode;
                    od.StaffName = o.StaffName;
                    od.StudyTerm = o.StudyTerm;
                    od.StudyYear = o.StudyYear;
                    od.Date = o.Date;
                    od.DonViCungCapName = o.DonViCungCapName;
                    od.UserNhap = o.UserNhap;
                    od.ManageName = o.ManageName;
                    od.IdCanBo = o.IdCanBo;
                    od.IdUserNhap = o.IdUserNhap;
                    result.Add(od);
                }

            });
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<OtherActivityDataDTO> GetSearchOtherActivityUser(Guid deptId, string studyYear, string activityManageCode, string manageCode, string studyTerm)
        {
            var result = new List<OtherActivityDataDTO>();
            SessionManager.DoWork(session =>
            {
                bool allDept = (deptId == Guid.Empty) ? true : false;
                bool allYear = (studyYear == null || studyYear == "" || studyYear == "null") ? true : false;
                bool allActivity = (activityManageCode == null || activityManageCode == "" || activityManageCode == "null") ? true : false;
                bool allManage = (manageCode == null || manageCode == "" || manageCode == "null") ? true : false;
                bool allTerm = (studyTerm == null || studyTerm == "" || studyTerm == "null") ? true : false;
                List<OtherActivityData> temp = session.Query<OtherActivityData>().Where(d =>
                (allDept || d.Department.Id == deptId)
                && (allYear || d.StudyYear == studyYear)
                && (allActivity || d.ActivityManageCode == activityManageCode)
                && (allManage || d.ManageCode == manageCode)
                && (allTerm || d.StudyTerm == studyTerm)
                ).OrderBy(r => r.Date).ToList();
                foreach (OtherActivityData o in temp)
                {
                    OtherActivityDataDTO od = new OtherActivityDataDTO();
                    od.Id = o.Id;
                    od.ActivityManageCode = o.ActivityManageCode;
                    od.DepartmentName = o.Department != null ? o.Department.Name : "";
                    od.ManageCode = o.ManageCode;
                    od.Name = o.Name;
                    od.NumberOfTime = o.NumberOfTime;
                    od.StaffCode = o.StaffCode;
                    od.StaffName = o.StaffName;
                    od.StudyTerm = o.StudyTerm;
                    od.StudyYear = o.StudyYear;
                    od.Date = o.Date;
                    od.DonViCungCapName = o.DonViCungCapName;
                    od.UserNhap = o.UserNhap;
                    od.ManageName = o.ManageName;
                    od.IdCanBo = o.IdCanBo;
                    od.IdUserNhap = o.IdUserNhap;
                    result.Add(od);
                }

            });
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<OtherActivityDataDTO> GetListOtherActivityUserDetail(Guid id)
        {
            var result = new List<OtherActivityDataDTO>();
            SessionManager.DoWork(session =>
            {
                List<OtherActivityData> temp = session.Query<OtherActivityData>().Where(d => d.Id == id).ToList();
                Guid deptId = Guid.Empty;
                string studyYear = temp.Select(r => r.StudyYear).FirstOrDefault();
                string activityManageCode = temp.Select(r => r.ActivityManageCode).FirstOrDefault();
                string manageCode = temp.Select(r => r.ManageCode).FirstOrDefault();
                string studyTerm = temp.Select(r => r.StudyTerm).FirstOrDefault();
                string name = temp.Select(r => r.Name).FirstOrDefault();
                List<OtherActivityData> templist = new List<OtherActivityData>();

                if (manageCode == null)
                {
                    templist = session.Query<OtherActivityData>().Where(d => d.ActivityManageCode == activityManageCode.Trim() && d.Name == name.Trim()).ToList();
                }
                else
                {
                    templist = session.Query<OtherActivityData>().Where(d => d.ActivityManageCode == activityManageCode.Trim() && d.ManageCode == manageCode.Trim() && d.Name == name.Trim()).ToList();
                }

                foreach (OtherActivityData o in templist)
                {
                    OtherActivityDataDTO od = new OtherActivityDataDTO();
                    od.Id = o.Id;
                    od.ActivityManageCode = o.ActivityManageCode;
                    od.DepartmentName = o.Department != null ? o.Department.Name : "";
                    od.ManageCode = o.ManageCode;
                    od.Name = o.Name;
                    od.NumberOfTime = o.NumberOfTime;
                    od.StaffCode = o.StaffCode;
                    od.StaffName = o.StaffName;
                    od.StudyTerm = o.StudyTerm;
                    od.StudyYear = o.StudyYear;
                    od.Date = o.Date;
                    od.DonViCungCapName = o.DonViCungCapName;
                    od.UserNhap = o.UserNhap;
                    od.IdCanBo = o.IdCanBo;
                    od.IdUserNhap = o.IdUserNhap;
                    result.Add(od);
                }

            });
            return result;
        }

        [Authorize]
        [Route("")]
        public ProfessorCriterionDTO GetDVCC(string MaNhomHoatDong)
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
        public OtherActivityDataDTO GetObj(Guid id)
        {
            var result = new OtherActivityDataDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<OtherActivityData>().SingleOrDefault(a => a.Id == id).Map<OtherActivityDataDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<OtherActivityDataDTO> GetObjMaster(Guid id)
        {
            List<OtherActivityDataDTO> result = new List<OtherActivityDataDTO>();
            SessionManager.DoWork(session =>
            {
                var mhd = session.Query<OtherActivityData>().Where(r => r.Id == id).Select(r => new { r.ManageCode, r.Name, r.ActivityManageCode }).FirstOrDefault();
                List<OtherActivityData> ot = session.Query<OtherActivityData>().Where(r => r.ActivityManageCode == mhd.ActivityManageCode && r.ManageCode == mhd.ManageCode && r.Name == mhd.Name).ToList();
                foreach (OtherActivityData item in ot)
                {
                    OtherActivityDataDTO newitem = new OtherActivityDataDTO();
                    newitem.Id = item.Id;
                    newitem.ManageCode = item.ManageCode;
                    newitem.ActivityManageCode = item.ActivityManageCode;
                    newitem.StaffCode = item.StaffCode;
                    newitem.NumberOfTime = item.NumberOfTime;
                    newitem.StudyTerm = item.StudyTerm;
                    newitem.StudyYear = item.StudyYear;
                    newitem.StaffName = item.StaffName;
                    newitem.UserNhap = item.UserNhap;
                    newitem.Name = item.Name;
                    newitem.IdUserNhap = item.IdUserNhap;
                    newitem.OrderNumber = item.OrderNumber;
                    newitem.Date = item.Date;
                    newitem.TotalParticipants = item.TotalParticipants;
                    newitem.DonViCungCapName = item.DonViCungCapName;
                    newitem.IdCanBo = item.IdCanBo;

                    result.Add(newitem);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<OtherActivityDataDetailDTO> GetListUserOtherActivity()
        {
            List<OtherActivityDataDetailDTO> result = new List<OtherActivityDataDetailDTO>();
            SessionManager.DoWork(session =>
            {
                List<OtherActivityDataDetail> temp = session.Query<OtherActivityDataDetail>().ToList();
                foreach (OtherActivityDataDetail p in temp)
                {
                    OtherActivityDataDetailDTO pd = new OtherActivityDataDetailDTO();
                    pd.Id = p.Id;
                    pd.StaffCode = p.StaffCode;
                    pd.StaffName = p.StaffName;
                    pd.NumberOfTime = p.NumberOfTime;
                    result.Add(pd);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<TargetGroupDetailDTO> GetListTargetGroupdetail()
        {
            List<TargetGroupDetailDTO> result = new List<TargetGroupDetailDTO>();
            SessionManager.DoWork(session =>
            {
                List<TargetGroupDetail> temp = session.Query<TargetGroupDetail>().ToList();
                foreach (TargetGroupDetail p in temp)
                {
                    TargetGroupDetailDTO pd = new TargetGroupDetailDTO();
                    pd.Id = p.Id;
                    pd.Name = p.Name;
                    result.Add(pd);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ProfessorCriterionDTO> GetListProfessorCriterion(Guid NhomMucTieuId)
        {
            List<ProfessorCriterionDTO> result = new List<ProfessorCriterionDTO>();
            SessionManager.DoWork(session =>
            {
                List<ProfessorCriterion> temp = session.Query<ProfessorCriterion>().Where(p => p.TargetGroupDetail.Id == NhomMucTieuId).ToList();
                foreach (ProfessorCriterion p in temp)
                {
                    ProfessorCriterionDTO pd = new ProfessorCriterionDTO();
                    pd.Id = p.Id;
                    pd.Name = p.Name;
                    pd.ManageCode = p.ManageCode;
                    pd.TargetGroupDetail = new TargetGroupDetail() { Id = p.TargetGroupDetail.Id,TargetGroupDetailType = new TargetGroupDetailType() { Id = p.TargetGroupDetail.TargetGroupDetailType.Id} };
                    result.Add(pd);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<TargetGroupDetailDTO> GetTargetGroupdetailType(Guid NhomMucTieuId)
        {
            List<TargetGroupDetailDTO> result = new List<TargetGroupDetailDTO>();
            SessionManager.DoWork(session =>
            {
                List<TargetGroupDetail> temp = session.Query<TargetGroupDetail>().Where(p => p.Id == NhomMucTieuId).ToList();
                foreach (TargetGroupDetail p in temp)
                {
                    TargetGroupDetailDTO pd = new TargetGroupDetailDTO();
                    pd.Id = p.Id;
                    pd.Name = p.Name;
                    pd.TargetGroupDetailTypeId = p.TargetGroupDetailType.Id;
                    result.Add(pd);
                }
            });
            return result;
        }

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
        public IEnumerable<CriterionDictionaryDTO> GetListDictionaryByManageCode(string manageCode)
        {
            List<CriterionDictionaryDTO> result = new List<CriterionDictionaryDTO>();
            SessionManager.DoWork(session =>
            {
                List<CriterionDictionary> temp = session.Query<CriterionDictionary>().Where(p => p.ProfessorCriterion.ManageCode == manageCode && manageCode != null && manageCode != "").OrderBy(q => q.OrderNumber).ToList();
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
        public OtherActivityDataDTO Put(OtherActivityDataDTO obj)
        {
            List<OtherActivityData> result = new List<OtherActivityData>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    ProfessorCriterion professor = session.Query<ProfessorCriterion>().Where(r => obj.ActivityManageCode == r.ManageCode).FirstOrDefault();
                    List<CriterionDictionary> Criterion = session.Query<CriterionDictionary>().Where(r => professor.Id == r.ProfessorCriterion.Id && r.StudyYears.Any(q => q.Name == obj.StudyYear)).ToList();
                    List<OtherActivityDataDetail> ListOtherData = session.Query<OtherActivityDataDetail>().ToList();
                    var tendanhmuc = "";
                    if (obj.ManageCode != null)
                    {
                        tendanhmuc = session.Query<CriterionDictionary>().Where(r => obj.ManageCode == r.ManageCode && professor.Id == r.ProfessorCriterion.Id).Select(r => r.Name).FirstOrDefault();
                    }
                    foreach (OtherActivityDataDetail item in ListOtherData)
                    {
                        OtherActivityData oad = new OtherActivityData();
                        oad.Id = item.Id;
                        oad.OrderNumber = obj.OrderNumber;
                        //if (obj.Date.Year == 1)
                        //    oad.Date = null;
                        oad.Date = obj.Date;
                        oad.TotalParticipants = obj.TotalParticipants;
                        oad.ActivityManageCode = obj.ActivityManageCode;
                        oad.ManageCode = obj.ManageCode;
                        oad.Name = obj.Name;
                        oad.StudyTerm = obj.StudyTerm;
                        oad.StudyYear = obj.StudyYear;
                        //Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();
                        oad.StaffCode = item.StaffCode;
                        oad.StaffName = item.StaffName;
                        oad.NumberOfTime = item.NumberOfTime;
                        oad.IdUserNhap = obj.IdUserNhap;
                        oad.UserNhap = obj.UserNhap;
                        oad.Department = new Department() { Id = item.Khoa.Id };
                        oad.DonViCungCapName = obj.DonViCungCapName;
                        foreach (CriterionDictionary cri in Criterion)
                        {
                            if ((item.NumberOfTime >= cri.DataRecord && item.NumberOfTime <= cri.DataMaxRecord))
                            {
                                oad.CriterionDictionaryId = new CriterionDictionary() { Id = cri.Id };
                            }
                        }
                        oad.ManageName = tendanhmuc;
                        oad.IdCanBo = item.IdCanBo;
                        oad.TargetGroupDetailId = obj.TargetGroupdetailType ;
                        oad.NhomMucTieu = new TargetGroupDetail() { Id = obj.NhomMucTieuId };
                        result.Add(oad);
                    }
                    foreach (OtherActivityData p in result)
                    {
                        session.SaveOrUpdate(p);
                    }
                    foreach (OtherActivityDataDetail it in ListOtherData)
                    {
                        session.Delete(it);
                    }
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("OtherActivityDataApi/Put", ex);
                throw ex;
            }
            return obj;
        }

        [Authorize]
        [Route("")]
        public OtherActivityDataDTO Puts(OtherActivityDataDTO obj)
        {
            List<OtherActivityData> result = new List<OtherActivityData>();
            try
            {

                SessionManager.DoWork(session =>
                {
                   
                    ProfessorCriterion professor = session.Query<ProfessorCriterion>().Where(r => obj.ActivityManageCode == r.ManageCode).FirstOrDefault();
                    List<CriterionDictionary> Criterion = session.Query<CriterionDictionary>().Where(r => professor.Id == r.ProfessorCriterion.Id).ToList();
                    //List<OtherActivityDataDetail> ListOtherData = session.Query<OtherActivityDataDetail>().ToList();
                    var tendanhmuc = session.Query<CriterionDictionary>().Where(r => obj.ManageCode == r.ManageCode && professor.Id == r.ProfessorCriterion.Id).Select(r => r.Name).FirstOrDefault();

                    List<OtherActivityData> listitem = session.Query<OtherActivityData>().Where(r => r.ManageCode == obj.ManageCode && r.ActivityManageCode == obj.ActivityManageCode && r.Name == obj.Name).ToList();

                    foreach (OtherActivityData oad in listitem)
                    {
                        oad.Id = oad.Id;
                        oad.OrderNumber = obj.OrderNumber;
                        oad.Date = obj.Date;
                        oad.TotalParticipants = obj.TotalParticipants;
                        oad.ActivityManageCode = obj.ActivityManageCode;
                        oad.ManageCode = obj.ManageCode;
                        oad.Name = obj.Name;
                        oad.StudyTerm = obj.StudyTerm;
                        oad.StudyYear = obj.StudyYear;
                        oad.IdUserNhap = obj.IdUserNhap;
                        oad.UserNhap = obj.UserNhap;
                        oad.DonViCungCapName = obj.DonViCungCapName;
                        oad.ManageName = tendanhmuc;
                        oad.NhomMucTieu = new TargetGroupDetail() { Id = obj.NhomMucTieuId };
                        result.Add(oad);
                    }
                    foreach (OtherActivityData p in result)
                    {
                        session.SaveOrUpdate(p);
                    }
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("OtherActivityDataApi/Puts", ex);
                throw ex;
            }
            return obj;
        }

        [Authorize]
        [Route("")]
        public OtherActivityDataDTO PutUser(OtherActivityDataDTO obj)
        {
            List<OtherActivityData> result = new List<OtherActivityData>();
            try
            {

                SessionManager.DoWork(session =>
                {
                    ProfessorCriterion professor = session.Query<ProfessorCriterion>().Where(r => obj.ActivityManageCode == r.ManageCode).FirstOrDefault();
                    List<CriterionDictionary> Criterion = session.Query<CriterionDictionary>().Where(r => professor.Id == r.ProfessorCriterion.Id).ToList();
                    var tendanhmuc = session.Query<CriterionDictionary>().Where(r => obj.ManageCode == r.ManageCode && professor.Id == r.ProfessorCriterion.Id).Select(r => r.Name).FirstOrDefault();
                    Staff staff = session.Query<Staff>().Where(s => s.StaffInfo.ManageCode == obj.StaffCode).SingleOrDefault();

                    OtherActivityData oad = new OtherActivityData();
                    oad.Id = Guid.NewGuid();
                    oad.OrderNumber = obj.OrderNumber;
                    oad.Date = obj.Date;
                    oad.ActivityManageCode = obj.ActivityManageCode;
                    oad.ManageCode = obj.ManageCode;
                    oad.Name = obj.Name;
                    oad.StudyTerm = obj.StudyTerm;
                    oad.StudyYear = obj.StudyYear;
                    oad.StaffCode = obj.StaffCode;
                    oad.StaffName = obj.StaffName;
                    oad.NumberOfTime = obj.NumberOfTime;
                    oad.IdUserNhap = obj.IdUserNhap;
                    oad.UserNhap = obj.UserNhap;
                    oad.Department = new Department() { Id = staff.Department.Id };
                    oad.DonViCungCapName = obj.DonViCungCapName;
                    foreach (CriterionDictionary cri in Criterion)
                    {
                        if ((obj.NumberOfTime >= cri.DataRecord && obj.NumberOfTime <= cri.DataMaxRecord))
                        {
                            oad.CriterionDictionaryId = new CriterionDictionary() { Id = cri.Id };
                        }
                    }
                    oad.ManageName = tendanhmuc;
                    oad.IdCanBo = staff.Id;
                    oad.NhomMucTieu = new TargetGroupDetail() { Id = obj.NhomMucTieuId };
                    result.Add(oad);
                    foreach (OtherActivityData p in result)
                    {
                        session.SaveOrUpdate(p);
                    }
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("OtherActivityDataApi/PutUser", ex);
                throw ex;
            }
            return obj;
        }
        [Authorize]
        [Route("")]
        public OtherActivityData Delete(OtherActivityData obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }

        [Authorize]
        [Route("")]
        public List<OtherActivityData> DeleteMulti(List<OtherActivityData> obj)
        {
            foreach (OtherActivityData i in obj)
            {
                SessionManager.DoWork(session => session.Delete(i));
            }
            return obj;
        }
    }
}
