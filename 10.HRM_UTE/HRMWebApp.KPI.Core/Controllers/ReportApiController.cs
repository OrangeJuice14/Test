using System;
using System.Collections.Generic;
using System.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;
using HRMWebApp.KPI.Core.Helpers;
using System.Web;
using HRMWebApp.KPI.Core.DTO.AdoDataClass;
using HRMWebApp.KPI.Core.DTO.PlanMakingDTOs;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ReportApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<Report_DepartmentStaffResultADO> GetReportDepartmentStaffResult(Guid planId, Guid departmentId)
        {
            return DataClassHelper.GetReportDepartmentStaffResult(planId, departmentId);
        }

        [Authorize]
        [Route("")]
        public IEnumerable<Report_TotalDepartmentResultADO> GetReportTotalDepartmentResult(Guid planId)
        {
            return DataClassHelper.GetReportTotalDepartmentResult(planId);
        }

        [Authorize]
        [Route("")]
        public IEnumerable<UserPlanKPIDTO> GetListPlanKPIDetailByDepartment(Guid planId, Guid departmentId)
        {
            List<UserPlanKPIDTO> result = new List<UserPlanKPIDTO>();
            SessionManager.DoWork(session =>
            {
                PlanKPI plan = session.Query<PlanKPI>().Where(q => q.Id == planId).SingleOrDefault();
                int planType = plan.PlanType.Id;
                var stafflist = session.Query<Staff>().Where(s => s.Department != null && s.Department.Id == departmentId && s.StaffStatus != null && s.StaffInfo != null && s.StaffProfile != null && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null);
                stafflist = stafflist.OrderByDescending(s => s.StaffInfo.Position.HSPCChucVu).ThenBy(s => s.StaffInfo.Position.ManageCode);
                IEnumerable<AgentObject> AgentObjectList = session.Query<AgentObject>();
                IEnumerable<AgentObject_Salary> AgentObject_SalaryList = session.Query<AgentObject_Salary>();
                
                //if (leader.StaffInfo.Position.AgentObjectType.Id == 3) //phòng ban
                //{
                //    foreach (Staff st in stafflist)
                //    {
                //        UserPlanKPIDTO upd = new UserPlanKPIDTO();
                //        upd.Id = planId;
                //        upd.AgentObjectId = Guid.Empty;
                //        AgentObject newObManage = AgentObjectList.FirstOrDefault(a => a.AgentObjectType.Id == 2);
                //        upd.AgentObjectId = newObManage != null ? newObManage.Id : Guid.Empty;
                //        upd.StaffId = st.Id;
                //        upd.StaffName = st.StaffProfile != null ? st.StaffProfile.Name : string.Empty;
                //        upd.PositionName = (st.StaffInfo != null && st.StaffInfo.Position != null) ? st.StaffInfo.Position.Name : string.Empty;
                //        upd.DepartmentId = departmentId;
                //        if (upd.PositionName != string.Empty)
                //        {
                //            if (st.StaffInfo.Position.AgentObjectType != null)
                //            {
                //                //upd.AgentObjectId = st.StaffInfo.Position.AgentObjectType.AgentObjects.First().Id;
                                
                //                AgentObject newObManage1 = AgentObjectList.FirstOrDefault(a => a.AgentObjectType.Id == st.StaffInfo.Position.AgentObjectType.Id && st.StaffInfo.Position.AgentObjectType.Id != 9);
                //                if (newObManage1 != null)
                //                    upd.AgentObjectId = newObManage1.Id;
                //            }
                //        }
                //        result.Add(upd);
                //    }
                //}
                //else //khoa
                //{
                //    foreach (Staff st in stafflist)
                //    {
                //        UserPlanKPIDTO upd = GetAgentObjectProfessor(plan, planType, st, AgentObjectList, AgentObject_SalaryList);
                //        if (upd != null)
                //            result.Add(upd);

                //        UserPlanKPIDTO upd2 = GetChucVuChinh(plan, AgentObjectList, st, planType);
                //        if (upd2 != null)
                //            result.Add(upd2);
                //    }
                //}

                foreach (Staff st in stafflist)
                {
                    IEnumerable<UserPlanKPIDTO> list = GetListUserPlanKPIDTO(plan, planType, st, AgentObjectList, AgentObject_SalaryList);
                    result.AddRange(list);
                }
            });
            return result;
        }

        public static UserPlanKPIDTO GetChucVuChinh(PlanKPI plan, IEnumerable<AgentObject> AgentObjectList, Staff staff, int planType)
        {
            //Chức vụ chính
            AgentObject newObManage = new AgentObject();
            UserPlanKPIDTO planDTOManage = null;
            if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null)
            {
                newObManage = AgentObjectList.FirstOrDefault(a => a.AgentObjectType.Id == staff.StaffInfo.Position.AgentObjectType.Id && staff.StaffInfo.Position.AgentObjectType.Id != 9);
                if (newObManage != null)
                {
                    planDTOManage = PlanKPIApiController.ParseUserPlan(newObManage, staff, plan, staff.StaffInfo.Position.AgentObjectType);
                }
            }
            else
            {
                int agentObjectTypeId = staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1;
                if (agentObjectTypeId == 2)
                {
                    newObManage = AgentObjectList.FirstOrDefault(a => a.AgentObjectType.Id == agentObjectTypeId && a.AgentObjectType.PlanTypes.Any(pt => pt.Id == planType));
                    if (newObManage != null)
                    {
                        planDTOManage = PlanKPIApiController.ParseUserPlan(newObManage, staff, plan, null);
                    }
                }

            }

            if (planDTOManage != null)
            {
                planDTOManage.DepartmentId = staff.Department.Id; //thêm department để phân biệt chức vụ kiêm nhiệm khác khoa
                //nếu là trưởng bộ môn thì gán departmentId là id của bộ môn
                if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null && (staff.StaffInfo.Position.AgentObjectType.Id == 6 || staff.StaffInfo.Position.AgentObjectType.Id == 12))
                    planDTOManage.DepartmentId = staff.StaffInfo.Subject != null ? staff.StaffInfo.Subject.Id : staff.Department.Id;
            }
            return planDTOManage;
        }

        public static UserPlanKPIDTO GetAgentObjectProfessor(PlanKPI plan, int planType, Staff staff, IEnumerable<AgentObject> AgentObjectList, IEnumerable<AgentObject_Salary> AgentObject_SalaryList)
        {
            UserPlanKPIDTO result = null;
            //so sánh mã ngạch
            List<AgentObject_Salary> agents_SalaryList = AgentObject_SalaryList.Where(sa => sa.ScaleSalary != null && staff.StaffSalaryInfo != null && staff.StaffSalaryInfo.ScaleSalary != null && sa.ScaleSalary.Id == staff.StaffSalaryInfo.ScaleSalary.Id).ToList();

            if (agents_SalaryList.Count > 1)
            {
                if (staff.StaffLevel != null && staff.StaffLevel.Qualification != null) //so sánh trình độ chuyên môn
                    agents_SalaryList = agents_SalaryList.Where(sa => sa.Qualification != null && sa.Qualification.Id == staff.StaffLevel.Qualification.Id).ToList();
            }
            if (agents_SalaryList.Count > 1)
            {
                if (staff.StaffLevel != null && staff.StaffLevel.AcademicTitle != null) //so sánh học hàm
                    agents_SalaryList = agents_SalaryList.Where(sa => sa.AcademicTitle != null && sa.AcademicTitle.Id == staff.StaffLevel.AcademicTitle.Id).ToList();
            }

            if (agents_SalaryList.Count > 0)
            {
                AgentObject agentObject = AgentObjectList.Where(sa => sa.Id == agents_SalaryList.First().AgentObject.Id).SingleOrDefault();
                if (agentObject != null && agentObject.AgentObjectType != null && agentObject.AgentObjectType.PlanTypes.Any(pt => pt.Id == planType))
                {
                    result = plan.Map<UserPlanKPIDTO>();
                    if (result != null)
                    {
                        result = PlanKPIApiController.ParseUserPlan(agentObject, staff, plan, null);
                    }
                }
            }
            return result;
        }

        public static List<UserPlanKPIDTO> GetListUserPlanKPIDTO(PlanKPI plan, int planType, Staff staff, IEnumerable<AgentObject> AgentObjectList, IEnumerable<AgentObject_Salary> AgentObject_SalaryList)
        {
            List<UserPlanKPIDTO> result = new List<UserPlanKPIDTO>();
            #region Giảng viên
            //so sánh mã ngạch
            List<AgentObject_Salary> agents_SalaryList = AgentObject_SalaryList.Where(sa => sa.ScaleSalary != null && staff.StaffSalaryInfo != null && staff.StaffSalaryInfo.ScaleSalary != null && sa.ScaleSalary.Id == staff.StaffSalaryInfo.ScaleSalary.Id).ToList();

            if (agents_SalaryList.Count > 1)
            {
                if (staff.StaffLevel != null && staff.StaffLevel.Qualification != null) //so sánh trình độ chuyên môn
                    agents_SalaryList = agents_SalaryList.Where(sa => sa.Qualification != null && sa.Qualification.Id == staff.StaffLevel.Qualification.Id).ToList();
            }
            if (agents_SalaryList.Count > 1)
            {
                if (staff.StaffLevel != null && staff.StaffLevel.AcademicTitle != null) //so sánh học hàm
                    agents_SalaryList = agents_SalaryList.Where(sa => sa.AcademicTitle != null && sa.AcademicTitle.Id == staff.StaffLevel.AcademicTitle.Id).ToList();
            }

            if (agents_SalaryList.Count > 0)
            {
                AgentObject agentObject = AgentObjectList.Where(sa => sa.Id == agents_SalaryList.First().AgentObject.Id).SingleOrDefault();
                if (agentObject != null && agentObject.AgentObjectType != null && agentObject.AgentObjectType.PlanTypes.Any(pt => pt.Id == planType))
                {
                    UserPlanKPIDTO planDTO = plan.Map<UserPlanKPIDTO>();
                    if (planDTO != null)
                    {
                        planDTO = PlanKPIApiController.ParseUserPlan(agentObject, staff, plan, null);
                        planDTO.DepartmentId = staff.Department.Id;
                        result.Add(planDTO);
                    }
                }
            }
            #endregion

            #region Chức vụ chính
            AgentObject newObManage = new AgentObject();
            UserPlanKPIDTO planDTOManage = null;
            if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null)
            {
                newObManage = AgentObjectList.FirstOrDefault(a => a.AgentObjectType.Id == staff.StaffInfo.Position.AgentObjectType.Id && staff.StaffInfo.Position.AgentObjectType.Id != 9);
                if (newObManage != null)
                {
                    planDTOManage = PlanKPIApiController.ParseUserPlan(newObManage, staff, plan, null);
                }
            }
            else
            {
                int agentObjectTypeId = staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1;
                if (agentObjectTypeId == 2)
                {
                    newObManage = AgentObjectList.FirstOrDefault(a => a.AgentObjectType.Id == agentObjectTypeId && a.AgentObjectType.PlanTypes.Any(pt => pt.Id == planType));
                    if (newObManage != null)
                    {
                        planDTOManage = PlanKPIApiController.ParseUserPlan(newObManage, staff, plan, null);
                    }
                }

            }

            if (planDTOManage != null)
            {
                planDTOManage.DepartmentId = staff.Department.Id; //thêm department để phân biệt chức vụ kiêm nhiệm khác khoa
                //nếu là trưởng bộ môn thì gán departmentId là id của bộ môn
                if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null && (staff.StaffInfo.Position.AgentObjectType.Id == 6 || staff.StaffInfo.Position.AgentObjectType.Id == 12))
                    planDTOManage.DepartmentId = staff.StaffInfo.Subject != null ? staff.StaffInfo.Subject.Id : staff.Department.Id;

                planDTOManage.PositionName = (staff.StaffInfo != null && staff.StaffInfo.Position != null) ? staff.StaffInfo.Position.Name : string.Empty;
                result.Add(planDTOManage);
            }
            #endregion

            return result;
        }
    }
}
