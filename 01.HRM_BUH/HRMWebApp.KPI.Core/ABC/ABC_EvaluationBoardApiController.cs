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
using System.Web.Configuration;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_EvaluationBoardApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_EvaluationBoardDTO> GetList() //tab đánh giá cấp dưới
        {
            List<ABC_EvaluationBoardDTO> result = new List<ABC_EvaluationBoardDTO>();
            SessionManager.DoWork(session =>
            {
                StaffDTO staff = GetCurrentUser();
                IEnumerable<ABC_EvaluationBoard> planList = new List<ABC_EvaluationBoard>();
                // Nếu trong đơn vị có nhân viên thì lấy tất cả
                if (session.Query<Staff>().Any(s => s.Department != null && s.Department.Id == staff.DepartmentId && s.StaffInfo.StaffType.ManageCode == "NV" && s.StaffStatus.NoLongerWork == 0))
                {
                    planList = session.Query<ABC_EvaluationBoard>();
                }
                else // Không có nhân viên thì không lấy tháng
                {
                    planList = session.Query<ABC_EvaluationBoard>().Where(e => e.EvaluationBoardType != 3);
                }
                foreach (ABC_EvaluationBoard pl in planList)
                {
                    ABC_EvaluationBoardDTO pd = pl.Map<ABC_EvaluationBoardDTO>();
                    pd.ParentId = pl.ABC_ParentEvaluationBoard != null ? (Guid?)pl.ABC_ParentEvaluationBoard.Id : null;
                    pd.EvaluationBoardType = pl.EvaluationBoardType;
                    result.Add(pd);
                }
            });
            result = result.OrderBy(r => r.FromDate).ToList();
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_EvaluationBoardDTO> GetListDepartment() //tab Hiệu trưởng đánh giá đơn vị
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
                    pd.ParentId = pl.ABC_ParentEvaluationBoard != null ? (Guid?)pl.ABC_ParentEvaluationBoard.Id : null;
                    pd.EvaluationBoardType = pl.EvaluationBoardType;
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
                //Nếu là giảng viên thì ko lấy tháng
                if (staff.StaffType == 1)
                {
                    planList = session.Query<ABC_EvaluationBoard>().Where(e => e.EvaluationBoardType != 3);
                }
                else
                {
                    planList = session.Query<ABC_EvaluationBoard>();
                }
                foreach (ABC_EvaluationBoard pl in planList)
                {
                    ABC_EvaluationBoardDTO pd = pl.Map<ABC_EvaluationBoardDTO>();
                    pd.ParentId = pl.ABC_ParentEvaluationBoard != null ? (Guid?)pl.ABC_ParentEvaluationBoard.Id : null;
                    pd.EvaluationBoardType = pl.EvaluationBoardType;
                    ABC_Rating rating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == pl.Id && r.Staff.Id == staff.Id).FirstOrDefault();
                    if (rating != null && rating.IsSupervisorRated==true)
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
            try
            {
                List<ABC_EvaluationBoardDTO> result = new List<ABC_EvaluationBoardDTO>();
                SessionManager.DoWork(session =>
                {
                    IEnumerable<ABC_EvaluationBoard> planList = new List<ABC_EvaluationBoard>();
                    planList = session.Query<ABC_EvaluationBoard>();

                    foreach (ABC_EvaluationBoard pl in planList)
                    {
                        ABC_EvaluationBoardDTO pd = pl.Map<ABC_EvaluationBoardDTO>();
                        pd.ParentId = pl.ABC_ParentEvaluationBoard != null ? (Guid?)pl.ABC_ParentEvaluationBoard.Id : null;
                        pd.EvaluationBoardType = pl.EvaluationBoardType;
                        result.Add(pd);
                    }
                });
                result = result.OrderBy(r => r.FromDate).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_EvaluationBoardApiController/GetListEvaluationManage", ex);
                throw ex;
            }
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
                evaluationBoardType = evaluationBoard.EvaluationBoardType;
                var evaluationFromDate = evaluationBoard.FromDate;
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                //Admin đơn vị ủy quyền
                if (applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000005")
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
                    var temp = session.Query<Staff>().Where(s => (s.Department.Id == departmentId ||
                    // 26/8/2019 lấy thêm trưởng đơn vị phòng ban con
                    (s.Department.ParentDepartment.Id == departmentId && s.StaffInfo.Position.LaQuanLy && s.StaffInfo.WebUsers.Any(u => u.WebGroupId == new Guid("00000000-0000-0000-0000-000000000004"))))
                    && s.Id != staffId && (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate));
                    temp = temp.OrderByDescending(s => s.StaffInfo.Position.HSPCChucVu).ThenBy(s => s.Title.Name).ThenBy(s => s.StaffProfile.FirstName);

                    foreach (Staff st in temp)
                    {
                        ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
                        std = ParseEBStaff(st, session, evaluationId);
                        result.Add(std);
                    }
                }
                //Trường hợp user là trưởng đơn vị, group là admin đơn vị hồ sơ thông tin lương
                else if (applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000004")
                {
                    Guid staffId = new Guid(applicationUser.Id);
                    Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();
                    var temp = session.Query<Staff>().Where(s => (s.Department.Id == staff.Department.Id || 
                    // 26/8/2019 lấy thêm trưởng đơn vị phòng ban con
                    (s.Department.ParentDepartment.Id == staff.Department.Id && s.StaffInfo.Position.LaQuanLy && s.StaffInfo.WebUsers.Any(u => u.WebGroupId == new Guid("00000000-0000-0000-0000-000000000004"))))
                    && s.Id != staff.Id && (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate));
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
            if (st.StaffInfo.Position != null)
                std.PositionName = st.StaffInfo.Position.Name;
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
            std.StaffType = st.StaffInfo.StaffType != null ? (st.StaffInfo.StaffType.Name.Contains("Giảng") ? 1 : 2) : 0;
            ABC_Rating staffRating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluationId && r.Staff.Id == st.Id).FirstOrDefault();
            std.IsRatedString = "Chưa đánh giá";
            std.IsSupervisorRatedString = "Chưa đánh giá";
            if (staffRating != null)
            {
                std.IsRated = staffRating.IsRated;
                std.IsSupervisorRated = staffRating.IsSupervisorRated;
                std.IsRatedString = std.IsRated == true ? "Đã đánh giá" : "Chưa đánh giá";
                std.IsSupervisorRatedString = std.IsSupervisorRated == true ? "Đã đánh giá" : "Chưa đánh giá";
                std.Record = ConvertDecimalToString1Place(staffRating.TotalSupervisorRecord);
                std.Classification = staffRating.Classification;
            }
            //std.Record = (std.Record == "0" || std.Record == "") ? "Chưa đánh giá" : std.Record;
            return std;
        }
        public ABC_EvaluationBoardStaffDTO ParseSyntheticEBStaffExcel(Staff st, NHibernate.ISession session, Guid evaluationId)
        {
            ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
            std.EvaluationId = evaluationId;
            std.EvaluationBoardType = session.Query<ABC_EvaluationBoard>().SingleOrDefault(q => q.Id == evaluationId).EvaluationBoardType;
            std.StaffId = st.Id;
            std.StaffName = st.StaffProfile.Name;
            std.PositionName = st.StaffInfo?.Position != null ? st.StaffInfo.Position.Name : "";
            if (st.StaffInfo?.Position != null)
                std.PositionName = st.StaffInfo?.Position.Name;
            else if (st.Title != null)
                std.PositionName = st.Title.Name;
            else
                std.PositionName = "";
            std.DepartmentName = st.Department?.Name;
            ABC_Rating staffRating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluationId && r.Staff.Id == st.Id).FirstOrDefault();
            if (staffRating != null)
            {
                std.StaffRecord = ConvertDecimalToString1Place(staffRating.TotalStaffRecord);
                std.Record = staffRating.IsSupervisorRated ? ConvertDecimalToString1Place(staffRating.TotalSupervisorRecord) : "";
                std.Classification = staffRating.IsSupervisorRated ? staffRating.Classification : "";
                std.RatingId = staffRating.Id;
                //std.EvaluationBoardType = staffRating.ABC_EvaluationBoard.EvaluationBoardType;
                std.ClassificationSecond = staffRating.ClassificationSecond;
                std.ClassificationThird = staffRating.ClassificationThird;
                std.NoteSecond = staffRating.NoteSecond;
                std.NoteThird = staffRating.NoteThird;
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
                if (applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000005")
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
            try
            {
                ///isGet1: true: chỉ lấy 1 cá nhân
                ///false: lấy tất cả cá nhân
                List<ABC_EvaluationBoardStaffDTO> result = new List<ABC_EvaluationBoardStaffDTO>();
                SessionManager.DoWork(session =>
                {
                    var evaluationFromDate = session.Query<ABC_EvaluationBoard>().Where(e => e.Id == evaluationId).Select(q => q.FromDate).SingleOrDefault();
                    //Lấy trưởng đơn vị
                    Staff staff = session.Query<Staff>().Where(s => s.Department.Id == departmentId && s.StaffInfo.Position != null && s.StaffInfo.WebUsers.Any(user => user.WebGroupId == Guid.Parse("00000000-0000-0000-0000-000000000004"))).FirstOrDefault();
                    //Lấy staff trừ trưởng đơn vị
                    var temp = session.Query<Staff>().Where(s => (s.Department.Id == departmentId ||
                        // 26/8/2019 lấy thêm trưởng đơn vị phòng ban con
                        (s.Department.ParentDepartment.Id == departmentId && s.StaffInfo.Position.LaQuanLy && s.StaffInfo.WebUsers.Any(u => u.WebGroupId == new Guid("00000000-0000-0000-0000-000000000004"))))
                        && s.Id != (staff != null ? staff.Id : Guid.Empty) && (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate));
                    temp = temp.OrderByDescending(s => s.StaffInfo.Position.HSPCChucVu).ThenBy(s => s.Title.Name).ThenBy(s => s.StaffProfile.FirstName).ThenBy(s => s.StaffProfile.Name);

                    var currentStaffId = GetCurrentUser().Id;
                    if (isGet1)
                        temp = temp.Where(s => s.Id == currentStaffId);
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

                        //nếu là đánh giá tháng thì không hiện xếp loại
                        if (std.EvaluationBoardType == 3)
                            std.Classification = "";
                    }
                });
                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_EvaluationBoardApiController/GetListStaffSyntheticEvaluationExcel", ex);
                throw ex;
            }
        }
        public Guid GetCurrentUserDepartment()
        {
            Guid result = Guid.Empty;
            SessionManager.DoWork(session =>
            {
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                if (applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000005")
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
                var deptLeaderList = session.Query<Staff>().Where(s => (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate) && s.StaffInfo.Position != null && s.StaffInfo.WebUsers.Any(user => user.WebGroupId == Guid.Parse("00000000-0000-0000-0000-000000000004"))).OrderBy(s => s.Department.OrderNumber);
                
                foreach (Staff st in deptLeaderList)
                {
                    ABC_EvaluationBoardStaffDTO std = new ABC_EvaluationBoardStaffDTO();
                    std.StaffName = st.StaffProfile.Name;
                    std.DepartmentName = st.Department.Name;
                    std.PositionName = st.StaffInfo.Position.Name;
                    ABC_Rating staffRating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluationId && r.Staff.Id == st.Id).FirstOrDefault();
                    if (staffRating != null)
                    {
                        std.IsRated = staffRating.IsRated;
                        std.IsSupervisorRated = staffRating.IsSupervisorRated;
                        std.IsRatedString = std.IsRated == true ? "Đã đánh giá" : "Chưa đánh giá";
                        std.IsSupervisorRatedString = std.IsSupervisorRated == true ? "Đã đánh giá" : "Chưa đánh giá";
                        std.Record = ConvertDecimalToString1Place(staffRating.TotalSupervisorRecord);
                        std.Classification = staffRating.Classification;
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
                Guid boPhanTruong = Guid.Empty;
                Guid.TryParse(System.Configuration.ConfigurationManager.AppSettings["BoPhanCha"], out boPhanTruong);
                //lấy luôn danh sách hiệu phó
                IEnumerable<Staff> deptLeaderList = session.Query<Staff>().Where(s => (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate) && s.Department.ParentDepartment.Id == boPhanTruong && s.StaffInfo.Position != null && s.StaffInfo.Position.LaQuanLy).OrderBy(s => s.Department.OrderNumber);
                if (isGet1)
                    deptLeaderList = deptLeaderList.Where(s => s.Department.Id == staff.Department?.Id);
                
                List<Staff> staffList = new List<Staff>();
                foreach (Staff st in deptLeaderList)
                {
                    if (staff != null && !isGet1)
                    {
                        foreach (var department in staff.Departments)
                        {
                            if (st.Department?.Id == department.Id)
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
                    //std.StaffId = st.Id;
                    //std.StaffName = st.StaffProfile.Name;
                    //std.DepartmentId = st.Department.Id;
                    //std.DepartmentName = st.Department.Name;
                    //std.PositionName = st.StaffInfo.Position.Name;
                    //ABC_Rating staffRating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluationId && r.Staff.Id == st.Id).FirstOrDefault();
                    //if (staffRating != null)
                    //{
                    //    std.IsRated = staffRating.IsRated;
                    //    std.IsSupervisorRated = staffRating.IsSupervisorRated;
                    //    std.IsRatedString = std.IsRated == true ? "Đã đánh giá" : "Chưa đánh giá";
                    //    std.IsSupervisorRatedString = std.IsSupervisorRated == true ? "Đã đánh giá" : "Chưa đánh giá";
                    //    std.Record = staffRating.TotalSupervisorRecord.ToString("#");
                    //    std.Classification = staffRating.Classification;
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
                var deptLeaderList = session.Query<Staff>().Where(s => (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate) && s.StaffInfo.Position != null && (s.StaffInfo.WebUsers.Any(user => user.WebGroupId == Guid.Parse("00000000-0000-0000-0000-000000000004")) || s.StaffInfo.Position.LaQuanLy)).OrderBy(s => s.Department.OrderNumber);
                var x = deptLeaderList.Select(q => q.Id).ToList();
                Helper.ErrorLog(String.Join(", ", x.ToArray()), null);
                var currentStaffId = GetCurrentUser().Id;
                Staff staff = session.Query<Staff>().SingleOrDefault(s => s.Id == currentStaffId);
                List<Staff> staffList = new List<Staff>();
                //staffList = deptLeaderList.Where(s => staff.Departments.Any(d => d.Id == s.Department.Id)).ToList();
                foreach (Staff st in deptLeaderList)
                {
                    foreach(var department in staff.Departments)
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
                    //std.PositionName = st.StaffInfo.Position.Name;
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
                if (applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000002")
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
                    //if (applicationUser.Id != null)
                    //{
                    //    Guid staffId = new Guid(applicationUser.Id);
                    //    Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();
                    //    if (staff != null && staff.StaffInfo.Position != null)
                    //    {
                    //        ///Lấy trưởng đơn vị, trừ trưởng bộ môn
                    //        string positionName = staff.StaffInfo.Position.Name;
                    //        //bool check = !positionName.Contains("Phó") && !positionName.Contains("bộ môn") && !positionName.Contains("Hiệu") && !positionName.Contains("Tổ");
                    //        bool check = !positionName.Contains("Phó") && !positionName.Contains("Hiệu") && !positionName.Contains("Tổ");
                    //        result = check == true ? 1 : 0;
                    //    }
                    //}
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
        public int GetCreateEvaluationBoard(int year)
        {
            int result = 1;
            SessionManager.DoWork(session =>
            {
                List<ABC_EvaluationBoard> ev = new List<ABC_EvaluationBoard>();
                //Tạo năm
                ABC_EvaluationBoard evYear = new ABC_EvaluationBoard();
                evYear.Id = Guid.NewGuid();
                evYear.Month = 1;
                evYear.Year = year;
                evYear.Name = "Năm " + year.ToString();
                evYear.FromDate = new DateTime(year, 1, 1);
                evYear.ToDate = new DateTime(year, 12, 31);
                evYear.EvaluationBoardType = 1;
                ev.Add(evYear);
                //Tạo nửa đầu năm 
                for (int i = 1; i <= 2; i++)
                {
                    ABC_EvaluationBoard evSemester = new ABC_EvaluationBoard();
                    evSemester.Id = Guid.NewGuid();
                    evSemester.ABC_ParentEvaluationBoard = evYear;
                    evSemester.EvaluationBoardType = 2;
                    switch (i)
                    {
                        case 1:
                            {
                                evSemester.Name = "6 tháng đầu năm " + year.ToString();
                                evSemester.Month = 1;
                                evSemester.Year = year;
                                evSemester.FromDate = evYear.FromDate;
                                evSemester.ToDate = evSemester.FromDate.Value.AddMonths(6).AddDays(-1);
                                ev.Add(evSemester);

                                //Tạo tháng
                                for (int j = 0; j <= 5; j++)
                                {
                                    ABC_EvaluationBoard evMonth = new ABC_EvaluationBoard();
                                    evMonth.Id = Guid.NewGuid();
                                    evMonth.ABC_ParentEvaluationBoard = evSemester;
                                    evMonth.EvaluationBoardType = 3;
                                    evMonth.FromDate = evSemester.FromDate.Value.AddMonths(j);
                                    evMonth.ToDate = evMonth.FromDate.Value.AddMonths(1).AddDays(-1);
                                    evMonth.Name = "Tháng " + (j + 1).ToString() + "/" + year.ToString();
                                    evMonth.Month = j + 1;
                                    evMonth.Year = year;
                                    ev.Add(evMonth);

                                }
                            }
                            break;
                        case 2:
                            {
                                evSemester.Name = "6 tháng cuối năm " + year.ToString();
                                evSemester.Month = 7;
                                evSemester.Year = year;
                                evSemester.FromDate = evYear.FromDate.Value.AddMonths(6);
                                evSemester.ToDate = evYear.ToDate;
                                ev.Add(evSemester);

                                //Tạo tháng
                                for (int j = 6; j <= 11; j++)
                                {
                                    ABC_EvaluationBoard evMonth = new ABC_EvaluationBoard();
                                    evMonth.Id = Guid.NewGuid();
                                    evMonth.ABC_ParentEvaluationBoard = evSemester;
                                    evMonth.EvaluationBoardType = 3;
                                    evMonth.FromDate = evSemester.FromDate.Value.AddMonths(j);
                                    evMonth.ToDate = evMonth.FromDate.Value.AddMonths(1).AddDays(-1);
                                    evMonth.Name = "Tháng " + (j + 1).ToString() + "/" + year.ToString();
                                    evMonth.Month = j + 1;
                                    evMonth.Year = year;
                                    ev.Add(evMonth);
                                }
                            }
                            break;
                    }
                }
                foreach (ABC_EvaluationBoard e in ev)
                {
                    session.Save(e);
                }

            });
            return result;
        }
        [Authorize]
        [Route("")]
        public bool GetCheckExistEvaluationBoard(int year)
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
            //Nếu là true: có tồn tại kế hoạch chưa khóa
            //Sửa lại: true khi chưa có bản đánh giá con nào được cấp trên khóa
            SessionManager.DoWork(session =>
            {
                //Lấy kỳ đánh giá
                ABC_EvaluationBoard eb = session.Query<ABC_EvaluationBoard>().Where(e => e.Id == evaluationId).SingleOrDefault();

                //nếu là hiệu trưởng thì không cần cấp trên đánh giá
                string tenChucVu = session.Query<Staff>().Where(q => q.Id == staffId).Select(q => q.StaffInfo.Position.Name).SingleOrDefault()?.ToLower();
                if (tenChucVu == "hiệu trưởng" || tenChucVu == "quyền hiệu trưởng")
                {
                    result = false;
                }
                else if (eb.EvaluationBoardType == 1 || (eb.EvaluationBoardType == 2 && staffType != 1)) //Đánh giá 6 tháng của GV ko cần xét
                {
                    //Nếu là đánh giá 6 tháng phải kiểm tra có ít nhất 1 tháng đã được cấp trên khóa
                    //Nếu là đánh giá năm phải kiểm tra ít nhất 1 đánh giá 6 tháng trong năm đã được cấp trên khóa
                    var listRating = session.Query<ABC_Rating>().Where(r =>
                    //Tất cả rating là con 
                    r.ABC_EvaluationBoard.ABC_ParentEvaluationBoard.Id == evaluationId
                    && r.Staff.Id == staffId
                    //&& r.Department.Id == departmentId //không xét bộ phận vì nhân viên có thể đổi bộ phận
                    ).ToList();
                    //Kiểm tra nếu có bất kỳ đánh giá tháng nào chưa đc khóa
                    //Đã sửa lại: kiểm tra đã có bản đánh giá con dc khóa
                    result = !listRating.Any(c => c.IsSupervisorRated == true);
                    if (listRating.Count() == 0)
                        result = true;
                }
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
                if (applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000005") //trường hợp kiêm nhiệm (không có ThongTinNhanVien)
                {
                    result.Id = Guid.Empty; 
                    result.DepartmentId = new Guid(applicationUser.DepartmentId);
                    var department = session.Query<Department>().SingleOrDefault(q => q.Id == result.DepartmentId);
                    result.StaffType = GetStaffTypeByDepartment(department);
                }
                else if (applicationUser.Id != null && applicationUser.Id != "")
                {
                    Guid staffId = new Guid(applicationUser.Id);
                    Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();
                    if (staff != null)
                    {
                        result.Id = staff.Id;
                        result.DepartmentId = staff.Department.Id;
                        //result.StaffType = staff.StaffInfo.StaffType != null ? (staff.StaffInfo.StaffType.Name.Contains("Giảng") ? 1 : 2) : 0;

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
            if (staff.Department != null && staff.Department.ManageCode != null)
            {
                if (!staff.StaffInfo.StaffType.ManageCode.Contains("NV") && (staff.Department.ManageCode.StartsWith("G") || staff.Department.ManageCode.StartsWith("B")) && !staff.Department.Name.ToLower().StartsWith("ban giám hiệu"))
                {
                    result = 1;
                }
                else result = 2;
            }
            else result = 0;
            return result;
        }
        //1: giảng viên, trưởng khoa, trưởng bộ môn
        //2: nhân viên, trưởng phòng
        public int GetStaffTypeByDepartment(Department department)
        {
            int result = 0;
            if (department.ManageCode.StartsWith("G") || department.ManageCode.StartsWith("B"))
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
            try
            {
                List<ABC_EvaluationBoardDTO> result = new List<ABC_EvaluationBoardDTO>();
                SessionManager.DoWork(session =>
                {
                    var temp = session.Query<ABC_EvaluationBoard>().OrderBy(eb => eb.FromDate).ThenBy(e => e.EvaluationBoardType);
                    foreach (ABC_EvaluationBoard eb in temp)
                    {
                        ABC_EvaluationBoardDTO ebd = new ABC_EvaluationBoardDTO();
                        ebd.Id = eb.Id;
                        ebd.Month = eb.Month;
                        ebd.Year = eb.Year;
                        ebd.FromDate = eb.FromDate;
                        ebd.ToDate = eb.ToDate;
                        ebd.EvaluationBoardType = eb.EvaluationBoardType;
                        if (eb.EvaluationBoardType == 2)
                            ebd.Name = "-- " + eb.Name;
                        else if (eb.EvaluationBoardType == 3)
                            ebd.Name = "---- " + eb.Name;
                        else
                            ebd.Name = eb.Name;
                        result.Add(ebd);
                    }
                });
                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_EvaluationBoardApiController/GetEvaluationBoardList", ex);
                throw ex;
            }
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
                var temp = factory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(new Guid(applicationUser.UserId));
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
                    std.StaffType = st.StaffInfo.StaffType != null ? (st.StaffInfo.StaffType.Name.Contains("Giảng") ? 1 : 2) : 0;
                    ABC_Rating staffRating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluation.Id && r.Staff.Id == st.Id).FirstOrDefault();
                    if (staffRating != null)
                    {
                        std.IsRated = staffRating.IsRated;
                        std.IsSupervisorRated = staffRating.IsSupervisorRated;
                        std.Record = ConvertDecimalToString1Place(staffRating.TotalSupervisorRecord);
                        std.Classification = evaluation.EvaluationBoardType != 3 ? staffRating.Classification : "";
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
                if (applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000001" || applicationUser.WebGroupId.ToUpper() == "05A1BF24-BD1C-455F-96F6-7C4237F4659E")
                {
                    result = 1;
                }
                //Nếu là admin đơn vị
                else if (applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000004" || applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000005")
                {
                    result = 2;
                }

            });
            return result;
        }

        public string ConvertDecimalToString1Place(decimal value)
        {
            return (Math.Round((value * 10), 0) / 10).ToString();
        }
    }
}
