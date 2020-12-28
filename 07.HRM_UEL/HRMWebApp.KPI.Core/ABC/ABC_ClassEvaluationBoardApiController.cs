using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.Controllers;
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

namespace HRMWebApp.KPI.Core.ABC
{
    public class ABC_ClassEvaluationBoardApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_ClassEvaluationBoardDTO> GetList() //tab đánh giá cấp dưới
        {
            List<ABC_ClassEvaluationBoardDTO> result = new List<ABC_ClassEvaluationBoardDTO>();
            SessionManager.DoWork(session =>
            {
                ABC_EvaluationBoardApiController evaluationBoardController = new ABC_EvaluationBoardApiController();
                StaffDTO staff = evaluationBoardController.GetCurrentUser();
                var planList = session.Query<ABC_ClassEvaluationBoard>();
                foreach (var pl in planList)
                {
                    ABC_ClassEvaluationBoardDTO pd = pl.Map<ABC_ClassEvaluationBoardDTO>();
                    pd.ParentId = pl.ABC_ParentClassEvaluationBoard?.Id;
                    result.Add(pd);
                }
            });
            result = result.OrderBy(r => r.FromDate).ToList();
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_ClassEvaluationBoardDTO> GetListStaff() //tab đánh giá bản thân
        {
            List<ABC_ClassEvaluationBoardDTO> result = new List<ABC_ClassEvaluationBoardDTO>();
            SessionManager.DoWork(session =>
            {
                ABC_EvaluationBoardApiController evaluationBoardController = new ABC_EvaluationBoardApiController();
                StaffDTO staff = evaluationBoardController.GetCurrentUser();
                var planList = session.Query<ABC_ClassEvaluationBoard>();
                foreach (var pl in planList)
                {
                    ABC_ClassEvaluationBoardDTO pd = pl.Map<ABC_ClassEvaluationBoardDTO>();
                    pd.ParentId = pl.ABC_ParentClassEvaluationBoard?.Id;
                    ABC_ClassRating rating = session.Query<ABC_ClassRating>().Where(r => r.ABC_ClassEvaluationBoard.Id == pl.Id && r.Staff.Id == staff.Id).FirstOrDefault();
                    if (rating != null)
                    {
                        pd.IsRatedSecond = rating.IsRatedSecond ? "Đã đánh giá" : "";
                        pd.IsRatedThird = rating.IsRatedThird ? "Đã đánh giá" : "";
                    }
                    result.Add(pd);
                }
            });
            result = result.OrderBy(r => r.FromDate).ToList();
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_ClassEvaluationBoardDTO> GetListEvaluationManage() //Quản lý đánh giá ABC
        {
            List<ABC_ClassEvaluationBoardDTO> result = new List<ABC_ClassEvaluationBoardDTO>();
            SessionManager.DoWork(session =>
            {
                var planList = session.Query<ABC_ClassEvaluationBoard>();

                foreach (var pl in planList)
                {
                    ABC_ClassEvaluationBoardDTO pd = pl.Map<ABC_ClassEvaluationBoardDTO>();
                    pd.ParentId = pl.ABC_ParentClassEvaluationBoard?.Id;
                    result.Add(pd);
                }
            });
            result = result.OrderBy(r => r.FromDate).ToList();
            return result;
        }

        [Authorize]
        [Route("")]
        public List<ABC_ClassEvaluationBoardDTO> GetEvaluationBoardList() //lấy danh sách năm
        {
            List<ABC_ClassEvaluationBoardDTO> result = new List<ABC_ClassEvaluationBoardDTO>();
            SessionManager.DoWork(session =>
            {
                var temp = session.Query<ABC_ClassEvaluationBoard>().OrderBy(eb => eb.FromDate);
                foreach (ABC_ClassEvaluationBoard eb in temp)
                {
                    ABC_ClassEvaluationBoardDTO ebd = new ABC_ClassEvaluationBoardDTO();
                    ebd.Id = eb.Id;
                    ebd.Month = eb.Month;
                    ebd.Year = eb.Year;
                    ebd.FromDate = eb.FromDate;
                    ebd.ToDate = eb.ToDate;
                    ebd.Name = eb.Name;
                    result.Add(ebd);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_ClassRatingDTO> GetListStaffEvaluationBySupervisor(Guid classEvaluationId)
        {
            List<ABC_ClassRatingDTO> result = new List<ABC_ClassRatingDTO>();
            SessionManager.DoWork(session =>
            {
                var classEvaluationBoard = session.Query<ABC_ClassEvaluationBoard>().Where(e => e.Id == classEvaluationId).SingleOrDefault();
                var evaluationFromDate = classEvaluationBoard.FromDate;
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                //Admin đơn vị ủy quyền
                if (applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000005")
                {
                    //Lấy đơn vị quản lý của admin ủy quyền
                    Guid departmentId = applicationUser.DepartmentId != null ? new Guid(applicationUser.DepartmentId) : Guid.Empty;
                    //Lấy trưởng đơn vị
                    Staff staff = session.Query<Staff>().Where(s =>
                    s.Department.Id == departmentId
                    && s.StaffProfile.GCRecord == null
                    && s.StaffInfo.Position != null
                    && s.StaffInfo.Position.LaQuanLy
                    ).FirstOrDefault();

                    Guid staffId = staff != null ? staff.Id : Guid.Empty;
                    //Lấy staff trừ trưởng đơn vị
                    var temp = session.Query<Staff>().Where(s => s.Department.Id == departmentId && s.Id != staffId && s.StaffProfile.GCRecord == null && (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate));
                    temp = temp.OrderByDescending(s => s.StaffInfo.Position.HSPCChucVu).ThenBy(s => s.Title.Name).ThenBy(s => s.StaffProfile.FirstName);

                    foreach (Staff st in temp)
                    {
                        ABC_ClassRatingDTO std = new ABC_ClassRatingDTO();
                        std = ParseEBStaff(st, session, classEvaluationId);
                        result.Add(std);
                    }
                }
                //Trường hợp user là trưởng đơn vị, group là admin đơn vị hồ sơ thông tin lương
                else if (applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000004")
                {
                    Guid staffId = new Guid(applicationUser.Id);
                    Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();
                    var temp = session.Query<Staff>().Where(s => s.Department.Id == staff.Department.Id && s.Id != staff.Id && s.StaffProfile.GCRecord == null && (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate));
                    temp = temp.OrderByDescending(s => s.StaffInfo.Position.HSPCChucVu).ThenBy(s => s.Title.Name).ThenBy(s => s.StaffProfile.FirstName);

                    foreach (Staff st in temp)
                    {
                        ABC_ClassRatingDTO std = new ABC_ClassRatingDTO();
                        std = ParseEBStaff(st, session, classEvaluationId);
                        result.Add(std);
                    }
                }
            });
            return result;
        }

        public ABC_ClassRatingDTO ParseEBStaff(Staff st, NHibernate.ISession session, Guid evaluationId)
        {
            ABC_ClassRatingDTO std = new ABC_ClassRatingDTO();
            std.Id = Guid.NewGuid();
            std.ABC_ClassEvaluationBoardId = evaluationId;
            std.StaffName = st.StaffProfile.Name;
            std.StaffId = st.Id;
            std.DepartmentId = st.Department.Id;
            std.StaffPosition = st?.StaffInfo?.Position?.Name ?? st?.Title?.Name ?? "";
            std.SupervisorType = GetSupervisorType(std.StaffId);

            ABC_ClassRating staffRating = session.Query<ABC_ClassRating>().Where(r => r.ABC_ClassEvaluationBoard.Id == evaluationId && r.Staff.Id == st.Id).FirstOrDefault();
            if (staffRating != null)
            {
                std.Id = staffRating.Id;
                std.IsRated = staffRating.IsRated;
                std.IsRatedSecond = staffRating.IsRatedSecond;
                std.IsRatedThird = staffRating.IsRatedThird;
                std.IsRatingLocked = staffRating.IsRatingLocked;
                std.Classification = staffRating.Classification;
                std.ClassificationSecond = staffRating.ClassificationSecond;
                std.NoteSecond = staffRating.NoteSecond;
                std.ClassificationThird = staffRating.ClassificationThird;
                std.NoteThird = staffRating.NoteThird;
            }
            return std;
        }

        [Authorize]
        [Route("")]
        public byte GetSupervisorType(Guid staffId)
        {
            byte result = 0;
            SessionManager.DoWork(session =>
            {
                var webgroupid = session.Query<DB.Entities.WebUser>().Where(q => q.StaffInfo.Id == staffId).Select(q => q.WebGroupId).FirstOrDefault();
                if (webgroupid != null)
                {
                    //trưởng đơn vị || trưởng đơn vị ủy quyền
                    if (webgroupid == new Guid("00000000-0000-0000-0000-000000000004") || webgroupid == new Guid("00000000-0000-0000-0000-000000000005"))
                    {
                        result = 1;
                    }
                    else if (webgroupid == new Guid("00000000-0000-0000-0000-000000000002"))
                    {
                        result = 2;
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<DepartmentDTO> GetDanhSachDonViDuocPhanQuyenChoHieuTruong(Guid staffId)
        {
            List<DepartmentDTO> result = new List<DepartmentDTO>();
            SessionManager.DoWork(session =>
            {
                var staff = session.Query<Staff>().Where(q => q.Id == staffId).SingleOrDefault();
                if (staff != null)
                {
                    foreach (var department in staff.Departments)
                    {
                        var x = department.Map<DepartmentDTO>();
                        result.Add(x);
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_ClassRatingDTO> GetListStaffEvaluationByDepartment(Guid classEvaluationId, Guid departmentId)
        {
            List<ABC_ClassRatingDTO> result = new List<ABC_ClassRatingDTO>();
            SessionManager.DoWork(session =>
            {
                var classEvaluationBoard = session.Query<ABC_ClassEvaluationBoard>().Where(e => e.Id == classEvaluationId).SingleOrDefault();
                var evaluationFromDate = classEvaluationBoard.FromDate;
                //Trường hợp user là BGH
                var temp = session.Query<Staff>().Where(s => s.Department.Id == departmentId && s.StaffProfile.GCRecord == null && (s.StaffStatus.NoLongerWork == 0 || s.InactivityDate > evaluationFromDate));
                temp = temp.OrderByDescending(s => s.StaffInfo.Position.HSPCChucVu).ThenBy(s => s.Title.Name).ThenBy(s => s.StaffProfile.FirstName);

                foreach (Staff st in temp)
                {
                    ABC_ClassRatingDTO std = new ABC_ClassRatingDTO();
                    std = ParseEBStaff(st, session, classEvaluationId);
                    result.Add(std);
                }
            });
            return result;
        }
    }
}
