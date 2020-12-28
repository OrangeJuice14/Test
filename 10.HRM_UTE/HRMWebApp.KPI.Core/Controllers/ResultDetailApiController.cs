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
        [Authorize]
        [Route("")]
        public IEnumerable<ResultDTO> GetListByStaffId(Guid id)
        {
            List<ResultDTO> result = new List<ResultDTO>();
            SessionManager.DoWork(session =>
            {
                if (id != Guid.Empty)
                {
                    List<PlanStaff> planStaffs = session.Query<PlanStaff>().Where(p => p.Staff.Id == id).ToList();
                    foreach (PlanStaff ps in planStaffs)
                    {
                        Result re = session.Query<Result>().FirstOrDefault(r => r.PlanStaff.Id == ps.Id);
                        if (re != null)
                        {
                            ResultDTO rd = new ResultDTO();
                            rd.Id = re.Id;
                            rd.PlanName = ps.PlanKPI.Name;
                            if (re.TotalRecord > 0)
                            {
                                rd.TotalRecord = re.TotalRecord.ToString();
                            }
                            else
                            {
                                rd.TotalRecord = "Chưa đánh giá";
                            }
                            if (re.TotalRecordSecond > 0)
                            {
                                rd.TotalRecordSecond = re.TotalRecordSecond.ToString();
                                rd.NumberOfEditing = re.NumberOfEditing.ToString();
                            }
                            else
                            {
                                rd.TotalRecordSecond = "Không có";
                                rd.NumberOfEditing = "Không có";
                            }

                            result.Add(rd);
                        }

                    }
                }

            });
            return result;
        }

        [Authorize]
        [Route("")]
        public Guid GetPlanIdByResultId(Guid id)
        {
            Guid result = Guid.Empty;
            SessionManager.DoWork(session =>
            {
                Result re = session.Query<Result>().Where(r => r.Id == id).FirstOrDefault();
                if (re != null)
                {
                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.Id == re.PlanStaff.Id).FirstOrDefault();
                    if (planStaff != null)
                    {
                        result = planStaff.PlanKPI.Id;
                    }
                }

            });
            return result;
        }

        [Authorize]
        [Route("")]
        public Guid GetStaffIdByResultId(Guid id)
        {
            Guid result = Guid.Empty;
            SessionManager.DoWork(session =>
            {
                Result re = session.Query<Result>().Where(r => r.Id == id).FirstOrDefault();
                if (re != null)
                {
                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.Id == re.PlanStaff.Id).FirstOrDefault();
                    if (planStaff != null)
                    {
                        result = planStaff.Staff.Id;
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public ResultDTO GetResult(Guid id)
        {
            ResultDTO result = new ResultDTO();
            SessionManager.DoWork(session =>
            {
                if (id != Guid.Empty)
                {
                    Result re = session.Query<Result>().FirstOrDefault(r => r.Id == id);
                    if (re != null)
                    {
                        result.Id = re.Id;
                        result.TotalRecordNumber = re.TotalRecord;
                        result.TotalRecordSecondNumber = re.TotalRecordSecond;
                    }
                }

            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ResultDetailDTO> GetList()
        {
            List<ResultDetailDTO> result = new List<ResultDetailDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ResultDetail>().ToList().Map<ResultDetailDTO>();
            });
            return result;
        }


        [Authorize]
        [Route("")]
        public IEnumerable<FileAttachmentDTO> GetFieldAttachmentByResultDetail(Guid id)
        {
            List<FileAttachmentDTO> result = new List<FileAttachmentDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ResultDetail>().FirstOrDefault(r => r.Id == id).FileAttachments.Map<FileAttachmentDTO>();
            });
            return result;
        }


        [Authorize]
        [Route("")]
        public RatingKPIResultDTO GetRatingResultDetail(Guid planId, Guid agentObjectId, Guid planStaffId, Guid supervisorId, Guid departmentId, byte isAdminRating, int isRate)
        {
            try
            {
                //isAdminRating: 0,1,2
                //0: ko phải admin đánh giá
                //1: admin đánh giá
                //2: quyền chỉ xem bảng đánh giá

                //planStaffId: mã nhân viên được xem
                RatingKPIResultDTO result = new RatingKPIResultDTO();
                //try
                //{
                Result ratingResult = new Result();

                if (supervisorId != Guid.Empty || AuthenticationHelper.IsAllowPlanMaking(new Guid(HttpContext.Current.User.Identity.GetUserId()), planStaffId != Guid.Empty ? planStaffId : new Guid(HttpContext.Current.User.Identity.GetUserId())))
                {
                    SessionManager.DoWorkVersion2(session =>
                 {
                     ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                     Guid deptId = applicationUser.DepartmentId != null ? new Guid(applicationUser.DepartmentId) : Guid.Empty;
                     Staff staff = new Staff();
                     var isSupervisor = false;
                     if (isAdminRating == 2)
                     {
                         result.IsViewer = true;
                     }
                     if (supervisorId == Guid.Empty && isAdminRating == 0 && planStaffId == Guid.Empty)
                     {
                         staff = ControllerHelpers.GetCurrentStaff(session);
                     }
                     else if ((supervisorId != Guid.Empty || isAdminRating == 1) && planStaffId != Guid.Empty)
                     {
                         staff = session.Query<Staff>().FirstOrDefault(s => s.Id == planStaffId);
                         result.IsSupervisor = true;
                         if (isAdminRating == 1)
                         {
                             result.IsAdminRating = true;
                         }
                         else
                         {
                             result.IsAdminRating = false;
                         }
                     }
                     else if (isAdminRating == 1 && departmentId != Guid.Empty)
                     {
                         Department temp = session.Query<Department>().Where(d => d.Id == departmentId).FirstOrDefault();
                         staff.Department = new Department() { Id = temp.Id };
                         switch (temp.DepartmentType)
                         {
                             case 1:
                                 {
                                     staff.StaffInfo = new StaffInfo() { Position = new Position() { AgentObjectType = new AgentObjectType() { Id = 3 } } };
                                 }
                                 break;
                             case 4:
                                 {
                                     staff.StaffInfo = new StaffInfo() { Position = new Position() { AgentObjectType = new AgentObjectType() { Id = 5 } } };
                                 }
                                 break;
                         }
                     }
                     Guid staffId = staff.Id;
                     PlanKPI plan = session.Query<PlanKPI>().Where(p => p.Id == planId).OrderByDescending(p => p.CreateTime).FirstOrDefault();
                     result.StartRatingTime = plan.RatingStartTime;
                     result.EndRatingTime = plan.RatingEndTime;

                     int AgentObjectTypeId = -1;
                     if (agentObjectId == Guid.Empty)
                     {
                         if (staff.StaffInfo.Position == null)
                         {
                             if (staff.StaffInfo.StaffType.ManageCode == "3")
                                 AgentObjectTypeId = 2; //Nhân viên
                             else
                                 AgentObjectTypeId = 1; //Giảng viên
                         }
                         else
                         {
                             AgentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
                         }
                     }
                     else
                     {
                         AgentObject ab = session.Query<AgentObject>().FirstOrDefault(a => a.Id == agentObjectId);
                         AgentObjectTypeId = ab.AgentObjectType.Id;
                         result.AgentObjectName = ab.Name;
                     }


                     //Chọn agentobjecttype cho đối tượng nếu như là giảng dạy
                     AgentObject ao = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).FirstOrDefault();
                     if (ao != null && ao.AgentObjectType.Id == 2)
                     {
                         StaffApiController controller = new StaffApiController();
                         result.StaffLeader = new StaffDTO();
                         if (staff.Id != Guid.Empty)
                             result.StaffLeader = controller.GetDepartmentLeader(staff.Department.Id);
                         else
                             //user kpi không có staff
                             result.StaffLeader = controller.GetDepartmentLeader(deptId);

                     }

                     if (staff.StaffInfo.AgentObjects.Count > 1)
                     {
                         AgentObject agentObj = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).FirstOrDefault();
                         AgentObjectTypeId = ao.AgentObjectType.Id;
                     }

                     PlanStaff planStaff = new PlanStaff();
                     if (AgentObjectTypeId == 1 || AgentObjectTypeId == 2)
                     {
                         planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
                         if (planStaff != null)
                             ratingResult = session.Query<Result>().FirstOrDefault(r => r.PlanStaff.Id == planStaff.Id && r.StaffRating.Id == staff.Id && r.RateId == isRate);
                     }
                     else if (AgentObjectTypeId == 3 || AgentObjectTypeId == 5)
                     {
                         if (staff.Id != Guid.Empty)
                             planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == departmentId && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
                         else
                             //user KPI ko co staff
                             planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == departmentId && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
                         if (planStaff != null)
                             ratingResult = session.Query<Result>().FirstOrDefault(r => r.PlanStaff.Id == planStaff.Id && r.RateId == isRate);
                     }
                     else if (AgentObjectTypeId == 6 || AgentObjectTypeId == 12)
                     {
                         planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == departmentId && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
                         if (planStaff != null)
                             ratingResult = session.Query<Result>().FirstOrDefault(r => r.PlanStaff.Id == planStaff.Id && r.RateId == isRate);
                     }
                     else if (AgentObjectTypeId == 7 || AgentObjectTypeId == 8 || AgentObjectTypeId == 10 || AgentObjectTypeId == 11)
                     {
                         planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == departmentId && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
                         if (planStaff != null)
                             ratingResult = session.Query<Result>().FirstOrDefault(r => r.PlanStaff.Id == planStaff.Id && r.RateId == isRate);
                     }

                     if (planStaff != null && ratingResult == null)
                     {
                         ratingResult = new Result();
                         ratingResult.Id = Guid.NewGuid();
                         ratingResult.PlanStaff = planStaff;
                         ratingResult.StaffRating = staff.Id != Guid.Empty ? staff : null;
                         ratingResult.Time = DateTime.Now;
                         ratingResult.IsLocked = false;
                         ratingResult.IsUnlocked = false;
                         ratingResult.IsCommitted = false;
                         ratingResult.IsUnlockedForRating = false;
                         ratingResult.TotalRecord = 0;
                         ratingResult.NumberOfEditing = 0;
                         ratingResult.RateId = isRate;

                         SessionManager.DoWork(session1 =>
                         {
                             session1.Save(ratingResult);
                         });
                     }

                     if (planStaff != null)
                     {
                         result.IsPlanLocked = planStaff.IsLocked;
                         if (result.IsSupervisor)
                         {
                             result.IsRated = ratingResult != null ? true : false;

                         }
                     }
                 });


                    SessionManager.DoWorkVersion2(session =>
                {

                    ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                    Guid deptId = applicationUser.DepartmentId != null ? new Guid(applicationUser.DepartmentId) : Guid.Empty;
                    Staff staff = new Staff();
                    if (supervisorId == Guid.Empty && isAdminRating == 0 && planStaffId == Guid.Empty)
                    {
                        staff = ControllerHelpers.GetCurrentStaff(session);
                    }
                    else if ((supervisorId != Guid.Empty || isAdminRating == 1) && planStaffId != Guid.Empty)
                    {
                        staff = session.Query<Staff>().FirstOrDefault(s => s.Id == planStaffId);
                    }
                    Guid staffId = staff.Id;
                    if (supervisorId == Guid.Empty && isAdminRating == 0)
                    {
                        result.IsSupervisor = false;
                    }
                    else if ((supervisorId != Guid.Empty || isAdminRating == 1) && departmentId == Guid.Empty)
                    {
                        staffId = planStaffId;
                        result.IsSupervisor = false;
                        if (supervisorId != Guid.Empty)
                        {
                            result.IsSupervisor = true;
                            Staff st = session.Query<Staff>().Where(s => s.Id == supervisorId).FirstOrDefault();
                            result.Supervisor = new StaffDTO();
                            result.Supervisor.Id = st != null ? st.Id : Guid.Empty;
                            result.Supervisor.Name = st != null ? st.StaffProfile.Name : "";
                        }
                    }
                    else if (isAdminRating == 1 && departmentId != Guid.Empty)
                    {
                        result.IsSupervisor = true;
                        Department temp = session.Query<Department>().Where(d => d.Id == departmentId).FirstOrDefault();
                        staff.Department = new Department() { Id = temp.Id };
                        switch (temp.DepartmentType)
                        {
                            case 1:
                                {
                                    staff.StaffInfo = new StaffInfo() { Position = new Position() { AgentObjectType = new AgentObjectType() { Id = 3 } } };
                                }
                                break;
                            case 4:
                                {
                                    staff.StaffInfo = new StaffInfo() { Position = new Position() { AgentObjectType = new AgentObjectType() { Id = 5 } } };
                                }
                                break;
                        }
                        result.IsAdminRating = true;
                    }

                    //staff = session.Query<Staff>().FirstOrDefault(s => s.Id == staffId);
                    PlanKPI plan = session.Query<PlanKPI>().Where(p => p.Id == planId).OrderByDescending(p => p.CreateTime).FirstOrDefault();
                    if (plan != null)
                    {
                        result.PlanTypeId = plan.PlanType.Id;
                        int AgentObjectTypeId = -1;
                        if (agentObjectId == Guid.Empty)
                        {
                            if (staff != null && staff.StaffInfo.Position == null)
                            {
                                if (staff.StaffInfo.StaffType.ManageCode == "3")
                                    AgentObjectTypeId = 2; //Nhân viên
                                else
                                    AgentObjectTypeId = 1; //Giảng viên
                            }
                            else
                            {
                                //Không có nhân viên cụ thể
                                AgentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
                            }
                        }
                        else
                        {
                            AgentObject ab = session.Query<AgentObject>().FirstOrDefault(a => a.Id == agentObjectId);
                            AgentObjectTypeId = ab.AgentObjectType.Id;
                        }
                        //Chọn agentobjecttype cho đối tượng nếu như là giảng dạy
                        AgentObject ao = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).FirstOrDefault();
                        if (ao != null && ao.AgentObjectType.Id == 2)
                        {
                            StaffApiController controller = new StaffApiController();
                            result.StaffLeader = new StaffDTO();
                            result.StaffLeader = controller.GetDepartmentLeader(staff.Department.Id);
                        }

                        if (staff.StaffInfo.AgentObjects.Count > 1)
                        {
                            AgentObject agentObj = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).FirstOrDefault();
                            AgentObjectTypeId = ao.AgentObjectType.Id;
                        }

                        PlanStaff planStaff = new PlanStaff();
                        if (AgentObjectTypeId == 1 || AgentObjectTypeId == 2)
                            planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
                        else if (AgentObjectTypeId == 3 || AgentObjectTypeId == 5)
                        {
                            //if (isAdminRating == 0)
                            //    planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == deptId && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
                            //else
                            //    planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.Department.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
                            planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == departmentId && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
                            //planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Staff.Id == staff.Id && ps.Department == null && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
                        }
                        else if (AgentObjectTypeId == 6 || AgentObjectTypeId == 12)
                        {
                            planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == departmentId && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
                        }
                        else if (AgentObjectTypeId == 7 || AgentObjectTypeId == 8 || AgentObjectTypeId == 11 || AgentObjectTypeId == 10)
                            planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == departmentId && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
                        if (planStaff != null)
                        {
                            ratingResult = session.Query<Result>().FirstOrDefault(r => r.PlanStaff.Id == planStaff.Id && r.RateId == isRate);


                            if (staff.Id != Guid.Empty) //User thường
                            {
                                result.StaffDTO = new StaffDTO();
                                result.StaffDTO.Id = staff.Id;
                                result.StaffDTO.Name = staff.StaffProfile.Name;
                                result.StaffDTO.Department = staff.Department.Map<DepartmentDTO>();
                                result.StaffDTO.DepartmentId = staff.Department.Id;
                                result.StaffDTO.Subject = staff.StaffInfo.Subject != null ? staff.StaffInfo.Subject.Map<DepartmentDTO>() : null;
                                result.StaffDTO.Position = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.Map<PositionDTO>() : null;
                                result.StaffDTO.UserName = staff.StaffInfo.WebUser.UserName;
                            }
                            else //User KPI
                            {
                                result.StaffDTO = new StaffDTO();
                                //result.StaffDTO.Department = staff.Department.Map<DepartmentDTO>();
                                result.StaffDTO.DepartmentId = staff.Department.Id;
                            }

                            if (AgentObjectTypeId == 6 || AgentObjectTypeId == 12)
                            {
                                if (departmentId != Guid.Empty)
                                {
                                    //Department temp = session.Query<Department>().Where(d => d.Id == departmentId).FirstOrDefault();

                                    //result.StaffDTO = new StaffDTO();
                                    //result.StaffDTO.Id = staff.Id;
                                    //result.StaffDTO.Name = staff.StaffProfile.Name;
                                    //result.StaffDTO.Department = temp.Map<DepartmentDTO>();
                                    //result.StaffDTO.DepartmentId = departmentId;
                                    //result.StaffDTO.Subject = temp.Map<DepartmentDTO>();
                                    //result.StaffDTO.Position = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.Map<PositionDTO>() : null;
                                    //result.StaffDTO.UserName = staff.StaffInfo.WebUser.UserName;    
                                }

                                if (staff.StaffInfo.SubPositions != null)
                                {
                                    result.StaffDTO.AgentObjectTypeId = AgentObjectTypeId;
                                    SubPosition sposition = staff.StaffInfo.SubPositions.Where(s => s.Position.AgentObjectType.Id == AgentObjectTypeId).Select(s => s).FirstOrDefault();
                                    if (sposition != null && sposition.Department != null)
                                    {
                                        //DepartmentId = sposition.Department.Id;
                                        result.StaffDTO.Subject = sposition.Department.Map<DepartmentDTO>();
                                        result.StaffDTO.Department = sposition.Department.ParentDepartment != null ? sposition.Department.ParentDepartment.Map<DepartmentDTO>() : null;
                                        result.StaffDTO.Position.Name = sposition.Position.Name;
                                    }
                                }
                            }

                            result.StaffId = staff.Id;

                            List<TargetGroupDetail> targets = session.Query<TargetGroupDetail>().Where(t => t.AgentObjects.Any(ag => ag.Id == agentObjectId && t.StudyYears.Any(y => plan.StudyYear1 != null ? y.Id == plan.StudyYear1.Id : false))).OrderBy(t => t.OrderNumber).ToList();
                            //if (AgentObjectTypeId == 5 || AgentObjectTypeId == 6 || AgentObjectTypeId == 12)
                            //    targets = session.Query<TargetGroupDetail>().Where(t => t.AgentObjects.Any(ag => ag.AgentObjectType.Id == 1)).OrderBy(t => t.OrderNumber).ToList();
                            result.TargetGroupRatingDTOs = new List<TargetGroupRatingDTO>();
                            List<ResultDetail> resultDetails = session.Query<ResultDetail>().Where(r => r.Result.Id == ratingResult.Id).ToList();
                            #region Original Result
                            double TotalOtherActivityHour = 0;
                            double TotalScienceResearchHour = 0;
                            foreach (TargetGroupDetail t in targets)
                            {
                                TargetGroupRatingDTO tg = new TargetGroupRatingDTO();
                                tg.TargetGroupDetailTypeId = t.TargetGroupDetailType.Id;
                                result.PlanStaffId = planStaff.Id;
                                result.StaffDTO.AgentObjectTypeId = AgentObjectTypeId;

                                switch (AgentObjectTypeId)
                                {
                                    case (int)AgentObjectTypes.GiangVien:
                                        {
                                            #region giảng viên
                                            List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();
                                            planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();

                                            if (t.TargetGroupDetailType.Id != 4)
                                            {
                                                foreach (PlanKPIDetail pld in planDetails)
                                                {
                                                    if (plan.StudyTerm == null)
                                                    {
                                                        plan.StudyTerm = "CaNam";
                                                    }
                                                    ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                    ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                    rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                    rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                    rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                    rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                    List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id).Map<CriterionDictionaryDTO>().ToList();
                                                    rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                    rd.PlanKPIDetailId = pld.Id;
                                                    rd.PlanKPIDetailName = pld.Name;
                                                    rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                    if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                    {
                                                        rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                    }
                                                    else
                                                        rd.IdDanhGiaChiTiet = 2;
                                                    rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };

                                                    if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                    {
                                                        if (t.TargetGroupDetailType.Id == 0 && pld.FromProfessorCriterion.DanhGiaChiTiet.Id == 1)
                                                        {

                                                            OtherActivityData dodulieudanhgia = new OtherActivityData();
                                                            dodulieudanhgia = session.Query<OtherActivityData>().Where(q => (q.StaffCode == staff.StaffInfo.ManageCode
                                                             || q.StaffCode == staff.StaffInfo.WebUser.UserName) && q.StudyTerm.Trim() == plan.StudyTerm.Trim() &&
                                                             q.StudyYear == plan.StudyYear && q.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).FirstOrDefault();

                                                            // CriterionDictionary listcrit = session.Query<CriterionDictionary>().Where(r => r.ManageCode == dodulieudanhgia).FirstOrDefault();

                                                            if (dodulieudanhgia != null)
                                                            {
                                                                rd.CurrentResult = dodulieudanhgia.CriterionDictionaryId.Name;
                                                                rd.SupervisorRecord = dodulieudanhgia.CriterionDictionaryId.Record;
                                                            }
                                                        }
                                                    }
                                                    result.DiemThuongDiemTru = session.Query<DiemThuongDiemTru>().Where(q => q.MaCanBo == staff.StaffInfo.ManageCode
                                                     || q.MaCanBo == staff.StaffInfo.WebUser.UserName && q.HocKy == plan.StudyTerm &&
                                                     q.NamHoc == plan.StudyYear && q.MaHoatDong == pld.FromProfessorCriterion.ManageCode).ToList();

                                                    switch (pld.FromProfessorCriterion.CriterionType.Id)
                                                    {
                                                        case 3:
                                                            {
                                                                if (pld.CurrentKPI != null)
                                                                {
                                                                    try
                                                                    {
                                                                        Guid crdId = new Guid(pld.CurrentKPI);
                                                                        CriterionDictionary crd = session.Query<CriterionDictionary>().Where(c => c.Id == crdId).FirstOrDefault();
                                                                        rd.RegisterTarget = crd.Name;
                                                                    }
                                                                    catch (Exception e)
                                                                    {

                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        default:
                                                            {
                                                                rd.RegisterTarget = pld.CurrentKPI;
                                                            }
                                                            break;
                                                    }
                                                    // phần này lúc trước gán cứng cho "ChatLuongGiangDay" và "GiangDaySoTietChuan" được import dữ liệu

                                                    //if (pld.FromProfessorCriterion.Id == new Guid(ConfigurationManager.AppSettings["ChatLuongGiangDay"]))
                                                    //{
                                                    //    try
                                                    //    {
                                                    //        ESurveyData edata = session.Query<ESurveyData>().Where(e => e.StaffId == staff.Id && e.StudyTerm == plan.StudyTerm && e.StudyYear == plan.StudyYear).FirstOrDefault();
                                                    //        if (edata != null)
                                                    //        {
                                                    //            double point = edata.RankingPoint;
                                                    //            CriterionDictionary crid = session.Query<CriterionDictionary>().Where(c => c.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id && c.DataRecord <= point && c.DataMaxRecord >= point).FirstOrDefault();
                                                    //            if (crid != null)
                                                    //            {
                                                    //                rd.CurrentResult = crid.Name + " (" + edata.RankingPoint + ")";
                                                    //            }
                                                    //        } 
                                                    //    }
                                                    //    catch
                                                    //    {

                                                    //    }
                                                    //}
                                                    //string GiangDaySoTietChuan = ConfigurationManager.AppSettings["GiangDaySoTietChuan"];
                                                    //if (pld.FromProfessorCriterion.Id.ToString().ToUpper() == GiangDaySoTietChuan)
                                                    //{
                                                    //    try
                                                    //    {
                                                    //        PMSData pdata = session.Query<PMSData>().Where(e => e.StaffId == staff.Id && e.StudyTerm == plan.StudyTerm && e.StudyYear == plan.StudyYear).FirstOrDefault();
                                                    //        if (pdata != null)
                                                    //        {
                                                    //            double percent = pdata.PercentShortage;
                                                    //            CriterionDictionary crid = session.Query<CriterionDictionary>().Where(c => c.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id && c.DataRecord <= percent && c.DataMaxRecord >= percent).FirstOrDefault();
                                                    //            if (crid != null)
                                                    //            {
                                                    //                rd.CurrentResult = crid.Name + " (" + pdata.NumberOfLesson + " tiết)";
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //    catch (Exception e)
                                                    //    {

                                                    //    }
                                                    //}

                                                    ////////////// ***********/////////////

                                                    // loại nghiên cứu khoa học
                                                    if (t.TargetGroupDetailType.Id == 5)
                                                    {
                                                        rd.ScienceResearches = new List<ScienceResearchDTO>();
                                                        foreach (ScienceResearch pa in pld.ScienceResearches)
                                                        {
                                                            ScienceResearchDTO pad = new ScienceResearchDTO();
                                                            pad.Id = pa.Id;
                                                            pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                                                            pad.NumberOfResearch = pa.NumberOfResearch;
                                                            pad.OrderNumber = pa.OrderNumber;
                                                            rd.ScienceResearches.Add(pad);
                                                        }
                                                        rd.ScienceResearches = rd.ScienceResearches.OrderBy(p => p.OrderNumber).ToList();
                                                        List<OtherActivityData> scienceResearch = new List<OtherActivityData>();
                                                        if (plan.PlanType.Id == 1)
                                                        {
                                                            scienceResearch = session.Query<OtherActivityData>().Where(s => (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                        && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                        }
                                                        else
                                                        {
                                                            scienceResearch = session.Query<OtherActivityData>().Where(s => (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                            && s.StudyTerm == plan.StudyTerm && s.StudyYear == plan.StudyYear && s.ManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                        }
                                                        foreach (OtherActivityData pa in scienceResearch)
                                                        {
                                                            ScienceResearchDataDTO pad = new ScienceResearchDataDTO();
                                                            pad.Name = pa.Name;
                                                            //  pad.NumberOfHour = tempcri != null ? tempcri.NumberOfHour : 0;
                                                            // pad.NumberOfTimeDouble = pa.NumberOfTime;
                                                            pad.ManageCode = pa.ManageCode;
                                                            rd.ActivityHour += pa.NumberOfTime;
                                                            pad.NumberOfTime = pa.NumberOfTime;
                                                            pad.Record = pa.CriterionDictionaryId.Record;
                                                            rd.SupervisorRecord += pa.CriterionDictionaryId.Record;
                                                            //  rd.SupervisorRecord = pa.CriterionDictionaryId.Record != 0? pa.CriterionDictionaryId.Record:0;
                                                            rd.ScienceResearchesResult.Add(pad);

                                                        }
                                                        TotalOtherActivityHour += rd.ActivityHour;
                                                    }
                                                    rd.Density = 1;
                                                    tg.ResultDetailDTOs.Add(rd);

                                                    if (rdetail != null)
                                                    {
                                                        //rd = rdetail.Map<ResultDetailRatingDTO>();
                                                        rd.PreviousResult = rdetail.PreviousResult;
                                                        //rd.CurrentResult = rdetail.CurrentResult;
                                                        // if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                        // {
                                                        //     if (pld.FromProfessorCriterion.Id.ToString().ToUpper() != GiangDaySoTietChuan &&
                                                        // pld.FromProfessorCriterion.Id.ToString().ToUpper() != ConfigurationManager.AppSettings["ChatLuongGiangDay"]
                                                        // && pld.FromProfessorCriterion.DanhGiaChiTiet.Id != 1)
                                                        //     {
                                                        //         rd.CurrentResult = rdetail.CurrentResult;
                                                        //     }
                                                        // }
                                                        // else
                                                        // {
                                                        //     if (pld.FromProfessorCriterion.Id.ToString().ToUpper() != GiangDaySoTietChuan &&
                                                        //pld.FromProfessorCriterion.Id.ToString().ToUpper() != ConfigurationManager.AppSettings["ChatLuongGiangDay"])
                                                        // {
                                                        //     rd.CurrentResult = rdetail.CurrentResult;
                                                        // }
                                                        // }

                                                        //rd.RegisterTarget = rdetail.RegisterTarget;
                                                        rd.Record = rdetail.Record;
                                                        // rd.SupervisorRecord = rdetail.SupervisorRecord;
                                                        rd.Id = rdetail.Id;
                                                        rd.Note = rdetail.Note;
                                                        rd.SupervisorNote = rdetail.SupervisorNote;
                                                        rd.FileAttachments = new List<FileAttachmentDTO>();
                                                        foreach (FileAttachment fad in rdetail.FileAttachments)
                                                        {
                                                            FileAttachmentDTO fa = new FileAttachmentDTO();
                                                            fa.Id = fad.Id;
                                                            fa.CreationTime = fad.CreationTime;
                                                            fa.Extension = fad.Extension;
                                                            fa.Name = fad.Name;
                                                            fa.Path = fad.Path;
                                                            fa.ResultDetailId = fad.ResultDetail.Id;
                                                            rd.FileAttachments.Add(fa);
                                                        }
                                                        rd.FileAttachmentCount = rd.FileAttachments.Count();
                                                        //rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
                                                    }
                                                    else
                                                    {
                                                        ResultDetail resultDetail = new ResultDetail();
                                                        resultDetail.Id = Guid.NewGuid();
                                                        rd.Id = resultDetail.Id;
                                                        resultDetail.CurrentResult = rd.CurrentResult;
                                                        resultDetail.PreviousResult = rd.PreviousResult;
                                                        if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                        {
                                                            resultDetail.RecordSecond = rd.Record;
                                                            resultDetail.Record = rd.RecordOld;
                                                        }
                                                        else
                                                        {
                                                            resultDetail.Record = rd.Record;
                                                        }
                                                        if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                        {
                                                            resultDetail.SupervisorRecord = rd.Record;
                                                        }
                                                        resultDetail.RegisterTarget = rd.RegisterTarget;
                                                        resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                        resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                        resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                        resultDetail.Result = ratingResult;
                                                        resultDetail.IsTargetGroupRating = false;

                                                        SessionManager.DoWork(session1 =>
                                                        {
                                                            session1.Save(resultDetail);
                                                        });
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                foreach (PlanKPIDetail pld in planDetails)
                                                {
                                                    ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                    ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                    List<OtherActivityData> hoatdongkhac = new List<OtherActivityData>();

                                                    if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                    {
                                                        rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                    }
                                                    else
                                                        rd.IdDanhGiaChiTiet = 2;

                                                    rd.ProfessorOtherActivities = new List<ProfessorOtherActivityDTO>();
                                                    rd.ActivityHour = 0;

                                                    //lúc trước đổ NCKH,HDK,Giảng dạy riêng nay gộp lại chung 1 form đổ

                                                    foreach (ProfessorOtherActivity pa in pld.ProfessorOtherActivities)
                                                    {
                                                        ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                                                        pad.Id = pa.Id;
                                                        pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                                                        pad.NumberOfTime = pa.NumberOfTime;
                                                        pad.OrderNumber = pa.OrderNumber;
                                                        rd.ProfessorOtherActivities.Add(pad);
                                                    }

                                                    rd.ProfessorOtherActivities = rd.ProfessorOtherActivities.ToList();

                                                    rd.ProfessorOtherActivitiesResult = new List<ProfessorOtherActivityDTO>();

                                                    List<OtherActivityData> otherActivities = new List<OtherActivityData>();
                                                    if (plan.PlanType.Id == 1) //KH năm thì lấy hoạt động của cả 2 học kỳ
                                                    {
                                                        otherActivities = session.Query<OtherActivityData>().Where(s =>
                                                        (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                        && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                    }
                                                    else
                                                    {
                                                        otherActivities = session.Query<OtherActivityData>().Where(s =>
                                                        (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName) && s.StudyTerm == plan.StudyTerm
                                                        && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                    }
                                                    foreach (OtherActivityData pa in otherActivities)
                                                    {
                                                        //try
                                                        //{
                                                        //  CriterionDictionary tempcri = session.Query<CriterionDictionary>().Where(c => c.ManageCode == pa.CriterionDictionaryId.ManageCode).FirstOrDefault();
                                                        ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                                                        pad.Name = pa.Name;
                                                        //  pad.NumberOfHour = tempcri != null ? tempcri.NumberOfHour : 0;
                                                        pad.NumberOfTimeDouble = pa.NumberOfTime;
                                                        pad.ManageCode = pa.ManageCode;
                                                        rd.ActivityHour += pa.NumberOfTime;
                                                        pad.SupervisorRecord = pa.CriterionDictionaryId.Record;
                                                        rd.SupervisorRecord += pa.CriterionDictionaryId.Record;
                                                        //  rd.SupervisorRecord = pa.CriterionDictionaryId.Record != 0? pa.CriterionDictionaryId.Record:0;
                                                        rd.ProfessorOtherActivitiesResult.Add(pad);
                                                        //}
                                                        //catch { }
                                                    }
                                                    TotalOtherActivityHour += rd.ActivityHour;
                                                    //int TotalHourDefault = session.Query<KPIConfiguration>().FirstOrDefault().TotalHourDefault;
                                                    //rd.SupervisorRecord = Math.Round((rd.ActivityHour / TotalHourDefault) * 100, 1);
                                                    rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                    rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                    rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                    rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                    rd.RegisterTarget = pld.CurrentKPI;
                                                    rd.CriterionDictionaries = pld.FromProfessorCriterion.CriterionDictionaries.Map<CriterionDictionaryDTO>();
                                                    rd.PlanKPIDetailId = pld.Id;
                                                    rd.PlanKPIDetailName = pld.Name;
                                                    rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                    rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };
                                                    tg.ResultDetailDTOs.Add(rd);
                                                    if (rdetail != null)
                                                    {
                                                        rd.PreviousResult = rdetail.PreviousResult;
                                                        rd.CurrentResult = rdetail.CurrentResult;

                                                        rd.RegisterTarget = rdetail.RegisterTarget;
                                                        if (rdetail.RecordSecond != 0 && ratingResult.IsUnlocked == true)
                                                        {
                                                            rd.Record = rdetail.RecordSecond;
                                                            rd.RecordOld = rdetail.Record;
                                                        }
                                                        else
                                                        {
                                                            rd.Record = rdetail.Record;
                                                            rd.RecordOld = rdetail.Record;
                                                        }
                                                        // rd.SupervisorRecord = rd.SupervisorRecord > 0 ? rd.SupervisorRecord : rdetail.SupervisorRecord;
                                                        rd.Id = rdetail.Id;
                                                    }
                                                    else
                                                    {
                                                        ResultDetail resultDetail = new ResultDetail();
                                                        resultDetail.Id = Guid.NewGuid();
                                                        rd.Id = resultDetail.Id;
                                                        resultDetail.CurrentResult = rd.CurrentResult;
                                                        resultDetail.PreviousResult = rd.PreviousResult;
                                                        if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                        {
                                                            resultDetail.RecordSecond = rd.Record;
                                                            resultDetail.Record = rd.RecordOld;
                                                        }
                                                        else
                                                        {
                                                            resultDetail.Record = rd.Record;
                                                        }
                                                        if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                        {
                                                            resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                        }
                                                        resultDetail.RegisterTarget = rd.RegisterTarget;
                                                        resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                        resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                        resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                        resultDetail.Result = ratingResult;
                                                        resultDetail.IsTargetGroupRating = false;

                                                        SessionManager.DoWork(session1 =>
                                                        {
                                                            session1.Save(resultDetail);
                                                        });
                                                    }


                                                }
                                                double totalDensity = tg.ResultDetailDTOs.Sum(tr => tr.MaxRecord);
                                                tg.ResultDetailDTOs.ForEach(r =>
                                                {
                                                    r.Density = Math.Round(r.MaxRecord / totalDensity, 2);
                                                });
                                            }
                                            #endregion
                                        }
                                        break;
                                    case (int)AgentObjectTypes.NhanVien:
                                        {
                                            #region nhân viên

                                            List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();
                                            switch (t.TargetGroupDetailType.Id)
                                            {
                                                #region case 0
                                                case 0:
                                                    {
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            if (plan.StudyTerm == null)
                                                            {
                                                                plan.StudyTerm = "CaNam";
                                                            }
                                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                            List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id).Map<CriterionDictionaryDTO>().ToList();
                                                            rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                            rd.PlanKPIDetailId = pld.Id;
                                                            rd.PlanKPIDetailName = pld.Name;
                                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                            }
                                                            else
                                                                rd.IdDanhGiaChiTiet = 2;
                                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };

                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                if (t.TargetGroupDetailType.Id == 0 && pld.FromProfessorCriterion.DanhGiaChiTiet.Id == 1)
                                                                {

                                                                    OtherActivityData dodulieudanhgia = new OtherActivityData();
                                                                    dodulieudanhgia = session.Query<OtherActivityData>().Where(q => (q.StaffCode == staff.StaffInfo.ManageCode
                                                                     || q.StaffCode == staff.StaffInfo.WebUser.UserName) && q.StudyTerm.Trim() == plan.StudyTerm.Trim() &&
                                                                     q.StudyYear == plan.StudyYear && q.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).FirstOrDefault();


                                                                    if (dodulieudanhgia != null)
                                                                    {
                                                                        rd.CurrentResult = dodulieudanhgia.CriterionDictionaryId.Name;
                                                                        rd.SupervisorRecord = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                        rd.Record = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                    }
                                                                }
                                                            }

                                                            switch (pld.FromProfessorCriterion.CriterionType.Id)
                                                            {
                                                                case 3:
                                                                    {
                                                                        if (pld.CurrentKPI != null)
                                                                        {
                                                                            try
                                                                            {
                                                                                Guid crdId = new Guid(pld.CurrentKPI);
                                                                                CriterionDictionary crd = session.Query<CriterionDictionary>().Where(c => c.Id == crdId).FirstOrDefault();
                                                                                rd.RegisterTarget = crd.Name;
                                                                            }
                                                                            catch (Exception e)
                                                                            {

                                                                            }
                                                                        }
                                                                    }
                                                                    break;
                                                                default:
                                                                    {
                                                                        rd.RegisterTarget = pld.CurrentKPI;
                                                                    }
                                                                    break;
                                                            }



                                                            //if (rdetail != null)
                                                            //{
                                                            //    rd.PreviousResult = rdetail.PreviousResult;
                                                            //    rd.Record = rdetail.Record;
                                                            //    // rd.SupervisorRecord = rdetail.SupervisorRecord;
                                                            //    rd.Id = rdetail.Id;
                                                            //    rd.Note = rdetail.Note;
                                                            //    rd.SupervisorNote = rdetail.SupervisorNote;
                                                            //    rd.FileAttachments = new List<FileAttachmentDTO>();
                                                            //    foreach (FileAttachment fad in rdetail.FileAttachments)
                                                            //    {
                                                            //        FileAttachmentDTO fa = new FileAttachmentDTO();
                                                            //        fa.Id = fad.Id;
                                                            //        fa.CreationTime = fad.CreationTime;
                                                            //        fa.Extension = fad.Extension;
                                                            //        fa.Name = fad.Name;
                                                            //        fa.Path = fad.Path;
                                                            //        fa.ResultDetailId = fad.ResultDetail.Id;
                                                            //        rd.FileAttachments.Add(fa);
                                                            //    }
                                                            //    rd.FileAttachmentCount = rd.FileAttachments.Count();
                                                            //    //rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
                                                            //}
                                                            //else
                                                            //{
                                                            //    ResultDetail resultDetail = new ResultDetail();
                                                            //    resultDetail.Id = Guid.NewGuid();
                                                            //    rd.Id = resultDetail.Id;
                                                            //    resultDetail.CurrentResult = rd.CurrentResult;
                                                            //    resultDetail.PreviousResult = rd.PreviousResult;
                                                            //    if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                            //    {
                                                            //        resultDetail.RecordSecond = rd.Record;
                                                            //        resultDetail.Record = rd.RecordOld;
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        resultDetail.Record = rd.Record;
                                                            //    }
                                                            //    if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                            //    {
                                                            //        resultDetail.SupervisorRecord = rd.Record;
                                                            //    }
                                                            //    resultDetail.RegisterTarget = rd.RegisterTarget;
                                                            //    resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                            //    resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                            //    resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                            //    resultDetail.Result = ratingResult;
                                                            //    resultDetail.IsTargetGroupRating = false;

                                                            //    SessionManager.DoWork(session1 =>
                                                            //    {
                                                            //        session1.Save(resultDetail);
                                                            //    });
                                                            //}
                                                            tg.ResultDetailDTOs.Add(rd);
                                                        }
                                                    }
                                                    break;
                                                #endregion
                                                #region case 5
                                                case 5:
                                                    {
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            if (plan.StudyTerm == null)
                                                            {
                                                                plan.StudyTerm = "CaNam";
                                                            }
                                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                            List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id).Map<CriterionDictionaryDTO>().ToList();
                                                            rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                            rd.PlanKPIDetailId = pld.Id;
                                                            rd.PlanKPIDetailName = pld.Name;
                                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                            }
                                                            else
                                                                rd.IdDanhGiaChiTiet = 2;
                                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };

                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                if (t.TargetGroupDetailType.Id == 0 && pld.FromProfessorCriterion.DanhGiaChiTiet.Id == 1)
                                                                {

                                                                    OtherActivityData dodulieudanhgia = new OtherActivityData();
                                                                    dodulieudanhgia = session.Query<OtherActivityData>().Where(q => (q.StaffCode == staff.StaffInfo.ManageCode
                                                                     || q.StaffCode == staff.StaffInfo.WebUser.UserName) && q.StudyTerm.Trim() == plan.StudyTerm.Trim() &&
                                                                     q.StudyYear == plan.StudyYear && q.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).FirstOrDefault();

                                                                    // CriterionDictionary listcrit = session.Query<CriterionDictionary>().Where(r => r.ManageCode == dodulieudanhgia).FirstOrDefault();

                                                                    if (dodulieudanhgia != null)
                                                                    {
                                                                        rd.CurrentResult = dodulieudanhgia.CriterionDictionaryId.Name;
                                                                        rd.SupervisorRecord = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                    }
                                                                }
                                                            }
                                                            rd.ScienceResearches = new List<ScienceResearchDTO>();
                                                            foreach (ScienceResearch pa in pld.ScienceResearches)
                                                            {
                                                                ScienceResearchDTO pad = new ScienceResearchDTO();
                                                                pad.Id = pa.Id;
                                                                pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                                                                pad.NumberOfResearch = pa.NumberOfResearch;
                                                                pad.OrderNumber = pa.OrderNumber;
                                                                rd.ScienceResearches.Add(pad);
                                                            }
                                                            rd.ScienceResearches = rd.ScienceResearches.OrderBy(p => p.OrderNumber).ToList();
                                                            List<OtherActivityData> scienceResearch = new List<OtherActivityData>();
                                                            if (plan.PlanType.Id == 1)
                                                            {
                                                                scienceResearch = session.Query<OtherActivityData>().Where(s => (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                            && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            else
                                                            {
                                                                scienceResearch = session.Query<OtherActivityData>().Where(s => (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                                && s.StudyTerm == plan.StudyTerm && s.StudyYear == plan.StudyYear && s.ManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            foreach (OtherActivityData pa in scienceResearch)
                                                            {
                                                                ScienceResearchDataDTO pad = new ScienceResearchDataDTO();
                                                                pad.Name = pa.Name;
                                                                //  pad.NumberOfHour = tempcri != null ? tempcri.NumberOfHour : 0;
                                                                // pad.NumberOfTimeDouble = pa.NumberOfTime;
                                                                pad.ManageCode = pa.ManageCode;
                                                                rd.ActivityHour += pa.NumberOfTime;
                                                                pad.NumberOfTime = pa.NumberOfTime;
                                                                pad.Record = pa.CriterionDictionaryId.Record;
                                                                rd.SupervisorRecord += pa.CriterionDictionaryId.Record;
                                                                rd.ScienceResearchesResult.Add(pad);

                                                            }
                                                            TotalOtherActivityHour += rd.ActivityHour;
                                                            rd.Density = 1;
                                                            tg.ResultDetailDTOs.Add(rd);
                                                            if (rdetail != null)
                                                            {
                                                                rd.PreviousResult = rdetail.PreviousResult;
                                                                rd.Record = rdetail.Record;
                                                                // rd.SupervisorRecord = rdetail.SupervisorRecord;
                                                                rd.Id = rdetail.Id;
                                                                rd.Note = rdetail.Note;
                                                                rd.SupervisorNote = rdetail.SupervisorNote;
                                                                rd.FileAttachments = new List<FileAttachmentDTO>();
                                                                foreach (FileAttachment fad in rdetail.FileAttachments)
                                                                {
                                                                    FileAttachmentDTO fa = new FileAttachmentDTO();
                                                                    fa.Id = fad.Id;
                                                                    fa.CreationTime = fad.CreationTime;
                                                                    fa.Extension = fad.Extension;
                                                                    fa.Name = fad.Name;
                                                                    fa.Path = fad.Path;
                                                                    fa.ResultDetailId = fad.ResultDetail.Id;
                                                                    rd.FileAttachments.Add(fa);
                                                                }
                                                                rd.FileAttachmentCount = rd.FileAttachments.Count();
                                                                //rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
                                                            }
                                                            else
                                                            {
                                                                ResultDetail resultDetail = new ResultDetail();
                                                                resultDetail.Id = Guid.NewGuid();
                                                                rd.Id = resultDetail.Id;
                                                                resultDetail.CurrentResult = rd.CurrentResult;
                                                                resultDetail.PreviousResult = rd.PreviousResult;
                                                                if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                                {
                                                                    resultDetail.RecordSecond = rd.Record;
                                                                    resultDetail.Record = rd.RecordOld;
                                                                }
                                                                else
                                                                {
                                                                    resultDetail.Record = rd.Record;
                                                                }
                                                                if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                                {
                                                                    resultDetail.SupervisorRecord = rd.Record;
                                                                }
                                                                resultDetail.RegisterTarget = rd.RegisterTarget;
                                                                resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                                resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                                resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                                resultDetail.Result = ratingResult;
                                                                resultDetail.IsTargetGroupRating = false;

                                                                SessionManager.DoWork(session1 =>
                                                                {
                                                                    session1.Save(resultDetail);
                                                                });
                                                            }
                                                        }
                                                    }
                                                    break;
                                                #endregion
                                                #region case 4
                                                case 4:
                                                    {
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                            List<OtherActivityData> hoatdongkhac = new List<OtherActivityData>();

                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                            }
                                                            else
                                                                rd.IdDanhGiaChiTiet = 2;

                                                            rd.ProfessorOtherActivities = new List<ProfessorOtherActivityDTO>();
                                                            rd.ActivityHour = 0;

                                                            //lúc trước đổ NCKH,HDK,Giảng dạy riêng nay gộp lại chung 1 form đổ

                                                            foreach (ProfessorOtherActivity pa in pld.ProfessorOtherActivities)
                                                            {
                                                                ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                                                                pad.Id = pa.Id;
                                                                pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                                                                pad.NumberOfTime = pa.NumberOfTime;
                                                                pad.OrderNumber = pa.OrderNumber;
                                                                rd.ProfessorOtherActivities.Add(pad);
                                                            }

                                                            rd.ProfessorOtherActivities = rd.ProfessorOtherActivities.ToList();

                                                            rd.ProfessorOtherActivitiesResult = new List<ProfessorOtherActivityDTO>();

                                                            List<OtherActivityData> otherActivities = new List<OtherActivityData>();
                                                            if (plan.PlanType.Id == 1) //KH năm thì lấy hoạt động của cả 2 học kỳ
                                                            {
                                                                otherActivities = session.Query<OtherActivityData>().Where(s =>
                                                                (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                                && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            else
                                                            {
                                                                otherActivities = session.Query<OtherActivityData>().Where(s =>
                                                                (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName) && s.StudyTerm == plan.StudyTerm
                                                                && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            foreach (OtherActivityData pa in otherActivities)
                                                            {
                                                                //try
                                                                //{
                                                                //  CriterionDictionary tempcri = session.Query<CriterionDictionary>().Where(c => c.ManageCode == pa.CriterionDictionaryId.ManageCode).FirstOrDefault();
                                                                ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                                                                pad.Name = pa.Name;
                                                                //  pad.NumberOfHour = tempcri != null ? tempcri.NumberOfHour : 0;
                                                                pad.NumberOfTimeDouble = pa.NumberOfTime;
                                                                pad.ManageCode = pa.ManageCode;
                                                                rd.ActivityHour += pa.NumberOfTime;
                                                                pad.SupervisorRecord = pa.CriterionDictionaryId.Record;
                                                                rd.SupervisorRecord += pa.CriterionDictionaryId.Record;
                                                                //  rd.SupervisorRecord = pa.CriterionDictionaryId.Record != 0? pa.CriterionDictionaryId.Record:0;
                                                                rd.ProfessorOtherActivitiesResult.Add(pad);
                                                                //}
                                                                //catch { }
                                                            }
                                                            TotalOtherActivityHour += rd.ActivityHour;
                                                            //int TotalHourDefault = session.Query<KPIConfiguration>().FirstOrDefault().TotalHourDefault;
                                                            //rd.SupervisorRecord = Math.Round((rd.ActivityHour / TotalHourDefault) * 100, 1);
                                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                            rd.RegisterTarget = pld.CurrentKPI;
                                                            rd.CriterionDictionaries = pld.FromProfessorCriterion.CriterionDictionaries.Map<CriterionDictionaryDTO>();
                                                            rd.PlanKPIDetailId = pld.Id;
                                                            rd.PlanKPIDetailName = pld.Name;
                                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };
                                                            tg.ResultDetailDTOs.Add(rd);
                                                            if (rdetail != null)
                                                            {
                                                                rd.PreviousResult = rdetail.PreviousResult;
                                                                rd.CurrentResult = rdetail.CurrentResult;

                                                                rd.RegisterTarget = rdetail.RegisterTarget;
                                                                if (rdetail.RecordSecond != 0 && ratingResult.IsUnlocked == true)
                                                                {
                                                                    rd.Record = rdetail.RecordSecond;
                                                                    rd.RecordOld = rdetail.Record;
                                                                }
                                                                else
                                                                {
                                                                    rd.Record = rdetail.Record;
                                                                    rd.RecordOld = rdetail.Record;
                                                                }
                                                                // rd.SupervisorRecord = rd.SupervisorRecord > 0 ? rd.SupervisorRecord : rdetail.SupervisorRecord;
                                                                rd.Id = rdetail.Id;
                                                            }
                                                            else
                                                            {
                                                                ResultDetail resultDetail = new ResultDetail();
                                                                resultDetail.Id = Guid.NewGuid();
                                                                rd.Id = resultDetail.Id;
                                                                resultDetail.CurrentResult = rd.CurrentResult;
                                                                resultDetail.PreviousResult = rd.PreviousResult;
                                                                if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                                {
                                                                    resultDetail.RecordSecond = rd.Record;
                                                                    resultDetail.Record = rd.RecordOld;
                                                                }
                                                                else
                                                                {
                                                                    resultDetail.Record = rd.Record;
                                                                }
                                                                if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                                {
                                                                    resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                                }
                                                                resultDetail.RegisterTarget = rd.RegisterTarget;
                                                                resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                                resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                                resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                                resultDetail.Result = ratingResult;
                                                                resultDetail.IsTargetGroupRating = false;

                                                                SessionManager.DoWork(session1 =>
                                                                {
                                                                    session1.Save(resultDetail);
                                                                });
                                                            }


                                                        }
                                                        double totalDensity = tg.ResultDetailDTOs.Sum(tr => tr.MaxRecord);
                                                        tg.ResultDetailDTOs.ForEach(r =>
                                                        {
                                                            r.Density = Math.Round(r.MaxRecord / totalDensity, 2);
                                                        });
                                                    }
                                                    break;
                                                #endregion
                                                #region case 1
                                                case 1:
                                                    {//Kế hoạch không có methods thì không lấy
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.Methods.Count > 0 && (p.TargetGroupDetail == null || p.TargetGroupDetail.Id == t.Id) && !p.IsDisable && !p.IsDelete).ToList();
                                                        if (t.ParentTargetGroupDetail != null)
                                                        {
                                                            tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).ToList().Map<CriterionDictionaryDTO>();
                                                        }
                                                        else
                                                        {
                                                            tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
                                                        }

                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == pld.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            if (isRate == 1)
                                                                Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            if (isRate == 2)
                                                                Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month == 12 && r.EndTime.Month <= 2).ToList();
                                                            if (isRate == 3)
                                                                Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            if (isRate == 4)
                                                                Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            if (isRate == 0)
                                                                Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                //rd.ResultName = crite.Name;
                                                                //rd.ManageCodeId = crite.FromPlanKPIDetail.ManageCode.Id;
                                                                //rd.ManageCodeName = crite.FromPlanKPIDetail.ManageCode.Name;
                                                                //rd.OrderNumber = crite.OrderNumber;
                                                                //rd.MaxRecord = crite.MaxRecord;

                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.Id = rdetail != null ? rdetail.Id : Guid.Empty;
                                                                    rd.ResultName = mt.Name; // tên mục tiêu
                                                                    rd.ManageCodeId = pld.ManageCode.Id;
                                                                    rd.Record = rdetail != null ? rdetail.Record : 0; // điểm tự đánh giá

                                                                    rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : 0; // điểm cấp trên đánh giá
                                                                    rd.PreviousResult = rdetail != null ? rdetail.PreviousResult : ""; // kết quả thực hiện
                                                                    rd.MaxRecord = rdetail != null ? rdetail.MaxRecord : 0;
                                                                    //rd.Density = rdetail.DensityResult;

                                                                }
                                                                rd.PlanKPIDetailId = pld.Id;
                                                                // rd.Density = rd.SupervisorRecord * rd.MaxRecord;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }

                                                    }
                                                    break;
                                                #endregion
                                                #region case 6
                                                case 6:
                                                    {
                                                        List<Criterion> CriterionDictionaries = new List<Criterion>();
                                                        Dictionary<Guid, Guid> planParentDetail = new Dictionary<Guid, Guid>();
                                                        CriterionDictionaries = session.Query<Criterion>().Where(c => (c.Department.Id == null && c.Staff.Id == staff.Id)).ToList();


                                                        //Lọc ra các criterion đúng nhóm tiêu chí                                       
                                                        CriterionDictionaries = CriterionDictionaries.Where(c => c.Staff != null && c.Staff.Id == staff.Id && c.TargetGroupDetail.TargetGroupDetailType.Id != 3 && c.FromPlanKPIDetail.PlanStaff.PlanKPI.Id == plan.Id
                                                        && c.FromPlanKPIDetail.IsDelete == false && c.FromPlanKPIDetail != null
                                                        && c.TargetGroupDetail.Id == t.Id).OrderBy(c => c.OrderNumber).ToList();

                                                        foreach (Criterion crite in CriterionDictionaries)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == crite.FromPlanKPIDetail.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(crite.FromPlanKPIDetail.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            if (isRate == 1)
                                                                Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            if (isRate == 2)
                                                                Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month == 12 && r.EndTime.Month <= 2).ToList();
                                                            if (isRate == 3)
                                                                Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            if (isRate == 4)
                                                                Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            if (isRate == 0)
                                                                Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.ResultName = mt.Name; // tên mục tiêu
                                                                    if (rdetail != null)
                                                                    {
                                                                        rd.Id = rdetail.Id;
                                                                        rd.Record = rdetail.Record; // điểm tự đánh giá
                                                                        rd.SupervisorRecord = rdetail.SupervisorRecord; // điểm cấp trên đánh giá
                                                                        rd.PreviousResult = rdetail.PreviousResult; // kết quả thực hiện
                                                                        rd.MaxRecord = rdetail.MaxRecord;
                                                                    }
                                                                    //else
                                                                    //{
                                                                    //    foreach (var de in mt.LeadDepartment)
                                                                    //    {
                                                                    //        if (de.DepartmentId.Id == departmentId)
                                                                    //        {
                                                                    //            rd.SupervisorRecord = de.DiemSo;
                                                                    //        }
                                                                    //    }
                                                                    //}

                                                                }
                                                                rd.PlanKPIDetailId = crite.FromPlanKPIDetail.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }
                                                    }
                                                    break;
                                                #endregion
                                                #region case 7
                                                case 7:
                                                    {
                                                        List<Criterion> CriterionDictionaries = new List<Criterion>();
                                                        Dictionary<Guid, Guid> planParentDetail = new Dictionary<Guid, Guid>();
                                                        CriterionDictionaries = session.Query<Criterion>().Where(c => (c.Department.Id == null && c.Staff.Id == staff.Id)).ToList();


                                                        //Lọc ra các criterion đúng nhóm tiêu chí                                       
                                                        CriterionDictionaries = CriterionDictionaries.Where(c => c.Staff != null && c.Staff.Id == staff.Id && c.TargetGroupDetail.TargetGroupDetailType.Id != 3 && c.FromPlanKPIDetail.PlanStaff.PlanKPI.Id == plan.Id
                                                        && c.FromPlanKPIDetail.IsDelete == false && c.FromPlanKPIDetail != null
                                                        && c.TargetGroupDetail.Id == t.Id).OrderBy(c => c.OrderNumber).ToList();

                                                        foreach (Criterion crite in CriterionDictionaries)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == crite.FromPlanKPIDetail.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(crite.FromPlanKPIDetail.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            if (isRate == 1)
                                                                Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            if (isRate == 2)
                                                                Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month == 12 && r.EndTime.Month <= 2).ToList();
                                                            if (isRate == 3)
                                                                Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            if (isRate == 4)
                                                                Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            if (isRate == 0)
                                                                Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.ResultName = mt.Name; // tên mục tiêu
                                                                    if (rdetail != null)
                                                                    {
                                                                        rd.Id = rdetail.Id;
                                                                        rd.Record = rdetail.Record; // điểm tự đánh giá
                                                                        rd.SupervisorRecord = rdetail.SupervisorRecord; // điểm cấp trên đánh giá
                                                                        rd.PreviousResult = rdetail.PreviousResult; // kết quả thực hiện
                                                                        rd.MaxRecord = rdetail.MaxRecord;
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (var de in mt.LeadDepartment)
                                                                        {
                                                                            if (de.DepartmentId.Id == departmentId)
                                                                            {
                                                                                rd.SupervisorRecord = de.DiemSo;
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                rd.PlanKPIDetailId = crite.FromPlanKPIDetail.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }
                                                    }
                                                    break;
                                                    #endregion
                                            }
                                            #endregion
                                        }
                                        break;
                                    case (int)AgentObjectTypes.PhongBan:
                                        {
                                            #region phòng ban

                                            List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();
                                            switch (t.TargetGroupDetailType.Id)
                                            {
                                                #region case 0
                                                case 0:
                                                    {
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            if (plan.StudyTerm == null)
                                                            {
                                                                plan.StudyTerm = "CaNam";
                                                            }
                                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                            List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id).Map<CriterionDictionaryDTO>().ToList();
                                                            rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                            rd.PlanKPIDetailId = pld.Id;
                                                            rd.PlanKPIDetailName = pld.Name;
                                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                            }
                                                            else
                                                                rd.IdDanhGiaChiTiet = 2;
                                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };

                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                if (t.TargetGroupDetailType.Id == 0 && pld.FromProfessorCriterion.DanhGiaChiTiet.Id == 1)
                                                                {

                                                                    OtherActivityData dodulieudanhgia = new OtherActivityData();
                                                                    dodulieudanhgia = session.Query<OtherActivityData>().Where(q => (q.StaffCode == staff.StaffInfo.ManageCode
                                                                     || q.StaffCode == staff.StaffInfo.WebUser.UserName) && q.StudyTerm.Trim() == plan.StudyTerm.Trim() &&
                                                                     q.StudyYear == plan.StudyYear && q.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).FirstOrDefault();


                                                                    if (dodulieudanhgia != null)
                                                                    {
                                                                        rd.CurrentResult = dodulieudanhgia.CriterionDictionaryId.Name;
                                                                        rd.SupervisorRecord = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                        rd.Record = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                    }
                                                                }
                                                            }

                                                            switch (pld.FromProfessorCriterion.CriterionType.Id)
                                                            {
                                                                case 3:
                                                                    {
                                                                        if (pld.CurrentKPI != null)
                                                                        {
                                                                            try
                                                                            {
                                                                                Guid crdId = new Guid(pld.CurrentKPI);
                                                                                CriterionDictionary crd = session.Query<CriterionDictionary>().Where(c => c.Id == crdId).FirstOrDefault();
                                                                                rd.RegisterTarget = crd.Name;
                                                                            }
                                                                            catch (Exception e)
                                                                            {

                                                                            }
                                                                        }
                                                                    }
                                                                    break;
                                                                default:
                                                                    {
                                                                        rd.RegisterTarget = pld.CurrentKPI;
                                                                    }
                                                                    break;
                                                            }



                                                            //if (rdetail != null)
                                                            //{
                                                            //    rd.PreviousResult = rdetail.PreviousResult;
                                                            //    rd.Record = rdetail.Record;
                                                            //    // rd.SupervisorRecord = rdetail.SupervisorRecord;
                                                            //    rd.Id = rdetail.Id;
                                                            //    rd.Note = rdetail.Note;
                                                            //    rd.SupervisorNote = rdetail.SupervisorNote;
                                                            //    rd.FileAttachments = new List<FileAttachmentDTO>();
                                                            //    foreach (FileAttachment fad in rdetail.FileAttachments)
                                                            //    {
                                                            //        FileAttachmentDTO fa = new FileAttachmentDTO();
                                                            //        fa.Id = fad.Id;
                                                            //        fa.CreationTime = fad.CreationTime;
                                                            //        fa.Extension = fad.Extension;
                                                            //        fa.Name = fad.Name;
                                                            //        fa.Path = fad.Path;
                                                            //        fa.ResultDetailId = fad.ResultDetail.Id;
                                                            //        rd.FileAttachments.Add(fa);
                                                            //    }
                                                            //    rd.FileAttachmentCount = rd.FileAttachments.Count();
                                                            //    //rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
                                                            //}
                                                            //else
                                                            //{
                                                            //    ResultDetail resultDetail = new ResultDetail();
                                                            //    resultDetail.Id = Guid.NewGuid();
                                                            //    rd.Id = resultDetail.Id;
                                                            //    resultDetail.CurrentResult = rd.CurrentResult;
                                                            //    resultDetail.PreviousResult = rd.PreviousResult;
                                                            //    if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                            //    {
                                                            //        resultDetail.RecordSecond = rd.Record;
                                                            //        resultDetail.Record = rd.RecordOld;
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        resultDetail.Record = rd.Record;
                                                            //    }
                                                            //    if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                            //    {
                                                            //        resultDetail.SupervisorRecord = rd.Record;
                                                            //    }
                                                            //    resultDetail.RegisterTarget = rd.RegisterTarget;
                                                            //    resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                            //    resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                            //    resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                            //    resultDetail.Result = ratingResult;
                                                            //    resultDetail.IsTargetGroupRating = false;

                                                            //    SessionManager.DoWork(session1 =>
                                                            //    {
                                                            //        session1.Save(resultDetail);
                                                            //    });
                                                            //}
                                                            tg.ResultDetailDTOs.Add(rd);
                                                        }
                                                    }
                                                    break;
                                                #endregion
                                                #region case 5
                                                case 5:
                                                    {
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            if (plan.StudyTerm == null)
                                                            {
                                                                plan.StudyTerm = "CaNam";
                                                            }
                                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                            List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id).Map<CriterionDictionaryDTO>().ToList();
                                                            rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                            rd.PlanKPIDetailId = pld.Id;
                                                            rd.PlanKPIDetailName = pld.Name;
                                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                            }
                                                            else
                                                                rd.IdDanhGiaChiTiet = 2;
                                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };

                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                if (t.TargetGroupDetailType.Id == 0 && pld.FromProfessorCriterion.DanhGiaChiTiet.Id == 1)
                                                                {

                                                                    OtherActivityData dodulieudanhgia = new OtherActivityData();
                                                                    dodulieudanhgia = session.Query<OtherActivityData>().Where(q => (q.StaffCode == staff.StaffInfo.ManageCode
                                                                     || q.StaffCode == staff.StaffInfo.WebUser.UserName) && q.StudyTerm.Trim() == plan.StudyTerm.Trim() &&
                                                                     q.StudyYear == plan.StudyYear && q.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).FirstOrDefault();

                                                                    // CriterionDictionary listcrit = session.Query<CriterionDictionary>().Where(r => r.ManageCode == dodulieudanhgia).FirstOrDefault();

                                                                    if (dodulieudanhgia != null)
                                                                    {
                                                                        rd.CurrentResult = dodulieudanhgia.CriterionDictionaryId.Name;
                                                                        rd.SupervisorRecord = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                    }
                                                                }
                                                            }
                                                            rd.ScienceResearches = new List<ScienceResearchDTO>();
                                                            foreach (ScienceResearch pa in pld.ScienceResearches)
                                                            {
                                                                ScienceResearchDTO pad = new ScienceResearchDTO();
                                                                pad.Id = pa.Id;
                                                                pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                                                                pad.NumberOfResearch = pa.NumberOfResearch;
                                                                pad.OrderNumber = pa.OrderNumber;
                                                                rd.ScienceResearches.Add(pad);
                                                            }
                                                            rd.ScienceResearches = rd.ScienceResearches.OrderBy(p => p.OrderNumber).ToList();
                                                            List<OtherActivityData> scienceResearch = new List<OtherActivityData>();
                                                            if (plan.PlanType.Id == 1)
                                                            {
                                                                scienceResearch = session.Query<OtherActivityData>().Where(s => (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                            && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            else
                                                            {
                                                                scienceResearch = session.Query<OtherActivityData>().Where(s => (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                                && s.StudyTerm == plan.StudyTerm && s.StudyYear == plan.StudyYear && s.ManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            foreach (OtherActivityData pa in scienceResearch)
                                                            {
                                                                ScienceResearchDataDTO pad = new ScienceResearchDataDTO();
                                                                pad.Name = pa.Name;
                                                                //  pad.NumberOfHour = tempcri != null ? tempcri.NumberOfHour : 0;
                                                                // pad.NumberOfTimeDouble = pa.NumberOfTime;
                                                                pad.ManageCode = pa.ManageCode;
                                                                rd.ActivityHour += pa.NumberOfTime;
                                                                pad.NumberOfTime = pa.NumberOfTime;
                                                                pad.Record = pa.CriterionDictionaryId.Record;
                                                                rd.SupervisorRecord += pa.CriterionDictionaryId.Record;
                                                                rd.ScienceResearchesResult.Add(pad);

                                                            }
                                                            TotalOtherActivityHour += rd.ActivityHour;
                                                            rd.Density = 1;
                                                            tg.ResultDetailDTOs.Add(rd);
                                                            if (rdetail != null)
                                                            {
                                                                rd.PreviousResult = rdetail.PreviousResult;
                                                                rd.Record = rdetail.Record;
                                                                // rd.SupervisorRecord = rdetail.SupervisorRecord;
                                                                rd.Id = rdetail.Id;
                                                                rd.Note = rdetail.Note;
                                                                rd.SupervisorNote = rdetail.SupervisorNote;
                                                                rd.FileAttachments = new List<FileAttachmentDTO>();
                                                                foreach (FileAttachment fad in rdetail.FileAttachments)
                                                                {
                                                                    FileAttachmentDTO fa = new FileAttachmentDTO();
                                                                    fa.Id = fad.Id;
                                                                    fa.CreationTime = fad.CreationTime;
                                                                    fa.Extension = fad.Extension;
                                                                    fa.Name = fad.Name;
                                                                    fa.Path = fad.Path;
                                                                    fa.ResultDetailId = fad.ResultDetail.Id;
                                                                    rd.FileAttachments.Add(fa);
                                                                }
                                                                rd.FileAttachmentCount = rd.FileAttachments.Count();
                                                                //rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
                                                            }
                                                            else
                                                            {
                                                                ResultDetail resultDetail = new ResultDetail();
                                                                resultDetail.Id = Guid.NewGuid();
                                                                rd.Id = resultDetail.Id;
                                                                resultDetail.CurrentResult = rd.CurrentResult;
                                                                resultDetail.PreviousResult = rd.PreviousResult;
                                                                if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                                {
                                                                    resultDetail.RecordSecond = rd.Record;
                                                                    resultDetail.Record = rd.RecordOld;
                                                                }
                                                                else
                                                                {
                                                                    resultDetail.Record = rd.Record;
                                                                }
                                                                if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                                {
                                                                    resultDetail.SupervisorRecord = rd.Record;
                                                                }
                                                                resultDetail.RegisterTarget = rd.RegisterTarget;
                                                                resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                                resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                                resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                                resultDetail.Result = ratingResult;
                                                                resultDetail.IsTargetGroupRating = false;

                                                                SessionManager.DoWork(session1 =>
                                                                {
                                                                    session1.Save(resultDetail);
                                                                });
                                                            }
                                                        }
                                                    }
                                                    break;
                                                #endregion
                                                #region case 4
                                                case 4:
                                                    {
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                            List<OtherActivityData> hoatdongkhac = new List<OtherActivityData>();

                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                            }
                                                            else
                                                                rd.IdDanhGiaChiTiet = 2;

                                                            rd.ProfessorOtherActivities = new List<ProfessorOtherActivityDTO>();
                                                            rd.ActivityHour = 0;

                                                            //lúc trước đổ NCKH,HDK,Giảng dạy riêng nay gộp lại chung 1 form đổ

                                                            foreach (ProfessorOtherActivity pa in pld.ProfessorOtherActivities)
                                                            {
                                                                ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                                                                pad.Id = pa.Id;
                                                                pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                                                                pad.NumberOfTime = pa.NumberOfTime;
                                                                pad.OrderNumber = pa.OrderNumber;
                                                                rd.ProfessorOtherActivities.Add(pad);
                                                            }

                                                            rd.ProfessorOtherActivities = rd.ProfessorOtherActivities.ToList();

                                                            rd.ProfessorOtherActivitiesResult = new List<ProfessorOtherActivityDTO>();

                                                            List<OtherActivityData> otherActivities = new List<OtherActivityData>();
                                                            if (plan.PlanType.Id == 1) //KH năm thì lấy hoạt động của cả 2 học kỳ
                                                            {
                                                                otherActivities = session.Query<OtherActivityData>().Where(s =>
                                                                (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                                && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            else
                                                            {
                                                                otherActivities = session.Query<OtherActivityData>().Where(s =>
                                                                (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName) && s.StudyTerm == plan.StudyTerm
                                                                && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            foreach (OtherActivityData pa in otherActivities)
                                                            {
                                                                //try
                                                                //{
                                                                //  CriterionDictionary tempcri = session.Query<CriterionDictionary>().Where(c => c.ManageCode == pa.CriterionDictionaryId.ManageCode).FirstOrDefault();
                                                                ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                                                                pad.Name = pa.Name;
                                                                //  pad.NumberOfHour = tempcri != null ? tempcri.NumberOfHour : 0;
                                                                pad.NumberOfTimeDouble = pa.NumberOfTime;
                                                                pad.ManageCode = pa.ManageCode;
                                                                rd.ActivityHour += pa.NumberOfTime;
                                                                pad.SupervisorRecord = pa.CriterionDictionaryId.Record;
                                                                rd.SupervisorRecord += pa.CriterionDictionaryId.Record;
                                                                //  rd.SupervisorRecord = pa.CriterionDictionaryId.Record != 0? pa.CriterionDictionaryId.Record:0;
                                                                rd.ProfessorOtherActivitiesResult.Add(pad);
                                                                //}
                                                                //catch { }
                                                            }
                                                            TotalOtherActivityHour += rd.ActivityHour;
                                                            //int TotalHourDefault = session.Query<KPIConfiguration>().FirstOrDefault().TotalHourDefault;
                                                            //rd.SupervisorRecord = Math.Round((rd.ActivityHour / TotalHourDefault) * 100, 1);
                                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                            rd.RegisterTarget = pld.CurrentKPI;
                                                            rd.CriterionDictionaries = pld.FromProfessorCriterion.CriterionDictionaries.Map<CriterionDictionaryDTO>();
                                                            rd.PlanKPIDetailId = pld.Id;
                                                            rd.PlanKPIDetailName = pld.Name;
                                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };
                                                            tg.ResultDetailDTOs.Add(rd);
                                                            if (rdetail != null)
                                                            {
                                                                rd.PreviousResult = rdetail.PreviousResult;
                                                                rd.CurrentResult = rdetail.CurrentResult;

                                                                rd.RegisterTarget = rdetail.RegisterTarget;
                                                                if (rdetail.RecordSecond != 0 && ratingResult.IsUnlocked == true)
                                                                {
                                                                    rd.Record = rdetail.RecordSecond;
                                                                    rd.RecordOld = rdetail.Record;
                                                                }
                                                                else
                                                                {
                                                                    rd.Record = rdetail.Record;
                                                                    rd.RecordOld = rdetail.Record;
                                                                }
                                                                // rd.SupervisorRecord = rd.SupervisorRecord > 0 ? rd.SupervisorRecord : rdetail.SupervisorRecord;
                                                                rd.Id = rdetail.Id;
                                                            }
                                                            else
                                                            {
                                                                ResultDetail resultDetail = new ResultDetail();
                                                                resultDetail.Id = Guid.NewGuid();
                                                                rd.Id = resultDetail.Id;
                                                                resultDetail.CurrentResult = rd.CurrentResult;
                                                                resultDetail.PreviousResult = rd.PreviousResult;
                                                                if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                                {
                                                                    resultDetail.RecordSecond = rd.Record;
                                                                    resultDetail.Record = rd.RecordOld;
                                                                }
                                                                else
                                                                {
                                                                    resultDetail.Record = rd.Record;
                                                                }
                                                                if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                                {
                                                                    resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                                }
                                                                resultDetail.RegisterTarget = rd.RegisterTarget;
                                                                resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                                resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                                resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                                resultDetail.Result = ratingResult;
                                                                resultDetail.IsTargetGroupRating = false;

                                                                SessionManager.DoWork(session1 =>
                                                                {
                                                                    session1.Save(resultDetail);
                                                                });
                                                            }


                                                        }
                                                        double totalDensity = tg.ResultDetailDTOs.Sum(tr => tr.MaxRecord);
                                                        tg.ResultDetailDTOs.ForEach(r =>
                                                        {
                                                            r.Density = Math.Round(r.MaxRecord / totalDensity, 2);
                                                        });
                                                    }
                                                    break;
                                                #endregion
                                                #region case 1
                                                case 1:
                                                    {//Kế hoạch không có methods thì không lấy
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.Methods.Count > 0 && (p.TargetGroupDetail == null || p.TargetGroupDetail.Id == t.Id) && !p.IsDisable && !p.IsDelete).ToList();
                                                        //if (t.ParentTargetGroupDetail != null)
                                                        //{
                                                        //    tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).ToList().Map<CriterionDictionaryDTO>();
                                                        //}
                                                        //else
                                                        //{
                                                        //    tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
                                                        //}

                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == pld.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            //if (isRate == 1)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            //if (isRate == 2)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month == 12 && r.EndTime.Month <= 2).ToList();
                                                            //if (isRate == 3)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            //if (isRate == 4)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            //if (isRate == 0)
                                                                Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                //rd.ResultName = crite.Name;
                                                                //rd.ManageCodeId = crite.FromPlanKPIDetail.ManageCode.Id;
                                                                //rd.ManageCodeName = crite.FromPlanKPIDetail.ManageCode.Name;
                                                                //rd.OrderNumber = crite.OrderNumber;
                                                                //rd.MaxRecord = crite.MaxRecord;

                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.Id = rdetail != null ? rdetail.Id : Guid.Empty;
                                                                    rd.ResultName = pld.TargetDetail; // tên mục tiêu
                                                                    rd.ManageCodeId = pld.ManageCode.Id;
                                                                    rd.Record = rdetail != null ? rdetail.Record : 0; // điểm tự đánh giá
                                                                    foreach (var diem in mt.LeadDepartment)
                                                                    {
                                                                        if (diem.DepartmentId.Id == departmentId)
                                                                        {
                                                                            rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : isRate == 1 ? diem.Diem1 : isRate == 2 ? diem.Diem2 : isRate == 3 ? diem.Diem3 : isRate == 4 ? diem.Diem4 : diem.DiemSo; // điểm cấp trên đánh giá
                                                                        }
                                                                    }

                                                                    rd.PreviousResult = rdetail != null ? rdetail.PreviousResult : ""; // kết quả thực hiện
                                                                    rd.MaxRecord = rdetail != null ? rdetail.MaxRecord : 0;
                                                                    //rd.Density = rdetail.DensityResult;

                                                                }
                                                                // rd.Density = rd.SupervisorRecord * rd.MaxRecord;
                                                                rd.PlanKPIDetailId = pld.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }

                                                    }
                                                    break;
                                                #endregion
                                                #region case 6
                                                case 6:
                                                    {
                                                        List<Criterion> CriterionDictionaries = new List<Criterion>();
                                                        Dictionary<Guid, Guid> planParentDetail = new Dictionary<Guid, Guid>();
                                                        CriterionDictionaries = session.Query<Criterion>().Where(c => (c.Department.Id == departmentId && c.Staff.Id == null)).ToList();


                                                        //Lọc ra các criterion đúng nhóm tiêu chí                                       
                                                        CriterionDictionaries = CriterionDictionaries.Where(c => c.Staff == null && c.Department.Id == departmentId && c.TargetGroupDetail.TargetGroupDetailType.Id != 3 && c.FromPlanKPIDetail.PlanStaff.PlanKPI.Id == plan.Id
                                                        && c.FromPlanKPIDetail.IsDelete == false && c.FromPlanKPIDetail != null
                                                        && c.TargetGroupDetail.Id == t.Id).OrderBy(c => c.OrderNumber).ToList();

                                                        foreach (Criterion crite in CriterionDictionaries)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == crite.FromPlanKPIDetail.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(crite.FromPlanKPIDetail.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            //if (isRate == 1)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            //if (isRate == 2)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month == 12 && r.EndTime.Month <= 2).ToList();
                                                            //if (isRate == 3)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            //if (isRate == 4)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            //if (isRate == 0)
                                                                Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.ResultName = oriParentPlanDetail.TargetDetail; // tên mục tiêu
                                                                    if (rdetail != null)
                                                                    {
                                                                        rd.Id = rdetail.Id;
                                                                        rd.Record = rdetail.Record; // điểm tự đánh giá
                                                                        foreach (var diem in mt.LeadDepartment)
                                                                        {
                                                                            if (diem.DepartmentId.Id == departmentId)
                                                                            {
                                                                                rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : isRate == 1 ? diem.Diem1 : isRate == 2 ? diem.Diem2 : isRate == 3 ? diem.Diem3 : isRate == 4 ? diem.Diem4 : diem.DiemSo; // điểm cấp trên đánh giá
                                                                            }
                                                                        }
                                                                        rd.PreviousResult = rdetail.PreviousResult; // kết quả thực hiện
                                                                        rd.MaxRecord = rdetail.MaxRecord;
                                                                    }
                                                                }
                                                                rd.PlanKPIDetailId = crite.FromPlanKPIDetail.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }

                                                    }
                                                    break;
                                                #endregion
                                                #region case 7
                                                case 7:
                                                    {
                                                        List<Criterion> CriterionDictionaries = new List<Criterion>();
                                                        Dictionary<Guid, Guid> planParentDetail = new Dictionary<Guid, Guid>();
                                                        CriterionDictionaries = session.Query<Criterion>().Where(c => (c.Department.Id == departmentId && c.Staff.Id == null)).ToList();


                                                        //Lọc ra các criterion đúng nhóm tiêu chí                                       
                                                        CriterionDictionaries = CriterionDictionaries.Where(c => c.Staff == null && c.Department.Id == departmentId && c.TargetGroupDetail.TargetGroupDetailType.Id != 3 && c.FromPlanKPIDetail.PlanStaff.PlanKPI.Id == plan.Id
                                                        && c.FromPlanKPIDetail.IsDelete == false && c.FromPlanKPIDetail != null
                                                        && c.TargetGroupDetail.Id == t.Id).OrderBy(c => c.OrderNumber).ToList();

                                                        foreach (Criterion crite in CriterionDictionaries)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == crite.FromPlanKPIDetail.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(crite.FromPlanKPIDetail.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            //if (isRate == 1)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            //if (isRate == 2)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 12 && r.EndTime.Month <= 2).ToList();
                                                            //if (isRate == 3)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            //if (isRate == 4)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            //if (isRate == 0)
                                                            Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.ResultName = oriParentPlanDetail.TargetDetail; // tên mục tiêu
                                                                    if (rdetail != null)
                                                                    {
                                                                        rd.Id = rdetail.Id;
                                                                        rd.Record = rdetail.Record; // điểm tự đánh giá
                                                                        foreach (var diem in mt.LeadDepartment)
                                                                        {
                                                                            if (diem.DepartmentId.Id == departmentId)
                                                                            {
                                                                                rd.SupervisorRecord = rdetail.SupervisorRecord; // điểm cấp trên đánh giá
                                                                            }
                                                                        }
                                                                        rd.PreviousResult = rdetail.PreviousResult; // kết quả thực hiện
                                                                        rd.MaxRecord = rdetail.MaxRecord;
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (var de in mt.LeadDepartment)
                                                                        {
                                                                            if (de.DepartmentId.Id == departmentId)
                                                                            {
                                                                                rd.SupervisorRecord = isRate == 1 ? de.Diem1 : isRate == 2 ? de.Diem2 : isRate == 3 ? de.Diem3 : isRate == 4 ? de.Diem4 : de.DiemSo;
                                                                            }

                                                                        }
                                                                    }

                                                                }
                                                                rd.PlanKPIDetailId = crite.FromPlanKPIDetail.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }
                                                    }
                                                    break;
                                                    #endregion
                                            }

                                            #endregion
                                        }
                                        break;
                                    case (int)AgentObjectTypes.Khoa:
                                        {
                                            #region khoa

                                            List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();
                                            switch (t.TargetGroupDetailType.Id)
                                            {
                                                #region case 1
                                                case 1:
                                                    {//Kế hoạch không có methods thì không lấy
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.Methods.Count > 0 && (p.TargetGroupDetail == null || p.TargetGroupDetail.Id == t.Id) && !p.IsDisable && !p.IsDelete).ToList();
                                                        if (t.ParentTargetGroupDetail != null)
                                                        {
                                                            tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).ToList().Map<CriterionDictionaryDTO>();
                                                        }
                                                        else
                                                        {
                                                            tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
                                                        }

                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == pld.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                //rd.ResultName = crite.Name;
                                                                //rd.ManageCodeId = crite.FromPlanKPIDetail.ManageCode.Id;
                                                                //rd.ManageCodeName = crite.FromPlanKPIDetail.ManageCode.Name;
                                                                //rd.OrderNumber = crite.OrderNumber;
                                                                //rd.MaxRecord = crite.MaxRecord;

                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.Id = rdetail != null ? rdetail.Id : Guid.Empty;
                                                                    rd.ResultName = pld.TargetDetail; // tên mục tiêu
                                                                    rd.ManageCodeId = pld.ManageCode.Id;
                                                                    rd.Record = rdetail != null ? rdetail.Record : 0; // điểm tự đánh giá
                                                                    foreach (var de in mt.LeadDepartment)
                                                                    {
                                                                        if (de.DepartmentId.Id == departmentId)
                                                                        {
                                                                            rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : isRate == 1 ? de.Diem1 : isRate == 2 ? de.Diem2 : isRate == 3 ? de.Diem3 : isRate == 4 ? de.Diem4 : de.DiemSo; // điểm cấp trên đánh giá
                                                                        }
                                                                    }

                                                                    rd.PreviousResult = rdetail != null ? rdetail.PreviousResult : ""; // kết quả thực hiện
                                                                    rd.MaxRecord = rdetail != null ? rdetail.MaxRecord : 0;
                                                                    //rd.Density = rdetail.DensityResult;

                                                                }
                                                                // rd.Density = rd.SupervisorRecord * rd.MaxRecord;
                                                                rd.PlanKPIDetailId = pld.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }

                                                    }
                                                    break;
                                                #endregion
                                                #region case 0
                                                case 0:
                                                    {
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            if (plan.StudyTerm == null)
                                                            {
                                                                plan.StudyTerm = "CaNam";
                                                            }
                                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                            List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id).Map<CriterionDictionaryDTO>().ToList();
                                                            rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                            rd.PlanKPIDetailId = pld.Id;
                                                            rd.PlanKPIDetailName = pld.Name;
                                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                            }
                                                            else
                                                                rd.IdDanhGiaChiTiet = 2;
                                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };

                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                if (t.TargetGroupDetailType.Id == 0 && pld.FromProfessorCriterion.DanhGiaChiTiet.Id == 1)
                                                                {

                                                                    OtherActivityData dodulieudanhgia = new OtherActivityData();
                                                                    dodulieudanhgia = session.Query<OtherActivityData>().Where(q => (q.StaffCode == staff.StaffInfo.ManageCode
                                                                     || q.StaffCode == staff.StaffInfo.WebUser.UserName) && q.StudyTerm.Trim() == plan.StudyTerm.Trim() &&
                                                                     q.StudyYear == plan.StudyYear && q.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).FirstOrDefault();


                                                                    if (dodulieudanhgia != null)
                                                                    {
                                                                        rd.CurrentResult = dodulieudanhgia.CriterionDictionaryId.Name;
                                                                        rd.SupervisorRecord = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                        rd.Record = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                    }
                                                                }
                                                            }

                                                            switch (pld.FromProfessorCriterion.CriterionType.Id)
                                                            {
                                                                case 3:
                                                                    {
                                                                        if (pld.CurrentKPI != null)
                                                                        {
                                                                            try
                                                                            {
                                                                                Guid crdId = new Guid(pld.CurrentKPI);
                                                                                CriterionDictionary crd = session.Query<CriterionDictionary>().Where(c => c.Id == crdId).FirstOrDefault();
                                                                                rd.RegisterTarget = crd.Name;
                                                                            }
                                                                            catch (Exception e)
                                                                            {

                                                                            }
                                                                        }
                                                                    }
                                                                    break;
                                                                default:
                                                                    {
                                                                        rd.RegisterTarget = pld.CurrentKPI;
                                                                    }
                                                                    break;
                                                            }

                                                            rd.Density = 1;
                                                            tg.ResultDetailDTOs.Add(rd);

                                                            //if (rdetail != null)
                                                            //{
                                                            //    rd.PreviousResult = rdetail.PreviousResult;
                                                            //    rd.Record = rdetail.Record;
                                                            //    // rd.SupervisorRecord = rdetail.SupervisorRecord;
                                                            //    rd.Id = rdetail.Id;
                                                            //    rd.Note = rdetail.Note;
                                                            //    rd.SupervisorNote = rdetail.SupervisorNote;
                                                            //    rd.FileAttachments = new List<FileAttachmentDTO>();
                                                            //    foreach (FileAttachment fad in rdetail.FileAttachments)
                                                            //    {
                                                            //        FileAttachmentDTO fa = new FileAttachmentDTO();
                                                            //        fa.Id = fad.Id;
                                                            //        fa.CreationTime = fad.CreationTime;
                                                            //        fa.Extension = fad.Extension;
                                                            //        fa.Name = fad.Name;
                                                            //        fa.Path = fad.Path;
                                                            //        fa.ResultDetailId = fad.ResultDetail.Id;
                                                            //        rd.FileAttachments.Add(fa);
                                                            //    }
                                                            //    rd.FileAttachmentCount = rd.FileAttachments.Count();
                                                            //    //rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
                                                            //}
                                                            //else
                                                            //{
                                                            //    ResultDetail resultDetail = new ResultDetail();
                                                            //    resultDetail.Id = Guid.NewGuid();
                                                            //    rd.Id = resultDetail.Id;
                                                            //    resultDetail.CurrentResult = rd.CurrentResult;
                                                            //    resultDetail.PreviousResult = rd.PreviousResult;
                                                            //    if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                            //    {
                                                            //        resultDetail.RecordSecond = rd.Record;
                                                            //        resultDetail.Record = rd.RecordOld;
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        resultDetail.Record = rd.Record;
                                                            //    }
                                                            //    if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                            //    {
                                                            //        resultDetail.SupervisorRecord = rd.Record;
                                                            //    }
                                                            //    resultDetail.RegisterTarget = rd.RegisterTarget;
                                                            //    resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                            //    resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                            //    resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                            //    resultDetail.Result = ratingResult;
                                                            //    resultDetail.IsTargetGroupRating = false;

                                                            //    SessionManager.DoWork(session1 =>
                                                            //    {
                                                            //        session1.Save(resultDetail);
                                                            //    });
                                                            //}
                                                        }
                                                    }
                                                    break;
                                                #endregion
                                                #region case 6
                                                case 6:
                                                    {
                                                        List<Criterion> CriterionDictionaries = new List<Criterion>();
                                                        Dictionary<Guid, Guid> planParentDetail = new Dictionary<Guid, Guid>();
                                                        CriterionDictionaries = session.Query<Criterion>().Where(c => (c.Department.Id == departmentId && c.Staff.Id == null)).ToList();


                                                        //Lọc ra các criterion đúng nhóm tiêu chí                                       
                                                        CriterionDictionaries = CriterionDictionaries.Where(c => c.Staff == null && c.Department.Id == departmentId && c.TargetGroupDetail.TargetGroupDetailType.Id != 3 && c.FromPlanKPIDetail.PlanStaff.PlanKPI.Id == plan.Id
                                                        && c.FromPlanKPIDetail.IsDelete == false && c.FromPlanKPIDetail != null
                                                        && c.TargetGroupDetail.Id == t.Id).OrderBy(c => c.OrderNumber).ToList();

                                                        foreach (Criterion crite in CriterionDictionaries)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == crite.FromPlanKPIDetail.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(crite.FromPlanKPIDetail.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            //if (isRate == 1)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            //if (isRate == 2)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month == 12 && r.EndTime.Month <= 2).ToList();
                                                            //if (isRate == 3)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            //if (isRate == 4)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            //if (isRate == 0)
                                                            Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                foreach (Method mt in Methods)
                                                                {

                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.ResultName = oriParentPlanDetail.TargetDetail; // tên mục tiêu
                                                                    if (rdetail != null)
                                                                    {
                                                                        rd.Id = rdetail.Id;
                                                                        rd.Record = rdetail.Record; // điểm tự đánh giá
                                                                        rd.SupervisorRecord = rdetail.SupervisorRecord; // điểm cấp trên đánh giá
                                                                        rd.PreviousResult = rdetail.PreviousResult; // kết quả thực hiện
                                                                        rd.MaxRecord = rdetail.MaxRecord;
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (var de in mt.LeadDepartment)
                                                                        {
                                                                            if (de.DepartmentId.Id == departmentId)
                                                                            {
                                                                                rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : isRate == 1 ? de.Diem1 : isRate == 2 ? de.Diem2 : isRate == 3 ? de.Diem3 : isRate == 4 ? de.Diem4 : de.DiemSo; // điểm cấp trên đánh giá
                                                                            }
                                                                        }

                                                                    }

                                                                }
                                                                rd.PlanKPIDetailId = crite.FromPlanKPIDetail.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }

                                                    }
                                                    break;
                                                #endregion
                                                #region case 7
                                                case 7:
                                                    {
                                                        List<Criterion> CriterionDictionaries = new List<Criterion>();
                                                        Dictionary<Guid, Guid> planParentDetail = new Dictionary<Guid, Guid>();
                                                        CriterionDictionaries = session.Query<Criterion>().Where(c => (c.Department.Id == departmentId && c.Staff.Id == null)).ToList();


                                                        //Lọc ra các criterion đúng nhóm tiêu chí                                       
                                                        CriterionDictionaries = CriterionDictionaries.Where(c => c.Staff == null && c.Department.Id == departmentId && c.TargetGroupDetail.TargetGroupDetailType.Id != 3 && c.FromPlanKPIDetail.PlanStaff.PlanKPI.Id == plan.Id
                                                        && c.FromPlanKPIDetail.IsDelete == false && c.FromPlanKPIDetail != null
                                                        && c.TargetGroupDetail.Id == t.Id).OrderBy(c => c.OrderNumber).ToList();

                                                        foreach (Criterion crite in CriterionDictionaries)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == crite.FromPlanKPIDetail.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(crite.FromPlanKPIDetail.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            //if (isRate == 1)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            //if (isRate == 2)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month == 12 && r.EndTime.Month <= 2).ToList();
                                                            //if (isRate == 3)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            //if (isRate == 4)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            //if (isRate == 0)
                                                            Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                foreach (Method mt in Methods)
                                                                {

                                                                    rd.ResultName = oriParentPlanDetail.TargetDetail; // tên mục tiêu
                                                                    if (rdetail != null)
                                                                    {
                                                                        rd.Id = rdetail.Id;
                                                                        rd.Record = rdetail.Record; // điểm tự đánh giá
                                                                        rd.SupervisorRecord = rdetail.SupervisorRecord; // điểm cấp trên đánh giá
                                                                        rd.PreviousResult = rdetail.PreviousResult; // kết quả thực hiện
                                                                        rd.MaxRecord = rdetail.MaxRecord;
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (var de in mt.LeadDepartment)
                                                                        {
                                                                            if (de.DepartmentId.Id == departmentId)
                                                                            {
                                                                                rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : isRate == 1 ? de.Diem1 : isRate == 2 ? de.Diem2 : isRate == 3 ? de.Diem3 : isRate == 4 ? de.Diem4 : de.DiemSo; // điểm cấp trên đánh giá
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                rd.PlanKPIDetailId = crite.FromPlanKPIDetail.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }

                                                    }
                                                    break;
                                                    #endregion
                                            }
                                            #endregion
                                        }
                                        break;
                                    case (int)AgentObjectTypes.BoMon:
                                        {
                                            #region bộ môn
                                            List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();
                                            switch (t.TargetGroupDetailType.Id)
                                            {
                                                #region case 1
                                                case 1:
                                                    {//Kế hoạch không có methods thì không lấy
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.Methods.Count > 0 && (p.TargetGroupDetail == null || p.TargetGroupDetail.Id == t.Id) && !p.IsDisable && !p.IsDelete).ToList();
                                                        if (t.ParentTargetGroupDetail != null)
                                                        {
                                                            tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).ToList().Map<CriterionDictionaryDTO>();
                                                        }
                                                        else
                                                        {
                                                            tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
                                                        }

                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == pld.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                //rd.ResultName = crite.Name;
                                                                //rd.ManageCodeId = crite.FromPlanKPIDetail.ManageCode.Id;
                                                                //rd.ManageCodeName = crite.FromPlanKPIDetail.ManageCode.Name;
                                                                //rd.OrderNumber = crite.OrderNumber;
                                                                //rd.MaxRecord = crite.MaxRecord;

                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.Id = rdetail != null ? rdetail.Id : Guid.Empty;
                                                                    rd.ResultName = pld.TargetDetail; // tên mục tiêu
                                                                    rd.ManageCodeId = pld.ManageCode.Id;
                                                                    rd.Record = rdetail != null ? rdetail.Record : 0; // điểm tự đánh giá
                                                                    foreach (var de in mt.LeadDepartment)
                                                                    {
                                                                        if (de.DepartmentId.Id == departmentId)
                                                                        {
                                                                            rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : isRate == 1 ? de.Diem1 : isRate == 2 ? de.Diem2 : isRate == 3 ? de.Diem3 : isRate == 4 ? de.Diem4 : de.DiemSo; // điểm cấp trên đánh giá
                                                                        }
                                                                    }

                                                                    rd.PreviousResult = rdetail != null ? rdetail.PreviousResult : ""; // kết quả thực hiện
                                                                    rd.MaxRecord = rdetail != null ? rdetail.MaxRecord : 0;
                                                                    //rd.Density = rdetail.DensityResult;

                                                                }
                                                                // rd.Density = rd.SupervisorRecord * rd.MaxRecord;
                                                                rd.PlanKPIDetailId = pld.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }

                                                    }
                                                    break;
                                                #endregion
                                                #region case 0
                                                case 0:
                                                    {
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            if (plan.StudyTerm == null)
                                                            {
                                                                plan.StudyTerm = "CaNam";
                                                            }
                                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                            List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id).Map<CriterionDictionaryDTO>().ToList();
                                                            rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                            rd.PlanKPIDetailId = pld.Id;
                                                            rd.PlanKPIDetailName = pld.Name;
                                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                            }
                                                            else
                                                                rd.IdDanhGiaChiTiet = 2;
                                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };

                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                if (t.TargetGroupDetailType.Id == 0 && pld.FromProfessorCriterion.DanhGiaChiTiet.Id == 1)
                                                                {

                                                                    OtherActivityData dodulieudanhgia = new OtherActivityData();
                                                                    dodulieudanhgia = session.Query<OtherActivityData>().Where(q => (q.StaffCode == staff.StaffInfo.ManageCode
                                                                     || q.StaffCode == staff.StaffInfo.WebUser.UserName) && q.StudyTerm.Trim() == plan.StudyTerm.Trim() &&
                                                                     q.StudyYear == plan.StudyYear && q.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).FirstOrDefault();


                                                                    if (dodulieudanhgia != null)
                                                                    {
                                                                        rd.CurrentResult = dodulieudanhgia.CriterionDictionaryId.Name;
                                                                        rd.SupervisorRecord = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                        rd.Record = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                    }
                                                                }
                                                            }

                                                            switch (pld.FromProfessorCriterion.CriterionType.Id)
                                                            {
                                                                case 3:
                                                                    {
                                                                        if (pld.CurrentKPI != null)
                                                                        {
                                                                            try
                                                                            {
                                                                                Guid crdId = new Guid(pld.CurrentKPI);
                                                                                CriterionDictionary crd = session.Query<CriterionDictionary>().Where(c => c.Id == crdId).FirstOrDefault();
                                                                                rd.RegisterTarget = crd.Name;
                                                                            }
                                                                            catch (Exception e)
                                                                            {

                                                                            }
                                                                        }
                                                                    }
                                                                    break;
                                                                default:
                                                                    {
                                                                        rd.RegisterTarget = pld.CurrentKPI;
                                                                    }
                                                                    break;
                                                            }

                                                            rd.Density = 1;
                                                            tg.ResultDetailDTOs.Add(rd);

                                                            //if (rdetail != null)
                                                            //{
                                                            //    rd.PreviousResult = rdetail.PreviousResult;
                                                            //    rd.Record = rdetail.Record;
                                                            //    // rd.SupervisorRecord = rdetail.SupervisorRecord;
                                                            //    rd.Id = rdetail.Id;
                                                            //    rd.Note = rdetail.Note;
                                                            //    rd.SupervisorNote = rdetail.SupervisorNote;
                                                            //    rd.FileAttachments = new List<FileAttachmentDTO>();
                                                            //    foreach (FileAttachment fad in rdetail.FileAttachments)
                                                            //    {
                                                            //        FileAttachmentDTO fa = new FileAttachmentDTO();
                                                            //        fa.Id = fad.Id;
                                                            //        fa.CreationTime = fad.CreationTime;
                                                            //        fa.Extension = fad.Extension;
                                                            //        fa.Name = fad.Name;
                                                            //        fa.Path = fad.Path;
                                                            //        fa.ResultDetailId = fad.ResultDetail.Id;
                                                            //        rd.FileAttachments.Add(fa);
                                                            //    }
                                                            //    rd.FileAttachmentCount = rd.FileAttachments.Count();
                                                            //    //rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
                                                            //}
                                                            //else
                                                            //{
                                                            //    ResultDetail resultDetail = new ResultDetail();
                                                            //    resultDetail.Id = Guid.NewGuid();
                                                            //    rd.Id = resultDetail.Id;
                                                            //    resultDetail.CurrentResult = rd.CurrentResult;
                                                            //    resultDetail.PreviousResult = rd.PreviousResult;
                                                            //    if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                            //    {
                                                            //        resultDetail.RecordSecond = rd.Record;
                                                            //        resultDetail.Record = rd.RecordOld;
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        resultDetail.Record = rd.Record;
                                                            //    }
                                                            //    if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                            //    {
                                                            //        resultDetail.SupervisorRecord = rd.Record;
                                                            //    }
                                                            //    resultDetail.RegisterTarget = rd.RegisterTarget;
                                                            //    resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                            //    resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                            //    resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                            //    resultDetail.Result = ratingResult;
                                                            //    resultDetail.IsTargetGroupRating = false;

                                                            //    SessionManager.DoWork(session1 =>
                                                            //    {
                                                            //        session1.Save(resultDetail);
                                                            //    });
                                                            //}
                                                        }
                                                    }
                                                    break;
                                                #endregion
                                                #region case 6
                                                case 6:
                                                    {
                                                        List<Criterion> CriterionDictionaries = new List<Criterion>();
                                                        Dictionary<Guid, Guid> planParentDetail = new Dictionary<Guid, Guid>();
                                                        CriterionDictionaries = session.Query<Criterion>().Where(c => (c.Department.Id == null && c.Staff.Id == staff.Id)).ToList();


                                                        //Lọc ra các criterion đúng nhóm tiêu chí                                       
                                                        CriterionDictionaries = CriterionDictionaries.Where(c => c.Staff != null && c.Staff.Id == staff.Id && c.TargetGroupDetail.TargetGroupDetailType.Id != 3 && c.FromPlanKPIDetail.PlanStaff.PlanKPI.Id == plan.Id
                                                        && c.FromPlanKPIDetail.IsDelete == false && c.FromPlanKPIDetail != null
                                                        && c.TargetGroupDetail.Id == t.Id).OrderBy(c => c.OrderNumber).ToList();

                                                        foreach (Criterion crite in CriterionDictionaries)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == crite.FromPlanKPIDetail.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(crite.FromPlanKPIDetail.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            //if (isRate == 1)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            //if (isRate == 2)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month == 12 && r.EndTime.Month <= 2).ToList();
                                                            //if (isRate == 3)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            //if (isRate == 4)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            //if (isRate == 0)
                                                            Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.ResultName = mt.Name; // tên mục tiêu
                                                                    if (rdetail != null)
                                                                    {
                                                                        rd.Id = rdetail.Id;
                                                                        rd.Record = rdetail.Record; // điểm tự đánh giá
                                                                        rd.SupervisorRecord = rdetail.SupervisorRecord; // điểm cấp trên đánh giá
                                                                        rd.PreviousResult = rdetail.PreviousResult; // kết quả thực hiện
                                                                        rd.MaxRecord = rdetail.MaxRecord;
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (var de in mt.LeadDepartment)
                                                                        {
                                                                            if (de.DepartmentId.Id == departmentId)
                                                                            {
                                                                                rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : isRate == 1 ? de.Diem1 : isRate == 2 ? de.Diem2 : isRate == 3 ? de.Diem3 : isRate == 4 ? de.Diem4 : de.DiemSo;
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                rd.PlanKPIDetailId = crite.FromPlanKPIDetail.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }

                                                    }
                                                    break;
                                                #endregion
                                                #region case 7
                                                case 7:
                                                    {
                                                        List<Criterion> CriterionDictionaries = new List<Criterion>();
                                                        Dictionary<Guid, Guid> planParentDetail = new Dictionary<Guid, Guid>();
                                                        CriterionDictionaries = session.Query<Criterion>().Where(c => (c.Department.Id == null && c.Staff.Id == staff.Id)).ToList();


                                                        //Lọc ra các criterion đúng nhóm tiêu chí                                       
                                                        CriterionDictionaries = CriterionDictionaries.Where(c => c.Staff != null && c.Staff.Id == staff.Id && c.TargetGroupDetail.TargetGroupDetailType.Id != 3 && c.FromPlanKPIDetail.PlanStaff.PlanKPI.Id == plan.Id
                                                        && c.FromPlanKPIDetail.IsDelete == false && c.FromPlanKPIDetail != null
                                                        && c.TargetGroupDetail.Id == t.Id).OrderBy(c => c.OrderNumber).ToList();

                                                        foreach (Criterion crite in CriterionDictionaries)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == crite.FromPlanKPIDetail.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(crite.FromPlanKPIDetail.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            //if (isRate == 1)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            //if (isRate == 2)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month == 12 && r.EndTime.Month <= 2).ToList();
                                                            //if (isRate == 3)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            //if (isRate == 4)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            //if (isRate == 0)
                                                            Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.ResultName = mt.Name; // tên mục tiêu
                                                                    if (rdetail != null)
                                                                    {
                                                                        rd.Id = rdetail.Id;
                                                                        rd.Record = rdetail.Record; // điểm tự đánh giá
                                                                        rd.SupervisorRecord = rdetail.SupervisorRecord; // điểm cấp trên đánh giá
                                                                        rd.PreviousResult = rdetail.PreviousResult; // kết quả thực hiện
                                                                        rd.MaxRecord = rdetail.MaxRecord;
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (var de in mt.LeadDepartment)
                                                                        {
                                                                            if (de.DepartmentId.Id == departmentId)
                                                                            {

                                                                                rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : isRate == 1 ? de.Diem1 : isRate == 2 ? de.Diem2 : isRate == 3 ? de.Diem3 : isRate == 4 ? de.Diem4 : de.DiemSo; // điểm cấp trên đánh giá

                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                rd.PlanKPIDetailId = crite.FromPlanKPIDetail.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }
                                                    }
                                                    break;
                                                    #endregion
                                            }
                                            #endregion
                                        }
                                        break;
                                    case (int)AgentObjectTypes.PhoPhongBan:
                                        {
                                            #region Phó phòng ban

                                            List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();
                                            switch (t.TargetGroupDetailType.Id)
                                            {
                                                #region case 0
                                                case 0:
                                                    {
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            if (plan.StudyTerm == null)
                                                            {
                                                                plan.StudyTerm = "CaNam";
                                                            }
                                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                            List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id).Map<CriterionDictionaryDTO>().ToList();
                                                            rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                            rd.PlanKPIDetailId = pld.Id;
                                                            rd.PlanKPIDetailName = pld.Name;
                                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                            }
                                                            else
                                                                rd.IdDanhGiaChiTiet = 2;
                                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };

                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                if (t.TargetGroupDetailType.Id == 0 && pld.FromProfessorCriterion.DanhGiaChiTiet.Id == 1)
                                                                {

                                                                    OtherActivityData dodulieudanhgia = new OtherActivityData();
                                                                    dodulieudanhgia = session.Query<OtherActivityData>().Where(q => (q.StaffCode == staff.StaffInfo.ManageCode
                                                                     || q.StaffCode == staff.StaffInfo.WebUser.UserName) && q.StudyTerm.Trim() == plan.StudyTerm.Trim() &&
                                                                     q.StudyYear == plan.StudyYear && q.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).FirstOrDefault();


                                                                    if (dodulieudanhgia != null)
                                                                    {
                                                                        rd.CurrentResult = dodulieudanhgia.CriterionDictionaryId.Name;
                                                                        rd.SupervisorRecord = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                        rd.Record = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                    }
                                                                }
                                                            }

                                                            switch (pld.FromProfessorCriterion.CriterionType.Id)
                                                            {
                                                                case 3:
                                                                    {
                                                                        if (pld.CurrentKPI != null)
                                                                        {
                                                                            try
                                                                            {
                                                                                Guid crdId = new Guid(pld.CurrentKPI);
                                                                                CriterionDictionary crd = session.Query<CriterionDictionary>().Where(c => c.Id == crdId).FirstOrDefault();
                                                                                rd.RegisterTarget = crd.Name;
                                                                            }
                                                                            catch (Exception e)
                                                                            {

                                                                            }
                                                                        }
                                                                    }
                                                                    break;
                                                                default:
                                                                    {
                                                                        rd.RegisterTarget = pld.CurrentKPI;
                                                                    }
                                                                    break;
                                                            }



                                                            //if (rdetail != null)
                                                            //{
                                                            //    rd.PreviousResult = rdetail.PreviousResult;
                                                            //    rd.Record = rdetail.Record;
                                                            //    // rd.SupervisorRecord = rdetail.SupervisorRecord;
                                                            //    rd.Id = rdetail.Id;
                                                            //    rd.Note = rdetail.Note;
                                                            //    rd.SupervisorNote = rdetail.SupervisorNote;
                                                            //    rd.FileAttachments = new List<FileAttachmentDTO>();
                                                            //    foreach (FileAttachment fad in rdetail.FileAttachments)
                                                            //    {
                                                            //        FileAttachmentDTO fa = new FileAttachmentDTO();
                                                            //        fa.Id = fad.Id;
                                                            //        fa.CreationTime = fad.CreationTime;
                                                            //        fa.Extension = fad.Extension;
                                                            //        fa.Name = fad.Name;
                                                            //        fa.Path = fad.Path;
                                                            //        fa.ResultDetailId = fad.ResultDetail.Id;
                                                            //        rd.FileAttachments.Add(fa);
                                                            //    }
                                                            //    rd.FileAttachmentCount = rd.FileAttachments.Count();
                                                            //    //rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
                                                            //}
                                                            //else
                                                            //{
                                                            //    ResultDetail resultDetail = new ResultDetail();
                                                            //    resultDetail.Id = Guid.NewGuid();
                                                            //    rd.Id = resultDetail.Id;
                                                            //    resultDetail.CurrentResult = rd.CurrentResult;
                                                            //    resultDetail.PreviousResult = rd.PreviousResult;
                                                            //    if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                            //    {
                                                            //        resultDetail.RecordSecond = rd.Record;
                                                            //        resultDetail.Record = rd.RecordOld;
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        resultDetail.Record = rd.Record;
                                                            //    }
                                                            //    if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                            //    {
                                                            //        resultDetail.SupervisorRecord = rd.Record;
                                                            //    }
                                                            //    resultDetail.RegisterTarget = rd.RegisterTarget;
                                                            //    resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                            //    resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                            //    resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                            //    resultDetail.Result = ratingResult;
                                                            //    resultDetail.IsTargetGroupRating = false;

                                                            //    SessionManager.DoWork(session1 =>
                                                            //    {
                                                            //        session1.Save(resultDetail);
                                                            //    });
                                                            //}
                                                            tg.ResultDetailDTOs.Add(rd);
                                                        }
                                                    }
                                                    break;
                                                #endregion
                                                #region case 5
                                                case 5:
                                                    {
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            if (plan.StudyTerm == null)
                                                            {
                                                                plan.StudyTerm = "CaNam";
                                                            }
                                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                            List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id).Map<CriterionDictionaryDTO>().ToList();
                                                            rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                            rd.PlanKPIDetailId = pld.Id;
                                                            rd.PlanKPIDetailName = pld.Name;
                                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                            }
                                                            else
                                                                rd.IdDanhGiaChiTiet = 2;
                                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };

                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                if (t.TargetGroupDetailType.Id == 0 && pld.FromProfessorCriterion.DanhGiaChiTiet.Id == 1)
                                                                {

                                                                    OtherActivityData dodulieudanhgia = new OtherActivityData();
                                                                    dodulieudanhgia = session.Query<OtherActivityData>().Where(q => (q.StaffCode == staff.StaffInfo.ManageCode
                                                                     || q.StaffCode == staff.StaffInfo.WebUser.UserName) && q.StudyTerm.Trim() == plan.StudyTerm.Trim() &&
                                                                     q.StudyYear == plan.StudyYear && q.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).FirstOrDefault();

                                                                    // CriterionDictionary listcrit = session.Query<CriterionDictionary>().Where(r => r.ManageCode == dodulieudanhgia).FirstOrDefault();

                                                                    if (dodulieudanhgia != null)
                                                                    {
                                                                        rd.CurrentResult = dodulieudanhgia.CriterionDictionaryId.Name;
                                                                        rd.SupervisorRecord = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                    }
                                                                }
                                                            }
                                                            rd.ScienceResearches = new List<ScienceResearchDTO>();
                                                            foreach (ScienceResearch pa in pld.ScienceResearches)
                                                            {
                                                                ScienceResearchDTO pad = new ScienceResearchDTO();
                                                                pad.Id = pa.Id;
                                                                pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                                                                pad.NumberOfResearch = pa.NumberOfResearch;
                                                                pad.OrderNumber = pa.OrderNumber;
                                                                rd.ScienceResearches.Add(pad);
                                                            }
                                                            rd.ScienceResearches = rd.ScienceResearches.OrderBy(p => p.OrderNumber).ToList();
                                                            List<OtherActivityData> scienceResearch = new List<OtherActivityData>();
                                                            if (plan.PlanType.Id == 1)
                                                            {
                                                                scienceResearch = session.Query<OtherActivityData>().Where(s => (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                            && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            else
                                                            {
                                                                scienceResearch = session.Query<OtherActivityData>().Where(s => (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                                && s.StudyTerm == plan.StudyTerm && s.StudyYear == plan.StudyYear && s.ManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            foreach (OtherActivityData pa in scienceResearch)
                                                            {
                                                                ScienceResearchDataDTO pad = new ScienceResearchDataDTO();
                                                                pad.Name = pa.Name;
                                                                //  pad.NumberOfHour = tempcri != null ? tempcri.NumberOfHour : 0;
                                                                // pad.NumberOfTimeDouble = pa.NumberOfTime;
                                                                pad.ManageCode = pa.ManageCode;
                                                                rd.ActivityHour += pa.NumberOfTime;
                                                                pad.NumberOfTime = pa.NumberOfTime;
                                                                pad.Record = pa.CriterionDictionaryId.Record;
                                                                rd.SupervisorRecord += pa.CriterionDictionaryId.Record;
                                                                rd.ScienceResearchesResult.Add(pad);

                                                            }
                                                            TotalOtherActivityHour += rd.ActivityHour;
                                                            rd.Density = 1;
                                                            tg.ResultDetailDTOs.Add(rd);
                                                            if (rdetail != null)
                                                            {
                                                                rd.PreviousResult = rdetail.PreviousResult;
                                                                rd.Record = rdetail.Record;
                                                                // rd.SupervisorRecord = rdetail.SupervisorRecord;
                                                                rd.Id = rdetail.Id;
                                                                rd.Note = rdetail.Note;
                                                                rd.SupervisorNote = rdetail.SupervisorNote;
                                                                rd.FileAttachments = new List<FileAttachmentDTO>();
                                                                foreach (FileAttachment fad in rdetail.FileAttachments)
                                                                {
                                                                    FileAttachmentDTO fa = new FileAttachmentDTO();
                                                                    fa.Id = fad.Id;
                                                                    fa.CreationTime = fad.CreationTime;
                                                                    fa.Extension = fad.Extension;
                                                                    fa.Name = fad.Name;
                                                                    fa.Path = fad.Path;
                                                                    fa.ResultDetailId = fad.ResultDetail.Id;
                                                                    rd.FileAttachments.Add(fa);
                                                                }
                                                                rd.FileAttachmentCount = rd.FileAttachments.Count();
                                                                //rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
                                                            }
                                                            else
                                                            {
                                                                ResultDetail resultDetail = new ResultDetail();
                                                                resultDetail.Id = Guid.NewGuid();
                                                                rd.Id = resultDetail.Id;
                                                                resultDetail.CurrentResult = rd.CurrentResult;
                                                                resultDetail.PreviousResult = rd.PreviousResult;
                                                                if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                                {
                                                                    resultDetail.RecordSecond = rd.Record;
                                                                    resultDetail.Record = rd.RecordOld;
                                                                }
                                                                else
                                                                {
                                                                    resultDetail.Record = rd.Record;
                                                                }
                                                                if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                                {
                                                                    resultDetail.SupervisorRecord = rd.Record;
                                                                }
                                                                resultDetail.RegisterTarget = rd.RegisterTarget;
                                                                resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                                resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                                resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                                resultDetail.Result = ratingResult;
                                                                resultDetail.IsTargetGroupRating = false;

                                                                SessionManager.DoWork(session1 =>
                                                                {
                                                                    session1.Save(resultDetail);
                                                                });
                                                            }
                                                        }
                                                    }
                                                    break;
                                                #endregion
                                                #region case 4
                                                case 4:
                                                    {
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                            List<OtherActivityData> hoatdongkhac = new List<OtherActivityData>();

                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                            }
                                                            else
                                                                rd.IdDanhGiaChiTiet = 2;

                                                            rd.ProfessorOtherActivities = new List<ProfessorOtherActivityDTO>();
                                                            rd.ActivityHour = 0;

                                                            //lúc trước đổ NCKH,HDK,Giảng dạy riêng nay gộp lại chung 1 form đổ

                                                            foreach (ProfessorOtherActivity pa in pld.ProfessorOtherActivities)
                                                            {
                                                                ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                                                                pad.Id = pa.Id;
                                                                pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                                                                pad.NumberOfTime = pa.NumberOfTime;
                                                                pad.OrderNumber = pa.OrderNumber;
                                                                rd.ProfessorOtherActivities.Add(pad);
                                                            }

                                                            rd.ProfessorOtherActivities = rd.ProfessorOtherActivities.ToList();

                                                            rd.ProfessorOtherActivitiesResult = new List<ProfessorOtherActivityDTO>();

                                                            List<OtherActivityData> otherActivities = new List<OtherActivityData>();
                                                            if (plan.PlanType.Id == 1) //KH năm thì lấy hoạt động của cả 2 học kỳ
                                                            {
                                                                otherActivities = session.Query<OtherActivityData>().Where(s =>
                                                                (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                                && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            else
                                                            {
                                                                otherActivities = session.Query<OtherActivityData>().Where(s =>
                                                                (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName) && s.StudyTerm == plan.StudyTerm
                                                                && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            foreach (OtherActivityData pa in otherActivities)
                                                            {
                                                                //try
                                                                //{
                                                                //  CriterionDictionary tempcri = session.Query<CriterionDictionary>().Where(c => c.ManageCode == pa.CriterionDictionaryId.ManageCode).FirstOrDefault();
                                                                ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                                                                pad.Name = pa.Name;
                                                                //  pad.NumberOfHour = tempcri != null ? tempcri.NumberOfHour : 0;
                                                                pad.NumberOfTimeDouble = pa.NumberOfTime;
                                                                pad.ManageCode = pa.ManageCode;
                                                                rd.ActivityHour += pa.NumberOfTime;
                                                                pad.SupervisorRecord = pa.CriterionDictionaryId.Record;
                                                                rd.SupervisorRecord += pa.CriterionDictionaryId.Record;
                                                                //  rd.SupervisorRecord = pa.CriterionDictionaryId.Record != 0? pa.CriterionDictionaryId.Record:0;
                                                                rd.ProfessorOtherActivitiesResult.Add(pad);
                                                                //}
                                                                //catch { }
                                                            }
                                                            TotalOtherActivityHour += rd.ActivityHour;
                                                            //int TotalHourDefault = session.Query<KPIConfiguration>().FirstOrDefault().TotalHourDefault;
                                                            //rd.SupervisorRecord = Math.Round((rd.ActivityHour / TotalHourDefault) * 100, 1);
                                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                            rd.RegisterTarget = pld.CurrentKPI;
                                                            rd.CriterionDictionaries = pld.FromProfessorCriterion.CriterionDictionaries.Map<CriterionDictionaryDTO>();
                                                            rd.PlanKPIDetailId = pld.Id;
                                                            rd.PlanKPIDetailName = pld.Name;
                                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };
                                                            tg.ResultDetailDTOs.Add(rd);
                                                            if (rdetail != null)
                                                            {
                                                                rd.PreviousResult = rdetail.PreviousResult;
                                                                rd.CurrentResult = rdetail.CurrentResult;

                                                                rd.RegisterTarget = rdetail.RegisterTarget;
                                                                if (rdetail.RecordSecond != 0 && ratingResult.IsUnlocked == true)
                                                                {
                                                                    rd.Record = rdetail.RecordSecond;
                                                                    rd.RecordOld = rdetail.Record;
                                                                }
                                                                else
                                                                {
                                                                    rd.Record = rdetail.Record;
                                                                    rd.RecordOld = rdetail.Record;
                                                                }
                                                                // rd.SupervisorRecord = rd.SupervisorRecord > 0 ? rd.SupervisorRecord : rdetail.SupervisorRecord;
                                                                rd.Id = rdetail.Id;
                                                            }
                                                            else
                                                            {
                                                                ResultDetail resultDetail = new ResultDetail();
                                                                resultDetail.Id = Guid.NewGuid();
                                                                rd.Id = resultDetail.Id;
                                                                resultDetail.CurrentResult = rd.CurrentResult;
                                                                resultDetail.PreviousResult = rd.PreviousResult;
                                                                if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                                {
                                                                    resultDetail.RecordSecond = rd.Record;
                                                                    resultDetail.Record = rd.RecordOld;
                                                                }
                                                                else
                                                                {
                                                                    resultDetail.Record = rd.Record;
                                                                }
                                                                if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                                {
                                                                    resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                                }
                                                                resultDetail.RegisterTarget = rd.RegisterTarget;
                                                                resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                                resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                                resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                                resultDetail.Result = ratingResult;
                                                                resultDetail.IsTargetGroupRating = false;

                                                                SessionManager.DoWork(session1 =>
                                                                {
                                                                    session1.Save(resultDetail);
                                                                });
                                                            }


                                                        }
                                                        double totalDensity = tg.ResultDetailDTOs.Sum(tr => tr.MaxRecord);
                                                        tg.ResultDetailDTOs.ForEach(r =>
                                                        {
                                                            r.Density = Math.Round(r.MaxRecord / totalDensity, 2);
                                                        });
                                                    }
                                                    break;
                                                #endregion
                                                #region case 1
                                                case 1:
                                                    {//Kế hoạch không có methods thì không lấy
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.Methods.Count > 0 && (p.TargetGroupDetail == null || p.TargetGroupDetail.Id == t.Id) && !p.IsDisable && !p.IsDelete).ToList();
                                                        if (t.ParentTargetGroupDetail != null)
                                                        {
                                                            tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).ToList().Map<CriterionDictionaryDTO>();
                                                        }
                                                        else
                                                        {
                                                            tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
                                                        }

                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == pld.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            //if (isRate == 1)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            //if (isRate == 2)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month == 12 && r.EndTime.Month <= 2).ToList();
                                                            //if (isRate == 3)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            //if (isRate == 4)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            //if (isRate == 0)
                                                            Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                //rd.ResultName = crite.Name;
                                                                //rd.ManageCodeId = crite.FromPlanKPIDetail.ManageCode.Id;
                                                                //rd.ManageCodeName = crite.FromPlanKPIDetail.ManageCode.Name;
                                                                //rd.OrderNumber = crite.OrderNumber;
                                                                //rd.MaxRecord = crite.MaxRecord;

                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.Id = rdetail != null ? rdetail.Id : Guid.Empty;
                                                                    rd.ResultName = pld.TargetDetail; // tên mục tiêu
                                                                    rd.ManageCodeId = pld.ManageCode.Id;
                                                                    rd.Record = rdetail != null ? rdetail.Record : 0; // điểm tự đánh giá
                                                                    foreach (var de in mt.LeadDepartment)
                                                                    {
                                                                        if (de.DepartmentId.Id == departmentId)
                                                                        {
                                                                            rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : isRate == 1 ? de.Diem1 : isRate == 2 ? de.Diem2 : isRate == 3 ? de.Diem3 : isRate == 4 ? de.Diem4 : de.DiemSo; // điểm cấp trên đánh giá
                                                                        }
                                                                    }

                                                                    rd.PreviousResult = rdetail != null ? rdetail.PreviousResult : ""; // kết quả thực hiện
                                                                    rd.MaxRecord = rdetail != null ? rdetail.MaxRecord : 0;
                                                                    //rd.Density = rdetail.DensityResult;

                                                                }
                                                                rd.PlanKPIDetailId = pld.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }
                                                    }
                                                    break;
                                                #endregion
                                                #region case 6
                                                case 6:
                                                    {
                                                        List<Criterion> CriterionDictionaries = new List<Criterion>();
                                                        Dictionary<Guid, Guid> planParentDetail = new Dictionary<Guid, Guid>();
                                                        CriterionDictionaries = session.Query<Criterion>().Where(c => (c.Department.Id == null && c.Staff.Id == staff.Id)).ToList();


                                                        //Lọc ra các criterion đúng nhóm tiêu chí                                       
                                                        CriterionDictionaries = CriterionDictionaries.Where(c => c.Staff != null && c.Staff.Id == staff.Id && c.TargetGroupDetail.TargetGroupDetailType.Id != 3 && c.FromPlanKPIDetail.PlanStaff.PlanKPI.Id == plan.Id
                                                        && c.FromPlanKPIDetail.IsDelete == false && c.FromPlanKPIDetail != null
                                                        && c.TargetGroupDetail.Id == t.Id).OrderBy(c => c.OrderNumber).ToList();

                                                        foreach (Criterion crite in CriterionDictionaries)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == crite.FromPlanKPIDetail.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(crite.FromPlanKPIDetail.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            //if (isRate == 1)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            //if (isRate == 2)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month == 12 && r.EndTime.Month <= 2).ToList();
                                                            //if (isRate == 3)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            //if (isRate == 4)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            //if (isRate == 0)
                                                            Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.ResultName = mt.Name; // tên mục tiêu
                                                                    if (rdetail != null)
                                                                    {
                                                                        rd.Id = rdetail.Id;
                                                                        rd.Record = rdetail.Record; // điểm tự đánh giá
                                                                        rd.SupervisorRecord = rdetail.SupervisorRecord; // điểm cấp trên đánh giá
                                                                        rd.PreviousResult = rdetail.PreviousResult; // kết quả thực hiện
                                                                        rd.MaxRecord = rdetail.MaxRecord;
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (var de in mt.LeadDepartment)
                                                                        {
                                                                            if (de.DepartmentId.Id == departmentId)
                                                                            {
                                                                                rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : isRate == 1 ? de.Diem1 : isRate == 2 ? de.Diem2 : isRate == 3 ? de.Diem3 : isRate == 4 ? de.Diem4 : de.DiemSo; // điểm cấp trên đánh giá

                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                rd.PlanKPIDetailId = crite.FromPlanKPIDetail.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }

                                                    }
                                                    break;
                                                #endregion
                                                #region case 7
                                                case 7:
                                                    {
                                                        List<Criterion> CriterionDictionaries = new List<Criterion>();
                                                        Dictionary<Guid, Guid> planParentDetail = new Dictionary<Guid, Guid>();
                                                        CriterionDictionaries = session.Query<Criterion>().Where(c => (c.Department.Id == null && c.Staff.Id == staff.Id)).ToList();


                                                        //Lọc ra các criterion đúng nhóm tiêu chí                                       
                                                        CriterionDictionaries = CriterionDictionaries.Where(c => c.Staff != null && c.Staff.Id == staff.Id && c.TargetGroupDetail.TargetGroupDetailType.Id != 3 && c.FromPlanKPIDetail.PlanStaff.PlanKPI.Id == plan.Id
                                                        && c.FromPlanKPIDetail.IsDelete == false && c.FromPlanKPIDetail != null
                                                        && c.TargetGroupDetail.Id == t.Id).OrderBy(c => c.OrderNumber).ToList();

                                                        foreach (Criterion crite in CriterionDictionaries)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == crite.FromPlanKPIDetail.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(crite.FromPlanKPIDetail.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            //if (isRate == 1)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            //if (isRate == 2)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month == 12 && r.EndTime.Month <= 2).ToList();
                                                            //if (isRate == 3)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            //if (isRate == 4)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            //if (isRate == 0)
                                                            Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.ResultName = mt.Name; // tên mục tiêu
                                                                    if (rdetail != null)
                                                                    {
                                                                        rd.Id = rdetail.Id;
                                                                        rd.Record = rdetail.Record; // điểm tự đánh giá
                                                                        rd.SupervisorRecord = rdetail.SupervisorRecord; // điểm cấp trên đánh giá
                                                                        rd.PreviousResult = rdetail.PreviousResult; // kết quả thực hiện
                                                                        rd.MaxRecord = rdetail.MaxRecord;
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (var de in mt.LeadDepartment)
                                                                        {
                                                                            if (de.DepartmentId.Id == departmentId)
                                                                            {
                                                                                rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : isRate == 1 ? de.Diem1 : isRate == 2 ? de.Diem2 : isRate == 3 ? de.Diem3 : isRate == 4 ? de.Diem4 : de.DiemSo; // điểm cấp trên đánh giá


                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                rd.PlanKPIDetailId = crite.FromPlanKPIDetail.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }
                                                    }
                                                    break;
                                                    #endregion
                                            }
                                            #endregion
                                        }
                                        break;
                                    case (int)AgentObjectTypes.PhoKhoa:
                                        {
                                            #region phó trường khoa


                                            List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();
                                            switch (t.TargetGroupDetailType.Id)
                                            {
                                                #region case 0
                                                case 0:
                                                    {
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            if (plan.StudyTerm == null)
                                                            {
                                                                plan.StudyTerm = "CaNam";
                                                            }
                                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                            List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id).Map<CriterionDictionaryDTO>().ToList();
                                                            rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                            rd.PlanKPIDetailId = pld.Id;
                                                            rd.PlanKPIDetailName = pld.Name;
                                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                            }
                                                            else
                                                                rd.IdDanhGiaChiTiet = 2;
                                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };

                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                if (t.TargetGroupDetailType.Id == 0 && pld.FromProfessorCriterion.DanhGiaChiTiet.Id == 1)
                                                                {

                                                                    OtherActivityData dodulieudanhgia = new OtherActivityData();
                                                                    dodulieudanhgia = session.Query<OtherActivityData>().Where(q => (q.StaffCode == staff.StaffInfo.ManageCode
                                                                     || q.StaffCode == staff.StaffInfo.WebUser.UserName) && q.StudyTerm.Trim() == plan.StudyTerm.Trim() &&
                                                                     q.StudyYear == plan.StudyYear && q.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).FirstOrDefault();


                                                                    if (dodulieudanhgia != null)
                                                                    {
                                                                        rd.CurrentResult = dodulieudanhgia.CriterionDictionaryId.Name;
                                                                        rd.SupervisorRecord = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                        rd.Record = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                    }
                                                                }
                                                            }

                                                            switch (pld.FromProfessorCriterion.CriterionType.Id)
                                                            {
                                                                case 3:
                                                                    {
                                                                        if (pld.CurrentKPI != null)
                                                                        {
                                                                            try
                                                                            {
                                                                                Guid crdId = new Guid(pld.CurrentKPI);
                                                                                CriterionDictionary crd = session.Query<CriterionDictionary>().Where(c => c.Id == crdId).FirstOrDefault();
                                                                                rd.RegisterTarget = crd.Name;
                                                                            }
                                                                            catch (Exception e)
                                                                            {

                                                                            }
                                                                        }
                                                                    }
                                                                    break;
                                                                default:
                                                                    {
                                                                        rd.RegisterTarget = pld.CurrentKPI;
                                                                    }
                                                                    break;
                                                            }



                                                            //if (rdetail != null)
                                                            //{
                                                            //    rd.PreviousResult = rdetail.PreviousResult;
                                                            //    rd.Record = rdetail.Record;
                                                            //    // rd.SupervisorRecord = rdetail.SupervisorRecord;
                                                            //    rd.Id = rdetail.Id;
                                                            //    rd.Note = rdetail.Note;
                                                            //    rd.SupervisorNote = rdetail.SupervisorNote;
                                                            //    rd.FileAttachments = new List<FileAttachmentDTO>();
                                                            //    foreach (FileAttachment fad in rdetail.FileAttachments)
                                                            //    {
                                                            //        FileAttachmentDTO fa = new FileAttachmentDTO();
                                                            //        fa.Id = fad.Id;
                                                            //        fa.CreationTime = fad.CreationTime;
                                                            //        fa.Extension = fad.Extension;
                                                            //        fa.Name = fad.Name;
                                                            //        fa.Path = fad.Path;
                                                            //        fa.ResultDetailId = fad.ResultDetail.Id;
                                                            //        rd.FileAttachments.Add(fa);
                                                            //    }
                                                            //    rd.FileAttachmentCount = rd.FileAttachments.Count();
                                                            //    //rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
                                                            //}
                                                            //else
                                                            //{
                                                            //    ResultDetail resultDetail = new ResultDetail();
                                                            //    resultDetail.Id = Guid.NewGuid();
                                                            //    rd.Id = resultDetail.Id;
                                                            //    resultDetail.CurrentResult = rd.CurrentResult;
                                                            //    resultDetail.PreviousResult = rd.PreviousResult;
                                                            //    if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                            //    {
                                                            //        resultDetail.RecordSecond = rd.Record;
                                                            //        resultDetail.Record = rd.RecordOld;
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        resultDetail.Record = rd.Record;
                                                            //    }
                                                            //    if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                            //    {
                                                            //        resultDetail.SupervisorRecord = rd.Record;
                                                            //    }
                                                            //    resultDetail.RegisterTarget = rd.RegisterTarget;
                                                            //    resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                            //    resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                            //    resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                            //    resultDetail.Result = ratingResult;
                                                            //    resultDetail.IsTargetGroupRating = false;

                                                            //    SessionManager.DoWork(session1 =>
                                                            //    {
                                                            //        session1.Save(resultDetail);
                                                            //    });
                                                            //}
                                                            tg.ResultDetailDTOs.Add(rd);
                                                        }
                                                    }
                                                    break;
                                                #endregion
                                                #region case 5
                                                case 5:
                                                    {
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            if (plan.StudyTerm == null)
                                                            {
                                                                plan.StudyTerm = "CaNam";
                                                            }
                                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                            List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == pld.FromProfessorCriterion.Id).Map<CriterionDictionaryDTO>().ToList();
                                                            rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                            rd.PlanKPIDetailId = pld.Id;
                                                            rd.PlanKPIDetailName = pld.Name;
                                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                            }
                                                            else
                                                                rd.IdDanhGiaChiTiet = 2;
                                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };

                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                if (t.TargetGroupDetailType.Id == 0 && pld.FromProfessorCriterion.DanhGiaChiTiet.Id == 1)
                                                                {

                                                                    OtherActivityData dodulieudanhgia = new OtherActivityData();
                                                                    dodulieudanhgia = session.Query<OtherActivityData>().Where(q => (q.StaffCode == staff.StaffInfo.ManageCode
                                                                     || q.StaffCode == staff.StaffInfo.WebUser.UserName) && q.StudyTerm.Trim() == plan.StudyTerm.Trim() &&
                                                                     q.StudyYear == plan.StudyYear && q.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).FirstOrDefault();

                                                                    // CriterionDictionary listcrit = session.Query<CriterionDictionary>().Where(r => r.ManageCode == dodulieudanhgia).FirstOrDefault();

                                                                    if (dodulieudanhgia != null)
                                                                    {
                                                                        rd.CurrentResult = dodulieudanhgia.CriterionDictionaryId.Name;
                                                                        rd.SupervisorRecord = dodulieudanhgia.CriterionDictionaryId.Record;
                                                                    }
                                                                }
                                                            }
                                                            rd.ScienceResearches = new List<ScienceResearchDTO>();
                                                            foreach (ScienceResearch pa in pld.ScienceResearches)
                                                            {
                                                                ScienceResearchDTO pad = new ScienceResearchDTO();
                                                                pad.Id = pa.Id;
                                                                pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                                                                pad.NumberOfResearch = pa.NumberOfResearch;
                                                                pad.OrderNumber = pa.OrderNumber;
                                                                rd.ScienceResearches.Add(pad);
                                                            }
                                                            rd.ScienceResearches = rd.ScienceResearches.OrderBy(p => p.OrderNumber).ToList();
                                                            List<OtherActivityData> scienceResearch = new List<OtherActivityData>();
                                                            if (plan.PlanType.Id == 1)
                                                            {
                                                                scienceResearch = session.Query<OtherActivityData>().Where(s => (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                            && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            else
                                                            {
                                                                scienceResearch = session.Query<OtherActivityData>().Where(s => (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                                && s.StudyTerm == plan.StudyTerm && s.StudyYear == plan.StudyYear && s.ManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            foreach (OtherActivityData pa in scienceResearch)
                                                            {
                                                                ScienceResearchDataDTO pad = new ScienceResearchDataDTO();
                                                                pad.Name = pa.Name;
                                                                //  pad.NumberOfHour = tempcri != null ? tempcri.NumberOfHour : 0;
                                                                // pad.NumberOfTimeDouble = pa.NumberOfTime;
                                                                pad.ManageCode = pa.ManageCode;
                                                                rd.ActivityHour += pa.NumberOfTime;
                                                                pad.NumberOfTime = pa.NumberOfTime;
                                                                pad.Record = pa.CriterionDictionaryId.Record;
                                                                rd.SupervisorRecord += pa.CriterionDictionaryId.Record;
                                                                rd.ScienceResearchesResult.Add(pad);

                                                            }
                                                            TotalOtherActivityHour += rd.ActivityHour;
                                                            rd.Density = 1;
                                                            tg.ResultDetailDTOs.Add(rd);
                                                            if (rdetail != null)
                                                            {
                                                                rd.PreviousResult = rdetail.PreviousResult;
                                                                rd.Record = rdetail.Record;
                                                                // rd.SupervisorRecord = rdetail.SupervisorRecord;
                                                                rd.Id = rdetail.Id;
                                                                rd.Note = rdetail.Note;
                                                                rd.SupervisorNote = rdetail.SupervisorNote;
                                                                rd.FileAttachments = new List<FileAttachmentDTO>();
                                                                foreach (FileAttachment fad in rdetail.FileAttachments)
                                                                {
                                                                    FileAttachmentDTO fa = new FileAttachmentDTO();
                                                                    fa.Id = fad.Id;
                                                                    fa.CreationTime = fad.CreationTime;
                                                                    fa.Extension = fad.Extension;
                                                                    fa.Name = fad.Name;
                                                                    fa.Path = fad.Path;
                                                                    fa.ResultDetailId = fad.ResultDetail.Id;
                                                                    rd.FileAttachments.Add(fa);
                                                                }
                                                                rd.FileAttachmentCount = rd.FileAttachments.Count();
                                                                //rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
                                                            }
                                                            else
                                                            {
                                                                ResultDetail resultDetail = new ResultDetail();
                                                                resultDetail.Id = Guid.NewGuid();
                                                                rd.Id = resultDetail.Id;
                                                                resultDetail.CurrentResult = rd.CurrentResult;
                                                                resultDetail.PreviousResult = rd.PreviousResult;
                                                                if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                                {
                                                                    resultDetail.RecordSecond = rd.Record;
                                                                    resultDetail.Record = rd.RecordOld;
                                                                }
                                                                else
                                                                {
                                                                    resultDetail.Record = rd.Record;
                                                                }
                                                                if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                                {
                                                                    resultDetail.SupervisorRecord = rd.Record;
                                                                }
                                                                resultDetail.RegisterTarget = rd.RegisterTarget;
                                                                resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                                resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                                resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                                resultDetail.Result = ratingResult;
                                                                resultDetail.IsTargetGroupRating = false;

                                                                SessionManager.DoWork(session1 =>
                                                                {
                                                                    session1.Save(resultDetail);
                                                                });
                                                            }
                                                        }
                                                    }
                                                    break;
                                                #endregion
                                                #region case 4
                                                case 4:
                                                    {
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail != null && p.TargetGroupDetail.Id == t.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.StudyYears.Any(y => y.Id == plan.StudyYear1.Id)).OrderBy(p => p.FromProfessorCriterion.OrderNumber).ToList();
                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == false);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                            List<OtherActivityData> hoatdongkhac = new List<OtherActivityData>();

                                                            if (pld.FromProfessorCriterion.DanhGiaChiTiet != null)
                                                            {
                                                                rd.IdDanhGiaChiTiet = pld.FromProfessorCriterion.DanhGiaChiTiet.Id;
                                                            }
                                                            else
                                                                rd.IdDanhGiaChiTiet = 2;

                                                            rd.ProfessorOtherActivities = new List<ProfessorOtherActivityDTO>();
                                                            rd.ActivityHour = 0;

                                                            //lúc trước đổ NCKH,HDK,Giảng dạy riêng nay gộp lại chung 1 form đổ

                                                            foreach (ProfessorOtherActivity pa in pld.ProfessorOtherActivities)
                                                            {
                                                                ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                                                                pad.Id = pa.Id;
                                                                pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                                                                pad.NumberOfTime = pa.NumberOfTime;
                                                                pad.OrderNumber = pa.OrderNumber;
                                                                rd.ProfessorOtherActivities.Add(pad);
                                                            }

                                                            rd.ProfessorOtherActivities = rd.ProfessorOtherActivities.ToList();

                                                            rd.ProfessorOtherActivitiesResult = new List<ProfessorOtherActivityDTO>();

                                                            List<OtherActivityData> otherActivities = new List<OtherActivityData>();
                                                            if (plan.PlanType.Id == 1) //KH năm thì lấy hoạt động của cả 2 học kỳ
                                                            {
                                                                otherActivities = session.Query<OtherActivityData>().Where(s =>
                                                                (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                                                && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            else
                                                            {
                                                                otherActivities = session.Query<OtherActivityData>().Where(s =>
                                                                (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName) && s.StudyTerm == plan.StudyTerm
                                                                && s.StudyYear == plan.StudyYear && s.ActivityManageCode == pld.FromProfessorCriterion.ManageCode).ToList();
                                                            }
                                                            foreach (OtherActivityData pa in otherActivities)
                                                            {
                                                                //try
                                                                //{
                                                                //  CriterionDictionary tempcri = session.Query<CriterionDictionary>().Where(c => c.ManageCode == pa.CriterionDictionaryId.ManageCode).FirstOrDefault();
                                                                ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                                                                pad.Name = pa.Name;
                                                                //  pad.NumberOfHour = tempcri != null ? tempcri.NumberOfHour : 0;
                                                                pad.NumberOfTimeDouble = pa.NumberOfTime;
                                                                pad.ManageCode = pa.ManageCode;
                                                                rd.ActivityHour += pa.NumberOfTime;
                                                                pad.SupervisorRecord = pa.CriterionDictionaryId.Record;
                                                                rd.SupervisorRecord += pa.CriterionDictionaryId.Record;
                                                                //  rd.SupervisorRecord = pa.CriterionDictionaryId.Record != 0? pa.CriterionDictionaryId.Record:0;
                                                                rd.ProfessorOtherActivitiesResult.Add(pad);
                                                                //}
                                                                //catch { }
                                                            }
                                                            TotalOtherActivityHour += rd.ActivityHour;
                                                            //int TotalHourDefault = session.Query<KPIConfiguration>().FirstOrDefault().TotalHourDefault;
                                                            //rd.SupervisorRecord = Math.Round((rd.ActivityHour / TotalHourDefault) * 100, 1);
                                                            rd.CriterionId = pld.FromProfessorCriterion.Id;
                                                            rd.CriterionName = pld.FromProfessorCriterion.Name;
                                                            rd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                                                            rd.Tooltip = pld.FromProfessorCriterion.Tooltip;
                                                            rd.RegisterTarget = pld.CurrentKPI;
                                                            rd.CriterionDictionaries = pld.FromProfessorCriterion.CriterionDictionaries.Map<CriterionDictionaryDTO>();
                                                            rd.PlanKPIDetailId = pld.Id;
                                                            rd.PlanKPIDetailName = pld.Name;
                                                            rd.MaxRecord = pld.FromProfessorCriterion.Record;
                                                            rd.TargetGroupDetail = new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id };
                                                            tg.ResultDetailDTOs.Add(rd);
                                                            if (rdetail != null)
                                                            {
                                                                rd.PreviousResult = rdetail.PreviousResult;
                                                                rd.CurrentResult = rdetail.CurrentResult;

                                                                rd.RegisterTarget = rdetail.RegisterTarget;
                                                                if (rdetail.RecordSecond != 0 && ratingResult.IsUnlocked == true)
                                                                {
                                                                    rd.Record = rdetail.RecordSecond;
                                                                    rd.RecordOld = rdetail.Record;
                                                                }
                                                                else
                                                                {
                                                                    rd.Record = rdetail.Record;
                                                                    rd.RecordOld = rdetail.Record;
                                                                }
                                                                // rd.SupervisorRecord = rd.SupervisorRecord > 0 ? rd.SupervisorRecord : rdetail.SupervisorRecord;
                                                                rd.Id = rdetail.Id;
                                                            }
                                                            else
                                                            {
                                                                ResultDetail resultDetail = new ResultDetail();
                                                                resultDetail.Id = Guid.NewGuid();
                                                                rd.Id = resultDetail.Id;
                                                                resultDetail.CurrentResult = rd.CurrentResult;
                                                                resultDetail.PreviousResult = rd.PreviousResult;
                                                                if (ratingResult.IsUnlocked == true && resultDetail.Record != rd.RecordOld)
                                                                {
                                                                    resultDetail.RecordSecond = rd.Record;
                                                                    resultDetail.Record = rd.RecordOld;
                                                                }
                                                                else
                                                                {
                                                                    resultDetail.Record = rd.Record;
                                                                }
                                                                if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                                                {
                                                                    resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                                }
                                                                resultDetail.RegisterTarget = rd.RegisterTarget;
                                                                resultDetail.SupervisorRecord = rd.SupervisorRecord;
                                                                resultDetail.PlanKPIDetail = new PlanKPIDetail() { Id = rd.PlanKPIDetailId };
                                                                resultDetail.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                                                resultDetail.Result = ratingResult;
                                                                resultDetail.IsTargetGroupRating = false;

                                                                SessionManager.DoWork(session1 =>
                                                                {
                                                                    session1.Save(resultDetail);
                                                                });
                                                            }


                                                        }
                                                        double totalDensity = tg.ResultDetailDTOs.Sum(tr => tr.MaxRecord);
                                                        tg.ResultDetailDTOs.ForEach(r =>
                                                        {
                                                            r.Density = Math.Round(r.MaxRecord / totalDensity, 2);
                                                        });
                                                    }
                                                    break;
                                                #endregion
                                                #region case 1
                                                case 1:
                                                    {//Kế hoạch không có methods thì không lấy
                                                        planDetails = planStaff.PlanKPIDetails.Where(p => p.Methods.Count > 0 && (p.TargetGroupDetail == null || p.TargetGroupDetail.Id == t.Id) && !p.IsDisable && !p.IsDelete).ToList();
                                                        if (t.ParentTargetGroupDetail != null)
                                                        {
                                                            tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).ToList().Map<CriterionDictionaryDTO>();
                                                        }
                                                        else
                                                        {
                                                            tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
                                                        }

                                                        foreach (PlanKPIDetail pld in planDetails)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == pld.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                //rd.ResultName = crite.Name;
                                                                //rd.ManageCodeId = crite.FromPlanKPIDetail.ManageCode.Id;
                                                                //rd.ManageCodeName = crite.FromPlanKPIDetail.ManageCode.Name;
                                                                //rd.OrderNumber = crite.OrderNumber;
                                                                //rd.MaxRecord = crite.MaxRecord;

                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.Id = rdetail != null ? rdetail.Id : Guid.Empty;
                                                                    rd.ResultName = pld.TargetDetail; // tên mục tiêu
                                                                    rd.ManageCodeId = pld.ManageCode.Id;
                                                                    rd.Record = rdetail != null ? rdetail.Record : 0; // điểm tự đánh giá
                                                                    foreach (var de in mt.LeadDepartment)
                                                                    {
                                                                        if (de.DepartmentId.Id == departmentId)
                                                                        {
                                                                            rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : isRate == 1 ? de.Diem1 : isRate == 2 ? de.Diem2 : isRate == 3 ? de.Diem3 : isRate == 4 ? de.Diem4 : de.DiemSo; // điểm cấp trên đánh giá
                                                                        }
                                                                    }

                                                                    rd.PreviousResult = rdetail != null ? rdetail.PreviousResult : ""; // kết quả thực hiện
                                                                    rd.MaxRecord = rdetail != null ? rdetail.MaxRecord : 0;
                                                                    //rd.Density = rdetail.DensityResult;

                                                                }
                                                                // rd.Density = rd.SupervisorRecord * rd.MaxRecord;
                                                                rd.PlanKPIDetailId = pld.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                #endregion
                                                #region case 6
                                                case 6:
                                                    {
                                                        List<Criterion> CriterionDictionaries = new List<Criterion>();
                                                        Dictionary<Guid, Guid> planParentDetail = new Dictionary<Guid, Guid>();
                                                        CriterionDictionaries = session.Query<Criterion>().Where(c => (c.Department.Id == null && c.Staff.Id == staff.Id)).ToList();


                                                        //Lọc ra các criterion đúng nhóm tiêu chí                                       
                                                        CriterionDictionaries = CriterionDictionaries.Where(c => c.Staff != null && c.Staff.Id == staff.Id && c.TargetGroupDetail.TargetGroupDetailType.Id != 3 && c.FromPlanKPIDetail.PlanStaff.PlanKPI.Id == plan.Id
                                                        && c.FromPlanKPIDetail.IsDelete == false && c.FromPlanKPIDetail != null
                                                        && c.TargetGroupDetail.Id == t.Id).OrderBy(c => c.OrderNumber).ToList();

                                                        foreach (Criterion crite in CriterionDictionaries)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == crite.FromPlanKPIDetail.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(crite.FromPlanKPIDetail.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            //if (isRate == 1)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            //if (isRate == 2)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month == 12 && r.EndTime.Month <= 2).ToList();
                                                            //if (isRate == 3)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            //if (isRate == 4)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            //if (isRate == 0)
                                                            Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.ResultName = mt.Name; // tên mục tiêu
                                                                    if (rdetail != null)
                                                                    {
                                                                        rd.Id = rdetail.Id;
                                                                        rd.Record = rdetail.Record; // điểm tự đánh giá
                                                                        rd.SupervisorRecord = rdetail.SupervisorRecord; // điểm cấp trên đánh giá
                                                                        rd.PreviousResult = rdetail.PreviousResult; // kết quả thực hiện
                                                                        rd.MaxRecord = rdetail.MaxRecord;
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (var de in mt.LeadDepartment)
                                                                        {
                                                                            if (de.DepartmentId.Id == departmentId)
                                                                            {
                                                                                rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : isRate == 1 ? de.Diem1 : isRate == 2 ? de.Diem2 : isRate == 3 ? de.Diem3 : isRate == 4 ? de.Diem4 : de.DiemSo; // điểm cấp trên đánh giá
                                                                            }

                                                                        }
                                                                    }

                                                                }
                                                                rd.PlanKPIDetailId = crite.FromPlanKPIDetail.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }

                                                    }
                                                    break;
                                                #endregion
                                                #region case 7
                                                case 7:
                                                    {
                                                        List<Criterion> CriterionDictionaries = new List<Criterion>();
                                                        Dictionary<Guid, Guid> planParentDetail = new Dictionary<Guid, Guid>();
                                                        CriterionDictionaries = session.Query<Criterion>().Where(c => (c.Department.Id == null && c.Staff.Id == staff.Id)).ToList();


                                                        //Lọc ra các criterion đúng nhóm tiêu chí                                       
                                                        CriterionDictionaries = CriterionDictionaries.Where(c => c.Staff != null && c.Staff.Id == staff.Id && c.TargetGroupDetail.TargetGroupDetailType.Id != 3 && c.FromPlanKPIDetail.PlanStaff.PlanKPI.Id == plan.Id
                                                        && c.FromPlanKPIDetail.IsDelete == false && c.FromPlanKPIDetail != null
                                                        && c.TargetGroupDetail.Id == t.Id).OrderBy(c => c.OrderNumber).ToList();

                                                        foreach (Criterion crite in CriterionDictionaries)
                                                        {
                                                            ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == crite.FromPlanKPIDetail.Id && r.TargetGroupDetail.Id == t.Id);
                                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();

                                                            PlanKPIDetail oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(crite.FromPlanKPIDetail.Id, session);

                                                            List<Method> Methods = new List<Method>();
                                                            //if (isRate == 1)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 9 && r.EndTime.Month <= 11).ToList();
                                                            //if (isRate == 2)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month == 12 && r.EndTime.Month <= 2).ToList();
                                                            //if (isRate == 3)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 3 && r.EndTime.Month <= 5).ToList();
                                                            //if (isRate == 4)
                                                            //    Methods = oriParentPlanDetail.Methods.Where(r => r.StartTime.Month >= 6 && r.EndTime.Month <= 8).ToList();
                                                            //if (isRate == 0)
                                                            Methods = oriParentPlanDetail.Methods.ToList();

                                                            if (Methods.Count > 0)
                                                            {
                                                                foreach (Method mt in Methods)
                                                                {
                                                                    MethodDTO it = new MethodDTO();
                                                                    rd.ResultName = mt.Name; // tên mục tiêu
                                                                    if (rdetail != null)
                                                                    {
                                                                        rd.Id = rdetail.Id;
                                                                        rd.Record = rdetail.Record; // điểm tự đánh giá
                                                                        rd.SupervisorRecord = rdetail.SupervisorRecord; // điểm cấp trên đánh giá
                                                                        rd.PreviousResult = rdetail.PreviousResult; // kết quả thực hiện
                                                                        rd.MaxRecord = rdetail.MaxRecord;
                                                                    }
                                                                    else
                                                                    {
                                                                        foreach (var de in mt.LeadDepartment)
                                                                        {
                                                                            if (de.DepartmentId.Id == departmentId)
                                                                            {
                                                                                rd.SupervisorRecord = rdetail != null ? rdetail.SupervisorRecord : isRate == 1 ? de.Diem1 : isRate == 2 ? de.Diem2 : isRate == 3 ? de.Diem3 : isRate == 4 ? de.Diem4 : de.DiemSo; // điểm cấp trên đánh giá
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                rd.PlanKPIDetailId = crite.FromPlanKPIDetail.Id;
                                                                tg.ResultDetailDTOs.Add(rd);
                                                            }

                                                        }
                                                    }
                                                    break;
                                                    #endregion
                                            }
                                            #endregion
                                        }
                                        break;
                                    case (int)AgentObjectTypes.HieuTruong:
                                        {
                                            #region hiệu trưởng

                                            List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();

                                            planDetails = planStaff.PlanKPIDetails.Where(p => p.Methods.Count > 0 && p.TargetGroupDetail.Id == t.Id && !p.IsDisable && !p.IsDelete).ToList();
                                            //switch (t.TargetGroupDetailType.Id)
                                            //{
                                            //    case 0:
                                            //        {
                                            //            planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == t.Id).ToList();
                                            //        }
                                            //        break;

                                            //}
                                            if (t.ParentTargetGroupDetail != null)
                                            {
                                                tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).ToList().Map<CriterionDictionaryDTO>();
                                            }
                                            else
                                            {
                                                tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
                                            }

                                            foreach (PlanKPIDetail pld in planDetails)
                                            {

                                                ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == pld.Id && r.TargetGroupDetail.Id == t.Id);
                                                ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                //rd.Id = pld.Id;
                                                rd.OrderNumber = pld.OrderNumber;
                                                rd.CriterionId = pld.FromCriterion != null ? pld.FromCriterion.Id : Guid.Empty;
                                                rd.CriterionName = pld.FromCriterion != null ? pld.FromCriterion.Name : string.Empty;
                                                rd.CriterionTypeId = pld.FromCriterion != null ? pld.FromCriterion.CriterionType.Id : -1;
                                                rd.Tooltip = pld.FromCriterion != null ? pld.FromCriterion.Tooltip : string.Empty;
                                                rd.Density = ControllerHelpers.GetOriginalDensity(pld.Id, session);
                                                if (tg.TargetGroupDetailTypeId == 3)
                                                {
                                                    rd.Density = 100;
                                                }
                                                try
                                                {
                                                    if (pld.CurrentKPI != null)
                                                    {
                                                        Guid currentKPIId = new Guid(pld.CurrentKPI);
                                                        if (t.ParentTargetGroupDetail != null)
                                                        {
                                                            rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).Select(c => c.Name).FirstOrDefault();
                                                        }
                                                        else
                                                        {
                                                            rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.Id).Select(c => c.Name).FirstOrDefault();
                                                        }

                                                    }
                                                }
                                                catch (Exception e)
                                                {

                                                }
                                                rd.PlanKPIDetailId = pld.Id;
                                                rd.PlanKPIDetailName = pld.TargetDetail;
                                                rd.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                if (pld.PlanKPIDetail_KPIs != null)
                                                {
                                                    foreach (PlanKPIDetail_KPI m in pld.PlanKPIDetail_KPIs)
                                                    {
                                                        PlanKPIDetail_KPIDTO kpi = new PlanKPIDetail_KPIDTO();
                                                        kpi.Id = m.Id;
                                                        kpi.Name = m.Name;
                                                        kpi.MeasureUnitId = m.MeasureUnit != null ? m.MeasureUnit.Id : 0;
                                                        kpi.MeasureUnitName = m.MeasureUnit != null ? m.MeasureUnit.Name : "";
                                                        rd.PlanKPIDetail_KPIs.Add(kpi);
                                                    }
                                                }

                                                rd.MaxRecord = ControllerHelpers.GetOriginalDensity(pld.Id, session);
                                                rd.PlanKPIDetailNameString = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).TargetDetail;

                                                if (rdetail != null)
                                                {
                                                    rd.PreviousResult = rdetail.PreviousResult;
                                                    rd.CurrentResult = rdetail.CurrentResult;
                                                    if (rdetail.RecordSecond != 0 && ratingResult.IsUnlocked == true)
                                                    {
                                                        rd.Record = rdetail.RecordSecond;
                                                        rd.RecordOld = rdetail.Record;
                                                    }
                                                    else
                                                    {
                                                        rd.Record = rdetail.Record;
                                                        rd.RecordOld = rdetail.Record;
                                                    }

                                                    rd.SupervisorRecord = rdetail.SupervisorRecord;
                                                    rd.Note = rdetail.Note;
                                                    rd.SupervisorNote = rdetail.SupervisorNote;
                                                    rd.FileAttachments = rdetail.FileAttachments.Map<FileAttachmentDTO>();
                                                    rd.FileAttachmentCount = rd.FileAttachments.Count();
                                                    //rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
                                                    rd.Id = rdetail.Id;
                                                }
                                                else
                                                {

                                                    rd.Id = Guid.NewGuid();
                                                    ResultDetail newResultDetail = new ResultDetail()
                                                    {
                                                        Id = rd.Id,
                                                        PlanKPIDetail = new PlanKPIDetail() { Id = pld.Id },
                                                        Result = ratingResult,
                                                        TargetGroupDetail = t
                                                    };
                                                    SessionManager.DoWork(session1 =>
                                                    {
                                                        session1.Save(newResultDetail);
                                                    });

                                                }
                                                if (t.TargetGroupDetailType.Id == 3 && rd.PlanKPIDetailName != null)
                                                {
                                                    List<CriterionDictionary> crds = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail != null && c.TargetGroupDetail.Id == t.Id).ToList();
                                                    try
                                                    {
                                                        rd.PlanKPIDetailNameString = crds.FirstOrDefault(c => c.Id == new Guid(rd.PlanKPIDetailName)).Name;
                                                    }
                                                    catch { }
                                                }

                                                tg.ResultDetailDTOs.Add(rd);
                                            }
                                            double totalDensity = tg.ResultDetailDTOs.Sum(tr => tr.Density);
                                            tg.ResultDetailDTOs.ForEach(r =>
                                            {
                                                r.Density = Math.Round(r.Density / totalDensity, 2);
                                                r.DensityPercent = Math.Round(r.Density * 100, 0);
                                            });

                                            #endregion
                                        }
                                        break;
                                    case (int)AgentObjectTypes.PhoHieuTruong:
                                        {
                                            #region phó hiệu trưởng

                                            List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();

                                            planDetails = planStaff.PlanKPIDetails.Where(p => p.Methods.Count > 0 && p.TargetGroupDetail.Id == t.Id && !p.IsDisable && !p.IsDelete).ToList();
                                            //switch (t.TargetGroupDetailType.Id)
                                            //{
                                            //    case 0:
                                            //        {
                                            //            planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == t.Id).ToList();
                                            //        }
                                            //        break;

                                            //}
                                            if (t.ParentTargetGroupDetail != null)
                                            {
                                                tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).ToList().Map<CriterionDictionaryDTO>();
                                            }
                                            else
                                            {
                                                tg.CriterionDictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
                                            }

                                            foreach (PlanKPIDetail pld in planDetails)
                                            {

                                                ResultDetail rdetail = resultDetails.FirstOrDefault(r => r.PlanKPIDetail != null && r.PlanKPIDetail.Id == pld.Id && r.TargetGroupDetail.Id == t.Id);
                                                ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                //rd.Id = pld.Id;
                                                rd.OrderNumber = pld.OrderNumber;
                                                rd.CriterionId = pld.FromCriterion != null ? pld.FromCriterion.Id : Guid.Empty;
                                                rd.CriterionName = pld.FromCriterion != null ? pld.FromCriterion.Name : string.Empty;
                                                rd.CriterionTypeId = pld.FromCriterion != null ? pld.FromCriterion.CriterionType.Id : -1;
                                                rd.Tooltip = pld.FromCriterion != null ? pld.FromCriterion.Tooltip : string.Empty;
                                                rd.Density = ControllerHelpers.GetOriginalDensity(pld.Id, session);
                                                if (tg.TargetGroupDetailTypeId == 3)
                                                {
                                                    rd.Density = 100;
                                                }
                                                try
                                                {
                                                    if (pld.CurrentKPI != null)
                                                    {
                                                        Guid currentKPIId = new Guid(pld.CurrentKPI);
                                                        if (t.ParentTargetGroupDetail != null)
                                                        {
                                                            rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.ParentTargetGroupDetail.Id).Select(c => c.Name).FirstOrDefault();
                                                        }
                                                        else
                                                        {
                                                            rd.CurrentKPIName = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId && c.TargetGroupDetail.Id == t.Id).Select(c => c.Name).FirstOrDefault();
                                                        }

                                                    }
                                                }
                                                catch (Exception e)
                                                {

                                                }
                                                rd.PlanKPIDetailId = pld.Id;
                                                rd.PlanKPIDetailName = pld.TargetDetail;
                                                rd.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                if (pld.PlanKPIDetail_KPIs != null)
                                                {
                                                    foreach (PlanKPIDetail_KPI m in pld.PlanKPIDetail_KPIs)
                                                    {
                                                        PlanKPIDetail_KPIDTO kpi = new PlanKPIDetail_KPIDTO();
                                                        kpi.Id = m.Id;
                                                        kpi.Name = m.Name;
                                                        kpi.MeasureUnitId = m.MeasureUnit != null ? m.MeasureUnit.Id : 0;
                                                        kpi.MeasureUnitName = m.MeasureUnit != null ? m.MeasureUnit.Name : "";
                                                        rd.PlanKPIDetail_KPIs.Add(kpi);
                                                    }
                                                }

                                                rd.MaxRecord = ControllerHelpers.GetOriginalDensity(pld.Id, session);
                                                rd.PlanKPIDetailNameString = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).TargetDetail;

                                                if (rdetail != null)
                                                {
                                                    rd.PreviousResult = rdetail.PreviousResult;
                                                    rd.CurrentResult = rdetail.CurrentResult;
                                                    if (rdetail.RecordSecond != 0 && ratingResult.IsUnlocked == true)
                                                    {
                                                        rd.Record = rdetail.RecordSecond;
                                                        rd.RecordOld = rdetail.Record;
                                                    }
                                                    else
                                                    {
                                                        rd.Record = rdetail.Record;
                                                        rd.RecordOld = rdetail.Record;
                                                    }

                                                    rd.SupervisorRecord = rdetail.SupervisorRecord;
                                                    rd.Note = rdetail.Note;
                                                    rd.SupervisorNote = rdetail.SupervisorNote;
                                                    rd.FileAttachments = rdetail.FileAttachments.Map<FileAttachmentDTO>();
                                                    rd.FileAttachmentCount = rd.FileAttachments.Count();
                                                    //rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
                                                    rd.Id = rdetail.Id;
                                                }
                                                else
                                                {

                                                    rd.Id = Guid.NewGuid();
                                                    ResultDetail newResultDetail = new ResultDetail()
                                                    {
                                                        Id = rd.Id,
                                                        PlanKPIDetail = new PlanKPIDetail() { Id = pld.Id },
                                                        Result = ratingResult,
                                                        TargetGroupDetail = t
                                                    };
                                                    SessionManager.DoWork(session1 =>
                                                    {
                                                        session1.Save(newResultDetail);
                                                    });

                                                }
                                                if (t.TargetGroupDetailType.Id == 3 && rd.PlanKPIDetailName != null)
                                                {
                                                    List<CriterionDictionary> crds = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail != null && c.TargetGroupDetail.Id == t.Id).ToList();
                                                    try
                                                    {
                                                        rd.PlanKPIDetailNameString = crds.FirstOrDefault(c => c.Id == new Guid(rd.PlanKPIDetailName)).Name;
                                                    }
                                                    catch { }
                                                }

                                                tg.ResultDetailDTOs.Add(rd);
                                            }
                                            double totalDensity = tg.ResultDetailDTOs.Sum(tr => tr.Density);
                                            tg.ResultDetailDTOs.ForEach(r =>
                                            {
                                                r.Density = Math.Round(r.Density / totalDensity, 2);
                                                r.DensityPercent = Math.Round(r.Density * 100, 0);
                                            });

                                            #endregion
                                        }
                                        break;
                                    case (int)AgentObjectTypes.Nganh:
                                        {
                                            #region Ngành
                                            List<ProfessorCriterionPlanDTO> criterions = session.Query<ProfessorCriterion>().Where(c => c.TargetGroupDetail.Id == t.Id).Map<ProfessorCriterionPlanDTO>().ToList();
                                            foreach (ProfessorCriterionPlanDTO crdi in criterions)
                                            {
                                                PlanKPIMakingDetailDTO pld = new PlanKPIMakingDetailDTO();
                                                List<ResultDetail> staffResultDetails = session.Query<ResultDetail>().Where(r => r.PlanKPIDetail.FromProfessorCriterion != null && r.PlanKPIDetail.FromProfessorCriterion.Id == crdi.Id && r.Result.StaffRating.StaffInfo.Subject.Id == staff.StaffInfo.Subject.Id && r.IsTargetGroupRating == false && r.SupervisorRecord != 0 && r.PlanKPIDetail.PlanStaff.PlanKPI.Id == planId).ToList();
                                                ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                                rd.CriterionId = crdi.Id;
                                                rd.CriterionName = crdi.Name;
                                                rd.CriterionTypeId = crdi.CriterionType.Id;
                                                rd.Tooltip = crdi.Tooltip;
                                                List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == crdi.Id).Map<CriterionDictionaryDTO>().ToList();
                                                rd.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                rd.PlanKPIDetailId = pld.Id;
                                                rd.PlanKPIDetailName = pld.Name;

                                                double resultRecord = staffResultDetails.Count > 0 ? staffResultDetails.Average(f => f.SupervisorRecord) : 0;
                                                rd.Record = Math.Round(resultRecord, 1);
                                                rd.SupervisorRecord = Math.Round(resultRecord, 1);
                                                rd.MaxRecord = crdi.Record;
                                                rd.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };

                                                switch (crdi.CriterionType.Id)
                                                {
                                                    case 3:
                                                        {
                                                            if (pld.CurrentKPI != null)
                                                            {
                                                                Guid crdId = new Guid(pld.CurrentKPI);
                                                                CriterionDictionary crd = session.Query<CriterionDictionary>().Where(c => c.Id == crdId).FirstOrDefault();
                                                                rd.RegisterTarget = crd.Name;
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        {
                                                            rd.RegisterTarget = pld.CurrentKPI;
                                                        }
                                                        break;
                                                }
                                                rd.Density = 1;
                                                tg.ResultDetailDTOs.Add(rd);
                                            }
                                            #endregion
                                        }
                                        break;
                                }

                                AgentObject_TargetGroup ag_tg = session.Query<AgentObject_TargetGroup>().Where(r => r.TargetGroupDetailId.Id == t.Id && r.AgentObjectId.Id == agentObjectId).FirstOrDefault();
                                tg.Id = t.Id;
                                tg.Name = t.Name;
                                result.RatingResultId = ratingResult.Id;
                                tg.Density = Convert.ToDouble(ag_tg.TiTrong);

                                #region lấy trọng số của chế độ làm việc
                                if (plan.PlanType.Id == 2 && plan.ParentPlan != null) //nếu là KH học kỳ thì lấy chế độ làm việc của KH năm
                                {
                                    var parentPlanStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == plan.ParentPlan.Id && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
                                    if (parentPlanStaff.AgentObjectDetail != null && parentPlanStaff.IsWorkingModeLocked)
                                    {
                                        if (t.TargetGroupDetailType.Id == 0)
                                            tg.Density = parentPlanStaff.AgentObjectDetail.NumberOfSectionDensity;
                                        else if (t.TargetGroupDetailType.Id == 5)
                                            tg.Density = parentPlanStaff.AgentObjectDetail.ScienceResearchDensity;
                                        else if (t.TargetGroupDetailType.Id == 4)
                                            tg.Density = parentPlanStaff.AgentObjectDetail.OtherActivityDensity;
                                    }
                                }
                                else //KH năm
                                {
                                    if (planStaff.AgentObjectDetail != null && planStaff.IsWorkingModeLocked)
                                    {
                                        if (t.TargetGroupDetailType.Id == 0)
                                            tg.Density = planStaff.AgentObjectDetail.NumberOfSectionDensity;
                                        else if (t.TargetGroupDetailType.Id == 5)
                                            tg.Density = planStaff.AgentObjectDetail.ScienceResearchDensity;
                                        else if (t.TargetGroupDetailType.Id == 4)
                                            tg.Density = planStaff.AgentObjectDetail.OtherActivityDensity;
                                    }
                                }
                                #endregion

                                result.TargetGroupRatingDTOs.Add(tg);
                            }
                            try //tính điểm NCKH và HDK của giảng viên
                            {
                                //ScienceResearchData scienceResearchData = null;
                                //if (plan.PlanType.Id == 1)
                                //{
                                //    scienceResearchData = session.Query<ScienceResearchData>().FirstOrDefault(s => (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                //    && (s.StudyTerm == null || s.StudyTerm == "") && s.StudyYear == plan.StudyYear);
                                //}
                                //else
                                //{
                                //    scienceResearchData = session.Query<ScienceResearchData>().FirstOrDefault(s => (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                //    && s.StudyTerm == plan.StudyTerm && s.StudyYear == plan.StudyYear);
                                //}
                                //TotalScienceResearchHour = scienceResearchData != null ? scienceResearchData.Record : 0;
                                OtherActivityData scienceResearchData = null;
                                if (plan.PlanType.Id == 1)
                                {
                                    scienceResearchData = session.Query<OtherActivityData>().FirstOrDefault(s => (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                    && (s.StudyTerm == null || s.StudyTerm == "") && s.StudyYear == plan.StudyYear);
                                }
                                else
                                {
                                    scienceResearchData = session.Query<OtherActivityData>().FirstOrDefault(s => (s.StaffCode == staff.StaffInfo.ManageCode || s.StaffCode == staff.StaffInfo.WebUser.UserName)
                                    && s.StudyTerm == plan.StudyTerm && s.StudyYear == plan.StudyYear);
                                }
                                TotalScienceResearchHour = scienceResearchData != null ? scienceResearchData.NumberOfTime : 0;

                                double ScienceResearchConfig = session.Query<KPIConfiguration>().FirstOrDefault().ScienceResearchConfig;
                                double OtherActivityConfig = session.Query<KPIConfiguration>().FirstOrDefault().OtherActivityConfig;
                                double GiangdayConfig = session.Query<KPIConfiguration>().FirstOrDefault().GiangdayConfig;
                                if (plan.PlanType.Id == 2 && plan.ParentPlan != null) //nếu là KH học kỳ thì lấy chế độ làm việc của KH năm
                                {
                                    var parentPlanStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == plan.ParentPlan.Id && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).FirstOrDefault();
                                    //Nghiên cứu khoa học
                                    if (parentPlanStaff.AgentObjectDetail != null && parentPlanStaff.AgentObjectDetail.ScienceResearch != 0)
                                    {
                                        result.ScienceResearchRecord = Math.Round(TotalScienceResearchHour / parentPlanStaff.AgentObjectDetail.ScienceResearch * ScienceResearchConfig, 1);
                                    }
                                    else result.ScienceResearchRecord = 0;
                                    //Hoạt động khác
                                    if (parentPlanStaff.AgentObjectDetail != null && parentPlanStaff.AgentObjectDetail.OtherActivity != 0)
                                    {
                                        result.OtherActivityRecord = Math.Round(TotalOtherActivityHour / parentPlanStaff.AgentObjectDetail.OtherActivity * OtherActivityConfig, 1);
                                    }
                                    // if(parentPlanStaff.AgentObjectDetail != null && parentPlanStaff.AgentObjectDetail.OtherActivity != 0)
                                    else result.OtherActivityRecord = 0;

                                    // điểm thưởng điểm trừ
                                    if (result.DiemThuongDiemTru != null)
                                    {
                                        foreach (var dtdt in result.DiemThuongDiemTru)
                                        {
                                            result.diemthuongdiemtruRecord += dtdt.SoDiem;
                                        }
                                    }

                                    //if(parentPlanStaff.AgentObjectDetail != null && parentPlanStaff.AgentObjectDetail.NumberOfSection != 0)
                                    //{
                                    //    result.NumberOfSectionRecord = Math.Round();
                                    //}
                                }
                                else //KH năm
                                {
                                    if (planStaff.AgentObjectDetail != null && planStaff.AgentObjectDetail.ScienceResearch != 0)
                                    {
                                        result.ScienceResearchRecord = Math.Round(TotalScienceResearchHour / planStaff.AgentObjectDetail.ScienceResearch * ScienceResearchConfig, 1);
                                    }
                                    else result.ScienceResearchRecord = 0;

                                    if (planStaff.AgentObjectDetail != null && planStaff.AgentObjectDetail.OtherActivity != 0)
                                    {
                                        result.OtherActivityRecord = Math.Round(TotalOtherActivityHour / planStaff.AgentObjectDetail.OtherActivity * OtherActivityConfig, 1);
                                    }
                                    else result.OtherActivityRecord = 0;
                                }
                            }
                            catch
                            {
                                result.ScienceResearchRecord = 0;
                                result.OtherActivityRecord = 0;
                            }
                            #endregion
                            var temBonusRecords = resultDetails.Where(r => r.Result != null && r.Result.StaffRating != null && r.Result.StaffRating.Id == staff.Id && r.PlanKPIDetail == null);
                            if (temBonusRecords.Count() <= 0)
                                result.BonusRecordList.Add(new ResultDetailRatingDTO() { Id = Guid.Empty, CurrentResult = "", Record = 0 });
                            else
                            {
                                result.BonusRecordList = new List<ResultDetailRatingDTO>();
                                foreach (ResultDetail rd in temBonusRecords)
                                {
                                    ResultDetailRatingDTO rdd = new ResultDetailRatingDTO();
                                    rdd.Note = rd.Note;
                                    rdd.SupervisorNote = rd.SupervisorNote;
                                    rdd.CurrentResult = rd.CurrentResult;
                                    rdd.Record = rd.Record;
                                    rdd.FileAttachments = new List<FileAttachmentDTO>();
                                    foreach (FileAttachment fad in rd.FileAttachments)
                                    {
                                        FileAttachmentDTO fa = new FileAttachmentDTO();
                                        fa.Id = fad.Id;
                                        fa.CreationTime = fad.CreationTime;
                                        fa.Extension = fad.Extension;
                                        fa.Name = fad.Name;
                                        fa.Path = fad.Path;
                                        fa.ResultDetailId = fad.ResultDetail.Id;
                                        rdd.FileAttachments.Add(fa);
                                    }
                                    rdd.Id = rd.Id;
                                    result.BonusRecordList.Add(rdd);
                                }
                            }
                            #region Additional Result
                            int agentObjectTypeId = -1;
                            if (staff.StaffInfo.Position == null)
                            {
                                if (staff.StaffInfo.StaffType.ManageCode == "3")
                                    agentObjectTypeId = 2; //Nhân viên
                                else
                                    agentObjectTypeId = 1; //Giảng viên
                            }
                            else
                            {
                                agentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
                            }
                            if (staff.StaffInfo.AgentObjects.Count > 1)
                            {
                                AgentObject agentObj = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).FirstOrDefault();
                                agentObjectTypeId = ao.AgentObjectType.Id;
                            }
                            switch (agentObjectTypeId)
                            {
                                case (int)AgentObjectTypes.GiangVien:
                                    {
                                        #region giảng viên
                                        result.ProfessorAdditionalResultDetailDTOs = new List<ResultDetailRatingDTO>();
                                        List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();
                                        //planDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail == null && !p.IsDisable).ToList();
                                        planDetails = planStaff.PlanKPIDetails.Where(pd =>
                                                            (pd.TargetGroupDetail == null && !pd.IsDisable && ControllerHelpers.GetOriginalMethods(pd, session).Count > 0
                                                            && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd, session).Max(m => m.EndTime)) <= 0) ||
                                                            (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd, session).Max(m => m.EndTime)) <= 0) ||
                                                            (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd, session).Max(m => m.EndTime)) >= 0)))).ToList();

                                        foreach (PlanKPIDetail pld in planDetails)
                                        {
                                            ResultDetail rdetail = session.Query<ResultDetail>().FirstOrDefault(r => r.PlanKPIDetail.Id == pld.Id && r.Result.StaffRating.Id == staff.Id && r.IsTargetGroupRating == true);
                                            ResultDetailRatingDTO rd = new ResultDetailRatingDTO();
                                            rd.CriterionName = pld.TargetDetail;

                                            if (rdetail == null)
                                            {
                                                rd.PlanKPIDetail = new PlanKPIDetail() { Id = pld.Id };
                                                rd.PlanKPIDetailId = pld.Id;
                                                rd.Id = Guid.NewGuid();
                                                ResultDetail newResultDetail = new ResultDetail()
                                                {
                                                    Id = rd.Id,
                                                    PlanKPIDetail = new PlanKPIDetail() { Id = pld.Id },
                                                    Result = ratingResult,
                                                    IsTargetGroupRating = true
                                                };
                                                SessionManager.DoWork(session1 =>
                                                {
                                                    session1.Save(newResultDetail);
                                                });
                                            }
                                            else
                                            {
                                                //rd = rdetail.Map<ResultDetailRatingDTO>();
                                                rd.PreviousResult = rdetail.PreviousResult;
                                                rd.CurrentResult = rdetail.CurrentResult;
                                                rd.RegisterTarget = rdetail.RegisterTarget;
                                                rd.Record = rdetail.Record;

                                                rd.SupervisorRecord = rdetail.SupervisorRecord;
                                                rd.PlanKPIDetailId = rdetail.PlanKPIDetail.Id;
                                                rd.Id = rdetail.Id;
                                                rd.Note = rdetail.Note;
                                                rd.SupervisorNote = rdetail.SupervisorNote;
                                                rd.FileAttachments = rdetail.FileAttachments.Map<FileAttachmentDTO>();
                                                rd.FileAttachmentCount = rd.FileAttachments.Count();
                                                //rd.FileAttachments.ForEach(r => r.Name = r.Name + r.Extension);
                                            }
                                            rd.IsConfirmed = rdetail != null ? rdetail.IsConfirmed : false;
                                            result.ProfessorAdditionalResultDetailDTOs.Add(rd);
                                        }
                                        #endregion
                                    }
                                    break;
                            }
                            #endregion

                            result.RatingResultId = ratingResult.Id;
                            result.IsLocked = ratingResult.IsLocked;
                            result.IsCommitted = ratingResult.IsCommitted;
                            result.TotalRecord = ratingResult.TotalRecord;
                            result.TotalRecordSecond = ratingResult.TotalRecordSecond;
                            result.MaxBonusRecord = session.Query<KPIConfiguration>().FirstOrDefault().MaxBonusRecord;
                            result.IsFreeRating = true;
                        }
                        else
                            result.Id = Guid.Empty;
                    }



                    foreach (TargetGroupRatingDTO tg in result.TargetGroupRatingDTOs)
                    {
                        try
                        {
                            //tg.ResultDetailDTOs = tg.ResultDetailDTOs.OrderBy(pl => ControllerHelpers.GetOriginalMethods(pl.PlanKPIDetailId).Min(pld => pld.StartTime)).ThenBy(pl => ControllerHelpers.GetOriginalMethods(pl.PlanKPIDetailId).Min(pld => pld.EndTime)).ThenBy(pl => pl.PlanKPIDetailName).ToList();
                            tg.ResultDetailDTOs = tg.ResultDetailDTOs.OrderBy(pl => pl.OrderNumber).ToList();
                        }
                        catch (Exception e)
                        {

                        }
                    }


                });

                    //Tính trọng số từ stored
                    if (result.StaffDTO != null)
                    {
                        switch (result.StaffDTO.AgentObjectTypeId)
                        {
                            case 2:
                            case 3:
                            case 7:
                                {
                                    GetDensityFormatResultDetailRatings_FromSP(result);
                                }
                                break;
                        }
                    }
                }
                //}
                //    catch (Exception e)
                //    {

                //    }

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ResultDetailApiController/GetRatingResultDetail", ex);
                throw ex;
            }
        }

        [Authorize]
        [Route("")]
        public ResultDetailDTO GetClass(Guid id)
        {
            ResultDetailDTO result = new ResultDetailDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ResultDetail>().FirstOrDefault(a => a.Id == id).Map<ResultDetailDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public RatingKPIResultDTO Put(RatingKPIResultDTO obj)
        {
            SessionManager.DoWork(session =>
            {
                Staff staff = session.Query<Staff>().FirstOrDefault(s => s.Id == obj.StaffId);
                PlanStaff planStaff = session.Query<PlanStaff>().FirstOrDefault(ps => ps.Id == obj.PlanStaffId);
                PlanKPI plan = session.Query<PlanKPI>().FirstOrDefault();
                Result ratingResult = session.Query<Result>().FirstOrDefault(r => r.Id == obj.RatingResultId);
                if (obj.IsSupervisor == true)
                {
                    ratingResult.IsLocked = true;
                }
                ratingResult.Time = DateTime.Now;
                ratingResult.TotalRecord = obj.TotalSumRecord;
                //điểm tạm của nhân viên tự đánh giá
                ratingResult.TempRecord = obj.TempRecord;
                if (obj.IsAdminRating == true)
                {
                    ratingResult.TotalRecordSecond = obj.TotalRecordSecond;
                    ratingResult.TotalRecord = obj.TotalRecord;
                    ratingResult.NumberOfEditing += 1;
                }
                session.Update(ratingResult);

                switch (obj.StaffDTO.AgentObjectTypeId)
                {
                    case (int)AgentObjectTypes.GiangVien:
                        {
                            #region Giảng viên


                            foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
                            {
                                foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
                                {
                                    ResultDetail rd = new ResultDetail();
                                    rd.CurrentResult = p.CurrentResult;
                                    rd.PreviousResult = p.PreviousResult;
                                    if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
                                    {
                                        rd.RecordSecond = p.Record;
                                        rd.Record = p.RecordOld;
                                    }
                                    else
                                    {
                                        rd.Record = p.Record;
                                    }
                                    if (tg.Id.ToString() == "00000000-0000-0000-0000-000000000002")
                                    {
                                        rd.SupervisorRecord = p.Record;
                                    }
                                    rd.RegisterTarget = p.RegisterTarget;
                                    rd.SupervisorRecord = p.SupervisorRecord;
                                    rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
                                    rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                    rd.Result = ratingResult;
                                    rd.IsTargetGroupRating = false;

                                    foreach (FileAttachmentDTO f in p.FileAttachments)
                                    {
                                        FileAttachment fa = new FileAttachment();
                                        fa.Id = f.Id;
                                        fa.CreationTime = f.CreationTime;
                                        fa.Extension = f.Extension;
                                        fa.Name = f.Name;
                                        fa.Path = f.Path;
                                        fa.ResultDetail = new ResultDetail() { Id = f.ResultDetailId };
                                        rd.FileAttachments.Add(fa);
                                    }

                                    if (p.Id == Guid.Empty)
                                    {
                                        rd.Id = Guid.NewGuid();
                                        session.Save(rd);
                                    }
                                    else
                                    {
                                        rd.Id = p.Id;
                                        session.Merge(rd);
                                    }
                                }
                            }
                            if (obj.ProfessorAdditionalResultDetailDTOs != null)
                            {
                                foreach (ResultDetailRatingDTO p in obj.ProfessorAdditionalResultDetailDTOs)
                                {
                                    ResultDetail rd = new ResultDetail();
                                    rd.CurrentResult = p.CurrentResult;
                                    rd.PreviousResult = p.PreviousResult;
                                    rd.Record = p.Record;
                                    rd.RegisterTarget = p.RegisterTarget;
                                    rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
                                    rd.IsTargetGroupRating = true;
                                    rd.IsConfirmed = p.IsConfirmed;
                                    rd.Result = ratingResult;
                                    rd.Note = p.Note;
                                    rd.SupervisorNote = p.SupervisorNote;

                                    foreach (FileAttachmentDTO f in p.FileAttachments)
                                    {
                                        FileAttachment fa = new FileAttachment();
                                        fa.Id = f.Id;
                                        fa.CreationTime = f.CreationTime;
                                        fa.Extension = f.Extension;
                                        fa.Name = f.Name;
                                        fa.Path = f.Path;
                                        fa.ResultDetail = new ResultDetail() { Id = f.ResultDetailId };
                                        rd.FileAttachments.Add(fa);
                                    }

                                    if (p.Id == Guid.Empty)
                                    {
                                        rd.Id = Guid.NewGuid();
                                        session.Save(rd);
                                    }
                                    else
                                    {
                                        rd.Id = p.Id;
                                        session.Merge(rd);
                                    }
                                }
                            }
                            #endregion
                        }
                        break;
                    case (int)AgentObjectTypes.NhanVien:
                        {
                            #region Nhân viên


                            foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
                            {
                                foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
                                {
                                    ResultDetail rd = new ResultDetail();
                                    rd.Note = p.Note;
                                    rd.SupervisorNote = p.SupervisorNote;
                                    rd.PreviousResult = p.PreviousResult;
                                    rd.CurrentResult = p.CurrentResult;
                                    rd.MaxRecord = Convert.ToInt32(p.MaxRecord);
                                    if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
                                    {
                                        rd.RecordSecond = p.Record;
                                        rd.Record = p.RecordOld;
                                    }
                                    else
                                    {
                                        rd.Record = p.Record;
                                    }
                                    rd.RegisterTarget = p.RegisterTarget;
                                    rd.SupervisorRecord = p.SupervisorRecord;
                                    rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
                                    rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                    rd.FileAttachments = new List<FileAttachment>();
                                    if (p.FileAttachments != null)
                                    {
                                        foreach (FileAttachmentDTO fad in p.FileAttachments)
                                        {
                                            FileAttachment fa = new FileAttachment();
                                            fa.Id = fad.Id;
                                            fa.CreationTime = fad.CreationTime;
                                            fa.Extension = fad.Extension;
                                            fa.Name = fad.Name;
                                            fa.Path = fad.Path;
                                            fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
                                            rd.FileAttachments.Add(fa);
                                        }
                                    }
                                    rd.Result = ratingResult;
                                    if (p.Id == Guid.Empty)
                                    {
                                        rd.Id = Guid.NewGuid();
                                        session.Save(rd);
                                    }
                                    else
                                    {
                                        session.Merge(rd);
                                    }
                                }
                            }
                            #endregion
                        }
                        break;
                    case (int)AgentObjectTypes.PhongBan:
                        {
                            #region Trưởng phòng


                            foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
                            {
                                foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
                                {
                                    ResultDetail rd = new ResultDetail();

                                    rd.Id = p.Id;
                                    rd.Note = p.Note;
                                    rd.SupervisorNote = p.SupervisorNote;
                                    rd.PreviousResult = p.PreviousResult;
                                    rd.CurrentResult = p.CurrentResult;
                                    rd.MaxRecord = Convert.ToInt32(p.MaxRecord);
                                    if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
                                    {
                                        rd.RecordSecond = p.Record;
                                        rd.Record = p.RecordOld;
                                    }
                                    else
                                    {
                                        rd.Record = p.Record;
                                    }
                                    rd.RegisterTarget = p.RegisterTarget;
                                    rd.SupervisorRecord = p.SupervisorRecord;
                                    rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
                                    rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                    rd.FileAttachments = new List<FileAttachment>();
                                    if (p.FileAttachments != null)
                                    {
                                        foreach (FileAttachmentDTO fad in p.FileAttachments)
                                        {
                                            FileAttachment fa = new FileAttachment();
                                            fa.Id = fad.Id;
                                            fa.CreationTime = fad.CreationTime;
                                            fa.Extension = fad.Extension;
                                            fa.Name = fad.Name;
                                            fa.Path = fad.Path;
                                            fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
                                            rd.FileAttachments.Add(fa);
                                        }
                                    }
                                    rd.Result = ratingResult;
                                    if (p.Id == Guid.Empty)
                                    {
                                        rd.Id = Guid.NewGuid();
                                        session.Save(rd);
                                    }
                                    else
                                    {
                                        session.Merge(rd);
                                    }
                                }
                            }
                            #endregion
                        }
                        break;
                    case (int)AgentObjectTypes.PhoPhongBan:
                        {
                            #region Phó phòng


                            foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
                            {
                                foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
                                {
                                    ResultDetail rd = new ResultDetail();

                                    rd.Id = p.Id;
                                    rd.Note = p.Note;
                                    rd.SupervisorNote = p.SupervisorNote;
                                    rd.PreviousResult = p.PreviousResult;
                                    rd.CurrentResult = p.CurrentResult;
                                    rd.MaxRecord = Convert.ToInt32(p.MaxRecord);
                                    if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
                                    {
                                        rd.RecordSecond = p.Record;
                                        rd.Record = p.RecordOld;
                                    }
                                    else
                                    {
                                        rd.Record = p.Record;
                                    }
                                    rd.RegisterTarget = p.RegisterTarget;
                                    rd.SupervisorRecord = p.SupervisorRecord;
                                    rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
                                    rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                    rd.FileAttachments = new List<FileAttachment>();
                                    if (p.FileAttachments != null)
                                    {
                                        foreach (FileAttachmentDTO fad in p.FileAttachments)
                                        {
                                            FileAttachment fa = new FileAttachment();
                                            fa.Id = fad.Id;
                                            fa.CreationTime = fad.CreationTime;
                                            fa.Extension = fad.Extension;
                                            fa.Name = fad.Name;
                                            fa.Path = fad.Path;
                                            fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
                                            rd.FileAttachments.Add(fa);
                                        }
                                    }
                                    rd.Result = ratingResult;
                                    if (p.Id == Guid.Empty)
                                    {
                                        rd.Id = Guid.NewGuid();
                                        session.Save(rd);
                                    }
                                    else
                                    {
                                        session.Merge(rd);
                                    }
                                }
                            }
                            #endregion
                        }
                        break;
                    case (int)AgentObjectTypes.Khoa:
                        {
                            #region Trưởng khoa

                            foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
                            {
                                foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
                                {
                                    ResultDetail rd = new ResultDetail();

                                    rd.Id = p.Id;
                                    rd.Note = p.Note;
                                    rd.SupervisorNote = p.SupervisorNote;
                                    rd.PreviousResult = p.PreviousResult;
                                    rd.CurrentResult = p.CurrentResult;
                                    rd.MaxRecord = Convert.ToInt32(p.MaxRecord);
                                    if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
                                    {
                                        rd.RecordSecond = p.Record;
                                        rd.Record = p.RecordOld;
                                    }
                                    else
                                    {
                                        rd.Record = p.Record;
                                    }
                                    rd.RegisterTarget = p.RegisterTarget;
                                    rd.SupervisorRecord = p.SupervisorRecord;
                                    rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
                                    rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                    rd.FileAttachments = new List<FileAttachment>();
                                    if (p.FileAttachments != null)
                                    {
                                        foreach (FileAttachmentDTO fad in p.FileAttachments)
                                        {
                                            FileAttachment fa = new FileAttachment();
                                            fa.Id = fad.Id;
                                            fa.CreationTime = fad.CreationTime;
                                            fa.Extension = fad.Extension;
                                            fa.Name = fad.Name;
                                            fa.Path = fad.Path;
                                            fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
                                            rd.FileAttachments.Add(fa);
                                        }
                                    }
                                    rd.Result = ratingResult;
                                    if (p.Id == Guid.Empty)
                                    {
                                        rd.Id = Guid.NewGuid();
                                        session.Save(rd);
                                    }
                                    else
                                    {
                                        session.Merge(rd);
                                    }
                                }
                            }
                            #endregion
                        }
                        break;
                    case (int)AgentObjectTypes.PhoKhoa:
                        {
                            #region Phó khoa


                            foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
                            {
                                foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
                                {
                                    ResultDetail rd = new ResultDetail();

                                    rd.Id = p.Id;
                                    rd.Note = p.Note;
                                    rd.SupervisorNote = p.SupervisorNote;
                                    rd.PreviousResult = p.PreviousResult;
                                    rd.CurrentResult = p.CurrentResult;
                                    rd.MaxRecord = Convert.ToInt32(p.MaxRecord);
                                    if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
                                    {
                                        rd.RecordSecond = p.Record;
                                        rd.Record = p.RecordOld;
                                    }
                                    else
                                    {
                                        rd.Record = p.Record;
                                    }
                                    rd.RegisterTarget = p.RegisterTarget;
                                    rd.SupervisorRecord = p.SupervisorRecord;
                                    rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
                                    rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                    rd.FileAttachments = new List<FileAttachment>();
                                    if (p.FileAttachments != null)
                                    {
                                        foreach (FileAttachmentDTO fad in p.FileAttachments)
                                        {
                                            FileAttachment fa = new FileAttachment();
                                            fa.Id = fad.Id;
                                            fa.CreationTime = fad.CreationTime;
                                            fa.Extension = fad.Extension;
                                            fa.Name = fad.Name;
                                            fa.Path = fad.Path;
                                            fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
                                            rd.FileAttachments.Add(fa);
                                        }
                                    }
                                    rd.Result = ratingResult;
                                    if (p.Id == Guid.Empty)
                                    {
                                        rd.Id = Guid.NewGuid();
                                        session.Save(rd);
                                    }
                                    else
                                    {
                                        session.Merge(rd);
                                    }
                                }
                            }
                            #endregion
                        }
                        break;
                    case (int)AgentObjectTypes.HieuTruong:
                        {
                            #region Hiệu trưởng


                            foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
                            {
                                foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
                                {
                                    ResultDetail rd = new ResultDetail();

                                    rd.Id = p.Id;
                                    rd.Note = p.Note;
                                    rd.SupervisorNote = p.SupervisorNote;
                                    rd.PreviousResult = p.PreviousResult;
                                    rd.CurrentResult = p.CurrentResult;
                                    if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
                                    {
                                        rd.RecordSecond = p.Record;
                                        rd.Record = p.RecordOld;
                                    }
                                    else
                                    {
                                        rd.Record = p.Record;
                                    }
                                    rd.RegisterTarget = p.RegisterTarget;
                                    rd.SupervisorRecord = p.SupervisorRecord;
                                    rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
                                    rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                    rd.FileAttachments = new List<FileAttachment>();
                                    if (p.FileAttachments != null)
                                    {
                                        foreach (FileAttachmentDTO fad in p.FileAttachments)
                                        {
                                            FileAttachment fa = new FileAttachment();
                                            fa.Id = fad.Id;
                                            fa.CreationTime = fad.CreationTime;
                                            fa.Extension = fad.Extension;
                                            fa.Name = fad.Name;
                                            fa.Path = fad.Path;
                                            fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
                                            rd.FileAttachments.Add(fa);
                                        }
                                    }
                                    rd.Result = ratingResult;
                                    if (p.Id == Guid.Empty)
                                    {
                                        rd.Id = Guid.NewGuid();
                                        session.Save(rd);
                                    }
                                    else
                                    {
                                        session.Merge(rd);
                                    }
                                }
                            }
                            #endregion
                        }
                        break;
                    case (int)AgentObjectTypes.PhoHieuTruong:
                        {
                            #region Phó Hiệu trưởng


                            foreach (TargetGroupRatingDTO tg in obj.TargetGroupRatingDTOs)
                            {
                                foreach (ResultDetailRatingDTO p in tg.ResultDetailDTOs)
                                {
                                    ResultDetail rd = new ResultDetail();

                                    rd.Id = p.Id;
                                    rd.Note = p.Note;
                                    rd.SupervisorNote = p.SupervisorNote;
                                    rd.PreviousResult = p.PreviousResult;
                                    rd.CurrentResult = p.CurrentResult;
                                    if (ratingResult.IsUnlocked == true && p.Record != p.RecordOld)
                                    {
                                        rd.RecordSecond = p.Record;
                                        rd.Record = p.RecordOld;
                                    }
                                    else
                                    {
                                        rd.Record = p.Record;
                                    }
                                    rd.RegisterTarget = p.RegisterTarget;
                                    rd.SupervisorRecord = p.SupervisorRecord;
                                    rd.PlanKPIDetail = new PlanKPIDetail() { Id = p.PlanKPIDetailId };
                                    rd.TargetGroupDetail = new TargetGroupDetail() { Id = tg.Id };
                                    rd.FileAttachments = new List<FileAttachment>();
                                    if (p.FileAttachments != null)
                                    {
                                        foreach (FileAttachmentDTO fad in p.FileAttachments)
                                        {
                                            FileAttachment fa = new FileAttachment();
                                            fa.Id = fad.Id;
                                            fa.CreationTime = fad.CreationTime;
                                            fa.Extension = fad.Extension;
                                            fa.Name = fad.Name;
                                            fa.Path = fad.Path;
                                            fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
                                            rd.FileAttachments.Add(fa);
                                        }
                                    }
                                    rd.Result = ratingResult;
                                    if (p.Id == Guid.Empty)
                                    {
                                        rd.Id = Guid.NewGuid();
                                        session.Save(rd);
                                    }
                                    else
                                    {
                                        session.Merge(rd);
                                    }
                                }
                            }
                            #endregion
                        }
                        break;
                }
                if (obj.IsSupervisor)
                {
                    foreach (ResultDetailRatingDTO p in obj.BonusRecordList)
                    {
                        ResultDetail rd = new ResultDetail();

                        rd.Id = p.Id;
                        rd.Note = p.Note;
                        rd.SupervisorNote = p.SupervisorNote;
                        rd.Record = p.Record;
                        rd.CurrentResult = p.CurrentResult;
                        rd.FileAttachments = new List<FileAttachment>();
                        if (p.FileAttachments != null)
                        {
                            foreach (FileAttachmentDTO fad in p.FileAttachments)
                            {
                                FileAttachment fa = new FileAttachment();
                                fa.Id = fad.Id;
                                fa.CreationTime = fad.CreationTime;
                                fa.Extension = fad.Extension;
                                fa.Name = fad.Name;
                                fa.Path = fad.Path;
                                fa.ResultDetail = new ResultDetail() { Id = fad.ResultDetailId };
                                rd.FileAttachments.Add(fa);
                            }
                        }

                        rd.Result = ratingResult;
                        if (p.Id == Guid.Empty)
                        {
                            rd.Id = Guid.NewGuid();
                            session.Save(rd);
                        }
                        else
                        {
                            session.Merge(rd);
                        }
                    }
                }
            });
            //gọi store để cập nhật Grade và bảng KPI_Result_TargetGroupDetail
            DataClassHelper.SaveRatingResult(obj.RatingResultId);
            return obj;
        }

        [Authorize]
        [Route("")]
        public RatingKPIResultDTO PutLock(RatingKPIResultDTO obj)
        {
            SessionManager.DoWork(session =>
            {
                RatingKPIResultDTO result = Put(obj);
                Result ratingResult = session.Query<Result>().FirstOrDefault(r => r.Id == obj.RatingResultId);
                ratingResult.IsCommitted = true;
                session.Update(ratingResult);
            });
            return obj;
        }

        [Authorize]
        [Route("")]
        public int PutActivity(PlanKPIMakingDetailDTO obj)
        {
            int result = 0;
            SessionManager.DoWork(session =>
            {
                PlanKPIDetail pdetail = session.Query<PlanKPIDetail>().Where(p => p.Id == obj.Id).FirstOrDefault();
                if (obj.ActivityIds != null)
                {
                    foreach (Guid did in obj.ActivityIds)
                    {
                        //Kiểm tra hoạt động hiện tại, nếu chưa có trong DB thì thêm mới
                        bool check = pdetail.ProfessorOtherActivities.Any(p => p.CriterionDictionary.Id == did);
                        if (!check)
                        {
                            ProfessorOtherActivity pa = new ProfessorOtherActivity()
                            {
                                Id = Guid.NewGuid(),
                                CriterionDictionary = new CriterionDictionary() { Id = did },
                                IsRating = 1,
                                PlanKPIDetail = new PlanKPIDetail() { Id = pdetail.Id }
                            };
                            session.Save(pa);
                        }
                    }
                }
                if (obj.ScienceResearchIds != null)
                {
                    foreach (Guid did in obj.ScienceResearchIds)
                    {
                        //Kiểm tra hoạt động hiện tại, nếu chưa có trong DB thì thêm mới
                        bool check = pdetail.ScienceResearches.Any(p => p.CriterionDictionary.Id == did);
                        if (!check)
                        {
                            ScienceResearch sr = new ScienceResearch()
                            {
                                Id = Guid.NewGuid(),
                                CriterionDictionary = new CriterionDictionary() { Id = did },
                                IsRating = 1,
                                PlanKPIDetail = new PlanKPIDetail() { Id = pdetail.Id }
                            };
                            session.Save(sr);
                        }
                    }
                }
                result = 1;
            });
            return result;
        }

        public RatingManage ParseRatingManage(Guid DepartmentId, Guid PlanKPIId, Guid StaffId, DateTime RatingStartTime, DateTime RatingEndTime)
        {
            RatingManage rm = new RatingManage();
            rm.Id = Guid.NewGuid();
            rm.Department = new Department() { Id = DepartmentId };
            rm.PlanKPI = new PlanKPI() { Id = PlanKPIId };
            rm.Staff = new Staff() { Id = StaffId };
            rm.RatingStartTime = RatingStartTime.ToLocalTime();
            rm.RatingEndTime = RatingEndTime.ToLocalTime();
            return rm;
        }

        [Authorize]
        [Route("")]
        public int PutUnlockRating(UnlockRatingDTO obj)
        {
            int result = 0;
            try
            {
                SessionManager.DoWork(session =>
                {
                    //Kiểm tra ngày tháng null
                    if (obj.RatingStartTime.Year == 1 || obj.RatingEndTime.Year == 1)
                    {
                        result = 2;
                    }
                    else
                    {
                        //Tất cả người trong đơn vị
                        if (obj.IsDepartment == true)
                        {
                            Department dept = session.Query<Department>().Where(d => d.Id == obj.DepartmentId).FirstOrDefault();
                            if (dept != null)
                            {
                                switch (dept.DepartmentType)
                                {
                                    //Phòng, Khoa
                                    case 1:
                                    case 4:
                                        {
                                            IEnumerable<Guid> staffIds = session.Query<Staff>().Where(s => s.Department.Id == dept.Id && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null).ToList().Select(s => s.Id);
                                            foreach (Guid s in staffIds)
                                            {
                                                //Xóa những dòng đã tồn tại
                                                string deleteStaff = string.Format("DELETE FROM {0} WHERE StaffId='{1}'", "KPI_RatingManage", s);
                                                session.CreateSQLQuery(deleteStaff).ExecuteUpdate();

                                                RatingManage rm = ParseRatingManage(dept.Id, obj.PlanKPIId, s, obj.RatingStartTime, obj.RatingEndTime);
                                                session.Save(rm);
                                            }
                                        }
                                        break;
                                    //Bộ môn
                                    case 3:
                                        {
                                            IEnumerable<Guid> staffIds = session.Query<Staff>().Where(s => s.StaffInfo.Subject.Id == dept.Id && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null).ToList().Select(s => s.Id);
                                            foreach (Guid s in staffIds)
                                            {
                                                //Xóa những dòng đã tồn tại
                                                string deleteStaff = string.Format("DELETE FROM {0} WHERE StaffId='{1}'", "KPI_RatingManage", s);
                                                session.CreateSQLQuery(deleteStaff).ExecuteUpdate();

                                                RatingManage rm = ParseRatingManage(dept.Id, obj.PlanKPIId, s, obj.RatingStartTime, obj.RatingEndTime);
                                                session.Save(rm);
                                            }
                                        }
                                        break;

                                }
                            }
                        }

                        //Những người được chọn
                        else
                        {
                            foreach (Guid s in obj.StaffIds)
                            {
                                //Xóa những dòng đã tồn tại
                                string deleteStaff = string.Format("DELETE FROM {0} WHERE StaffId='{1}'", "KPI_RatingManage", s);
                                session.CreateSQLQuery(deleteStaff).ExecuteUpdate();

                                RatingManage rm = ParseRatingManage(obj.DepartmentId, obj.PlanKPIId, s, obj.RatingStartTime, obj.RatingEndTime);
                                session.Save(rm);
                            }
                        }
                        result = 1;
                    }
                });
            }
            catch (Exception e)
            {
                result = 0;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public bool GetCheckUnlockable(Guid planId)
        {
            bool result = false;
            try
            {
                SessionManager.DoWork(session =>
                {
                    Staff staff = ControllerHelpers.GetCurrentStaff(session);
                    DateTime currentDate = DateTime.Now;
                    if (staff != null)
                    {
                        result = session.Query<RatingManage>().Any(r => r.PlanKPI.Id == planId && r.Staff.Id == staff.Id && r.RatingStartTime <= currentDate && r.RatingEndTime >= currentDate);
                    }
                });
            }
            catch (Exception e)
            {
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public Guid GetUnlock(Guid id)
        {
            Guid result = Guid.Empty;
            SessionManager.DoWork(session =>
            {
                Result rd = session.Query<Result>().FirstOrDefault(a => a.Id == id);
                rd.Time = DateTime.Now;
                rd.IsLocked = false;
                rd.IsUnlocked = true;
                session.Update(rd);
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int PutEditRecord(ResultDTO obj)
        {
            int result = 0;
            SessionManager.DoWork(session =>
            {
                Result rd = session.Query<Result>().FirstOrDefault(a => a.Id == obj.Id);
                if (rd.IsUnlocked == false)
                {
                    result = 2;
                }
                else
                {
                    rd.Time = DateTime.Now;
                    rd.TotalRecordSecond = obj.TotalRecordSecondNumber;
                    session.Update(rd);
                    result = 1;
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public ResultDetail Delete(ResultDetail obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
        }
        [Authorize]
        [Route("")]
        public IEnumerable<OtherActivityDataDTO> GetListOtherDetail(string ManageCode)
        {
            List<OtherActivityDataDTO> result = new List<OtherActivityDataDTO>();
            try
            {
                return DataClassHelper.DanhSachHoatDongKhac(ManageCode);

            }
            catch (Exception ex)
            {
                Helper.ErrorLog("Api/resultDetailApi/GetListOtherDetail", ex);
                throw ex;
            }
            return result;
        }

        #region OtherFunctions
        public List<ResultDetailRatingDTO> GetDensityFormatResultDetailRatings(List<ResultDetailRatingDTO> resultDetails, int targetTypeId, Guid staffId, NHibernate.ISession session)
        {
            double totalRealDensity = 0;
            foreach (ResultDetailRatingDTO rdr in resultDetails)
            {
                //Lấy tỷ trọng đc phân công
                double staffDensity = 0;
                //targetTypeId=3: Dạng các hoạt động khác chỉ co 1 và 1 kế hoạch
                if (targetTypeId == 3)
                    staffDensity = 100;
                else
                {
                    //staffDensity = session.Query<PlanDetailSubStaff>().Where(p => p.PlanKPIDetail.Id == rdr.PlanKPIDetailId && p.Staff.Id == staffId).Select(p => p.Density).FirstOrDefault();

                    //kế hoạch không nhận từ cấp trên
                    //if (staffDensity == 0)
                    //    staffDensity = session.Query<PlanKPIDetail>().FirstOrDefault(a => a.Id == rdr.PlanKPIDetailId).MaxRecord;

                    PlanDetailSubStaff substaff = session.Query<PlanDetailSubStaff>().Where(p => p.PlanKPIDetail.Id == rdr.PlanKPIDetailId && p.Staff.Id == staffId).FirstOrDefault();
                    if (substaff != null)
                        staffDensity = substaff.Density;
                    else //kế hoạch không nhận từ cấp trên
                        staffDensity = session.Query<PlanKPIDetail>().FirstOrDefault(a => a.Id == rdr.PlanKPIDetailId).MaxRecord;
                }
                //Tỷ trọng của cá nhân đối với công việc
                //double staffRealDensity = staffDensity / rdr.Density;
                //rdr.StaffRealDensity = staffRealDensity;
                //totalRealDensity += staffRealDensity;
                double staffRealDensity = 0;
                if (rdr.CriterionId != Guid.Empty)
                {
                    staffRealDensity = Math.Round((staffDensity * rdr.Density) / 100, 0);
                }
                else
                {
                    staffRealDensity = staffDensity;
                }
                rdr.StaffRealDensity = staffRealDensity;
                totalRealDensity += staffRealDensity;
            }
            resultDetails.ForEach(r =>
            {
                r.Density = Math.Round(r.StaffRealDensity / totalRealDensity, 2);
                r.DensityPercent = Math.Round(r.Density * 100, 0);
            });
            return resultDetails;
        }

        private static void GetDensityFormatResultDetailRatings_FromSP(RatingKPIResultDTO result)
        {
            SessionManager.DoWork(session =>
            {
                var resultId = session.Query<Result>().FirstOrDefault(q => q.Id == result.RatingResultId).Id;
                List<Dictionary<string, object>> resultDetailList = DataClassHelper.UpdateDensityResult(resultId);
                foreach (var tg in result.TargetGroupRatingDTOs)
                {
                    foreach (var rd in tg.ResultDetailDTOs)
                    {
                        //var resultDetail = session.Query<ResultDetail>().FirstOrDefault(q => q.Id == rd.Id);
                        //if (resultDetail != null)
                        //{
                        //    //rd.DensityPercent = resultDetail.DensityResult;
                        //}
                        foreach (var resultDetail in resultDetailList)
                        {
                            if (rd.Id == new Guid(resultDetail["ID"].ToString()))
                            {
                                rd.DensityPercent = Convert.ToDouble(resultDetail["DensityResult"]);
                                rd.Density = rd.DensityPercent / 100;
                            }
                        }
                    }
                }
            });
        }
        #endregion

    }
}
