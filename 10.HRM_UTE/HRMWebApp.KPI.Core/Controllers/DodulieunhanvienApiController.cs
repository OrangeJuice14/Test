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
using HRMWebApp.KPI.Core.DTO.PlanMakingDTOs;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class DodulieunhanvienApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<DanhMucMTCLDTO> GetListManageCode(int capmuctieu)
        {
            List<DanhMucMTCLDTO> result = new List<DanhMucMTCLDTO>();
            SessionManager.DoWork(session =>
            {
                List<DanhMucMTCL> temp = session.Query<DanhMucMTCL>().Where(r=>r.CapDanhMuc == capmuctieu).ToList();
                foreach (DanhMucMTCL p in temp)
                {
                    DanhMucMTCLDTO pd = new DanhMucMTCLDTO();
                    pd.Id = p.Id;
                    pd.OidDanhMucCha = p.OidDanhMucCha;
                    pd.MaDanhMuc = p.MaDanhMuc;
                    pd.TenDanhMuc = p.TenDanhMuc;
                    pd.CapDanhMuc = p.CapDanhMuc;
                    result.Add(pd);
                }
            });
            return result;
        }

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
        public IEnumerable<TargetGroupDetailDTO> GetListTargetGroupDetail()
        {
            var result = new List<TargetGroupDetailDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<TargetGroupDetail>().Where(r=>r.TargetGroupDetailType.Id == 1).OrderBy(d => d.OrderNumber).Map<TargetGroupDetailDTO>().ToList();

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

        [Authorize]
        [Route("")]
        public IEnumerable<PlanKPIMakingDetailDTO> GetSearchOtherActivity(int capmuctieu, Guid TargetGroupDetailId)
        {
           
            var result = new List<PlanKPIMakingDetailDTO>();
            SessionManager.DoWork(session =>
            {
                bool allDept = (capmuctieu == 0) ? true : false;
                bool allActivity = (TargetGroupDetailId == Guid.Empty) ? true : false;

                var temp = session.Query<PlanKPIDetail>().Where(d =>
                (allDept || d.CapMucTieu == capmuctieu)
                && (allActivity || d.TargetGroupDetail.Id == TargetGroupDetailId)
                ).Select(r => new { r.DanhMucMTCL, r.TargetDetail }).Distinct().ToList();

                List<PlanKPIDetail> mylist = new List<PlanKPIDetail>();
                foreach (var od in temp)
                {
                    PlanKPIDetail oad = new PlanKPIDetail();
                    oad = session.Query<PlanKPIDetail>().Where(r => r.DanhMucMTCL.Id == od.DanhMucMTCL.Id && r.TargetDetail == od.TargetDetail).FirstOrDefault();
                    mylist.Add(oad);
                }

                foreach (PlanKPIDetail o in mylist)
                {
                    PlanKPIMakingDetailDTO od = new PlanKPIMakingDetailDTO();
                    od.Id = o.Id;
                    od.TargetDetail = o.TargetDetail;
                    od.StartTime = o.StartTime;
                    od.ManageId = o.ManageCode.Id;
                    od.Name = o.Name;
                    od.EndTime = o.EndTime;
                    result.Add(od);
                }
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<PlanKPIMakingDetailDTO> GetSearchOtherActivityUser(int capmuctieu, Guid TargetGroupDetailId)
        {
            var result = new List<PlanKPIMakingDetailDTO>();
            SessionManager.DoWork(session =>
            {
                bool allDept = (capmuctieu == 0) ? true : false;
                bool allActivity = (TargetGroupDetailId == Guid.Empty) ? true : false;
                List<PlanKPIDetail> temp = session.Query<PlanKPIDetail>().Where(d =>
                (allDept || d.CapMucTieu == capmuctieu)
                && (allActivity || d.TargetGroupDetail.Id == TargetGroupDetailId)
                ).ToList();

                foreach (PlanKPIDetail o in temp)
                {
                    PlanKPIMakingDetailDTO od = new PlanKPIMakingDetailDTO();
                    od.Id = o.Id;
                    od.TargetDetail = o.TargetDetail;
                    od.StartTime = o.StartTime;
                    od.ManageId = o.ManageCode.Id;
                    od.Name = o.Name;
                    od.EndTime = o.EndTime;
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
        public IEnumerable<MTCLDetailDTO> GetListUserMTCL()
        {
            List<MTCLDetailDTO> result = new List<MTCLDetailDTO>();
            SessionManager.DoWork(session =>
            {
                List<MTCLDetail> temp = session.Query<MTCLDetail>().ToList();
                foreach (MTCLDetail p in temp)
                {
                    MTCLDetailDTO pd = new MTCLDetailDTO();
                    pd.Id = p.Id;
                    pd.MaCanBo = p.MaCanBo;
                    pd.TenCanBo = p.TenCanBo;
                    pd.SoDiem = p.SoDiem;
                    pd.Khoa = new Department() { Id = p.Khoa.Id };
                    result.Add(pd);
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
                //.Where(p => (p.CriterionType.Id == 4 && p.TargetGroupDetail.Id == new Guid("E5C77D1A-5C95-464A-9846-23A987698C11")) || (p.TargetGroupDetail.Id == new Guid("00000000-0000-0000-0000-000000000002")) || (p.TargetGroupDetail.Id == new Guid("00000000-0000-0000-0000-000000000001")))
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
        public void Put(PlanKPIMakingDetailDTO obj)
        {
            List<PlanKPIDetail> result = new List<PlanKPIDetail>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    List<MTCLDetail> ListMTCL = session.Query<MTCLDetail>().ToList();
                    PlanKPI plankpi = session.Query<PlanKPI>().Where(r => r.StudyYear == obj.StudyYear && r.PlanType.Id == 1).FirstOrDefault();
                    DanhMucMTCL danhmuc = session.Query<DanhMucMTCL>().Where(r => r.Id == obj.DanhMucMTCLId).FirstOrDefault();

                    foreach (MTCLDetail item in ListMTCL)
                    {
                        Staff staff = session.Query<Staff>().SingleOrDefault(s => s.Id == item.IdCanBo.Id);
                        WebUser webuser = session.Query<WebUser>().Where(r => r.StaffInfo.Id == item.IdCanBo.Id).FirstOrDefault();
                        ApplicationUser applicationUser = AuthenticationHelper.GetUserById(webuser.Id, webuser.UserName);
                        AgentObject ang = session.Query<AgentObject>().Where(r => r.AgentObjectType.Id == Convert.ToInt32(applicationUser.AgentObjectTypeId)).FirstOrDefault();

                        PlanKPIDetail oad = new PlanKPIDetail();
                        oad.Id = item.Id;
                        oad.OrderNumber = obj.OrderNumber;
                        oad.CapMucTieu = obj.CapMucTieu;
                        oad.TargetGroupDetail = new TargetGroupDetail() { Id = obj.TargetGroupDetailId };
                        oad.TargetDetail = obj.TargetDetail;
                        oad.CreateTime = DateTime.Now;
                        oad.StartTime = obj.StartTime;
                        oad.EndTime = obj.EndTime;
                        oad.ManageCode = new ManageCode() {Id = danhmuc.OidDanhMucCha.Id };
                        oad.BasicResource = obj.BasicResource;
                        oad.IsMoved = false;
                       // oad.DanhMucMTCL = new DanhMucMTCL() { Id = obj.DanhMucMTCL};

                        PlanStaff pls = new PlanStaff();
                        pls.Id = Guid.NewGuid();
                        pls.Staff = staff;
                        pls.ModifiedTime = DateTime.Now;
                        pls.IsLocked = false;
                        pls.PlanKPI = new PlanKPI() { Id = plankpi.Id };
                      //  pls.Department = new Department() { Id = item.Khoa.Id };
                        pls.AgentObject = new AgentObject() {Id = ang.Id };
                        pls.AgentObjectType = new AgentObjectType() { Id = ang.AgentObjectType.Id };
                        session.Save(pls);

                        oad.PlanStaff = pls;
                     //   result.Add(oad);
                        session.SaveOrUpdate(oad);

                        Criterion cri = new Criterion();
                        cri.Id = Guid.NewGuid();
                        cri.Name = obj.TargetDetail;
                        cri.MaxRecord = item.SoDiem;
                        cri.TargetGroupDetail = new TargetGroupDetail() { Id = obj.TargetGroupDetailId };
                        cri.PlanKPI = new PlanKPI() { Id = plankpi.Id };
                        cri.CriterionType = new CriterionType() { Id = 1 };
                        cri.OrderNumber = obj.OrderNumber;
                        cri.FromPlanKPIDetail = new PlanKPIDetail() {Id = item.Id };
                        cri.Staff = new Staff() { Id = item.IdCanBo.Id };
                        session.Save(cri);

                    }
                 //   session.SaveOrUpdate(result);
                 foreach(MTCLDetail n in ListMTCL)
                    {
                        session.Delete(n);
                    }
                   
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DodulieunhanvienApi/Put", ex);
                throw ex;
            }
        }

        //[Authorize]
        //[Route("")]
        //public PlanDetailMakingDTO Puts(PlanDetailMakingDTO obj)
        //{
        //    List<PlanKPIDetail> result = new List<PlanKPIDetail>();
        //    try
        //    {

        //        SessionManager.DoWork(session =>
        //        {
        //            List<PlanKPIDetail> listitem = session.Query<PlanKPIDetail>().Where(r=>r.).ToList();

        //            foreach (PlanKPIDetail oad in listitem)
        //            {
        //                oad.Id = oad.Id;
        //                //oad.OrderNumber = obj.OrderNumber;
        //                //oad.Date = obj.Date;
        //                //oad.TotalParticipants = obj.TotalParticipants;
        //                //oad.ActivityManageCode = obj.ActivityManageCode;
        //                //oad.ManageCode = obj.ManageCode;
        //                //oad.Name = obj.Name;
        //                //oad.StudyTerm = obj.StudyTerm;
        //                //oad.StudyYear = obj.StudyYear;
        //                //oad.IdUserNhap = obj.IdUserNhap;
        //                //oad.UserNhap = obj.UserNhap;
        //                //oad.DonViCungCapName = obj.DonViCungCapName;
        //                //oad.ManageName = tendanhmuc;
        //                result.Add(oad);
        //            }
        //            session.SaveOrUpdate(result);
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Helper.ErrorLog("OtherActivityDataApi/Puts", ex);
        //        throw ex;
        //    }
        //    return obj;
        //}

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
