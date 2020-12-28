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
using HRMWebApp.KPI.Core.Security;
using HRMWebApp.KPI.Core.Helpers;
using Microsoft.AspNet.Identity;
using System.Web;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class PlanKPIApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<PlanKPIDTO> GetList()
        {
            List<PlanKPIDTO> result = new List<PlanKPIDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<PlanKPI>().ToList().Map<PlanKPIDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public string GetDepartmentId()
        {
            Guid staffId = SessionHelper.Data<Guid>(SessionKey.ThongTinNhanVien);
            string DepartmentId = "";
            SessionManager.DoWork(session =>
            {
                Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();
                DepartmentId = staff.Department.Id.ToString();
            });
            return DepartmentId;
        }

        [Authorize]
        [Route("")]
        public PlanKPIDTO GetObj(Guid id)
        {
            PlanKPIDTO result = new PlanKPIDTO();
            SessionManager.DoWork(session =>
            {
                PlanKPI pk = session.Query<PlanKPI>().SingleOrDefault(a => a.Id == id);
                result = pk.Map<PlanKPIDTO>();
                result.PlanTypeId = pk.PlanType != null ? pk.PlanType.Id : -1;
                result.ParentId = pk.ParentPlan != null ? pk.ParentPlan.Id : (Guid?)null;
                result.AgentObjectIds = new List<Guid>();
                result.StudyTerm = pk.StudyTerm;
                result.StudyYear = pk.StudyYear;
                result.StudyYearId = pk.StudyYear1 != null ? pk.StudyYear1.Id : Guid.Empty;
                //foreach (AgentObject a in pk.AgentObjects)
                //{
                //    result.AgentObjectIds.Add(a.Id);
                //}
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<PlanTypeDTO> GetListPlanType()
        {
            List<PlanTypeDTO> result = new List<PlanTypeDTO>();
            SessionManager.DoWork(session =>
            {
                List<PlanType> temp = session.Query<PlanType>().ToList();
                foreach (PlanType pt in temp)
                {
                    PlanTypeDTO pd = pt.Map<PlanTypeDTO>();
                    result.Add(pd);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public bool GetIsSupervisor()
        {
            bool IsSupervisor = false;
            SessionManager.DoWork(session =>
            {
                Guid staffId = SessionHelper.Data<Guid>(SessionKey.ThongTinNhanVien);
                Staff staff = session.Query<Staff>().SingleOrDefault(s => s.Id == staffId);
                IsSupervisor = staff.StaffRoles.Count > 0 && staff.StaffRoles.Any(s => s.Id == 1);
            });
            return IsSupervisor;
        }

        [Authorize]
        [Route("")]
        public int GetCreatePlanKPIAllYear(Guid studyYearId)
        {
            int result = 1;
            SessionManager.DoWork(session =>
            {
                StudyYear st = session.Query<StudyYear>().SingleOrDefault(s => s.Id == studyYearId);
                int year1 = 0;
                int year2 = 0;
                if (st.Name != null)
                {
                    year1 = int.Parse(st.Name.Substring(0, 4));
                    year2 = int.Parse(st.Name.Substring(st.Name.Length - 4));
                }
                List<PlanKPI> pl = new List<PlanKPI>();
                //Tạo năm
                PlanKPI planYear = new PlanKPI();
                planYear.Id = Guid.NewGuid();
                planYear.Name = "Kế hoạch năm " + year1.ToString() + " - " + year2.ToString();
                planYear.StudyYear = st.Name;
                planYear.StudyYear1 = new StudyYear() { Id = studyYearId };
                planYear.StartTime = new DateTime(year1, 9, 1);
                planYear.EndTime = new DateTime(year2, 8, 31);
                planYear.RatingStartTime = planYear.StartTime;
                planYear.RatingEndTime = planYear.EndTime;
                planYear.PlanType = new PlanType() { Id = 1 };
                pl.Add(planYear);
                //Tạo học kỳ
                //for (int i = 1; i <= 2; i++)
                //{
                //    PlanKPI planSemester = new PlanKPI();
                //    planSemester.Id = Guid.NewGuid();
                //    planSemester.ParentPlan = planYear;
                //    planSemester.PlanType = new PlanType() { Id = 2 };
                //    switch (i)
                //    {
                //        case 1: //học kỳ 1
                //            {
                //                planSemester.Name = "Kế hoạch học kỳ I năm " + year1.ToString() + " - " + year2.ToString();
                //                planSemester.StudyTerm = "HK01";
                //                planSemester.StudyYear = st.Name;
                //                planSemester.StudyYear1 = new StudyYear() { Id = studyYearId };
                //                planSemester.StartTime = planYear.StartTime;
                //                planSemester.EndTime = planSemester.StartTime.AddMonths(6).AddDays(-1);
                //                planSemester.RatingStartTime = planSemester.StartTime;
                //                planSemester.RatingEndTime = planSemester.EndTime;
                //                pl.Add(planSemester);

                //                //Tạo tháng
                //                for (int j = 0; j <= 5; j++)
                //                {
                //                    PlanKPI planMonth = new PlanKPI();
                //                    planMonth.Id = Guid.NewGuid();
                //                    planMonth.ParentPlan = planSemester;
                //                    planMonth.PlanType = new PlanType() { Id = 3 };
                //                    planMonth.StartTime = planSemester.StartTime.AddMonths(j);
                //                    planMonth.EndTime = planMonth.StartTime.AddMonths(1).AddDays(-1);
                //                    planMonth.RatingStartTime = planMonth.StartTime;
                //                    planMonth.RatingEndTime = planMonth.EndTime;
                //                    planMonth.Name = "Kế hoạch tháng " + planMonth.StartTime.Month.ToString();
                //                    planMonth.StudyTerm = "HK01";
                //                    planMonth.StudyYear = st.Name;
                //                    planMonth.StudyYear1 = new StudyYear() { Id = studyYearId };
                //                    pl.Add(planMonth);

                //                }
                //            }
                //            break;
                //        case 2: //học kỳ 2
                //            {
                //                planSemester.Name = "Kế hoạch học kỳ II năm " + year1.ToString() + " - " + year2.ToString();
                //                planSemester.StudyTerm = "HK02";
                //                planSemester.StudyYear = st.Name;
                //                planSemester.StudyYear1 = new StudyYear() { Id = studyYearId };
                //                planSemester.StartTime = planYear.StartTime.AddMonths(6);
                //                planSemester.EndTime = planYear.EndTime;
                //                planSemester.RatingStartTime = planSemester.StartTime;
                //                planSemester.RatingEndTime = planSemester.EndTime;
                //                pl.Add(planSemester);

                //                //Tạo tháng
                //                for (int j = 0; j <= 5; j++)
                //                {
                //                    PlanKPI planMonth = new PlanKPI();
                //                    planMonth.Id = Guid.NewGuid();
                //                    planMonth.ParentPlan = planSemester;
                //                    planMonth.PlanType = new PlanType() { Id = 3 };
                //                    planMonth.StartTime = planSemester.StartTime.AddMonths(j);
                //                    planMonth.EndTime = planMonth.StartTime.AddMonths(1).AddDays(-1);
                //                    planMonth.RatingStartTime = planMonth.StartTime;
                //                    planMonth.RatingEndTime = planMonth.EndTime;
                //                    planMonth.Name = "Kế hoạch tháng " + planMonth.StartTime.Month.ToString();
                //                    planMonth.StudyTerm = "HK02";
                //                    planMonth.StudyYear = st.Name;
                //                    planMonth.StudyYear1 = new StudyYear() { Id = studyYearId };
                //                    pl.Add(planMonth);
                //                }
                //            }
                //            break;
                //    }
                //}
                foreach (PlanKPI p in pl)
                {
                    p.CreateTime = DateTime.Now;
                    session.Save(p);
                }

            });
            return result;
        }

        [Authorize]
        [Route("")]
        public bool GetCheckExistPlanKPI(Guid studyYearId)
        {
            bool result = false;
            SessionManager.DoWork(session =>
            {
                StudyYear st = session.Query<StudyYear>().SingleOrDefault(s => s.Id == studyYearId);
                result = session.Query<PlanKPI>().Any(p => p.StudyYear1 != null ? p.StudyYear1.Id == studyYearId : false);
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<UserPlanKPIDTO> GetListByUserId()
        {
            List<UserPlanKPIDTO> result = new List<UserPlanKPIDTO>();
            SessionManager.DoWork(session =>
            {
                Staff staff = ControllerHelpers.GetCurrentStaff(session);
                ApplicationUser currentUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                //bool IsSupervisor = staff.StaffRoles.Count > 0 && staff.StaffRoles.Any(s => s.Id == 1);
                //if (IsSupervisor && NormalStaffId != Guid.Empty)
                //{
                //    staff = session.Query<Staff>().SingleOrDefault(s => s.Id == NormalStaffId);                    
                //}
                List<AgentObject> agentObjects = new List<AgentObject>();
                if (staff != null)
                {
                    if (currentUser.IsKPIs == false && (currentUser.WebGroupId == "00000000-0000-0000-0000-000000000000" || currentUser.WebGroupId == "00000000-0000-0000-0000-000000000001" || currentUser.WebGroupId == "00000000-0000-0000-0000-000000000001"))
                    {
                        agentObjects = session.Query<AgentObject>().Where(a => a.AgentObjectType.Id == 4).ToList();
                    }
                }
                else
                {
                    agentObjects = staff.StaffInfo.AgentObjects.ToList();
                }
                Guid AgentObjectTypeId = Guid.Empty;

                foreach (AgentObject ob in agentObjects)
                {
                    PlanKPI plan = session.Query<PlanKPI>().Where(p => p.AgentObjects.Any(a => a.Id == ob.Id) && p.StartTime.Date <= DateTime.Now.Date && p.EndTime.Date >= DateTime.Now.Date).ToList().LastOrDefault();
                    if (plan != null)
                    {
                        ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                        if (!applicationUser.IsKPIs)
                        {
                            UserPlanKPIDTO planDTO = plan.Map<UserPlanKPIDTO>();
                            if (planDTO != null)
                            {
                                planDTO.AgentObjectName = ob.Name;
                                planDTO.AgentObjectId = ob.Id;
                                planDTO.StaffName = staff != null ? staff.StaffProfile.Name : string.Empty;
                                planDTO.StaffId = staff != null ? staff.Id : Guid.Empty;
                                planDTO.AgentObjectTypeId = ob.AgentObjectType.Id;
                                result.Add(planDTO);
                            }
                        }
                        else
                        {
                            UserPlanKPIDTO planDTO = plan.Map<UserPlanKPIDTO>();
                            //AgentObjectType at = session.Query<AgentObjectType>().Where(a =>a.Id== a.Id.ToString());
                            planDTO.AgentObjectName = applicationUser.AgentObjectTypeId;
                            planDTO.AgentObjectId = ob.Id;
                            planDTO.StaffName = staff.StaffProfile.Name;
                            planDTO.StaffId = staff.Id;
                            planDTO.AgentObjectTypeId = Convert.ToInt32(applicationUser.AgentObjectTypeId);
                            result.Add(planDTO);
                        }
                    }
                }
            });
            foreach (UserPlanKPIDTO plan in result)
            {
                plan.Name = "Soạn " + plan.Name;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public bool GetCheckIsSchoolPlan(Guid PlandId)
        {
            bool check = false;
            SessionManager.DoWork(session =>
            {
                PlanKPI plan = session.Query<PlanKPI>().SingleOrDefault(s => s.Id == PlandId);
                if (plan.ParentPlan == null)
                {
                    check = true;
                }
            });
            return check;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<UserPlanKPIDTO> GetListByPlanId(Guid NormalStaffId, Guid PlanId)
        {
            bool check = GetCheckIsSchoolPlan(PlanId);
            if (check == true)
            {
                List<UserPlanKPIDTO> result = new List<UserPlanKPIDTO>();
                SessionManager.DoWork(session =>
                {

                    Staff staff = ControllerHelpers.GetCurrentStaff(session);

                    bool IsSupervisor = staff.StaffRoles.Count > 0 && staff.StaffRoles.Any(s => s.Id == 1);
                    if (IsSupervisor && NormalStaffId != Guid.Empty)
                    {
                        staff = session.Query<Staff>().SingleOrDefault(s => s.Id == NormalStaffId);
                    }

                    List<Guid> agentObjectsId = staff.StaffInfo.AgentObjects.Select(s => s.Id).ToList();


                    foreach (AgentObject ob in staff.StaffInfo.AgentObjects)
                    {
                        PlanKPI plan = session.Query<PlanKPI>().Where(p => p.Id == PlanId).SingleOrDefault();
                        if (plan != null)
                        {
                            UserPlanKPIDTO planDTO = plan.Map<UserPlanKPIDTO>();
                            if (planDTO != null)
                            {
                                planDTO.AgentObjectName = ob.Name;
                                planDTO.AgentObjectId = ob.Id;
                                planDTO.StaffName = staff.StaffProfile.Name;
                                planDTO.StaffId = staff.Id;
                                planDTO.AgentObjectTypeId = ob.AgentObjectType.Id;
                                result.Add(planDTO);
                            }
                        }
                    }
                });
                return result;
            }
            else
            {
                List<UserPlanKPIDTO> result = new List<UserPlanKPIDTO>();
                SessionManager.DoWork(session =>
                {
                    Staff staff = ControllerHelpers.GetCurrentStaff(session);

                    bool IsSupervisor = staff.StaffRoles.Count > 0 && staff.StaffRoles.Any(s => s.Id == 1);
                    if (IsSupervisor && NormalStaffId != Guid.Empty)
                    {
                        staff = session.Query<Staff>().SingleOrDefault(s => s.Id == NormalStaffId);
                    }

                    List<Guid> agentObjectsId = staff.StaffInfo.AgentObjects.Select(s => s.Id).ToList();


                    foreach (AgentObject ob in staff.StaffInfo.AgentObjects)
                    {
                        PlanKPI plan = session.Query<PlanKPI>().Where(p => p.AgentObjects.Any(a => a.Id == ob.Id) && p.StartTime.Date <= DateTime.Now.Date && p.EndTime.Date >= DateTime.Now.Date).ToList().LastOrDefault();
                        if (plan != null)
                        {
                            UserPlanKPIDTO planDTO = plan.Map<UserPlanKPIDTO>();
                            if (planDTO != null)
                            {
                                planDTO.AgentObjectName = ob.Name;
                                planDTO.AgentObjectId = ob.Id;
                                planDTO.StaffName = staff.StaffProfile.Name;
                                planDTO.StaffId = staff.Id;
                                planDTO.AgentObjectTypeId = ob.AgentObjectType.Id;
                                result.Add(planDTO);
                            }
                        }
                    }
                });
                return result;
            }

        }

        public static UserPlanKPIDTO ParseUserPlan(AgentObject ob, Staff staff, PlanKPI plan, AgentObjectType agentObjectType)
        {
            UserPlanKPIDTO planDTO = new UserPlanKPIDTO();
            planDTO.AgentObjectName = ob != null ? ob.Name : agentObjectType.Name;
            //if (agentObjectType!=null && agentObjectType.Id==6)
            //{
            //    planDTO.AgentObjectName = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.Name : ob.AgentObjectType.Name;
            //}            
            planDTO.Name = plan != null ? plan.Name : planDTO.AgentObjectName;
            planDTO.AgentObjectId = ob != null ? ob.Id : Guid.Empty;
           // planDTO.AgentObjectName = ob != null ? ob.Name : string.Empty;
            planDTO.Id = plan.Id;
            planDTO.StaffName = staff.StaffProfile != null ? staff.StaffProfile.Name : string.Empty;
            planDTO.StaffId = staff.Id;
            planDTO.AgentObjectTypeId = ob != null ? ob.AgentObjectType.Id : agentObjectType.Id;
            return planDTO;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<UserPlanKPIDTO> GetPlanListDepartment(Guid normalStaffId, Guid planId)
        {
            List<UserPlanKPIDTO> result = new List<UserPlanKPIDTO>();
            SessionManager.DoWork(session =>
            {
                Staff staff = ControllerHelpers.GetCurrentStaff(session);


                bool IsSupervisor = staff.StaffRoles != null && staff.StaffRoles.Count > 0 && staff.StaffRoles.Any(s => s.Id == 1);
                if (IsSupervisor && normalStaffId != Guid.Empty)
                {
                    staff = session.Query<Staff>().SingleOrDefault(s => s.Id == normalStaffId);
                }

                List<Guid> agentObjectsId = staff.StaffInfo.AgentObjects.Select(s => s.Id).ToList();

                PlanKPI plan = session.Query<PlanKPI>().Where(p => p.Id == planId).SingleOrDefault();
                int planType = plan.PlanType.Id;

                //Trường hợp: User thường đăng nhập
                if (staff.Id != Guid.Empty)
                {
                    if (!SessionHelper.Data<bool>(SessionKey.IsKPIs))
                    {
                        List<AgentObject_Salary> agents_SalaryList = new List<AgentObject_Salary>();
                        if (staff.StaffSalaryInfo != null && staff.StaffSalaryInfo.ScaleSalary != null) //so sánh mã ngạch
                        {
                            agents_SalaryList = session.Query<AgentObject_Salary>().Where(sa => sa.ScaleSalary != null && sa.ScaleSalary.Id == staff.StaffSalaryInfo.ScaleSalary.Id).ToList();
                        }
                        if (agents_SalaryList.Count > 1) 
                        {
                            if (staff.StaffLevel != null && staff.StaffLevel.Qualification != null) //so sánh trình độ chuyên môn
                                agents_SalaryList = agents_SalaryList.Where(sa => sa.Qualification != null && sa.Qualification.Id == staff.StaffLevel.Qualification.Id).ToList();
                            else if (staff.StaffLevel != null && staff.StaffLevel.Qualification == null)
                                agents_SalaryList = agents_SalaryList.Where(sa => sa.Qualification == null).ToList();
                        }
                        if (agents_SalaryList.Count > 1) 
                        {
                            if (staff.StaffLevel != null && staff.StaffLevel.AcademicTitle != null) //so sánh học hàm
                                agents_SalaryList = agents_SalaryList.Where(sa => sa.AcademicTitle != null && sa.AcademicTitle.Id == staff.StaffLevel.AcademicTitle.Id).ToList();
                            else if (staff.StaffLevel != null && staff.StaffLevel.AcademicTitle == null)
                                agents_SalaryList = agents_SalaryList.Where(sa => sa.AcademicTitle == null).ToList();
                        }

                        if (agents_SalaryList.Count > 0)
                        {
                            var agentObject = session.Query<AgentObject>().Where(sa => sa.Id == agents_SalaryList.First().AgentObject.Id && sa.AgentObjectType.PlanTypes.Any(pt => pt.Id == planType)).SingleOrDefault();
                            UserPlanKPIDTO planDTO = plan.Map<UserPlanKPIDTO>();
                            if (planDTO != null && agentObject != null)
                            {
                                planDTO = ParseUserPlan(agentObject, staff, plan, null);
                                result.Add(planDTO);
                            }
                        }
                    }

                    //Chức vụ chính
                    AgentObject newObManage = new AgentObject();
                    UserPlanKPIDTO planDTOManage = null;
                    if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null)
                    {
                        newObManage = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == staff.StaffInfo.Position.AgentObjectType.Id && staff.StaffInfo.Position.AgentObjectType.Id != 9 && a.AgentObjectType.PlanTypes.Any(pt => pt.Id == planType));
                        if (newObManage != null)
                            planDTOManage = ParseUserPlan(newObManage, staff, plan, staff.StaffInfo.Position.AgentObjectType);

                    }
                    else
                    {
                        int agentObjectTypeId = staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1;
                        if (agentObjectTypeId == 2)
                        {
                            newObManage = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == agentObjectTypeId && a.AgentObjectType.PlanTypes.Any(pt => pt.Id == planType));
                            // sửa nhân viên không có kế hoạch năm => có
                            //if (newObManage != null)
                            //{
                                AgentObjectType at = session.Query<AgentObjectType>().SingleOrDefault(a => a.Id == agentObjectTypeId);

                                planDTOManage = ParseUserPlan(newObManage, staff, plan, at);

                           // }
                        }

                    }

                    //planDTOManage = ParseUserPlan(planDTOManage, newObManage, staff, staff.StaffInfo.Position.AgentObjectType);
                    if (planDTOManage != null)
                    {
                        planDTOManage.DepartmentId = staff.Department.Id; //thêm department để phân biệt chức vụ kiêm nhiệm khác khoa
                        //nếu là trưởng bộ môn thì gán departmentId là id của bộ môn
                        if (staff.StaffInfo.Position != null && staff.StaffInfo.Position.AgentObjectType != null && (staff.StaffInfo.Position.AgentObjectType.Id == 6 || staff.StaffInfo.Position.AgentObjectType.Id == 12))
                            planDTOManage.DepartmentId = staff.StaffInfo.Subject != null ? staff.StaffInfo.Subject.Id : staff.Department.Id;
                        result.Add(planDTOManage);
                    }

                    foreach (SubPosition subPo in staff.StaffInfo.SubPositions)
                    {
                        UserPlanKPIDTO planDTO = plan.Map<UserPlanKPIDTO>();
                        if (planDTO != null)
                        {
                            AgentObject newOb = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == subPo.Position.AgentObjectType.Id && a.AgentObjectType.PlanTypes.Any(pt => pt.Id == planType));
                            if (newOb != null)
                            {
                                planDTO = ParseUserPlan(newOb, staff, plan, subPo.Position.AgentObjectType);
                                planDTO.DepartmentId = subPo.Department.Id; //thêm department để phân biệt chức vụ kiêm nhiệm khác khoa
                                result.Add(planDTO);
                            }
                        }
                    }
                }
            });
            foreach (UserPlanKPIDTO plan in result)
            {
                plan.Name = "Soạn " + plan.Name;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<PlanKPIDTO> GetListByDepartment()
        {
            List<PlanKPIDTO> result = new List<PlanKPIDTO>();
            SessionManager.DoWork(session =>
            {
                Staff staff = ControllerHelpers.GetCurrentStaff(session);
                List<Guid> agentObjectsId = staff.StaffInfo.AgentObjects.Select(s => s.Id).ToList();
                if (staff.StaffInfo.Position != null)
                {
                    List<PlanKPI> pl = session.Query<PlanKPI>().Where(r=>r.PlanType.Id == 1).ToList();
                    foreach (PlanKPI pl2 in pl)
                    {
                        PlanKPIDTO pd = pl2.Map<PlanKPIDTO>();
                        if (pl2.ParentPlan != null)
                        {
                            pd.ParentId = pl2.ParentPlan.Id;
                            pd.PlanTypeId = pl2.PlanType.Id;
                        }
                        else
                        {
                            pd.ParentId = null;
                            pd.PlanTypeId = pl2.PlanType.Id;
                        }
                        result.Add(pd);
                    }


                }
                else
                {
                    List<PlanKPI> pl = new List<PlanKPI>();
                    //Lấy kế hoạch cho nhân viên
                    if (staff.StaffInfo.AgentObjects.Count <= 0)
                    {
                        int agentObjectTypeId = staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1;
                        //pl = session.Query<PlanKPI>().Where(p => p.PlanType.AgentObjectTypes.Any(a => a.Id == agentObjectTypeId)).ToList();
                        pl = session.Query<PlanKPI>().Where(r => r.PlanType.Id == 1).ToList();
                        foreach (PlanKPI pl2 in pl)
                        {
                            PlanKPIDTO pd = pl2.Map<PlanKPIDTO>();
                            if (pl2.ParentPlan != null)
                            {
                                pd.ParentId = pl2.ParentPlan.Id;
                                pd.PlanTypeId = pl2.PlanType.Id;
                            }
                            else
                            {
                                pd.ParentId = null;
                                pd.PlanTypeId = pl2.PlanType.Id;
                            }
                            result.Add(pd);
                        }
                    }
                    else
                    {
                        foreach (AgentObject ob in staff.StaffInfo.AgentObjects)
                        {
                            //pl = session.Query<PlanKPI>().Where(p => p.PlanType.AgentObjectTypes.Any(a => a.Id == ob.AgentObjectType.Id)).ToList();
                            pl = session.Query<PlanKPI>().Where(r => r.PlanType.Id == 1).ToList();
                            foreach (PlanKPI pl2 in pl)
                            {
                                PlanKPIDTO pd = pl2.Map<PlanKPIDTO>();
                                if (pl2.ParentPlan != null)
                                {
                                    pd.ParentId = pl2.ParentPlan.Id;
                                    pd.PlanTypeId = pl2.PlanType.Id;
                                }
                                else
                                {
                                    pd.ParentId = null;
                                    pd.PlanTypeId = pl2.PlanType.Id;
                                }
                                result.Add(pd);
                            }
                        }
                    }
                }
                List<PlanKPIDTO> result1 = result.GroupBy(r => r.Id).Select(r => r.First()).ToList();
                result = result1.OrderBy(r => r.StartTime).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<PlanKPIDTO> GetListByDepartment(Guid studyYearId)
        {
            List<PlanKPIDTO> result = new List<PlanKPIDTO>();
            SessionManager.DoWork(session =>
            {
                Staff staff = ControllerHelpers.GetCurrentStaff(session);
                List<Guid> agentObjectsId = staff.StaffInfo.AgentObjects.Select(s => s.Id).ToList();
                if (staff.StaffInfo.Position != null)
                {
                    List<PlanKPI> pl = session.Query<PlanKPI>().Where(p => p.StudyYear1.Id == studyYearId && p.PlanType.Id == 1).ToList();
                    foreach (PlanKPI pl2 in pl)
                    {
                        PlanKPIDTO pd = pl2.Map<PlanKPIDTO>();
                        if (pl2.ParentPlan != null)
                        {
                            pd.ParentId = pl2.ParentPlan.Id;
                            pd.PlanTypeId = pl2.PlanType.Id;
                        }
                        else
                        {
                            pd.ParentId = null;
                            pd.PlanTypeId = pl2.PlanType.Id;
                        }
                        result.Add(pd);
                    }


                }
                else
                {
                    List<PlanKPI> pl = new List<PlanKPI>();
                    //Lấy kế hoạch cho nhân viên
                    if (staff.StaffInfo.AgentObjects.Count <= 0)
                    {
                        int agentObjectTypeId = staff.StaffInfo.StaffType.ManageCode == "3" ? 2 : 1;
                        //pl = session.Query<PlanKPI>().Where(p => p.PlanType.AgentObjectTypes.Any(a => a.Id == agentObjectTypeId)).ToList();
                        pl = session.Query<PlanKPI>().Where(p => p.StudyYear1.Id == studyYearId && p.PlanType.Id == 1).ToList();
                        foreach (PlanKPI pl2 in pl)
                        {
                            PlanKPIDTO pd = pl2.Map<PlanKPIDTO>();
                            if (pl2.ParentPlan != null)
                            {
                                pd.ParentId = pl2.ParentPlan.Id;
                                pd.PlanTypeId = pl2.PlanType.Id;
                            }
                            else
                            {
                                pd.ParentId = null;
                                pd.PlanTypeId = pl2.PlanType.Id;
                            }
                            result.Add(pd);
                        }
                    }
                    else
                    {
                        foreach (AgentObject ob in staff.StaffInfo.AgentObjects)
                        {
                            //pl = session.Query<PlanKPI>().Where(p => p.PlanType.AgentObjectTypes.Any(a => a.Id == ob.AgentObjectType.Id)).ToList();
                            pl = session.Query<PlanKPI>().Where(p => p.StudyYear1.Id == studyYearId && p.PlanType.Id == 1).ToList();
                            foreach (PlanKPI pl2 in pl)
                            {
                                PlanKPIDTO pd = pl2.Map<PlanKPIDTO>();
                                if (pl2.ParentPlan != null)
                                {
                                    pd.ParentId = pl2.ParentPlan.Id;
                                    pd.PlanTypeId = pl2.PlanType.Id;
                                }
                                else
                                {
                                    pd.ParentId = null;
                                    pd.PlanTypeId = pl2.PlanType.Id;
                                }
                                result.Add(pd);
                            }
                        }
                    }
                }
                List<PlanKPIDTO> result1 = result.GroupBy(r => r.Id).Select(r => r.First()).ToList();
                result = result1.OrderBy(r => r.StartTime).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<StudyYearDTO> GetYearList()
        {
            List<StudyYearDTO> result = new List<StudyYearDTO>();
            SessionManager.DoWork(session =>
            {
                List<Guid> a = session.Query<PlanKPI>().Select(q => q.StudyYear1 != null ? q.StudyYear1.Id : Guid.Empty).ToList();
                result = session.Query<StudyYear>().Where(q => a.Contains(q.Id)).ToList().Map<StudyYearDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<PlanKPIDTO> GetListByStudyYearId(Guid studyYearId)
        {
            List<PlanKPIDTO> result = new List<PlanKPIDTO>();
            SessionManager.DoWork(session =>
            {
                List<PlanKPI> planList = new List<PlanKPI>();
                planList = session.Query<PlanKPI>().Where(p => p.StudyYear1 != null && p.StudyYear1.Id == studyYearId && p.PlanType.Id == 1).ToList();

                foreach (PlanKPI pl in planList)
                {
                    PlanKPIDTO pd = pl.Map<PlanKPIDTO>();
                    pd.ParentId = pl.ParentPlan != null ? (Guid?)pl.ParentPlan.Id : null;

                    result.Add(pd);
                }

            });
            result = result.OrderBy(r => r.StartTime).ToList();
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<PlanKPIDTO> GetListByAngentObjectId(Guid? classId)
        {
            List<PlanKPIDTO> result = new List<PlanKPIDTO>();
            Guid? classIdFormat = classId ?? null;
            SessionManager.DoWork(session =>
            {
                List<PlanKPI> planList = new List<PlanKPI>();
                if (classIdFormat == null)
                {
                    planList = session.Query<PlanKPI>().Where(p => p.Id != Guid.Empty).ToList();

                }
                else
                    planList = session.Query<PlanKPI>().Where(a => a.AgentObjects.Any(b => b.Id == classIdFormat && b.Id != Guid.Empty)).ToList();

                foreach (PlanKPI pl in planList)
                {
                    PlanKPIDTO pd = pl.Map<PlanKPIDTO>();
                    pd.ParentId = pl.ParentPlan != null ? (Guid?)pl.ParentPlan.Id : null;

                    result.Add(pd);
                }

            });
            result = result.OrderBy(r => r.StartTime).ToList();
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<PlanKPIDTO> GetListByParentId(Guid parentId)
        {
            List<PlanKPIDTO> result = new List<PlanKPIDTO>();
            SessionManager.DoWork(session =>
            {
                if (parentId != Guid.Empty)
                {
                    List<PlanKPI> planList = session.Query<PlanKPI>().Where(p => p.ParentPlan.Id == parentId).ToList();
                    foreach (PlanKPI pl in planList)
                    {
                        PlanKPIDTO pd = pl.Map<PlanKPIDTO>();
                        pd.ParentId = pl.ParentPlan.Id;
                        result.Add(pd);
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<PlanKPIDTO> GetParentPlanListById(Guid id)
        {
            var result = new List<PlanKPIDTO>();
            SessionManager.DoWork(session =>
            {
                PlanKPI plan = new PlanKPI();
                plan = session.Query<PlanKPI>().Where(p => p.Id == id).SingleOrDefault();
                if (plan.ParentPlan != null)
                {
                    result = session.Query<PlanKPI>().Where(p => p.PlanType == plan.ParentPlan.PlanType).OrderBy(p => p.StartTime).Map<PlanKPIDTO>().ToList();
                }

            });
            return result;
        }

        [Authorize]
        [Route("")]
        public PlanKPIDTO PutNewPlan(PlanKPIDTO obj)
        {
            List<PlanKPI> result = new List<PlanKPI>();
            SessionManager.DoWork(session =>
            {
                //Tạo kế hoạch năm
                PlanKPI yearplan = new PlanKPI();
                yearplan.Id = Guid.NewGuid();
                yearplan.Name = "Kế hoạch năm " + obj.FromYear.ToString() + " - " + obj.ToYear.ToString();
                yearplan.ParentPlan = null;
                yearplan.StartTime = obj.StartTime.ToLocalTime();
                yearplan.EndTime = obj.EndTime.ToLocalTime();
                yearplan.RatingStartTime = obj.StartTime.ToLocalTime();
                yearplan.RatingEndTime = obj.EndTime.ToLocalTime();
                yearplan.StudyYear = obj.FromYear.ToString() + "-" + obj.ToYear.ToString();
                yearplan.CreateTime = DateTime.Now.ToLocalTime();
                yearplan.PlanType = new PlanType() { Id = 1 };
                List<Guid> yearAgentObjectIds = session.Query<AgentObject>().Select(a => a.Id).ToList();
                yearplan.AgentObjects = new List<AgentObject>();
                foreach (Guid a in yearAgentObjectIds)
                {
                    yearplan.AgentObjects.Add(new AgentObject() { Id = a });
                }
                result.Add(yearplan);

                //Tạo kế hoạch học kỳ
                for (int i = 1; i <= 2; i++)
                {
                    PlanKPI semesterPlan = new PlanKPI();
                    semesterPlan.Id = Guid.NewGuid();
                    semesterPlan.ParentPlan = yearplan;
                    semesterPlan.PlanType = new PlanType() { Id = 2 };
                    semesterPlan.CreateTime = DateTime.Now.ToLocalTime();
                    List<Guid> semesterAgentObjectIds = session.Query<AgentObject>().Select(a => a.Id).ToList();
                    semesterPlan.AgentObjects = new List<AgentObject>();
                    foreach (Guid a in semesterAgentObjectIds)
                    {
                        semesterPlan.AgentObjects.Add(new AgentObject() { Id = a });
                    }
                    switch (i)
                    {
                        case 1:
                            {
                                semesterPlan.Name = "Kế hoạch học kỳ I năm " + obj.FromYear.ToString() + " - " + obj.ToYear.ToString();
                                semesterPlan.StartTime = yearplan.StartTime;
                                semesterPlan.EndTime = semesterPlan.StartTime.AddMonths(6).AddDays(-1);
                                semesterPlan.RatingStartTime = semesterPlan.StartTime;
                                semesterPlan.RatingEndTime = semesterPlan.EndTime;
                                semesterPlan.StudyTerm = "HK01";
                                semesterPlan.StudyYear = obj.FromYear.ToString() + "-" + obj.ToYear.ToString();
                                result.Add(semesterPlan);

                                //Tạo kế hoạch tháng
                                for (int j = 0; j <= 5; j++)
                                {
                                    PlanKPI monthPlan = new PlanKPI();
                                    monthPlan.Id = Guid.NewGuid();
                                    monthPlan.ParentPlan = semesterPlan;
                                    monthPlan.PlanType = new PlanType() { Id = 2 };
                                    monthPlan.CreateTime = DateTime.Now.ToLocalTime();
                                    List<Guid> monthAgentObjectIds = session.Query<AgentObject>().Where(a => a.AgentObjectType.Id == 2 || a.AgentObjectType.Id == 3 || a.AgentObjectType.Id == 4 || a.AgentObjectType.Id == 5 || a.AgentObjectType.Id == 6).Select(a => a.Id).ToList();
                                    monthPlan.AgentObjects = new List<AgentObject>();
                                    foreach (Guid a in monthAgentObjectIds)
                                    {
                                        monthPlan.AgentObjects.Add(new AgentObject() { Id = a });
                                    }
                                    monthPlan.StartTime = semesterPlan.StartTime.AddMonths(j);
                                    monthPlan.EndTime = monthPlan.StartTime.AddMonths(1).AddDays(-1);
                                    monthPlan.RatingStartTime = monthPlan.StartTime;
                                    monthPlan.RatingEndTime = monthPlan.EndTime;
                                    monthPlan.Name = "Kế hoạch tháng " + monthPlan.StartTime.Month.ToString();
                                    result.Add(monthPlan);
                                }
                            }
                            break;
                        case 2:
                            {
                                semesterPlan.Name = "Kế hoạch học kỳ II năm " + obj.FromYear.ToString() + " - " + obj.ToYear.ToString();
                                semesterPlan.StartTime = yearplan.StartTime.AddMonths(6);
                                semesterPlan.EndTime = yearplan.EndTime;
                                semesterPlan.RatingStartTime = semesterPlan.StartTime;
                                semesterPlan.RatingEndTime = semesterPlan.EndTime;
                                semesterPlan.StudyTerm = "HK02";
                                semesterPlan.StudyYear = obj.FromYear.ToString() + "-" + obj.ToYear.ToString();
                                result.Add(semesterPlan);

                                //Tạo kế hoạch tháng
                                for (int j = 0; j <= 5; j++)
                                {
                                    PlanKPI monthPlan = new PlanKPI();
                                    monthPlan.Id = Guid.NewGuid();
                                    monthPlan.ParentPlan = semesterPlan;
                                    monthPlan.PlanType = new PlanType() { Id = 3 };
                                    monthPlan.CreateTime = DateTime.Now.ToLocalTime();
                                    List<Guid> monthAgentObjectIds = session.Query<AgentObject>().Where(a => a.AgentObjectType.Id == 2 || a.AgentObjectType.Id == 3 || a.AgentObjectType.Id == 4 || a.AgentObjectType.Id == 5 || a.AgentObjectType.Id == 6).Select(a => a.Id).ToList();
                                    monthPlan.AgentObjects = new List<AgentObject>();
                                    foreach (Guid a in monthAgentObjectIds)
                                    {
                                        monthPlan.AgentObjects.Add(new AgentObject() { Id = a });
                                    }
                                    monthPlan.StartTime = semesterPlan.StartTime.AddMonths(j);
                                    monthPlan.EndTime = monthPlan.StartTime.AddMonths(1).AddDays(-1);
                                    monthPlan.RatingStartTime = monthPlan.StartTime;
                                    monthPlan.RatingEndTime = monthPlan.EndTime;
                                    monthPlan.Name = "Kế hoạch tháng " + monthPlan.StartTime.Month.ToString();
                                    result.Add(monthPlan);
                                }
                            }
                            break;
                    }
                }

                foreach (PlanKPI p in result)
                {
                    session.Save(p);
                }
            });

            return obj;
        }

        [Authorize]
        [Route("")]
        public PlanKPIDTO Put(PlanKPIDTO obj)
        {
            PlanKPI result = new PlanKPI();
            SessionManager.DoWork(session =>
            {
                PlanKPI parentPlan = new PlanKPI();
                if (obj.ParentId != null)
                {
                    parentPlan = session.Query<PlanKPI>().Where(p => p.Id == obj.ParentId).SingleOrDefault();
                    result.ParentPlan = parentPlan;
                }
                result.Id = obj.Id;
                result.Name = obj.Name;
                result.PlanType = new PlanType() { Id = obj.PlanTypeId };
                result.StartTime = obj.StartTime.ToLocalTime();
                result.EndTime = obj.EndTime.ToLocalTime();
                result.RatingStartTime = obj.RatingStartTime.ToLocalTime();
                result.RatingEndTime = obj.RatingEndTime.ToLocalTime();
                result.StudyTerm = obj.StudyTerm;
                result.StudyYear = obj.StudyYear;
                result.StudyYear1 = new StudyYear() { Id = obj.StudyYearId };
                result.CreateTime = DateTime.Now;
                session.SaveOrUpdate(result);
            });

            return obj;
        }

        [Authorize]
        [Route("")]
        public PlanKPIDTO PutNew(PlanKPIDTO obj)
        {
            PlanKPI result = new PlanKPI();
            SessionManager.DoWork(session =>
            {
                result.Id = Guid.NewGuid();
                result.Name = obj.Name;
                result.PlanType = new PlanType() { Id = obj.PlanTypeId };
                result.StartTime = obj.StartTime.ToLocalTime();
                result.EndTime = obj.EndTime.ToLocalTime();
                result.RatingStartTime = obj.RatingStartTime.ToLocalTime();
                result.RatingEndTime = obj.RatingEndTime.ToLocalTime();
                result.CreateTime = DateTime.Now;
                switch (obj.PlanTypeId)
                {
                    case 1:
                        {

                        }
                        break;
                    case 2:
                        {
                            PlanKPI parentPlan = session.Query<PlanKPI>().Where(p => p.PlanType.Id == 1 && p.StartTime <= result.StartTime && p.EndTime >= result.EndTime).FirstOrDefault();
                            if (parentPlan != null)
                            {
                                result.ParentPlan = new PlanKPI() { Id = parentPlan.Id };
                            }
                        }
                        break;
                    case 3:
                        {
                            PlanKPI parentPlan = session.Query<PlanKPI>().Where(p => p.PlanType.Id == 2 && p.StartTime <= result.StartTime && p.EndTime >= result.EndTime).FirstOrDefault();
                            if (parentPlan != null)
                            {
                                result.ParentPlan = new PlanKPI() { Id = parentPlan.Id };
                            }
                        }
                        break;
                }
                session.SaveOrUpdate(result);
            });

            return obj;
        }

        public DateTime GetDateTime()
        {
            return DateTime.Now.ToLocalTime();
        }

        public static string GetPlanMonthAndYear(Guid planId)
        {
            string result = "";
            SessionManager.DoWork(session =>
            {
                PlanKPI plan = session.Query<PlanKPI>().SingleOrDefault(q => q.Id == planId);
                if (plan != null)
                {
                    //var text = plan.Name;
                    //var punctuation = text.Where(Char.IsPunctuation).Distinct().ToArray();
                    //var words = text.Split().Select(x => x.Trim(punctuation));
                    result = plan.Name.Substring(9);
                    if (plan.PlanType.Id == 3)
                        result = result + "/" + plan.StartTime.Year;
                }
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public PlanKPI Delete(PlanKPI obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
        }
        //public PlanKPIDTO Put(PlanKPIDTO obj)
        //{
        //    PlanKPI result = new PlanKPI();
        //    SessionManager.DoWork(session =>
        //    {
        //        List<AgentObject> agentObjects = new List<AgentObject>();
        //        foreach (Guid p in obj.AgentObjectIds)
        //        {
        //            agentObjects.Add(new AgentObject() { Id = p });
        //        }
        //        result = obj.Map<PlanKPI>();
        //        if (obj.Id == Guid.Empty)
        //        {
        //            result.Id = Guid.NewGuid();
        //            if (obj.SelectedId == null)
        //                result.ParentPlan = null;
        //            else
        //            {
        //                if (obj.SelectedParentId == null)
        //                    result.ParentPlan = new PlanKPI() { Id = (Guid)obj.SelectedId };
        //                else
        //                {
        //                    result.ParentPlan = new PlanKPI() { Id = (Guid)obj.SelectedParentId };
        //                }
        //            }
        //            result.CreateTime = DateTime.Now;
        //        }
        //        if (obj.ParentId != null)
        //            result.ParentPlan = new PlanKPI() { Id = (Guid)obj.ParentId };
        //        result.CreateTime = DateTime.Now;
        //        result.AgentObjects = agentObjects;
        //        session.SaveOrUpdate(result);
        //    });

        //    return obj;
        //}
    }
}
