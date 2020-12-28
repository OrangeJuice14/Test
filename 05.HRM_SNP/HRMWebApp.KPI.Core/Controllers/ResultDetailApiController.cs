using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using HRMWebApp.KPI.Core.DTO.RatingKPIDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Globalization;
using Newtonsoft.Json;
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;
using HRMWebApp.KPI.Core.Helpers;
using System.Configuration;
using HRMWebApp.KPI.Core.DTO.PlanMakingDTOs;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ResultDetailApiController : ApiController
    {
        //[Authorize]
        //[Route("")]
        //public IEnumerable<ResultDTO> GetListByStaffId(Guid id)
        //{
        //    List<ResultDTO> result = new List<ResultDTO>();
        //    SessionManager.DoWork(session =>
        //    {
        //        if (id != Guid.Empty)
        //        {
        //            List<PlanStaff> planStaffs = session.Query<PlanStaff>().Where(p => p.Staff.Id == id).ToList();
        //            foreach (PlanStaff ps in planStaffs)
        //            {
        //                Result re = session.Query<Result>().SingleOrDefault(r => r.PlanStaff.Id == ps.Id);
        //                if (re != null)
        //                {
        //                    ResultDTO rd = new ResultDTO();
        //                    rd.Id = re.Id;
        //                    rd.PlanName = ps.PlanKPI.Name;
        //                    if (re.TotalRecord > 0)
        //                    {
        //                        rd.TotalRecord = re.TotalRecord.ToString();
        //                    }
        //                    else
        //                    {
        //                        rd.TotalRecord = "Chưa đánh giá";
        //                    }
        //                    if (re.TotalRecordSecond > 0)
        //                    {
        //                        rd.TotalRecordSecond = re.TotalRecordSecond.ToString();
        //                        rd.NumberOfEditing = re.NumberOfEditing.ToString();
        //                    }
        //                    else
        //                    {
        //                        rd.TotalRecordSecond = "Không có";
        //                        rd.NumberOfEditing = "Không có";
        //                    }

        //                    result.Add(rd);
        //                }

        //            }
        //        }

        //    });
        //    return result;
        //}

        //[Authorize]
        //[Route("")]
        //public Guid GetPlanIdByResultId(Guid id)
        //{
        //    Guid result = Guid.Empty;
        //    SessionManager.DoWork(session =>
        //    {
        //        Result re = session.Query<Result>().Where(r => r.Id == id).SingleOrDefault();
        //        if (re != null)
        //        {
        //            PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.Id == re.PlanStaff.Id).SingleOrDefault();
        //            if (planStaff != null)
        //            {
        //                result = planStaff.PlanKPI.Id;
        //            }
        //        }

        //    });
        //    return result;
        //}

        //[Authorize]
        //[Route("")]
        //public Guid GetStaffIdByResultId(Guid id)
        //{
        //    Guid result = Guid.Empty;
        //    SessionManager.DoWork(session =>
        //    {
        //        Result re = session.Query<Result>().Where(r => r.Id == id).SingleOrDefault();
        //        if (re != null)
        //        {
        //            PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.Id == re.PlanStaff.Id).SingleOrDefault();
        //            if (planStaff != null)
        //            {
        //                result = planStaff.Staff.Id;
        //            }
        //        }
        //    });
        //    return result;
        //}

        //[Authorize]
        //[Route("")]
        //public ResultDTO GetResult(Guid id)
        //{
        //    ResultDTO result = new ResultDTO();
        //    SessionManager.DoWork(session =>
        //    {
        //        if (id != Guid.Empty)
        //        {
        //            Result re = session.Query<Result>().SingleOrDefault(r => r.Id == id);
        //            if (re != null)
        //            {
        //                result.Id = re.Id;
        //                result.TotalRecordNumber = re.TotalRecord;
        //                result.TotalRecordSecondNumber = re.TotalRecordSecond;
        //            }
        //        }

        //    });
        //    return result;
        //}

        //[Authorize]
        //[Route("")]
        //public IEnumerable<ResultDetailDTO> GetList()
        //{
        //    List<ResultDetailDTO> result = new List<ResultDetailDTO>();
        //    SessionManager.DoWork(session =>
        //    {
        //        result = session.Query<ResultDetail>().ToList().Map<ResultDetailDTO>();
        //    });
        //    return result;
        //}


        //[Authorize]
        //[Route("")]
        //public IEnumerable<FileAttachmentDTO> GetFieldAttachmentByResultDetail(Guid id)
        //{
        //    List<FileAttachmentDTO> result = new List<FileAttachmentDTO>();
        //    SessionManager.DoWork(session =>
        //    {
        //        result = session.Query<ResultDetail>().SingleOrDefault(r => r.Id == id).FileAttachments.Map<FileAttachmentDTO>();
        //    });
        //    return result;
        //}


        //[Authorize]
        //[Route("")]
        //public RatingKPIResultDTO GetRatingResultDetail(Guid planId, Guid agentObjectId, Guid planStaffId, Guid supervisorId,Guid departmentId, byte isAdminRating)
        //{
        //    //planStaffId: mã nhân viên được xem
        //    RatingKPIResultDTO result = new RatingKPIResultDTO();
        //    //try
        //    //{
        //        Result ratingResult = new Result();

        //    if (AuthenticationHelper.IsAllowPlanMaking(new Guid(HttpContext.Current.User.Identity.GetUserId()), planStaffId != Guid.Empty ? planStaffId : new Guid(HttpContext.Current.User.Identity.GetUserId())))
        //    {
        //        SessionManager.DoWork(session =>
        //         {
        //             ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
        //             Guid deptId = applicationUser.DepartmentId != null ? new Guid(applicationUser.DepartmentId) : Guid.Empty;
        //             Staff staff = new Staff();
        //             var isSupervisor = false;
        //             if (supervisorId == Guid.Empty && isAdminRating == 0 && planStaffId == Guid.Empty)
        //             {
        //                 staff = ControllerHelpers.GetCurrentStaff(session);
        //             }
        //             else if ((supervisorId != Guid.Empty || isAdminRating == 1) && planStaffId != Guid.Empty)
        //             {
        //                 staff = session.Query<Staff>().SingleOrDefault(s => s.Id == planStaffId);
        //                 result.IsSupervisor = true;
        //                 if (isAdminRating == 1)
        //                 {
        //                     result.IsAdminRating = true;
        //                 }
        //                 else
        //                 {
        //                     result.IsAdminRating = false;
        //                 }
        //             }
        //             else if (isAdminRating == 1 && departmentId != Guid.Empty)
        //             {
        //                 Department temp = session.Query<Department>().Where(d => d.Id == departmentId).SingleOrDefault();
        //                 staff.Department = new Department() { Id = temp.Id };
        //                 switch (temp.DepartmentType)
        //                 {
        //                     case 1:
        //                         {
        //                             staff.StaffInfo = new StaffInfo() { Position = new Position() { AgentObjectType = new AgentObjectType() { Id = 3 } } };
        //                         }
        //                         break;
        //                     case 4:
        //                         {
        //                             staff.StaffInfo = new StaffInfo() { Position = new Position() { AgentObjectType = new AgentObjectType() { Id = 5 } } };
        //                         }
        //                         break;
        //                 }
        //             }
        //             Guid staffId = staff.Id;
        //             PlanKPI plan = session.Query<PlanKPI>().Where(p => p.Id == planId).OrderByDescending(p => p.CreateTime).FirstOrDefault();
        //             result.StartRatingTime = plan.RatingStartTime;
        //             result.EndRatingTime = plan.RatingEndTime;

        //             int AgentObjectTypeId = -1;
        //             if (agentObjectId == Guid.Empty)
        //             {
        //                 if (staff.StaffInfo.Position == null)
        //                 {
        //                     if (staff.StaffInfo.StaffType.ManageCode == "3")
        //                         AgentObjectTypeId = 2; //Nhân viên
        //                         else
        //                         AgentObjectTypeId = 1; //Giảng viên
        //                     }
        //                 else
        //                 {
        //                     AgentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
        //                 }
        //             }
        //             else
        //             {
        //                 AgentObject ab = session.Query<AgentObject>().SingleOrDefault(a => a.Id == agentObjectId);
        //                 AgentObjectTypeId = ab.AgentObjectType.Id;
        //                 result.AgentObjectName = ab.Name;
        //             }


        //                 //Chọn agentobjecttype cho đối tượng nếu như là giảng dạy
        //                 AgentObject ao = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).SingleOrDefault();
        //             if (ao != null && ao.AgentObjectType.Id == 2)
        //             {
        //                 StaffApiController controller = new StaffApiController();
        //                 result.StaffLeader = new StaffDTO();
        //                 if (staff.Id != Guid.Empty)
        //                     result.StaffLeader = controller.GetDepartmentLeader(staff.Department.Id);
        //                 else
        //                         //user kpi không có staff
        //                         result.StaffLeader = controller.GetDepartmentLeader(deptId);

        //             }

        //             if (staff.StaffInfo.AgentObjects.Count > 1)
        //             {
        //                 AgentObject agentObj = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).SingleOrDefault();
        //                 AgentObjectTypeId = ao.AgentObjectType.Id;
        //             }

        //             PlanStaff planStaff = new PlanStaff();
        //             if (AgentObjectTypeId == 1 || AgentObjectTypeId == 2)
        //             {
        //                 planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
        //                 if (planStaff != null)
        //                     ratingResult = session.Query<Result>().SingleOrDefault(r => r.PlanStaff.Id == planStaff.Id && r.StaffRating.Id == staff.Id);
        //             }
        //             else if (AgentObjectTypeId == 3 || AgentObjectTypeId == 5)
        //             {
        //                 if (staff.Id != Guid.Empty)
        //                     planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.Department.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
        //                 else
        //                         //user KPI ko co staff
        //                         planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.Department.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
        //                 if (planStaff != null)
        //                     ratingResult = session.Query<Result>().SingleOrDefault(r => r.PlanStaff.Id == planStaff.Id);
        //             }
        //             else if (AgentObjectTypeId == 6)
        //             {
        //                 planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.StaffInfo.Subject.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
        //                 if (planStaff != null)
        //                     ratingResult = session.Query<Result>().SingleOrDefault(r => r.PlanStaff.Id == planStaff.Id);
        //             }
        //             else if (AgentObjectTypeId == 7 || AgentObjectTypeId == 8)
        //             {
        //                 planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.Department.Id && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
        //                 if (planStaff != null)
        //                     ratingResult = session.Query<Result>().SingleOrDefault(r => r.PlanStaff.Id == planStaff.Id);
        //             }
        //             else if (AgentObjectTypeId == 9)
        //             {
        //                 planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.StaffInfo.Subject.Id && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
        //                 if (planStaff != null)
        //                     ratingResult = session.Query<Result>().SingleOrDefault(r => r.PlanStaff.Id == planStaff.Id);
        //             }
        //             else if (AgentObjectTypeId == 11 || AgentObjectTypeId == 10)
        //             {
        //                 planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.Department.Id && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
        //                 if (planStaff != null)
        //                     ratingResult = session.Query<Result>().SingleOrDefault(r => r.PlanStaff.Id == planStaff.Id);
        //             }

        //             if (planStaff != null && ratingResult == null)
        //             {
        //                 ratingResult = new Result();
        //                 ratingResult.Id = Guid.NewGuid();
        //                 ratingResult.PlanStaff = planStaff;
        //                 ratingResult.StaffRating = staff.Id != Guid.Empty ? staff : null;
        //                 ratingResult.Time = DateTime.Now;
        //                 ratingResult.IsLocked = false;
        //                 ratingResult.IsUnlocked = false;
        //                 ratingResult.IsCommitted = false;
        //                 ratingResult.IsUnlockedForRating = false;
        //                 ratingResult.TotalRecord = 0;
        //                 ratingResult.NumberOfEditing = 0;

        //                 session.Save(ratingResult);
        //             }

        //             if (planStaff != null)
        //             {
        //                 result.IsPlanLocked = planStaff.IsLocked;
        //                 if (result.IsSupervisor)
        //                 {
        //                     result.IsRated = ratingResult != null ? true : false;

        //                 }
        //             }
        //         });


        //        SessionManager.DoWork(session =>
        //        {

        //            ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
        //            Guid deptId = applicationUser.DepartmentId != null ? new Guid(applicationUser.DepartmentId) : Guid.Empty;
        //            Staff staff = new Staff();
        //            if (supervisorId == Guid.Empty && isAdminRating == 0 && planStaffId == Guid.Empty)
        //            {
        //                staff = ControllerHelpers.GetCurrentStaff(session);
        //            }
        //            else if ((supervisorId != Guid.Empty || isAdminRating == 1) && planStaffId != Guid.Empty)
        //            {
        //                staff = session.Query<Staff>().SingleOrDefault(s => s.Id == planStaffId);
        //            }
        //            Guid staffId = staff.Id;
        //            if (supervisorId == Guid.Empty && isAdminRating == 0)
        //            {
        //                result.IsSupervisor = false;
        //            }
        //            else if ((supervisorId != Guid.Empty || isAdminRating == 1) && departmentId == Guid.Empty)
        //            {
        //                staffId = planStaffId;
        //                result.IsSupervisor = false;
        //                if (supervisorId != Guid.Empty)
        //                {
        //                    result.IsSupervisor = true;
        //                    Staff st = session.Query<Staff>().Where(s => s.Id == supervisorId).SingleOrDefault();
        //                    result.Supervisor = new StaffDTO();
        //                    result.Supervisor.Id = st != null ? st.Id : Guid.Empty;
        //                    result.Supervisor.Name = st != null ? st.StaffProfile.Name : "";
        //                }
        //            }
        //            else if (isAdminRating == 1 && departmentId != Guid.Empty)
        //            {
        //                result.IsSupervisor = true;
        //                Department temp = session.Query<Department>().Where(d => d.Id == departmentId).SingleOrDefault();
        //                staff.Department = new Department() { Id = temp.Id };
        //                switch (temp.DepartmentType)
        //                {
        //                    case 1:
        //                        {
        //                            staff.StaffInfo = new StaffInfo() { Position = new Position() { AgentObjectType = new AgentObjectType() { Id = 3 } } };
        //                        }
        //                        break;
        //                    case 4:
        //                        {
        //                            staff.StaffInfo = new StaffInfo() { Position = new Position() { AgentObjectType = new AgentObjectType() { Id = 5 } } };
        //                        }
        //                        break;
        //                }
        //                result.IsAdminRating = true;
        //            }

        //            //staff = session.Query<Staff>().SingleOrDefault(s => s.Id == staffId);
        //            PlanKPI plan = session.Query<PlanKPI>().Where(p => p.Id == planId).OrderByDescending(p => p.CreateTime).FirstOrDefault();
        //            if (plan != null)
        //            {
        //                result.PlanTypeId = plan.PlanType.Id;
        //                int AgentObjectTypeId = -1;
        //                if (agentObjectId == Guid.Empty)
        //                {
        //                    if (staff != null && staff.StaffInfo.Position == null)
        //                    {
        //                        if (staff.StaffInfo.StaffType.ManageCode == "3")
        //                            AgentObjectTypeId = 2; //Nhân viên
        //                        else
        //                            AgentObjectTypeId = 1; //Giảng viên
        //                    }
        //                    else
        //                    {
        //                        //Không có nhân viên cụ thể
        //                        AgentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
        //                    }
        //                }
        //                else
        //                {
        //                    AgentObject ab = session.Query<AgentObject>().SingleOrDefault(a => a.Id == agentObjectId);
        //                    AgentObjectTypeId = ab.AgentObjectType.Id;
        //                }
        //                //Chọn agentobjecttype cho đối tượng nếu như là giảng dạy
        //                AgentObject ao = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).SingleOrDefault();
        //                if (ao != null && ao.AgentObjectType.Id == 2)
        //                {
        //                    StaffApiController controller = new StaffApiController();
        //                    result.StaffLeader = new StaffDTO();
        //                    result.StaffLeader = controller.GetDepartmentLeader(staff.Department.Id);
        //                }

        //                if (staff.StaffInfo.AgentObjects.Count > 1)
        //                {
        //                    AgentObject agentObj = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).SingleOrDefault();
        //                    AgentObjectTypeId = ao.AgentObjectType.Id;
        //                }

        //                PlanStaff planStaff = new PlanStaff();
        //                if (AgentObjectTypeId == 1 || AgentObjectTypeId == 2)
        //                    planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
        //                else if (AgentObjectTypeId == 3 || AgentObjectTypeId == 5)
        //                    if (isAdminRating == 0)
        //                        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == deptId && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
        //                    else
        //                        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.Department.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
        //                else if (AgentObjectTypeId == 6)
        //                    planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.StaffInfo.Subject.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
        //                else if (AgentObjectTypeId == 7 || AgentObjectTypeId == 8 || AgentObjectTypeId == 11 || AgentObjectTypeId == 10)
        //                    planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.Department.Id && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
        //                if (planStaff != null)
        //                {
        //                    ratingResult = session.Query<Result>().SingleOrDefault(r => r.PlanStaff.Id == planStaff.Id);


        //                    if (staff.Id != Guid.Empty) //User thường
        //                    {
        //                        result.StaffDTO = new StaffDTO();
        //                        result.StaffDTO.Id = staff.Id;
        //                        result.StaffDTO.Name = staff.StaffProfile.Name;
        //                        result.StaffDTO.Department = staff.Department.Map<DepartmentDTO>();
        //                        result.StaffDTO.DepartmentId = staff.Department.Id;
        //                        result.StaffDTO.Subject = staff.StaffInfo.Subject != null ? staff.StaffInfo.Subject.Map<DepartmentDTO>() : null;
        //                        result.StaffDTO.Position = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.Map<PositionDTO>() : null;
        //                        result.StaffDTO.UserName = staff.StaffInfo.WebUser.UserName;
        //                    }
        //                    else //User KPI
        //                    {
        //                        result.StaffDTO = new StaffDTO();
        //                        //result.StaffDTO.Department = staff.Department.Map<DepartmentDTO>();
        //                        result.StaffDTO.DepartmentId = staff.Department.Id;
        //                    }

        //                    result.StaffId = staff.Id;


        //                    List<TargetGroupDetail> targets = session.Query<TargetGroupDetail>().Where(t => t.AgentObjects.Any(ag => ag.Id == agentObjectId)).OrderBy(t => t.OrderNumber).ToList();
        //                    if (AgentObjectTypeId == 5 || AgentObjectTypeId == 6)
        //                        targets = session.Query<TargetGroupDetail>().Where(t => t.AgentObjects.Any(ag => ag.AgentObjectType.Id == 1)).OrderBy(t => t.OrderNumber).ToList();
        //                    result.TargetGroupRatingDTOs = new List<TargetGroupRatingDTO>();
        //                    List<ResultDetail> resultDetails = session.Query<ResultDetail>().Where(r => r.Result.Id == ratingResult.Id).ToList();
        //                    #region Original Result

        //                    foreach (TargetGroupDetail t in targets)
        //                    {
        //                        TargetGroupRatingDTO tg = new TargetGroupRatingDTO();
        //                        tg.TargetGroupDetailTypeId = t.TargetGroupDetailType.Id;
        //                        result.PlanStaffId = planStaff.Id;
        //                        result.StaffDTO.AgentObjectTypeId = AgentObjectTypeId;

        //                        switch (AgentObjectTypeId)
        //                        {
        //                            case (int)AgentObjectTypes.GiangVien:
        //                                {
        //                                    #region giảng viên
        //                                    List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();
        //                                    planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
        //                                    if (t.TargetGroupDetailType.Id != 4)
        //                                    {
        //                                        foreach (PlanKPIDetail pld in planDetails)
        //                                        {
        //                                            ResultDetail rdetail = session.Query<ResultDetail>().SingleOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
        //                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
        //                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
        //                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
        //                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
        //                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
        //                                            List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id).Map<CriterionDictionaryDTO>().ToList();
        //                                            rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
        //                                            rd.PlanKPIDetailId = pld.Id;
        //                                            rd.PlanKPIDetailName = pld.Name;
        //                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
        //                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };

        //                                            switch (pld.FromProfessorCriterion.CriterionType.Id)
        //                                            {
        //                                                case 3:
        //                                                    {
        //                                                        if (pld.CurrentKPI != null)
        //                                                        {
        //                                                            try
        //                                                            {
        //                                                                Guid crdId = new Guid(pld.CurrentKPI);
        //                                                                CriterionDictionary crd = session.Query<CriterionDictionary>().Where(c => c.Id == crdId).SingleOrDefault();
        //                                                                rd.RegisterTarget = crd.Name;
        //                                                            }
        //                                                            catch (Exception e)
        //                                                            {

        //                                                            }
        //                                                        }
        //                                                    }
        //                                                    break;
        //                                                default:
        //                                                    {
        //                                                        rd.RegisterTarget = pld.CurrentKPI;
        //                                                    }
        //                                                    break;
        //                                            }
        //                                            if (pld.FromProfessorCriterion.Id == new Guid(ConfigurationManager.AppSettings["ChatLuongGiangDay"]))
        //                                            {
        //                                                ESurveyData edata = session.Query<ESurveyData>().Where(e => e.StaffId == staff.Id && e.StudyTerm == plan.StudyTerm && e.StudyYear == plan.StudyYear).SingleOrDefault();
        //                                                if (edata != null)
        //                                                {
        //                                                    double point = edata.RankingPoint;
        //                                                    CriterionDictionary crid = session.Query<CriterionDictionary>().Where(c => c.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id && c.DataRecord < point && c.DataMaxRecord >= point).SingleOrDefault();
        //                                                    if (crid != null)
        //                                                    {
        //                                                        rd.SupervisorRecord = crid.Record;
        //                                                        rd.Record = crid.Record;
        //                                                        rd.CurrentResult = crid.Name;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        rd.SupervisorRecord = 0;
        //                                                    }

        //                                                }
        //                                            }
        //                                            string GiangDaySoTietChuan = ConfigurationManager.AppSettings["GiangDaySoTietChuan"];
        //                                            if (pld.FromProfessorCriterion.Id.ToString().ToUpper() == GiangDaySoTietChuan)
        //                                            {
        //                                                PMSData pdata = session.Query<PMSData>().Where(e => e.StaffId == staff.Id && e.StudyTerm == plan.StudyTerm && e.StudyYear == plan.StudyYear).SingleOrDefault();
        //                                                if (pdata != null)
        //                                                {
        //                                                    double percent = pdata.PercentShortage;
        //                                                    rd.CurrentResult = pdata.NumberOfLesson.ToString();
        //                                                    CriterionDictionary crid = session.Query<CriterionDictionary>().Where(c => c.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id && c.DataRecord < percent && c.DataMaxRecord >= percent).SingleOrDefault();
        //                                                    if (crid != null)
        //                                                    {
        //                                                        rd.SupervisorRecord = crid.Record;
        //                                                    }
        //                                                }
        //                                                else
        //                                                {
        //                                                    rd.SupervisorRecord = 0;
        //                                                }
        //                                            }
        //                                            if (t.TargetGroupDetailType.Id == 5)
        //                                            {
        //                                                rd.ScienceResearches = new List<ScienceResearchDTO>();
        //                                                foreach (ScienceResearch pa in pld.ScienceResearches)
        //                                                {
        //                                                    ScienceResearchDTO pad = new ScienceResearchDTO();
        //                                                    pad.Id = pa.Id;
        //                                                    pad.Name = pa.CriterionDictionary.Name;
        //                                                    pad.NumberOfResearch = pa.NumberOfResearch;
        //                                                    rd.ScienceResearches.Add(pad);
        //                                                }
        //                                                var scienceResearch = session.Query<ScienceResearchData>().FirstOrDefault(s => s.StaffCode == staff.StaffInfo.WebUser.UserName && s.StudyTerm == plan.StudyTerm && s.StudyYear == plan.StudyYear && s.ManageCode == pld.FromProfessorCriterion.ManageCode);
        //                                                rd.SupervisorRecord = scienceResearch != null ? scienceResearch.Record : 0;
        //                                                rd.CurrentKPI = scienceResearch != null ? scienceResearch.Name : null;
        //                                            }

        //                                            if (rdetail != null)
        //                                            {
        //                                                //rd = rdetail.Map<ResultDetailRatingDTO>();
        //                                                rd.PreviousResult = rdetail.PreviousResult;
        //                                                rd.CurrentResult = rdetail.CurrentResult;
        //                                                rd.RegisterTarget = rdetail.RegisterTarget;
        //                                                rd.Record = rdetail.Record;
        //                                                rd.SupervisorRecord = rdetail.SupervisorRecord;
        //                                                rd.Id = rdetail.Id;
        //                                                rd.Note = rdetail.Note;
        //                                                rd.SupervisorNote = rdetail.SupervisorNote;
        //                                                rd.FileAttachments = new List<FileAttachmentDTO>();
        //                                                foreach (FileAttachment fad in rdetail.FileAttachments)
        //                                                {
        //                                                    FileAttachmentDTO fa = new FileAttachmentDTO();
        //                                                    fa.Id = fad.Id;
        //                                                    fa.CreationTime = fad.CreationTime;
        //                                                    fa.Extension = fad.Extension;
        //                                                    fa.Name = fad.Name;
        //                                                    fa.Path = fad.Path;
        //                                                    fa.ResultDetailId = fad.ResultDetail.Id;
        //                                                    rd.FileAttachments.Add(fa);
        //                                                }
        //                                                rd.FileAttachmentCount = rd.FileAttachments.Count();
        //                                                rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
        //                                            }
        //                                            rd.Density = 1;
        //                                            tg.ResultDetailDTOs.Add(rd);
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        foreach (PlanKPIDetail pld in planDetails)
        //                                        {
        //                                            ResultDetail rdetail = session.Query<ResultDetail>().SingleOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
        //                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
        //                                            rd.ProfessorOtherActivities = new List<ProfessorOtherActivityDTO>();
        //                                            rd.ActivityHour = 0;
        //                                            foreach (ProfessorOtherActivity pa in pld.ProfessorOtherActivities)
        //                                            {
        //                                                ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
        //                                                pad.Id = pa.Id;
        //                                                pad.Name = pa.CriterionDictionary.Name;
        //                                                pad.NumberOfTime = pa.NumberOfTime;
        //                                                rd.ProfessorOtherActivities.Add(pad);
        //                                            }
        //                                            rd.ProfessorOtherActivitiesResult = new List<ProfessorOtherActivityDTO>();
        //                                            List<OtherActivityData> otherActivities = session.Query<OtherActivityData>().Where(s =>
        //                                            s.StaffCode == staff.StaffInfo.WebUser.UserName && s.StudyTerm == plan.StudyTerm
        //                                            && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
        //                                            foreach (OtherActivityData pa in otherActivities)
        //                                            {
        //                                                CriterionDictionary tempcri = session.Query<CriterionDictionary>().Where(c => c.ManageCode == pa.ManageCode).SingleOrDefault();
        //                                                ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
        //                                                pad.Name = pa.Name;
        //                                                pad.NumberOfHour = tempcri != null ? tempcri.NumberOfHour : 0;
        //                                                pad.NumberOfTimeDouble = pa.NumberOfTime;
        //                                                rd.ActivityHour += pad.NumberOfHour * pa.NumberOfTime;
        //                                                rd.ProfessorOtherActivitiesResult.Add(pad);
        //                                            }
        //                                            int TotalHourDefault = session.Query<KPIConfiguration>().FirstOrDefault().TotalHourDefault;
        //                                            rd.SupervisorRecord = Math.Round((rd.ActivityHour / TotalHourDefault) * 100, 1);
        //                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
        //                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
        //                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
        //                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
        //                                            rd.RegisterTarget = pld.CurrentKPI;
        //                                            rd.CriterionDictionaries = pld.FromProfessorCriterion.CriterionDictionaries.Map<CriterionDictionaryDTO>();
        //                                            rd.PlanKPIDetailId = pld.Id;
        //                                            rd.PlanKPIDetailName = pld.Name;
        //                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
        //                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };
        //                                            if (rdetail != null)
        //                                            {
        //                                                rd.PreviousResult = rdetail.PreviousResult;
        //                                                rd.CurrentResult = rdetail.CurrentResult;
        //                                                rd.RegisterTarget = rdetail.RegisterTarget;
        //                                                if (rdetail.RecordSecond != 0 && ratingResult.IsUnlocked == true)
        //                                                {
        //                                                    rd.Record = rdetail.RecordSecond;
        //                                                    rd.RecordOld = rdetail.Record;
        //                                                }
        //                                                else
        //                                                {
        //                                                    rd.Record = rdetail.Record;
        //                                                    rd.RecordOld = rdetail.Record;
        //                                                }
        //                                                rd.SupervisorRecord = rd.SupervisorRecord > 0 ? rd.SupervisorRecord : rdetail.SupervisorRecord;
        //                                                rd.Id = rdetail.Id;
        //                                            }

        //                                            tg.ResultDetailDTOs.Add(rd);
        //                                        }
        //                                        double totalDensity = tg.ResultDetailDTOs.Sum(tr => tr.MaxRecord);
        //                                        tg.ResultDetailDTOs.ForEach(r =>
        //                                        {
        //                                            r.Density = Math.Round(r.MaxRecord / totalDensity, 2);
        //                                        });
        //                                    }
        //                                    #endregion
        //                                }
        //                                break;
        //                            case (int)AgentObjectTypes.NhanVien:
        //                                {
        //                                    #region nhân viên

        //                                    List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();
        //                                    switch (t.TargetGroupDetailType.Id)
        //                                    {
        //                                        case 1:
        //                                            {
        //                                                planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == t.Id && !p.IsDisable && !p.IsDelete).ToList();
        //                                            }
        //                                            break;
        //                                        case 2:
        //                                            {
        //                                                planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id && !p.IsDisable && !p.IsDelete).ToList();

        //                                            }
        //                                            break;
        //                                        case 3:
        //                                            {
        //                                                planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == t.Id && !p.IsDisable && !p.IsDelete).ToList();
        //                                            }
        //                                            break;
        //                                    }
        //                                    if (t.ParentTargetGroupDetail != null)
        //                                    {
        //                                        tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).ToList().Map<CriterionDictionaryDTO>();
        //                                    }
        //                                    else
        //                                    {
        //                                        tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
        //                                    }

        //                                    foreach (PlanKPIDetail pld in planDetails)
        //                                    {

        //                                        ResultDetail rdetail = resultDetails.SingleOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == pld.Id && r.TargetGroupDetail.Id == t.Id);
        //                                        ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
        //                                        //rd.Id = pld.Id;

        //                                        rd.CriterionId = pld.FromCriterion != null ? pld.FromCriterion.Id : Guid.Empty;
        //                                        rd.CriterionName = pld.FromCriterion != null ? pld.FromCriterion.Name : string.Empty;
        //                                        rd.CriterionTypeId = pld.FromCriterion != null ? pld.FromCriterion.CriterionType.Id : -1;
        //                                        rd.Tooltip = pld.FromCriterion != null ? pld.FromCriterion.Tooltip : string.Empty;
        //                                        rd.Density = ControllerHelpers.GetOriginalDensity(pld.Id, session);// .GetOriginalParentPlanKPIDetail(pld.Id, session).MaxRecord;
        //                                        if (tg.TargetGroupDetailTypeId == 3)
        //                                        {
        //                                            rd.Density = 100;
        //                                        }
        //                                        try
        //                                        {
        //                                            if (pld.CurrentKPI != null)
        //                                            {
        //                                                Guid currentKPIId = new Guid(pld.CurrentKPI);
        //                                                if (t.ParentTargetGroupDetail != null)
        //                                                {
        //                                                    rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).Select(c => c.Name).SingleOrDefault();
        //                                                }
        //                                                else
        //                                                {
        //                                                    rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.Id).Select(c => c.Name).SingleOrDefault();
        //                                                }

        //                                            }
        //                                        }
        //                                        catch (Exception e)
        //                                        {

        //                                        }
        //                                        rd.PlanKPIDetailId = pld.Id;
        //                                        rd.PlanKPIDetailName = pld.TargetDetail;
        //                                        rd.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
        //                                        if (pld.PlanKPIDetail_KPIs != null)
        //                                        {
        //                                            foreach (PlanKPIDetail_KPI m in pld.PlanKPIDetail_KPIs)
        //                                            {
        //                                                PlanKPIDetail_KPIDTO kpi = new PlanKPIDetail_KPIDTO();
        //                                                kpi.Id = m.Id;
        //                                                kpi.Name = m.Name;
        //                                                kpi.MeasureUnitId = m.MeasureUnit != null ? m.MeasureUnit.Id : 0;
        //                                                kpi.MeasureUnitName = m.MeasureUnit != null ? m.MeasureUnit.Name : "";
        //                                                rd.PlanKPIDetail_KPIs.Add(kpi);
        //                                            }
        //                                        }

        //                                        rd.MaxRecord = ControllerHelpers.GetOriginalDensity(pld.Id, session);
        //                                        rd.PlanKPIDetailNameString = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).TargetDetail;

        //                                        if (rdetail != null)
        //                                        {
        //                                            rd.PreviousResult = rdetail.PreviousResult;
        //                                            rd.CurrentResult = rdetail.CurrentResult;
        //                                            if (rdetail.RecordSecond != 0 && ratingResult.IsUnlocked == true)
        //                                            {
        //                                                rd.Record = rdetail.RecordSecond;
        //                                                rd.RecordOld = rdetail.Record;
        //                                            }
        //                                            else
        //                                            {
        //                                                rd.Record = rdetail.Record;
        //                                                rd.RecordOld = rdetail.Record;
        //                                            }

        //                                            rd.SupervisorRecord = rdetail.SupervisorRecord;
        //                                            rd.Note = rdetail.Note;
        //                                            rd.SupervisorNote = rdetail.SupervisorNote;
        //                                            rd.FileAttachments = rdetail.FileAttachments.Map<FileAttachmentDTO>();
        //                                            rd.FileAttachmentCount = rd.FileAttachments.Count();
        //                                            rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
        //                                            rd.Id = rdetail.Id;
        //                                        }
        //                                        else
        //                                        {

        //                                            rd.Id = Guid.NewGuid();
        //                                            ResultDetail newResultDetail = new ResultDetail()
        //                                            {
        //                                                Id = rd.Id,
        //                                                PlanKPIDetail = new PlanKPIDetail() { Id = pld.Id },
        //                                                Result = ratingResult,
        //                                                TargetGroupDetail = t
        //                                            };
        //                                            session.Save(newResultDetail);

        //                                        }
        //                                        if (t.TargetGroupDetailType.Id == 3 && rd.PlanKPIDetailName != null)
        //                                        {
        //                                            List<CriterionDictionary> crds = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail != null && c.TargetGroupDetail.Id == t.Id).ToList();
        //                                            rd.PlanKPIDetailNameString = crds.SingleOrDefault(c => c.Id == new Guid(rd.PlanKPIDetailName)).Name;
        //                                        }

        //                                        tg.ResultDetailDTOs.Add(rd);
        //                                    }
        //                                    double totalDensity = tg.ResultDetailDTOs.Sum(tr => tr.Density);
        //                                    tg.ResultDetailDTOs.ForEach(r =>
        //                                    {
        //                                        r.Density = Math.Round(r.Density / totalDensity, 2);
        //                                        r.DensityPercent = Math.Round(r.Density * 100, 0);
        //                                    });

        //                                    #endregion
        //                                }
        //                                break;
        //                            case (int)AgentObjectTypes.PhongBan:
        //                                {
        //                                    #region phòng ban

        //                                    List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();
        //                                    switch (t.TargetGroupDetailType.Id)
        //                                    {
        //                                        case 1:
        //                                            {
        //                                                planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail == null || p.TargetGroupDetail.Id == t.Id && !p.IsDisable && !p.IsDelete).ToList();
        //                                            }
        //                                            break;
        //                                        case 2:
        //                                            {
        //                                                planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail == null || p.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id && !p.IsDisable && !p.IsDelete).ToList();

        //                                            }
        //                                            break;
        //                                        case 3:
        //                                            {
        //                                                planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail == null || p.TargetGroupDetail.Id == t.Id && !p.IsDisable && !p.IsDelete).ToList();
        //                                            }
        //                                            break;
        //                                    }
        //                                    if (t.ParentTargetGroupDetail != null)
        //                                    {
        //                                        tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).ToList().Map<CriterionDictionaryDTO>();
        //                                    }
        //                                    else
        //                                    {
        //                                        tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
        //                                    }

        //                                    foreach (PlanKPIDetail pld in planDetails)
        //                                    {

        //                                        ResultDetail rdetail = resultDetails.SingleOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == pld.Id && r.TargetGroupDetail.Id == t.Id);
        //                                        ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
        //                                        //rd.Id = pld.Id;

        //                                        rd.CriterionId = pld.FromCriterion != null ? pld.FromCriterion.Id : Guid.Empty;
        //                                        rd.CriterionName = pld.FromCriterion != null ? pld.FromCriterion.Name : string.Empty;
        //                                        rd.CriterionTypeId = pld.FromCriterion != null ? pld.FromCriterion.CriterionType.Id : -1;
        //                                        rd.Tooltip = pld.FromCriterion != null ? pld.FromCriterion.Tooltip : string.Empty;
        //                                        rd.Density = ControllerHelpers.GetOriginalDensity(pld.Id, session);
        //                                        if (tg.TargetGroupDetailTypeId == 3)
        //                                        {
        //                                            rd.Density = 100;
        //                                        }
        //                                        try
        //                                        {
        //                                            if (pld.CurrentKPI != null)
        //                                            {
        //                                                Guid currentKPIId = new Guid(pld.CurrentKPI);
        //                                                if (t.ParentTargetGroupDetail != null)
        //                                                {
        //                                                    rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).Select(c => c.Name).SingleOrDefault();
        //                                                }
        //                                                else
        //                                                {
        //                                                    rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.Id).Select(c => c.Name).SingleOrDefault();
        //                                                }

        //                                            }
        //                                        }
        //                                        catch (Exception e)
        //                                        {

        //                                        }
        //                                        rd.PlanKPIDetailId = pld.Id;
        //                                        rd.PlanKPIDetailName = pld.TargetDetail;
        //                                        rd.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
        //                                        if (pld.PlanKPIDetail_KPIs != null)
        //                                        {
        //                                            foreach (PlanKPIDetail_KPI m in pld.PlanKPIDetail_KPIs)
        //                                            {
        //                                                PlanKPIDetail_KPIDTO kpi = new PlanKPIDetail_KPIDTO();
        //                                                kpi.Id = m.Id;
        //                                                kpi.Name = m.Name;
        //                                                kpi.MeasureUnitId = m.MeasureUnit != null ? m.MeasureUnit.Id : 0;
        //                                                kpi.MeasureUnitName = m.MeasureUnit != null ? m.MeasureUnit.Name : "";
        //                                                rd.PlanKPIDetail_KPIs.Add(kpi);
        //                                            }
        //                                        }

        //                                        rd.MaxRecord = ControllerHelpers.GetOriginalDensity(pld.Id, session);
        //                                        rd.PlanKPIDetailNameString = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).TargetDetail;
        //                                        if (rdetail != null)
        //                                        {
        //                                            rd.PreviousResult = rdetail.PreviousResult;
        //                                            rd.CurrentResult = rdetail.CurrentResult;
        //                                            if (rdetail.RecordSecond != 0 && ratingResult.IsUnlocked == true)
        //                                            {
        //                                                rd.Record = rdetail.RecordSecond;
        //                                                rd.RecordOld = rdetail.Record;
        //                                            }
        //                                            else
        //                                            {
        //                                                rd.Record = rdetail.Record;
        //                                                rd.RecordOld = rdetail.Record;
        //                                            }

        //                                            rd.SupervisorRecord = rdetail.SupervisorRecord;
        //                                            rd.Note = rdetail.Note;

        //                                            rd.SupervisorNote = rdetail.SupervisorNote;
        //                                            rd.FileAttachments = rdetail.FileAttachments.Map<FileAttachmentDTO>();
        //                                            rd.FileAttachmentCount = rd.FileAttachments.Count();
        //                                            rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
        //                                            rd.Id = rdetail.Id;
        //                                        }
        //                                        else
        //                                        {

        //                                            rd.Id = Guid.NewGuid();
        //                                            ResultDetail newResultDetail = new ResultDetail()
        //                                            {
        //                                                Id = rd.Id,
        //                                                PlanKPIDetail = new PlanKPIDetail() { Id = pld.Id },
        //                                                Result = ratingResult,
        //                                                TargetGroupDetail = t
        //                                            };
        //                                            session.Save(newResultDetail);

        //                                        }
        //                                        if (t.TargetGroupDetailType.Id == 3 && rd.PlanKPIDetailName != null)
        //                                        {
        //                                            List<CriterionDictionary> crds = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail != null && c.TargetGroupDetail.Id == t.Id).ToList();
        //                                            rd.PlanKPIDetailNameString = crds.SingleOrDefault(c => c.Id == new Guid(rd.PlanKPIDetailName)).Name;
        //                                        }

        //                                        tg.ResultDetailDTOs.Add(rd);
        //                                    }
        //                                    double totalDensity = tg.ResultDetailDTOs.Sum(tr => tr.Density);
        //                                    tg.ResultDetailDTOs.ForEach(r =>
        //                                    {
        //                                        if (totalDensity > 0)
        //                                        {
        //                                            r.Density = Math.Round(r.Density / totalDensity, 2);
        //                                            r.DensityPercent = Math.Round(r.Density * 100, 0);
        //                                        }
        //                                    });

        //                                    #endregion
        //                                }
        //                                break;
        //                            case (int)AgentObjectTypes.Khoa:
        //                                {
        //                                    #region khoa
        //                                    List<ProfessorCriterionPlanDTO> criterions = session.Query<ProfessorCriterion>().Where(c => c.TargetGroupDetail.Id == t.Id).Map<ProfessorCriterionPlanDTO>().ToList();
        //                                    foreach (ProfessorCriterionPlanDTO crdi in criterions)
        //                                    {
        //                                        PlanKPIMakingDetailDTO pld = new PlanKPIMakingDetailDTO();
        //                                        List<ResultDetail> staffResultDetails = session.Query<ResultDetail>().Where(r => r.PlanKPIDetail.FromProfessorCriterion != null && r.PlanKPIDetail.FromProfessorCriterion.Id == crdi.Id && r.Result.StaffRating.Department.Id == staff.Department.Id && r.IsTargetGroupRating == false && r.SupervisorRecord != 0 && r.PlanKPIDetail.PlanStaff.PlanKPI.Id == planId).ToList();
        //                                        ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
        //                                        rd.CriterionId = crdi.Id;
        //                                        rd.CriterionName = crdi.Name;
        //                                        rd.CriterionTypeId = crdi.CriterionType.Id;
        //                                        rd.Tooltip = crdi.Tooltip;
        //                                        List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == crdi.Id).Map<CriterionDictionaryDTO>().ToList();
        //                                        rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
        //                                        rd.PlanKPIDetailId = pld.Id;
        //                                        rd.PlanKPIDetailName = pld.Name;

        //                                        double resultRecord = staffResultDetails.Count > 0 ? staffResultDetails.Average(f => f.SupervisorRecord) : 0;
        //                                        rd.Record = Math.Round(resultRecord, 1);
        //                                        rd.SupervisorRecord = Math.Round(resultRecord, 1);
        //                                        rd.MaxRecord = crdi.Record;
        //                                        rd.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };

        //                                        switch (crdi.CriterionType.Id)
        //                                        {
        //                                            case 3:
        //                                                {
        //                                                    if (pld.CurrentKPI != null)
        //                                                    {
        //                                                        Guid crdId = new Guid(pld.CurrentKPI);
        //                                                        CriterionDictionary crd = session.Query<CriterionDictionary>().Where(c => c.Id == crdId).SingleOrDefault();
        //                                                        rd.RegisterTarget = crd.Name;
        //                                                    }
        //                                                }
        //                                                break;
        //                                            default:
        //                                                {
        //                                                    rd.RegisterTarget = pld.CurrentKPI;
        //                                                }
        //                                                break;
        //                                        }
        //                                        rd.Density = 1;
        //                                        tg.ResultDetailDTOs.Add(rd);
        //                                    }
        //                                    #endregion
        //                                }
        //                                break;
        //                            case (int)AgentObjectTypes.BoMon:
        //                                {
        //                                    #region bộ môn
        //                                    List<ProfessorCriterionPlanDTO> criterions = session.Query<ProfessorCriterion>().Where(c => c.TargetGroupDetail.Id == t.Id).Map<ProfessorCriterionPlanDTO>().ToList();
        //                                    foreach (ProfessorCriterionPlanDTO crdi in criterions)
        //                                    {
        //                                        PlanKPIMakingDetailDTO pld = new PlanKPIMakingDetailDTO();
        //                                        List<ResultDetail> staffResultDetails = session.Query<ResultDetail>().Where(r => r.PlanKPIDetail.FromProfessorCriterion != null && r.PlanKPIDetail.FromProfessorCriterion.Id == crdi.Id && r.Result.StaffRating.StaffInfo.Subject.Id == staff.StaffInfo.Subject.Id && r.IsTargetGroupRating == false && r.SupervisorRecord != 0 && r.PlanKPIDetail.PlanStaff.PlanKPI.Id == planId).ToList();
        //                                        ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
        //                                        rd.CriterionId = crdi.Id;
        //                                        rd.CriterionName = crdi.Name;
        //                                        rd.CriterionTypeId = crdi.CriterionType.Id;
        //                                        rd.Tooltip = crdi.Tooltip;
        //                                        List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == crdi.Id).Map<CriterionDictionaryDTO>().ToList();
        //                                        rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
        //                                        rd.PlanKPIDetailId = pld.Id;
        //                                        rd.PlanKPIDetailName = pld.Name;

        //                                        double resultRecord = staffResultDetails.Count > 0 ? staffResultDetails.Average(f => f.SupervisorRecord) : 0;
        //                                        rd.Record = Math.Round(resultRecord, 1);
        //                                        rd.SupervisorRecord = Math.Round(resultRecord, 1);
        //                                        rd.MaxRecord = crdi.Record;
        //                                        rd.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };

        //                                        switch (crdi.CriterionType.Id)
        //                                        {
        //                                            case 3:
        //                                                {
        //                                                    if (pld.CurrentKPI != null)
        //                                                    {
        //                                                        Guid crdId = new Guid(pld.CurrentKPI);
        //                                                        CriterionDictionary crd = session.Query<CriterionDictionary>().Where(c => c.Id == crdId).SingleOrDefault();
        //                                                        rd.RegisterTarget = crd.Name;
        //                                                    }
        //                                                }
        //                                                break;
        //                                            default:
        //                                                {
        //                                                    rd.RegisterTarget = pld.CurrentKPI;
        //                                                }
        //                                                break;
        //                                        }
        //                                        rd.Density = 1;
        //                                        tg.ResultDetailDTOs.Add(rd);
        //                                    }
        //                                    #endregion
        //                                }
        //                                break;
        //                            case (int)AgentObjectTypes.PhoPhongBan:
        //                                {
        //                                    #region Phó phòng ban

        //                                    List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();
        //                                    switch (t.TargetGroupDetailType.Id)
        //                                    {
        //                                        case 1:
        //                                            {
        //                                                planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == t.Id && !p.IsDisable && !p.IsDelete).ToList();
        //                                            }
        //                                            break;
        //                                        case 2:
        //                                            {
        //                                                planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id && !p.IsDisable && !p.IsDelete).ToList();

        //                                            }
        //                                            break;
        //                                        case 3:
        //                                            {
        //                                                planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == t.Id && !p.IsDisable && !p.IsDelete).ToList();
        //                                            }
        //                                            break;
        //                                    }
        //                                    if (t.ParentTargetGroupDetail != null)
        //                                    {
        //                                        tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).ToList().Map<CriterionDictionaryDTO>();
        //                                    }
        //                                    else
        //                                    {
        //                                        tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
        //                                    }

        //                                    foreach (PlanKPIDetail pld in planDetails)
        //                                    {

        //                                        ResultDetail rdetail = resultDetails.SingleOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == pld.Id && r.TargetGroupDetail.Id == t.Id);
        //                                        ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
        //                                        //rd.Id = pld.Id;

        //                                        rd.CriterionId = pld.FromCriterion != null ? pld.FromCriterion.Id : Guid.Empty;
        //                                        rd.CriterionName = pld.FromCriterion != null ? pld.FromCriterion.Name : string.Empty;
        //                                        rd.CriterionTypeId = pld.FromCriterion != null ? pld.FromCriterion.CriterionType.Id : -1;
        //                                        rd.Tooltip = pld.FromCriterion != null ? pld.FromCriterion.Tooltip : string.Empty;
        //                                        rd.Density = pld.MaxRecord;
        //                                        if (tg.TargetGroupDetailTypeId == 3)
        //                                        {
        //                                            rd.Density = 100;
        //                                        }
        //                                        try
        //                                        {
        //                                            if (pld.CurrentKPI != null)
        //                                            {
        //                                                Guid currentKPIId = new Guid(pld.CurrentKPI);
        //                                                if (t.ParentTargetGroupDetail != null)
        //                                                {
        //                                                    rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).Select(c => c.Name).SingleOrDefault();
        //                                                }
        //                                                else
        //                                                {
        //                                                    rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.Id).Select(c => c.Name).SingleOrDefault();
        //                                                }

        //                                            }
        //                                        }
        //                                        catch (Exception e)
        //                                        {

        //                                        }
        //                                        rd.PlanKPIDetailId = pld.Id;
        //                                        rd.PlanKPIDetailName = pld.TargetDetail;
        //                                        rd.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
        //                                        if (pld.PlanKPIDetail_KPIs != null)
        //                                        {
        //                                            foreach (PlanKPIDetail_KPI m in pld.PlanKPIDetail_KPIs)
        //                                            {
        //                                                PlanKPIDetail_KPIDTO kpi = new PlanKPIDetail_KPIDTO();
        //                                                kpi.Id = m.Id;
        //                                                kpi.Name = m.Name;
        //                                                kpi.MeasureUnitId = m.MeasureUnit != null ? m.MeasureUnit.Id : 0;
        //                                                kpi.MeasureUnitName = m.MeasureUnit != null ? m.MeasureUnit.Name : "";
        //                                                rd.PlanKPIDetail_KPIs.Add(kpi);
        //                                            }
        //                                        }

        //                                        rd.MaxRecord = ControllerHelpers.GetOriginalDensity(pld.Id, session);
        //                                        rd.PlanKPIDetailNameString = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).TargetDetail;

        //                                        if (rdetail != null)
        //                                        {
        //                                            rd.PreviousResult = rdetail.PreviousResult;
        //                                            rd.CurrentResult = rdetail.CurrentResult;
        //                                            if (rdetail.RecordSecond != 0 && ratingResult.IsUnlocked == true)
        //                                            {
        //                                                rd.Record = rdetail.RecordSecond;
        //                                                rd.RecordOld = rdetail.Record;
        //                                            }
        //                                            else
        //                                            {
        //                                                rd.Record = rdetail.Record;
        //                                                rd.RecordOld = rdetail.Record;
        //                                            }

        //                                            rd.SupervisorRecord = rdetail.SupervisorRecord;
        //                                            rd.Note = rdetail.Note;
        //                                            rd.SupervisorNote = rdetail.SupervisorNote;
        //                                            rd.FileAttachments = rdetail.FileAttachments.Map<FileAttachmentDTO>();
        //                                            rd.FileAttachmentCount = rd.FileAttachments.Count();
        //                                            rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
        //                                            rd.Id = rdetail.Id;
        //                                        }
        //                                        else
        //                                        {

        //                                            rd.Id = Guid.NewGuid();
        //                                            ResultDetail newResultDetail = new ResultDetail()
        //                                            {
        //                                                Id = rd.Id,
        //                                                PlanKPIDetail = new PlanKPIDetail() { Id = pld.Id },
        //                                                Result = ratingResult,
        //                                                TargetGroupDetail = t
        //                                            };
        //                                            session.Save(newResultDetail);

        //                                        }
        //                                        if (t.TargetGroupDetailType.Id == 3 && rd.PlanKPIDetailName != null)
        //                                        {
        //                                            List<CriterionDictionary> crds = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail != null && c.TargetGroupDetail.Id == t.Id).ToList();
        //                                            rd.PlanKPIDetailNameString = crds.SingleOrDefault(c => c.Id == new Guid(rd.PlanKPIDetailName)).Name;
        //                                        }

        //                                        tg.ResultDetailDTOs.Add(rd);
        //                                    }
        //                                    double totalDensity = tg.ResultDetailDTOs.Sum(tr => tr.Density);
        //                                    tg.ResultDetailDTOs.ForEach(r =>
        //                                    {
        //                                        r.Density = Math.Round(r.Density / totalDensity, 2);
        //                                        r.DensityPercent = Math.Round(r.Density * 100, 0);
        //                                    });

        //                                    #endregion
        //                                }
        //                                break;
        //                            case (int)AgentObjectTypes.PhoKhoa:
        //                                {
        //                                    #region phó trường khoa

        //                                    List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();

        //                                    planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == t.Id && !p.IsDisable && !p.IsDelete).ToList();
        //                                    //switch (t.TargetGroupDetailType.Id)
        //                                    //{
        //                                    //    case 0:
        //                                    //        {
        //                                    //            planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == t.Id).ToList();
        //                                    //        }
        //                                    //        break;

        //                                    //}
        //                                    if (t.ParentTargetGroupDetail != null)
        //                                    {
        //                                        tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).ToList().Map<CriterionDictionaryDTO>();
        //                                    }
        //                                    else
        //                                    {
        //                                        tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
        //                                    }

        //                                    foreach (PlanKPIDetail pld in planDetails)
        //                                    {

        //                                        ResultDetail rdetail = resultDetails.SingleOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == pld.Id && r.TargetGroupDetail.Id == t.Id);
        //                                        ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
        //                                        //rd.Id = pld.Id;

        //                                        rd.CriterionId = pld.FromCriterion != null ? pld.FromCriterion.Id : Guid.Empty;
        //                                        rd.CriterionName = pld.FromCriterion != null ? pld.FromCriterion.Name : string.Empty;
        //                                        rd.CriterionTypeId = pld.FromCriterion != null ? pld.FromCriterion.CriterionType.Id : -1;
        //                                        rd.Tooltip = pld.FromCriterion != null ? pld.FromCriterion.Tooltip : string.Empty;
        //                                        rd.Density = ControllerHelpers.GetOriginalDensity(pld.Id, session);
        //                                        if (tg.TargetGroupDetailTypeId == 3)
        //                                        {
        //                                            rd.Density = 100;
        //                                        }
        //                                        try
        //                                        {
        //                                            if (pld.CurrentKPI != null)
        //                                            {
        //                                                Guid currentKPIId = new Guid(pld.CurrentKPI);
        //                                                if (t.ParentTargetGroupDetail != null)
        //                                                {
        //                                                    rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).Select(c => c.Name).SingleOrDefault();
        //                                                }
        //                                                else
        //                                                {
        //                                                    rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.Id).Select(c => c.Name).SingleOrDefault();
        //                                                }

        //                                            }
        //                                        }
        //                                        catch (Exception e)
        //                                        {

        //                                        }
        //                                        rd.PlanKPIDetailId = pld.Id;
        //                                        rd.PlanKPIDetailName = pld.TargetDetail;
        //                                        rd.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
        //                                        if (pld.PlanKPIDetail_KPIs != null)
        //                                        {
        //                                            foreach (PlanKPIDetail_KPI m in pld.PlanKPIDetail_KPIs)
        //                                            {
        //                                                PlanKPIDetail_KPIDTO kpi = new PlanKPIDetail_KPIDTO();
        //                                                kpi.Id = m.Id;
        //                                                kpi.Name = m.Name;
        //                                                kpi.MeasureUnitId = m.MeasureUnit != null ? m.MeasureUnit.Id : 0;
        //                                                kpi.MeasureUnitName = m.MeasureUnit != null ? m.MeasureUnit.Name : "";
        //                                                rd.PlanKPIDetail_KPIs.Add(kpi);
        //                                            }
        //                                        }

        //                                        rd.MaxRecord = ControllerHelpers.GetOriginalDensity(pld.Id, session);
        //                                        rd.PlanKPIDetailNameString = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).TargetDetail;

        //                                        if (rdetail != null)
        //                                        {
        //                                            rd.PreviousResult = rdetail.PreviousResult;
        //                                            rd.CurrentResult = rdetail.CurrentResult;
        //                                            if (rdetail.RecordSecond != 0 && ratingResult.IsUnlocked == true)
        //                                            {
        //                                                rd.Record = rdetail.RecordSecond;
        //                                                rd.RecordOld = rdetail.Record;
        //                                            }
        //                                            else
        //                                            {
        //                                                rd.Record = rdetail.Record;
        //                                                rd.RecordOld = rdetail.Record;
        //                                            }

        //                                            rd.SupervisorRecord = rdetail.SupervisorRecord;
        //                                            rd.Note = rdetail.Note;
        //                                            rd.SupervisorNote = rdetail.SupervisorNote;
        //                                            rd.FileAttachments = rdetail.FileAttachments.Map<FileAttachmentDTO>();
        //                                            rd.FileAttachmentCount = rd.FileAttachments.Count();
        //                                            rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
        //                                            rd.Id = rdetail.Id;
        //                                        }
        //                                        else
        //                                        {

        //                                            rd.Id = Guid.NewGuid();
        //                                            ResultDetail newResultDetail = new ResultDetail()
        //                                            {
        //                                                Id = rd.Id,
        //                                                PlanKPIDetail = new PlanKPIDetail() { Id = pld.Id },
        //                                                Result = ratingResult,
        //                                                TargetGroupDetail = t
        //                                            };
        //                                            session.Save(newResultDetail);

        //                                        }
        //                                        if (t.TargetGroupDetailType.Id == 3 && rd.PlanKPIDetailName != null)
        //                                        {
        //                                            List<CriterionDictionary> crds = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail != null && c.TargetGroupDetail.Id == t.Id).ToList();
        //                                            rd.PlanKPIDetailNameString = crds.SingleOrDefault(c => c.Id == new Guid(rd.PlanKPIDetailName)).Name;
        //                                        }

        //                                        tg.ResultDetailDTOs.Add(rd);
        //                                    }
        //                                    double totalDensity = tg.ResultDetailDTOs.Sum(tr => tr.Density);
        //                                    tg.ResultDetailDTOs.ForEach(r =>
        //                                    {
        //                                        r.Density = Math.Round(r.Density / totalDensity, 2);
        //                                        r.DensityPercent = Math.Round(r.Density * 100, 0);
        //                                    });

        //                                    #endregion
        //                                }
        //                                break;
        //                            case (int)AgentObjectTypes.HieuTruong:
        //                                {
        //                                    #region hiệu trưởng

        //                                    List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();

        //                                    planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == t.Id && !p.IsDisable && !p.IsDelete).ToList();
        //                                    //switch (t.TargetGroupDetailType.Id)
        //                                    //{
        //                                    //    case 0:
        //                                    //        {
        //                                    //            planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == t.Id).ToList();
        //                                    //        }
        //                                    //        break;

        //                                    //}
        //                                    if (t.ParentTargetGroupDetail != null)
        //                                    {
        //                                        tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).ToList().Map<CriterionDictionaryDTO>();
        //                                    }
        //                                    else
        //                                    {
        //                                        tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
        //                                    }

        //                                    foreach (PlanKPIDetail pld in planDetails)
        //                                    {

        //                                        ResultDetail rdetail = resultDetails.SingleOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == pld.Id && r.TargetGroupDetail.Id == t.Id);
        //                                        ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
        //                                        //rd.Id = pld.Id;

        //                                        rd.CriterionId = pld.FromCriterion != null ? pld.FromCriterion.Id : Guid.Empty;
        //                                        rd.CriterionName = pld.FromCriterion != null ? pld.FromCriterion.Name : string.Empty;
        //                                        rd.CriterionTypeId = pld.FromCriterion != null ? pld.FromCriterion.CriterionType.Id : -1;
        //                                        rd.Tooltip = pld.FromCriterion != null ? pld.FromCriterion.Tooltip : string.Empty;
        //                                        rd.Density = ControllerHelpers.GetOriginalDensity(pld.Id, session);
        //                                        if (tg.TargetGroupDetailTypeId == 3)
        //                                        {
        //                                            rd.Density = 100;
        //                                        }
        //                                        try
        //                                        {
        //                                            if (pld.CurrentKPI != null)
        //                                            {
        //                                                Guid currentKPIId = new Guid(pld.CurrentKPI);
        //                                                if (t.ParentTargetGroupDetail != null)
        //                                                {
        //                                                    rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).Select(c => c.Name).SingleOrDefault();
        //                                                }
        //                                                else
        //                                                {
        //                                                    rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.Id).Select(c => c.Name).SingleOrDefault();
        //                                                }

        //                                            }
        //                                        }
        //                                        catch (Exception e)
        //                                        {

        //                                        }
        //                                        rd.PlanKPIDetailId = pld.Id;
        //                                        rd.PlanKPIDetailName = pld.TargetDetail;
        //                                        rd.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
        //                                        if (pld.PlanKPIDetail_KPIs != null)
        //                                        {
        //                                            foreach (PlanKPIDetail_KPI m in pld.PlanKPIDetail_KPIs)
        //                                            {
        //                                                PlanKPIDetail_KPIDTO kpi = new PlanKPIDetail_KPIDTO();
        //                                                kpi.Id = m.Id;
        //                                                kpi.Name = m.Name;
        //                                                kpi.MeasureUnitId = m.MeasureUnit != null ? m.MeasureUnit.Id : 0;
        //                                                kpi.MeasureUnitName = m.MeasureUnit != null ? m.MeasureUnit.Name : "";
        //                                                rd.PlanKPIDetail_KPIs.Add(kpi);
        //                                            }
        //                                        }

        //                                        rd.MaxRecord = ControllerHelpers.GetOriginalDensity(pld.Id, session);
        //                                        rd.PlanKPIDetailNameString = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).TargetDetail;

        //                                        if (rdetail != null)
        //                                        {
        //                                            rd.PreviousResult = rdetail.PreviousResult;
        //                                            rd.CurrentResult = rdetail.CurrentResult;
        //                                            if (rdetail.RecordSecond != 0 && ratingResult.IsUnlocked == true)
        //                                            {
        //                                                rd.Record = rdetail.RecordSecond;
        //                                                rd.RecordOld = rdetail.Record;
        //                                            }
        //                                            else
        //                                            {
        //                                                rd.Record = rdetail.Record;
        //                                                rd.RecordOld = rdetail.Record;
        //                                            }

        //                                            rd.SupervisorRecord = rdetail.SupervisorRecord;
        //                                            rd.Note = rdetail.Note;
        //                                            rd.SupervisorNote = rdetail.SupervisorNote;
        //                                            rd.FileAttachments = rdetail.FileAttachments.Map<FileAttachmentDTO>();
        //                                            rd.FileAttachmentCount = rd.FileAttachments.Count();
        //                                            rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
        //                                            rd.Id = rdetail.Id;
        //                                        }
        //                                        else
        //                                        {

        //                                            rd.Id = Guid.NewGuid();
        //                                            ResultDetail newResultDetail = new ResultDetail()
        //                                            {
        //                                                Id = rd.Id,
        //                                                PlanKPIDetail = new PlanKPIDetail() { Id = pld.Id },
        //                                                Result = ratingResult,
        //                                                TargetGroupDetail = t
        //                                            };
        //                                            session.Save(newResultDetail);

        //                                        }
        //                                        if (t.TargetGroupDetailType.Id == 3 && rd.PlanKPIDetailName != null)
        //                                        {
        //                                            List<CriterionDictionary> crds = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail != null && c.TargetGroupDetail.Id == t.Id).ToList();
        //                                            rd.PlanKPIDetailNameString = crds.SingleOrDefault(c => c.Id == new Guid(rd.PlanKPIDetailName)).Name;
        //                                        }

        //                                        tg.ResultDetailDTOs.Add(rd);
        //                                    }
        //                                    double totalDensity = tg.ResultDetailDTOs.Sum(tr => tr.Density);
        //                                    tg.ResultDetailDTOs.ForEach(r =>
        //                                    {
        //                                        r.Density = Math.Round(r.Density / totalDensity, 2);
        //                                        r.DensityPercent = Math.Round(r.Density * 100, 0);
        //                                    });

        //                                    #endregion
        //                                }
        //                                break;
        //                            case (int)AgentObjectTypes.PhoHieuTruong:
        //                                {
        //                                    #region phó hiệu trưởng

        //                                    List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();

        //                                    planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == t.Id && !p.IsDisable && !p.IsDelete).ToList();
        //                                    //switch (t.TargetGroupDetailType.Id)
        //                                    //{
        //                                    //    case 0:
        //                                    //        {
        //                                    //            planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == t.Id).ToList();
        //                                    //        }
        //                                    //        break;

        //                                    //}
        //                                    if (t.ParentTargetGroupDetail != null)
        //                                    {
        //                                        tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).ToList().Map<CriterionDictionaryDTO>();
        //                                    }
        //                                    else
        //                                    {
        //                                        tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
        //                                    }

        //                                    foreach (PlanKPIDetail pld in planDetails)
        //                                    {

        //                                        ResultDetail rdetail = resultDetails.SingleOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == pld.Id && r.TargetGroupDetail.Id == t.Id);
        //                                        ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
        //                                        //rd.Id = pld.Id;

        //                                        rd.CriterionId = pld.FromCriterion != null ? pld.FromCriterion.Id : Guid.Empty;
        //                                        rd.CriterionName = pld.FromCriterion != null ? pld.FromCriterion.Name : string.Empty;
        //                                        rd.CriterionTypeId = pld.FromCriterion != null ? pld.FromCriterion.CriterionType.Id : -1;
        //                                        rd.Tooltip = pld.FromCriterion != null ? pld.FromCriterion.Tooltip : string.Empty;
        //                                        rd.Density = ControllerHelpers.GetOriginalDensity(pld.Id, session);
        //                                        if (tg.TargetGroupDetailTypeId == 3)
        //                                        {
        //                                            rd.Density = 100;
        //                                        }
        //                                        try
        //                                        {
        //                                            if (pld.CurrentKPI != null)
        //                                            {
        //                                                Guid currentKPIId = new Guid(pld.CurrentKPI);
        //                                                if (t.ParentTargetGroupDetail != null)
        //                                                {
        //                                                    rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).Select(c => c.Name).SingleOrDefault();
        //                                                }
        //                                                else
        //                                                {
        //                                                    rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.Id).Select(c => c.Name).SingleOrDefault();
        //                                                }

        //                                            }
        //                                        }
        //                                        catch (Exception e)
        //                                        {

        //                                        }
        //                                        rd.PlanKPIDetailId = pld.Id;
        //                                        rd.PlanKPIDetailName = pld.TargetDetail;
        //                                        rd.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
        //                                        if (pld.PlanKPIDetail_KPIs != null)
        //                                        {
        //                                            foreach (PlanKPIDetail_KPI m in pld.PlanKPIDetail_KPIs)
        //                                            {
        //                                                PlanKPIDetail_KPIDTO kpi = new PlanKPIDetail_KPIDTO();
        //                                                kpi.Id = m.Id;
        //                                                kpi.Name = m.Name;
        //                                                kpi.MeasureUnitId = m.MeasureUnit != null ? m.MeasureUnit.Id : 0;
        //                                                kpi.MeasureUnitName = m.MeasureUnit != null ? m.MeasureUnit.Name : "";
        //                                                rd.PlanKPIDetail_KPIs.Add(kpi);
        //                                            }
        //                                        }

        //                                        rd.MaxRecord = ControllerHelpers.GetOriginalDensity(pld.Id, session);
        //                                        rd.PlanKPIDetailNameString = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).TargetDetail;

        //                                        if (rdetail != null)
        //                                        {
        //                                            rd.PreviousResult = rdetail.PreviousResult;
        //                                            rd.CurrentResult = rdetail.CurrentResult;
        //                                            if (rdetail.RecordSecond != 0 && ratingResult.IsUnlocked == true)
        //                                            {
        //                                                rd.Record = rdetail.RecordSecond;
        //                                                rd.RecordOld = rdetail.Record;
        //                                            }
        //                                            else
        //                                            {
        //                                                rd.Record = rdetail.Record;
        //                                                rd.RecordOld = rdetail.Record;
        //                                            }

        //                                            rd.SupervisorRecord = rdetail.SupervisorRecord;
        //                                            rd.Note = rdetail.Note;
        //                                            rd.SupervisorNote = rdetail.SupervisorNote;
        //                                            rd.FileAttachments = rdetail.FileAttachments.Map<FileAttachmentDTO>();
        //                                            rd.FileAttachmentCount = rd.FileAttachments.Count();
        //                                            rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
        //                                            rd.Id = rdetail.Id;
        //                                        }
        //                                        else
        //                                        {

        //                                            rd.Id = Guid.NewGuid();
        //                                            ResultDetail newResultDetail = new ResultDetail()
        //                                            {
        //                                                Id = rd.Id,
        //                                                PlanKPIDetail = new PlanKPIDetail() { Id = pld.Id },
        //                                                Result = ratingResult,
        //                                                TargetGroupDetail = t
        //                                            };
        //                                            session.Save(newResultDetail);

        //                                        }
        //                                        if (t.TargetGroupDetailType.Id == 3 && rd.PlanKPIDetailName != null)
        //                                        {
        //                                            List<CriterionDictionary> crds = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail != null && c.TargetGroupDetail.Id == t.Id).ToList();
        //                                            rd.PlanKPIDetailNameString = crds.SingleOrDefault(c => c.Id == new Guid(rd.PlanKPIDetailName)).Name;
        //                                        }

        //                                        tg.ResultDetailDTOs.Add(rd);
        //                                    }
        //                                    double totalDensity = tg.ResultDetailDTOs.Sum(tr => tr.Density);
        //                                    tg.ResultDetailDTOs.ForEach(r =>
        //                                    {
        //                                        r.Density = Math.Round(r.Density / totalDensity, 2);
        //                                        r.DensityPercent = Math.Round(r.Density * 100, 0);
        //                                    });

        //                                    #endregion
        //                                }
        //                                break;
        //                        }

        //                        tg.Id = t.Id;
        //                        tg.Name = t.Name;
        //                        result.RatingResultId = ratingResult.Id;
        //                        tg.Density = t.Density;
        //                        result.TargetGroupRatingDTOs.Add(tg);
        //                    }
        //                    #endregion
        //                    var temBonusRecords = resultDetails.Where(r => r.Result.StaffRating.Id == staff.Id && r.PlanKPIDetail == null);
        //                    if (temBonusRecords.Count() <= 0)
        //                        result.BonusRecordList.Add(new ResultDetailRatingDTO() { Id = Guid.Empty, CurrentResult = "", Record = 0 });
        //                    else
        //                    {
        //                        result.BonusRecordList = new List<ResultDetailRatingDTO>();
        //                        foreach (ResultDetail rd in temBonusRecords)
        //                        {
        //                            ResultDetailRatingDTO rdd = new ResultDetailRatingDTO();
        //                            rdd.Note = rd.Note;
        //                            rdd.SupervisorNote = rd.SupervisorNote;
        //                            rdd.CurrentResult = rd.CurrentResult;
        //                            rdd.Record = rd.Record;
        //                            rdd.FileAttachments = new List<FileAttachmentDTO>();
        //                            foreach (FileAttachment fad in rd.FileAttachments)
        //                            {
        //                                FileAttachmentDTO fa = new FileAttachmentDTO();
        //                                fa.Id = fad.Id;
        //                                fa.CreationTime = fad.CreationTime;
        //                                fa.Extension = fad.Extension;
        //                                fa.Name = fad.Name;
        //                                fa.Path = fad.Path;
        //                                fa.ResultDetailId = fad.ResultDetail.Id;
        //                                rdd.FileAttachments.Add(fa);
        //                            }
        //                            rdd.Id = rd.Id;
        //                            result.BonusRecordList.Add(rdd);
        //                        }
        //                    }
        //                    #region Additional Result
        //                    int agentObjectTypeId = -1;
        //                    if (staff.StaffInfo.Position == null)
        //                    {
        //                        if (staff.StaffInfo.StaffType.ManageCode == "3")
        //                            agentObjectTypeId = 2; //Nhân viên
        //                        else
        //                            agentObjectTypeId = 1; //Giảng viên
        //                    }
        //                    else
        //                    {
        //                        agentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
        //                    }
        //                    if (staff.StaffInfo.AgentObjects.Count > 1)
        //                    {
        //                        AgentObject agentObj = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).SingleOrDefault();
        //                        agentObjectTypeId = ao.AgentObjectType.Id;
        //                    }
        //                    switch (agentObjectTypeId)
        //                    {
        //                        case (int)AgentObjectTypes.GiangVien:
        //                            {
        //                                #region giảng viên
        //                                result.ProfessorAdditionalResultDetailDTOs = new List<ResultDetailRatingDTO>();
        //                                List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();
        //                                planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail == null && !p.IsDisable).ToList();

        //                                foreach (PlanKPIDetail pld in planDetails)
        //                                {
        //                                    ResultDetail rdetail = session.Query<ResultDetail>().SingleOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == true);
        //                                    ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
        //                                    rd.CriterionName = pld.TargetDetail;

        //                                    if (rdetail == null)
        //                                    {
        //                                        rd.PlanKPIDetail = new PlanKPIDetail() { Id = pld.Id };
        //                                        rd.PlanKPIDetailId = pld.Id;
        //                                        rd.Id = Guid.NewGuid();
        //                                        ResultDetail newResultDetail = new ResultDetail()
        //                                        {
        //                                            Id = rd.Id,
        //                                            PlanKPIDetail = new PlanKPIDetail() { Id = pld.Id },
        //                                            Result = ratingResult,
        //                                            IsTargetGroupRating = true
        //                                        };
        //                                        session.Save(newResultDetail);
        //                                    }
        //                                    else
        //                                    {
        //                                        //rd = rdetail.Map<ResultDetailRatingDTO>();
        //                                        rd.PreviousResult = rdetail.PreviousResult;
        //                                        rd.CurrentResult = rdetail.CurrentResult;
        //                                        rd.RegisterTarget = rdetail.RegisterTarget;
        //                                        rd.Record = rdetail.Record;

        //                                        rd.SupervisorRecord = rdetail.SupervisorRecord;
        //                                        rd.PlanKPIDetailId = rdetail.PlanKPIDetail.Id;
        //                                        rd.Id = rdetail.Id;
        //                                        rd.Note = rdetail.Note;
        //                                        rd.SupervisorNote = rdetail.SupervisorNote;
        //                                        rd.FileAttachments = rdetail.FileAttachments.Map<FileAttachmentDTO>();
        //                                        rd.FileAttachmentCount = rd.FileAttachments.Count();
        //                                        rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
        //                                    }
        //                                    rd.IsConfirmed = rdetail != null ? rdetail.IsConfirmed : false;
        //                                    result.ProfessorAdditionalResultDetailDTOs.Add(rd);
        //                                }
        //                                #endregion
        //                            }
        //                            break;
        //                    }
        //                    #endregion

        //                    result.RatingResultId = ratingResult.Id;
        //                    result.IsLocked = ratingResult.IsLocked;
        //                    result.IsCommitted = ratingResult.IsCommitted;
        //                    result.TotalRecord = ratingResult.TotalRecord;
        //                    result.TotalRecordSecond = ratingResult.TotalRecordSecond;
        //                    result.MaxBonusRecord = session.Query<KPIConfiguration>().FirstOrDefault().MaxBonusRecord;
        //                    result.IsFreeRating = true;
        //                }
        //                else
        //                    result.Id = Guid.Empty;
        //            }
                       


        //                foreach (TargetGroupRatingDTO tg in result.TargetGroupRatingDTOs)
        //                {
        //                    try
        //                    {
        //                        tg.ResultDetailDTOs = tg.ResultDetailDTOs.OrderBy(pl => ControllerHelpers.GetOriginalMethods(pl.PlanKPIDetailId).Min(pld => pld.StartTime)).ThenBy(pl => ControllerHelpers.GetOriginalMethods(pl.PlanKPIDetailId).Min(pld => pld.EndTime)).ThenBy(pl => pl.PlanKPIDetailName).ToList();
        //                    }
        //                    catch (Exception e)
        //                    {

        //                    }
        //                }
                    
                    
        //            });
            
        //        }
        ////}
        ////    catch (Exception e)
        ////    {

        ////    }
        
        //    return result;
        //}

        //[Authorize]
        //[Route("")]
        //public ResultDetailDTO GetClass(Guid id)
        //{
        //    ResultDetailDTO result = new ResultDetailDTO();
        //    SessionManager.DoWork(session =>
        //    {
        //        result = session.Query<ResultDetail>().SingleOrDefault(a => a.Id == id).Map<ResultDetailDTO>();
        //    });
        //    return result;
        //}

        //[Authorize]
        //[Route("")]
        //public RatingKPIResultDTO Put(RatingKPIResultDTO obj)
        //{
        //    SessionManager.DoWork(session =>
        //    {
        //        Staff staff = session.Query<Staff>().SingleOrDefault(s => s.Id == obj.StaffId);
        //        PlanStaff planStaff = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Id == obj.PlanStaffId);
        //        PlanKPI plan = session.Query<PlanKPI>().FirstOrDefault();
        //        Result ratingResult = session.Query<Result>().SingleOrDefault(r => r.Id == obj.RatingResultId);
        //        if (obj.IsSupervisor == true)
        //        {
        //            ratingResult.IsLocked = true;
        //        }
        //        ratingResult.Time = DateTime.Now;
        //        ratingResult.TotalRecord = obj.TotalSumRecord;
        //        //điểm tạm của nhân viên tự đánh giá
        //        ratingResult.TempRecord = obj.TempRecord;
        //        if (obj.IsAdminRating == true)
        //        {
        //            ratingResult.TotalRecordSecond = obj.TotalRecordSecond;
        //            ratingResult.TotalRecord = obj.TotalRecord;
        //            ratingResult.NumberOfEditing += 1;
        //        }
        //        session.Update(ratingResult);

        //        switch (obj.StaffDTO.AgentObjectTypeId)
        //        {
        //            case (int)AgentObjectTypes.GiangVien:
        //                {
        //                    #region Giảng viên


        //                    foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
        //                    {
        //                        foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
        //                        {
        //                            ResultDetail rd = new ResultDetail();
        //                            rd.CurrentResult = p.CurrentResult;
        //                            rd.PreviousResult = p.PreviousResult;
        //                            if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
        //                            {
        //                                rd.RecordSecond = p.Record;
        //                                rd.Record = p.RecordOld;
        //                            }
        //                            else
        //                            {
        //                                rd.Record = p.Record;
        //                            }
        //                            if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
        //                            {
        //                                rd.SupervisorRecord = p.Record;
        //                            }
        //                            rd.RegisterTarget = p.RegisterTarget;
        //                            rd.SupervisorRecord = p.SupervisorRecord;
        //                            rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
        //                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
        //                            rd.Result = ratingResult;
        //                            rd.IsTargetGroupRating = false;
        //                            if (p.Id == Guid.Empty)
        //                            {
        //                                rd.Id = Guid.NewGuid();
        //                                session.Save(rd);
        //                            }
        //                            else
        //                            {
        //                                rd.Id = p.Id;
        //                                session.Merge(rd);
        //                            }

        //                        }
        //                    }
        //                    foreach (ResultDetailRatingDTO p in obj.ProfessorAdditionalResultDetailDTOs)
        //                    {
        //                        ResultDetail rd = new ResultDetail();
        //                        rd.CurrentResult = p.CurrentResult;
        //                        rd.PreviousResult = p.PreviousResult;
        //                        rd.Record = p.Record;
        //                        rd.RegisterTarget = p.RegisterTarget;
        //                        rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
        //                        rd.IsTargetGroupRating = true;
        //                        rd.IsConfirmed = p.IsConfirmed;
        //                        rd.Result = ratingResult;
        //                        rd.Note = p.Note;
        //                        rd.SupervisorNote = p.SupervisorNote;
        //                        if (p.Id == Guid.Empty)
        //                        {
        //                            rd.Id = Guid.NewGuid();
        //                            session.Save(rd);
        //                        }
        //                        else
        //                        {
        //                            rd.Id = p.Id;
        //                            session.Merge(rd);
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                break;
        //            case (int)AgentObjectTypes.NhanVien:
        //                {
        //                    #region Nhân viên


        //                    foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
        //                    {
        //                        foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
        //                        {
        //                            ResultDetail rd = new ResultDetail();

        //                            rd.Id = p.Id;
        //                            rd.Note = p.Note;
        //                            rd.SupervisorNote = p.SupervisorNote;
        //                            rd.PreviousResult = p.PreviousResult;
        //                            rd.CurrentResult = p.CurrentResult;
        //                            if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
        //                            {
        //                                rd.RecordSecond = p.Record;
        //                                rd.Record = p.RecordOld;
        //                            }
        //                            else
        //                            {
        //                                rd.Record = p.Record;
        //                            }
        //                            rd.RegisterTarget = p.RegisterTarget;
        //                            rd.SupervisorRecord = p.SupervisorRecord;
        //                            rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
        //                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
        //                            rd.FileAttachments = new List<FileAttachment>();
        //                            if (p.FileAttachments != null)
        //                            {
        //                                foreach (FileAttachmentDTO fad in p.FileAttachments)
        //                                {
        //                                    FileAttachment fa = new FileAttachment();
        //                                    fa.Id = fad.Id;
        //                                    fa.CreationTime = fad.CreationTime;
        //                                    fa.Extension = fad.Extension;
        //                                    fa.Name = fad.Name;
        //                                    fa.Path = fad.Path;
        //                                    fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
        //                                    rd.FileAttachments.Add(fa);
        //                                }
        //                            }
        //                            rd.Result = ratingResult;
        //                            if (p.Id == Guid.Empty)
        //                            {
        //                                rd.Id = Guid.NewGuid();
        //                                session.Save(rd);
        //                            }
        //                            else
        //                            {
        //                                session.Merge(rd);
        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                break;
        //            case (int)AgentObjectTypes.PhongBan:
        //                {
        //                    #region Trưởng phòng


        //                    foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
        //                    {
        //                        foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
        //                        {
        //                            ResultDetail rd = new ResultDetail();

        //                            rd.Id = p.Id;
        //                            rd.Note = p.Note;
        //                            rd.SupervisorNote = p.SupervisorNote;
        //                            rd.PreviousResult = p.PreviousResult;
        //                            rd.CurrentResult = p.CurrentResult;
        //                            if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
        //                            {
        //                                rd.RecordSecond = p.Record;
        //                                rd.Record = p.RecordOld;
        //                            }
        //                            else
        //                            {
        //                                rd.Record = p.Record;
        //                            }
        //                            rd.RegisterTarget = p.RegisterTarget;
        //                            rd.SupervisorRecord = p.SupervisorRecord;
        //                            rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
        //                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
        //                            rd.FileAttachments = new List<FileAttachment>();
        //                            if (p.FileAttachments != null)
        //                            {
        //                                foreach (FileAttachmentDTO fad in p.FileAttachments)
        //                                {
        //                                    FileAttachment fa = new FileAttachment();
        //                                    fa.Id = fad.Id;
        //                                    fa.CreationTime = fad.CreationTime;
        //                                    fa.Extension = fad.Extension;
        //                                    fa.Name = fad.Name;
        //                                    fa.Path = fad.Path;
        //                                    fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
        //                                    rd.FileAttachments.Add(fa);
        //                                }
        //                            }
        //                            rd.Result = ratingResult;
        //                            if (p.Id == Guid.Empty)
        //                            {
        //                                rd.Id = Guid.NewGuid();
        //                                session.Save(rd);
        //                            }
        //                            else
        //                            {
        //                                session.Merge(rd);
        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                break;
        //            case (int)AgentObjectTypes.PhoPhongBan:
        //                {
        //                    #region Phó phòng


        //                    foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
        //                    {
        //                        foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
        //                        {
        //                            ResultDetail rd = new ResultDetail();

        //                            rd.Id = p.Id;
        //                            rd.Note = p.Note;
        //                            rd.SupervisorNote = p.SupervisorNote;
        //                            rd.PreviousResult = p.PreviousResult;
        //                            rd.CurrentResult = p.CurrentResult;
        //                            if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
        //                            {
        //                                rd.RecordSecond = p.Record;
        //                                rd.Record = p.RecordOld;
        //                            }
        //                            else
        //                            {
        //                                rd.Record = p.Record;
        //                            }
        //                            rd.RegisterTarget = p.RegisterTarget;
        //                            rd.SupervisorRecord = p.SupervisorRecord;
        //                            rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
        //                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
        //                            rd.FileAttachments = new List<FileAttachment>();
        //                            if (p.FileAttachments != null)
        //                            {
        //                                foreach (FileAttachmentDTO fad in p.FileAttachments)
        //                                {
        //                                    FileAttachment fa = new FileAttachment();
        //                                    fa.Id = fad.Id;
        //                                    fa.CreationTime = fad.CreationTime;
        //                                    fa.Extension = fad.Extension;
        //                                    fa.Name = fad.Name;
        //                                    fa.Path = fad.Path;
        //                                    fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
        //                                    rd.FileAttachments.Add(fa);
        //                                }
        //                            }
        //                            rd.Result = ratingResult;
        //                            if (p.Id == Guid.Empty)
        //                            {
        //                                rd.Id = Guid.NewGuid();
        //                                session.Save(rd);
        //                            }
        //                            else
        //                            {
        //                                session.Merge(rd);
        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                break;
        //            case (int)AgentObjectTypes.PhoKhoa:
        //                {
        //                    #region Phó khoa


        //                    foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
        //                    {
        //                        foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
        //                        {
        //                            ResultDetail rd = new ResultDetail();

        //                            rd.Id = p.Id;
        //                            rd.Note = p.Note;
        //                            rd.SupervisorNote = p.SupervisorNote;
        //                            rd.PreviousResult = p.PreviousResult;
        //                            rd.CurrentResult = p.CurrentResult;
        //                            if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
        //                            {
        //                                rd.RecordSecond = p.Record;
        //                                rd.Record = p.RecordOld;
        //                            }
        //                            else
        //                            {
        //                                rd.Record = p.Record;
        //                            }
        //                            rd.RegisterTarget = p.RegisterTarget;
        //                            rd.SupervisorRecord = p.SupervisorRecord;
        //                            rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
        //                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
        //                            rd.FileAttachments = new List<FileAttachment>();
        //                            if (p.FileAttachments != null)
        //                            {
        //                                foreach (FileAttachmentDTO fad in p.FileAttachments)
        //                                {
        //                                    FileAttachment fa = new FileAttachment();
        //                                    fa.Id = fad.Id;
        //                                    fa.CreationTime = fad.CreationTime;
        //                                    fa.Extension = fad.Extension;
        //                                    fa.Name = fad.Name;
        //                                    fa.Path = fad.Path;
        //                                    fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
        //                                    rd.FileAttachments.Add(fa);
        //                                }
        //                            }
        //                            rd.Result = ratingResult;
        //                            if (p.Id == Guid.Empty)
        //                            {
        //                                rd.Id = Guid.NewGuid();
        //                                session.Save(rd);
        //                            }
        //                            else
        //                            {
        //                                session.Merge(rd);
        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                break;
        //            case (int)AgentObjectTypes.HieuTruong:
        //                {
        //                    #region Hiệu trưởng


        //                    foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
        //                    {
        //                        foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
        //                        {
        //                            ResultDetail rd = new ResultDetail();

        //                            rd.Id = p.Id;
        //                            rd.Note = p.Note;
        //                            rd.SupervisorNote = p.SupervisorNote;
        //                            rd.PreviousResult = p.PreviousResult;
        //                            rd.CurrentResult = p.CurrentResult;
        //                            if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
        //                            {
        //                                rd.RecordSecond = p.Record;
        //                                rd.Record = p.RecordOld;
        //                            }
        //                            else
        //                            {
        //                                rd.Record = p.Record;
        //                            }
        //                            rd.RegisterTarget = p.RegisterTarget;
        //                            rd.SupervisorRecord = p.SupervisorRecord;
        //                            rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
        //                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
        //                            rd.FileAttachments = new List<FileAttachment>();
        //                            if (p.FileAttachments != null)
        //                            {
        //                                foreach (FileAttachmentDTO fad in p.FileAttachments)
        //                                {
        //                                    FileAttachment fa = new FileAttachment();
        //                                    fa.Id = fad.Id;
        //                                    fa.CreationTime = fad.CreationTime;
        //                                    fa.Extension = fad.Extension;
        //                                    fa.Name = fad.Name;
        //                                    fa.Path = fad.Path;
        //                                    fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
        //                                    rd.FileAttachments.Add(fa);
        //                                }
        //                            }
        //                            rd.Result = ratingResult;
        //                            if (p.Id == Guid.Empty)
        //                            {
        //                                rd.Id = Guid.NewGuid();
        //                                session.Save(rd);
        //                            }
        //                            else
        //                            {
        //                                session.Merge(rd);
        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                break;
        //            case (int)AgentObjectTypes.PhoHieuTruong:
        //                {
        //                    #region Phó Hiệu trưởng


        //                    foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
        //                    {
        //                        foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
        //                        {
        //                            ResultDetail rd = new ResultDetail();

        //                            rd.Id = p.Id;
        //                            rd.Note = p.Note;
        //                            rd.SupervisorNote = p.SupervisorNote;
        //                            rd.PreviousResult = p.PreviousResult;
        //                            rd.CurrentResult = p.CurrentResult;
        //                            if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
        //                            {
        //                                rd.RecordSecond = p.Record;
        //                                rd.Record = p.RecordOld;
        //                            }
        //                            else
        //                            {
        //                                rd.Record = p.Record;
        //                            }
        //                            rd.RegisterTarget = p.RegisterTarget;
        //                            rd.SupervisorRecord = p.SupervisorRecord;
        //                            rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
        //                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
        //                            rd.FileAttachments = new List<FileAttachment>();
        //                            if (p.FileAttachments != null)
        //                            {
        //                                foreach (FileAttachmentDTO fad in p.FileAttachments)
        //                                {
        //                                    FileAttachment fa = new FileAttachment();
        //                                    fa.Id = fad.Id;
        //                                    fa.CreationTime = fad.CreationTime;
        //                                    fa.Extension = fad.Extension;
        //                                    fa.Name = fad.Name;
        //                                    fa.Path = fad.Path;
        //                                    fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
        //                                    rd.FileAttachments.Add(fa);
        //                                }
        //                            }
        //                            rd.Result = ratingResult;
        //                            if (p.Id == Guid.Empty)
        //                            {
        //                                rd.Id = Guid.NewGuid();
        //                                session.Save(rd);
        //                            }
        //                            else
        //                            {
        //                                session.Merge(rd);
        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                break;
        //        }
        //        if (obj.IsSupervisor)
        //        {
        //            foreach (ResultDetailRatingDTO p in obj.BonusRecordList)
        //            {
        //                ResultDetail rd = new ResultDetail();

        //                rd.Id = p.Id;
        //                rd.Note = p.Note;
        //                rd.SupervisorNote = p.SupervisorNote;
        //                rd.Record = p.Record;
        //                rd.CurrentResult = p.CurrentResult;
        //                rd.FileAttachments = new List<FileAttachment>();
        //                if (p.FileAttachments != null)
        //                {
        //                    foreach (FileAttachmentDTO fad in p.FileAttachments)
        //                    {
        //                        FileAttachment fa = new FileAttachment();
        //                        fa.Id = fad.Id;
        //                        fa.CreationTime = fad.CreationTime;
        //                        fa.Extension = fad.Extension;
        //                        fa.Name = fad.Name;
        //                        fa.Path = fad.Path;
        //                        fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
        //                        rd.FileAttachments.Add(fa);
        //                    }
        //                }

        //                rd.Result = ratingResult;
        //                if (p.Id == Guid.Empty)
        //                {
        //                    rd.Id = Guid.NewGuid();
        //                    session.Save(rd);
        //                }
        //                else
        //                {
        //                    session.Merge(rd);
        //                }
        //            }
        //        }


        //    });

        //    return obj;
        //}

        //[Authorize]
        //[Route("")]
        //public RatingKPIResultDTO PutLock(RatingKPIResultDTO obj)
        //{
        //    SessionManager.DoWork(session =>
        //    {
        //        RatingKPIResultDTO result = Put(obj);
        //        Result ratingResult = session.Query<Result>().SingleOrDefault(r => r.Id == obj.RatingResultId);
        //        ratingResult.IsCommitted = true;
        //        session.Update(ratingResult);
        //    });
        //    return obj;
        //}
        //public RatingManage ParseRatingManage(Guid DepartmentId, Guid PlanKPIId, Guid StaffId, DateTime RatingStartTime, DateTime RatingEndTime)
        //{
        //    RatingManage rm = new RatingManage();
        //    rm.Id = Guid.NewGuid();
        //    rm.Department = new Department() { Id = DepartmentId };
        //    rm.PlanKPI = new PlanKPI() { Id = PlanKPIId };
        //    rm.Staff = new Staff() { Id = StaffId };
        //    rm.RatingStartTime = RatingStartTime.ToLocalTime();
        //    rm.RatingEndTime = RatingEndTime.ToLocalTime();
        //    return rm;
        //}
        //public int PutUnlockRating(UnlockRatingDTO obj)
        //{
        //    int result = 0;
        //    try
        //    {
        //        SessionManager.DoWork(session =>
        //        {
        //            //Kiểm tra ngày tháng null
        //            if (obj.RatingStartTime.Year == 1 || obj.RatingEndTime.Year == 1)
        //            {
        //                result = 2;
        //            }
        //            else
        //            {
        //                //Tất cả người trong đơn vị
        //                if (obj.IsDepartment == true)
        //                {
        //                    Department dept = session.Query<Department>().Where(d => d.Id == obj.DepartmentId).SingleOrDefault();
        //                    if (dept != null)
        //                    {
        //                        switch (dept.DepartmentType)
        //                        {
        //                            //Phòng, Khoa
        //                            case 1:
        //                            case 4:
        //                                {
        //                                    IEnumerable<Guid> staffIds = session.Query<Staff>().Where(s => s.Department.Id == dept.Id && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null).ToList().Select(s => s.Id);
        //                                    foreach (Guid s in staffIds)
        //                                    {
        //                                        //Xóa những dòng đã tồn tại
        //                                        string deleteStaff = string.Format("DELETE FROM {0} WHERE StaffId='{1}'", "KPI_RatingManage", s);
        //                                        session.CreateSQLQuery(deleteStaff).ExecuteUpdate();

        //                                        RatingManage rm = ParseRatingManage(dept.Id, obj.PlanKPIId, s, obj.RatingStartTime, obj.RatingEndTime);
        //                                        session.Save(rm);
        //                                    }
        //                                }
        //                                break;
        //                            //Bộ môn
        //                            case 3:
        //                                {
        //                                    IEnumerable<Guid> staffIds = session.Query<Staff>().Where(s => s.StaffInfo.Subject.Id == dept.Id && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null).ToList().Select(s => s.Id);
        //                                    foreach (Guid s in staffIds)
        //                                    {
        //                                        //Xóa những dòng đã tồn tại
        //                                        string deleteStaff = string.Format("DELETE FROM {0} WHERE StaffId='{1}'", "KPI_RatingManage", s);
        //                                        session.CreateSQLQuery(deleteStaff).ExecuteUpdate();

        //                                        RatingManage rm = ParseRatingManage(dept.Id, obj.PlanKPIId, s, obj.RatingStartTime, obj.RatingEndTime);
        //                                        session.Save(rm);
        //                                    }
        //                                }
        //                                break;

        //                        }
        //                    }
        //                }

        //                //Những người được chọn
        //                else
        //                {
        //                    foreach (Guid s in obj.StaffIds)
        //                    {
        //                        //Xóa những dòng đã tồn tại
        //                        string deleteStaff = string.Format("DELETE FROM {0} WHERE StaffId='{1}'", "KPI_RatingManage", s);
        //                        session.CreateSQLQuery(deleteStaff).ExecuteUpdate();

        //                        RatingManage rm = ParseRatingManage(obj.DepartmentId, obj.PlanKPIId, s, obj.RatingStartTime, obj.RatingEndTime);
        //                        session.Save(rm);
        //                    }
        //                }
        //                result = 1;
        //            }
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        result = 0;
        //    }
        //    return result;
        //}

        //[Authorize]
        //[Route("")]
        //public bool GetCheckUnlockable(Guid planId)
        //{
        //    bool result = false;
        //    try
        //    {
        //        SessionManager.DoWork(session =>
        //        {
        //            Staff staff = ControllerHelpers.GetCurrentStaff(session);
        //            DateTime currentDate = DateTime.Now;
        //            if (staff != null)
        //            {
        //                result = session.Query<RatingManage>().Any(r => r.PlanKPI.Id == planId && r.Staff.Id == staff.Id && r.RatingStartTime <= currentDate && r.RatingEndTime >= currentDate);
        //            }
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //    return result;
        //}

        //[Authorize]
        //[Route("")]
        //public Guid GetUnlock(Guid id)
        //{
        //    Guid result = Guid.Empty;
        //    SessionManager.DoWork(session =>
        //    {
        //        Result rd = session.Query<Result>().SingleOrDefault(a => a.Id == id);
        //        rd.Time = DateTime.Now;
        //        rd.IsLocked = false;
        //        rd.IsUnlocked = true;
        //        session.Update(rd);
        //    });
        //    return result;
        //}
        //public int PutEditRecord(ResultDTO obj)
        //{
        //    int result = 0;
        //    SessionManager.DoWork(session =>
        //    {
        //        Result rd = session.Query<Result>().SingleOrDefault(a => a.Id == obj.Id);
        //        if (rd.IsUnlocked == false)
        //        {
        //            result = 2;
        //        }
        //        else
        //        {
        //            rd.Time = DateTime.Now;
        //            rd.TotalRecordSecond = obj.TotalRecordSecondNumber;
        //            session.Update(rd);
        //            result = 1;
        //        }
        //    });
        //    return result;
        //}

        //[Authorize]
        //[Route("")]
        //public ResultDetail Delete(ResultDetail obj)
        //{
        //    SessionManager.DoWork(session => session.Delete(obj));
        //    return obj;
        //    //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
        //}
    }
}
