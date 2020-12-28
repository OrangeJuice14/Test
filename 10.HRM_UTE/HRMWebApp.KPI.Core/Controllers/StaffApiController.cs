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
        [Authorize]
        [Route("")]
        public IEnumerable<StaffDTO> GetList()
        {
            var result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                List<Staff> temp = new List<Staff>();
                temp = session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord==null).ToList();
                foreach (Staff s in temp)
                {
                    StaffDTO sd = new StaffDTO();
                    sd.Id = s.Id;
                    sd.Name = s.StaffProfile.Name;
                    result.Add(sd);
                }

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
        public StaffDTO ParsePureStaff(Staff staff)
        {
            StaffDTO sd = new StaffDTO();
            sd.Id = staff.Id;
            sd.Name = staff.StaffProfile.Name;
            sd.DepartmentName = staff.Department.Name;
            return sd;
        }
        public StaffDTO ParseFullStaff(Staff staff)
        {
            StaffDTO sd = new StaffDTO();
            sd.Id = staff.Id;
            sd.Department.Id = staff.Department != null ? staff.Department.Id : Guid.Empty;
            sd.Department.Name = staff.Department != null ? staff.Department.Name : "";
            sd.DepartmentId = staff.Department != null ? staff.Department.Id : Guid.Empty;
            sd.DepartmentName = staff.Department != null ? staff.Department.Name : "";
            sd.Name = staff.StaffProfile != null ? staff.StaffProfile.Name : "";
            sd.ManageCode = staff.StaffInfo != null ? staff.StaffInfo.ManageCode : "";
            sd.PositionName = staff.StaffInfo.Position != null ? staff.StaffInfo.Position.Name : "";
            return sd;
        }

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
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
        [Authorize]
        [Route("")]
        public IEnumerable<StaffDTO> GetSearchOnlyProfessor()
        {
            List<StaffDTO> result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                    List<Staff> templist = new List<Staff>();
                    templist = session.Query<Staff>().Where(
                        s => s.StaffStatus.NoLongerWork == 0 
                        && s.StaffProfile.GCRecord == null 
                        && s.StaffInfo.WebUser != null 
                        && s.StaffInfo.StaffType.ManageCode != "3").ToList();
                    foreach (Staff s in templist)
                    {
                        StaffDTO sd = new StaffDTO();
                        sd = ParsePureStaff(s);
                        result.Add(sd);
                    }
            });
            return result;
        }
        //[Authorize]
        //[Route("")]
        //public IEnumerable<StaffDTO> GetDepartmentStaff(int agentObjectTypeId, Guid planId)
        //{
        //    List<StaffDTO> result = new List<StaffDTO>();
        //    SessionManager.DoWork(session =>
        //    {
        //        Staff staff = ControllerHelpers.GetCurrentStaff(session);
        //        Guid staffId = staff.Id;
        //        switch (agentObjectTypeId)
        //        {
        //            case (int)AgentObjectTypes.PhongBan:
        //                {
        //                    List<Staff> stafflist = session.Query<Staff>().Where(s => s.Department.Id == staff.Department.Id && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffInfo.Position==null).ToList();
        //                    foreach (Staff s in stafflist)
        //                    {
        //                        if (s.Id != staffId)
        //                        {
        //                            StaffDTO sd = new StaffDTO();
        //                            sd.Name = s.StaffProfile.Name;
        //                            sd.Id = s.Id;
        //                            sd.AgentObjectIds = new List<Guid>();

        //                            //if (staff.StaffInfo.Position == null)
        //                            //{
        //                                AgentObject ao = session.Query<AgentObject>().SingleOrDefault(a => a.AgentObjectType.Id == 2);
        //                                sd.AgentObjectIds.Add(ao.Id);
        //                            //}
        //                            //else
        //                            //{
        //                            //    foreach (AgentObject a in s.StaffInfo.AgentObjects)
        //                            //    {
        //                            //        if (a.AgentObjectType.Id == 2)
        //                            //        {
        //                            //            sd.AgentObjectIds.Add(a.Id);
        //                            //        }
        //                            //    }
        //                            //}



        //                            if (sd.AgentObjectIds.Count > 0)
        //                            {
        //                                sd.AgentObjectId = sd.AgentObjectIds.FirstOrDefault();
        //                            }
        //                            sd.IsApproved = false;
        //                            sd.IsStaffRated = false;
        //                            sd.IsSupervisorRated = false;
        //                            PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null).SingleOrDefault();
        //                            if(planStaff!=null)
        //                            {
        //                                //Kế hoạch được trưởng phòng duyệt
        //                                sd.IsApproved = planStaff.IsLocked;
        //                                Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
        //                                if (rs!=null)
        //                                {
        //                                    //Trưởng phòng đánh giá
        //                                    sd.IsSupervisorRated = rs.TotalRecord > 0? true : false;
        //                                    //Nhân viên đã đánh giá
        //                                    sd.IsStaffRated = rs.TempRecord>0 || sd.IsSupervisorRated==true ? true : false;

        //                                }
        //                            }
        //                            result.Add(sd);
        //                        }
        //                    }
        //                }
        //                break;
        //                //User có cả 3 kế hoạch năm hk tháng
        //            case 100:
        //                {
        //                    List<Staff> stafflist = session.Query<Staff>().Where(s => s.Department.Id == staff.Department.Id && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffProfile.GCRecord == null).ToList();
        //                    foreach (Staff s in stafflist)
        //                    {
        //                        if (s.Id != staffId)
        //                        {
        //                            StaffDTO sd = new StaffDTO();
        //                            sd.Name = s.StaffProfile.Name;
        //                            sd.Id = s.Id;
        //                            sd.AgentObjectIds = new List<Guid>();
        //                            foreach (AgentObject a in s.StaffInfo.AgentObjects)
        //                            {
        //                                if (a.AgentObjectType.Id == 2)
        //                                {
        //                                    sd.AgentObjectIds.Add(a.Id);
        //                                }
        //                            }
        //                            if (sd.AgentObjectIds.Count > 0)
        //                            {
        //                                sd.AgentObjectId = sd.AgentObjectIds.FirstOrDefault();
        //                            }
        //                            sd.IsApproved = false;
        //                            sd.IsStaffRated = false;
        //                            sd.IsSupervisorRated = false;
        //                            PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null).SingleOrDefault();
        //                            if (planStaff != null)
        //                            {
        //                                //Kế hoạch được trưởng phòng duyệt
        //                                sd.IsApproved = planStaff.IsLocked;
        //                                Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
        //                                if (rs != null)
        //                                {
        //                                    //Trưởng phòng đánh giá
        //                                    sd.IsSupervisorRated = rs.TotalRecord > 0 && rs.IsLocked == true ? true : false;
        //                                    //Nhân viên đã đánh giá
        //                                    sd.IsStaffRated = (rs.TotalRecord > 0 && rs.IsLocked == false) || sd.IsSupervisorRated == true ? true : false;

        //                                }
        //                            }
        //                            result.Add(sd);
        //                        }
        //                    }
        //                }
        //                break;
        //            case (int)AgentObjectTypes.BoMon:
        //                {
        //                    List<Staff> stafflist = session.Query<Staff>().Where(s => s.StaffInfo.Subject.Id == staff.StaffInfo.Subject.Id && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.StaffProfile.GCRecord == null).ToList();
        //                    foreach (Staff s in stafflist)
        //                    {
        //                        if (s.Id!=staffId)
        //                        { 
        //                            StaffDTO sd =new StaffDTO();
        //                            sd.Name = s.StaffProfile.Name;
        //                            sd.Id = s.Id;
        //                            sd.AgentObjectIds = new List<Guid>();
        //                            foreach (AgentObject a in s.StaffInfo.AgentObjects)
        //                            {
        //                                if (a.AgentObjectType.Id==1)
        //                                {
        //                                    sd.AgentObjectIds.Add(a.Id);
        //                                }                                       
        //                            }
        //                            if (sd.AgentObjectIds.Count>0)
        //                            {
        //                                sd.AgentObjectId = sd.AgentObjectIds.FirstOrDefault();
        //                            }
        //                            sd.IsApproved = false;
        //                            sd.IsStaffRated = false;
        //                            sd.IsSupervisorRated = false;
        //                            PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null).SingleOrDefault();
        //                            if (planStaff != null)
        //                            {
        //                                //Kế hoạch được trưởng bộ môn duyệt
        //                                sd.IsApproved = planStaff.IsLocked;
        //                                Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
        //                                if (rs != null)
        //                                {
        //                                    //Trưởng bộ môn  đánh giá
        //                                    sd.IsSupervisorRated = rs.TotalRecord > 0 && rs.IsLocked == true ? true : false;
        //                                    //Giảng viên đã đánh giá
        //                                    sd.IsStaffRated = (rs.TotalRecord > 0 && rs.IsLocked == false) || sd.IsSupervisorRated == true ? true : false;

        //                                }
        //                            }
        //                            result.Add(sd);
        //                        }
        //                    }
        //                }
        //                break;
        //            case (int)AgentObjectTypes.Khoa:
        //                {
        //                    List<Staff> stafflist = session.Query<Staff>().Where(s => s.Department.Id == staff.Department.Id && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && (s.StaffInfo.Position.AgentObjectType.Id==8 || s.StaffInfo.SubPositions.Any(p=>p.Position.AgentObjectType.Id==8))).ToList();
        //                    foreach (Staff s in stafflist)
        //                    {
        //                        if (s.Id != staffId)
        //                        {
        //                            StaffDTO sd = new StaffDTO();
        //                            sd.Name = s.StaffProfile.Name;
        //                            sd.Id = s.Id;
        //                            AgentObject a = session.Query<AgentObject>().Where(ag => ag.AgentObjectType.Id == 8).FirstOrDefault();
        //                            sd.AgentObjectId = a != null ? a.Id : Guid.Empty;
        //                            sd.IsApproved = false;
        //                            sd.IsStaffRated = false;
        //                            sd.IsSupervisorRated = false;
        //                            PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null).SingleOrDefault();
        //                            if (planStaff != null)
        //                            {
        //                                //Kế hoạch được trưởng khoa duyệt
        //                                sd.IsApproved = planStaff.IsLocked;
        //                                Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
        //                                if (rs != null)
        //                                {
        //                                    //Trưởng khoa  đánh giá
        //                                    sd.IsSupervisorRated = rs.TotalRecord > 0 && rs.IsLocked == true ? true : false;
        //                                    //Phó trưởng khoa đã đánh giá
        //                                    sd.IsStaffRated = (rs.TotalRecord > 0 && rs.IsLocked == false) || sd.IsSupervisorRated == true ? true : false;

        //                                }
        //                            }
        //                            result.Add(sd);
        //                        }
        //                    }
        //                }
        //                break;
        //        }                
        //    });
        //    return result;
        //}
        
        [Authorize]
        [Route("")]
        public Dictionary<string, object> GetDepartmentStaff(int agentObjectTypeId, Guid planId)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            var data = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                Staff staff = ControllerHelpers.GetCurrentStaff(session);
                Guid staffId = staff.Id;
                switch (agentObjectTypeId)
                {
                    case (int)AgentObjectTypes.PhongBan:
                        {
                            List<Staff> stafflist = session.Query<Staff>().Where(s => s.Department != null && s.Department.Id == staff.Department.Id && s.StaffStatus != null && s.StaffInfo != null && s.StaffProfile != null && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && (s.StaffInfo.Position == null || (s.StaffInfo.Position != null && s.StaffInfo.Position.AgentObjectType != null && s.StaffInfo.Position.AgentObjectType.Id == 2))).ToList();
                            foreach (Staff s in stafflist)
                            {
                                if (s.Id != staffId)
                                {
                                    StaffDTO sd = new StaffDTO();
                                    sd.Name = s.StaffProfile.Name;
                                    sd.Id = s.Id;
                                    sd.DepartmentId = s.Department.Id;
                                    sd.AgentObjectIds = new List<Guid>();

                                    //if (staff.StaffInfo.Position == null)
                                    //{
                                    AgentObject ao = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == 2);
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
                                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null && p.AgentObjectType != null && p.AgentObjectType.Id == 2).SingleOrDefault();
                                    if (planStaff != null)
                                    {
                                        //Kế hoạch được trưởng phòng duyệt
                                        sd.IsApproved = planStaff.IsLocked;
                                        Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                        if (rs != null)
                                        {
                                            //Trưởng phòng đánh giá
                                            sd.IsSupervisorRated = rs.IsLocked ? true : false;
                                            //Nhân viên đã đánh giá
                                            sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                                        }
                                    }
                                    data.Add(sd);
                                }
                            }
                            result["department"] = data;
                        }
                        break;
                    //User có cả 3 kế hoạch năm hk tháng
                    //Chỗ này ko hiểu lắm
                    case 100:
                        {
                            List<Staff> stafflist = session.Query<Staff>().Where(s => s.Department != null && s.Department.Id == staff.Department.Id && s.StaffStatus != null && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile != null && s.StaffProfile.GCRecord == null).ToList();
                            foreach (Staff s in stafflist)
                            {
                                if (s.Id != staffId)
                                {
                                    StaffDTO sd = new StaffDTO();
                                    sd.Name = s.StaffProfile.Name;
                                    sd.Id = s.Id;
                                    sd.DepartmentId = s.Department.Id;
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
                                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null && p.AgentObjectType != null && p.AgentObjectType.Id == 2).SingleOrDefault();
                                    if (planStaff != null)
                                    {
                                        //Kế hoạch được trưởng phòng duyệt
                                        sd.IsApproved = planStaff.IsLocked;
                                        Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                        if (rs != null)
                                        {
                                            //Trưởng phòng đánh giá
                                            sd.IsSupervisorRated = rs.IsLocked ? true : false;
                                            //Nhân viên đã đánh giá
                                            sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                                        }
                                    }
                                    data.Add(sd);
                                }
                            }
                            result["case100"] = data;
                        }
                        break;
                    case (int)AgentObjectTypes.BoMon:
                        {
                            List<Staff> listOfDepartment = session.Query<Staff>().Where(s => s.Department != null && s.Department.Id == staff.Department.Id && s.StaffStatus != null && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile != null && s.StaffProfile.GCRecord == null).ToList();
                            List<Staff> listOfSubject = listOfDepartment.Where(s => s.StaffInfo != null && s.StaffInfo.Subject != null && s.StaffInfo.Subject.Id == staff.StaffInfo.Subject.Id && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null).ToList();
                            List<Staff> staffList = listOfSubject.Where(s => s.StaffInfo != null && s.StaffInfo.StaffType != null && s.StaffInfo.StaffType.ManageCode == "3").ToList();
                            List<Staff> professorList = listOfSubject.Where(s => s.StaffInfo != null && s.StaffInfo.StaffType != null && s.StaffInfo.StaffType.ManageCode != "3").ToList();

                            foreach (Staff s in professorList)
                            {
                                if (s.Id != staffId)
                                {
                                    StaffDTO sd = new StaffDTO();
                                    sd.Name = s.StaffProfile.Name;
                                    sd.Id = s.Id;
                                    sd.DepartmentId = s.Department.Id;
                                    sd.AgentObjectIds = new List<Guid>();
                                    foreach (AgentObject a in s.StaffInfo.AgentObjects)
                                    {
                                        if (a.AgentObjectType.Id == 1)
                                        {
                                            sd.AgentObjectIds.Add(a.Id);
                                        }
                                    }
                                    if (sd.AgentObjectIds.Count > 0)
                                    {
                                        sd.AgentObjectId = sd.AgentObjectIds.FirstOrDefault();
                                    }
                                    else
                                    {
                                        //gán AgentObject giảng viên
                                        sd.AgentObjectId = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == 1).Id;
                                    }
                                    sd.IsApproved = false;
                                    sd.IsStaffRated = false;
                                    sd.IsSupervisorRated = false;
                                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null && p.AgentObjectType != null && p.AgentObjectType.Id == 1).SingleOrDefault();
                                    if (planStaff != null)
                                    {
                                        sd.PlanStaffId = planStaff.Id; //dùng trong chức năng duyệt chế độ làm việc của giảng viên
                                        sd.WorkingModeName = (planStaff.AgentObjectDetail != null && planStaff.AgentObjectDetail.WorkingMode != null) ? planStaff.AgentObjectDetail.WorkingMode.Name : null;
                                        sd.IsWorkingModeLocked = planStaff.IsWorkingModeLocked;
                                        //Kế hoạch được trưởng bộ môn duyệt
                                        sd.IsApproved = planStaff.IsLocked;
                                        Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                        if (rs != null)
                                        {
                                            //Trưởng bộ môn  đánh giá
                                            sd.IsSupervisorRated = rs.IsLocked ? true : false;
                                            //Giảng viên đã đánh giá
                                            sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                                        }
                                    }
                                    data.Add(sd);
                                }
                            }
                            result["subjectProfessor"] = data;

                            data = new List<StaffDTO>();

                            foreach (Staff s in staffList)
                            {
                                if (s.Id != staffId)
                                {
                                    StaffDTO sd = new StaffDTO();
                                    sd.Name = s.StaffProfile.Name;
                                    sd.Id = s.Id;
                                    sd.DepartmentId = s.Department.Id;
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
                                    else
                                    {
                                        //gán AgentObject nhân viên
                                        sd.AgentObjectId = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == 2).Id;
                                    }
                                    sd.IsApproved = false;
                                    sd.IsStaffRated = false;
                                    sd.IsSupervisorRated = false;
                                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null && p.AgentObjectType != null && p.AgentObjectType.Id == 2).SingleOrDefault();
                                    if (planStaff != null)
                                    {
                                        //Kế hoạch được trưởng bộ môn duyệt
                                        sd.IsApproved = planStaff.IsLocked;
                                        Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                        if (rs != null)
                                        {
                                            //Trưởng bộ môn  đánh giá
                                            sd.IsSupervisorRated = rs.IsLocked ? true : false;
                                            //Giảng viên đã đánh giá
                                            sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                                        }
                                    }
                                    data.Add(sd);
                                }
                            }
                            result["subjectStaff"] = data;
                        }
                        break;
                    case (int)AgentObjectTypes.Khoa:
                        {
                            List<Staff> list = session.Query<Staff>().Where(s => s.Department != null && s.Department.Id == staff.Department.Id && s.StaffStatus != null && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile != null && s.StaffProfile.GCRecord == null).ToList();
                            
                            //toàn bộ nhân sự của khoa có bộ môn
                            List<Staff> listHasSubject = list.Where(s => s.StaffInfo != null && s.StaffInfo.Subject != null && s.StaffInfo.Subject.Id != staff.Department.Id).OrderBy(s => s.StaffInfo.Subject.Id).ToList();
                            
                            //toàn bộ nhân sự của khoa không có bộ môn
                            List<Staff> listSubjectNull = list.Where(s => s.StaffInfo != null && (s.StaffInfo.Subject == null || s.StaffInfo.Subject.Id == staff.Department.Id)).ToList();

                            //giảng viên của khoa có bộ môn
                            List<Staff> professorListHasSubject = listHasSubject.Where(s => s.StaffInfo != null && s.StaffInfo.StaffType !=null && s.StaffInfo.StaffType.ManageCode == "4").ToList();

                            //giảng viên của khoa không có bộ môn
                            List<Staff> professorListSubjectNull = listSubjectNull.Where(s => s.StaffInfo != null && s.StaffInfo.StaffType != null && s.StaffInfo.StaffType.ManageCode == "4").ToList();

                            //nhân viên của khoa có bộ môn
                            List<Staff> staffListHasSubject = listHasSubject.Where(s => s.StaffInfo != null && s.StaffInfo.StaffType != null && s.StaffInfo.StaffType.ManageCode == "3").ToList();

                            //nhân viên của khoa không có bộ môn
                            List<Staff> staffListSubjectNull = listSubjectNull.Where(s => s.StaffInfo != null && s.StaffInfo.StaffType != null && s.StaffInfo.StaffType.ManageCode == "3").ToList();

                            //Danh sách bộ môn thuộc khoa
                            var subjects = session.Query<Department>().Where(d => d.ParentDepartment != null && d.ParentDepartment.Id == staff.Department.Id && d.IsDisable == false).ToList();


                            //nếu trưởng khoa kiêm nhiệm trưởng bộ môn thì loại bộ môn kiêm nhiệm ra khỏi danh sách bộ môn
                            if (staff.StaffInfo.SubPositions != null)
                            {
                                foreach (var subp in staff.StaffInfo.SubPositions)
                                {
                                    if (subp.Position.AgentObjectType.Id == 6 || subp.Position.AgentObjectType.Id == 12)
                                    {
                                        subjects = subjects.Where(s => s.Id != subp.Department.Id).ToList();
                                    }
                                }
                            }
                            ////trưởng khoa có chức vụ kiêm nhiệm là trưởng bộ môn cùng khoa
                            //if (staff.StaffInfo.SubPositions != null && staff.StaffInfo.SubPositions.Count > 0)
                            //{
                            //    foreach (var subp in staff.StaffInfo.SubPositions)
                            //    {
                            //        if (subp.Position.AgentObjectType.Id == 6 || subp.Position.AgentObjectType.Id == 12)
                            //        {
                            //            subjects = subjects.Where(s => s.Id != subp.Department.Id).ToList();
                            //        }
                            //        foreach (Staff s in professorListHasSubject.Where(a => a.StaffInfo != null && a.StaffInfo.Subject != null && a.StaffInfo.Subject.Id == subp.Department.Id))
                            //        {
                            //            if (s.Id != staffId)
                            //            {
                            //                StaffDTO sd = new StaffDTO();
                            //                sd.Name = s.StaffProfile.Name;
                            //                sd.Subject.Name = s.StaffInfo.Subject.Name;
                            //                sd.Id = s.Id;
                            //                sd.DepartmentId = s.Department.Id;
                            //                sd.AgentObjectIds = new List<Guid>();

                            //                foreach (AgentObject a in s.StaffInfo.AgentObjects)
                            //                {
                            //                    if (a.AgentObjectType.Id == 1)
                            //                    {
                            //                        sd.AgentObjectIds.Add(a.Id);
                            //                    }
                            //                }
                            //                if (sd.AgentObjectIds.Count > 0)
                            //                {
                            //                    sd.AgentObjectId = sd.AgentObjectIds.FirstOrDefault();
                            //                }
                            //                else {
                            //                    //gán AgentObject giảng viên
                            //                    sd.AgentObjectId = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == 1).Id;
                            //                }
                            //                sd.IsApproved = false;
                            //                sd.IsStaffRated = false;
                            //                sd.IsSupervisorRated = false;
                            //                PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null && p.AgentObjectType != null && p.AgentObjectType.Id == 1).SingleOrDefault();
                            //                if (planStaff != null)
                            //                {
                            //                    sd.PlanStaffId = planStaff.Id; //dùng trong chức năng duyệt chế độ làm việc của giảng viên
                            //                    sd.WorkingModeName = (planStaff.AgentObjectDetail != null && planStaff.AgentObjectDetail.WorkingMode != null) ? planStaff.AgentObjectDetail.WorkingMode.Name : null;
                            //                    sd.IsWorkingModeLocked = planStaff.IsWorkingModeLocked;
                            //                    //Kế hoạch được trưởng phòng duyệt
                            //                    sd.IsApproved = planStaff.IsLocked;
                            //                    Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                            //                    if (rs != null)
                            //                    {
                            //                        //Trưởng phòng đánh giá
                            //                        sd.IsSupervisorRated = rs.IsLocked ? true : false;
                            //                        //Nhân viên đã đánh giá
                            //                        sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                            //                    }
                            //                }
                            //                data.Add(sd);
                            //            }
                            //        }
                            //    }
                            //}
                            //result["professorListHasSubject_SubPositionSubject"] = data;

                            data = new List<StaffDTO>();

                            foreach (var subject in subjects)
                            {
                                //danh sách giảng viên có bộ môn (trưởng bộ môn đánh giá)
                                foreach (Staff s in professorListHasSubject.Where(a => a.StaffInfo != null && a.StaffInfo.Subject != null && a.StaffInfo.Subject.Id == subject.Id))
                                {
                                    if (s.Id != staffId)
                                    {
                                        StaffDTO sd = new StaffDTO();
                                        sd.Name = s.StaffProfile.Name;
                                        sd.Subject.Name = s.StaffInfo.Subject.Name;
                                        sd.Id = s.Id;
                                        sd.DepartmentId = s.Department.Id;
                                        sd.AgentObjectIds = new List<Guid>();
                                        
                                        foreach (AgentObject a in s.StaffInfo.AgentObjects)
                                        {
                                            if (a.AgentObjectType.Id == 1)
                                            {
                                                sd.AgentObjectIds.Add(a.Id);
                                            }
                                        }
                                        if (sd.AgentObjectIds.Count > 0)
                                        {
                                            sd.AgentObjectId = sd.AgentObjectIds.FirstOrDefault();
                                        }
                                        else {
                                            //gán AgentObject giảng viên
                                            sd.AgentObjectId = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == 1).Id;
                                        }
                                        sd.IsApproved = false;
                                        sd.IsStaffRated = false;
                                        sd.IsSupervisorRated = false;
                                        PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null && p.AgentObjectType != null && p.AgentObjectType.Id == 1).SingleOrDefault();
                                        if (planStaff != null)
                                        {
                                            //Kế hoạch được trưởng phòng duyệt
                                            sd.IsApproved = planStaff.IsLocked;
                                            Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                            if (rs != null)
                                            {
                                                //Trưởng phòng đánh giá
                                                sd.IsSupervisorRated = rs.IsLocked ? true : false;
                                                //Nhân viên đã đánh giá
                                                sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                                            }
                                        }
                                        data.Add(sd);
                                    }
                                }
                            }
                            result["professorListHasSubject"] = data;

                            data = new List<StaffDTO>();

                            //danh sách giảng viên không thuộc bộ môn nào (trưởng khoa đánh giá)
                            foreach (Staff s in professorListSubjectNull)
                            {
                                if (s.Id != staffId)
                                {
                                    StaffDTO sd = new StaffDTO();
                                    sd.Name = s.StaffProfile.Name;
                                    sd.Subject.Name = s.StaffInfo.Subject != null ? s.StaffInfo.Subject.Name : null;
                                    sd.Id = s.Id;
                                    sd.DepartmentId = s.Department.Id;
                                    sd.AgentObjectIds = new List<Guid>();
                                    //sd.StaffType = s.StaffInfo.StaffType != null ? (s.StaffInfo.StaffType.Name.Contains("giảng") ? 1 : 2) : 1;
                                    
                                    foreach (AgentObject a in s.StaffInfo.AgentObjects)
                                    {
                                        if (a.AgentObjectType.Id == 1)
                                        {
                                            sd.AgentObjectIds.Add(a.Id);
                                        }
                                    }
                                    if (sd.AgentObjectIds.Count > 0)
                                    {
                                        sd.AgentObjectId = sd.AgentObjectIds.FirstOrDefault();
                                    }
                                    else {
                                        //gán AgentObject giảng viên
                                        sd.AgentObjectId = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == 1).Id;
                                    }
                                    sd.IsApproved = false;
                                    sd.IsStaffRated = false;
                                    sd.IsSupervisorRated = false;
                                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null && p.AgentObjectType != null && p.AgentObjectType.Id == 1).SingleOrDefault();
                                    if (planStaff != null)
                                    {
                                        sd.PlanStaffId = planStaff.Id; //dùng trong chức năng duyệt chế độ làm việc của giảng viên
                                        sd.WorkingModeName = (planStaff.AgentObjectDetail != null && planStaff.AgentObjectDetail.WorkingMode != null) ? planStaff.AgentObjectDetail.WorkingMode.Name : null;
                                        sd.IsWorkingModeLocked = planStaff.IsWorkingModeLocked;
                                        //Kế hoạch được trưởng phòng duyệt
                                        sd.IsApproved = planStaff.IsLocked;
                                        Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                        if (rs != null)
                                        {
                                            //Trưởng phòng đánh giá
                                            sd.IsSupervisorRated = rs.IsLocked ? true : false;
                                            //Nhân viên đã đánh giá
                                            sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                                        }
                                    }
                                    data.Add(sd);
                                }
                            }
                            result["professorListSubjectNull"] = data;

                            data = new List<StaffDTO>();

                            //danh sách nhân viên có bộ môn
                            foreach(Staff s in staffListHasSubject)
                            {
                                if (s.Id != staffId)
                                {
                                    StaffDTO sd = new StaffDTO();
                                    sd.Name = s.StaffProfile.Name;
                                    sd.Subject.Name = s.StaffInfo.Subject.Name;
                                    sd.Id = s.Id;
                                    sd.DepartmentId = s.Department.Id;
                                    sd.AgentObjectIds = new List<Guid>();
                                    //sd.StaffType = s.StaffInfo.StaffType != null ? (s.StaffInfo.StaffType.Name.Contains("giảng") ? 1 : 2) : 1;

                                    //gán AgentObject nhân viên
                                    AgentObject ao = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == 2);
                                    sd.AgentObjectIds.Add(ao.Id);
                                    
                                    if (sd.AgentObjectIds.Count > 0)
                                    {
                                        sd.AgentObjectId = sd.AgentObjectIds.FirstOrDefault();
                                    }
                                    sd.IsApproved = false;
                                    sd.IsStaffRated = false;
                                    sd.IsSupervisorRated = false;
                                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null && p.AgentObjectType != null && p.AgentObjectType.Id == 2).SingleOrDefault();
                                    if (planStaff != null)
                                    {
                                        //Kế hoạch được trưởng phòng duyệt
                                        sd.IsApproved = planStaff.IsLocked;
                                        Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                        if (rs != null)
                                        {
                                            //Trưởng phòng đánh giá
                                            sd.IsSupervisorRated = rs.IsLocked ? true : false;
                                            //Nhân viên đã đánh giá
                                            sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                                        }
                                    }
                                    data.Add(sd);
                                }
                            }
                            result["staffListHasSubject"] = data;

                            data = new List<StaffDTO>();

                            //danh sách nhân viên không bộ môn
                            foreach (Staff s in staffListSubjectNull)
                            {
                                if (s.Id != staffId)
                                {
                                    StaffDTO sd = new StaffDTO();
                                    sd.Name = s.StaffProfile.Name;
                                    sd.Subject.Name = s.StaffInfo.Subject != null ? s.StaffInfo.Subject.Name : null;
                                    sd.Id = s.Id;
                                    sd.DepartmentId = s.Department.Id;
                                    sd.AgentObjectIds = new List<Guid>();
                                    //sd.StaffType = s.StaffInfo.StaffType != null ? (s.StaffInfo.StaffType.Name.Contains("giảng") ? 1 : 2) : 1;

                                    //gán AgentObject nhân viên
                                    AgentObject ao = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == 2);
                                    sd.AgentObjectIds.Add(ao.Id);
                                    
                                    if (sd.AgentObjectIds.Count > 0)
                                    {
                                        sd.AgentObjectId = sd.AgentObjectIds.FirstOrDefault();
                                    }
                                    sd.IsApproved = false;
                                    sd.IsStaffRated = false;
                                    sd.IsSupervisorRated = false;
                                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department == null && p.AgentObjectType != null && p.AgentObjectType.Id == 2).SingleOrDefault();
                                    if (planStaff != null)
                                    {
                                        //Kế hoạch được trưởng phòng duyệt
                                        sd.IsApproved = planStaff.IsLocked;
                                        Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                        if (rs != null)
                                        {
                                            //Trưởng phòng đánh giá
                                            sd.IsSupervisorRated = rs.IsLocked ? true : false;
                                            //Nhân viên đã đánh giá
                                            sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                                        }
                                    }
                                    data.Add(sd);
                                }
                            }
                            result["staffListSubjectNull"] = data;
                        }
                        break;
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public Dictionary<string, object> GetStaffInSubPosition(int agentObjectTypeId, Guid planId)
        {
            var data = new List<StaffDTO>();
            var result = new Dictionary<string, object>();
            SessionManager.DoWork(session =>
            {
                Staff staff = ControllerHelpers.GetCurrentStaff(session);
                Guid staffId = staff.Id;
                PlanKPI plan = session.Query<PlanKPI>().Where(p => p.Id == planId).SingleOrDefault();
                int planType = plan.PlanType.Id;

                //thấy người trong bộ phận họ kiêm nhiệm
                if (staff.StaffInfo.SubPositions != null)
                {
                    foreach (var subp in staff.StaffInfo.SubPositions)
                    {
                        data = new List<StaffDTO>();

                        IQueryable<Staff> staffListInSubPosition = null;
                        switch (subp.Position.AgentObjectType.Id)
                        {
                            case (int)AgentObjectTypes.PhongBan:
                                {
                                    #region Trưởng phòng
                                    staffListInSubPosition = session.Query<Staff>().Where(q => q.StaffProfile.GCRecord == null && q.StaffStatus.NoLongerWork == 0 && q.Department.Id == subp.Department.Id).OrderByDescending(q => q.StaffInfo.Position.HSPCChucVu).AsQueryable();
                                    staffListInSubPosition = staffListInSubPosition.Where(q => q.StaffInfo.Position.AgentObjectType.Id == 7 || q.StaffInfo.StaffType.ManageCode == "3").AsQueryable();
                                    //chỉ lấy phó phòng và nhân viên
                                    foreach (var s in staffListInSubPosition)
                                    {
                                        if (s.Id != staffId)
                                        {
                                            StaffDTO sd = new StaffDTO();
                                            sd.Name = s.StaffProfile.Name;
                                            sd.Id = s.Id;
                                            sd.DepartmentId = s.Department.Id;
                                            sd.AgentObjectIds = new List<Guid>();

                                            sd.IsApproved = false;
                                            sd.IsStaffRated = false;
                                            sd.IsSupervisorRated = false;
                                            PlanStaff planStaff = null;
                                            if (s.StaffInfo.Position != null && s.StaffInfo.Position.AgentObjectType != null)
                                            {
                                                planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department.Id == s.Department.Id && p.AgentObjectType != null && p.AgentObjectType.Id == s.StaffInfo.Position.AgentObjectType.Id).SingleOrDefault();
                                            }
                                            else if (s.StaffInfo.Position == null)
                                            {
                                                planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department.Id == null && p.AgentObjectType != null && p.AgentObjectType.Id == 2).SingleOrDefault();
                                            }
                                            if (planStaff != null)
                                            {
                                                //Kế hoạch được trưởng phòng duyệt
                                                sd.IsApproved = planStaff.IsLocked;
                                                Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                                if (rs != null)
                                                {
                                                    //Trưởng phòng đánh giá
                                                    sd.IsSupervisorRated = rs.IsLocked ? true : false;
                                                    //Nhân viên đã đánh giá
                                                    sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                                                }
                                            }
                                            sd.PositionName = s.StaffInfo.Position != null ? s.StaffInfo.Position.Name : "";

                                            AgentObject ao = null;
                                            if (s.StaffInfo.Position != null && s.StaffInfo.Position.AgentObjectType != null)
                                            {
                                                ao = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == s.StaffInfo.Position.AgentObjectType.Id && a.AgentObjectType.PlanTypes.Any(pt => pt.Id == planType));
                                                sd.AgentObjectTypeId = s.StaffInfo.Position.AgentObjectType.Id;
                                            }
                                            else if (s.StaffInfo.Position == null)
                                            {
                                                ao = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == 2 && a.AgentObjectType.PlanTypes.Any(pt => pt.Id == planType));
                                                sd.AgentObjectTypeId = 2;
                                            }
                                            if (ao != null)
                                            {
                                                sd.AgentObjectIds.Add(ao.Id);
                                                sd.AgentObjectId = ao.Id;
                                                data.Add(sd);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                break;
                            case (int)AgentObjectTypes.Khoa:
                                {
                                    #region Trưởng khoa
                                    staffListInSubPosition = session.Query<Staff>().Where(q => q.StaffProfile.GCRecord == null && q.StaffStatus.NoLongerWork == 0 && q.StaffInfo.Position != null && q.Department.Id == subp.Department.Id).OrderByDescending(q => q.StaffInfo.Position.HSPCChucVu).AsQueryable();
                                    staffListInSubPosition = staffListInSubPosition.Where(q => q.StaffInfo.Position.AgentObjectType.Id == 8 || q.StaffInfo.Position.AgentObjectType.Id == 6).AsQueryable();
                                    foreach (var s in staffListInSubPosition)
                                    {
                                        if (s.Id != staffId)
                                        {
                                            StaffDTO sd = new StaffDTO();
                                            sd.Name = s.StaffProfile.Name;
                                            sd.Id = s.Id;
                                            sd.DepartmentId = s.Department.Id;
                                            sd.AgentObjectIds = new List<Guid>();

                                            sd.IsApproved = false;
                                            sd.IsStaffRated = false;
                                            sd.IsSupervisorRated = false;
                                            PlanStaff planStaff = null;
                                            if (s.StaffInfo.Position.AgentObjectType != null)
                                            {
                                                planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department.Id == s.Department.Id && p.AgentObjectType != null && p.AgentObjectType.Id == s.StaffInfo.Position.AgentObjectType.Id).SingleOrDefault();
                                            }
                                            if (planStaff != null)
                                            {
                                                //Kế hoạch được trưởng phòng duyệt
                                                sd.IsApproved = planStaff.IsLocked;
                                                Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                                if (rs != null)
                                                {
                                                    //Trưởng phòng đánh giá
                                                    sd.IsSupervisorRated = rs.IsLocked ? true : false;
                                                    //Nhân viên đã đánh giá
                                                    sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                                                }
                                            }
                                            sd.PositionName = s.StaffInfo.Position != null ? s.StaffInfo.Position.Name : "";

                                            AgentObject ao = null;
                                            if (s.StaffInfo.Position.AgentObjectType != null)
                                            {
                                                ao = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == s.StaffInfo.Position.AgentObjectType.Id && a.AgentObjectType.PlanTypes.Any(pt => pt.Id == planType));
                                                sd.AgentObjectTypeId = s.StaffInfo.Position.AgentObjectType.Id;
                                            }
                                            if (ao != null)
                                            {
                                                sd.AgentObjectIds.Add(ao.Id);
                                                sd.AgentObjectId = ao.Id;
                                                data.Add(sd);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                break;
                            case (int)AgentObjectTypes.BoMon:
                            case (int)AgentObjectTypes.Nganh:
                                {
                                    #region Bộ môn, ngành
                                    staffListInSubPosition = session.Query<Staff>().Where(q => q.StaffProfile.GCRecord == null && q.StaffStatus.NoLongerWork == 0 && q.Department.Id == subp.Department.ParentDepartment.Id && q.StaffInfo.Subject.Id == subp.Department.Id).AsQueryable();
                                    staffListInSubPosition = staffListInSubPosition.Where(q => q.StaffInfo.StaffType.ManageCode != "3").AsQueryable();
                                    foreach (var s in staffListInSubPosition)
                                    {
                                        if (s.Id != staffId)
                                        {
                                            StaffDTO sd = new StaffDTO();
                                            sd.Name = s.StaffProfile.Name;
                                            sd.Id = s.Id;
                                            sd.DepartmentId = s.Department.Id;
                                            sd.AgentObjectIds = new List<Guid>();

                                            sd.IsApproved = false;
                                            sd.IsStaffRated = false;
                                            sd.IsSupervisorRated = false;
                                            PlanStaff planStaff = null;
                                            planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department.Id == null && p.AgentObjectType != null && p.AgentObjectType.Id == 1).SingleOrDefault();
                                            if (planStaff != null)
                                            {
                                                sd.PlanStaffId = planStaff.Id; //dùng trong chức năng duyệt chế độ làm việc của giảng viên
                                                sd.WorkingModeName = (planStaff.AgentObjectDetail != null && planStaff.AgentObjectDetail.WorkingMode != null) ? planStaff.AgentObjectDetail.WorkingMode.Name : null;
                                                sd.IsWorkingModeLocked = planStaff.IsWorkingModeLocked;
                                                //Kế hoạch được trưởng phòng duyệt
                                                sd.IsApproved = planStaff.IsLocked;
                                                Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                                if (rs != null)
                                                {
                                                    //Trưởng phòng đánh giá
                                                    sd.IsSupervisorRated = rs.IsLocked ? true : false;
                                                    //Nhân viên đã đánh giá
                                                    sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                                                }
                                            }
                                            //sd.PositionName = s.StaffInfo.Position != null ? s.StaffInfo.Position.Name : "";
                                            
                                            AgentObject ao = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == 1 && a.AgentObjectType.PlanTypes.Any(pt => pt.Id == planType));
                                            sd.AgentObjectTypeId = 1;
                                            if (ao != null)
                                            {
                                                sd.AgentObjectIds.Add(ao.Id);
                                                sd.AgentObjectId = ao.Id;
                                                data.Add(sd);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                break;
                        }
                        if (data.Count > 0)
                            result.Add(subp.Department.Name, data);
                    }
                }
            });
            return result;
        }

        public static StaffDTO ParseStaffDTOForViewPlanDetail(Staff s, Guid planId, NHibernate.ISession session)
        {
            StaffDTO sd = new StaffDTO();
            sd.Name = s.StaffProfile.Name;
            sd.Id = s.Id;
            sd.DepartmentId = s.Department.Id;
            sd.AgentObjectIds = new List<Guid>();

            sd.IsApproved = false;
            sd.IsStaffRated = false;
            sd.IsSupervisorRated = false;
            PlanStaff planStaff = null;
            if (s.StaffInfo.Position != null && s.StaffInfo.Position.AgentObjectType != null)
            {
                planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department.Id == s.Department.Id && p.AgentObjectType.Id == s.StaffInfo.Position.AgentObjectType.Id).SingleOrDefault();
            }
            else if (s.StaffInfo.Position == null)
            {
                planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department.Id == null).SingleOrDefault();
            }
            if (planStaff != null)
            {
                sd.PlanStaffId = planStaff.Id; //dùng trong chức năng duyệt chế độ làm việc của giảng viên
                sd.WorkingModeName = (planStaff.AgentObjectDetail != null && planStaff.AgentObjectDetail.WorkingMode != null) ? planStaff.AgentObjectDetail.WorkingMode.Name : null;
                sd.IsWorkingModeLocked = planStaff.IsWorkingModeLocked;
                //Kế hoạch được trưởng phòng duyệt
                sd.IsApproved = planStaff.IsLocked;
                Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                if (rs != null)
                {
                    //Trưởng phòng đánh giá
                    sd.IsSupervisorRated = rs.IsLocked ? true : false;
                    //Nhân viên đã đánh giá
                    sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                }
            }
            sd.PositionName = s.StaffInfo.Position != null ? s.StaffInfo.Position.Name : "";

            AgentObject ao = null;
            if (s.StaffInfo.Position != null && s.StaffInfo.Position.AgentObjectType != null)
            {
                ao = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == s.StaffInfo.Position.AgentObjectType.Id);
            }
            else if (s.StaffInfo.Position == null)
            {
                ao = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == 2);
            }
            if (ao != null)
            {
                sd.AgentObjectIds.Add(ao.Id);
                sd.AgentObjectId = ao.Id;
            }
            return sd;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<StaffDTO> GetSubjectStaff(int agentObjectTypeId, Guid planId)
        {
            var result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                Staff staff = ControllerHelpers.GetCurrentStaff(session);
                Guid staffId = staff.Id;

                List<Staff> stafflist = session.Query<Staff>().Where(s => s.Department.Id == staff.Department.Id && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && (s.StaffInfo.Position.AgentObjectType.Id == 6 || s.StaffInfo.Position.AgentObjectType.Id == 12)).ToList();
                
                foreach (Staff s in stafflist)
                {
                    if (s.Id != staffId)
                    {
                        StaffDTO sd = new StaffDTO();
                        sd.Name = s.StaffProfile.Name;
                        sd.Id = s.Id;
                        //sd.DepartmentId = s.Department.Id;
                        sd.DepartmentId = s.StaffInfo.Subject.Id; //id của bộ môn
                        sd.AgentObjectIds = new List<Guid>();
                        //sd.StaffType = s.StaffInfo.StaffType != null ? (s.StaffInfo.StaffType.Name.Contains("giảng") ? 1 : 2) : 1;

                        //gán AgentObject nhân viên
                        AgentObject ao = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == 6);
                        sd.AgentObjectIds.Add(ao.Id);

                        //foreach (AgentObject a in s.StaffInfo.AgentObjects)
                        //{
                        //    if (a.AgentObjectType.Id == 1)
                        //    {
                        //        sd.AgentObjectIds.Add(a.Id);
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
                        if (planStaff != null)
                        {
                            //Kế hoạch được trưởng phòng duyệt
                            sd.IsApproved = planStaff.IsLocked;
                            Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                            if (rs != null)
                            {
                                //Trưởng phòng đánh giá
                                sd.IsSupervisorRated = rs.IsLocked ? true : false;
                                //Nhân viên đã đánh giá
                                sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                            }
                        }
                        result.Add(sd);
                    }
                }

                var staffList_SubPosition = session.Query<Staff>().Where(q => q.StaffStatus.NoLongerWork == 0 && q.StaffProfile.GCRecord == null && q.StaffInfo.SubPositions.Any(qq => qq.GCRecord == null && (qq.Position.AgentObjectType.Id == 6 || qq.Position.AgentObjectType.Id == 12) && (qq.Department.Id == staff.Department.Id || qq.Department.ParentDepartment.Id == staff.Department.Id))).AsQueryable();
                foreach (var s in staffList_SubPosition)
                {
                    if (s.Id != staffId)
                    {
                        foreach (var spos in s.StaffInfo.SubPositions)
                        {
                            StaffDTO sd = new StaffDTO();
                            sd.Name = s.StaffProfile.Name;
                            sd.Id = s.Id;
                            sd.DepartmentId = spos.Department.Id; //id của bộ phận kiêm nhiệm
                            sd.AgentObjectIds = new List<Guid>();
                            sd.PositionName = spos.Position.Name;
                            AgentObject ao = spos.Position.AgentObjectType != null ? session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == spos.Position.AgentObjectType.Id) : null;
                            if (ao != null)
                            {
                                sd.AgentObjectIds.Add(ao.Id);
                                sd.AgentObjectId = ao.Id;
                            }

                            sd.IsApproved = false;
                            sd.IsStaffRated = false;
                            sd.IsSupervisorRated = false;
                            PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == s.Id && p.Department.Id == spos.Department.Id && p.AgentObjectType.Id == spos.Position.AgentObjectType.Id).AsQueryable().SingleOrDefault();
                            if (planStaff != null)
                            {
                                //Kế hoạch được trưởng phòng duyệt
                                sd.IsApproved = planStaff.IsLocked;
                                Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                                if (rs != null)
                                {
                                    //Trưởng phòng đánh giá
                                    sd.IsSupervisorRated = rs.IsLocked ? true : false;
                                    //Nhân viên đã đánh giá
                                    sd.IsStaffRated = rs.TempRecord > 0 || sd.IsSupervisorRated == true ? true : false;

                                }
                            }
                            result.Add(sd);
                        }
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
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

                result.Department = new DepartmentDTO();
                if (staff.Department != null)
                {
                    result.Department.Id = staff.Department.Id;
                    result.Department.DepartmentType = staff.Department.DepartmentType;
                    result.Department.Name = staff.Department.Name;
                    result.Department.ParentId = staff.Department.ParentDepartment != null ? staff.Department.ParentDepartment.Id : Guid.Empty;
                }
                //result.Department = staff.Department != null ? staff.Department.Map<DepartmentDTO>() : null;

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

        [Authorize]
        [Route("")]
        public Guid GetCurrentUserGroupId()
        {
            Guid groupId = Guid.Empty;
            SessionManager.DoWork(session =>
            {
                groupId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId);
            });
            return groupId;
        }

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
        public IEnumerable<StaffDTO> GetListbyId(Guid departmentId)
        {
            var result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                result = departmentId == Guid.Empty ? session.Query<Staff>().Where(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null).ToList().Map<StaffDTO>() : session.Query<Staff>().Where(a => a.Department.Id == departmentId && a.StaffStatus.NoLongerWork == 0 && a.StaffProfile.GCRecord == null).Map<StaffDTO>();
                foreach (StaffDTO st in result)
                {
                    st.Name = st.StaffProfile.Name;
                    st.Checked = false;
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

        [Authorize]
        [Route("")]
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
                    IQueryable<Staff> resultQuery_SubPosition = null;
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
                                resultQuery = session.Query<Staff>().Where(a => a.StaffProfile.GCRecord == null && a.StaffStatus.NoLongerWork == 0 && (a.StaffInfo.Position.AgentObjectType.Id == typeId || a.StaffInfo.Position.AgentObjectType.ParentAgentObjectType.Id == typeId)).AsQueryable();
                                resultQuery_SubPosition = session.Query<Staff>().Where(s => s.StaffProfile.GCRecord == null && s.StaffStatus.NoLongerWork == 0 && s.StaffInfo.SubPositions.Any(ss => ss.Department.Id == departmentId && ss.GCRecord == null && (ss.Position.AgentObjectType.Id == typeId || ss.Position.AgentObjectType.ParentAgentObjectType.Id == typeId))).AsQueryable();
                            }
                            break;
                    }

                    if (departmentId != Guid.Empty)
                    {
                        List<Staff> staffResult = resultQuery.Where(s => s.Department.Id == departmentId).ToList();
                        if (resultQuery_SubPosition != null)
                            staffResult.AddRange(resultQuery_SubPosition);

                        staffResult = staffResult.OrderByDescending(q => (q.StaffInfo != null && q.StaffInfo.Position != null) ? q.StaffInfo.Position.HSPCChucVu : 0).ToList();
                        foreach (Staff st in staffResult)
                        {
                            StaffDTO staffDTO = st.Map<StaffDTO>();
                            staffDTO.Position = st.StaffInfo.Position != null ? st.StaffInfo.Position.Map<PositionDTO>() : null;
                            result.Add(staffDTO);
                        }
                    }
                    else
                        result = resultQuery.ToList().Map<StaffDTO>();
                    foreach (StaffDTO st in result)
                    {
                        st.Name = st.StaffProfile.Name;

                    }
                }
                if (typeId == 99 || typeId == 100)
                {
                    Guid currentStaffId = ControllerHelpers.GetCurrentStaff(session).Id;
                    result = result.Where(a => a.Id != currentStaffId).ToList();
                }
            });

            return result;
        }

        [Authorize]
        [Route("")]
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

                if (departmentId != Guid.Empty)
                {
                    List<Staff> listtemp = resultQuery.Where(s => s.Department.Id == departmentId).ToList();
                    foreach (Staff st in listtemp)
                    {
                        StaffDTO std = ParseFullStaff(st);
                        result.Add(std);
                    }
                    foreach (StaffDTO s in result)
                    {
                        AgentObject a = session.Query<AgentObject>().Where(ag => ag.AgentObjectType.Id == selectedTypeId).FirstOrDefault();
                        s.AgentObjectId = a != null ? a.Id : Guid.Empty;
                    }
                }
                else
                {
                    List<Staff> listtemp = resultQuery.ToList();
                    foreach (Staff st in listtemp)
                    {
                        StaffDTO std = ParseFullStaff(st);
                        result.Add(std);
                    }
                }

                foreach (StaffDTO st in result)
                {
                    st.IsApproved = false;
                    st.IsStaffRated = false;
                    st.IsSupervisorRated = false;
                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff.Id == st.Id && p.AgentObjectType.Id == selectedTypeId).FirstOrDefault();
                    if (planStaff != null)
                    {
                        //Kế hoạch được trưởng phòng duyệt
                        st.IsApproved = planStaff.IsLocked;
                        Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                        if (rs != null)
                        {
                            //Trưởng phòng đánh giá
                            st.IsSupervisorRated = rs.IsLocked ? true : false;
                            //Nhân viên đã đánh giá
                            st.IsStaffRated = rs.TempRecord > 0 || st.IsSupervisorRated == true ? true : false;

                        }
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public StaffDTO GetDepartmentLeader(Guid departmentId)
        {
            var result = new StaffDTO();
            SessionManager.DoWork(session =>
            {
                Staff st = ControllerHelpers.GetCurrentStaff(session);
                //Nếu ở bộ môn và ko có chức vụ => giảng viên
                if (st.StaffInfo.Subject != null && st.StaffInfo.Position == null)
                {
                    //Lấy trưởng bộ môn
                    Staff staff = session.Query<Staff>().FirstOrDefault(s => s.StaffInfo.Subject!=null && s.StaffInfo.Subject.Id==st.StaffInfo.Subject.Id && s.StaffInfo.Position!=null && s.StaffInfo.Position.AgentObjectType.Id == 6);
                    if (staff != null)
                        result = ParseStaff(staff);
                }
                else
                {
                    if (departmentId != Guid.Empty)
                    {
                        Staff staff = session.Query<Staff>().FirstOrDefault(s => s.Department.Id == departmentId && (s.StaffInfo.Position.AgentObjectType.Id == 3 || s.StaffInfo.Position.AgentObjectType.Id == 5));
                        if (staff != null)
                            result = ParseStaff(staff);
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// Lấy cấp trên trực tiếp
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="agentObjectId"></param>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        public StaffDTO GetDirectLeader(Guid departmentId, Guid agentObjectId, Guid normalStaffId)
        {
            var result = new StaffDTO();
            SessionManager.DoWork(session =>
            {
                Staff st = null;
                if (normalStaffId != Guid.Empty)
                {
                    st = session.Query<Staff>().SingleOrDefault(q => q.Id == normalStaffId);
                }
                if (st == null)
                {
                    st = ControllerHelpers.GetCurrentStaff(session);
                }
                int agentObjectTypeId = session.Query<AgentObject>().SingleOrDefault(a => a.Id == agentObjectId).AgentObjectType.Id;
                Staff staff = null;

                switch (agentObjectTypeId)
                {
                    case 1: //giảng viên
                        {
                            if (st.StaffInfo.Subject != null)
                            {
                                //Lấy trưởng bộ môn (trưởng ngành)
                                staff = session.Query<Staff>().FirstOrDefault(s => s.StaffStatus != null && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile != null && s.StaffProfile.GCRecord == null && s.StaffInfo != null && s.StaffInfo.Subject != null && s.StaffInfo.Subject.Id == st.StaffInfo.Subject.Id && s.StaffInfo.Position != null && s.StaffInfo.Position.AgentObjectType != null && (s.StaffInfo.Position.AgentObjectType.Id == 6 || s.StaffInfo.Position.AgentObjectType.Id == 12));
                            }

                            if (staff == null)
                            {
                                if (departmentId != Guid.Empty)
                                {
                                    //Lấy trưởng phòng, trưởng khoa
                                    staff = session.Query<Staff>().FirstOrDefault(s => s.StaffStatus != null && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile != null && s.StaffProfile.GCRecord == null && s.Department != null && s.Department.Id == departmentId && s.StaffInfo.Position != null && s.StaffInfo.Position.AgentObjectType != null && (s.StaffInfo.Position.AgentObjectType.Id == 3 || s.StaffInfo.Position.AgentObjectType.Id == 5));
                                }
                            }

                            if (staff != null)
                                result = ParseStaff(staff);
                            break;
                        }
                    case 2: //nhân viên
                        {
                            if (st.StaffInfo.Subject != null)
                            {
                                //Lấy trưởng bộ môn (trưởng ngành)
                                staff = session.Query<Staff>().FirstOrDefault(s => s.StaffStatus != null && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile != null && s.StaffProfile.GCRecord == null && s.StaffInfo != null && s.StaffInfo.Subject != null && s.StaffInfo.Subject.Id == st.StaffInfo.Subject.Id && s.StaffInfo.Position != null && s.StaffInfo.Position.AgentObjectType != null && (s.StaffInfo.Position.AgentObjectType.Id == 6 || s.StaffInfo.Position.AgentObjectType.Id == 12));
                            }

                            if (staff == null)
                            {
                                if (departmentId != Guid.Empty)
                                {
                                    //Lấy trưởng phòng, trưởng khoa
                                    staff = session.Query<Staff>().FirstOrDefault(s => s.StaffStatus != null && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile != null && s.StaffProfile.GCRecord == null && s.Department != null && s.Department.Id == departmentId && s.StaffInfo.Position != null && s.StaffInfo.Position.AgentObjectType != null && (s.StaffInfo.Position.AgentObjectType.Id == 3 || s.StaffInfo.Position.AgentObjectType.Id == 5));
                                }
                            }

                            if (staff != null)
                                result = ParseStaff(staff);
                            break;
                        }
                    default: //trưởng bộ môn, phó khoa, phó phòng
                        {
                            if (departmentId != Guid.Empty)
                            {
                                //Lấy trưởng phòng, trưởng khoa
                                staff = session.Query<Staff>().FirstOrDefault(s => s.StaffStatus != null && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile != null && s.StaffProfile.GCRecord == null && s.Department != null && s.Department.Id == departmentId && s.StaffInfo.Position != null && s.StaffInfo.Position.AgentObjectType != null && (s.StaffInfo.Position.AgentObjectType.Id == 3 || s.StaffInfo.Position.AgentObjectType.Id == 5));
                                if (staff == null)
                                {
                                    Department Department = session.Query<Department>().SingleOrDefault(d => d.Id == departmentId);
                                    if (Department != null && Department.ParentDepartment != null)
                                        staff = session.Query<Staff>().FirstOrDefault(s => s.StaffStatus != null && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile != null && s.StaffProfile.GCRecord == null && s.Department != null && s.Department.Id == Department.ParentDepartment.Id && s.StaffInfo.Position != null && s.StaffInfo.Position.AgentObjectType != null && (s.StaffInfo.Position.AgentObjectType.Id == 3 || s.StaffInfo.Position.AgentObjectType.Id == 5));
                                }
                                if (staff != null)
                                    result = ParseStaff(staff);
                            }
                            break;
                        }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public static Staff GetStaffDepartmentLeader(Guid departmentId)
        {
            var result = new Staff();
            SessionManager.DoWork(session =>
            {
                if (departmentId != Guid.Empty)
                {
                    result = session.Query<Staff>().FirstOrDefault(s => s.Department != null && s.Department.Id == departmentId && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && (s.StaffInfo.Position.AgentObjectType.Id == 3 || s.StaffInfo.Position.AgentObjectType.Id == 5));
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<StaffDTO> GetProfessorInSubject(int typeId)
        {
            var result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                if (typeId == 6 || typeId == 12)
                {
                    Staff staff = ControllerHelpers.GetCurrentStaff(session);
                    Guid subjectId = staff.StaffInfo.Subject.Id;
                    Guid departmentId = staff.Department.Id;
                    var staffDepartmentList = session.Query<Staff>().Where(a => a.Department.Id == departmentId && a.StaffStatus.NoLongerWork == 0 && a.StaffProfile.GCRecord == null).ToList();
                    result = staffDepartmentList.Where(a => a.StaffInfo.Subject != null && a.StaffInfo.Subject.Id == subjectId).ToList().Map<StaffDTO>();
                    foreach (StaffDTO st in result)
                    {
                        st.Name = st.StaffProfile.Name;
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<StaffDTO> GetStaffInFaculty(int typeId, Guid departmentId, Guid planId) //lấy danh sách người thực hiện của khoa (gồm phó khoa, thư ký, nhân viên kỹ thuật)
        {
            var result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                if (typeId == (int)AgentObjectTypes.Khoa)
                {
                    Staff staff = ControllerHelpers.GetCurrentStaff(session);
                    if (departmentId == Guid.Empty)
                        departmentId = staff.Department != null ? staff.Department.Id : Guid.Empty;

                    var selectedTypeId = (int)AgentObjectTypes.PhoKhoa;
                    var staffInFaculty = session.Query<Staff>().Where(s => s.Department != null && s.Department.Id == departmentId && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null);
                    var subFaculty = staffInFaculty.Where(s => (s.StaffInfo.Position.AgentObjectType.Id == selectedTypeId || s.StaffInfo.SubPositions.Any(p => p.Position.AgentObjectType.Id == selectedTypeId)));
                    var listTemp1 = staffInFaculty.Where(a => a.Jobs != null && a.Jobs.ManageCode == "TK_HC" && !subFaculty.Any(s => s.Id == a.Id)).ToList();
                    var listTemp2 = staffInFaculty.Where(a => a.Jobs != null && a.Jobs.ManageCode == "44" && !subFaculty.Any(s => s.Id == a.Id)).ToList();
                    foreach (Staff st in subFaculty)
                    {
                        StaffDTO std = ParseFullStaff(st);
                        std.Name = std.Name + " - Phó đơn vị";
                        result.Add(std);
                    }
                    foreach (Staff st in listTemp1)
                    {
                        StaffDTO std = ParseFullStaff(st);
                        std.Name = std.Name + " - Thư ký hành chính";
                        result.Add(std);
                    }
                    foreach (Staff st in listTemp2)
                    {
                        StaffDTO std = ParseFullStaff(st);
                        std.Name = std.Name + " - Nhân viên kỹ thuật";
                        result.Add(std);
                    }
                    foreach (StaffDTO s in result)
                    {
                        AgentObject a = session.Query<AgentObject>().Where(ag => ag.AgentObjectType.Id == selectedTypeId).FirstOrDefault();
                        s.AgentObjectId = a != null ? a.Id : Guid.Empty;
                    }
                }
                else if (typeId == (int)AgentObjectTypes.PhoKhoa)
                {
                    Staff staff = ControllerHelpers.GetCurrentStaff(session);
                    if (departmentId == Guid.Empty)
                        departmentId = staff.Department != null ? staff.Department.Id : Guid.Empty;

                    var selectedTypeId = (int)AgentObjectTypes.PhoKhoa;
                    var staffInFaculty = session.Query<Staff>().Where(s => s.Department != null && s.Department.Id == departmentId && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null);
                    var listTemp1 = staffInFaculty.Where(a => a.Jobs != null && a.Jobs.ManageCode == "TK_HC").ToList();
                    var listTemp2 = staffInFaculty.Where(a => a.Jobs != null && a.Jobs.ManageCode == "44").ToList();
                    
                    foreach (Staff st in listTemp1)
                    {
                        StaffDTO std = ParseFullStaff(st);
                        std.Name = std.Name + " - Thư ký hành chính";
                        result.Add(std);
                    }
                    foreach (Staff st in listTemp2)
                    {
                        StaffDTO std = ParseFullStaff(st);
                        std.Name = std.Name + " - Nhân viên kỹ thuật";
                        result.Add(std);
                    }
                    foreach (StaffDTO s in result)
                    {
                        AgentObject a = session.Query<AgentObject>().Where(ag => ag.AgentObjectType.Id == selectedTypeId).FirstOrDefault();
                        s.AgentObjectId = a != null ? a.Id : Guid.Empty;
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<StaffDTO> GetListDepartmentLeader(Guid planId)
        {
            var result = new List<StaffDTO>();
            SessionManager.DoWork(session =>
            {
                var staff = ControllerHelpers.GetCurrentStaff(session);
                var staffList = session.Query<Staff>().Where(q => q.StaffProfile.GCRecord == null && q.StaffStatus.NoLongerWork == 0 && (q.StaffInfo.Position.AgentObjectType.Id == 3 || q.StaffInfo.Position.AgentObjectType.Id == 5));
                foreach (Staff st in staffList)
                {
                    if (staff.Departments.Any(q => q.Id == st.Department.Id))
                    {
                        StaffDTO stdto = ParseFullStaff(st);
                        stdto.AgentObjectTypeId = st.StaffInfo.Position.AgentObjectType.Id;
                        stdto.AgentObjectId = st.StaffInfo.Position.AgentObjectType.AgentObjects.First().Id;
                        result.Add(stdto);
                    }
                }
                var staffListSubPosition = session.Query<Staff>().Where(q => q.StaffProfile.GCRecord == null && q.StaffStatus.NoLongerWork == 0 && q.StaffInfo.SubPositions.Any(qq => qq.GCRecord == null && (qq.Position.AgentObjectType.Id == 3 || qq.Position.AgentObjectType.Id == 5)));
                foreach (Staff st in staffListSubPosition)
                {
                    foreach (var subPosition in st.StaffInfo.SubPositions)
                    {
                        if (staff.Departments.Any(q => q.Id == subPosition.Department.Id))
                        {
                            if (subPosition.Position.AgentObjectType != null && (subPosition.Position.AgentObjectType.Id == 3 || subPosition.Position.AgentObjectType.Id == 5))
                            {
                                StaffDTO stdto = ParseFullStaff(st);
                                stdto.Department.Id = subPosition.Department != null ? subPosition.Department.Id : Guid.Empty;
                                stdto.Department.Name = subPosition.Department != null ? subPosition.Department.Name : "";
                                stdto.DepartmentId = subPosition.Department != null ? subPosition.Department.Id : Guid.Empty;
                                stdto.DepartmentName = subPosition.Department != null ? subPosition.Department.Name : "";
                                stdto.AgentObjectTypeId = subPosition.Position.AgentObjectType.Id;
                                stdto.AgentObjectId = subPosition.Position.AgentObjectType.AgentObjects.First().Id;
                                result.Add(stdto);
                            }
                        }
                    }
                }
                foreach (var st in result)
                {
                    st.IsApproved = false;
                    st.IsStaffRated = false;
                    st.IsSupervisorRated = false;
                    PlanStaff planStaff = session.Query<PlanStaff>().Where(p => p.PlanKPI.Id == planId && p.Staff == null && p.Department.Id == st.DepartmentId && p.AgentObjectType.Id == st.AgentObjectTypeId).SingleOrDefault();
                    if (planStaff != null)
                    {
                        //Kế hoạch được trưởng phòng duyệt
                        st.IsApproved = planStaff.IsLocked;
                        Result rs = session.Query<Result>().Where(r => r.PlanStaff.Id == planStaff.Id).SingleOrDefault();
                        if (rs != null)
                        {
                            //Trưởng phòng đánh giá
                            st.IsSupervisorRated = rs.IsLocked ? true : false;
                            //Nhân viên đã đánh giá
                            st.IsStaffRated = rs.TempRecord > 0 || st.IsSupervisorRated == true ? true : false;

                        }
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int GetCheckIsSupervisor()
        {
            int result = 0;
            SessionManager.DoWork(session =>
            {
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                //BGH
                if (applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000003")
                {
                    result = 2;
                }
                else
                //Admin đơn vị ủy quyền
                if (applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000005")
                {
                    result = 3;
                }
                else
                //Tài khoản Admin Đơn vị (hồ sơ và thông tin lương)
                if (applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000004")
                {
                    result = 1;
                }
                //Tài khoản bình thường
                else if (applicationUser.WebGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
                {
                    result = 0;
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public bool GetCheckStaffInFaculty(Guid staffId) //kiểm tra nếu có công việc là thư ký hành chính hoặc nhân viên kỹ thuật thì return true
        {
            var result = false;
            SessionManager.DoWork(session =>
            {
                try
                {
                    Staff staff = session.Query<Staff>().SingleOrDefault(s => s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null && s.Id == staffId && s.Jobs != null && (s.Jobs.ManageCode == "TK_HC" || s.Jobs.ManageCode == "44"));
                    if (staff != null)
                        result = true;
                }
                catch (Exception e)
                {

                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
        public Staff Delete(Staff obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }
    }
}
