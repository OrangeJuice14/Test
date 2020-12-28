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

namespace HRMWebApp.KPI.Core.Controllers
{
    public class StaffApiController : ApiController
    {
        public IEnumerable<StaffDTO> GetList()
        {
            var result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0).ToList().Map<StaffDTO>();

            });
            return result;
        }
        public StaffDTO ParseStaff(Staff staff)
        {
            StaffDTO sd = new StaffDTO();
            sd.Id = staff.Id;
            sd.Name = staff.StaffProfile.Name;
            sd.ManageCode = staff.StaffInfo.ManageCode;
            sd.DepartmentName = staff.Department.Name;
            sd.PositionName = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.Name : "";
            return sd;
        }
        public Dictionary<string, object> GetListPaging(int skip, int take, Guid departmentId)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            int totalRecord = 0;
            var data = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                //data = session.Query<Staff>().Skip(skip).Take(take).ToList().Map<StaffDTO>();
                List<Staff> data1 = departmentId == Guid.Empty ? session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.WebUser !=null && s.StaffInfo.WebUser !=null).Skip(skip).Take(take).ToList() : session.Query<Staff>().Where(a => a.Department.Id == departmentId && a.StaffStatus.NoLongerWork == 0 && a.StaffProfile.GCRecord == null && a.StaffInfo!= null).Skip(skip).Take(take).ToList();
                data = departmentId == Guid.Empty ? session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.WebUser !=null && s.StaffInfo.WebUser !=null).Skip(skip).Take(take).ToList().Map<StaffDTO>() : session.Query<Staff>().Where(a => a.Department.Id == departmentId && a.StaffStatus.NoLongerWork == 0 && a.StaffProfile.GCRecord == null && a.StaffInfo != null && a.StaffInfo.WebUser !=null).Skip(skip).Take(take).ToList().Map<StaffDTO>();
                if (data.Count<=0)
                {
                    data = departmentId == Guid.Empty ? session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.WebUser !=null && s.StaffInfo.WebUser !=null).Skip(skip).Take(take).ToList().Map<StaffDTO>() : session.Query<Staff>().Where(a => a.StaffInfo.Subject.Id == departmentId && a.StaffStatus.NoLongerWork == 0 && a.StaffProfile.GCRecord == null && a.StaffInfo.WebUser !=null && a.StaffInfo.WebUser !=null).Skip(skip).Take(take).ToList().Map<StaffDTO>();
                }
                foreach (StaffDTO st in data)
                {
                    st.Name = st.StaffProfile.Name;
                }
                totalRecord = departmentId == Guid.Empty ? session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.WebUser !=null && s.StaffInfo.WebUser !=null).Count() : session.Query<Staff>().Where(a => a.Department.Id == departmentId && a.StaffStatus.NoLongerWork == 0 && a.StaffProfile.GCRecord == null && a.StaffInfo.WebUser !=null && a.StaffInfo.WebUser !=null).Count();
            });
            result["data"] = data;
            result["total"] = totalRecord;
            return result;
        }
        public IEnumerable<StaffDTO> GetListAll()
        {
            List<StaffDTO> result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                List<Staff> templist = new List<Staff>();
                templist = session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.WebUser != null && s.StaffInfo.WebUser != null).ToList();
                foreach (Staff s in templist)
                {
                    StaffDTO sd = new StaffDTO();
                    sd = ParseStaff(s);
                    result.Add(sd);
                }
            });
            return result;
        }
        public IEnumerable<StaffDTO> GetListAdminLeader()
        {
            List<StaffDTO> result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                List<Staff> templist = new List<Staff>();
                templist = session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && (s.StaffInfo.Position.AgentObjectType.Id==10 || s.StaffInfo.Position.AgentObjectType.Id == 11)).OrderBy(s=>s.StaffInfo.Position.AgentObjectType.Id).ToList();
                foreach (Staff s in templist)
                {
                    StaffDTO sd = new StaffDTO();
                    sd = ParseStaff(s);
                    result.Add(sd);
                }
            });
            return result;
        }
        public Dictionary<string, object> GetSearch(int skip, int take, Guid departmentId, string staffName)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            int totalRecord = 0;
            var data = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                //data = session.Query<Staff>().Skip(skip).Take(take).ToList().Map<StaffDTO>();
                if (staffName != null)
                {
                    data = departmentId == Guid.Empty ? session.Query<Staff>().Where(s => s.StaffProfile.Name.Contains(staffName) && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.WebUser !=null).Skip(skip).Take(take).ToList().Map<StaffDTO>() : session.Query<Staff>().Where(a => a.Department.Id == departmentId && a.StaffProfile.Name.Contains(staffName) && a.StaffStatus.NoLongerWork == 0 && a.StaffProfile.GCRecord == null && a.StaffInfo.WebUser !=null).Skip(skip).Take(take).ToList().Map<StaffDTO>();
                    if (data.Count <= 0)
                    {
                        data = departmentId == Guid.Empty ? session.Query<Staff>().Where(s => s.StaffProfile.Name.Contains(staffName) && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.WebUser !=null).Skip(skip).Take(take).ToList().Map<StaffDTO>() : session.Query<Staff>().Where(a => a.StaffInfo.Subject.Id == departmentId && a.StaffProfile.Name.Contains(staffName) && a.StaffStatus.NoLongerWork == 0 && a.StaffProfile.GCRecord == null && a.StaffInfo.WebUser !=null).Skip(skip).Take(take).ToList().Map<StaffDTO>();
                    }
                }
                else
                {
                    data = departmentId == Guid.Empty ? session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.WebUser !=null).Skip(skip).Take(take).ToList().Map<StaffDTO>() : session.Query<Staff>().Where(a => a.Department.Id == departmentId && a.StaffStatus.NoLongerWork == 0 && a.StaffProfile.GCRecord == null && a.StaffInfo.WebUser !=null).Skip(skip).Take(take).ToList().Map<StaffDTO>();
                    if (data.Count <= 0)
                    {
                        data = departmentId == Guid.Empty ? session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.WebUser !=null).Skip(skip).Take(take).ToList().Map<StaffDTO>() : session.Query<Staff>().Where(a => a.StaffInfo.Subject.Id == departmentId && a.StaffStatus.NoLongerWork == 0 && a.StaffProfile.GCRecord == null && a.StaffInfo.WebUser !=null).Skip(skip).Take(take).ToList().Map<StaffDTO>();
                    }
                }
                foreach (StaffDTO st in data)
                {
                    st.Name = st.StaffProfile.Name;
                }
                totalRecord = departmentId == Guid.Empty ? session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.WebUser !=null && s.StaffInfo.WebUser !=null).Count() : session.Query<Staff>().Where(a => a.Department.Id == departmentId && a.StaffStatus.NoLongerWork == 0 && a.StaffProfile.GCRecord == null && a.StaffInfo.WebUser !=null && a.StaffInfo.WebUser !=null).Count();
            });
            result["data"] = data;
            result["total"] = totalRecord;
            return result;
        }
        public IEnumerable<StaffDTO> GetAutoCompleteAll()
        {
            List<StaffDTO> result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                List<Staff> templist = new List<Staff>();
                templist = session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.WebUser != null).ToList();
                foreach (Staff s in templist)
                {
                    StaffDTO sd = new StaffDTO();
                    sd = ParseStaff(s);
                    result.Add(sd);
                }
            });
            return result;
        }
        public IEnumerable<StaffDTO> GetSearchDept(Guid deptId)
        {
            List<StaffDTO> result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                if (deptId!=Guid.Empty)
                {
                    List<Staff> templist = new List<Staff>();
                    templist = session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.WebUser != null && s.Department.Id == deptId && s.StaffInfo.Position==null).ToList();
                    foreach (Staff s in templist)
                    {
                        StaffDTO sd = new StaffDTO();
                        sd = ParseStaff(s);
                        result.Add(sd);
                    }
                }               
            });
            return result;
        }
        public IEnumerable<StaffDTO> GetSearchDeptOnlyProfessor(Guid deptId)
        {
            List<StaffDTO> result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                if (deptId != Guid.Empty)
                {
                    List<Staff> templist = new List<Staff>();
                    templist = session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.WebUser != null && s.Department.Id == deptId && s.StaffInfo.Position == null && s.StaffInfo.StaffType.ManageCode=="4").ToList();
                    foreach (Staff s in templist)
                    {
                        StaffDTO sd = new StaffDTO();
                        sd = ParseStaff(s);
                        result.Add(sd);
                    }
                }
            });
            return result;
        }
        public IEnumerable<StaffDTO> GetDepartmentStaff(int agentObjectTypeId, Guid planId)
        {
            List<StaffDTO> result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                Staff staff = ControllerHelpers.GetCurrentStaff(session);
                Guid staffId = staff.Id;
                switch (agentObjectTypeId)
                {
                    case (int)AgentObjectTypes.PhongBan:
                        {
                            List<Staff> stafflist = session.Query<Staff>().Where(s => s.Department.Id == staff.Department.Id && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.Position==null).ToList();
                            foreach (Staff s in stafflist)
                            {
                                if (s.Id != staffId)
                                {
                                    StaffDTO sd = new StaffDTO();
                                    sd.Name = s.StaffProfile.Name;
                                    sd.Id = s.Id;
                                    sd.AgentObjectIds = new List<Guid>();

                                    //if (staff.StaffInfo.Position == null)
                                    //{
                                        AgentObject ao = session.Query<AgentObject>().SingleOrDefault(a => a.AgentObjectType.Id == 2);
                                        sd.AgentObjectIds.Add(ao.Id);
                                    //}
                                    //else
                                    //{
                                    //    foreach (AgentObject a in s.StaffInfo.AgentObjects)
                                    //    {
                                    //        if (a.AgentObjectType.Id == 2)
                                    //        {
                                    //            sd.AgentObjectIds.Add(a.Id);
                                    //        }
                                    //    }
                                    //}



                                    if (sd.AgentObjectIds.Count > 0)
                                    {
                                        sd.AgentObjectId = sd.AgentObjectIds.FirstOrDefault();
                                    }
                                    sd.IsApproved = false;
                                    sd.IsStaffRated = false;
                                    sd.IsSupervisorRated = false;
                                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null).SingleOrDefault();
                                    if(planStaff!=null)
                                    {
                                        //Kế hoạch được trưởng phòng duyệt
                                        sd.IsApproved = planStaff.IsLocked;
                                        Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                        if (rs!=null)
                                        {
                                            //Trưởng phòng đánh giá
                                            sd.IsSupervisorRated = rs.TotalRecord > 0? true : false;
                                            //Nhân viên đã đánh giá
                                            sd.IsStaffRated = rs.TempRecord>0 || sd.IsSupervisorRated==true ? true : false;

                                        }
                                    }
                                    result.Add(sd);
                                }
                            }
                        }
                        break;
                        //User có cả 3 kế hoạch năm hk tháng
                    case 100:
                        {
                            List<Staff> stafflist = session.Query<Staff>().Where(s => s.Department.Id == staff.Department.Id && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffProfile.GCRecord == null).ToList();
                            foreach (Staff s in stafflist)
                            {
                                if (s.Id != staffId)
                                {
                                    StaffDTO sd = new StaffDTO();
                                    sd.Name = s.StaffProfile.Name;
                                    sd.Id = s.Id;
                                    sd.AgentObjectIds = new List<Guid>();
                                    foreach (AgentObject a in s.StaffInfo.AgentObjects)
                                    {
                                        if (a.AgentObjectType.Id == 2)
                                        {
                                            sd.AgentObjectIds.Add(a.Id);
                                        }
                                    }
                                    if (sd.AgentObjectIds.Count > 0)
                                    {
                                        sd.AgentObjectId = sd.AgentObjectIds.FirstOrDefault();
                                    }
                                    sd.IsApproved = false;
                                    sd.IsStaffRated = false;
                                    sd.IsSupervisorRated = false;
                                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null).SingleOrDefault();
                                    if (planStaff != null)
                                    {
                                        //Kế hoạch được trưởng phòng duyệt
                                        sd.IsApproved = planStaff.IsLocked;
                                        Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                        if (rs != null)
                                        {
                                            //Trưởng phòng đánh giá
                                            sd.IsSupervisorRated = rs.TotalRecord > 0 && rs.IsLocked == true ? true : false;
                                            //Nhân viên đã đánh giá
                                            sd.IsStaffRated = (rs.TotalRecord > 0 && rs.IsLocked == false) || sd.IsSupervisorRated == true ? true : false;

                                        }
                                    }
                                    result.Add(sd);
                                }
                            }
                        }
                        break;
                    case (int)AgentObjectTypes.BoMon:
                        {
                            List<Staff> stafflist = session.Query<Staff>().Where(s => s.StaffInfo.Subject.Id == staff.StaffInfo.Subject.Id && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffProfile.GCRecord == null).ToList();
                            foreach (Staff s in stafflist)
                            {
                                if (s.Id!=staffId)
                                { 
                                    StaffDTO sd =new StaffDTO();
                                    sd.Name = s.StaffProfile.Name;
                                    sd.Id = s.Id;
                                    sd.AgentObjectIds = new List<Guid>();
                                    foreach (AgentObject a in s.StaffInfo.AgentObjects)
                                    {
                                        if (a.AgentObjectType.Id==1)
                                        {
                                            sd.AgentObjectIds.Add(a.Id);
                                        }                                       
                                    }
                                    if (sd.AgentObjectIds.Count>0)
                                    {
                                        sd.AgentObjectId = sd.AgentObjectIds.FirstOrDefault();
                                    }
                                    sd.IsApproved = false;
                                    sd.IsStaffRated = false;
                                    sd.IsSupervisorRated = false;
                                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null).SingleOrDefault();
                                    if (planStaff != null)
                                    {
                                        //Kế hoạch được trưởng bộ môn duyệt
                                        sd.IsApproved = planStaff.IsLocked;
                                        Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                        if (rs != null)
                                        {
                                            //Trưởng bộ môn  đánh giá
                                            sd.IsSupervisorRated = rs.TotalRecord > 0 && rs.IsLocked == true ? true : false;
                                            //Giảng viên đã đánh giá
                                            sd.IsStaffRated = (rs.TotalRecord > 0 && rs.IsLocked == false) || sd.IsSupervisorRated == true ? true : false;

                                        }
                                    }
                                    result.Add(sd);
                                }
                            }
                        }
                        break;
                    case (int)AgentObjectTypes.Khoa:
                        {
                            List<Staff> stafflist = session.Query<Staff>().Where(s => s.Department.Id == staff.Department.Id && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && (s.StaffInfo.Position.AgentObjectType.Id==8 || s.StaffInfo.SubPositions.Any(p=>p.Position.AgentObjectType.Id==8))).ToList();
                            foreach (Staff s in stafflist)
                            {
                                if (s.Id != staffId)
                                {
                                    StaffDTO sd = new StaffDTO();
                                    sd.Name = s.StaffProfile.Name;
                                    sd.Id = s.Id;
                                    AgentObject a = session.Query<AgentObject>().Where(ag => ag.AgentObjectType.Id == 8).FirstOrDefault();
                                    sd.AgentObjectId = a != null ? a.Id : Guid.Empty;
                                    sd.IsApproved = false;
                                    sd.IsStaffRated = false;
                                    sd.IsSupervisorRated = false;
                                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null).SingleOrDefault();
                                    if (planStaff != null)
                                    {
                                        //Kế hoạch được trưởng khoa duyệt
                                        sd.IsApproved = planStaff.IsLocked;
                                        Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                        if (rs != null)
                                        {
                                            //Trưởng khoa  đánh giá
                                            sd.IsSupervisorRated = rs.TotalRecord > 0 && rs.IsLocked == true ? true : false;
                                            //Phó trưởng khoa đã đánh giá
                                            sd.IsStaffRated = (rs.TotalRecord > 0 && rs.IsLocked == false) || sd.IsSupervisorRated == true ? true : false;

                                        }
                                    }
                                    result.Add(sd);
                                }
                            }
                        }
                        break;
                }                
            });
            return result;
        }
        public StaffDTO GetObj(Guid id)
        {
            var result = new StaffDTO();
            SessionManager.DoWork(session =>
            {
                Staff staff = session.Query<Staff>().SingleOrDefault(a => a.Id == id);
                result = staff.Map<StaffDTO>();
                result.DepartmentId = result.Department.Id;
                result.AgentObjectIds = new List<Guid>();
                result.DepartmentIds = new List<Guid>();
                if (staff != null)
                    foreach (var agentObject in staff.StaffInfo.AgentObjects)
                    {
                        result.AgentObjectIds.Add(agentObject.Id);
                    }
            });
            return result;
        }
        public string GetDepartmentLeaderAgentObjectId(Guid departmentId)
        {
            string result = "";
            SessionManager.DoWork(session =>
            {
                Department dept = session.Query<Department>().Where(d => d.Id == departmentId).SingleOrDefault();
                if (dept!=null)
                {
                    int deptType = dept.DepartmentType;
                    switch(deptType)
                    {
                        //Phòng ban
                        case 1:
                            {
                                //Gắn cứng đối tượng phòng ban
                                result = "9DD10B40-440D-4903-AA2B-6E9A9910B218";
                            }
                            break;
                        //Khoa
                        case 4:
                            {
                                //Gắn cứng đối tượng khoa
                                result = "A02E6A2E-21FC-4C37-B87E-F5DCA1128263";
                            }
                            break;
                        //Khác
                        default:
                            {

                            }
                            break;
                    }
                }
                
            });
            return result;
        }
        public string GetDepartmentLeaderId(Guid departmentId)
        {
            string result = "";
            SessionManager.DoWork(session =>
            {
                Department dept = session.Query<Department>().Where(d => d.Id == departmentId).SingleOrDefault();
                if (dept != null)
                {
                    int deptType = dept.DepartmentType;
                    switch (deptType)
                    {
                        //Phòng ban
                        case 1:
                            {
                                Staff staff = session.Query<Staff>().Where(s => s.Department.Id == departmentId && s.StaffInfo.Position.AgentObjectType.Id == 3 && s.StaffProfile.GCRecord == null && s.StaffStatus.NoLongerWork == 0).SingleOrDefault();
                                if (staff != null)
                                {
                                    result = staff.Id.ToString();
                                }
                            }
                            break;
                        //Khoa
                        case 4:
                            {
                                Staff staff = session.Query<Staff>().Where(s => s.Department.Id == departmentId && s.StaffInfo.Position.AgentObjectType.Id == 5 && s.StaffProfile.GCRecord == null && s.StaffStatus.NoLongerWork == 0).SingleOrDefault();
                                if (staff != null)
                                {
                                    result = staff.Id.ToString();
                                }
                            }
                            break;
                        //Khác
                        default:
                            {

                            }
                            break;
                    }
                }

            });
            return result;
        }
        public StaffDTO getCurrentStaff()
        {
            var result = new StaffDTO();
            SessionManager.DoWork(session =>
            {
                Staff staff = ControllerHelpers.GetCurrentStaff(session);
                ApplicationUser currentUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);

                if (staff != null && staff.Id != Guid.Empty)
                    result.Id = staff.Id;
                else
                    //user KPI ko co staff
                    result.Id = new Guid("00000000-0000-0000-0000-000000000001");


                result.Department =staff.Department!=null? staff.Department.Map<DepartmentDTO>():null;
              
                if (staff.Id != Guid.Empty)
                {
                    
                    result.Name = staff.StaffProfile.Name;
                    result.StaffStatusId = staff.StaffStatus.Id;
                   


                    result.DepartmentId = result.Department.Id;
                    result.AgentObjectIds = new List<Guid>();
                    if (staff != null)
                        foreach (var agentObject in staff.StaffInfo.AgentObjects)
                        {
                            result.AgentObjectIds.Add(agentObject.Id);
                        }
                }
                else
                {
                    result.AgentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
                }
            });
            return result;
        }
        public Guid GetCurrentUserGroupId()
        {
            Guid groupId = Guid.Empty;
            SessionManager.DoWork(session =>
            {
                groupId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId);
            });
            return groupId;
        }

        public int GetAgentObjectTypeIdByStaffId(Guid id)
        {
            int result = -1;
            SessionManager.DoWork(session =>
            {
                Staff staff = session.Query<Staff>().Where(s => s.Id == id).SingleOrDefault();
                if (staff != null && staff.StaffInfo.Position == null)
                {
                    if (staff.StaffInfo.StaffType.ManageCode == "3")
                        result = 2; //Nhân viên
                    else
                        result = 1; //Giảng viên
                }
            });
            return result;
        }
        public Guid GetAgentObjectIdByStaffId(Guid id)
        {
            Guid result = Guid.Empty;
            SessionManager.DoWork(session =>
            {
                Staff staff = session.Query<Staff>().Where(s => s.Id == id).SingleOrDefault();
                if (staff != null && staff.StaffInfo.Position == null)
                {
                    result = staff.StaffInfo.AgentObjects.FirstOrDefault().Id;
                }
            });
            return result;
        }

        public IEnumerable<StaffDTO> GetListbyId(Guid departmentId)
        {
            var result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                result = departmentId == Guid.Empty ? session.Query<Staff>().ToList().Map<StaffDTO>() : session.Query<Staff>().Where(a => a.Department.Id == departmentId).Map<StaffDTO>();
                foreach (StaffDTO st in result)
                {
                    st.Name = st.StaffProfile.Name;
                }
            });

            return result;
        }

        /// <summary>
        /// GetStaffByAgentObjectType
        /// </summary>
        /// <param name="typeId">Mã loại đối tượng. Ghi chú: 99 nhân viên, 100: giảng viên</param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public IEnumerable<StaffDTO> GetStaffByAgentObjectType(int typeId, Guid departmentId, int userRole)
        {
            var result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                if (userRole == 1)
                {
                    Guid IdBGH = new Guid("406F6E70-C3D9-49D8-B6F6-E39649BA8458");
                    List<Staff> resultQuery = null;
                    resultQuery = session.Query<Staff>().Where(a => a.StaffProfile.GCRecord == null && a.StaffStatus.NoLongerWork == 0 && a.Department.Id== IdBGH).ToList();
                    foreach (Staff st in resultQuery)
                    {
                        StaffDTO staffDTO = st.Map<StaffDTO>();
                        result.Add(staffDTO);
                    }
                    foreach (StaffDTO st in result)
                    {
                        st.Name = st.StaffProfile.Name;

                    }
                }
                else
                {
                    IQueryable<Staff> resultQuery = null;
                    switch (typeId)
                    {
                        case 99:
                            {
                                //Chỉ lấy nhân viên phòng ban
                                //resultQuery = session.Query<Staff>().Where(a => a.StaffInfo.Position == null && a.StaffStatus.NoLongerWork == 0 && a.StaffInfo.StaffType.ManageCode == "3" && a.StaffProfile.GCRecord == null && a.StaffInfo.WebUser !=null).AsQueryable();
                                //Lấy tất cả trừ trưởng phòng
                                resultQuery = session.Query<Staff>().Where(a => (a.StaffInfo.Position==null || a.StaffInfo.Position.AgentObjectType.Id!=3) && a.StaffStatus.NoLongerWork == 0 && a.StaffProfile.GCRecord == null && a.StaffInfo.WebUser != null).AsQueryable();
                            }
                            break;
                        case 100:
                            {
                                resultQuery = session.Query<Staff>().Where(a => a.StaffInfo.Position == null && a.StaffStatus.NoLongerWork == 0 && a.StaffInfo.StaffType.ManageCode == "4" && a.StaffProfile.GCRecord == null && a.StaffInfo.WebUser !=null).AsQueryable();
                            }
                            break;
                        default:
                            {
                                resultQuery = session.Query<Staff>().Where(a => a.StaffProfile.GCRecord == null && a.StaffStatus.NoLongerWork == 0 && a.StaffInfo.Position.AgentObjectType.Id == typeId || a.StaffInfo.Position.AgentObjectType.ParentAgentObjectType.Id == typeId).AsQueryable();
                            }
                            break;
                    }

                    if (departmentId != Guid.Empty)
                    {
                        if (typeId == 3)
                        {
                            List<Staff> staffResult = session.Query<Staff>().Where(s => s.Department.Id == departmentId && (s.StaffInfo.Position.Name.Contains("Trưởng Phòng") || s.StaffInfo.Position.Name.Contains("Phó Trưởng phòng"))).ToList();
                            foreach (Staff st in staffResult)
                            {
                                StaffDTO staffDTO = st.Map<StaffDTO>();
                                staffDTO.Position = st.StaffInfo.Position != null ? st.StaffInfo.Position.Map<PositionDTO>() : null;
                                result.Add(staffDTO);
                            }
                        }
                        else
                        {
                            List<Staff> staffResult = resultQuery.Where(s => s.Department.Id == departmentId).ToList();
                            foreach (Staff st in staffResult)
                            {
                                StaffDTO staffDTO = st.Map<StaffDTO>();
                                staffDTO.Position = st.StaffInfo.Position != null ? st.StaffInfo.Position.Map<PositionDTO>() : null;
                                result.Add(staffDTO);
                            }
                        }
                        //result = staffResult.Map<StaffDTO>();


                    }
                    else
                        result = resultQuery.ToList().Map<StaffDTO>();
                    foreach (StaffDTO st in result)
                    {
                        st.Name = st.StaffProfile.Name;

                    }
                }
            });
            return result;
        }
        public IEnumerable<StaffDTO> GetViceDepartmentStaff(int typeId, Guid departmentId, Guid planId)
        {
            var result = new List<StaffDTO>();

         
            SessionManager.DoWork(session =>
            {
                if (departmentId == Guid.Empty)
                    departmentId = ControllerHelpers.GetCurrentStaff(session).Department.Id;


                int selectedTypeId = 0;
                switch(typeId)
                {
                    case (int)AgentObjectTypes.Khoa:
                        {
                            selectedTypeId = (int)AgentObjectTypes.PhoKhoa;
                        }
                        break;
                    case (int)AgentObjectTypes.PhongBan:
                        {
                            selectedTypeId = (int)AgentObjectTypes.PhoPhongBan;
                        }
                        break;
                    case (int)AgentObjectTypes.HieuTruong:
                        {
                            selectedTypeId = (int)AgentObjectTypes.PhoHieuTruong;
                        }
                        break;
                }
                IQueryable<Staff> resultQuery = session.Query<Staff>().Where(s => s.Department.Id == departmentId && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && (s.StaffInfo.Position.AgentObjectType.Id == selectedTypeId || s.StaffInfo.SubPositions.Any(p => p.Position.AgentObjectType.Id == selectedTypeId)));


                //IQueryable<Staff> resultQuery = null;
                //resultQuery = session.Query<Staff>().Where(a => a.StaffInfo.Position.AgentObjectType.ParentAgentObjectType.Id == typeId).AsQueryable();
                if (departmentId != Guid.Empty)
                {
                    result = resultQuery.Where(s => s.Department.Id == departmentId).ToList().Map<StaffDTO>();
                    foreach (StaffDTO s in result)
                    {
                        AgentObject a = session.Query<AgentObject>().Where(ag => ag.AgentObjectType.Id == selectedTypeId).FirstOrDefault();
                        s.AgentObjectId = a != null ? a.Id : Guid.Empty;
                    }
                }
                else
                {
                    result = resultQuery.ToList().Map<StaffDTO>();                                      
                }
                foreach (StaffDTO st in result)
                {
                    st.Name = st.StaffProfile.Name;
                    st.IsApproved = false;
                    st.IsStaffRated = false;
                    st.IsSupervisorRated = false;
                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == st.Id && p.AgentObjectType.Id==st.AgentObjectTypeId).SingleOrDefault();
                    if (planStaff != null)
                    {
                        //Kế hoạch được trưởng phòng duyệt
                        st.IsApproved = planStaff.IsLocked;
                        Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                        if (rs != null)
                        {
                            //Trưởng phòng đánh giá
                            st.IsSupervisorRated = rs.TotalRecord > 0 ? true : false;
                            //Nhân viên đã đánh giá
                            st.IsStaffRated = rs.TempRecord > 0 || st.IsSupervisorRated == true ? true : false;

                        }
                    }
                }
            });
            return result;
        }

        public StaffDTO GetDepartmentLeader(Guid departmentId)
        {
            var result = new StaffDTO();
            SessionManager.DoWork(session =>
            {                             
                if (departmentId != Guid.Empty)
                {
                    Staff staff = session.Query<Staff>().FirstOrDefault(s => s.Department.Id == departmentId && (s.StaffInfo.Position.AgentObjectType.Id == 3 || s.StaffInfo.Position.AgentObjectType.Id == 5));
                    if(staff!=null)
                        result = ParseStaff(staff);
                }                              
            });
            return result;
        }

        public static Staff GetStaffDepartmentLeader(Guid departmentId)
        {
            var result = new Staff();
            SessionManager.DoWork(session =>
            {
                if (departmentId != Guid.Empty)
                {
                    result = session.Query<Staff>().FirstOrDefault(s => s.Department.Id == departmentId && (s.StaffInfo.Position.AgentObjectType.Id == 3 || s.StaffInfo.Position.AgentObjectType.Id == 5));
                }
            });
            return result;
        }
       
        public IEnumerable<StaffDTO> GetProfessorInSubject(int typeId)
        {
            var result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                if (typeId == 6)
                {
                    Staff staff = ControllerHelpers.GetCurrentStaff(session);
                    Guid subjectId = staff.StaffInfo.Subject.Id;
                    result = session.Query<Staff>().Where(a => a.StaffInfo.Subject.Id == subjectId).ToList().Map<StaffDTO>();
                    foreach (StaffDTO st in result)
                    {
                        st.Name = st.StaffProfile.Name;
                    }
                }
            });
            return result;
        }
        public int Put(StaffDTO obj)
        {
            StaffInfo staff = new StaffInfo();
            SessionManager.DoWork(session =>
                {
                    staff = session.Query<StaffInfo>().SingleOrDefault(a => a.Id == obj.Id);
                    staff.AgentObjects = new List<AgentObject>();
                    foreach (var id in obj.AgentObjectIds)
                    {
                        staff.AgentObjects.Add(new AgentObject { Id = id });
                    }
                    session.Merge(staff);
                });
            return 1;
        }
        public int PutDepartmentManage(StaffDTO obj)
        {
            Staff staff = new Staff();
            int result = 0;
            try
            {
                SessionManager.DoWork(session =>
                {
                    staff = session.Query<Staff>().SingleOrDefault(a => a.Id == obj.Id);
                    staff.Departments = new List<Department>();
                    obj.DepartmentIds.Remove(new Guid("00000000-0000-0000-0000-000000000001"));
                    obj.DepartmentIds.Remove(new Guid("00000000-0000-0000-0000-000000000002"));
                    obj.DepartmentIds.Remove(new Guid("00000000-0000-0000-0000-000000000003"));
                    foreach (var id in obj.DepartmentIds)
                    {
                        Department dept = new Department { Id = id };
                        bool check = session.Query<Staff>().Any(s =>s.Id!=obj.Id && s.Departments.Contains(dept));
                        if (check)
                        {
                            result = 2;
                            break;
                        }
                        else
                        {
                            staff.Departments.Add(dept);
                            session.Update(staff);
                            result = 1;
                        }
                        
                    }
                });
            }
            catch (Exception e)
            {
                result = 0;
            }         
            return result;
        }

        public Staff Delete(Staff obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }
    }
}
