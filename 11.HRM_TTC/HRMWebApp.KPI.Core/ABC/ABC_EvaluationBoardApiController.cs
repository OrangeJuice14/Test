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
using HRMWebApp.KPI.Core.Security;
using System.Web.Http;
using System.Web;
using Microsoft.AspNet.Identity;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using HRMWeb_Business.Predefined;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_EvaluationBoardApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_EvaluationBoardDTO> GetList() // tab đánh giá cấp dưới, Đánh giá đơn vị
        {
            List<ABC_EvaluationBoardDTO> result = new List<ABC_EvaluationBoardDTO>();
            SessionManager.DoWork(session =>
            {
                StaffDTO staff = GetCurrentUser();
                IEnumerable<ABC_EvaluationBoard> planList = new List<ABC_EvaluationBoard>();

                planList = session.Query<ABC_EvaluationBoard>();

                foreach (ABC_EvaluationBoard pl in planList)
                {
                    ABC_EvaluationBoardDTO pd = pl.Map<ABC_EvaluationBoardDTO>();
                    result.Add(pd);
                }
            });
            result = result.OrderBy(r => r.FromDate).ToList();
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_EvaluationBoardDTO> GetListStaff() //tab đánh giá bản thân
        {
            List<ABC_EvaluationBoardDTO> result = new List<ABC_EvaluationBoardDTO>();
            SessionManager.DoWork(session =>
            {
                StaffDTO staff = GetCurrentUser();
                IEnumerable<ABC_EvaluationBoard> planList = new List<ABC_EvaluationBoard>();

                planList = session.Query<ABC_EvaluationBoard>();

                foreach (ABC_EvaluationBoard pl in planList)
                {
                    ABC_EvaluationBoardDTO pd = pl.Map<ABC_EvaluationBoardDTO>();
                    ABC_Rating rating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == pl.Id && r.Staff.Id == staff.Id && r.Department.Id == staff.DepartmentId).FirstOrDefault();
                    if (rating != null && rating.IsSupervisorRated == true)
                        pd.IsSupervisorRated = "Đã đánh giá";
                    result.Add(pd);
                }
            });
            result = result.OrderBy(r => r.FromDate).ToList();
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_EvaluationBoardDTO> GetListEvaluationManage() //Quản lý đánh giá ABC
        {
            List<ABC_EvaluationBoardDTO> result = new List<ABC_EvaluationBoardDTO>();
            SessionManager.DoWork(session =>
            {
                IEnumerable<ABC_EvaluationBoard> planList = new List<ABC_EvaluationBoard>();
                planList = session.Query<ABC_EvaluationBoard>();

                foreach (ABC_EvaluationBoard pl in planList)
                {
                    ABC_EvaluationBoardDTO pd = pl.Map<ABC_EvaluationBoardDTO>();
                    result.Add(pd);
                }
            });
            result = result.OrderBy(r => r.FromDate).ToList();
            return result;
        }

        [Authorize]
        [Route("")]
        public ABC_EvaluationBoard GetObj(Guid Id)
        {
            ABC_EvaluationBoard result = new ABC_EvaluationBoard();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_EvaluationBoard>().Where(e => e.Id == Id).SingleOrDefault();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_EvaluationBoardStaffDTO> GetListStaffEvaluationBySupervisor(Guid evaluationId)
        {
            List<ABC_EvaluationBoardStaffDTO> result = new List<ABC_EvaluationBoardStaffDTO>();
            int evaluationBoardType = 0;
            SessionManager.DoWork(session =>
            {
                var evaluationBoard = session.Query<ABC_EvaluationBoard>().Where(e => e.Id == evaluationId).SingleOrDefault();
                var evaluationFromDate = evaluationBoard.FromDate;
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                //Admin đơn vị ủy quyền
                if (new Guid(applicationUser.WebGroupId) == WebGroupConst.TruongPhongUyQuyenID)
                {
                    //Lấy đơn vị quản lý của admin ủy quyền
                    Guid departmentId = applicationUser.DepartmentId != null ? new Guid(applicationUser.DepartmentId) : Guid.Empty;
                    //Lấy trưởng đơn vị
                    Staff staff = session.Query<Staff>().Where(s =>
                    s.Department.Id == departmentId
                    && s.StaffInfo.Position != null
                    && s.StaffInfo.Position.LaQuanLy
                    ).FirstOrDefault();

                    Guid staffId = staff != null ? staff.Id : Guid.Empty;
                    //Lấy staff trừ trưởng đơn vị
                    var temp = session.Query<Staff>().Where(s => s.Department.Id == departmentId && s.Id != staffId && (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate));
                    temp = temp.OrderByDescending(s => s.StaffInfo.Position.HSPCChucVu).ThenBy(s => s.Title.Name).ThenBy(s => s.StaffProfile.FirstName);

                    foreach (Staff st in temp)
                    {
                        ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
                        std = ParseEBStaff(st, session, evaluationId);
                        result.Add(std);
                    }
                }
                //Trường hợp user là trưởng đơn vị, group là admin đơn vị hồ sơ thông tin lương
                else if (new Guid(applicationUser.WebGroupId) == WebGroupConst.TruongPhongID)
                {
                    Guid staffId = new Guid(applicationUser.Id);
                    Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();
                    var temp = session.Query<Staff>().Where(s => s.Department.Id == staff.Department.Id && s.Id != staff.Id && (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate));
                    temp = temp.OrderByDescending(s => s.StaffInfo.Position.HSPCChucVu).ThenBy(s => s.Title.Name).ThenBy(s => s.StaffProfile.FirstName);

                    foreach (Staff st in temp)
                    {
                        ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
                        std = ParseEBStaff(st, session, evaluationId);
                        result.Add(std);
                    }
                }

            });
            if (evaluationBoardType == 3)
            {
                result = result.Where(s => s.StaffType == 2).ToList();
            }
            return result;
        }

        public ABC_EvaluationBoardStaffDTO ParseEBStaff(Staff st, NHibernate.ISession session, Guid evaluationId)
        {
            ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
            std.EvaluationId = evaluationId;
            std.StaffName = st.StaffProfile.Name;
            std.StaffId = st.Id;
            std.DepartmentId = st.Department.Id;
            if (st.StaffInfo?.Position != null)
                std.PositionName = st.StaffInfo?.Position.Name;
            else if (st.Title != null)
                std.PositionName = st.Title.Name;
            else
                std.PositionName = "";
            //std.StaffType = GetStaffType(st);
            std.StaffType = GetStaffTypeForEvaluationBoard(st, evaluationId, session);

            ABC_Rating staffRating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluationId && r.Staff.Id == st.Id).FirstOrDefault();
            if (staffRating != null)
            {
                std.IsRated = staffRating.IsRated;
                std.IsSupervisorRated = staffRating.IsSupervisorRated;
            }
            return std;
        }

        public ABC_EvaluationBoardStaffDTO ParseSyntheticEBStaff(Staff st, NHibernate.ISession session, Guid evaluationId)
        {
            ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
            std.EvaluationId = evaluationId;
            std.StaffName = st.StaffProfile.Name;
            std.StaffId = st.Id;
            std.StaffType = st.StaffInfo?.StaffType != null ? (st.StaffInfo.StaffType.Name.Contains("Giảng") ? 1 : 2) : 0;
            ABC_Rating staffRating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluationId && r.Staff.Id == st.Id).FirstOrDefault();
            std.IsRatedString = "Chưa đánh giá";
            std.IsSupervisorRatedString = "Chưa đánh giá";
            if (staffRating != null)
            {
                std.IsRated = staffRating.IsRated;
                std.IsSupervisorRated = staffRating.IsSupervisorRated;
                std.IsRatedString = std.IsRated == true ? "Đã đánh giá" : "Chưa đánh giá";
                std.IsSupervisorRatedString = std.IsSupervisorRated == true ? "Đã đánh giá" : "Chưa đánh giá";
                std.Record = staffRating.StaffNote;
            }
            //std.Record = (std.Record == "0" || std.Record == "") ? "Chưa đánh giá" : std.Record;
            return std;
        }

        public ABC_EvaluationBoardStaffDTO ParseSyntheticEBStaffExcel(Staff st, NHibernate.ISession session, Guid evaluationId)
        {
            ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
            std.EvaluationId = evaluationId;
            std.StaffId = st.Id;
            std.StaffName = st.StaffProfile.Name;
            std.PositionName = st.StaffInfo?.Position != null ? st.StaffInfo.Position.Name : "";
            if (st.StaffInfo?.Position != null)
                std.PositionName = st.StaffInfo?.Position.Name;
            else if (st.Title != null)
                std.PositionName = st.Title.Name;
            else
                std.PositionName = "";
            std.DepartmentName = st.Department.Name;
            ABC_Rating staffRating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluationId && r.Staff.Id == st.Id).FirstOrDefault();
            if (staffRating != null)
            {
                std.RatingId = staffRating.Id;
                //std.EvaluationBoardType = staffRating.ABC_EvaluationBoard.EvaluationBoardType;
            }
            return std;
        }

        [Authorize]
        [Route("")]
        public List<ABC_EvaluationBoardStaffDTO> GetListStaffSyntheticEvaluation(Guid evaluationId)
        {
            List<ABC_EvaluationBoardStaffDTO> result = new List<ABC_EvaluationBoardStaffDTO>();
            SessionManager.DoWork(session =>
            {
                var evaluationFromDate = session.Query<ABC_EvaluationBoard>().Where(e => e.Id == evaluationId).Select(q => q.FromDate).SingleOrDefault();
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                //Admin đơn vị ủy quyền
                if (new Guid(applicationUser.WebGroupId) == WebGroupConst.TruongPhongUyQuyenID)
                {
                    //Lấy đơn vị quản lý của admin ủy quyền
                    Guid departmentId = applicationUser.DepartmentId != null ? new Guid(applicationUser.DepartmentId) : Guid.Empty;
                    //Lấy trưởng đơn vị
                    Staff staff = session.Query<Staff>().Where(s =>
                    s.Department.Id == departmentId
                    && s.StaffInfo.Position != null
                    && s.StaffInfo.Position.LaQuanLy
                    ).FirstOrDefault();
                    Guid staffId = staff != null ? staff.Id : Guid.Empty;
                    //Lấy staff trừ trưởng đơn vị
                    var temp = session.Query<Staff>().Where(s => s.Department.Id == departmentId && s.Id != staffId && (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate));
                    temp = temp.OrderByDescending(s => s.StaffInfo.Position.HSPCChucVu).ThenBy(s => s.Title.Name).ThenBy(s => s.StaffProfile.FirstName);
                    foreach (Staff st in temp)
                    {
                        ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
                        std = ParseSyntheticEBStaff(st, session, evaluationId);
                        result.Add(std);
                    }
                }
                else
                {
                    Guid staffId = new Guid(applicationUser.Id);
                    Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();
                    var temp = session.Query<Staff>().Where(s => s.Department.Id == staff.Department.Id && s.Id != staff.Id && (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate));
                    temp = temp.OrderByDescending(s => s.StaffInfo.Position.HSPCChucVu).ThenBy(s => s.Title.Name).ThenBy(s => s.StaffProfile.FirstName);
                    foreach (Staff st in temp)
                    {
                        ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
                        std = ParseSyntheticEBStaff(st, session, evaluationId);
                        result.Add(std);
                    }
                }

                result = result.OrderByDescending(r => r.Record).ToList();
            });
            return result;
        }

        public List<ABC_EvaluationBoardStaffDTO> GetListStaffSyntheticEvaluationExcel(Guid evaluationId, Guid departmentId, bool isGet1)
        {
            ///isGet1: true: chỉ lấy 1 cá nhân
            ///false: lấy tất cả cá nhân
            List<ABC_EvaluationBoardStaffDTO> result = new List<ABC_EvaluationBoardStaffDTO>();
            SessionManager.DoWork(session =>
            {
                var evaluationFromDate = session.Query<ABC_EvaluationBoard>().Where(e => e.Id == evaluationId).Select(q => q.FromDate).SingleOrDefault();
                //Lấy trưởng đơn vị
                Staff staff = session.Query<Staff>().Where(s => s.Department.Id == departmentId && s.StaffInfo.Position != null && s.StaffInfo.WebUsers.Any(q => q.WebGroupId == WebGroupConst.TruongPhongID)).FirstOrDefault();
                //Lấy staff trừ trưởng đơn vị
                var temp = session.Query<Staff>().Where(s => s.Department.Id == departmentId && s.Id != (staff != null ? staff.Id : Guid.Empty) && (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate));
                temp = temp.OrderByDescending(s => s.StaffInfo.Position.HSPCChucVu).ThenBy(s => s.Title.Name).ThenBy(s => s.StaffProfile.FirstName).ThenBy(s => s.StaffProfile.Name);

                if (isGet1)
                    temp = temp.Where(s => s.Id == GetCurrentUser().Id);
                //nếu dùng List thì phải viết thế này để không bị lỗi null exception
                //temp = temp.OrderByDescending(s => s.StaffInfo.Position?.HSPCChucVu).ThenBy(s => s.Title?.Name).ThenBy(s => s.StaffProfile.FirstName).ToList();

                foreach (Staff st in temp)
                {
                    ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
                    std = ParseSyntheticEBStaffExcel(st, session, evaluationId);
                    result.Add(std);
                }
                //result = result.OrderByDescending(r => r.Record).ThenBy(a => a.Classification).ToList();
                int stt = 0;
                foreach (ABC_EvaluationBoardStaffDTO std in result)
                {
                    stt++;
                    std.OrderNumber = stt;
                }
            });
            return result;
        }

        public Guid GetCurrentUserDepartment()
        {
            Guid result = Guid.Empty;
            SessionManager.DoWork(session =>
            {
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                if (new Guid(applicationUser.WebGroupId) == WebGroupConst.TruongPhongUyQuyenID)
                {
                    result = applicationUser.DepartmentId != null ? new Guid(applicationUser.DepartmentId) : Guid.Empty;
                }
                else
                {
                    Guid staffId = new Guid(applicationUser.Id);
                    Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();
                    result = staff.Department.Id;
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public List<ABC_EvaluationBoardStaffDTO> GetListDepartmentLeaderSyntheticEvaluationExcel(Guid evaluationId)
        {
            List<ABC_EvaluationBoardStaffDTO> result = new List<ABC_EvaluationBoardStaffDTO>();
            SessionManager.DoWork(session =>
            {
                var evaluationFromDate = session.Query<ABC_EvaluationBoard>().Where(e => e.Id == evaluationId).Select(q => q.FromDate).SingleOrDefault();
                //var deptLeaderList = session.Query<Staff>().Where(s => s.StaffInfo.Position != null && !s.StaffInfo.Position.Name.Contains("Phó") && !s.StaffInfo.Position.Name.Contains("bộ môn") && !s.StaffInfo.Position.Name.Contains("Hiệu") && !s.StaffInfo.Position.Name.Contains("Tổ")).OrderBy(s => s.Department.OrderNumber);
                var deptLeaderList = session.Query<Staff>().Where(s => s.StaffInfo.Position != null && s.StaffInfo.Position.LaQuanLy && (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate)).OrderBy(s => s.Department.OrderNumber);
                var currentStaffId = GetCurrentUser().Id;
                Staff staff = session.Query<Staff>().SingleOrDefault(s => s.Id == currentStaffId);
                List<Staff> staffList = new List<Staff>();
                foreach (Staff st in deptLeaderList)
                {
                    if (staff != null)
                    {
                        foreach (var department in staff.Departments)
                        {
                            if (st.Department.Id == department.Id)
                                staffList.Add(st);
                        }
                    }
                    else //trường hợp user ko có thông tin nhân viên //user psc, admin, ...
                        staffList.Add(st);
                }
                foreach (Staff st in staffList)
                {
                    ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
                    std = ParseSyntheticEBStaffExcel(st, session, evaluationId);
                    std.PositionName = std.PositionName + "/" + std.DepartmentName;
                    result.Add(std);
                }
                //result = result.OrderByDescending(r => r.Record).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_EvaluationBoardStaffDTO> GetListDepartmentLeaderSyntheticEvaluation(Guid evaluationId)
        {
            List<ABC_EvaluationBoardStaffDTO> result = new List<ABC_EvaluationBoardStaffDTO>();
            SessionManager.DoWork(session =>
            {
                var evaluationFromDate = session.Query<ABC_EvaluationBoard>().Where(e => e.Id == evaluationId).Select(q => q.FromDate).SingleOrDefault();
                //var deptLeaderList = session.Query<Staff>().Where(s => s.StaffInfo.Position != null && !s.StaffInfo.Position.Name.Contains("Phó") && !s.StaffInfo.Position.Name.Contains("bộ môn") && !s.StaffInfo.Position.Name.Contains("Hiệu") && !s.StaffInfo.Position.Name.Contains("Tổ")).OrderBy(s => s.Department.OrderNumber);
                //var deptLeaderList = session.Query<Staff>().Where(s => s.StaffInfo.Position != null && !s.StaffInfo.Position.Name.Contains("Phó") && !s.StaffInfo.Position.Name.Contains("Hiệu") && !s.StaffInfo.Position.Name.Contains("Tổ")).OrderBy(s => s.Department.OrderNumber);
                var deptLeaderList = session.Query<Staff>().Where(s => (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate) && s.StaffInfo.Position != null && s.StaffInfo.WebUsers.Any(q => q.WebGroupId == WebGroupConst.TruongPhongID)).OrderBy(s => s.Department.OrderNumber);

                foreach (Staff st in deptLeaderList)
                {
                    ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
                    std.StaffName = st.StaffProfile.Name;
                    std.DepartmentName = st.Department.Name;
                    std.PositionName = st.StaffInfo?.Position?.Name;
                    ABC_Rating staffRating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluationId && r.Staff.Id == st.Id).FirstOrDefault();
                    if (staffRating != null)
                    {
                        std.IsRated = staffRating.IsRated;
                        std.IsSupervisorRated = staffRating.IsSupervisorRated;
                        std.IsRatedString = std.IsRated == true ? "Đã đánh giá" : "Chưa đánh giá";
                        std.IsSupervisorRatedString = std.IsSupervisorRated == true ? "Đã đánh giá" : "Chưa đánh giá";
                    }
                    std.Record = (std.Record == "0" || std.Record == "") ? "Chưa đánh giá" : std.Record;
                    result.Add(std);
                }
                result = result.OrderByDescending(r => r.Record).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_EvaluationBoardStaffDTO> GetListDepartmentLeaderSyntheticEvaluation_Principal(Guid evaluationId, bool isGet1)
        {
            ///isGet1: true: chỉ lấy 1 trưởng đơn vị
            ///false: lấy tất cả trưởng đơn vị
            List<ABC_EvaluationBoardStaffDTO> result = new List<ABC_EvaluationBoardStaffDTO>();
            SessionManager.DoWork(session =>
            {
                var evaluationFromDate = session.Query<ABC_EvaluationBoard>().Where(e => e.Id == evaluationId).Select(q => q.FromDate).SingleOrDefault();
                var currentStaffId = GetCurrentUser().Id;
                Staff staff = session.Query<Staff>().SingleOrDefault(s => s.Id == currentStaffId);
                //lấy luôn danh sách hiệu phó
                IEnumerable<Staff> deptLeaderList = session.Query<Staff>().Where(s => (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate) && s.StaffInfo.Position != null && s.StaffInfo.Position.LaQuanLy).OrderBy(s => s.Department.OrderNumber);
                if (isGet1)
                    deptLeaderList = deptLeaderList.Where(s => s.Department.Id == staff.Department.Id);

                List<Staff> staffList = new List<Staff>();
                foreach (Staff st in deptLeaderList)
                {
                    if (staff != null && !isGet1)
                    {
                        foreach (var department in staff.Departments)
                        {
                            if (st.Department.Id == department.Id)
                                staffList.Add(st);
                        }
                    }
                    else //trường hợp user ko có thông tin nhân viên //user psc, admin, ... thì lấy hết trưởng đơn vị
                        staffList.Add(st);
                }
                foreach (Staff st in staffList)
                {
                    ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
                    std = ParseSyntheticEBStaffExcel(st, session, evaluationId);
                    //std.StaffId = st.Id;
                    //std.StaffName = st.StaffProfile.Name;
                    //std.DepartmentId = st.Department.Id;
                    //std.DepartmentName = st.Department.Name;
                    //std.PositionName = st.StaffInfo?.Position?.Name;
                    //ABC_Rating staffRating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluationId && r.Staff.Id == st.Id).FirstOrDefault();
                    //if (staffRating != null)
                    //{
                    //    std.IsRated = staffRating.IsRated;
                    //    std.IsSupervisorRated = staffRating.IsSupervisorRated;
                    //    std.IsRatedString = std.IsRated == true ? "Đã đánh giá" : "Chưa đánh giá";
                    //    std.IsSupervisorRatedString = std.IsSupervisorRated == true ? "Đã đánh giá" : "Chưa đánh giá";
                    //}
                    //std.Record = (std.Record == "0" || std.Record == "") ? "Chưa đánh giá" : std.Record;
                    result.Add(std);
                }
                //result = result.OrderByDescending(r => r.Record).ToList();
                int stt = 0;
                foreach (ABC_EvaluationBoardStaffDTO std in result)
                {
                    stt++;
                    std.OrderNumber = stt;
                }
            });
            return result;
        }

        ///// <summary>
        ///// Lấy ra trưởng đơn vị của phòng ban, khoa
        ///// </summary>
        ///// <param name="departmentId"></param>
        ///// <param name="session"></param>
        ///// <returns></returns>
        //public Staff GetDepartmentLeader(Guid departmentId, NHibernate.ISession session)
        //{
        //    return session.Query<Staff>().Where(s => s.Department.Id == departmentId && s.StaffInfo.Position != null && !s.StaffInfo.StaffType.ManageCode.Contains("NV") && (s.Department.ManageCode.Contains("K.") || s.Department.ManageCode.Contains("P."))).FirstOrDefault();
        //}

        ////lấy danh sách trưởng phòng, trưởng khoa (chưa đúng)
        //public IEnumerable<Staff> GetDepartmentLeaderList(NHibernate.ISession session)
        //{
        //    return session.Query<Staff>().Where(s => s.StaffInfo.Position != null && !s.StaffInfo.StaffType.ManageCode.Contains("NV") && (s.Department.ManageCode.Contains("K.") || s.Department.ManageCode.Contains("P."))).ToList();
        //}

        ///// <summary>
        ///// Kiểm tra nếu là trưởng đơn vị thì return true
        ///// </summary>
        ///// <param name="departmentId"></param>
        ///// <param name="session"></param>
        ///// <returns></returns>
        //public bool CheckDepartmentLeader(Staff staff, NHibernate.ISession session)
        //{
        //    if (staff.StaffInfo.Position != null && !staff.StaffInfo.StaffType.ManageCode.Contains("NV") && (staff.Department.ManageCode.Contains("K.") || staff.Department.ManageCode.Contains("P.")))
        //    {
        //        return true;
        //    }
        //    else return false;
        //}

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_EvaluationBoardStaffDTO> GetListDepartmentLeaderEvaluation(Guid evaluationId)
        {
            List<ABC_EvaluationBoardStaffDTO> result = new List<ABC_EvaluationBoardStaffDTO>();
            SessionManager.DoWork(session =>
            {
                var evaluationFromDate = session.Query<ABC_EvaluationBoard>().Where(e => e.Id == evaluationId).Select(q => q.FromDate).SingleOrDefault();
                //var deptLeaderList = session.Query<Staff>().Where(s => s.StaffInfo.Position != null && !s.StaffInfo.Position.Name.Contains("Phó") && !s.StaffInfo.Position.Name.Contains("bộ môn") && !s.StaffInfo.Position.Name.Contains("Hiệu") && !s.StaffInfo.Position.Name.Contains("Tổ")).OrderBy(s => s.Department.OrderNumber);
                var deptLeaderList = session.Query<Staff>().Where(s => (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate) && s.StaffInfo.Position != null && (s.StaffInfo.WebUsers.Any(q => q.WebGroupId == WebGroupConst.TruongPhongID) || s.StaffInfo.Position.LaQuanLy)).OrderBy(s => s.Department.OrderNumber);
                var currentStaffId = GetCurrentUser().Id;
                Staff staff = session.Query<Staff>().SingleOrDefault(s => s.Id == currentStaffId);
                List<Staff> staffList = new List<Staff>();
                //staffList = deptLeaderList.Where(s => staff.Departments.Any(d => d.Id == s.Department.Id)).ToList();
                foreach (Staff st in deptLeaderList)
                {
                    foreach (var department in staff.Departments)
                    {
                        if (st.Department != null && st.Department.Id == department.Id)
                            staffList.Add(st);
                    }
                }
                foreach (Staff st in staffList)
                {
                    ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
                    std.EvaluationId = evaluationId;
                    std.StaffName = st.StaffProfile.Name;
                    std.DepartmentName = st.Department.Name;
                    std.DepartmentId = st.Department.Id;
                    //std.PositionName = st.StaffInfo?.Position?.Name;
                    std.StaffType = GetStaffTypeForEvaluationBoard(st, evaluationId, session);
                    std.StaffId = st.Id;
                    ABC_Rating staffRating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluationId && r.Staff.Id == st.Id).FirstOrDefault();
                    if (staffRating != null)
                    {
                        std.IsRated = staffRating.IsRated;
                        std.IsSupervisorRated = staffRating.IsSupervisorRated;
                    }
                    result.Add(std);
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
                if (new Guid(applicationUser.WebGroupId) == WebGroupConst.HieuTruongID)
                {
                    result = 2;
                }
                else
                //Admin đơn vị ủy quyền
                if (new Guid(applicationUser.WebGroupId) == WebGroupConst.TruongPhongUyQuyenID)
                {
                    result = 3;
                }
                else
                //Tài khoản Admin Đơn vị (hồ sơ và thông tin lương)
                if (new Guid(applicationUser.WebGroupId) == WebGroupConst.TruongPhongID)
                {
                    result = 1;
                }
                //Tài khoản bình thường
                else if (new Guid(applicationUser.WebGroupId) == WebGroupConst.TaiKhoanCaNhanID)
                {
                    result = 0;
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int GetCreateEvaluationBoard(string year) // cho thêm vô đây fromdate todate
        {
            int result = 1;
            SessionManager.DoWork(session =>
            {
                List<ABC_EvaluationBoard> ev = new List<ABC_EvaluationBoard>();
                
                //Tạo Quý 1
                ABC_EvaluationBoard evQuater1 = new ABC_EvaluationBoard();
                evQuater1.Id = Guid.NewGuid();
                evQuater1.Quater = 1;
                evQuater1.Year = year;
                evQuater1.Name = "Năm " + year.ToString();
                evQuater1.FromDate = new DateTime(int.Parse(year.Substring(0, 4)), 8, 1);
                evQuater1.ToDate = new DateTime(int.Parse(year.Substring(0, 4)), 10, 31);
                ev.Add(evQuater1);

                //Tạo Quý 2
                ABC_EvaluationBoard evQuater2 = new ABC_EvaluationBoard();
                evQuater2.Id = Guid.NewGuid();
                evQuater2.Quater = 1;
                evQuater2.Year = year;
                evQuater2.Name = "Năm " + year.ToString();
                evQuater2.FromDate = new DateTime(int.Parse(year.Substring(0, 4)), 11, 1);
                evQuater2.ToDate = new DateTime(int.Parse(year.Substring(0, 4)), 1, 31);
                ev.Add(evQuater2);

                //Tạo Quý 3
                ABC_EvaluationBoard evQuater3 = new ABC_EvaluationBoard();
                evQuater3.Id = Guid.NewGuid();
                evQuater3.Quater = 1;
                evQuater3.Year = year;
                evQuater3.Name = "Năm " + year.ToString();
                evQuater3.FromDate = new DateTime(int.Parse(year.Substring(0, 4)), 2, 1);
                evQuater3.ToDate = new DateTime(int.Parse(year.Substring(year.Length - 1, 4)), 4, 30);
                ev.Add(evQuater3);

                //Tạo Quý 4
                ABC_EvaluationBoard evQuater4 = new ABC_EvaluationBoard();
                evQuater4.Id = Guid.NewGuid();
                evQuater4.Quater = 1;
                evQuater4.Year = year;
                evQuater4.Name = "Năm " + year.ToString();
                evQuater4.FromDate = new DateTime(int.Parse(year.Substring(0, 4)), 5, 1);
                evQuater4.ToDate = new DateTime(int.Parse(year.Substring(year.Length - 1, 4)), 7, 31);
                ev.Add(evQuater4);

                foreach (ABC_EvaluationBoard e in ev)
                {
                    session.Save(e);
                }

            });
            return result;
        }

        [Authorize]
        [Route("")]
        public bool GetCheckExistEvaluationBoard(string year)
        {
            bool result = false;
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_EvaluationBoard>().Any(e => e.Year == year);
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public bool GetCheckLockAllChildRating(Guid evaluationId, Guid staffId, Guid departmentId, int staffType)
        {
            bool result = false;
            // Nếu là true: có tồn tại kế hoạch chưa khóa
            // Sửa lại: true khi chưa có bản đánh giá con nào được cấp trên khóa
            SessionManager.DoWork(session =>
            {
                //Lấy kỳ đánh giá
                ABC_EvaluationBoard eb = session.Query<ABC_EvaluationBoard>().Where(e => e.Id == evaluationId).SingleOrDefault();

                var listRating = session.Query<ABC_Rating>().Where(r =>
                    r.ABC_EvaluationBoard.Id == evaluationId
                    && r.Staff.Id == staffId
                    && r.Department.Id == departmentId
                ).ToList();

                //Kiểm tra nếu có bất kỳ đánh giá tháng nào chưa đc khóa
                result = !listRating.Any(c => c.IsSupervisorRated == true);
                if (listRating.Count() == 0)
                    result = true;
            });

            return result;
        }

        [Authorize]
        [Route("")]
        public ABC_LockedRatingDepartment GetLockedRatingDepartment(Guid evaluationId, Guid departmentId)
        {
            ABC_LockedRatingDepartment result = new ABC_LockedRatingDepartment();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_LockedRatingDepartment>().Where(r => r.ABC_EvaluationBoard.Id == evaluationId && r.Department.Id == departmentId).FirstOrDefault();
            });
            return result;
        }

        /// <summary>
        /// Khóa và mở khóa đánh giá cả đơn vị
        /// </summary>
        /// <param name="evaluationId"></param>
        /// <param name="departmentId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        public int GetLockUnlockRating(Guid evaluationId, Guid departmentId, int type)
        {

            int result = 0;
            //Nếu là true: có tồn tại kế hoạch chưa khóa
            SessionManager.DoWork(session =>
            {
                try
                {


                    //Kiểm tra đã khóa trước đó hay chưa
                    ABC_LockedRatingDepartment lockedDept = GetLockedRatingDepartment(evaluationId, departmentId);

                    //Nếu chưa có thì thêm mới
                    if (lockedDept == null)
                    {
                        lockedDept = new ABC_LockedRatingDepartment();
                        lockedDept.Id = Guid.NewGuid();
                        lockedDept.Status = type == 1 ? true : false;
                        lockedDept.Department = new Department() { Id = departmentId };
                        lockedDept.ABC_EvaluationBoard = new ABC_EvaluationBoard() { Id = evaluationId };
                        session.Save(lockedDept);
                    }

                    //Lấy tất cả rating của cả đơn vị trong kỳ đánh giá
                    var temp = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluationId && r.Department.Id == departmentId);
                    //Loại 1 là khóa, loại 2 là mở khóa
                    if (type == 1)
                    {
                        foreach (ABC_Rating rating in temp)
                        {
                            rating.IsRatingLocked = true;
                            session.Update(rating);
                        }

                        //Cập nhật lại trạng thái trong bảng khóa
                        lockedDept.Status = true;
                        session.Update(lockedDept);
                    }
                    else if (type == 2)
                    {
                        foreach (ABC_Rating rating in temp)
                        {
                            rating.IsRatingLocked = false;
                            session.Update(rating);
                        }

                        //Cập nhật lại trạng thái trong bảng khóa
                        lockedDept.Status = false;
                        session.Update(lockedDept);
                    }
                    result = 1;
                }
                catch (Exception e)
                {
                    result = 0;
                }


            });
            return result;
        }

        /// <summary>
        /// - Mở khóa cho trưởng đơn vị đánh giá cá nhân
        /// - Mở khóa cho cá nhân tự đánh giá
        /// - Mở khóa cho Hiệu trưởng đánh giá trưởng đơn vị
        /// - Mở khóa cho trưởng đơn vị tự đánh giá
        /// </summary>
        /// <param name="ratingId"></param>
        /// <param name="unlockMode">
        /// 1: Mở khóa cho trưởng đơn vị đánh giá cá nhân.
        /// 2: Mở khóa cho cá nhân tự đánh giá lại, đồng thời mở khóa cho trưởng đơn vị nếu trưởng đơn vị đã đánh giá
        /// </param>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        public int GetUnlockRatingStaff(Guid ratingId, int unlockMode)
        {
            int result = 0;
            SessionManager.DoWork(session =>
            {
                try
                {
                    var ratingStaffs = session.Query<ABC_Rating>().SingleOrDefault(r => r.Id == ratingId);
                    if (unlockMode == 1)
                    {
                        if (ratingStaffs.IsSupervisorRated == false)
                            result = 2;
                        else
                        {
                            ratingStaffs.IsSupervisorRated = false;
                            session.Update(ratingStaffs);
                            result = 1;
                        }
                    }
                    else if (unlockMode == 2)
                    {
                        if (ratingStaffs.IsRated == false)
                            result = 2;
                        else
                        {
                            ratingStaffs.IsRated = false;
                            result = 1;
                            if (ratingStaffs.IsSupervisorRated == true)
                            {
                                ratingStaffs.IsSupervisorRated = false;
                                result = 3;
                            }
                            session.Update(ratingStaffs);
                        }
                    }
                }
                catch (Exception e)
                {
                    result = 0;
                }
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public StaffDTO GetCurrentUser()
        {
            StaffDTO result = new StaffDTO();
            SessionManager.DoWork(session =>
            {
                //Lấy user hiện tại
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                if (applicationUser.Id != null && applicationUser.Id != "")
                {
                    Guid staffId = new Guid(applicationUser.Id);
                    Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();
                    if (staff != null)
                    {
                        result.Id = staff.Id;
                        result.DepartmentId = staff.Department.Id;
                        //result.StaffType = staff.StaffInfo?.StaffType != null ? (staff.StaffInfo.StaffType.Name.Contains("Giảng") ? 1 : 2) : 0;

                        //Sửa ngày 19/07/2016: Trưởng khoa, trưởng bộ môn, giảng viên evaluationBoard ko hiện từng tháng.
                        result.StaffType = GetStaffType(staff);
                    }
                }

            });
            return result;
        }

        //1: giảng viên, trưởng khoa, trưởng bộ môn
        //2: nhân viên, trưởng phòng
        public int GetStaffType(Staff staff)
        {
            int result = 0;
            if (staff.StaffInfo?.StaffType?.ManageCode?.StartsWith("G") == true)
            {
                result = 1;
            }
            else result = 2;
            return result;
        }
        public int GetStaffTypeForEvaluationBoard(Staff staff, Guid evaluationId, NHibernate.ISession session)
        {
            int result = 0;
            try
            {
                ThongTinDanhGia obj = session.Query<ThongTinDanhGia>().SingleOrDefault(q => q.StaffInfo.Id == staff.Id && q.ABC_EvaluationBoard.Id == evaluationId);
                if (obj != null)
                {
                    result = Convert.ToInt32(obj.MaDoiTuongDanhGia);
                    if (!obj.DanhGia || (result != 1 && result != 2))
                        result = 0;
                }
                else
                {
                    result = GetStaffType(staff);
                }
            }
            catch { }
            return result;
        }
        [Authorize]
        [Route("")]
        public List<ABC_EvaluationBoardDTO> GetEvaluationBoardList()
        {
            List<ABC_EvaluationBoardDTO> result = new List<ABC_EvaluationBoardDTO>();
            SessionManager.DoWork(session =>
            {
                var temp = session.Query<ABC_EvaluationBoard>().OrderBy(eb => eb.FromDate);
                foreach (ABC_EvaluationBoard eb in temp)
                {
                    ABC_EvaluationBoardDTO ebd = new ABC_EvaluationBoardDTO();
                    ebd.Id = eb.Id;
                    ebd.Quater = eb.Quater;
                    ebd.Year = eb.Year;
                    ebd.Name = eb.Name;
                    ebd.FromDate = eb.FromDate;
                    ebd.ToDate = eb.ToDate;
                    result.Add(ebd);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public List<DepartmentDTO> GetDepartmentList()
        {
            List<DepartmentDTO> result = new List<DepartmentDTO>();
            SessionManager.DoWork(session =>
            {
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                BoPhan_Factory factory = BoPhan_Factory.New();
                var temp = factory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(new Guid(applicationUser.UserId), new Guid(applicationUser.CongTyId));
                if (temp.Count() > 0)
                {
                    foreach (BoPhan d in temp)
                    {
                        DepartmentDTO dd = new DepartmentDTO();
                        dd.Id = d.Oid;
                        dd.Name = d.TenBoPhan;
                        result.Add(dd);
                    }
                }
                else
                {
                    var temp1 = session.Query<Department>().Where(q => q.Id == new Guid(applicationUser.DepartmentId));
                    foreach (Department d in temp1)
                    {
                        DepartmentDTO dd = new DepartmentDTO();
                        dd.Id = d.Id;
                        dd.Name = d.Name;
                        result.Add(dd);
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public DepartmentDTO GetDepartment(Guid Id)
        {
            DepartmentDTO result = new DepartmentDTO();
            SessionManager.DoWork(session =>
            {
                var temp = session.Query<Department>().Where(d => d.Id == Id).SingleOrDefault();
                result.Id = temp.Id;
                result.Name = temp.Name;
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_EvaluationBoardStaffDTO> GetListStaffEvaluationByDepartment(Guid evaluationId, Guid departmentId)
        {
            List<ABC_EvaluationBoardStaffDTO> result = new List<ABC_EvaluationBoardStaffDTO>();
            SessionManager.DoWork(session =>
            {
                var evaluation = session.Query<ABC_EvaluationBoard>().Where(q => q.Id == evaluationId).SingleOrDefault();
                var evaluationFromDate = evaluation.FromDate;
                var temp = session.Query<Staff>().Where(s => s.Department.Id == departmentId && (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate));
                temp = temp.OrderByDescending(s => s.StaffInfo.Position.HSPCChucVu).ThenBy(s => s.Title.Name).ThenBy(s => s.StaffProfile.FirstName).ThenBy(s => s.StaffProfile.Name);

                foreach (Staff st in temp)
                {
                    ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
                    std.StaffName = st.StaffProfile.Name;
                    std.StaffId = st.Id;
                    std.DepartmentId = st.Department.Id;
                    std.StaffType = st.StaffInfo?.StaffType != null ? (st.StaffInfo.StaffType.Name.Contains("Giảng") ? 1 : 2) : 0;
                    ABC_Rating staffRating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluation.Id && r.Staff.Id == st.Id).FirstOrDefault();
                    if (staffRating != null)
                    {
                        std.IsRated = staffRating.IsRated;
                    }
                    result.Add(std);
                }
                //result = result.OrderByDescending(r => r.Record).ToList();
                int stt = 0;
                foreach (ABC_EvaluationBoardStaffDTO std in result)
                {
                    stt++;
                    std.OrderNumber = stt;
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public byte GetCheckIsAdminGroup()
        {
            byte result = 0;
            SessionManager.DoWork(session =>
            {
                //Lấy user hiện tại
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                //Nếu là admin
                if (new Guid(applicationUser.WebGroupId) == WebGroupConst.AdminId || new Guid(applicationUser.WebGroupId) == WebGroupConst.QuanTriTruongID)
                {
                    result = 1;
                }
                //Nếu là admin đơn vị
                else if (new Guid(applicationUser.WebGroupId) == WebGroupConst.TruongPhongID || new Guid(applicationUser.WebGroupId) == WebGroupConst.TruongPhongUyQuyenID)
                {
                    result = 2;
                }

            });
            return result;
        }
    }
}
