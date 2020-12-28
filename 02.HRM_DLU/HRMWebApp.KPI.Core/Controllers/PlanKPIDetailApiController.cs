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
using HRMWebApp.KPI.Core.DTO.PlanMakingDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Globalization;
using Newtonsoft.Json;
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;
using HRMWebApp.KPI.Core.Helpers;
using System.Runtime.Caching;
using System.Diagnostics;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class PlanKPIDetailApiController : ApiController
    {


        public string Put(PlanKPIMakingDetailDTO obj)
        {
            string result = "0";
            PlanKPIDetail pdetail = new PlanKPIDetail();
            //1: Thành công
            //2: Thất bại
            //3: Đơn vị chủ trì trùng với đơn vị phối hợp

            //Xóa các kế hoạch chi tiết không tồn tại
            //try
            //{
            SessionManager.DoWork(session =>
            {

                Staff staff = session.Query<Staff>().SingleOrDefault(s => s.Id == obj.StaffId);
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                PlanStaff planStaff = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Id == obj.PlanStaffId);
                if (planStaff == null)
                {
                    planStaff = new PlanStaff();
                    planStaff.Id = Guid.NewGuid();
                    planStaff.PlanKPI = new PlanKPI() { Id = obj.PlanId };
                    planStaff.IsLocked = false;
                    planStaff.ModifiedTime = DateTime.Now;
                    planStaff.Staff = staff;
                    session.Save(planStaff);
                }
                else
                {
                    planStaff.ModifiedTime = DateTime.Now;
                    session.Update(planStaff);
                }
                TargetGroupDetail tg = session.Query<TargetGroupDetail>().Where(t => t.Id == obj.TargetGroupDetail.Id).SingleOrDefault();
                int agentObjectTypeId = -1;


                if (staff != null)
                {

                    agentObjectTypeId = obj.AgentObjectTypeId;

                    //if (staff.StaffInfo.Position == null)
                    //{
                    //    if (staff.StaffInfo.StaffType.ManageCode == "3")
                    //        agentObjectTypeId = 2; //Nhân viên
                    //    else
                    //        agentObjectTypeId = 1; //Giảng viên
                    //}
                    //else
                    //{
                    //    if (!applicationUser.IsKPIs)
                    //        agentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
                    //    else
                    //        agentObjectTypeId = Convert.ToInt32(applicationUser.AgentObjectTypeId);
                    //}

                    //if (!SessionHelper.Data<bool>(SessionKey.IsKPIs) && staff.StaffInfo.AgentObjects.Count > 1)
                    //{
                    //    AgentObject ao = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).SingleOrDefault();
                    //    if (ao != null)
                    //    {
                    //        agentObjectTypeId = ao.AgentObjectType.Id;
                    //        //result.StaffDTO.AgentObjectTypeId = AgentObjectTypeId;
                    //    }
                    //}

                }
                else
                    agentObjectTypeId = 3;

                switch (agentObjectTypeId)
                {
                    case 1:
                        {
                            if (obj.Id == Guid.Empty)
                            {
                                if (tg.TargetGroupDetailType.Id != 4 && tg.TargetGroupDetailType.Id != 5)
                                {
                                    pdetail = ControllerHelpers.ParsePlanKPIDetail(obj, null, planStaff, new TargetGroupPlanMakingDTO() { TargetGroupId = obj.TargetGroupDetail.Id }, true);
                                    if (pdetail.SubDepartments.Any(sd => sd.Id == pdetail.LeadDepartment.Id) == true)
                                    {
                                        result = "3";
                                    }
                                    //pdetail.MeasureUnit = new MeasureUnit() { Id = obj.MeasureUnitDTO.Id };
                                    if (result != "3")
                                    {
                                        session.Save(pdetail);
                                        result = pdetail.Id.ToString();
                                    }
                                }
                                else
                                {
                                    pdetail = ControllerHelpers.ParseProfessorPlanKPIDetail(obj, null, planStaff, new TargetGroupPlanMakingDTO() { TargetGroupId = obj.TargetGroupDetail.Id }, true, session);
                                    session.Save(pdetail);
                                    result = pdetail.Id.ToString();
                                }

                            }
                            else
                            {
                                PlanKPIDetail pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == obj.Id);
                                if (pld != null)
                                {
                                    if (tg.TargetGroupDetailType.Id != 4 && tg.TargetGroupDetailType.Id != 5)
                                    {
                                        pdetail = ControllerHelpers.ParsePlanKPIDetail(obj, pld, planStaff, new TargetGroupPlanMakingDTO() { TargetGroupId = obj.TargetGroupDetail.Id }, false);
                                        if (pdetail.SubDepartments.Any(sd => sd.Id == pdetail.LeadDepartment.Id) == true)
                                        {
                                            result = "3";
                                        }
                                        if (result != "3")
                                        {
                                            session.Merge(pdetail);
                                        }
                                    }
                                    else
                                    {
                                        pdetail = ControllerHelpers.ParseProfessorPlanKPIDetail(obj, pld, planStaff, new TargetGroupPlanMakingDTO() { TargetGroupId = obj.TargetGroupDetail.Id }, false, session);
                                        session.Merge(pdetail);
                                    }

                                }
                                result = pld.Id.ToString();
                            }
                        }
                        break;
                    default:
                        {
                            if (obj.Id == Guid.Empty)
                            {
                                pdetail = ControllerHelpers.ParsePlanKPIDetail(obj, null, planStaff, new TargetGroupPlanMakingDTO() { TargetGroupId = obj.TargetGroupDetail.Id }, true);
                                if (pdetail.SubDepartments.Any(sd => sd.Id == pdetail.LeadDepartment.Id) == true)
                                {
                                    result = "3";
                                }
                                if (result != "3")
                                {
                                    session.Save(pdetail);
                                    result = pdetail.Id.ToString();
                                }
                            }
                            else
                            {
                                PlanKPIDetail pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == obj.Id);
                                if (pld != null)
                                {

                                    pdetail = ControllerHelpers.ParsePlanKPIDetail(obj, pld, planStaff, new TargetGroupPlanMakingDTO() { TargetGroupId = obj.TargetGroupDetail.Id }, false);
                                    if (pdetail.SubDepartments.Any(sd => sd.Id == pdetail.LeadDepartment.Id) == true)
                                    {
                                        result = "3";
                                    }
                                    if (result != "3")
                                    {
                                        session.Merge(pdetail);
                                    }
                                }
                                result = pld.Id.ToString();
                            }
                        }
                        break;
                }
                ControllerHelpers.UpdatePlanDetailDic(pdetail, 1, session);
            });
            //}
            //catch (Exception e)
            //{
            //    return "2";
            //}


            return result;
        }

        public PlanDetailMakingDTO GetList(Guid planId, Guid agentObjectId, Guid NormalStaffId, int userRole, int isSupervisor)
        {
            Guid DepartmentId = Guid.Empty;
            PlanDetailMakingDTO result = new PlanDetailMakingDTO();

            //Mặc định kế hoạch đã khóa (vô hiệu hóa nút khóa)
            result.IsDisable = true;

            string logMessage = "";
            MemoryCacher cacher = new MemoryCacher();
            var watch = Stopwatch.StartNew();
            SessionManager.DoWork(session =>
            {
                if (AuthenticationHelper.IsAllowPlanMaking(new Guid(HttpContext.Current.User.Identity.GetUserId()), NormalStaffId != Guid.Empty ? NormalStaffId : new Guid(HttpContext.Current.User.Identity.GetUserId())))
                {
                    //Guid staffId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id); //SessionHelper.Data<Guid>(SessionKey.ThongTinNhanVien);
                    //Staff staff = session.Query<Staff>().SingleOrDefault(s => s.Id == staffId);
                    //Preparing data
                    PlanKPI plan = session.Query<PlanKPI>().Where(p => p.Id == planId).FirstOrDefault();
                    Staff staff = ControllerHelpers.GetCurrentStaff(session);
                    logMessage = "(PlanId: " + planId + " - planName: "+ plan.Name+ ") " + "(firstStaffId: " + staff.Id + " - firstStaffName: " + staff.StaffProfile.Name + ")";
                    List<Criterion> temdepartmentPlanCriterions = new List<Criterion>();
                    ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                    List<PlanKPIDetail> newPlanDetails = new List<PlanKPIDetail>();

                    if (isSupervisor == 1 && NormalStaffId != Guid.Empty)
                    {
                        result.Supervisor = session.Query<Staff>().SingleOrDefault(s => s.Id == staff.Id).Map<StaffDTO>();
                        result.IsSupervisor = true;
                        staff = session.Query<Staff>().SingleOrDefault(s => s.Id == NormalStaffId);
                    }
                    logMessage += "(normalStaffId: " + staff.Id + " - fnormalStaffName: " + staff.StaffProfile.Name + ")";

                    PlanKPI parentPlan = plan.ParentPlan != null ? plan.ParentPlan : null;
                    result.StartPlanTime = plan.StartTime;
                    result.EndPlanTime = plan.EndTime;
                    if (plan != null)
                    {
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
                            result.StaffDTO.Id = staff.Id;
                            result.StaffDTO.Department = staff.Department != null ? staff.Department.Map<DepartmentDTO>() : null;
                        }


                        AgentObject agentObject = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).SingleOrDefault();
                        if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null)
                        {
                            result.StaffDTO.AgentObjectTypeId = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.AgentObjectType.Id : (staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1);
                            if (staff.StaffInfo.Subject != null)
                                DepartmentId = staff.StaffInfo.Subject.Id;
                            else
                                DepartmentId = staff.Department.Id;
                        }
                        else
                        {
                            result.StaffDTO.AgentObjectTypeId = agentObject.AgentObjectType.Id;
                            SubPosition sposition = staff.StaffInfo.SubPositions.Where(s => s.Position.AgentObjectType.Id == agentObject.AgentObjectType.Id).Select(s => s).FirstOrDefault();
                            if (sposition != null)
                                DepartmentId = sposition.Department.Id;
                        }
                        PlanStaff planStaff = null;
                        List<TargetGroupDetail> targets = session.Query<TargetGroupDetail>().Where(t => t.AgentObjects.Any(ag => ag.Id == agentObjectId)).OrderBy(t => t.OrderNumber).ToList();
                        result.TargetGroupPlanMakings = new List<TargetGroupPlanMakingDTO>();
                        #region Original Plan
                        int AgentObjectTypeId = -1;
                        if (userRole == 1)
                        {
                            AgentObjectTypeId = 4;
                            //planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.WebGroupId == new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId)).FirstOrDefault();
                            planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && (ps.WebGroupId == new Guid("00000000-0000-0000-0000-000000000001") || ps.WebGroupId == new Guid("00000000-0000-0000-0000-000000000002") || ps.WebGroupId == new Guid("00000000-0000-0000-0000-000000000003"))).FirstOrDefault();
                            if (planStaff == null)
                            {
                                planStaff = new PlanStaff();
                                planStaff.Id = Guid.NewGuid();
                                planStaff.PlanKPI = new PlanKPI() { Id = planId };
                                planStaff.Staff = new Staff() { Id = staff.Id };
                                planStaff.ModifiedTime = DateTime.Now;
                                planStaff.WebGroupId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId);
                                session.Save(planStaff);
                            }
                            //result.IsLocked = planStaff != null ? planStaff.IsLocked : false;
                        }
                        else
                        {
                            if (agentObject == null)
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
                                    if (!applicationUser.IsKPIs)
                                        AgentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
                                    else
                                        AgentObjectTypeId = Convert.ToInt32(applicationUser.AgentObjectTypeId);
                                }

                                if (!SessionHelper.Data<bool>(SessionKey.IsKPIs) && staff.StaffInfo.AgentObjects.Count > 1)
                                {
                                    AgentObject ao = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).SingleOrDefault();
                                    if (ao != null)
                                    {
                                        AgentObjectTypeId = ao.AgentObjectType.Id;
                                        result.StaffDTO.AgentObjectTypeId = AgentObjectTypeId;
                                    }
                                }
                            }
                            else
                            {
                                AgentObjectTypeId = agentObject.AgentObjectType.Id;
                                result.StaffDTO.AgentObjectTypeId = AgentObjectTypeId;
                            }

                            #region Chọn planStaff Cho từng đối tượng

                            //Chọn planstaff cho từng đối tượng

                            switch (AgentObjectTypeId)
                            {
                                //Dành cho giảng viên
                                case (int)AgentObjectTypes.GiangVien:
                                    {
                                        #region Giảng viên                                     
                                        if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null)
                                        {
                                            result.StaffDTO.AgentObjectTypeId = (int)AgentObjectTypes.GiangVien;

                                            DepartmentId = staff.Department.Id;
                                        }
                                        else
                                        {
                                            result.StaffDTO.AgentObjectTypeId = agentObject.AgentObjectType.Id;
                                            SubPosition sposition = staff.StaffInfo.SubPositions.Where(s => s.Position.AgentObjectType.Id == agentObject.AgentObjectType.Id).Select(s => s).FirstOrDefault();
                                            if (sposition != null)
                                                DepartmentId = sposition.Department.Id;
                                        }
                                        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Staff.Id == staff.Id && ps.Department == null && ps.AgentObjectType.Id == AgentObjectTypeId).SingleOrDefault();
                                        if (planStaff == null)
                                        {
                                            planStaff = new PlanStaff();
                                            planStaff.Id = Guid.NewGuid();
                                            planStaff.PlanKPI = plan;
                                            planStaff.Staff = new Staff() { Id = staff.Id };
                                            planStaff.ModifiedTime = DateTime.Now;
                                            planStaff.AgentObjectType = new AgentObjectType() { Id = AgentObjectTypeId };
                                            session.Save(planStaff);
                                        }
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.NhanVien:
                                    {
                                        #region NhanVien    
                                        if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null)
                                        {
                                            result.StaffDTO.AgentObjectTypeId = (int)AgentObjectTypes.NhanVien;

                                            DepartmentId = staff.Department.Id;
                                        }
                                        else
                                        {
                                            result.StaffDTO.AgentObjectTypeId = agentObject.AgentObjectType.Id;
                                            SubPosition sposition = staff.StaffInfo.SubPositions.Where(s => s.Position.AgentObjectType.Id == agentObject.AgentObjectType.Id).Select(s => s).FirstOrDefault();
                                            if (sposition != null)
                                                DepartmentId = sposition.Department.Id;
                                        }
                                        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Staff.Id == staff.Id && ps.Department == null && ps.AgentObjectType.Id == AgentObjectTypeId).SingleOrDefault();
                                        if (planStaff == null)
                                        {
                                            planStaff = new PlanStaff();
                                            planStaff.Id = Guid.NewGuid();
                                            planStaff.PlanKPI = new PlanKPI() { Id = planId };
                                            planStaff.Staff = new Staff() { Id = staff.Id };
                                            planStaff.ModifiedTime = DateTime.Now;
                                            planStaff.AgentObjectType = new AgentObjectType() { Id = AgentObjectTypeId };
                                            session.Save(planStaff);
                                        }
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhongBan:
                                    {
                                        #region PhongBan   
                                        if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null)
                                        {
                                            result.StaffDTO.AgentObjectTypeId = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.AgentObjectType.Id : (staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1);

                                            DepartmentId = staff.Department.Id;
                                        }
                                        else
                                        {
                                            result.StaffDTO.AgentObjectTypeId = agentObject.AgentObjectType.Id;
                                            SubPosition sposition = staff.StaffInfo.SubPositions.Where(s => s.Position.AgentObjectType.Id == agentObject.AgentObjectType.Id).Select(s => s).FirstOrDefault();
                                            if (sposition != null)
                                                DepartmentId = sposition.Department.Id;
                                        }
                                        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department != null && ps.Department.Id == DepartmentId && ps.AgentObjectType.Id == AgentObjectTypeId).SingleOrDefault();
                                        if (planStaff == null)
                                        {
                                            planStaff = new PlanStaff();
                                            planStaff.Id = Guid.NewGuid();
                                            planStaff.PlanKPI = new PlanKPI() { Id = planId };
                                        //if(staff.Id!=Guid.Empty) //User thường
                                        //    planStaff.Staff = new Staff() { Id = staff.Id };
                                            planStaff.Department = new Department() { Id = DepartmentId };
                                            planStaff.ModifiedTime = DateTime.Now;
                                            planStaff.AgentObjectType = new AgentObjectType() { Id = AgentObjectTypeId };
                                            session.Save(planStaff);
                                        }


                                        //Lưu cache dữ liệu:
                                        temdepartmentPlanCriterions = session.Query<Criterion>().Where(c => (c.Department.Id == DepartmentId && c.Staff == null)).ToList();
                                        //cacher.Add("Criterion", temdepartmentPlanCriterions, DateTimeOffset.UtcNow.AddHours(1));
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.Khoa:
                                    {
                                        #region Khoa   
                                        if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null)
                                        {
                                            result.StaffDTO.AgentObjectTypeId = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.AgentObjectType.Id : (staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1);

                                            DepartmentId = staff.Department.Id;
                                        }
                                        else
                                        {
                                            result.StaffDTO.AgentObjectTypeId = agentObject.AgentObjectType.Id;
                                            SubPosition sposition = staff.StaffInfo.SubPositions.Where(s => s.Position.AgentObjectType.Id == agentObject.AgentObjectType.Id).Select(s => s).FirstOrDefault();
                                            if (sposition != null)
                                                DepartmentId = sposition.Department.Id;
                                        }
                                        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department != null && ps.Department.Id == DepartmentId && ps.AgentObjectType.Id == AgentObjectTypeId).SingleOrDefault();
                                        if (planStaff == null)
                                        {
                                            planStaff = new PlanStaff();
                                            planStaff.Id = Guid.NewGuid();
                                            planStaff.PlanKPI = new PlanKPI() { Id = planId };
                                        //planStaff.Staff = new Staff() { Id = staff.Id };
                                            planStaff.Department = new Department() { Id = staff.Department.Id };
                                            planStaff.ModifiedTime = DateTime.Now;
                                            planStaff.AgentObjectType = new AgentObjectType() { Id = AgentObjectTypeId };
                                            session.Save(planStaff);
                                        }
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.BoMon:
                                    {
                                        #region BoMon
                                        if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null)
                                        {
                                            result.StaffDTO.AgentObjectTypeId = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.AgentObjectType.Id : (staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1);

                                            DepartmentId = staff.StaffInfo.Subject.Id;
                                        }
                                        else
                                        {
                                            result.StaffDTO.AgentObjectTypeId = agentObject.AgentObjectType.Id;
                                            SubPosition sposition = staff.StaffInfo.SubPositions.Where(s => s.Position.AgentObjectType.Id == agentObject.AgentObjectType.Id).Select(s => s).FirstOrDefault();
                                            if (sposition != null)
                                                DepartmentId = sposition.Department.Id;
                                        }
                                        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department != null && ps.Department.Id == DepartmentId && ps.AgentObjectType.Id == AgentObjectTypeId).SingleOrDefault();
                                        if (planStaff == null)
                                        {
                                            planStaff = new PlanStaff();
                                            planStaff.Id = Guid.NewGuid();
                                            planStaff.PlanKPI = new PlanKPI() { Id = planId };
                                            if (staff.Id != Guid.Empty) //User thường
                                                planStaff.Staff = new Staff() { Id = staff.Id };
                                            planStaff.Department = new Department() { Id = staff.StaffInfo.Subject.Id };
                                            planStaff.ModifiedTime = DateTime.Now;
                                            planStaff.AgentObjectType = new AgentObjectType() { Id = AgentObjectTypeId };
                                            session.Save(planStaff);
                                        }
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhoPhongBan: //Phó phong ban
                                    {
                                        #region PhoPhongBan
                                        if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null)
                                        {
                                            result.StaffDTO.AgentObjectTypeId = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.AgentObjectType.Id : (staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1);

                                            DepartmentId = staff.Department.Id;
                                        }
                                        else
                                        {
                                            result.StaffDTO.AgentObjectTypeId = agentObject.AgentObjectType.Id;
                                            SubPosition sposition = staff.StaffInfo.SubPositions.Where(s => s.Position.AgentObjectType.Id == agentObject.AgentObjectType.Id).Select(s => s).FirstOrDefault();
                                            if (sposition != null)
                                                DepartmentId = sposition.Department.Id;
                                        }
                                        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department != null && ps.Department.Id == DepartmentId && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).SingleOrDefault();
                                        if (planStaff == null)
                                        {
                                            planStaff = new PlanStaff();
                                            planStaff.Id = Guid.NewGuid();
                                            planStaff.PlanKPI = new PlanKPI() { Id = planId };
                                            planStaff.Staff = new Staff() { Id = staff.Id };
                                            planStaff.Department = new Department() { Id = staff.Department.Id };
                                            planStaff.ModifiedTime = DateTime.Now;
                                            planStaff.AgentObjectType = new AgentObjectType() { Id = AgentObjectTypeId };
                                            session.Save(planStaff);
                                        }
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhoKhoa: //Phó khoa
                                    {
                                        #region PhoKhoa
                                        if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null)
                                        {
                                            result.StaffDTO.AgentObjectTypeId = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.AgentObjectType.Id : (staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1);

                                            DepartmentId = staff.Department.Id;
                                        }
                                        else
                                        {
                                            result.StaffDTO.AgentObjectTypeId = agentObject.AgentObjectType.Id;
                                            SubPosition sposition = staff.StaffInfo.SubPositions.Where(s => s.Position.AgentObjectType.Id == agentObject.AgentObjectType.Id).Select(s => s).FirstOrDefault();
                                            if (sposition != null)
                                                DepartmentId = sposition.Department.Id;
                                        }
                                        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department != null && ps.Department.Id == DepartmentId && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).SingleOrDefault();
                                        if (planStaff == null)
                                        {
                                            planStaff = new PlanStaff();
                                            planStaff.Id = Guid.NewGuid();
                                            planStaff.PlanKPI = new PlanKPI() { Id = planId };
                                            planStaff.Staff = new Staff() { Id = staff.Id };
                                            planStaff.Department = new Department() { Id = staff.Department.Id };
                                            planStaff.ModifiedTime = DateTime.Now;
                                            planStaff.AgentObjectType = new AgentObjectType() { Id = AgentObjectTypeId };
                                            session.Save(planStaff);
                                        }
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhoBoMon: //Phó bộ môn
                                    {
                                        #region PhoBoMon
                                        if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null)
                                        {
                                            result.StaffDTO.AgentObjectTypeId = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.AgentObjectType.Id : (staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1);

                                            DepartmentId = staff.Department.Id;
                                        }
                                        else
                                        {
                                            result.StaffDTO.AgentObjectTypeId = agentObject.AgentObjectType.Id;
                                            SubPosition sposition = staff.StaffInfo.SubPositions.Where(s => s.Position.AgentObjectType.Id == agentObject.AgentObjectType.Id).Select(s => s).FirstOrDefault();
                                            if (sposition != null)
                                                DepartmentId = sposition.Department.Id;
                                        }
                                        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department != null && ps.Department.Id == DepartmentId && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).SingleOrDefault();
                                        if (planStaff == null)
                                        {
                                            planStaff = new PlanStaff();
                                            planStaff.Id = Guid.NewGuid();
                                            planStaff.PlanKPI = new PlanKPI() { Id = planId };
                                            planStaff.Staff = new Staff() { Id = staff.Id };
                                            planStaff.Department = new Department() { Id = staff.Department.Id };
                                            planStaff.ModifiedTime = DateTime.Now;
                                            planStaff.AgentObjectType = new AgentObjectType() { Id = AgentObjectTypeId };
                                            session.Save(planStaff);
                                        }
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.HieuTruong: // Hiệu trưởng
                                    {
                                        #region HieuTruong
                                        if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null)
                                        {
                                            result.StaffDTO.AgentObjectTypeId = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.AgentObjectType.Id : (staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1);

                                            DepartmentId = staff.Department.Id;
                                        }
                                        else
                                        {
                                            result.StaffDTO.AgentObjectTypeId = agentObject.AgentObjectType.Id;
                                            SubPosition sposition = staff.StaffInfo.SubPositions.Where(s => s.Position.AgentObjectType.Id == agentObject.AgentObjectType.Id).Select(s => s).FirstOrDefault();
                                            if (sposition != null)
                                                DepartmentId = sposition.Department.Id;
                                        }
                                        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department != null && ps.Department.Id == DepartmentId && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).SingleOrDefault();
                                        if (planStaff == null)
                                        {
                                            planStaff = new PlanStaff();
                                            planStaff.Id = Guid.NewGuid();
                                            planStaff.PlanKPI = new PlanKPI() { Id = planId };
                                            planStaff.Staff = new Staff() { Id = staff.Id };
                                            planStaff.Department = new Department() { Id = staff.Department.Id };
                                            planStaff.ModifiedTime = DateTime.Now;
                                            planStaff.AgentObjectType = new AgentObjectType() { Id = AgentObjectTypeId };
                                            session.Save(planStaff);
                                        }
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhoHieuTruong: //Phó Hiệu trưởng
                                    {
                                        #region PhoHieuTruong
                                        if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null)
                                        {
                                            result.StaffDTO.AgentObjectTypeId = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.AgentObjectType.Id : (staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1);

                                            DepartmentId = staff.Department.Id;
                                        }
                                        else
                                        {
                                            result.StaffDTO.AgentObjectTypeId = agentObject.AgentObjectType.Id;
                                            SubPosition sposition = staff.StaffInfo.SubPositions.Where(s => s.Position.AgentObjectType.Id == agentObject.AgentObjectType.Id).Select(s => s).FirstOrDefault();
                                            if (sposition != null)
                                                DepartmentId = sposition.Department.Id;
                                        }
                                        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department != null && ps.Department.Id == DepartmentId && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId).SingleOrDefault();
                                        if (planStaff == null)
                                        {
                                            planStaff = new PlanStaff();
                                            planStaff.Id = Guid.NewGuid();
                                            planStaff.PlanKPI = new PlanKPI() { Id = planId };
                                            planStaff.Staff = new Staff() { Id = staff.Id };
                                            planStaff.Department = new Department() { Id = staff.Department.Id };
                                            planStaff.ModifiedTime = DateTime.Now;
                                            planStaff.AgentObjectType = new AgentObjectType() { Id = AgentObjectTypeId };
                                            session.Save(planStaff);
                                        }
                                        #endregion
                                    }
                                    break;
                            }
                            result.IsLocked = planStaff.IsLocked;
                            #endregion

                        }
                        logMessage += "(planStaffId: " + planStaff.Id + ")";
                        foreach (TargetGroupDetail t in targets)
                        {
                            TargetGroupPlanMakingDTO tg = new TargetGroupPlanMakingDTO();

                            List<CriterionDictionaryDTO> dictionaries = new List<CriterionDictionaryDTO>();
                            dictionaries = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id).ToList().Map<CriterionDictionaryDTO>();
                            tg.TargetGroupDetailTypeId = t.TargetGroupDetailType.Id;
                            tg.Density = t.Density;
                            tg.CriterionDictionaries = dictionaries;

                            //Dữ liệu gán của kế hoạch
                            List<PlanKPIDetail> planDetails = new List<PlanKPIDetail>();
                            List<PlanKPIDetail> parentPlanKPIDetail = new List<PlanKPIDetail>();
                            List<PlanKPIDetail> parentPlanKPIDetail2 = new List<PlanKPIDetail>();
                            List<PlanKPIDetail> planDetails2 = new List<PlanKPIDetail>();
                            List<Criterion> temstaffPlanCriterions = new List<Criterion>();
                            List<Criterion> staffPlanCriterions = new List<Criterion>();
                            PlanStaff parentPlanStaff = new PlanStaff();
                            PlanStaff parentPlanStaff2 = new PlanStaff();
                            List<Criterion> departmentPlanCriterions = new List<Criterion>();
                            List<CriterionPlanDTO> criterions = new List<CriterionPlanDTO>();


                            #region Lấy dữ liệu từ db
                            switch (AgentObjectTypeId)
                            {
                                case (int)AgentObjectTypes.NhanVien:
                                    {
                                        #region Nhân viên                                       
                                        //if (staff.StaffInfo.Subject != null)
                                        // temstaffPlanCriterions = session.Query<Criterion>().Where(c => (c.Staff.Id == staff.Id || c.StaffLeader.Id == staff.Id || c.Department.Id == staff.StaffInfo.Subject.Id) && c.TargetGroupDetail.TargetGroupDetailType.Id != 3).ToList();
                                        // else
                                        temstaffPlanCriterions = session.Query<Criterion>().Where(c => (c.Staff.Id == staff.Id || c.StaffLeader.Id == staff.Id) && c.TargetGroupDetail.TargetGroupDetailType.Id != 3).ToList();

                                        staffPlanCriterions = temstaffPlanCriterions.Where(c =>
                                       (c.FromPlanKPIDetail == null || !c.FromPlanKPIDetail.IsDisable && ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Count > 0)
                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                        (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                        (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) >= 0))).ToList();



                                        if (planStaff != null)
                                        {
                                            //Lấy planstaff của năm
                                            if (plan.ParentPlan != null && plan.ParentPlan.ParentPlan != null)
                                            {
                                                parentPlanStaff2 = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Staff.Id == staff.Id && ps.PlanKPI.Id == plan.ParentPlan.ParentPlan.Id && ps.Department == null);
                                                if (parentPlanStaff2 == null)
                                                    parentPlanStaff2 = new PlanStaff();
                                            }

                                            //Lấy planstaff của học kỳ
                                            if (plan.ParentPlan != null)
                                            {
                                                parentPlanStaff = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Staff.Id == staff.Id && ps.PlanKPI.Id == plan.ParentPlan.Id && ps.Department == null);
                                                if (parentPlanStaff == null)
                                                    parentPlanStaff = new PlanStaff();
                                            }




                                            if (parentPlanStaff != null)
                                            {
                                                //Lấy planDetail của học kỳ
                                                parentPlanKPIDetail = parentPlanStaff.PlanKPIDetails.Where(pd =>
                                                    (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                  && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }

                                            if (parentPlanStaff2 != null)
                                            {
                                                //Lấy planDetail của năm học
                                                parentPlanKPIDetail2 = parentPlanStaff2.PlanKPIDetails.Where(pd =>
                                                  (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }


                                            //Danh sách tổng hộp các planDetail của học kỳ và năm (ưu tiên học kỳ)
                                            List<PlanKPIDetail> parentMergePlanKPIDetail = parentPlanKPIDetail2.Where(p => !parentPlanKPIDetail.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();

                                            foreach (PlanKPIDetail pkd in parentMergePlanKPIDetail)
                                            {
                                                parentPlanKPIDetail.Add(pkd);
                                            }

                                            planDetails = planStaff.PlanKPIDetails.Where(pd => (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && pd.FromCriterion == null) ||
                                                          (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();

                                            planDetails2 = new List<PlanKPIDetail>();

                                            //planDetails2 = session.Query<PlanKPIDetail>().Where(pd =>
                                            //   pd.TargetGroupDetail.Id != t.Id && pd.Methods.Count > 0
                                            //   && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) >= 0))).ToList();

                                            result.PlanStaffId = planStaff.Id;

                                            List<PlanKPIDetail> mergetDetails = parentPlanKPIDetail.Where(p => !planDetails.Any(p2 => (p2.FromCriterion != null && p.FromCriterion != null && p2.FromCriterion.Id == p.FromCriterion.Id) || ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();
                                            foreach (PlanKPIDetail pld in mergetDetails)
                                            {
                                                planDetails.Add(pld);
                                            }
                                        }
                                        else
                                            result.PlanStaffId = Guid.Empty;


                                        tg.TargetGroupDetailTypeId = t.TargetGroupDetailType.Id;

                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhongBan:
                                    {
                                        #region Phòng ban

                                        #region Lấy dữ liệu từ db


                                        temdepartmentPlanCriterions = temdepartmentPlanCriterions.Where(c => (c.Department.Id == DepartmentId) && c.TargetGroupDetail.TargetGroupDetailType.Id != 3).ToList();


                                        List<Criterion> nullcri = temdepartmentPlanCriterions.Where(c => c.FromPlanKPIDetail == null).ToList();



                                        //List<Criterion> departmentPlanCriterions = temdepartmentPlanCriterions.Where(c =>
                                        // (c.FromPlanKPIDetail == null || !c.FromPlanKPIDetail.IsDisable && ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Count > 0)
                                        //    && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                        //                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                        //                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) >= 0))).ToList();



                                        foreach (Criterion cri in temdepartmentPlanCriterions)
                                        {
                                            List<MethodDTO> originalMethods = ControllerHelpers.GetOriginalMethods(cri.FromPlanKPIDetail.Id, session);

                                            bool isValid = (cri.FromPlanKPIDetail == null || !cri.FromPlanKPIDetail.IsDisable && originalMethods.Count > 0) && !cri.FromPlanKPIDetail.IsDelete
                                            && ((plan.StartTime.CompareTo(originalMethods.Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(originalMethods.Max(m => m.EndTime)) <= 0) ||
                                                          (plan.EndTime.CompareTo(originalMethods.Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(originalMethods.Max(m => m.EndTime)) <= 0) ||
                                                          (plan.StartTime.CompareTo(originalMethods.Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(originalMethods.Max(m => m.EndTime)) >= 0));
                                            if (isValid)
                                                departmentPlanCriterions.Add(cri);
                                        }



                                        if (planStaff != null)
                                        {
                                            //Lấy planstaff của năm
                                            if (plan.ParentPlan != null && plan.ParentPlan.ParentPlan != null)
                                                parentPlanStaff2 = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Department.Id == staff.Department.Id && ps.PlanKPI.Id == plan.ParentPlan.ParentPlan.Id && ps.AgentObjectType.Id == AgentObjectTypeId);
                                            //Lấy planstaff của học kỳ
                                            if (plan.ParentPlan != null)
                                                parentPlanStaff = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Department.Id == staff.Department.Id && ps.PlanKPI.Id == plan.ParentPlan.Id && ps.AgentObjectType.Id == AgentObjectTypeId);

                                            if (parentPlanStaff != null)
                                            {
                                                //Lấy planDetail của học kỳ
                                                //parentPlanKPIDetail = parentPlanStaff.PlanKPIDetails.Where(pd =>
                                                //    (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                //  && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                //  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                //  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();


                                                foreach (PlanKPIDetail pd in parentPlanStaff.PlanKPIDetails)
                                                {
                                                    List<MethodDTO> originalMethods = ControllerHelpers.GetOriginalMethods(pd.Id, session);

                                                    bool isValid = (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && originalMethods.Count > 0 && pd.IsDelete == false
                                                  && ((plan.StartTime.CompareTo(originalMethods.Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(originalMethods.Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(originalMethods.Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(originalMethods.Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(originalMethods.Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(originalMethods.Max(m => m.EndTime)) >= 0)));
                                                    if (isValid)
                                                        parentPlanKPIDetail.Add(pd);
                                                }
                                            }

                                            if (parentPlanStaff2 != null)
                                            {
                                                //Lấy planDetail của năm học
                                                //parentPlanKPIDetail2 = parentPlanStaff2.PlanKPIDetails.Where(pd =>
                                                //  (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                //          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                //  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                //  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();

                                                foreach (PlanKPIDetail pd in parentPlanStaff2.PlanKPIDetails)
                                                {
                                                    List<MethodDTO> originalMethods = ControllerHelpers.GetOriginalMethods(pd.Id, session);

                                                    bool isValid = (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && originalMethods.Count > 0 && pd.IsDelete == false
                                                  && ((plan.StartTime.CompareTo(originalMethods.Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(originalMethods.Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(originalMethods.Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(originalMethods.Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(originalMethods.Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(originalMethods.Max(m => m.EndTime)) >= 0)));
                                                    if (isValid)
                                                        parentPlanKPIDetail2.Add(pd);
                                                }

                                            }


                                            //Danh sách tổng hộp các planDetail của học kỳ và năm (ưu tiên học kỳ)
                                            List<PlanKPIDetail> parentMergePlanKPIDetail = parentPlanKPIDetail2.Where(p => !parentPlanKPIDetail.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();

                                            foreach (PlanKPIDetail pkd in parentMergePlanKPIDetail)
                                            {
                                                parentPlanKPIDetail.Add(pkd);
                                            }

                                            //planDetails = planStaff.PlanKPIDetails.Where(pd => (((pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id) || pd.IsFromEoffice == true) && pd.FromCriterion == null) ||
                                            //              (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                            //              && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();

                                            foreach (PlanKPIDetail pd in planStaff.PlanKPIDetails)
                                            {
                                                List<MethodDTO> originalMethods = ControllerHelpers.GetOriginalMethods(pd.Id, session);

                                                bool isValid = (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && originalMethods.Count > 0
                                              && ((plan.StartTime.CompareTo(originalMethods.Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(originalMethods.Max(m => m.EndTime)) <= 0) ||
                                              (plan.EndTime.CompareTo(originalMethods.Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(originalMethods.Max(m => m.EndTime)) <= 0) ||
                                              (plan.StartTime.CompareTo(originalMethods.Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(originalMethods.Max(m => m.EndTime)) >= 0)));
                                                if (isValid)
                                                    planDetails.Add(pd);
                                            }
                                            #endregion
                                            planDetails2 = new List<PlanKPIDetail>();

                                            //planDetails2 = session.Query<PlanKPIDetail>().Where(pd =>
                                            //   pd.TargetGroupDetail.Id != t.Id && pd.Methods.Count > 0
                                            //   && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) >= 0))).ToList();

                                            result.PlanStaffId = planStaff.Id;

                                            List<PlanKPIDetail> mergetDetails = parentPlanKPIDetail.Where(p => !planDetails.Any(p2 => (p2.FromCriterion != null && p.FromCriterion != null && p2.FromCriterion.Id == p.FromCriterion.Id) || ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();
                                            foreach (PlanKPIDetail pld in mergetDetails)
                                            {
                                                planDetails.Add(pld);
                                            }
                                        }
                                        else
                                            result.PlanStaffId = Guid.Empty;
                                        tg.TargetGroupDetailTypeId = t.TargetGroupDetailType.Id;
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.BanGiamHieu:
                                    {
                                        #region Ban giám hiệu           
                                        if (planStaff != null)
                                        {
                                            int planMonth = plan.StartTime.Month;
                                            int year = plan.StartTime.Year;

                                            Guid webGroupId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId);


                                            if (plan.ParentPlan != null && plan.ParentPlan.ParentPlan != null)
                                                parentPlanStaff = session.Query<PlanStaff>().SingleOrDefault(ps => (ps.WebGroupId == new Guid("00000000-0000-0000-0000-000000000001") || ps.WebGroupId == new Guid("00000000-0000-0000-0000-000000000002") || ps.WebGroupId == new Guid("00000000-0000-0000-0000-000000000003")) && ps.PlanKPI.Id == plan.ParentPlan.ParentPlan.Id);
                                            else if (plan.ParentPlan != null)
                                                parentPlanStaff = session.Query<PlanStaff>().SingleOrDefault(ps => (ps.WebGroupId == new Guid("00000000-0000-0000-0000-000000000001") || ps.WebGroupId == new Guid("00000000-0000-0000-0000-000000000002") || ps.WebGroupId == new Guid("00000000-0000-0000-0000-000000000003")) && ps.PlanKPI.Id == plan.ParentPlan.Id);

                                            if (parentPlanStaff != null)
                                            {
                                                parentPlanKPIDetail = parentPlanStaff.PlanKPIDetails.Where(pd => pd.TargetGroupDetail != null &&
                                                    pd.TargetGroupDetail.Id == t.Id && pd.Methods.Count > 0
                                                  && ((plan.StartTime.CompareTo(pd.Methods.Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(pd.Methods.Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(pd.Methods.Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(pd.Methods.Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(pd.Methods.Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(pd.Methods.Max(m => m.EndTime)) >= 0))).ToList();
                                            }

                                            planDetails = planStaff.PlanKPIDetails.Where(pd => pd.TargetGroupDetail != null &&
                                                          pd.TargetGroupDetail.Id == t.Id).ToList();


                                            result.PlanStaffId = planStaff.Id;

                                            List<PlanKPIDetail> mergetDetails = parentPlanKPIDetail != null ? parentPlanKPIDetail.Where(p => !planDetails.Any(p2 => p2.ParentPlanKPIDetail != null && p2.ParentPlanKPIDetail.Id == p.Id)).ToList() : new List<PlanKPIDetail>();

                                            foreach (PlanKPIDetail pld in mergetDetails)
                                            {
                                                planDetails.Add(pld);
                                            }

                                            result.PlanStaffId = planStaff.Id;
                                        }
                                        else
                                            result.PlanStaffId = Guid.Empty;
                                        tg.PlanKPIDetails = new List<PlanKPIMakingDetailDTO>();
                                        criterions = session.Query<Criterion>().Where(c => c.TargetGroupDetail != null && c.TargetGroupDetail.Id == t.Id && c.Department.Id == staff.Department.Id && c.PlanKPI.Id == planId).Map<CriterionPlanDTO>().ToList();
                                        tg.Criterions = criterions;
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.Khoa:
                                    {
                                        #region Khoa

                                        departmentPlanCriterions = session.Query<Criterion>().Where(c => c.Department.Id == DepartmentId && c.Staff == null && c.TargetGroupDetail.Id == t.Id
                                            //&& (c.FromPlanKPIDetail == null || !c.FromPlanKPIDetail.IsDisable) 
                                            && c.FromPlanKPIDetail.Methods.Count > 0
                                            && ((plan.StartTime.CompareTo(c.FromPlanKPIDetail.Methods.Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(c.FromPlanKPIDetail.Methods.Max(m => m.EndTime)) <= 0) ||
                                                          (plan.EndTime.CompareTo(c.FromPlanKPIDetail.Methods.Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(c.FromPlanKPIDetail.Methods.Max(m => m.EndTime)) <= 0) ||
                                                          (plan.StartTime.CompareTo(c.FromPlanKPIDetail.Methods.Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(c.FromPlanKPIDetail.Methods.Max(m => m.EndTime)) >= 0))).ToList();


                                        if (planStaff != null)
                                        {

                                            //Lấy planstaff của năm
                                            if (plan.ParentPlan != null && plan.ParentPlan.ParentPlan != null)
                                                parentPlanStaff2 = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Department.Id == staff.Department.Id && ps.PlanKPI.Id == plan.ParentPlan.ParentPlan.Id && ps.AgentObjectType.Id == AgentObjectTypeId);
                                            //Lấy planstaff của học kỳ
                                            if (plan.ParentPlan != null)
                                                parentPlanStaff = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Department.Id == staff.Department.Id && ps.PlanKPI.Id == plan.ParentPlan.Id && ps.AgentObjectType.Id == AgentObjectTypeId);

                                            if (parentPlanStaff != null)
                                            {
                                                //Lấy planDetail của học kỳ
                                                parentPlanKPIDetail = parentPlanStaff.PlanKPIDetails.Where(pd =>
                                                    (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                  && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }

                                            if (parentPlanStaff2 != null)
                                            {
                                                //Lấy planDetail của năm học
                                                parentPlanKPIDetail2 = parentPlanStaff2.PlanKPIDetails.Where(pd =>
                                                  (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }


                                            //Danh sách tổng hộp các planDetail của học kỳ và năm (ưu tiên học kỳ)
                                            List<PlanKPIDetail> parentMergePlanKPIDetail = parentPlanKPIDetail2.Where(p => !parentPlanKPIDetail.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();

                                            foreach (PlanKPIDetail pkd in parentMergePlanKPIDetail)
                                            {
                                                parentPlanKPIDetail.Add(pkd);
                                            }

                                            planDetails = planStaff.PlanKPIDetails.Where(pd => (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && pd.FromCriterion == null) ||
                                                          (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();

                                            planDetails2 = new List<PlanKPIDetail>();

                                            //planDetails2 = session.Query<PlanKPIDetail>().Where(pd =>
                                            //   pd.TargetGroupDetail.Id != t.Id && pd.Methods.Count > 0
                                            //   && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) >= 0))).ToList();

                                            result.PlanStaffId = planStaff.Id;

                                            List<PlanKPIDetail> mergetDetails = parentPlanKPIDetail.Where(p => !planDetails.Any(p2 => (p2.FromCriterion != null && p.FromCriterion != null && p2.FromCriterion.Id == p.FromCriterion.Id) || ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();
                                            foreach (PlanKPIDetail pld in mergetDetails)
                                            {
                                                planDetails.Add(pld);
                                            }
                                        }
                                        else
                                            result.PlanStaffId = Guid.Empty;
                                        tg.TargetGroupDetailTypeId = t.TargetGroupDetailType.Id;
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.BoMon:
                                    {
                                        #region Bộ môn

                                        List<Criterion> templist = session.Query<Criterion>().Where(c => c.Department.Id == DepartmentId && c.Staff == null && c.TargetGroupDetail.Id == t.Id).ToList();
                                        departmentPlanCriterions = templist.Where(c => !c.FromPlanKPIDetail.IsDisable && ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Count > 0
                                            && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                          (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                          (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) >= 0))).ToList();

                                        if (planStaff != null)
                                        {

                                            //Lấy planstaff của năm
                                            if (plan.ParentPlan != null && plan.ParentPlan.ParentPlan != null)
                                                parentPlanStaff2 = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Department.Id == staff.StaffInfo.Subject.Id && ps.PlanKPI.Id == plan.ParentPlan.ParentPlan.Id && ps.AgentObjectType.Id == AgentObjectTypeId);
                                            //Lấy planstaff của học kỳ
                                            if (plan.ParentPlan != null)
                                                parentPlanStaff = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Department.Id == staff.StaffInfo.Subject.Id && ps.PlanKPI.Id == plan.ParentPlan.Id && ps.AgentObjectType.Id == AgentObjectTypeId);

                                            if (parentPlanStaff != null)
                                            {
                                                //Lấy planDetail của học kỳ
                                                parentPlanKPIDetail = parentPlanStaff.PlanKPIDetails.Where(pd =>
                                                    (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                  && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }

                                            if (parentPlanStaff2 != null)
                                            {
                                                //Lấy planDetail của năm học
                                                parentPlanKPIDetail2 = parentPlanStaff2.PlanKPIDetails.Where(pd =>
                                                  (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }


                                            //Danh sách tổng hộp các planDetail của học kỳ và năm (ưu tiên học kỳ)
                                            List<PlanKPIDetail> parentMergePlanKPIDetail = parentPlanKPIDetail2.Where(p => !parentPlanKPIDetail.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();

                                            foreach (PlanKPIDetail pkd in parentMergePlanKPIDetail)
                                            {
                                                parentPlanKPIDetail.Add(pkd);
                                            }

                                            planDetails = planStaff.PlanKPIDetails.Where(pd => (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && pd.FromCriterion == null) ||
                                                          (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();

                                            planDetails2 = new List<PlanKPIDetail>();

                                            //planDetails2 = session.Query<PlanKPIDetail>().Where(pd =>
                                            //   pd.TargetGroupDetail.Id != t.Id && pd.Methods.Count > 0
                                            //   && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) >= 0))).ToList();

                                            result.PlanStaffId = planStaff.Id;

                                            List<PlanKPIDetail> mergetDetails = parentPlanKPIDetail.Where(p => !planDetails.Any(p2 => (p2.FromCriterion != null && p.FromCriterion != null && p2.FromCriterion.Id == p.FromCriterion.Id) || ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();
                                            foreach (PlanKPIDetail pld in mergetDetails)
                                            {
                                                planDetails.Add(pld);
                                            }
                                        }
                                        else
                                            result.PlanStaffId = Guid.Empty;
                                        tg.TargetGroupDetailTypeId = t.TargetGroupDetailType.Id;

                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhoPhongBan:
                                    {
                                        #region Phó phòng ban

                                        temstaffPlanCriterions = session.Query<Criterion>().Where(c => (c.Staff.Id == staff.Id || c.StaffLeader.Id == staff.Id) && c.TargetGroupDetail.TargetGroupDetailType.Id != 3).ToList();


                                        staffPlanCriterions = temstaffPlanCriterions.Where(c =>
                                         (c.FromPlanKPIDetail == null || !c.FromPlanKPIDetail.IsDisable && ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Count > 0)
                                            && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                          (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                          (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) >= 0))).ToList();

                                        if (planStaff != null)
                                        {
                                            //Lấy planstaff của năm
                                            if (plan.ParentPlan != null && plan.ParentPlan.ParentPlan != null)
                                            {
                                                parentPlanStaff2 = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Staff.Id == staff.Id && ps.PlanKPI.Id == plan.ParentPlan.ParentPlan.Id && ps.Department == null);
                                                if (parentPlanStaff2 == null)
                                                    parentPlanStaff2 = new PlanStaff();
                                            }

                                            //Lấy planstaff của học kỳ
                                            if (plan.ParentPlan != null)
                                            {
                                                parentPlanStaff = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Staff.Id == staff.Id && ps.PlanKPI.Id == plan.ParentPlan.Id && ps.Department == null);
                                                if (parentPlanStaff == null)
                                                    parentPlanStaff = new PlanStaff();
                                            }




                                            if (parentPlanStaff != null)
                                            {
                                                //Lấy planDetail của học kỳ
                                                parentPlanKPIDetail = parentPlanStaff.PlanKPIDetails.Where(pd =>
                                                    (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                  && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }

                                            if (parentPlanStaff2 != null)
                                            {
                                                //Lấy planDetail của năm học
                                                parentPlanKPIDetail2 = parentPlanStaff2.PlanKPIDetails.Where(pd =>
                                                  (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }


                                            //Danh sách tổng hộp các planDetail của học kỳ và năm (ưu tiên học kỳ)
                                            List<PlanKPIDetail> parentMergePlanKPIDetail = parentPlanKPIDetail2.Where(p => !parentPlanKPIDetail.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();

                                            foreach (PlanKPIDetail pkd in parentMergePlanKPIDetail)
                                            {
                                                parentPlanKPIDetail.Add(pkd);
                                            }

                                            planDetails = planStaff.PlanKPIDetails.Where(pd => (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && pd.FromCriterion == null) ||
                                                          (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();

                                            planDetails2 = new List<PlanKPIDetail>();

                                            //planDetails2 = session.Query<PlanKPIDetail>().Where(pd =>
                                            //   pd.TargetGroupDetail.Id != t.Id && pd.Methods.Count > 0
                                            //   && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) >= 0))).ToList();

                                            result.PlanStaffId = planStaff.Id;

                                            List<PlanKPIDetail> mergetDetails = parentPlanKPIDetail.Where(p => !planDetails.Any(p2 => (p2.FromCriterion != null && p.FromCriterion != null && p2.FromCriterion.Id == p.FromCriterion.Id) || ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();
                                            foreach (PlanKPIDetail pld in mergetDetails)
                                            {
                                                planDetails.Add(pld);
                                            }
                                        }
                                        else
                                            result.PlanStaffId = Guid.Empty;


                                        tg.TargetGroupDetailTypeId = t.TargetGroupDetailType.Id;

                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhoKhoa:
                                    {
                                        #region Phó khoa

                                        //List<Criterion> departmentPlanCriterions = session.Query<Criterion>().Where(c => c.Staff.Id == staff.Id && c.TargetGroupDetail.Id == t.Id
                                        //    ).ToList();

                                        List<Criterion> templist = session.Query<Criterion>().Where(c => c.Staff.Id == staff.Id && c.TargetGroupDetail.Id == t.Id).ToList();
                                        departmentPlanCriterions = templist.Where(c => !c.FromPlanKPIDetail.IsDisable && ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Count > 0
                                            && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                          (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                          (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) >= 0))).ToList();


                                        if (planStaff != null)
                                        {

                                            //Lấy planstaff của năm
                                            if (plan.ParentPlan != null && plan.ParentPlan.ParentPlan != null)
                                                parentPlanStaff2 = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Department.Id == staff.Department.Id && ps.PlanKPI.Id == plan.ParentPlan.ParentPlan.Id && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId);
                                            //Lấy planstaff của học kỳ
                                            if (plan.ParentPlan != null)
                                                parentPlanStaff = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Department.Id == staff.Department.Id && ps.PlanKPI.Id == plan.ParentPlan.Id && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId);

                                            if (parentPlanStaff != null)
                                            {
                                                //Lấy planDetail của học kỳ
                                                parentPlanKPIDetail = parentPlanStaff.PlanKPIDetails.Where(pd =>
                                                    (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                  && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }

                                            if (parentPlanStaff2 != null)
                                            {
                                                //Lấy planDetail của năm học
                                                parentPlanKPIDetail2 = parentPlanStaff2.PlanKPIDetails.Where(pd =>
                                                  (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }


                                            //Danh sách tổng hộp các planDetail của học kỳ và năm (ưu tiên học kỳ)
                                            List<PlanKPIDetail> parentMergePlanKPIDetail = parentPlanKPIDetail2.Where(p => !parentPlanKPIDetail.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();

                                            foreach (PlanKPIDetail pkd in parentMergePlanKPIDetail)
                                            {
                                                parentPlanKPIDetail.Add(pkd);
                                            }

                                            planDetails = planStaff.PlanKPIDetails.Where(pd => (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && pd.FromCriterion == null) ||
                                                          (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();

                                            planDetails2 = new List<PlanKPIDetail>();

                                            //planDetails2 = session.Query<PlanKPIDetail>().Where(pd =>
                                            //   pd.TargetGroupDetail.Id != t.Id && pd.Methods.Count > 0
                                            //   && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) >= 0))).ToList();

                                            result.PlanStaffId = planStaff.Id;

                                            List<PlanKPIDetail> mergetDetails = parentPlanKPIDetail.Where(p => !planDetails.Any(p2 => (p2.FromCriterion != null && p.FromCriterion != null && p2.FromCriterion.Id == p.FromCriterion.Id) || ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();
                                            foreach (PlanKPIDetail pld in mergetDetails)
                                            {
                                                planDetails.Add(pld);
                                            }
                                        }
                                        else
                                            result.PlanStaffId = Guid.Empty;
                                        tg.TargetGroupDetailTypeId = t.TargetGroupDetailType.Id;
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhoBoMon:
                                    {
                                        #region Phó BỘ MÔN

                                        List<Criterion> templist = session.Query<Criterion>().Where(c => c.Staff.Id == staff.Id && c.TargetGroupDetail.Id == t.Id).ToList();
                                        departmentPlanCriterions = templist.Where(c => !c.FromPlanKPIDetail.IsDisable && ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Count > 0
                                            && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                          (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                          (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) >= 0))).ToList();

                                        if (planStaff != null)
                                        {

                                            //Lấy planstaff của năm
                                            if (plan.ParentPlan != null && plan.ParentPlan.ParentPlan != null)
                                                parentPlanStaff2 = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Department.Id == staff.StaffInfo.Subject.Id && ps.PlanKPI.Id == plan.ParentPlan.ParentPlan.Id);
                                            //Lấy planstaff của học kỳ
                                            if (plan.ParentPlan != null)
                                                parentPlanStaff = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Department.Id == staff.StaffInfo.Subject.Id && ps.PlanKPI.Id == plan.ParentPlan.Id);

                                            if (parentPlanStaff != null)
                                            {
                                                //Lấy planDetail của học kỳ
                                                parentPlanKPIDetail = parentPlanStaff.PlanKPIDetails.Where(pd =>
                                                    (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                  && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }

                                            if (parentPlanStaff2 != null)
                                            {
                                                //Lấy planDetail của năm học
                                                parentPlanKPIDetail2 = parentPlanStaff2.PlanKPIDetails.Where(pd =>
                                                  (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }


                                            //Danh sách tổng hộp các planDetail của học kỳ và năm (ưu tiên học kỳ)
                                            List<PlanKPIDetail> parentMergePlanKPIDetail = parentPlanKPIDetail2.Where(p => !parentPlanKPIDetail.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();

                                            foreach (PlanKPIDetail pkd in parentMergePlanKPIDetail)
                                            {
                                                parentPlanKPIDetail.Add(pkd);
                                            }

                                            planDetails = planStaff.PlanKPIDetails.Where(pd => (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && pd.FromCriterion == null) ||
                                                          (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();

                                            planDetails2 = new List<PlanKPIDetail>();

                                            //planDetails2 = session.Query<PlanKPIDetail>().Where(pd =>
                                            //   pd.TargetGroupDetail.Id != t.Id && pd.Methods.Count > 0
                                            //   && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) >= 0))).ToList();

                                            result.PlanStaffId = planStaff.Id;

                                            List<PlanKPIDetail> mergetDetails = parentPlanKPIDetail.Where(p => !planDetails.Any(p2 => (p2.FromCriterion != null && p.FromCriterion != null && p2.FromCriterion.Id == p.FromCriterion.Id) || ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();
                                            foreach (PlanKPIDetail pld in mergetDetails)
                                            {
                                                planDetails.Add(pld);
                                            }
                                        }
                                        else
                                            result.PlanStaffId = Guid.Empty;
                                        tg.TargetGroupDetailTypeId = t.TargetGroupDetailType.Id;
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.HieuTruong:
                                    {
                                        #region Hiệu trưởng

                                        //List<Criterion> departmentPlanCriterions = session.Query<Criterion>().Where(c => c.Staff.Id == staff.Id && c.TargetGroupDetail.Id == t.Id
                                        //    ).ToList();

                                        List<Criterion> templist = session.Query<Criterion>().Where(c => c.Staff.Id == staff.Id && c.TargetGroupDetail.Id == t.Id).ToList();
                                        departmentPlanCriterions = templist.Where(c => !c.FromPlanKPIDetail.IsDisable && ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Count > 0
                                            && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                          (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                          (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) >= 0))).ToList();


                                        if (planStaff != null)
                                        {

                                            //Lấy planstaff của năm
                                            if (plan.ParentPlan != null && plan.ParentPlan.ParentPlan != null)
                                                parentPlanStaff2 = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Department.Id == staff.Department.Id && ps.PlanKPI.Id == plan.ParentPlan.ParentPlan.Id && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId);
                                            //Lấy planstaff của học kỳ
                                            if (plan.ParentPlan != null)
                                                parentPlanStaff = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Department.Id == staff.Department.Id && ps.PlanKPI.Id == plan.ParentPlan.Id && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId);

                                            if (parentPlanStaff != null)
                                            {
                                                //Lấy planDetail của học kỳ
                                                parentPlanKPIDetail = parentPlanStaff.PlanKPIDetails.Where(pd =>
                                                    (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                  && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }

                                            if (parentPlanStaff2 != null)
                                            {
                                                //Lấy planDetail của năm học
                                                parentPlanKPIDetail2 = parentPlanStaff2.PlanKPIDetails.Where(pd =>
                                                  (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }


                                            //Danh sách tổng hộp các planDetail của học kỳ và năm (ưu tiên học kỳ)
                                            List<PlanKPIDetail> parentMergePlanKPIDetail = parentPlanKPIDetail2.Where(p => !parentPlanKPIDetail.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();

                                            foreach (PlanKPIDetail pkd in parentMergePlanKPIDetail)
                                            {
                                                parentPlanKPIDetail.Add(pkd);
                                            }

                                            planDetails = planStaff.PlanKPIDetails.Where(pd => (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && pd.FromCriterion == null) ||
                                                          (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();

                                            planDetails2 = new List<PlanKPIDetail>();

                                            //planDetails2 = session.Query<PlanKPIDetail>().Where(pd =>
                                            //   pd.TargetGroupDetail.Id != t.Id && pd.Methods.Count > 0
                                            //   && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) >= 0))).ToList();

                                            result.PlanStaffId = planStaff.Id;

                                            List<PlanKPIDetail> mergetDetails = parentPlanKPIDetail.Where(p => !planDetails.Any(p2 => (p2.FromCriterion != null && p.FromCriterion != null && p2.FromCriterion.Id == p.FromCriterion.Id) || ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();
                                            foreach (PlanKPIDetail pld in mergetDetails)
                                            {
                                                planDetails.Add(pld);
                                            }
                                        }
                                        else
                                            result.PlanStaffId = Guid.Empty;
                                        tg.TargetGroupDetailTypeId = t.TargetGroupDetailType.Id;


                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhoHieuTruong:
                                    {
                                        #region Cho Phó Hiệu trưởng

                                        //List<Criterion> departmentPlanCriterions = session.Query<Criterion>().Where(c => c.Staff.Id == staff.Id && c.TargetGroupDetail.Id == t.Id
                                        //    ).ToList();

                                        List<Criterion> templist = session.Query<Criterion>().Where(c => c.Staff.Id == staff.Id && c.TargetGroupDetail.Id == t.Id).ToList();
                                        departmentPlanCriterions = templist.Where(c => !c.FromPlanKPIDetail.IsDisable && ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Count > 0
                                            && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                          (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                          (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) >= 0))).ToList();


                                        if (planStaff != null)
                                        {

                                            //Lấy planstaff của năm
                                            if (plan.ParentPlan != null && plan.ParentPlan.ParentPlan != null)
                                                parentPlanStaff2 = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Department.Id == staff.Department.Id && ps.PlanKPI.Id == plan.ParentPlan.ParentPlan.Id && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId);
                                            //Lấy planstaff của học kỳ
                                            if (plan.ParentPlan != null)
                                                parentPlanStaff = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Department.Id == staff.Department.Id && ps.PlanKPI.Id == plan.ParentPlan.Id && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == AgentObjectTypeId);

                                            if (parentPlanStaff != null)
                                            {
                                                //Lấy planDetail của học kỳ
                                                parentPlanKPIDetail = parentPlanStaff.PlanKPIDetails.Where(pd =>
                                                    (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                  && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }

                                            if (parentPlanStaff2 != null)
                                            {
                                                //Lấy planDetail của năm học
                                                parentPlanKPIDetail2 = parentPlanStaff2.PlanKPIDetails.Where(pd =>
                                                  (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();
                                            }


                                            //Danh sách tổng hộp các planDetail của học kỳ và năm (ưu tiên học kỳ)
                                            List<PlanKPIDetail> parentMergePlanKPIDetail = parentPlanKPIDetail2.Where(p => !parentPlanKPIDetail.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();

                                            foreach (PlanKPIDetail pkd in parentMergePlanKPIDetail)
                                            {
                                                parentPlanKPIDetail.Add(pkd);
                                            }

                                            planDetails = planStaff.PlanKPIDetails.Where(pd => (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && pd.FromCriterion == null) ||
                                                          (pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id && ControllerHelpers.GetOriginalMethods(pd.Id, session).Count > 0
                                                          && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                  (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id, session).Max(m => m.EndTime)) >= 0)))).ToList();

                                            planDetails2 = new List<PlanKPIDetail>();

                                            //planDetails2 = session.Query<PlanKPIDetail>().Where(pd =>
                                            //   pd.TargetGroupDetail.Id != t.Id && pd.Methods.Count > 0
                                            //   && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) <= 0) ||
                                            //      (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(pd.Id,session).Max(m => m.EndTime)) >= 0))).ToList();

                                            result.PlanStaffId = planStaff.Id;

                                            List<PlanKPIDetail> mergetDetails = parentPlanKPIDetail.Where(p => !planDetails.Any(p2 => (p2.FromCriterion != null && p.FromCriterion != null && p2.FromCriterion.Id == p.FromCriterion.Id) || ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();
                                            foreach (PlanKPIDetail pld in mergetDetails)
                                            {
                                                planDetails.Add(pld);
                                            }
                                        }
                                        else
                                            result.PlanStaffId = Guid.Empty;
                                        tg.TargetGroupDetailTypeId = t.TargetGroupDetailType.Id;

                                        #endregion
                                    }
                                    break;
                            }
                            #endregion //Lấy dữ liệu từ database

                            //--------------------------------------------
                            #region Xử lý dữ liệu
                            //---------------------------------------------
                            switch (AgentObjectTypeId)
                            {
                                //Dành cho giảng viên
                                case (int)AgentObjectTypes.GiangVien:
                                    {
                                        #region giảng viên
                                        planDetails = new List<PlanKPIDetail>();
                                        if (planStaff != null)
                                        {
                                            planDetails = planStaff.PlanKPIDetails.Where(pd => pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id).ToList();// session.Query<PlanKPIDetail>().Where(p => p.TargetGroupDetail.Id == t.Id && p.PlanKPI.Id == plan.Id && p.Staff.Id == staffId).ToList();
                                            result.PlanStaffId = planStaff.Id;
                                        }
                                        else
                                            result.PlanStaffId = Guid.Empty;
                                        tg.PlanKPIDetails = new List<PlanKPIMakingDetailDTO>();
                                        switch (t.TargetGroupDetailType.Id)
                                        {
                                            case 0:
                                                {
                                                    List<ProfessorCriterionPlanDTO> pCriterions = session.Query<ProfessorCriterion>().Where(c => c.TargetGroupDetail.Id == t.Id).Map<ProfessorCriterionPlanDTO>().ToList();
                                                    tg.ProfessorCriterions = pCriterions;

                                                    PlanKPIDetail planDetail = null;
                                                    foreach (ProfessorCriterionPlanDTO crd in tg.ProfessorCriterions)
                                                    {
                                                        planDetail = null;
                                                        PlanKPIMakingDetailDTO pld = new PlanKPIMakingDetailDTO();
                                                        if (planStaff != null)
                                                        {
                                                            planDetail = planDetails.SingleOrDefault(p => p.PlanStaff.Id == planStaff.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.Id == crd.Id);
                                                        }


                                                        if (planDetail != null)
                                                        {
                                                            pld = ControllerHelpers.ParsePlanDetailFromProfessorCriterion(crd, planDetail, null, null, null, session, 1);
                                                        }
                                                        else
                                                        {

                                                            pld = ControllerHelpers.ParsePlanDetailFromProfessorCriterion(crd, planDetail, plan, t, planStaff, session, 2);


                                                            //session.Save(pldetail);
                                                        }


                                                        switch (pld.CriterionTypeId)
                                                        {
                                                            case 2:
                                                                {

                                                                }
                                                                break;
                                                            case 3:
                                                                {
                                                                    List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == crd.Id && d.LevelIndex > 0).Map<CriterionDictionaryDTO>().ToList();
                                                                    pld.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                                    if (planDetail == null)
                                                                        //pld.CurrentKPI = planDetail.CurrentKPI;
                                                                        pld.CurrentKPI = pld.CriterionDictionaries.Count > 0 ? pld.CriterionDictionaries.FirstOrDefault().Id.ToString() : "";
                                                                    pld.OrderNumber = crd.OrderNumber;
                                                                }
                                                                break;
                                                                //case 4:
                                                                //    {
                                                                //        using (var client = new HttpClient())
                                                                //        {
                                                                //            var baseUrl = "";
                                                                //            if (Request.Properties.ContainsKey("MS_HttpContext"))
                                                                //            {
                                                                //                var ip = ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                                                                //                var host = ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.Url.Host;
                                                                //                var port = ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.Url.Port;
                                                                //                baseUrl = string.Format("http://{0}:{1}/", host, port);
                                                                //            }
                                                                //            //string hostName = request.GetRequestContext().VirtualPathRoot;
                                                                //            client.BaseAddress = new Uri(baseUrl);
                                                                //            client.DefaultRequestHeaders.Accept.Clear();
                                                                //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                                                //            // New code:                                                          
                                                                //            HttpResponseMessage response = client.GetAsync(string.Format("{0}?staffId={1}&agentObjectId={2}&requestType=1", crd.ServiceUrl, staff.Id, agentObjectId)).Result;
                                                                //            if (response.IsSuccessStatusCode)
                                                                //            {
                                                                //                string numberOfSection = response.Content.ReadAsStringAsync().Result;
                                                                //                pld.CurrentKPI = numberOfSection;
                                                                //            }
                                                                //        }
                                                                //    }
                                                                //    break;
                                                        }
                                                        pld.Tooltip = crd.Tooltip;

                                                        tg.PlanKPIDetails.Add(pld);
                                                    }
                                                }
                                                break;
                                            case 4:
                                                {
                                                    List<ProfessorCriterionPlanDTO> pCriterions = session.Query<ProfessorCriterion>().Where(c => c.TargetGroupDetail.Id == t.Id).Map<ProfessorCriterionPlanDTO>().ToList();
                                                    tg.ProfessorCriterions = pCriterions;

                                                    PlanKPIDetail planDetail = null;
                                                    foreach (ProfessorCriterionPlanDTO crd in tg.ProfessorCriterions)
                                                    {
                                                        planDetail = null;
                                                        PlanKPIMakingDetailDTO pld = new PlanKPIMakingDetailDTO();
                                                        if (planStaff != null)
                                                        {
                                                            planDetail = planDetails.SingleOrDefault(p => p.PlanStaff.Id == planStaff.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.Id == crd.Id);
                                                        }


                                                        if (planDetail != null)
                                                        {
                                                            pld = ControllerHelpers.ParsePlanDetailFromProfessorCriterion(crd, planDetail, null, null, null, session, 1);
                                                        }
                                                        else
                                                        {

                                                            pld = ControllerHelpers.ParsePlanDetailFromProfessorCriterion(crd, planDetail, plan, t, planStaff, session, 2);
                                                        }


                                                        switch (pld.CriterionTypeId)
                                                        {
                                                            case 3:
                                                                {
                                                                    List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == crd.Id && d.LevelIndex > 0).Map<CriterionDictionaryDTO>().ToList();
                                                                    pld.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                                    if (planDetail == null)
                                                                        //pld.CurrentKPI = planDetail.CurrentKPI;
                                                                        pld.CurrentKPI = pld.CriterionDictionaries.Count > 0 ? pld.CriterionDictionaries.FirstOrDefault().Id.ToString() : "";
                                                                    pld.OrderNumber = crd.OrderNumber;
                                                                }
                                                                break;
                                                        }
                                                        pld.Tooltip = crd.Tooltip;

                                                        tg.PlanKPIDetails.Add(pld);
                                                    }
                                                }
                                                break;
                                            case 5:
                                                {
                                                    List<ProfessorCriterionPlanDTO> pCriterions = session.Query<ProfessorCriterion>().Where(c => c.TargetGroupDetail.Id == t.Id).Map<ProfessorCriterionPlanDTO>().ToList();
                                                    tg.ProfessorCriterions = pCriterions;

                                                    PlanKPIDetail planDetail = null;
                                                    foreach (ProfessorCriterionPlanDTO crd in tg.ProfessorCriterions)
                                                    {
                                                        planDetail = null;
                                                        PlanKPIMakingDetailDTO pld = new PlanKPIMakingDetailDTO();
                                                        if (planStaff != null)
                                                        {
                                                            planDetail = planDetails.SingleOrDefault(p => p.PlanStaff.Id == planStaff.Id && p.FromProfessorCriterion != null && p.FromProfessorCriterion.Id == crd.Id);
                                                        }


                                                        if (planDetail != null)
                                                        {
                                                            pld = ControllerHelpers.ParsePlanDetailFromProfessorCriterion(crd, planDetail, null, null, null, session, 1);
                                                        }
                                                        else
                                                        {



                                                            pld = ControllerHelpers.ParsePlanDetailFromProfessorCriterion(crd, planDetail, plan, t, planStaff, session, 2);


                                                        }


                                                        switch (pld.CriterionTypeId)
                                                        {
                                                            case 3:
                                                                {
                                                                    List<CriterionDictionaryDTO> dictionaryDTOs = session.Query<CriterionDictionary>().Where(d => d.ProfessorCriterion.Id == crd.Id && d.LevelIndex > 0).Map<CriterionDictionaryDTO>().ToList();
                                                                    pld.CriterionDictionaries = dictionaryDTOs.OrderBy(c => c.OrderNumber).ToList();
                                                                    if (planDetail == null)
                                                                        //pld.CurrentKPI = planDetail.CurrentKPI;
                                                                        pld.CurrentKPI = pld.CriterionDictionaries.Count > 0 ? pld.CriterionDictionaries.FirstOrDefault().Id.ToString() : "";
                                                                    pld.OrderNumber = crd.OrderNumber;
                                                                }
                                                                break;
                                                        }
                                                        pld.Tooltip = crd.Tooltip;

                                                        tg.PlanKPIDetails.Add(pld);
                                                    }
                                                }
                                                break;
                                        }

                                        tg.TargetGroupId = t.Id;
                                        tg.TargetGroupName = t.Name;
                                        tg.PlanKPIDetails = tg.PlanKPIDetails.OrderBy(p => p.OrderNumber).ToList();
                                        tg.PlanKPIDetails = tg.PlanKPIDetails.OrderBy(p => p.CreateTime).ToList();
                                        #endregion
                                    }
                                    break;
                                //Dành cho nhân viên
                                case (int)AgentObjectTypes.NhanVien:
                                    {
                                        #region Nhân viên

                                        switch (t.TargetGroupDetailType.Id)
                                        {
                                            case 0:
                                                {

                                                }
                                                break;
                                            case 1:
                                                {
                                                    foreach (Criterion cri in staffPlanCriterions)
                                                    {
                                                        PlanKPIDetail pld = planDetails.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id);
                                                        PlanKPIDetail pld2 = planDetails2.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id && pl.TargetGroupDetail.Id != t.Id);
                                                        if (pld == null)
                                                        {
                                                            PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(null, cri.FromPlanKPIDetail, 2, session);
                                                            PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(cri.FromPlanKPIDetail);
                                                            childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                            childPlan.ParentPlanKPIDetail = cri.FromPlanKPIDetail;
                                                            childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                            childPlan.PlanStaff = planStaff;
                                                            childPlan.Id = Guid.NewGuid();
                                                            childPlan.MaxRecord = pd.MaxRecord;
                                                            childPlan.StaffLeader = cri.FromPlanKPIDetail.StaffLeader != null ? new Staff() { Id = cri.FromPlanKPIDetail.StaffLeader.Id } : null;
                                                            childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                            pd.BasicResource = string.Empty;
                                                            pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                            foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                            {
                                                                PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                                child.Id = Guid.NewGuid();
                                                                child.Name = kpi.Name;
                                                                child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                                child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                                childPlan.PlanKPIDetail_KPIs.Add(child);

                                                                PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                                parent.Id = kpi.Id;
                                                                parent.Name = kpi.Name;
                                                                pd.ParentPlanKPIDetail_KPIs.Add(parent);
                                                            }
                                                            session.Save(childPlan);
                                                            newPlanDetails.Add(childPlan);
                                                            pd.Id = childPlan.Id;

                                                            if (pd.IsLocked == false)
                                                                result.IsDisable = false;
                                                            tg.PlanKPIDetails.Add(pd);
                                                        }
                                                        else if (!pld.IsDelete)
                                                        {
                                                            PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);



                                                            pd.PreviousKPISecond = pld2 != null ? pld2.PreviousKPI : "";

                                                            try
                                                            {
                                                                if (pd.CurrentKPI != null)
                                                                {
                                                                    Guid currentKPIId = new Guid(pd.CurrentKPI);
                                                                    pd.CurrentKPISecond = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == currentKPIId).Select(c => c.Name).SingleOrDefault();
                                                                }
                                                            }
                                                            catch (Exception e)
                                                            {

                                                            }

                                                            if (pd.StaffLeader == null)
                                                            {
                                                                StaffApiController controller = new StaffApiController();
                                                                pd.StaffLeader = new StaffDTO();
                                                                pd.StaffLeader.Id = controller.GetDepartmentLeader(staff.Department.Id).Id;
                                                            }
                                                            pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                            PlanKPIDetailDTO parentpl = ControllerHelpers.GetParentPlanKPIDetail(pd.Id);
                                                            foreach (PlanKPIDetail_KPIDTO kpi in parentpl.PlanKPIDetail_KPIs)
                                                            {
                                                                PlanKPIDetail_KPIDTO kd = new PlanKPIDetail_KPIDTO();
                                                                kd.Id = kpi.Id;
                                                                kd.Name = kpi.Name;
                                                                pd.ParentPlanKPIDetail_KPIs.Add(kd);
                                                            }

                                                            if (pd.IsLocked == false)
                                                                result.IsDisable = false;
                                                            tg.PlanKPIDetails.Add(pd);
                                                        }
                                                    }
                                                    List<PlanKPIDetail> OutDeleteDetails = planDetails.Where(p => p.FromCriterion != null && !staffPlanCriterions.Any(dp => dp.Id == p.FromCriterion.Id)).ToList();
                                                    foreach (PlanKPIDetail pld in OutDeleteDetails)
                                                    {
                                                        PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                        pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);

                                                        if (pd.IsLocked == false)
                                                            result.IsDisable = false;
                                                        tg.PlanKPIDetails.Add(pd);
                                                    }
                                                    //Danh sách planDetail tự tạo không kế thừa cấp trên
                                                    List<PlanKPIDetail> CanDeleteDetails = planDetails.Where(p => p.FromCriterion == null).ToList();
                                                    foreach (PlanKPIDetail pld in CanDeleteDetails)
                                                    {

                                                        PlanKPIDetail existPld = planStaff.PlanKPIDetails.SingleOrDefault(pl => ControllerHelpers.GetOriginalParentPlanKPIDetail(pl.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).Id);
                                                        //Nếu không tồn tại thì tạo mới plandetail
                                                        PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                        if (existPld == null)
                                                        {
                                                            pd = new PlanKPIMakingDetailDTO();
                                                            pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);


                                                            PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                            childPlan.ParentPlanKPIDetail = pld;
                                                            childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                            childPlan.PlanStaff = planStaff;
                                                            childPlan.Id = Guid.NewGuid();
                                                            childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                            foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                            {
                                                                PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                                child.Id = Guid.NewGuid();
                                                                child.Name = kpi.Name;
                                                                child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                                child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                                childPlan.PlanKPIDetail_KPIs.Add(child);
                                                            }
                                                            session.Save(childPlan);
                                                            newPlanDetails.Add(childPlan);
                                                            pd.Id = childPlan.Id;

                                                            if (pd.IsLocked == false)
                                                                result.IsDisable = false;
                                                            tg.PlanKPIDetails.Add(pd);
                                                        }
                                                        else
                                                        {
                                                            pd = new PlanKPIMakingDetailDTO();
                                                            pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);

                                                            if (pd.IsLocked == false)
                                                                result.IsDisable = false;
                                                            tg.PlanKPIDetails.Add(pd);
                                                        }
                                                        if (pd.CurrentKPI != null)
                                                        {
                                                            Guid currentKPIId = new Guid(pd.CurrentKPI);
                                                            pd.CurrentKPISecond = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == currentKPIId).Select(c => c.Name).SingleOrDefault();
                                                        }

                                                    }
                                                }
                                                break;
                                            case 2:
                                                {

                                                }
                                                break;
                                            case 3:
                                                {
                                                    Criterion staffPlanCriterion = session.Query<Criterion>().SingleOrDefault(c => (c.Department.Id == staff.Department.Id)
                                                        && c.TargetGroupDetail.TargetGroupDetailType.Id == 3
                                                        && c.TargetGroupDetail.Id == t.Id
                                                       && !c.FromPlanKPIDetail.IsDisable);

                                                    List<PlanKPIDetail> parentPlanKPIDetailType3 = parentPlanStaff != null ? parentPlanStaff.PlanKPIDetails.Where(pd => pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id).ToList() : new List<PlanKPIDetail>();
                                                    List<PlanKPIDetail> parentPlanKPIDetail2Type3 = parentPlanStaff2 != null ? parentPlanStaff2.PlanKPIDetails.Where(pd => pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id).ToList() : new List<PlanKPIDetail>();

                                                    List<PlanKPIDetail> parentMergePlanKPIDetail = parentPlanKPIDetail2Type3.Where(p => !parentPlanKPIDetailType3.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();

                                                    foreach (PlanKPIDetail pldk in parentMergePlanKPIDetail)
                                                    {
                                                        parentPlanKPIDetailType3.Add(pldk);
                                                    }

                                                    planDetails = planStaff.PlanKPIDetails.Where(pd => pd.TargetGroupDetail != null &&
                                                          pd.TargetGroupDetail.Id == t.Id).ToList();

                                                    if (planDetails.Count <= 0)
                                                    {
                                                        List<PlanKPIDetail> planDetailMerges = planDetails.Where(p => !parentMergePlanKPIDetail.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();
                                                        //foreach (PlanKPIDetail plk in planDetailMerges)
                                                        //    planDetails.Add(plk);
                                                        PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                        if (parentPlanKPIDetailType3.Count > 0)
                                                        {
                                                            foreach (PlanKPIDetail pld in parentPlanKPIDetailType3)
                                                            {

                                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);

                                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                                childPlan.ParentPlanKPIDetail = new PlanKPIDetail() { Id = pld.Id };
                                                                childPlan.PlanStaff = planStaff;

                                                                childPlan.Id = Guid.NewGuid();

                                                                childPlan.CreateTime = DateTime.Now;

                                                                session.Save(childPlan);
                                                                newPlanDetails.Add(childPlan);
                                                                pd.Id = childPlan.Id;
                                                                if (pd.TargetDetail != null)
                                                                {
                                                                    Guid targetDetailId = new Guid(pd.TargetDetail);
                                                                    pd.TargetDetailName = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == targetDetailId).Select(c => c.Name).SingleOrDefault();
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (staffPlanCriterion != null)
                                                            {
                                                                pd = ControllerHelpers.ParsePlanDetail(null, staffPlanCriterion.FromPlanKPIDetail, 2, session);

                                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(staffPlanCriterion.FromPlanKPIDetail);
                                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                                childPlan.ParentPlanKPIDetail = new PlanKPIDetail() { Id = staffPlanCriterion.FromPlanKPIDetail.Id };
                                                                childPlan.PlanStaff = planStaff;

                                                                childPlan.Id = Guid.NewGuid();

                                                                childPlan.CreateTime = DateTime.Now;
                                                                childPlan.StaffLeader = staffPlanCriterion.FromPlanKPIDetail.StaffLeader != null ? new Staff() { Id = staffPlanCriterion.FromPlanKPIDetail.StaffLeader.Id } : null;
                                                                session.Evict(staffPlanCriterion.FromPlanKPIDetail);
                                                                session.Save(childPlan);
                                                                newPlanDetails.Add(childPlan);
                                                                pd.Id = childPlan.Id;
                                                            }
                                                            else
                                                            {
                                                                pd.Id = Guid.Empty;
                                                                pd.CriterionDictionaries = dictionaries;
                                                                pd.StartTime = plan.StartTime;
                                                                pd.EndTime = plan.EndTime;
                                                                pd.CanDelete = false;

                                                                PlanKPIDetail childPlan = new PlanKPIDetail();
                                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                                childPlan.StartTime = DateTime.Now;
                                                                childPlan.EndTime = DateTime.Now;
                                                                childPlan.PlanStaff = planStaff;
                                                                childPlan.Id = Guid.NewGuid();

                                                                childPlan.CreateTime = DateTime.Now;

                                                                session.Save(childPlan);
                                                                newPlanDetails.Add(childPlan);
                                                                pd.Id = childPlan.Id;
                                                            }
                                                            if (pd.TargetDetail != null)
                                                            {
                                                                Guid targetDetailId = new Guid(pd.TargetDetail);
                                                                pd.TargetDetailName = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == targetDetailId).Select(c => c.Name).SingleOrDefault();
                                                            }
                                                        }

                                                        if (pd.IsLocked == false)
                                                            result.IsDisable = false;
                                                        tg.PlanKPIDetails.Add(pd);
                                                    }
                                                    else
                                                    {
                                                        PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                        //PlanKPIDetail pld = planDetails.FirstOrDefault(pl => pl.PlanStaff.Id == planStaff.Id != null && pl.TargetGroupDetail.Id == t.Id);
                                                        pd = ControllerHelpers.ParsePlanDetail(null, planDetails.First(), 2, session);
                                                        pd.CriterionDictionaries = dictionaries;
                                                        pd.CanDelete = false;
                                                        if (pd.TargetDetail != null)
                                                        {
                                                            Guid targetDetailId = new Guid(pd.TargetDetail);
                                                            pd.TargetDetailName = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == targetDetailId).Select(c => c.Name).SingleOrDefault();
                                                        }

                                                        if (pd.IsLocked == false)
                                                            result.IsDisable = false;
                                                        tg.PlanKPIDetails.Add(pd);
                                                    }
                                                }
                                                break;
                                        }
                                        tg.PlanKPIDetails = tg.PlanKPIDetails.OrderBy(p => p.CreateTime).ToList();

                                        tg.TargetGroupId = t.Id;
                                        tg.TargetGroupName = t.Name;



                                        //criterions = session.Query<Criterion>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Department.Id == staff.Department.Id && c.PlanKPI.Id == planId).Map<CriterionPlanDTO>().ToList();
                                        //tg.Criterions = criterions;
                                        #endregion
                                    }
                                    break;
                                //Dành cho trưởng phòng ban
                                case (int)AgentObjectTypes.PhongBan:
                                    {
                                        #region Cho Trưởng phòng ban
                                        switch (t.TargetGroupDetailType.Id)
                                        {
                                            case 0:
                                                {

                                                }
                                                break;
                                            case 1:
                                                {
                                                    foreach (Criterion cri in departmentPlanCriterions)
                                                    {
                                                        PlanKPIDetail pld = planDetails.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id);
                                                        PlanKPIDetail pld2 = planDetails2.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id && pl.TargetGroupDetail.Id != t.Id);
                                                        if (pld == null)
                                                        {
                                                            PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(null, cri.FromPlanKPIDetail, 2, session);
                                                            PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(cri.FromPlanKPIDetail);
                                                            childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                            childPlan.ParentPlanKPIDetail = cri.FromPlanKPIDetail;
                                                            childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                            childPlan.PlanStaff = planStaff;
                                                            childPlan.Id = Guid.NewGuid();
                                                            childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                            childPlan.StaffLeader = StaffApiController.GetStaffDepartmentLeader(staff.Department.Id);
                                                            pd.BasicResource = string.Empty;
                                                            pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                            foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                            {
                                                                PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                                child.Id = Guid.NewGuid();
                                                                child.Name = kpi.Name;
                                                                child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                                child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                                childPlan.PlanKPIDetail_KPIs.Add(child);

                                                                PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                                parent.Id = kpi.Id;
                                                                parent.Name = kpi.Name;
                                                                pd.ParentPlanKPIDetail_KPIs.Add(parent);
                                                            }

                                                            session.Save(childPlan);
                                                            newPlanDetails.Add(childPlan);


                                                            pd.Id = childPlan.Id;
                                                            if (childPlan.ParentPlanKPIDetail.StaffLeader != null)
                                                            {
                                                                pd.AdminLeaderId = childPlan.ParentPlanKPIDetail.StaffLeader.Id;
                                                                pd.AdminLeaderName = childPlan.ParentPlanKPIDetail.StaffLeader.StaffProfile.Name;
                                                            }

                                                            //mặc định ban đầu chỉ đạo là bản thân
                                                            pd.StaffLeader = new StaffDTO();
                                                            pd.StaffLeader.Id = staff.Id;
                                                            if (staff.Id != Guid.Empty)
                                                                pd.StaffLeader.StaffProfile = new StaffProfile() { Id = staff.StaffProfile.Id, Name = staff.StaffProfile.Name };

                                                            //if (pd.StaffLeader == null)
                                                            //{
                                                            //    StaffApiController controller = new StaffApiController();
                                                            //    pd.StaffLeader = new StaffDTO();
                                                            //    pd.StaffLeader.Id = controller.GetDepartmentLeader(staff.Department.Id).Id;
                                                            //}
                                                            if (pd.IsLocked == false)
                                                                result.IsDisable = false;
                                                            tg.PlanKPIDetails.Add(pd);
                                                        }
                                                        else if (!pld.IsDelete)
                                                        {
                                                            PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                            if (pld.PlanStaff.Id != planStaff.Id)
                                                            {
                                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                                childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                                childPlan.ParentPlanKPIDetail = pld;
                                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                                childPlan.PlanStaff = planStaff;
                                                                childPlan.Id = Guid.NewGuid();
                                                                childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                                childPlan.StaffLeader = new Staff { Id = staff.Id };

                                                                foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                                {
                                                                    PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                                    child.Id = Guid.NewGuid();
                                                                    child.Name = kpi.Name;
                                                                    child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                                    child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                                    childPlan.PlanKPIDetail_KPIs.Add(child);

                                                                    PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                                    parent.Id = kpi.Id;
                                                                    parent.Name = kpi.Name;

                                                                }
                                                                if (parentPlanKPIDetail.Any(pp => pp.Id == pld.Id))
                                                                {
                                                                    foreach (Staff stf in pld.SubStaffs)
                                                                    {
                                                                        childPlan.SubStaffs.Add(stf);
                                                                    }
                                                                    //foreach (Staff stf in childPlan.SubStaffs)
                                                                    //{
                                                                    //    //pd.SubDepartmentIds.Add(subd.Id);
                                                                    //    pd.SubStaffNames.Add(stf.StaffProfile.Name);
                                                                    //}
                                                                }
                                                                session.Save(childPlan);
                                                                newPlanDetails.Add(childPlan);

                                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                                                pd.Id = childPlan.Id;
                                                            }
                                                            else
                                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);



                                                            pd.PreviousKPISecond = pld2 != null ? pld2.PreviousKPI : "";

                                                            try
                                                            {
                                                                if (pd.CurrentKPI != null)
                                                                {
                                                                    Guid currentKPIId = new Guid(pd.CurrentKPI);
                                                                    pd.CurrentKPISecond = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == currentKPIId).Select(c => c.Name).SingleOrDefault();
                                                                }
                                                            }
                                                            catch (Exception e)
                                                            {

                                                            }

                                                            if (pd.StaffLeader == null)
                                                            {
                                                                StaffApiController controller = new StaffApiController();
                                                                pd.StaffLeader = new StaffDTO();
                                                                pd.StaffLeader.Id = controller.GetDepartmentLeader(staff.Department.Id).Id;
                                                            }
                                                            pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                            PlanKPIDetailDTO parentpl = ControllerHelpers.GetParentPlanKPIDetail(pld.Id, session);
                                                            foreach (PlanKPIDetail_KPIDTO kpi in parentpl.PlanKPIDetail_KPIs)
                                                            {
                                                                PlanKPIDetail_KPIDTO kd = new PlanKPIDetail_KPIDTO();
                                                                kd.Id = kpi.Id;
                                                                kd.Name = kpi.Name;
                                                                pd.ParentPlanKPIDetail_KPIs.Add(kd);
                                                            }
                                                            if (parentpl.StaffLeader != null)
                                                            {
                                                                pd.AdminLeaderId = parentpl.StaffLeader.Id;
                                                                pd.AdminLeaderName = parentpl.StaffLeader.StaffProfile.Name;
                                                            }
                                                            //foreach (Staff substaff in pld.SubStaffs)
                                                            //{
                                                            //    pd.SubStaffNames.Add(substaff.StaffProfile.Name);
                                                            //}
                                                            if (pd.IsLocked == false)
                                                                result.IsDisable = false;
                                                            tg.PlanKPIDetails.Add(pd);
                                                        }
                                                    }
                                                    List<PlanKPIDetail> OutDeleteDetails = planDetails.Where(p => p.FromCriterion != null && !departmentPlanCriterions.Any(dp => dp.Id == p.FromCriterion.Id)).ToList();
                                                    foreach (PlanKPIDetail pld in OutDeleteDetails)
                                                    {
                                                        PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                        pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                                        if (pd.IsLocked == false)
                                                            result.IsDisable = false;
                                                        tg.PlanKPIDetails.Add(pd);
                                                    }
                                                    //Danh sách planDetail tự tạo không kế thừa cấp trên
                                                    List<PlanKPIDetail> CanDeleteDetails = planDetails.Where(p => p.FromCriterion == null).ToList();
                                                    foreach (PlanKPIDetail pld in CanDeleteDetails)
                                                    {

                                                        PlanKPIDetail existPld = planStaff.PlanKPIDetails.SingleOrDefault(pl => ControllerHelpers.GetOriginalParentPlanKPIDetail(pl.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).Id);
                                                        //Nếu không tồn tại thì tạo mới plandetail
                                                        PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                        if (existPld == null)
                                                        {
                                                            pd = new PlanKPIMakingDetailDTO();
                                                            pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);


                                                            PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                            childPlan.ParentPlanKPIDetail = pld;
                                                            childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                            childPlan.PlanStaff = planStaff;
                                                            childPlan.Id = Guid.NewGuid();
                                                            childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                            foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                            {
                                                                PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                                child.Id = Guid.NewGuid();
                                                                child.Name = kpi.Name;
                                                                child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                                child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                                childPlan.PlanKPIDetail_KPIs.Add(child);
                                                            }
                                                            session.Save(childPlan);
                                                            newPlanDetails.Add(childPlan);
                                                            pd.Id = childPlan.Id;

                                                            if (pd.IsLocked == false)
                                                                result.IsDisable = false;
                                                            tg.PlanKPIDetails.Add(pd);
                                                        }
                                                        else
                                                        {
                                                            pd = new PlanKPIMakingDetailDTO();
                                                            pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);

                                                            if (pd.IsLocked == false)
                                                                result.IsDisable = false;
                                                            tg.PlanKPIDetails.Add(pd);
                                                        }

                                                        if (pd.StaffLeader == null)
                                                        {
                                                            StaffApiController controller = new StaffApiController();
                                                            pd.StaffLeader = new StaffDTO();
                                                            pd.StaffLeader.Id = controller.GetDepartmentLeader(staff.Department.Id).Id;
                                                        }
                                                        foreach (Staff substaff in pld.SubStaffs)
                                                        {
                                                            pd.SubStaffNames.Add(substaff.StaffProfile.Name);
                                                        }
                                                        if (pd.CurrentKPI != null)
                                                        {
                                                            Guid currentKPIId = new Guid(pd.CurrentKPI);
                                                            pd.CurrentKPISecond = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == currentKPIId).Select(c => c.Name).SingleOrDefault();
                                                        }

                                                    }
                                                }
                                                break;
                                            case 2:
                                                {

                                                }
                                                break;
                                            case 3:
                                                {

                                                    List<PlanKPIDetail> parentPlanKPIDetailType3 = parentPlanStaff != null ? parentPlanStaff.PlanKPIDetails.Where(pd => pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id).ToList() : new List<PlanKPIDetail>();
                                                    List<PlanKPIDetail> parentPlanKPIDetail2Type3 = parentPlanStaff2 != null ? parentPlanStaff2.PlanKPIDetails.Where(pd => pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id).ToList() : new List<PlanKPIDetail>();

                                                    List<PlanKPIDetail> parentMergePlanKPIDetail = parentPlanKPIDetail2Type3.Where(p => !parentPlanKPIDetailType3.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();

                                                    foreach (PlanKPIDetail pldk in parentMergePlanKPIDetail)
                                                    {
                                                        parentPlanKPIDetailType3.Add(pldk);
                                                    }

                                                    planDetails = planStaff.PlanKPIDetails.Where(pd => pd.TargetGroupDetail != null &&
                                                          pd.TargetGroupDetail.Id == t.Id).ToList();

                                                    if (planDetails.Count <= 0)
                                                    {
                                                        List<PlanKPIDetail> planDetailMerges = planDetails.Where(p => !parentMergePlanKPIDetail.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();
                                                        //foreach (PlanKPIDetail plk in planDetailMerges)
                                                        //    planDetails.Add(plk);
                                                        PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                        if (parentPlanKPIDetailType3.Count > 0)
                                                        {
                                                            foreach (PlanKPIDetail pld in parentPlanKPIDetailType3)
                                                            {

                                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);

                                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };

                                                                //childPlan.ParentPlanKPIDetail = new PlanKPIDetail() { Id = pld.Id };
                                                                childPlan.PlanStaff = planStaff;
                                                                childPlan.CreateTime = DateTime.Now;
                                                                childPlan.Id = Guid.NewGuid();

                                                                childPlan.CreateTime = DateTime.Now;

                                                                //session.Evict(pld);
                                                                session.Save(childPlan);
                                                                //session.Merge(pld);
                                                                newPlanDetails.Add(childPlan);
                                                                pd.Id = childPlan.Id;

                                                            }
                                                        }
                                                        else
                                                        {
                                                            pd.Id = Guid.Empty;
                                                            pd.CriterionDictionaries = dictionaries;
                                                            pd.StartTime = plan.StartTime;
                                                            pd.EndTime = plan.EndTime;
                                                            pd.CanDelete = false;


                                                            PlanKPIDetail childPlan = new PlanKPIDetail();
                                                            childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                            childPlan.StartTime = DateTime.Now;
                                                            childPlan.EndTime = DateTime.Now;
                                                            childPlan.PlanStaff = planStaff;
                                                            childPlan.Id = Guid.NewGuid();

                                                            childPlan.CreateTime = DateTime.Now;

                                                            session.Save(childPlan);
                                                            newPlanDetails.Add(childPlan);
                                                            pd.Id = childPlan.Id;

                                                        }
                                                        if (pd.StaffLeader == null)
                                                        {
                                                            StaffApiController controller = new StaffApiController();
                                                            pd.StaffLeader = new StaffDTO();
                                                            pd.StaffLeader.Id = controller.GetDepartmentLeader(staff.Department.Id).Id;
                                                        }
                                                        if (pd.IsLocked == false)
                                                            result.IsDisable = false;
                                                        tg.PlanKPIDetails.Add(pd);
                                                    }
                                                    else
                                                    {
                                                        PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                        //PlanKPIDetail pld = planDetails.FirstOrDefault(pl => pl.PlanStaff.Id == planStaff.Id != null && pl.TargetGroupDetail.Id == t.Id);
                                                        pd = ControllerHelpers.ParsePlanDetail(null, planDetails.First(), 2, session);
                                                        pd.CriterionDictionaries = dictionaries;
                                                        pd.CanDelete = false;
                                                        foreach (Staff substaff in planDetails.First().SubStaffs)
                                                        {
                                                            pd.SubStaffNames.Add(substaff.StaffProfile.Name);
                                                        }
                                                        if (pd.TargetDetail != null)
                                                        {
                                                            Guid targetDetailId = new Guid(pd.TargetDetail);
                                                            pd.TargetDetailName = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == targetDetailId).Select(c => c.Name).SingleOrDefault();
                                                        }
                                                        if (pd.IsLocked == false)
                                                            result.IsDisable = false;
                                                        tg.PlanKPIDetails.Add(pd);
                                                    }
                                                }
                                                break;
                                        }
                                        tg.PlanKPIDetails = tg.PlanKPIDetails.OrderBy(p => p.CreateTime).ToList();

                                        tg.TargetGroupId = t.Id;
                                        tg.TargetGroupName = t.Name;


                                        //criterions = session.Query<Criterion>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Department.Id == staff.Department.Id && c.PlanKPI.Id == planId).Map<CriterionPlanDTO>().ToList();
                                        //tg.Criterions = criterions;
                                        #endregion
                                    }
                                    break;
                                //Ban giám hiệu
                                case (int)AgentObjectTypes.BanGiamHieu:
                                    {
                                        #region Ban Giam hiệu                                                                            
                                        foreach (PlanKPIDetail pld in planDetails)
                                        {
                                            PlanKPIDetail pld1 = planStaff.PlanKPIDetails.SingleOrDefault(pl => pl.Id == pld.Id);
                                            if (pld1 == null)
                                            {
                                                PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);

                                                foreach (Department subd in pld.SubDepartments)
                                                {
                                                    //pd.SubDepartmentIds.Add(subd.Id);
                                                    pd.SubDepartmentNames.Add(subd.Name);
                                                }
                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                            else if (!pld1.IsDelete)
                                            {
                                                PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                                foreach (Department dep in pld.SubDepartments)
                                                {
                                                    pd.SubDepartmentNames.Add(dep.Name);
                                                }
                                                if (parentPlanKPIDetail.Any(pp => pp.Id == pld.Id))
                                                {
                                                    PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                    childPlan.ParentPlanKPIDetail = pld;
                                                    childPlan.PlanStaff = planStaff;
                                                    childPlan.Id = Guid.NewGuid();

                                                    foreach (Department dep in pld.SubDepartments)
                                                    {
                                                        childPlan.SubDepartments.Add(dep);
                                                    }
                                                    foreach (Department subd in childPlan.SubDepartments)
                                                    {
                                                        //pd.SubDepartmentIds.Add(subd.Id);
                                                        pd.SubDepartmentNames.Add(subd.Name);
                                                    }
                                                    session.Save(childPlan);
                                                    newPlanDetails.Add(childPlan);

                                                    pd.Id = childPlan.Id;
                                                    pd.ParentPlanKPIDetailId = pld.Id;
                                                    
                                                }
                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                        }
                                        tg.PlanKPIDetails = tg.PlanKPIDetails.OrderBy(p => p.CreateTime).ToList();

                                        tg.TargetGroupId = t.Id;
                                        tg.TargetGroupName = t.Name;

                                        criterions = session.Query<Criterion>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Department.Id == staff.Department.Id && c.PlanKPI.Id == planId).Map<CriterionPlanDTO>().ToList();
                                        tg.Criterions = criterions;
                                        #endregion
                                    }
                                    break;
                                //Khoa/Viện/Trung tâm
                                case (int)AgentObjectTypes.Khoa:
                                    {
                                        #region Cho Khoa/trung tâm


                                        #region loại thường
                                        //Lấy từ criterion lúc chưa có kế hoạch
                                        foreach (Criterion cri in departmentPlanCriterions)
                                        {
                                            PlanKPIDetail pld = planDetails.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id);
                                            PlanKPIDetail pld2 = planDetails2.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id && pl.TargetGroupDetail.Id != t.Id);
                                            if (pld == null)
                                            {
                                                PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(cri, null, 1, session);
                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(cri.FromPlanKPIDetail);
                                                childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                childPlan.ParentPlanKPIDetail = cri.FromPlanKPIDetail;
                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                childPlan.PlanStaff = planStaff;
                                                childPlan.Id = Guid.NewGuid();
                                                childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                childPlan.StaffLeader = new Staff { Id = staff.Id };
                                                pd.BasicResource = string.Empty;
                                                pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                    child.Id = Guid.NewGuid();
                                                    child.Name = kpi.Name;
                                                    child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                    child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                    childPlan.PlanKPIDetail_KPIs.Add(child);

                                                    PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                    parent.Id = kpi.Id;
                                                    parent.Name = kpi.Name;
                                                    pd.ParentPlanKPIDetail_KPIs.Add(parent);
                                                }
                                                //if (parentPlanKPIDetail.Any(pp => pp.Id == pld.Id))
                                                //{
                                                //    foreach (Staff stf in pld.SubStaffs)
                                                //    {
                                                //        childPlan.SubStaffs.Add(stf);
                                                //    }

                                                //    foreach (Department dep in pld.SubDepartments)
                                                //    {
                                                //        childPlan.SubDepartments.Add(dep);
                                                //    }

                                                //}
                                                session.Save(childPlan);
                                                newPlanDetails.Add(childPlan);
                                                pd.Id = childPlan.Id;
                                                if (childPlan.ParentPlanKPIDetail.StaffLeader != null)
                                                {
                                                    pd.AdminLeaderId = childPlan.ParentPlanKPIDetail.StaffLeader.Id;
                                                    pd.AdminLeaderName = childPlan.ParentPlanKPIDetail.StaffLeader.StaffProfile.Name;
                                                }
                                                //mặc định ban đầu chỉ đạo là bản thân
                                                pd.StaffLeader = new StaffDTO();
                                                pd.StaffLeader.Id = staff.Id;
                                                pd.StaffLeader.StaffProfile = new StaffProfile() { Id = staff.StaffProfile.Id, Name = staff.StaffProfile.Name };
                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                            else if (!pld.IsDelete)
                                            {
                                                PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                if (pld.PlanStaff.Id != planStaff.Id)
                                                {
                                                    PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                    childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                    childPlan.ParentPlanKPIDetail = pld;
                                                    childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                    childPlan.PlanStaff = planStaff;
                                                    childPlan.Id = Guid.NewGuid();
                                                    childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                    childPlan.StaffLeader = new Staff { Id = staff.Id };

                                                    foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                    {
                                                        PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                        child.Id = Guid.NewGuid();
                                                        child.Name = kpi.Name;
                                                        child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                        child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                        childPlan.PlanKPIDetail_KPIs.Add(child);

                                                        PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                        parent.Id = kpi.Id;
                                                        parent.Name = kpi.Name;

                                                    }
                                                    if (parentPlanKPIDetail.Any(pp => pp.Id == pld.Id))
                                                    {
                                                        foreach (Staff stf in pld.SubStaffs)
                                                        {
                                                            childPlan.SubStaffs.Add(stf);
                                                        }
                                                        //foreach (Staff stf in childPlan.SubStaffs)
                                                        //{
                                                        //    //pd.SubDepartmentIds.Add(subd.Id);
                                                        //    pd.SubStaffNames.Add(stf.StaffProfile.Name);
                                                        //}
                                                    }
                                                    session.Save(childPlan);
                                                    newPlanDetails.Add(childPlan);

                                                    pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                                    pd.Id = childPlan.Id;
                                                }
                                                else
                                                    pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);



                                                pd.PreviousKPISecond = pld2 != null ? pld2.PreviousKPI : "";

                                                try
                                                {
                                                    if (pd.CurrentKPI != null)
                                                    {
                                                        Guid currentKPIId = new Guid(pd.CurrentKPI);
                                                        pd.CurrentKPISecond = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == currentKPIId).Select(c => c.Name).SingleOrDefault();
                                                    }
                                                }
                                                catch (Exception e)
                                                {

                                                }

                                                if (pd.StaffLeader == null)
                                                {
                                                    StaffApiController controller = new StaffApiController();
                                                    pd.StaffLeader = new StaffDTO();
                                                    pd.StaffLeader.Id = controller.GetDepartmentLeader(staff.Department.Id).Id;
                                                }
                                                pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                PlanKPIDetailDTO parentpl = ControllerHelpers.GetParentPlanKPIDetail(pld.Id, session);
                                                foreach (PlanKPIDetail_KPIDTO kpi in parentpl.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPIDTO kd = new PlanKPIDetail_KPIDTO();
                                                    kd.Id = kpi.Id;
                                                    kd.Name = kpi.Name;
                                                    pd.ParentPlanKPIDetail_KPIs.Add(kd);
                                                }
                                                if (parentpl.StaffLeader != null)
                                                {
                                                    pd.AdminLeaderId = parentpl.StaffLeader.Id;
                                                    pd.AdminLeaderName = parentpl.StaffLeader.StaffProfile.Name;
                                                }
                                                //foreach (Staff substaff in pld.SubStaffs)
                                                //{
                                                //    pd.SubStaffNames.Add(substaff.StaffProfile.Name);
                                                //}
                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                        }
                                        //Danh sách planDetail tự tạo không kế thừa cấp trên
                                        List<PlanKPIDetail> CanDeleteDetails = planDetails.Where(p => p.FromCriterion == null).ToList();
                                        foreach (PlanKPIDetail pld in CanDeleteDetails)
                                        {

                                            PlanKPIDetail existPld = planStaff.PlanKPIDetails.SingleOrDefault(pl => ControllerHelpers.GetOriginalParentPlanKPIDetail(pl.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).Id);
                                            //Nếu không tồn tại thì tạo mới plandetail
                                            PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                            if (existPld == null)
                                            {
                                                pd = new PlanKPIMakingDetailDTO();
                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);


                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                childPlan.ParentPlanKPIDetail = pld;
                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                childPlan.PlanStaff = planStaff;
                                                childPlan.Id = Guid.NewGuid();
                                                childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                    child.Id = Guid.NewGuid();
                                                    child.Name = kpi.Name;
                                                    child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                    child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                    childPlan.PlanKPIDetail_KPIs.Add(child);
                                                }

                                                session.Save(childPlan);
                                                newPlanDetails.Add(childPlan);
                                                pd.Id = childPlan.Id;

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                            else
                                            {
                                                pd = new PlanKPIMakingDetailDTO();
                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                                //Gắn rỗng cho cột đơn vị phối hợp thực hiện
                                                pd.SubDepartmentNames = new List<string>();

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                        }
                                        #endregion
                                        tg.PlanKPIDetails = tg.PlanKPIDetails.OrderBy(p => p.CreateTime).ToList();

                                        tg.TargetGroupId = t.Id;
                                        tg.TargetGroupName = t.Name;
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.BoMon:
                                    {
                                        #region BỘ MÔN

                                        #region loại thường
                                        foreach (Criterion cri in departmentPlanCriterions)
                                        {
                                            PlanKPIDetail pld = planDetails.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id);
                                            PlanKPIDetail pld2 = planDetails2.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id && pl.TargetGroupDetail.Id != t.Id);
                                            if (pld == null)
                                            {
                                                PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(cri, null, 1, session);
                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(cri.FromPlanKPIDetail);
                                                childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                childPlan.ParentPlanKPIDetail = cri.FromPlanKPIDetail;
                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                childPlan.PlanStaff = planStaff;
                                                childPlan.Id = Guid.NewGuid();
                                                childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                childPlan.StaffLeader = cri.FromPlanKPIDetail.StaffLeader != null ? new Staff() { Id = cri.FromPlanKPIDetail.StaffLeader.Id } : null;
                                                pd.BasicResource = string.Empty;
                                                pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                    child.Id = Guid.NewGuid();
                                                    child.Name = kpi.Name;
                                                    child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                    child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                    childPlan.PlanKPIDetail_KPIs.Add(child);

                                                    PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                    parent.Id = kpi.Id;
                                                    parent.Name = kpi.Name;
                                                    pd.ParentPlanKPIDetail_KPIs.Add(parent);
                                                }
                                                //if (parentPlanKPIDetail.Any(pp => pp.Id == pld.Id))
                                                //{
                                                //    foreach (Staff stf in pld.SubStaffs)
                                                //    {
                                                //        childPlan.SubStaffs.Add(stf);
                                                //    }
                                                //}
                                                session.Save(childPlan);
                                                newPlanDetails.Add(childPlan);
                                                pd.Id = childPlan.Id;

                                                //Kế hoạch mới từ criterion mặc định substaff rỗng
                                                pd.SubStaffNames = new List<string>();

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                            else if (!pld.IsDelete)
                                            {
                                                PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                if (pld.PlanStaff.Id != planStaff.Id)
                                                {
                                                    PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                    childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                    childPlan.ParentPlanKPIDetail = pld;
                                                    childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                    childPlan.PlanStaff = planStaff;
                                                    childPlan.Id = Guid.NewGuid();
                                                    childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                    childPlan.StaffLeader = new Staff { Id = staff.Id };

                                                    foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                    {
                                                        PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                        child.Id = Guid.NewGuid();
                                                        child.Name = kpi.Name;
                                                        child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                        child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                        childPlan.PlanKPIDetail_KPIs.Add(child);

                                                        PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                        parent.Id = kpi.Id;
                                                        parent.Name = kpi.Name;

                                                    }
                                                    if (parentPlanKPIDetail.Any(pp => pp.Id == pld.Id))
                                                    {
                                                        foreach (Staff stf in pld.SubStaffs)
                                                        {
                                                            childPlan.SubStaffs.Add(stf);
                                                        }
                                                        //foreach (Staff stf in childPlan.SubStaffs)
                                                        //{
                                                        //    //pd.SubDepartmentIds.Add(subd.Id);
                                                        //    pd.SubStaffNames.Add(stf.StaffProfile.Name);
                                                        //}
                                                    }
                                                    session.Save(childPlan);
                                                    newPlanDetails.Add(childPlan);

                                                    pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                                    pd.Id = childPlan.Id;
                                                }
                                                else
                                                    pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);



                                                pd.PreviousKPISecond = pld2 != null ? pld2.PreviousKPI : "";

                                                try
                                                {
                                                    if (pd.CurrentKPI != null)
                                                    {
                                                        Guid currentKPIId = new Guid(pd.CurrentKPI);
                                                        pd.CurrentKPISecond = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == currentKPIId).Select(c => c.Name).SingleOrDefault();
                                                    }
                                                }
                                                catch (Exception e)
                                                {

                                                }

                                                if (pd.StaffLeader == null)
                                                {
                                                    StaffApiController controller = new StaffApiController();
                                                    pd.StaffLeader = new StaffDTO();
                                                    pd.StaffLeader.Id = controller.GetDepartmentLeader(staff.Department.Id).Id;
                                                }
                                                pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                PlanKPIDetailDTO parentpl = ControllerHelpers.GetParentPlanKPIDetail(pld.Id, session);
                                                foreach (PlanKPIDetail_KPIDTO kpi in parentpl.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPIDTO kd = new PlanKPIDetail_KPIDTO();
                                                    kd.Id = kpi.Id;
                                                    kd.Name = kpi.Name;
                                                    pd.ParentPlanKPIDetail_KPIs.Add(kd);
                                                }
                                                if (parentpl.StaffLeader != null)
                                                {
                                                    pd.AdminLeaderId = parentpl.StaffLeader.Id;
                                                    pd.AdminLeaderName = parentpl.StaffLeader.StaffProfile.Name;
                                                }
                                                //foreach (Staff substaff in pld.SubStaffs)
                                                //{
                                                //    pd.SubStaffNames.Add(substaff.StaffProfile.Name);
                                                //}
                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                        }

                                        //Danh sách planDetail tự tạo không kế thừa cấp trên
                                        List<PlanKPIDetail> CanDeleteDetails = planDetails.Where(p => p.FromCriterion == null).ToList();
                                        foreach (PlanKPIDetail pld in CanDeleteDetails)
                                        {

                                            PlanKPIDetail existPld = planStaff.PlanKPIDetails.SingleOrDefault(pl => ControllerHelpers.GetOriginalParentPlanKPIDetail(pl.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).Id);
                                            //Nếu không tồn tại thì tạo mới plandetail
                                            PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                            if (existPld == null)
                                            {
                                                pd = new PlanKPIMakingDetailDTO();
                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);


                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                childPlan.ParentPlanKPIDetail = pld;
                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                childPlan.PlanStaff = planStaff;
                                                childPlan.Id = Guid.NewGuid();
                                                childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                    child.Id = Guid.NewGuid();
                                                    child.Name = kpi.Name;
                                                    child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                    child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                    childPlan.PlanKPIDetail_KPIs.Add(child);
                                                }
                                                session.Save(childPlan);
                                                newPlanDetails.Add(childPlan);
                                                pd.Id = childPlan.Id;

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                            else
                                            {
                                                pd = new PlanKPIMakingDetailDTO();
                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);

                                                //Subdept hiện tại lấy bộ môn, nhưng hiển thị cột đơn vị phối hợp thực hiện, do đó gắn rỗng cho SubdeptNames
                                                pd.SubDepartmentNames = new List<string>();

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                        }
                                        #endregion

                                        tg.PlanKPIDetails = tg.PlanKPIDetails.OrderBy(p => p.CreateTime).ToList();

                                        tg.TargetGroupId = t.Id;
                                        tg.TargetGroupName = t.Name;


                                        //criterions = session.Query<Criterion>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Department.Id == staff.Department.Id && c.PlanKPI.Id == planId).Map<CriterionPlanDTO>().ToList();
                                        //tg.Criterions = criterions;
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhoPhongBan:
                                    {
                                        #region Phó phòng

                                        switch (t.TargetGroupDetailType.Id)
                                        {
                                            case 0:
                                                {

                                                }
                                                break;
                                            case 1:
                                                {
                                                    foreach (Criterion cri in staffPlanCriterions)
                                                    {
                                                        PlanKPIDetail pld = planDetails.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id);
                                                        PlanKPIDetail pld2 = planDetails2.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id && pl.TargetGroupDetail.Id != t.Id);
                                                        if (pld == null)
                                                        {
                                                            PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(null, cri.FromPlanKPIDetail, 2, session);
                                                            PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(cri.FromPlanKPIDetail);
                                                            childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                            childPlan.ParentPlanKPIDetail = cri.FromPlanKPIDetail;
                                                            childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                            childPlan.PlanStaff = planStaff;
                                                            childPlan.Id = Guid.NewGuid();
                                                            childPlan.MaxRecord = pd.MaxRecord;
                                                            childPlan.StaffLeader = cri.FromPlanKPIDetail.StaffLeader != null ? new Staff() { Id = cri.FromPlanKPIDetail.StaffLeader.Id } : null;
                                                            childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                            pd.BasicResource = string.Empty;
                                                            pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                            foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                            {
                                                                PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                                child.Id = Guid.NewGuid();
                                                                child.Name = kpi.Name;
                                                                child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                                child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                                childPlan.PlanKPIDetail_KPIs.Add(child);

                                                                PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                                parent.Id = kpi.Id;
                                                                parent.Name = kpi.Name;
                                                                pd.ParentPlanKPIDetail_KPIs.Add(parent);
                                                            }
                                                            session.Save(childPlan);
                                                            newPlanDetails.Add(childPlan);
                                                            pd.Id = childPlan.Id;

                                                            if (pd.IsLocked == false)
                                                                result.IsDisable = false;
                                                            tg.PlanKPIDetails.Add(pd);
                                                        }
                                                        else if (!pld.IsDelete)
                                                        {
                                                            PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);



                                                            pd.PreviousKPISecond = pld2 != null ? pld2.PreviousKPI : "";

                                                            try
                                                            {
                                                                if (pd.CurrentKPI != null)
                                                                {
                                                                    Guid currentKPIId = new Guid(pd.CurrentKPI);
                                                                    pd.CurrentKPISecond = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == currentKPIId).Select(c => c.Name).SingleOrDefault();
                                                                }
                                                            }
                                                            catch (Exception e)
                                                            {

                                                            }

                                                            if (pd.StaffLeader == null)
                                                            {
                                                                StaffApiController controller = new StaffApiController();
                                                                pd.StaffLeader = new StaffDTO();
                                                                pd.StaffLeader.Id = controller.GetDepartmentLeader(staff.Department.Id).Id;
                                                            }
                                                            pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                            PlanKPIDetailDTO parentpl = ControllerHelpers.GetParentPlanKPIDetail(pd.Id, session);
                                                            foreach (PlanKPIDetail_KPIDTO kpi in parentpl.PlanKPIDetail_KPIs)
                                                            {
                                                                PlanKPIDetail_KPIDTO kd = new PlanKPIDetail_KPIDTO();
                                                                kd.Id = kpi.Id;
                                                                kd.Name = kpi.Name;
                                                                pd.ParentPlanKPIDetail_KPIs.Add(kd);
                                                            }

                                                            if (pd.IsLocked == false)
                                                                result.IsDisable = false;
                                                            tg.PlanKPIDetails.Add(pd);
                                                        }
                                                    }
                                                    List<PlanKPIDetail> OutDeleteDetails = planDetails.Where(p => p.FromCriterion != null && !staffPlanCriterions.Any(dp => dp.Id == p.FromCriterion.Id)).ToList();
                                                    foreach (PlanKPIDetail pld in OutDeleteDetails)
                                                    {
                                                        PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                        pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);

                                                        if (pd.IsLocked == false)
                                                            result.IsDisable = false;
                                                        tg.PlanKPIDetails.Add(pd);
                                                    }
                                                    //Danh sách planDetail tự tạo không kế thừa cấp trên
                                                    List<PlanKPIDetail> CanDeleteDetails = planDetails.Where(p => p.FromCriterion == null).ToList();
                                                    foreach (PlanKPIDetail pld in CanDeleteDetails)
                                                    {

                                                        PlanKPIDetail existPld = planStaff.PlanKPIDetails.SingleOrDefault(pl => ControllerHelpers.GetOriginalParentPlanKPIDetail(pl.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).Id);
                                                        //Nếu không tồn tại thì tạo mới plandetail
                                                        PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                        if (existPld == null)
                                                        {
                                                            pd = new PlanKPIMakingDetailDTO();
                                                            pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);


                                                            PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                            childPlan.ParentPlanKPIDetail = pld;
                                                            childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                            childPlan.PlanStaff = planStaff;
                                                            childPlan.Id = Guid.NewGuid();
                                                            childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                            foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                            {
                                                                PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                                child.Id = Guid.NewGuid();
                                                                child.Name = kpi.Name;
                                                                child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                                child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                                childPlan.PlanKPIDetail_KPIs.Add(child);
                                                            }
                                                            session.Save(childPlan);
                                                            newPlanDetails.Add(childPlan);
                                                            pd.Id = childPlan.Id;

                                                            if (pd.IsLocked == false)
                                                                result.IsDisable = false;
                                                            tg.PlanKPIDetails.Add(pd);
                                                        }
                                                        else
                                                        {
                                                            pd = new PlanKPIMakingDetailDTO();
                                                            pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);

                                                            if (pd.IsLocked == false)
                                                                result.IsDisable = false;
                                                            tg.PlanKPIDetails.Add(pd);
                                                        }
                                                        if (pd.CurrentKPI != null)
                                                        {
                                                            Guid currentKPIId = new Guid(pd.CurrentKPI);
                                                            pd.CurrentKPISecond = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == currentKPIId).Select(c => c.Name).SingleOrDefault();
                                                        }

                                                    }
                                                }
                                                break;
                                            case 2:
                                                {

                                                }
                                                break;
                                            case 3:
                                                {
                                                    Criterion staffPlanCriterion = session.Query<Criterion>().SingleOrDefault(c => (c.Department.Id == staff.Department.Id)
                                                        && c.TargetGroupDetail.TargetGroupDetailType.Id == 3
                                                        && c.TargetGroupDetail.Id == t.Id
                                                       && !c.FromPlanKPIDetail.IsDisable);

                                                    List<PlanKPIDetail> parentPlanKPIDetailType3 = parentPlanStaff != null ? parentPlanStaff.PlanKPIDetails.Where(pd => pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id).ToList() : new List<PlanKPIDetail>();
                                                    List<PlanKPIDetail> parentPlanKPIDetail2Type3 = parentPlanStaff2 != null ? parentPlanStaff2.PlanKPIDetails.Where(pd => pd.TargetGroupDetail != null && pd.TargetGroupDetail.Id == t.Id).ToList() : new List<PlanKPIDetail>();

                                                    List<PlanKPIDetail> parentMergePlanKPIDetail = parentPlanKPIDetail2Type3.Where(p => !parentPlanKPIDetailType3.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();

                                                    foreach (PlanKPIDetail pldk in parentMergePlanKPIDetail)
                                                    {
                                                        parentPlanKPIDetailType3.Add(pldk);
                                                    }

                                                    planDetails = planStaff.PlanKPIDetails.Where(pd => pd.TargetGroupDetail != null &&
                                                          pd.TargetGroupDetail.Id == t.Id).ToList();

                                                    if (planDetails.Count <= 0)
                                                    {
                                                        List<PlanKPIDetail> planDetailMerges = planDetails.Where(p => !parentMergePlanKPIDetail.Any(p2 => ControllerHelpers.GetOriginalParentPlanKPIDetail(p2.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id)).ToList();
                                                        //foreach (PlanKPIDetail plk in planDetailMerges)
                                                        //    planDetails.Add(plk);
                                                        PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                        if (parentPlanKPIDetailType3.Count > 0)
                                                        {
                                                            foreach (PlanKPIDetail pld in parentPlanKPIDetailType3)
                                                            {

                                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);

                                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                                childPlan.ParentPlanKPIDetail = new PlanKPIDetail() { Id = pld.Id };
                                                                childPlan.PlanStaff = planStaff;

                                                                childPlan.Id = Guid.NewGuid();

                                                                childPlan.CreateTime = DateTime.Now;

                                                                session.Save(childPlan);
                                                                newPlanDetails.Add(childPlan);
                                                                pd.Id = childPlan.Id;
                                                                if (pd.TargetDetail != null)
                                                                {
                                                                    Guid targetDetailId = new Guid(pd.TargetDetail);
                                                                    pd.TargetDetailName = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == targetDetailId).Select(c => c.Name).SingleOrDefault();
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (staffPlanCriterion != null)
                                                            {
                                                                pd = ControllerHelpers.ParsePlanDetail(null, staffPlanCriterion.FromPlanKPIDetail, 2, session);

                                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(staffPlanCriterion.FromPlanKPIDetail);
                                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                                childPlan.ParentPlanKPIDetail = new PlanKPIDetail() { Id = staffPlanCriterion.FromPlanKPIDetail.Id };
                                                                childPlan.PlanStaff = planStaff;

                                                                childPlan.Id = Guid.NewGuid();

                                                                childPlan.CreateTime = DateTime.Now;
                                                                childPlan.StaffLeader = staffPlanCriterion.FromPlanKPIDetail.StaffLeader != null ? new Staff() { Id = staffPlanCriterion.FromPlanKPIDetail.StaffLeader.Id } : null;
                                                                session.Save(childPlan);
                                                                newPlanDetails.Add(childPlan);

                                                                pd.Id = childPlan.Id;
                                                            }
                                                            else
                                                            {
                                                                pd.Id = Guid.Empty;
                                                                pd.CriterionDictionaries = dictionaries;
                                                                pd.StartTime = plan.StartTime;
                                                                pd.EndTime = plan.EndTime;
                                                                pd.CanDelete = false;

                                                                PlanKPIDetail childPlan = new PlanKPIDetail();
                                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                                childPlan.StartTime = DateTime.Now;
                                                                childPlan.EndTime = DateTime.Now;
                                                                childPlan.PlanStaff = planStaff;
                                                                childPlan.Id = Guid.NewGuid();

                                                                childPlan.CreateTime = DateTime.Now;

                                                                session.Save(childPlan);
                                                                newPlanDetails.Add(childPlan);
                                                                pd.Id = childPlan.Id;
                                                            }
                                                            if (pd.TargetDetail != null)
                                                            {
                                                                Guid targetDetailId = new Guid(pd.TargetDetail);
                                                                pd.TargetDetailName = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == targetDetailId).Select(c => c.Name).SingleOrDefault();
                                                            }
                                                        }

                                                        if (pd.IsLocked == false)
                                                            result.IsDisable = false;
                                                        tg.PlanKPIDetails.Add(pd);
                                                    }
                                                    else
                                                    {
                                                        PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                        //PlanKPIDetail pld = planDetails.FirstOrDefault(pl => pl.PlanStaff.Id == planStaff.Id != null && pl.TargetGroupDetail.Id == t.Id);
                                                        pd = ControllerHelpers.ParsePlanDetail(null, planDetails.First(), 2, session);
                                                        pd.CriterionDictionaries = dictionaries;
                                                        pd.CanDelete = false;
                                                        if (pd.TargetDetail != null)
                                                        {
                                                            Guid targetDetailId = new Guid(pd.TargetDetail);
                                                            pd.TargetDetailName = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == targetDetailId).Select(c => c.Name).SingleOrDefault();
                                                        }

                                                        if (pd.IsLocked == false)
                                                            result.IsDisable = false;
                                                        tg.PlanKPIDetails.Add(pd);
                                                    }
                                                }
                                                break;
                                        }
                                        tg.PlanKPIDetails = tg.PlanKPIDetails.OrderBy(p => p.CreateTime).ToList();

                                        tg.TargetGroupId = t.Id;
                                        tg.TargetGroupName = t.Name;



                                        //criterions = session.Query<Criterion>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Department.Id == staff.Department.Id && c.PlanKPI.Id == planId).Map<CriterionPlanDTO>().ToList();
                                        //tg.Criterions = criterions;
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhoKhoa:
                                    {
                                        #region Cho Phó Khoa/trung tâm


                                        #region loại thường
                                        //Lấy từ criterion lúc chưa có kế hoạch
                                        foreach (Criterion cri in departmentPlanCriterions)
                                        {
                                            PlanKPIDetail pld = planDetails.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id);
                                            PlanKPIDetail pld2 = planDetails2.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id && pl.TargetGroupDetail.Id != t.Id);
                                            if (pld == null)
                                            {
                                                PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(cri, null, 1, session);
                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(cri.FromPlanKPIDetail);
                                                childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                childPlan.ParentPlanKPIDetail = cri.FromPlanKPIDetail;
                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                childPlan.PlanStaff = planStaff;
                                                childPlan.Id = Guid.NewGuid();
                                                childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                childPlan.StaffLeader = new Staff { Id = staff.Id };
                                                pd.BasicResource = string.Empty;
                                                pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                    child.Id = Guid.NewGuid();
                                                    child.Name = kpi.Name;
                                                    child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                    child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                    childPlan.PlanKPIDetail_KPIs.Add(child);

                                                    PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                    parent.Id = kpi.Id;
                                                    parent.Name = kpi.Name;
                                                    pd.ParentPlanKPIDetail_KPIs.Add(parent);
                                                }

                                                session.Save(childPlan);
                                                newPlanDetails.Add(childPlan);
                                                pd.Id = childPlan.Id;
                                                if (childPlan.ParentPlanKPIDetail.StaffLeader != null)
                                                {
                                                    pd.AdminLeaderId = childPlan.ParentPlanKPIDetail.StaffLeader.Id;
                                                    pd.AdminLeaderName = childPlan.ParentPlanKPIDetail.StaffLeader.StaffProfile.Name;
                                                }
                                                //mặc định ban đầu chỉ đạo là bản thân
                                                pd.StaffLeader = new StaffDTO();
                                                pd.StaffLeader.Id = staff.Id;
                                                pd.StaffLeader.StaffProfile = new StaffProfile() { Id = staff.StaffProfile.Id, Name = staff.StaffProfile.Name };

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                            else if (!pld.IsDelete)
                                            {
                                                PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                if (pld.PlanStaff.Id != planStaff.Id)
                                                {
                                                    PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                    childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                    childPlan.ParentPlanKPIDetail = pld;
                                                    childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                    childPlan.PlanStaff = planStaff;
                                                    childPlan.Id = Guid.NewGuid();
                                                    childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                    childPlan.StaffLeader = new Staff { Id = staff.Id };

                                                    foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                    {
                                                        PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                        child.Id = Guid.NewGuid();
                                                        child.Name = kpi.Name;
                                                        child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                        child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                        childPlan.PlanKPIDetail_KPIs.Add(child);

                                                        PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                        parent.Id = kpi.Id;
                                                        parent.Name = kpi.Name;

                                                    }
                                                    if (parentPlanKPIDetail.Any(pp => pp.Id == pld.Id))
                                                    {
                                                        foreach (Staff stf in pld.SubStaffs)
                                                        {
                                                            childPlan.SubStaffs.Add(stf);
                                                        }
                                                        //foreach (Staff stf in childPlan.SubStaffs)
                                                        //{
                                                        //    //pd.SubDepartmentIds.Add(subd.Id);
                                                        //    pd.SubStaffNames.Add(stf.StaffProfile.Name);
                                                        //}
                                                    }
                                                    session.Save(childPlan);
                                                    newPlanDetails.Add(childPlan);

                                                    pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                                    pd.Id = childPlan.Id;
                                                }
                                                else
                                                    pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);



                                                pd.PreviousKPISecond = pld2 != null ? pld2.PreviousKPI : "";

                                                try
                                                {
                                                    if (pd.CurrentKPI != null)
                                                    {
                                                        Guid currentKPIId = new Guid(pd.CurrentKPI);
                                                        pd.CurrentKPISecond = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == currentKPIId).Select(c => c.Name).SingleOrDefault();
                                                    }
                                                }
                                                catch (Exception e)
                                                {

                                                }

                                                if (pd.StaffLeader == null)
                                                {
                                                    StaffApiController controller = new StaffApiController();
                                                    pd.StaffLeader = new StaffDTO();
                                                    pd.StaffLeader.Id = controller.GetDepartmentLeader(staff.Department.Id).Id;
                                                }
                                                pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                PlanKPIDetailDTO parentpl = ControllerHelpers.GetParentPlanKPIDetail(pld.Id, session);
                                                foreach (PlanKPIDetail_KPIDTO kpi in parentpl.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPIDTO kd = new PlanKPIDetail_KPIDTO();
                                                    kd.Id = kpi.Id;
                                                    kd.Name = kpi.Name;
                                                    pd.ParentPlanKPIDetail_KPIs.Add(kd);
                                                }
                                                if (parentpl.StaffLeader != null)
                                                {
                                                    pd.AdminLeaderId = parentpl.StaffLeader.Id;
                                                    pd.AdminLeaderName = parentpl.StaffLeader.StaffProfile.Name;
                                                }
                                                //foreach (Staff substaff in pld.SubStaffs)
                                                //{
                                                //    pd.SubStaffNames.Add(substaff.StaffProfile.Name);
                                                //}

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                        }
                                        //Danh sách planDetail tự tạo không kế thừa cấp trên
                                        List<PlanKPIDetail> CanDeleteDetails = planDetails.Where(p => p.FromCriterion == null).ToList();
                                        foreach (PlanKPIDetail pld in CanDeleteDetails)
                                        {

                                            PlanKPIDetail existPld = planStaff.PlanKPIDetails.SingleOrDefault(pl => ControllerHelpers.GetOriginalParentPlanKPIDetail(pl.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).Id);
                                            //Nếu không tồn tại thì tạo mới plandetail
                                            PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                            if (existPld == null)
                                            {
                                                pd = new PlanKPIMakingDetailDTO();
                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);


                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                childPlan.ParentPlanKPIDetail = pld;
                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                childPlan.PlanStaff = planStaff;
                                                childPlan.Id = Guid.NewGuid();
                                                childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                    child.Id = Guid.NewGuid();
                                                    child.Name = kpi.Name;
                                                    child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                    child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                    childPlan.PlanKPIDetail_KPIs.Add(child);
                                                }

                                                session.Save(childPlan);
                                                newPlanDetails.Add(childPlan);
                                                pd.Id = childPlan.Id;

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                            else
                                            {
                                                pd = new PlanKPIMakingDetailDTO();
                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                                //Gắn rỗng cho cột đơn vị phối hợp thực hiện
                                                pd.SubDepartmentNames = new List<string>();

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                        }
                                        #endregion
                                        tg.PlanKPIDetails = tg.PlanKPIDetails.OrderBy(p => p.CreateTime).ToList();

                                        tg.TargetGroupId = t.Id;
                                        tg.TargetGroupName = t.Name;
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhoBoMon:
                                    {
                                        #region Phó BỘ MÔN


                                        #region loại thường
                                        foreach (Criterion cri in departmentPlanCriterions)
                                        {
                                            PlanKPIDetail pld = planDetails.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id);
                                            PlanKPIDetail pld2 = planDetails2.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id && pl.TargetGroupDetail.Id != t.Id);
                                            if (pld == null)
                                            {
                                                PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(cri, null, 1, session);
                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(cri.FromPlanKPIDetail);
                                                childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                childPlan.ParentPlanKPIDetail = cri.FromPlanKPIDetail;
                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                childPlan.PlanStaff = planStaff;
                                                childPlan.Id = Guid.NewGuid();
                                                childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                childPlan.StaffLeader = cri.FromPlanKPIDetail.StaffLeader != null ? new Staff() { Id = cri.FromPlanKPIDetail.StaffLeader.Id } : null;
                                                pd.BasicResource = string.Empty;
                                                pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                    child.Id = Guid.NewGuid();
                                                    child.Name = kpi.Name;
                                                    child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                    child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                    childPlan.PlanKPIDetail_KPIs.Add(child);

                                                    PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                    parent.Id = kpi.Id;
                                                    parent.Name = kpi.Name;
                                                    pd.ParentPlanKPIDetail_KPIs.Add(parent);
                                                }
                                                session.Save(childPlan);
                                                newPlanDetails.Add(childPlan);
                                                pd.Id = childPlan.Id;

                                                //Kế hoạch mới từ criterion mặc định substaff rỗng
                                                pd.SubStaffNames = new List<string>();

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                            else if (!pld.IsDelete)
                                            {
                                                PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                if (pld.PlanStaff.Id != planStaff.Id)
                                                {
                                                    PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                    childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                    childPlan.ParentPlanKPIDetail = pld;
                                                    childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                    childPlan.PlanStaff = planStaff;
                                                    childPlan.Id = Guid.NewGuid();
                                                    childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                    childPlan.StaffLeader = new Staff { Id = staff.Id };

                                                    foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                    {
                                                        PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                        child.Id = Guid.NewGuid();
                                                        child.Name = kpi.Name;
                                                        child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                        child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                        childPlan.PlanKPIDetail_KPIs.Add(child);

                                                        PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                        parent.Id = kpi.Id;
                                                        parent.Name = kpi.Name;

                                                    }
                                                    if (parentPlanKPIDetail.Any(pp => pp.Id == pld.Id))
                                                    {
                                                        foreach (Staff stf in pld.SubStaffs)
                                                        {
                                                            childPlan.SubStaffs.Add(stf);
                                                        }
                                                        //foreach (Staff stf in childPlan.SubStaffs)
                                                        //{
                                                        //    //pd.SubDepartmentIds.Add(subd.Id);
                                                        //    pd.SubStaffNames.Add(stf.StaffProfile.Name);
                                                        //}
                                                    }
                                                    session.Save(childPlan);
                                                    newPlanDetails.Add(childPlan);

                                                    pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                                    pd.Id = childPlan.Id;
                                                }
                                                else
                                                    pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);



                                                pd.PreviousKPISecond = pld2 != null ? pld2.PreviousKPI : "";

                                                try
                                                {
                                                    if (pd.CurrentKPI != null)
                                                    {
                                                        Guid currentKPIId = new Guid(pd.CurrentKPI);
                                                        pd.CurrentKPISecond = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == currentKPIId).Select(c => c.Name).SingleOrDefault();
                                                    }
                                                }
                                                catch (Exception e)
                                                {

                                                }

                                                if (pd.StaffLeader == null)
                                                {
                                                    StaffApiController controller = new StaffApiController();
                                                    pd.StaffLeader = new StaffDTO();
                                                    pd.StaffLeader.Id = controller.GetDepartmentLeader(staff.Department.Id).Id;
                                                }
                                                pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                PlanKPIDetailDTO parentpl = ControllerHelpers.GetParentPlanKPIDetail(pld.Id, session);
                                                foreach (PlanKPIDetail_KPIDTO kpi in parentpl.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPIDTO kd = new PlanKPIDetail_KPIDTO();
                                                    kd.Id = kpi.Id;
                                                    kd.Name = kpi.Name;
                                                    pd.ParentPlanKPIDetail_KPIs.Add(kd);
                                                }
                                                if (parentpl.StaffLeader != null)
                                                {
                                                    pd.AdminLeaderId = parentpl.StaffLeader.Id;
                                                    pd.AdminLeaderName = parentpl.StaffLeader.StaffProfile.Name;
                                                }
                                                //foreach (Staff substaff in pld.SubStaffs)
                                                //{
                                                //    pd.SubStaffNames.Add(substaff.StaffProfile.Name);
                                                //}

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                        }

                                        //Danh sách planDetail tự tạo không kế thừa cấp trên
                                        List<PlanKPIDetail> CanDeleteDetails = planDetails.Where(p => p.FromCriterion == null).ToList();
                                        foreach (PlanKPIDetail pld in CanDeleteDetails)
                                        {

                                            PlanKPIDetail existPld = planStaff.PlanKPIDetails.SingleOrDefault(pl => ControllerHelpers.GetOriginalParentPlanKPIDetail(pl.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).Id);
                                            //Nếu không tồn tại thì tạo mới plandetail
                                            PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                            if (existPld == null)
                                            {
                                                pd = new PlanKPIMakingDetailDTO();
                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);


                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                childPlan.ParentPlanKPIDetail = pld;
                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                childPlan.PlanStaff = planStaff;
                                                childPlan.Id = Guid.NewGuid();
                                                childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                    child.Id = Guid.NewGuid();
                                                    child.Name = kpi.Name;
                                                    child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                    child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                    childPlan.PlanKPIDetail_KPIs.Add(child);
                                                }
                                                session.Save(childPlan);
                                                newPlanDetails.Add(childPlan);
                                                pd.Id = childPlan.Id;

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                            else
                                            {
                                                pd = new PlanKPIMakingDetailDTO();
                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);

                                                //Subdept hiện tại lấy bộ môn, nhưng hiển thị cột đơn vị phối hợp thực hiện, do đó gắn rỗng cho SubdeptNames
                                                pd.SubDepartmentNames = new List<string>();

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                        }
                                        #endregion

                                        tg.PlanKPIDetails = tg.PlanKPIDetails.OrderBy(p => p.CreateTime).ToList();

                                        tg.TargetGroupId = t.Id;
                                        tg.TargetGroupName = t.Name;


                                        //criterions = session.Query<Criterion>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Department.Id == staff.Department.Id && c.PlanKPI.Id == planId).Map<CriterionPlanDTO>().ToList();
                                        //tg.Criterions = criterions;
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.HieuTruong:
                                    {
                                        #region Hiệu trưởng

                                        //List<Criterion> departmentPlanCriterions = session.Query<Criterion>().Where(c => c.Staff.Id == staff.Id && c.TargetGroupDetail.Id == t.Id
                                        //    ).ToList();


                                        #region loại thường
                                        //Lấy từ criterion lúc chưa có kế hoạch
                                        foreach (Criterion cri in departmentPlanCriterions)
                                        {
                                            PlanKPIDetail pld = planDetails.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id);
                                            PlanKPIDetail pld2 = planDetails2.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id && pl.TargetGroupDetail.Id != t.Id);
                                            if (pld == null)
                                            {
                                                PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(cri, null, 1, session);
                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(cri.FromPlanKPIDetail);
                                                childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                childPlan.ParentPlanKPIDetail = cri.FromPlanKPIDetail;
                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                childPlan.PlanStaff = planStaff;
                                                childPlan.Id = Guid.NewGuid();
                                                childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                childPlan.StaffLeader = new Staff { Id = staff.Id };
                                                pd.BasicResource = string.Empty;
                                                pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                    child.Id = Guid.NewGuid();
                                                    child.Name = kpi.Name;
                                                    child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                    child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                    childPlan.PlanKPIDetail_KPIs.Add(child);

                                                    PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                    parent.Id = kpi.Id;
                                                    parent.Name = kpi.Name;
                                                    pd.ParentPlanKPIDetail_KPIs.Add(parent);
                                                }

                                                session.Save(childPlan);
                                                newPlanDetails.Add(childPlan);

                                                pd.Id = childPlan.Id;
                                                if (childPlan.ParentPlanKPIDetail.StaffLeader != null)
                                                {
                                                    pd.AdminLeaderId = childPlan.ParentPlanKPIDetail.StaffLeader.Id;
                                                    pd.AdminLeaderName = childPlan.ParentPlanKPIDetail.StaffLeader.StaffProfile.Name;
                                                }
                                                //mặc định ban đầu chỉ đạo là bản thân
                                                pd.StaffLeader = new StaffDTO();
                                                pd.StaffLeader.Id = staff.Id;
                                                pd.StaffLeader.StaffProfile = new StaffProfile() { Id = staff.StaffProfile.Id, Name = staff.StaffProfile.Name };

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                            else if (!pld.IsDelete)
                                            {
                                                PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                if (pld.PlanStaff.Id != planStaff.Id)
                                                {
                                                    PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                    childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                    childPlan.ParentPlanKPIDetail = pld;
                                                    childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                    childPlan.PlanStaff = planStaff;
                                                    childPlan.Id = Guid.NewGuid();
                                                    childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                    childPlan.StaffLeader = new Staff { Id = staff.Id };

                                                    foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                    {
                                                        PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                        child.Id = Guid.NewGuid();
                                                        child.Name = kpi.Name;
                                                        child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                        child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                        childPlan.PlanKPIDetail_KPIs.Add(child);

                                                        PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                        parent.Id = kpi.Id;
                                                        parent.Name = kpi.Name;

                                                    }
                                                    if (parentPlanKPIDetail.Any(pp => pp.Id == pld.Id))
                                                    {
                                                        foreach (Staff stf in pld.SubStaffs)
                                                        {
                                                            childPlan.SubStaffs.Add(stf);
                                                        }
                                                        //foreach (Staff stf in childPlan.SubStaffs)
                                                        //{
                                                        //    //pd.SubDepartmentIds.Add(subd.Id);
                                                        //    pd.SubStaffNames.Add(stf.StaffProfile.Name);
                                                        //}
                                                    }
                                                    session.Save(childPlan);
                                                    newPlanDetails.Add(childPlan);

                                                    pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                                    pd.Id = childPlan.Id;
                                                }
                                                else
                                                    pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);



                                                pd.PreviousKPISecond = pld2 != null ? pld2.PreviousKPI : "";

                                                try
                                                {
                                                    if (pd.CurrentKPI != null)
                                                    {
                                                        Guid currentKPIId = new Guid(pd.CurrentKPI);
                                                        pd.CurrentKPISecond = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == currentKPIId).Select(c => c.Name).SingleOrDefault();
                                                    }
                                                }
                                                catch (Exception e)
                                                {

                                                }

                                                if (pd.StaffLeader == null)
                                                {
                                                    StaffApiController controller = new StaffApiController();
                                                    pd.StaffLeader = new StaffDTO();
                                                    pd.StaffLeader.Id = controller.GetDepartmentLeader(staff.Department.Id).Id;
                                                }
                                                pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                PlanKPIDetailDTO parentpl = ControllerHelpers.GetParentPlanKPIDetail(pld.Id, session);
                                                foreach (PlanKPIDetail_KPIDTO kpi in parentpl.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPIDTO kd = new PlanKPIDetail_KPIDTO();
                                                    kd.Id = kpi.Id;
                                                    kd.Name = kpi.Name;
                                                    pd.ParentPlanKPIDetail_KPIs.Add(kd);
                                                }
                                                if (parentpl.StaffLeader != null)
                                                {
                                                    pd.AdminLeaderId = parentpl.StaffLeader.Id;
                                                    pd.AdminLeaderName = parentpl.StaffLeader.StaffProfile.Name;
                                                }
                                                //foreach (Staff substaff in pld.SubStaffs)
                                                //{
                                                //    pd.SubStaffNames.Add(substaff.StaffProfile.Name);
                                                //}

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                        }
                                        //Danh sách planDetail tự tạo không kế thừa cấp trên
                                        List<PlanKPIDetail> CanDeleteDetails = planDetails.Where(p => p.FromCriterion == null && !p.IsDelete).ToList();
                                        foreach (PlanKPIDetail pld in CanDeleteDetails)
                                        {

                                            PlanKPIDetail existPld = planStaff.PlanKPIDetails.SingleOrDefault(pl => ControllerHelpers.GetOriginalParentPlanKPIDetail(pl.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).Id);
                                            //Nếu không tồn tại thì tạo mới plandetail
                                            PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                            if (existPld == null)
                                            {
                                                pd = new PlanKPIMakingDetailDTO();
                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);


                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                childPlan.ParentPlanKPIDetail = pld;
                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                childPlan.PlanStaff = planStaff;
                                                childPlan.Id = Guid.NewGuid();
                                                childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                    child.Id = Guid.NewGuid();
                                                    child.Name = kpi.Name;
                                                    child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                    child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                    childPlan.PlanKPIDetail_KPIs.Add(child);
                                                }

                                                session.Save(childPlan);
                                                newPlanDetails.Add(childPlan);
                                                pd.Id = childPlan.Id;

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                            else
                                            {
                                                pd = new PlanKPIMakingDetailDTO();
                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                                //Gắn rỗng cho cột đơn vị phối hợp thực hiện
                                                pd.SubDepartmentNames = new List<string>();

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                        }
                                        #endregion
                                        tg.PlanKPIDetails = tg.PlanKPIDetails.OrderBy(p => p.CreateTime).ToList();

                                        tg.TargetGroupId = t.Id;
                                        tg.TargetGroupName = t.Name;
                                        #endregion
                                    }
                                    break;
                                case (int)AgentObjectTypes.PhoHieuTruong:
                                    {
                                        #region Cho Phó Hiệu trưởng

                                        #region loại thường
                                        //Lấy từ criterion lúc chưa có kế hoạch
                                        foreach (Criterion cri in departmentPlanCriterions)
                                        {
                                            PlanKPIDetail pld = planDetails.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id);
                                            PlanKPIDetail pld2 = planDetails2.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id && pl.TargetGroupDetail.Id != t.Id);
                                            if (pld == null)
                                            {
                                                PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(cri, null, 1, session);
                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(cri.FromPlanKPIDetail);
                                                childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                childPlan.ParentPlanKPIDetail = cri.FromPlanKPIDetail;
                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                childPlan.PlanStaff = planStaff;
                                                childPlan.Id = Guid.NewGuid();
                                                childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                childPlan.StaffLeader = new Staff { Id = staff.Id };
                                                pd.BasicResource = string.Empty;
                                                pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                    child.Id = Guid.NewGuid();
                                                    child.Name = kpi.Name;
                                                    child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                    child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                    childPlan.PlanKPIDetail_KPIs.Add(child);

                                                    PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                    parent.Id = kpi.Id;
                                                    parent.Name = kpi.Name;
                                                    pd.ParentPlanKPIDetail_KPIs.Add(parent);
                                                }

                                                session.Save(childPlan);
                                                newPlanDetails.Add(childPlan);
                                                pd.Id = childPlan.Id;
                                                if (childPlan.ParentPlanKPIDetail.StaffLeader != null)
                                                {
                                                    pd.AdminLeaderId = childPlan.ParentPlanKPIDetail.StaffLeader.Id;
                                                    pd.AdminLeaderName = childPlan.ParentPlanKPIDetail.StaffLeader.StaffProfile.Name;
                                                }
                                                //mặc định ban đầu chỉ đạo là bản thân
                                                pd.StaffLeader = new StaffDTO();
                                                pd.StaffLeader.Id = staff.Id;
                                                pd.StaffLeader.StaffProfile = new StaffProfile() { Id = staff.StaffProfile.Id, Name = staff.StaffProfile.Name };

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                            else if (!pld.IsDelete)
                                            {
                                                PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                                if (pld.PlanStaff.Id != planStaff.Id)
                                                {
                                                    PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                    childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                                    childPlan.ParentPlanKPIDetail = pld;
                                                    childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                    childPlan.PlanStaff = planStaff;
                                                    childPlan.Id = Guid.NewGuid();
                                                    childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                    childPlan.StaffLeader = new Staff { Id = staff.Id };

                                                    foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                    {
                                                        PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                        child.Id = Guid.NewGuid();
                                                        child.Name = kpi.Name;
                                                        child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                        child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                        childPlan.PlanKPIDetail_KPIs.Add(child);

                                                        PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                        parent.Id = kpi.Id;
                                                        parent.Name = kpi.Name;

                                                    }
                                                    if (parentPlanKPIDetail.Any(pp => pp.Id == pld.Id))
                                                    {
                                                        foreach (Staff stf in pld.SubStaffs)
                                                        {
                                                            childPlan.SubStaffs.Add(stf);
                                                        }
                                                        //foreach (Staff stf in childPlan.SubStaffs)
                                                        //{
                                                        //    //pd.SubDepartmentIds.Add(subd.Id);
                                                        //    pd.SubStaffNames.Add(stf.StaffProfile.Name);
                                                        //}
                                                    }
                                                    session.Save(childPlan);
                                                    newPlanDetails.Add(childPlan);

                                                    pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                                    pd.Id = childPlan.Id;
                                                }
                                                else
                                                    pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);



                                                pd.PreviousKPISecond = pld2 != null ? pld2.PreviousKPI : "";

                                                try
                                                {
                                                    if (pd.CurrentKPI != null)
                                                    {
                                                        Guid currentKPIId = new Guid(pd.CurrentKPI);
                                                        pd.CurrentKPISecond = session.Query<CriterionDictionary>().Where(c => c.TargetGroupDetail.Id == t.Id && c.Id == currentKPIId).Select(c => c.Name).SingleOrDefault();
                                                    }
                                                }
                                                catch (Exception e)
                                                {

                                                }

                                                if (pd.StaffLeader == null)
                                                {
                                                    StaffApiController controller = new StaffApiController();
                                                    pd.StaffLeader = new StaffDTO();
                                                    pd.StaffLeader.Id = controller.GetDepartmentLeader(staff.Department.Id).Id;
                                                }
                                                pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                PlanKPIDetailDTO parentpl = ControllerHelpers.GetParentPlanKPIDetail(pld.Id, session);
                                                foreach (PlanKPIDetail_KPIDTO kpi in parentpl.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPIDTO kd = new PlanKPIDetail_KPIDTO();
                                                    kd.Id = kpi.Id;
                                                    kd.Name = kpi.Name;
                                                    pd.ParentPlanKPIDetail_KPIs.Add(kd);
                                                }
                                                if (parentpl.StaffLeader != null)
                                                {
                                                    pd.AdminLeaderId = parentpl.StaffLeader.Id;
                                                    pd.AdminLeaderName = parentpl.StaffLeader.StaffProfile.Name;
                                                }
                                                //foreach (Staff substaff in pld.SubStaffs)
                                                //{
                                                //    pd.SubStaffNames.Add(substaff.StaffProfile.Name);
                                                //}

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);


                                                //PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                                //pd.PreviousKPISecond = pld2 != null ? pld2.PreviousKPI : "";
                                                //pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                                //PlanKPIDetailDTO parentpl = ControllerHelpers.GetParentPlanKPIDetail(pd.Id);
                                                //foreach (PlanKPIDetail_KPIDTO kpi in parentpl.PlanKPIDetail_KPIs)
                                                //{
                                                //    PlanKPIDetail_KPIDTO kd = new PlanKPIDetail_KPIDTO();
                                                //    kd.Id = kpi.Id;
                                                //    kd.Name = kpi.Name;
                                                //    pd.ParentPlanKPIDetail_KPIs.Add(kd);
                                                //}
                                                //if (pld.ParentPlanKPIDetail.StaffLeader != null)
                                                //{
                                                //    pd.AdminLeaderId = pld.ParentPlanKPIDetail.StaffLeader.Id;
                                                //    pd.AdminLeaderName = pld.ParentPlanKPIDetail.StaffLeader.StaffProfile.Name;
                                                //}
                                                ////Gắn rỗng cho SubDeptIds
                                                ////pd.SubDepartmentIds = new List<Guid>();




                                                //tg.PlanKPIDetails.Add(pd);
                                            }
                                        }
                                        //Danh sách planDetail tự tạo không kế thừa cấp trên
                                        List<PlanKPIDetail> CanDeleteDetails = planDetails.Where(p => p.FromCriterion == null && !p.IsDelete).ToList();
                                        foreach (PlanKPIDetail pld in CanDeleteDetails)
                                        {

                                            PlanKPIDetail existPld = planStaff.PlanKPIDetails.SingleOrDefault(pl => ControllerHelpers.GetOriginalParentPlanKPIDetail(pl.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).Id);
                                            //Nếu không tồn tại thì tạo mới plandetail
                                            PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
                                            if (existPld == null)
                                            {
                                                pd = new PlanKPIMakingDetailDTO();
                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);


                                                PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(pld);
                                                childPlan.ParentPlanKPIDetail = pld;
                                                childPlan.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                                                childPlan.PlanStaff = planStaff;
                                                childPlan.Id = Guid.NewGuid();
                                                childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                                foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                                {
                                                    PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                    child.Id = Guid.NewGuid();
                                                    child.Name = kpi.Name;
                                                    child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                    child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                    childPlan.PlanKPIDetail_KPIs.Add(child);
                                                }

                                                session.Save(childPlan);
                                                newPlanDetails.Add(childPlan);
                                                pd.Id = childPlan.Id;

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                            else
                                            {
                                                pd = new PlanKPIMakingDetailDTO();
                                                pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                                //Gắn rỗng cho cột đơn vị phối hợp thực hiện
                                                pd.SubDepartmentNames = new List<string>();

                                                if (pd.IsLocked == false)
                                                    result.IsDisable = false;
                                                tg.PlanKPIDetails.Add(pd);
                                            }
                                        }
                                        #endregion
                                        tg.PlanKPIDetails = tg.PlanKPIDetails.OrderBy(p => p.CreateTime).ToList();

                                        tg.TargetGroupId = t.Id;
                                        tg.TargetGroupName = t.Name;
                                        #endregion
                                    }
                                    break;
                            }

                            result.PlanId = planId;
                            result.AgentObjectId = agentObjectId;
                            result.AgentObjectName = agentObject.Name;
                            result.TargetGroupPlanMakings.Add(tg);
                            result.Vision = planStaff.Vision != null ? planStaff.Vision : null;
                            result.Mission = planStaff.Mission != null ? planStaff.Mission : null;


                        }
                        #endregion
                        #region Additional Plan Kế hoạch phân thêm cho giảng viên

                        //int agentObjectTypeId = -1;
                        //if (staff.StaffInfo.Position == null)
                        //{
                        //    if (staff.StaffInfo.StaffType.ManageCode == "3")
                        //        agentObjectTypeId = 2; //Nhân viên
                        //    else
                        //        agentObjectTypeId = 1; //Giảng viên
                        //}
                        //else
                        //{
                        //    //if (staff.StaffInfo.Position.AgentObjectType != null)
                        //    //{
                        //    //    result.StaffDTO.AgentObjectTypeId = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.AgentObjectType.Id : (staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1);

                        //    //    if (staff.StaffInfo.Subject != null)
                        //    //        DepartmentId = staff.StaffInfo.Subject.Id;
                        //    //    else
                        //    //        DepartmentId = staff.Department.Id;
                        //    //}
                        //    //else
                        //    //{
                        //    //    result.StaffDTO.AgentObjectTypeId = agentObject.AgentObjectType.Id;
                        //    //    SubPosition sposition = staff.StaffInfo.SubPositions.Where(s => s.Position.AgentObjectType.Id == agentObject.AgentObjectType.Id).Select(s => s).FirstOrDefault();
                        //    //    if (sposition != null)
                        //    //        DepartmentId = sposition.Department.Id;
                        //    //}
                        //    SubPosition subposition = staff.StaffInfo.SubPositions.Where(s => s.Position.AgentObjectType.Id == agentObject.AgentObjectType.Id).Select(s => s).FirstOrDefault();
                        //    if (subposition != null)
                        //    {
                        //        DepartmentId = subposition.Department.Id;
                        //        agentObjectTypeId = subposition.Position.AgentObjectType.Id;
                        //    }
                        //    else
                        //    {
                        //        agentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
                        //    }

                        //}
                        switch (AgentObjectTypeId)
                        {
                            case 1:
                                {
                                    List<PlanKPIDetail> additionalPlanDetails = new List<PlanKPIDetail>();
                                    result.AdditionalPlanDetails = new List<PlanKPIMakingDetailDTO>();
                                    additionalPlanDetails = planStaff.PlanKPIDetails.Where(pd => pd.TargetGroupDetail == null).ToList();
                                    List<Criterion> templist = session.Query<Criterion>().Where(c => c.Staff.Id == staff.Id).ToList();

                                    List<Criterion> professorCriterion = templist.Where(c => !c.FromPlanKPIDetail.IsDisable && ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Count > 0
                                        && ((plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                      (plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) >= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) <= 0) ||
                                                      (plan.StartTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Min(m => m.StartTime)) <= 0 && plan.EndTime.CompareTo(ControllerHelpers.GetOriginalMethods(c.FromPlanKPIDetail.Id, session).Max(m => m.EndTime)) >= 0))).ToList();
                                    foreach (Criterion cri in professorCriterion)
                                    {
                                        PlanKPIDetail pld = additionalPlanDetails.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id);
                                        //PlanKPIDetail pld2 = planDetails2.SingleOrDefault(pl => pl.FromCriterion != null && pl.FromCriterion.Id == cri.Id && pl.TargetGroupDetail.Id != t.Id);
                                        if (pld == null)
                                        {
                                            PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(null, cri.FromPlanKPIDetail, 2, session);
                                            PlanKPIDetail childPlan = ControllerHelpers.CopyPlanKPIDetail(cri.FromPlanKPIDetail);
                                            childPlan.FromCriterion = new Criterion() { Id = cri.Id };
                                            childPlan.ParentPlanKPIDetail = cri.FromPlanKPIDetail;
                                            childPlan.PlanStaff = planStaff;
                                            childPlan.Id = Guid.NewGuid();
                                            childPlan.MaxRecord = pd.MaxRecord;
                                            childPlan.BasicResource = childPlan.ParentPlanKPIDetail.BasicResource;
                                            childPlan.TargetGroupDetail = null;
                                            childPlan.StaffLeader = cri.FromPlanKPIDetail.StaffLeader != null ? new Staff() { Id = cri.FromPlanKPIDetail.StaffLeader.Id } : null;
                                            childPlan.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                                            pd.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                                            foreach (PlanKPIDetail_KPI kpi in childPlan.ParentPlanKPIDetail.PlanKPIDetail_KPIs)
                                            {
                                                PlanKPIDetail_KPI child = new PlanKPIDetail_KPI();
                                                child.Id = Guid.NewGuid();
                                                child.Name = kpi.Name;
                                                child.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnit { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                                                child.PlanKPIDetail = new PlanKPIDetail { Id = childPlan.Id };
                                                childPlan.PlanKPIDetail_KPIs.Add(child);

                                                PlanKPIDetail_KPIDTO parent = new PlanKPIDetail_KPIDTO();
                                                parent.Id = kpi.Id;
                                                parent.Name = kpi.Name;
                                                pd.ParentPlanKPIDetail_KPIs.Add(parent);
                                            }
                                            session.Save(childPlan);

                                            pd.Id = childPlan.Id;
                                            //Staff subjectLeader = session.Query<Staff>().Where(s => s.StaffInfo.Subject.Id == staff.StaffInfo.Subject.Id && s.StaffInfo.Position.AgentObjectType.Id == 6).SingleOrDefault();
                                            Staff subjectLeader = ControllerHelpers.GetSubjectLeaderFromCriterion(cri);
                                            pd.StaffLeader = new StaffDTO();
                                            pd.StaffLeader.Id = subjectLeader.Id;
                                            pd.StaffLeader.Name = subjectLeader.StaffProfile!=null ? subjectLeader.StaffProfile.Name:"";
                                            result.AdditionalPlanDetails.Add(pd);
                                        }
                                        else
                                        {
                                            PlanKPIMakingDetailDTO pd = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                                            //Staff subjectLeader = session.Query<Staff>().Where(s => s.StaffInfo.Subject.Id == staff.StaffInfo.Subject.Id && s.StaffInfo.Position.AgentObjectType.Id == 6).SingleOrDefault();
                                            Staff subjectLeader = ControllerHelpers.GetSubjectLeaderFromCriterion(cri);
                                            pd.StaffLeader = new StaffDTO();
                                            pd.StaffLeader.Id = subjectLeader.Id;
                                            pd.StaffLeader.Name = subjectLeader.StaffProfile != null ? subjectLeader.StaffProfile.Name:"";
                                            result.AdditionalPlanDetails.Add(pd);
                                        }
                                    }
                                }
                                break;
                        }
                        #endregion   //  Xử lý dữ liệu

                        #endregion
                    }

                    //Cập nhật cache dữ liệu
                    foreach (PlanKPIDetail pld in newPlanDetails)
                    {
                        ControllerHelpers.UpdatePlanDetailDic(pld, 1, session);
                    }

                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;

                    logMessage += "(Time: " + elapsedMs+")";

                    NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
                    logger.Fatal(logMessage);

                }
                else
                {

                }
            });

            SessionManager.DoWork(session =>
            {
                foreach (TargetGroupPlanMakingDTO tg in result.TargetGroupPlanMakings)
                {
                    try
                    {
                        tg.PlanKPIDetails = tg.PlanKPIDetails.OrderBy(pl => ControllerHelpers.GetOriginalMethods(pl.Id, session).Min(pld => pld.StartTime)).ThenBy(pl => ControllerHelpers.GetOriginalMethods(pl.Id, session).Min(pld => pld.EndTime)).ThenBy(pl => pl.TargetDetail).ToList();
                        //tg.PlanKPIDetails = tg.PlanKPIDetails.OrderBy(pl => pl.TargetDetail).ToList();
                    }
                    catch (Exception e)
                    {

                    }
                }
            });
            return result;
        }

        public PlanKPIMakingDetailDTO GetObjDTO(Guid id, int agentObjectTypeId)
        {
            PlanKPIMakingDetailDTO result = new PlanKPIMakingDetailDTO();

            PlanKPIDetail pld = new PlanKPIDetail();
            SessionManager.DoWork(session =>
            {
                pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == id);
                if (agentObjectTypeId != 1)
                {
                    result = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                }
                result.AgentObjectTypeId = agentObjectTypeId;
                switch (agentObjectTypeId)
                {
                    case (int)AgentObjectTypes.GiangVien:
                        {
                            result.Id = pld.Id;
                            result.Name = pld.Name;
                            result.BasicResource = pld.BasicResource;
                            result.ExecuteMethod = pld.ExecuteMethod;
                            result.StartDateString = pld.StartTime.ToLocalTime().ToString("dd/MM/yyyy");
                            result.EndTimeString = pld.EndTime.ToLocalTime().ToString("dd/MM/yyyy");
                            result.CurrentKPI = pld.CurrentKPI;
                            result.PreviousKPI = pld.PreviousKPI;
                            result.TargetDetail = pld.TargetDetail;
                            result.TargetDetailName = pld.FromProfessorCriterion != null ? pld.FromProfessorCriterion.Name : "";
                            result.FromProfessorCriterion = new ProfessorCriterionDTO();
                            result.FromProfessorCriterion.Id = pld.FromProfessorCriterion.Id;
                            result.FromProfessorCriterion.Record = pld.FromProfessorCriterion.Record;
                            result.StartTime = pld.StartTime;
                            result.EndTime = pld.EndTime;
                            result.TargetDetail = pld.TargetDetail;
                            result.ProfessorOtherActivities = new List<ProfessorOtherActivityDTO>();
                            foreach (ProfessorOtherActivity pa in pld.ProfessorOtherActivities)
                            {
                                ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                                pad.Id = pa.Id;
                                pad.Name = pa.CriterionDictionary.Name;
                                pad.NumberOfTime = pa.NumberOfTime;
                                pad.NumberOfHour = pa.CriterionDictionary.NumberOfHour;
                                result.ProfessorOtherActivities.Add(pad);
                            }
                            result.ScienceResearches = new List<ScienceResearchDTO>();
                            foreach (ScienceResearch pa in pld.ScienceResearches)
                            {
                                ScienceResearchDTO pad = new ScienceResearchDTO();
                                pad.Id = pa.Id;
                                pad.Name = pa.CriterionDictionary.Name;
                                pad.NumberOfResearch = pa.NumberOfResearch;
                                result.ScienceResearches.Add(pad);
                            }
                            result.StartDateString = pld.StartTime.ToString("dd/MM/yyyy");
                            result.EndTimeString = pld.EndTime.ToString("dd/MM/yyyy");

                        }
                        break;
                    case (int)AgentObjectTypes.NhanVien:
                        {
                            //if (result.StaffLeader == null)
                            //{
                            //    StaffApiController controller = new StaffApiController();
                            //    result.StaffLeader = new StaffDTO();
                            //    //result.StaffLeader.Id = controller.GetDepartmentLeader(staff.Department.Id).Id;
                            //}
                            result.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                            PlanKPIDetailDTO parentpl = ControllerHelpers.GetParentPlanKPIDetail(result.Id);
                            if (parentpl.PlanKPIDetail_KPIs != null)
                            {
                                foreach (PlanKPIDetail_KPIDTO kpi in parentpl.PlanKPIDetail_KPIs)
                                {
                                    PlanKPIDetail_KPIDTO kd = new PlanKPIDetail_KPIDTO();
                                    kd.Id = kpi.Id;
                                    kd.Name = kpi.Name;
                                    result.ParentPlanKPIDetail_KPIs.Add(kd);
                                }
                            }
                            try
                            {
                                if (result.CurrentKPI != null)
                                {
                                    Guid currentKPIId = new Guid(result.CurrentKPI);
                                    result.CurrentKPISecond = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId).Select(c => c.Name).SingleOrDefault();
                                }
                                if (result.TargetDetail != null)
                                {
                                    Guid targetDetailId = new Guid(result.TargetDetail);
                                    result.TargetDetailName = session.Query<CriterionDictionary>().Where(c => c.Id == targetDetailId).Select(c => c.Name).SingleOrDefault();
                                }
                            }
                            catch (Exception e)
                            {

                            }
                        }
                        break;
                    case (int)AgentObjectTypes.PhongBan:
                        {
                            //if (result.StaffLeader == null)
                            //{
                            //    StaffApiController controller = new StaffApiController();
                            //    result.StaffLeader = new StaffDTO();
                            //    result.StaffLeader.Id = controller.GetDepartmentLeader(staff.Department.Id).Id;
                            //}
                            result.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                            PlanKPIDetailDTO parentpl = ControllerHelpers.GetParentPlanKPIDetail(result.Id);
                            if (parentpl.PlanKPIDetail_KPIs != null)
                            {
                                foreach (PlanKPIDetail_KPIDTO kpi in parentpl.PlanKPIDetail_KPIs)
                                {
                                    PlanKPIDetail_KPIDTO kd = new PlanKPIDetail_KPIDTO();
                                    kd.Id = kpi.Id;
                                    kd.Name = kpi.Name;
                                    result.ParentPlanKPIDetail_KPIs.Add(kd);
                                }
                            }
                            //if (pld.ParentPlanKPIDetail != null && pld.ParentPlanKPIDetail.StaffLeader != null)
                            //{
                            //    result.AdminLeaderId = pld.ParentPlanKPIDetail.StaffLeader.Id;
                            //    result.AdminLeaderName = pld.ParentPlanKPIDetail.StaffLeader.StaffProfile.Name;
                            //}
                            //Lấy BGH chỉ đạo
                            Guid adminPlanDetailId = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session) != null ? ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).Id : Guid.Empty;
                            PlanKPIDetail adminPlanDetail = new PlanKPIDetail();
                            adminPlanDetail = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == adminPlanDetailId);
                            //Kiểm tra nếu là plandetail của BGH mới gắn BGH chỉ đạo
                            if (adminPlanDetail != null && adminPlanDetail.PlanStaff.WebGroupId != Guid.Empty)
                            {
                                result.AdminLeaderId = adminPlanDetail.StaffLeader != null ? adminPlanDetail.StaffLeader.Id : Guid.Empty;
                                result.AdminLeaderName = adminPlanDetail.StaffLeader != null ? adminPlanDetail.StaffLeader.StaffProfile.Name : "";
                            }
                            try
                            {
                                if (result.CurrentKPI != null)
                                {
                                    Guid currentKPIId = new Guid(result.CurrentKPI);
                                    result.CurrentKPISecond = session.Query<CriterionDictionary>().Where(c => c.Id == currentKPIId).Select(c => c.Name).SingleOrDefault();
                                }
                                if (result.TargetDetail != null)
                                {
                                    Guid targetDetailId = new Guid(result.TargetDetail);
                                    result.TargetDetailName = session.Query<CriterionDictionary>().Where(c => c.Id == targetDetailId).Select(c => c.Name).SingleOrDefault();
                                }
                            }
                            catch (Exception e)
                            {

                            }
                        }
                        break;
                    case (int)AgentObjectTypes.BanGiamHieu:
                        {
                            foreach (Department subd in pld.SubDepartments)
                            {
                                //pd.SubDepartmentIds.Add(subd.Id);
                                result.SubDepartmentNames.Add(subd.Name);
                            }
                        }
                        break;
                    case (int)AgentObjectTypes.Khoa:
                        {
                            PlanKPIDetailDTO parentpl = ControllerHelpers.GetParentPlanKPIDetail(result.Id);
                            if (parentpl.PlanKPIDetail_KPIs != null)
                            {
                                foreach (PlanKPIDetail_KPIDTO kpi in parentpl.PlanKPIDetail_KPIs)
                                {
                                    PlanKPIDetail_KPIDTO kd = new PlanKPIDetail_KPIDTO();
                                    kd.Id = kpi.Id;
                                    kd.Name = kpi.Name;
                                    result.ParentPlanKPIDetail_KPIs.Add(kd);
                                }
                            }
                            //Lấy BGH chỉ đạo
                            Guid adminPlanDetailId = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session) != null ? ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).Id : Guid.Empty;
                            PlanKPIDetail adminPlanDetail = new PlanKPIDetail();
                            adminPlanDetail = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == adminPlanDetailId);
                            //Kiểm tra nếu là plandetail của BGH mới gắn BGH chỉ đạo
                            if (adminPlanDetail != null && adminPlanDetail.PlanStaff.WebGroupId != Guid.Empty)
                            {
                                result.AdminLeaderId = adminPlanDetail.StaffLeader != null ? adminPlanDetail.StaffLeader.Id : Guid.Empty;
                                result.AdminLeaderName = adminPlanDetail.StaffLeader != null ? adminPlanDetail.StaffLeader.StaffProfile.Name : "";
                            }
                        }
                        break;
                    case (int)AgentObjectTypes.BoMon:
                        {
                            if (result.StaffLeader == null)
                            {
                                StaffApiController controller = new StaffApiController();
                                result.StaffLeader = new StaffDTO();
                            }
                            result.ParentPlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                            PlanKPIDetailDTO parentpl = ControllerHelpers.GetParentPlanKPIDetail(result.Id);
                            if (parentpl.PlanKPIDetail_KPIs != null)
                            {
                                foreach (PlanKPIDetail_KPIDTO kpi in parentpl.PlanKPIDetail_KPIs)
                                {
                                    PlanKPIDetail_KPIDTO kd = new PlanKPIDetail_KPIDTO();
                                    kd.Id = kpi.Id;
                                    kd.Name = kpi.Name;
                                    result.ParentPlanKPIDetail_KPIs.Add(kd);
                                }
                            }
                        }
                        break;
                }
            });
            return result;
        }

        public PlanKPIMakingDetailDTO GetObj(Guid id, int agentObjectTypeId)
        {
            PlanKPIMakingDetailDTO result = new PlanKPIMakingDetailDTO();
            SessionManager.DoWork(session =>
            {
                PlanKPIDetail pld = session.Query<PlanKPIDetail>().SingleOrDefault(a => a.Id == id);
                result = ControllerHelpers.ParsePlanDetail(null, pld, 2, session);
                result.AgentObjectTypeId = agentObjectTypeId;
                //result = session.Query<PlanKPIDetail>().SingleOrDefault(a => a.Id == id).Map<PlanKPIDetailDTO>();
            });
            return result;
        }

        public PlanDetailMakingDTO Put1(PlanDetailMakingDTO obj)
        {
            //Xóa các kế hoạch chi tiết không tồn tại
            //SessionManager.DoWork(session =>
            //{
            //    PlanStaff planStaff = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Id == obj.PlanStaffId);
            //    if (planStaff != null)
            //    {
            //        Staff staff = session.Query<Staff>().SingleOrDefault(s => s.Id == obj.StaffDTO.Id);
            //        List<Guid> agentObjectsId = staff.StaffInfo.AgentObjects.Select(s => s.Id).ToList();
            //        foreach (TargetGroupPlanMakingDTO tg in obj.TargetGroupPlanMakings)
            //        {
            //            if (tg.TargetGroupDetailTypeId != 2)
            //            {
            //                List<PlanKPIDetail> originalPlanKPIDetails = planStaff.PlanKPIDetails.Where(p => p.TargetGroupDetail.Id == tg.TargetGroupId).ToList();
            //                List<PlanKPIDetail> deletingPlanKPIDetails = originalPlanKPIDetails.Where(p => !tg.PlanKPIDetails.Any(p1 => p1.Id == p.Id)).ToList();
            //                foreach (PlanKPIDetail pd in deletingPlanKPIDetails)
            //                {
            //                    bool canDelete = !session.Query<ResultDetail>().Any(rd => rd.PlanKPIDetail.Id == pd.Id);
            //                    if (canDelete)
            //                    {
            //                        session.Delete(pd);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //});


            SessionManager.DoWork(session =>
            {
                Staff staff = session.Query<Staff>().SingleOrDefault(s => s.Id == obj.StaffDTO.Id);
                PlanStaff planStaff = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Id == obj.PlanStaffId);
                if (planStaff == null)
                {
                    planStaff = new PlanStaff();
                    planStaff.Id = Guid.NewGuid();
                    planStaff.PlanKPI = new PlanKPI() { Id = obj.PlanId };
                    planStaff.IsLocked = false;
                    planStaff.ModifiedTime = DateTime.Now;
                    planStaff.Staff = staff;
                    planStaff.Vision = obj.Vision;
                    planStaff.Mission = obj.Mission;
                    session.Save(planStaff);
                }
                else
                {
                    planStaff.ModifiedTime = DateTime.Now;
                    planStaff.Vision = obj.Vision;
                    planStaff.Mission = obj.Mission;
                    session.Update(planStaff);
                }

                List<Guid> agentObjectsId = staff.StaffInfo.AgentObjects.Select(s => s.Id).ToList();
                int AgentObjectTypeId = -1;
                if (obj.AgentObjectId == Guid.Empty)
                    AgentObjectTypeId = obj.StaffDTO.AgentObjectTypeId;
                else
                {
                    AgentObject agentObject = session.Query<AgentObject>().SingleOrDefault(a => a.Id == obj.AgentObjectId);
                    AgentObjectTypeId = agentObject.AgentObjectType.Id;
                }

                foreach (TargetGroupPlanMakingDTO tg in obj.TargetGroupPlanMakings)
                {
                    switch (AgentObjectTypeId)
                    {
                        case 1:
                            {
                                #region Giangvien
                                if (tg.TargetGroupDetailTypeId == 0)
                                {
                                    foreach (PlanKPIMakingDetailDTO p in tg.PlanKPIDetails)
                                    {
                                        PlanKPIDetail updatePlanKPIDetail = new PlanKPIDetail();
                                        updatePlanKPIDetail = session.Query<PlanKPIDetail>().Where(pld => pld.Id == p.Id).SingleOrDefault();
                                        updatePlanKPIDetail.TargetDetail = p.TargetDetail;
                                        updatePlanKPIDetail.Name = p.Name;
                                        updatePlanKPIDetail.BasicResource = p.BasicResource;
                                        updatePlanKPIDetail.ExecuteMethod = p.ExecuteMethod;
                                        updatePlanKPIDetail.StartTime = p.StartTime.ToLocalTime();
                                        updatePlanKPIDetail.EndTime = p.EndTime.ToLocalTime();
                                        updatePlanKPIDetail.PreviousKPI = p.PreviousKPI;
                                        updatePlanKPIDetail.CurrentKPI = p.CurrentKPI;
                                        updatePlanKPIDetail.PlanStaff = planStaff;
                                        session.Merge(updatePlanKPIDetail);
                                    }
                                }

                                #endregion
                            }
                            break;
                        //Dành cho nhân viên
                        case 2:
                            {

                            }
                            break;
                        case 3:
                            {

                            }
                            break;
                        case 4:
                            {

                            }
                            break;
                        case 5:
                            {

                            }
                            break;
                        case 6:
                            {

                            }
                            break;
                    }



                }
            });
            return obj;
        }

        //public PlanStaff PutLockPlan(Guid Obj)
        //{
        //    PlanStaff planStaff = null;
        //    //SessionManager.DoWork(session =>
        //    //{
        //    //    planStaff = session.Query<PlanStaff>().Where(ps => ps.Id == planStaffId).FirstOrDefault();
        //    //    planStaff.IsLocked = true;
        //    //    session.Update(planStaff);
        //    //});
        //    return planStaff;
        //}
        public bool PutLockPlanStaff(PlanDetailMakingDTO obj)
        {
            PlanStaff ps = new PlanStaff();
            SessionManager.DoWork(session =>
            {
                ps = session.Query<PlanStaff>().SingleOrDefault(pt => pt.Id == obj.PlanStaffId);
                ps.IsLocked = !ps.IsLocked;
                session.Update(ps);
            });
            return ps.IsLocked;
        }
        public int PutVision(PlanDetailMakingDTO obj)
        {
            int result = 0;
            try
            {
                SessionManager.DoWork(session =>
                {
                    PlanStaff ps = session.Query<PlanStaff>().SingleOrDefault(pt => pt.Id == obj.PlanStaffId);
                    ps.Vision = obj.Vision;
                    session.Update(ps);
                    result = 1;
                });
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public int PutMission(PlanDetailMakingDTO obj)
        {
            int result = 0;
            try
            {
                SessionManager.DoWork(session =>
                {
                    PlanStaff ps = session.Query<PlanStaff>().SingleOrDefault(pt => pt.Id == obj.PlanStaffId);
                    ps.Mission = obj.Mission;
                    session.Update(ps);
                    result = 1;
                });
            }
            catch (Exception e)
            {
            }
            return result;
        }

        public IEnumerable<MeasureUnit> GetMeasureUnits()
        {
            List<MeasureUnit> result = new List<MeasureUnit>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<MeasureUnit>().ToList();
            });
            return result;
        }

        public PlanDetailMakingDTO PutLock(PlanDetailMakingDTO obj)
        {
            SessionManager.DoWork(session =>
            {
                Staff staff = session.Query<Staff>().SingleOrDefault(s => s.Id == obj.StaffDTO.Id);
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                Guid deptId = new Guid(applicationUser.DepartmentId != null ? applicationUser.DepartmentId : Guid.Empty.ToString());
                PlanStaff planStaff = session.Query<PlanStaff>().SingleOrDefault(ps => ps.Id == obj.PlanStaffId);
                if (planStaff == null)
                {
                    planStaff = new PlanStaff();
                    planStaff.Id = Guid.NewGuid();
                    planStaff.PlanKPI = new PlanKPI() { Id = obj.PlanId };
                    planStaff.IsLocked = false;
                    planStaff.ModifiedTime = DateTime.Now;
                    planStaff.Staff = staff;
                    session.Save(planStaff);
                }
                else
                {
                    planStaff.ModifiedTime = DateTime.Now;
                    session.Update(planStaff);
                }
                TargetGroupPlanMakingDTO copyTg = null;
                foreach (TargetGroupPlanMakingDTO tg in obj.TargetGroupPlanMakings)
                {
                    int AgentObjectTypeId = -1;
                    AgentObjectTypeId = obj.StaffDTO.AgentObjectTypeId;
                    switch (AgentObjectTypeId)
                    {
                        case 1:
                            {

                            }
                            break;
                        case 2:
                            {

                            }
                            break;
                        //Trưởng phòng
                        case 3:
                            {
                                #region Trưởng phòng
                                switch (tg.TargetGroupDetailTypeId)
                                {
                                    case 1:
                                        {
                                            copyTg = tg;
                                            foreach (PlanKPIMakingDetailDTO p in tg.PlanKPIDetails)
                                            {
                                                PlanKPIDetail pdn = new PlanKPIDetail();
                                                //PlanKPIDetail updatePlanKPIDetail = session.Query<PlanKPIDetail>().SingleOrDefault(pld => pld.Id == p.Id);
                                                //pdn = ControllerHelpers.ParsePlanKPIDetail(p, updatePlanKPIDetail, planStaff, tg, false);

                                                //Thêm vào các criterion mới
                                                PlanKPIDetail plDetail = session.Query<PlanKPIDetail>().Single(pd => pd.Id == p.Id);
                                                if (!plDetail.IsDelete)
                                                {
                                                    //pdn.Criterions = plDetail.Criterions;
                                                    List<Criterion> leadCriterions = plDetail.Criterions.Where(c => c.Staff != null && c.Staff.Id == plDetail.StaffLeader.Id).ToList();
                                                    if (leadCriterions.Count <= 0)
                                                    {
                                                        Criterion criterion = new Criterion();
                                                        List<Criterion> criterions = session.Query<Criterion>().Where(c => c.StaffLeader.Id == p.StaffLeader.Id).ToList();
                                                        Criterion existCri = criterions.SingleOrDefault(c => c.FromPlanKPIDetail != null && ControllerHelpers.GetOriginalParentPlanKPIDetail(c.FromPlanKPIDetail.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id);
                                                        if (existCri == null)
                                                        {
                                                            criterion = ControllerHelpers.ParseCriterion(tg, obj, pdn, p, p.LeadDepartment.Id, 2, true);
                                                            criterion.Department = null;
                                                            session.Save(criterion);
                                                        }
                                                        else
                                                        {
                                                            existCri.Name = p.TargetDetail;
                                                            existCri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                                            existCri.Department = null;
                                                            existCri.StaffLeader = p.StaffLeader != null ? new Staff() { Id = p.StaffLeader.Id } : null;
                                                            existCri.MaxRecord = p.MaxRecord;
                                                            session.Merge(existCri);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        foreach (Criterion cri in leadCriterions)
                                                        {
                                                            cri.Name = p.TargetDetail;
                                                            cri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                                            //cri.Department = p.LeadDepartment != null ? new Department() { Id = p.LeadDepartment.Id } : null;
                                                            cri.MaxRecord = p.MaxRecord;
                                                            session.Merge(cri);
                                                        }
                                                    }

                                                    foreach (Guid sid in p.SubStaffIds)
                                                    {
                                                        if (sid != p.StaffLeader.Id)
                                                        {
                                                            List<Criterion> criterions = session.Query<Criterion>().Where(c => c.Staff.Id == sid).ToList();
                                                            Criterion existCri = criterions.SingleOrDefault(c => (c.FromPlanKPIDetail != null && ControllerHelpers.GetOriginalParentPlanKPIDetail(c.FromPlanKPIDetail.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id));
                                                            if (existCri == null)
                                                            {
                                                                Criterion criterion = new Criterion();
                                                                criterion = ControllerHelpers.ParseCriterion(tg, obj, pdn, p, Guid.Empty, 2, true);
                                                                criterion.StaffLeader = null;
                                                                criterion.Staff = new Staff() { Id = sid };
                                                                criterion.Department = null;
                                                                session.Save(criterion);
                                                                //pdn.Criterions.Add(criterion);
                                                            }
                                                            else
                                                            {
                                                                existCri.Name = p.TargetDetail;
                                                                existCri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                                                existCri.Staff = new Staff() { Id = sid };
                                                                existCri.MaxRecord = p.MaxRecord;
                                                                session.Merge(existCri);
                                                            }

                                                        }

                                                    }
                                                }
                                                    
                                                plDetail.IsLocked = true;
                                                session.Update(plDetail);
                                            }
                                        }
                                        break;
                                    case 2:
                                        {


                                        }
                                        break;
                                    case 3:
                                        {

                                            foreach (PlanKPIMakingDetailDTO p in tg.PlanKPIDetails)
                                            {
                                                List<Criterion> criterions = new List<Criterion>();
                                                if (staff != null)
                                                    criterions = session.Query<Criterion>().Where(c => c.Department.Id == staff.Department.Id && c.TargetGroupDetail.Id == tg.TargetGroupId).ToList();
                                                else
                                                    //User KPI ko có Staff                                                 
                                                    criterions = session.Query<Criterion>().Where(c => c.Department.Id == deptId && c.TargetGroupDetail.Id == tg.TargetGroupId).ToList();


                                                if (criterions.Count > 0)
                                                {
                                                    Criterion existCri = criterions.FirstOrDefault();
                                                    existCri.Name = p.TargetDetail;
                                                    existCri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                                    session.Merge(existCri);
                                                }
                                                else
                                                {
                                                    PlanKPIDetail pdn = new PlanKPIDetail();
                                                    pdn = ControllerHelpers.ParsePlanKPIDetail(p, null, planStaff, tg, true);
                                                    Criterion criterion = new Criterion();
                                                    if (staff != null)
                                                        criterion = ControllerHelpers.ParseNewCriterion(p.TargetDetail, staff.Department, null, pdn, new PlanKPI() { Id = obj.PlanId }, new TargetGroupDetail() { Id = tg.TargetGroupId }, 1, 0);
                                                    else
                                                        //User KPI ko có Staff       
                                                        criterion = ControllerHelpers.ParseNewCriterion(p.TargetDetail, new Department { Id = deptId }, null, pdn, new PlanKPI() { Id = obj.PlanId }, new TargetGroupDetail() { Id = tg.TargetGroupId }, 1, 0);
                                                    criterion.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                                    session.Save(criterion);
                                                }
                                                PlanKPIDetail plDetail = session.Query<PlanKPIDetail>().Single(pd => pd.Id == p.Id);
                                                plDetail.IsLocked = true;
                                                session.Update(plDetail);
                                            }
                                        }
                                        break;
                                }
                                #endregion
                            }
                            break;
                        case 4:
                            {
                                #region Ban Giám hiệu
                                List<Criterion> totalCriterions = session.Query<Criterion>().ToList();
                                foreach (PlanKPIMakingDetailDTO p in tg.PlanKPIDetails)
                                {
                                    PlanKPIDetail pdn = new PlanKPIDetail();


                                    PlanKPIDetail plDetail = session.Query<PlanKPIDetail>().Single(pd => pd.Id == p.Id);
                                    pdn.Criterions = plDetail.Criterions;

                                    //Khóa người chỉ đạo
                                    List<Criterion> leadStaffCriterions = pdn.Criterions.Where(c => c.Staff != null && c.Staff.Id == plDetail.StaffLeader.Id).ToList();
                                    if (leadStaffCriterions.Count <= 0)
                                    {
                                        Criterion criterion = new Criterion();
                                        List<Criterion> criterions = session.Query<Criterion>().Where(c => c.Staff.Id == p.StaffLeader.Id).ToList();
                                        Criterion existCri = criterions.SingleOrDefault(c => c.FromPlanKPIDetail != null && ControllerHelpers.GetOriginalParentPlanKPIDetail(c.FromPlanKPIDetail.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id);
                                        if (existCri == null)
                                        {
                                            criterion = ControllerHelpers.ParseCriterion(tg, obj, pdn, p, p.StaffLeader.Id, 1, false);
                                            session.Save(criterion);
                                            //pdn.Criterions.Add(criterion);
                                        }
                                        else
                                        {
                                            existCri.Name = p.TargetDetail;
                                            existCri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                            existCri.Staff = p.StaffId != null ? new Staff() { Id = p.StaffLeader.Id } : null;
                                            existCri.MaxRecord = p.MaxRecord;
                                            session.Merge(existCri);
                                        }
                                    }
                                    else
                                    {
                                        foreach (Criterion cri in leadStaffCriterions)
                                        {
                                            cri.Name = p.TargetDetail;
                                            cri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                            cri.Department = p.LeadDepartment != null ? new Department() { Id = p.LeadDepartment.Id } : null;
                                            cri.MaxRecord = p.MaxRecord;
                                            session.Merge(cri);
                                        }
                                    }


                                    //Khóa đơn vị chỉ đạo
                                    List<Criterion> leadCriterions = pdn.Criterions.Where(c => c.Department != null && c.Department.Id == plDetail.LeadDepartment.Id).ToList();
                                    if (leadCriterions.Count <= 0)
                                    {
                                        Criterion criterion = new Criterion();
                                        List<Criterion> criterions = session.Query<Criterion>().Where(c => c.Department.Id == p.LeadDepartment.Id).ToList();
                                        Criterion existCri = criterions.SingleOrDefault(c => c.FromPlanKPIDetail != null && ControllerHelpers.GetOriginalParentPlanKPIDetail(c.FromPlanKPIDetail.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id);
                                        if (existCri == null)
                                        {
                                            criterion = ControllerHelpers.ParseCriterion(tg, obj, pdn, p, p.LeadDepartment.Id, 2, false);
                                            session.Save(criterion);
                                            //pdn.Criterions.Add(criterion);
                                        }
                                        else
                                        {
                                            existCri.Name = p.TargetDetail;
                                            existCri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                            existCri.Department = p.LeadDepartment != null ? new Department() { Id = p.LeadDepartment.Id } : null;
                                            existCri.MaxRecord = p.MaxRecord;
                                            session.Merge(existCri);
                                        }
                                    }
                                    else
                                    {
                                        foreach (Criterion cri in leadCriterions)
                                        {
                                            cri.Name = p.TargetDetail;
                                            cri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                            cri.Department = p.LeadDepartment != null ? new Department() { Id = p.LeadDepartment.Id } : null;
                                            cri.MaxRecord = p.MaxRecord;
                                            session.Merge(cri);
                                        }
                                    }


                                    //Khóa đơn vị phối hợp
                                    foreach (Guid did in p.SubDepartmentIds)
                                    {
                                        if (did != p.LeadDepartment.Id)
                                        {
                                            List<Criterion> subCriterions = pdn.Criterions.Where(c => c.Department != null && c.Department.Id == did).ToList();
                                            if (subCriterions.Count <= 0)
                                            {

                                                Criterion existCri = totalCriterions.SingleOrDefault(c => c.Department != null && c.Department.Id == did && (c.FromPlanKPIDetail != null && ControllerHelpers.GetOriginalParentPlanKPIDetail(c.FromPlanKPIDetail.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id));
                                                if (existCri == null)
                                                {
                                                    Criterion criterion = new Criterion();
                                                    criterion = ControllerHelpers.ParseCriterion(tg, obj, pdn, p, did, 2, false);
                                                    session.Save(criterion);
                                                    totalCriterions.Add(criterion);
                                                }
                                                else
                                                {
                                                    existCri.Name = p.TargetDetail;
                                                    existCri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                                    existCri.Department = p.LeadDepartment != null ? new Department() { Id = did } : null;
                                                    existCri.MaxRecord = p.MaxRecord;
                                                    session.Merge(existCri);
                                                }
                                            }
                                            else
                                            {
                                                foreach (Criterion cri in subCriterions)
                                                {
                                                    cri.Name = p.TargetDetail;
                                                    cri.MaxRecord = p.MaxRecord;
                                                    session.Update(cri);
                                                }
                                            }
                                        }
                                    }
                                    plDetail.IsLocked = true;
                                    session.Update(plDetail);
                                }
                                #endregion
                            }
                            break;
                        case 5:
                            {
                                #region Khoa Viện Trung Tâm
                                foreach (PlanKPIMakingDetailDTO p in tg.PlanKPIDetails)
                                {
                                    PlanKPIDetail pdn = new PlanKPIDetail();

                                    PlanKPIDetail plDetail = session.Query<PlanKPIDetail>().Single(pd => pd.Id == p.Id);
                                    pdn.Criterions = plDetail.Criterions;


                                    foreach (Guid did in p.SubjectIds)
                                    {
                                        if (did != p.LeadDepartment.Id)
                                        {
                                            List<Criterion> subCriterions = pdn.Criterions.Where(c => c.Department != null && c.Department.Id == did).ToList();
                                            if (subCriterions.Count <= 0)
                                            {
                                                List<Criterion> criterions = session.Query<Criterion>().Where(c => c.Department.Id == did).ToList();
                                                Criterion existCri = criterions.SingleOrDefault(c => (c.FromPlanKPIDetail != null && ControllerHelpers.GetOriginalParentPlanKPIDetail(c.FromPlanKPIDetail.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id));
                                                if (existCri == null)
                                                {
                                                    Criterion criterion = new Criterion();
                                                    criterion = ControllerHelpers.ParseCriterion(tg, obj, pdn, p, did, 2, false);
                                                    session.Save(criterion);
                                                    //pdn.Criterions.Add(criterion);
                                                }
                                                else
                                                {
                                                    existCri.Name = p.TargetDetail;
                                                    existCri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                                    existCri.Department = p.LeadDepartment != null ? new Department() { Id = did } : null;
                                                    existCri.MaxRecord = p.MaxRecord;
                                                    session.Merge(existCri);
                                                }
                                            }
                                            else
                                            {
                                                foreach (Criterion cri in subCriterions)
                                                {
                                                    cri.Name = p.TargetDetail;
                                                    cri.MaxRecord = p.MaxRecord;
                                                    session.Update(cri);
                                                }
                                            }
                                        }
                                    }

                                    foreach (Guid sid in p.SubStaffIds)
                                    {

                                        List<Criterion> subCriterions = pdn.Criterions.Where(c => c.Staff != null && c.Staff.Id == sid).ToList();
                                        if (subCriterions.Count <= 0)
                                        {
                                            List<Criterion> criterions = session.Query<Criterion>().Where(c => c.Staff.Id == sid).ToList();
                                            Criterion existCri = criterions.SingleOrDefault(c => (c.FromPlanKPIDetail != null && ControllerHelpers.GetOriginalParentPlanKPIDetail(c.FromPlanKPIDetail.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id));
                                            if (existCri == null)
                                            {
                                                Criterion criterion = new Criterion();
                                                criterion = ControllerHelpers.ParseCriterion(tg, obj, pdn, p, sid, 5, false);
                                                session.Save(criterion);
                                                //pdn.Criterions.Add(criterion);
                                            }
                                            else
                                            {
                                                existCri.Name = p.TargetDetail;
                                                existCri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                                existCri.MaxRecord = p.MaxRecord;
                                                session.Merge(existCri);
                                            }
                                        }
                                        else
                                        {
                                            foreach (Criterion cri in subCriterions)
                                            {
                                                cri.Name = p.TargetDetail;
                                                cri.MaxRecord = p.MaxRecord;
                                                session.Update(cri);
                                            }
                                        }

                                    }
                                    //Khóa người chỉ đạo
                                    List<Criterion> leadStaffCriterions = pdn.Criterions.Where(c => c.Staff != null && c.Staff.Id == plDetail.StaffLeader.Id).ToList();
                                    if (leadStaffCriterions.Count <= 0)
                                    {
                                        Criterion criterion = new Criterion();
                                        List<Criterion> criterions = session.Query<Criterion>().Where(c => c.Staff.Id == p.StaffLeader.Id).ToList();
                                        Criterion existCri = criterions.SingleOrDefault(c => c.FromPlanKPIDetail != null && ControllerHelpers.GetOriginalParentPlanKPIDetail(c.FromPlanKPIDetail.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id);
                                        if (existCri == null)
                                        {
                                            criterion = ControllerHelpers.ParseCriterion(tg, obj, pdn, p, p.StaffLeader.Id, 1, false);
                                            session.Save(criterion);
                                            //pdn.Criterions.Add(criterion);
                                        }
                                        else
                                        {
                                            existCri.Name = p.TargetDetail;
                                            existCri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                            existCri.Staff = p.StaffId != null ? new Staff() { Id = p.StaffLeader.Id } : null;
                                            existCri.MaxRecord = p.MaxRecord;
                                            session.Merge(existCri);
                                        }
                                    }
                                    else
                                    {
                                        foreach (Criterion cri in leadStaffCriterions)
                                        {
                                            cri.Name = p.TargetDetail;
                                            cri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                            cri.Department = p.LeadDepartment != null ? new Department() { Id = p.LeadDepartment.Id } : null;
                                            cri.MaxRecord = p.MaxRecord;
                                            session.Merge(cri);
                                        }
                                    }
                                    plDetail.IsLocked = true;
                                    session.Update(plDetail);
                                }
                                #endregion
                            }
                            break;
                        case 6:
                            {
                                #region Khoa bộ môn
                                foreach (PlanKPIMakingDetailDTO p in tg.PlanKPIDetails)
                                {
                                    PlanKPIDetail pdn = new PlanKPIDetail();

                                    PlanKPIDetail plDetail = session.Query<PlanKPIDetail>().Single(pd => pd.Id == p.Id);
                                    pdn.Criterions = plDetail.Criterions;


                                    foreach (Guid did in p.SubDepartmentIds)
                                    {
                                        if (did != p.LeadDepartment.Id)
                                        {
                                            List<Criterion> subCriterions = pdn.Criterions.Where(c => c.Department != null && c.Department.Id == did).ToList();
                                            if (subCriterions.Count <= 0)
                                            {
                                                List<Criterion> criterions = session.Query<Criterion>().Where(c => c.Department.Id == did).ToList();
                                                Criterion existCri = criterions.SingleOrDefault(c => (c.FromPlanKPIDetail != null && ControllerHelpers.GetOriginalParentPlanKPIDetail(c.FromPlanKPIDetail.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id));
                                                if (existCri == null)
                                                {
                                                    Criterion criterion = new Criterion();
                                                    criterion = ControllerHelpers.ParseCriterion(tg, obj, pdn, p, did, 2, false);
                                                    session.Save(criterion);
                                                    //pdn.Criterions.Add(criterion);
                                                }
                                                else
                                                {
                                                    existCri.Name = p.TargetDetail;
                                                    existCri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                                    existCri.Department = p.LeadDepartment != null ? new Department() { Id = did } : null;
                                                    existCri.MaxRecord = p.MaxRecord;
                                                    session.Merge(existCri);
                                                }
                                            }
                                            else
                                            {
                                                foreach (Criterion cri in subCriterions)
                                                {
                                                    cri.Name = p.TargetDetail;
                                                    cri.MaxRecord = p.MaxRecord;
                                                    session.Update(cri);
                                                }
                                            }
                                        }
                                    }

                                    foreach (Guid sid in p.SubStaffIds)
                                    {

                                        List<Criterion> subCriterions = pdn.Criterions.Where(c => c.Staff != null && c.Staff.Id == sid).ToList();
                                        if (subCriterions.Count <= 0)
                                        {
                                            List<Criterion> criterions = session.Query<Criterion>().Where(c => c.Staff.Id == sid).ToList();
                                            Criterion existCri = criterions.SingleOrDefault(c => (c.FromPlanKPIDetail != null && ControllerHelpers.GetOriginalParentPlanKPIDetail(c.FromPlanKPIDetail.Id, session).Id == ControllerHelpers.GetOriginalParentPlanKPIDetail(p.Id, session).Id));
                                            if (existCri == null)
                                            {
                                                Criterion criterion = new Criterion();
                                                criterion = ControllerHelpers.ParseCriterion(tg, obj, pdn, p, sid, 5, false);
                                                session.Save(criterion);
                                                //pdn.Criterions.Add(criterion);
                                            }
                                            else
                                            {
                                                existCri.Name = p.TargetDetail;
                                                existCri.FromPlanKPIDetail = new PlanKPIDetail() { Id = p.Id };
                                                existCri.MaxRecord = p.MaxRecord;
                                                session.Merge(existCri);
                                            }
                                        }
                                        else
                                        {
                                            foreach (Criterion cri in subCriterions)
                                            {
                                                cri.Name = p.TargetDetail;
                                                cri.MaxRecord = p.MaxRecord;
                                                session.Update(cri);
                                            }
                                        }
                                    }
                                    plDetail.IsLocked = true;
                                    session.Update(plDetail);
                                }
                                #endregion
                            }
                            break;
                    }
                }
            });
            return obj;
        }
        //public int Delete(PlanKPIDetail obj)
        //{
        //    int result = 0;
        //    try
        //    {
        //        SessionManager.DoWork(session =>
        //        {
        //            if (obj != null)
        //            {
        //                bool check = false;
        //                check = session.Query<Criterion>().Any(c => c.FromPlanKPIDetail.Id == obj.Id);
        //                if (check == false)
        //                {
        //                    session.Delete(obj);
        //                    result = 1;
        //                }
        //            }
        //        });
        //        SessionManager.DoWork(session =>
        //        {
        //            PlanKPIDetail pkpiDetail = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == obj.Id);
        //            if (pkpiDetail == null)
        //                ControllerHelpers.UpdatePlanDetailDic(obj, 2, session);
        //        });
        //    }
        //    catch(Exception e)
        //    {
        //        result = 0;
        //    }
        //    return result;
        //}
        public int Delete(PlanKPIDetail obj)
        {
            int result = 0;
            //try
            //{
            SessionManager.DoWork(session =>
            {
                PlanKPIDetail deletePlan = session.Query<PlanKPIDetail>().SingleOrDefault(pl => pl.Id == obj.Id);
                if (deletePlan != null)
                {
                    List<ResultDetail> rs = session.Query<ResultDetail>().Where(r => r.PlanKPIDetail.Id == obj.Id).ToList();
                    if (rs != null)
                    {
                        //foreach(ResultDetail rt in rs)
                        //{                                
                        //    session.Delete(rt);
                        //}


                    }
                    deletePlan.IsDelete = true;
                    session.Merge(deletePlan);
                    result = 1;
                }
            });
            SessionManager.DoWork(session =>
            {
                PlanKPIDetail pkpiDetail = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == obj.Id);
                if (pkpiDetail == null)
                    ControllerHelpers.UpdatePlanDetailDic(obj, 2, session);
            });
            //}
            //catch (Exception e)
            //{
            //    result = 0;
            //}
            return result;
        }

        public int DeleteDisable(PlanKPIDetail obj)
        {
            int result = 0;
            //try
            //{
            SessionManager.DoWork(session =>
            {
                PlanKPIDetail disablePlan = session.Query<PlanKPIDetail>().SingleOrDefault(pl => pl.Id == obj.Id);
                if (disablePlan != null)
                {
                    List<PlanKPIDetail> childPlanDetail = ControllerHelpers.GetAllChildPlanKPIDetail(obj.Id, session);
                    disablePlan.IsDisable = !disablePlan.IsDisable;
                    session.Merge(disablePlan);

                    foreach(PlanKPIDetail pld in childPlanDetail)
                    {
                        pld.IsDisable = disablePlan.IsDisable;
                        session.Update(pld);
                    }
                                      
                    result = 1;
                }
            });
            SessionManager.DoWork(session =>
            {
                PlanKPIDetail pkpiDetail = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == obj.Id);
                if (pkpiDetail == null)
                    ControllerHelpers.UpdatePlanDetailDic(obj, 1, session);
            });
            //}
            //catch (Exception e)
            //{
            //    result = 0;
            //}
            return result;
        }

        public void PutRefreshUpdatePlanDetailCache()
        {
            SessionManager.DoWork(session =>
            {
                ControllerHelpers.UpdatePlanDetailDic(session);
            });

        }

        public void PutUpdatePlanDetailCache(PlanKPIDetail planDetail)
        {
            SessionManager.DoWork(session =>
            {
                ControllerHelpers.UpdatePlanDetailDic(planDetail, 1, session);
            });

        }
    }
}
