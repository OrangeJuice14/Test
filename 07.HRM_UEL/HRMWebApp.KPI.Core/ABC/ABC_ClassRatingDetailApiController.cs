using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.Core.Predefined;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using System.Web;
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;
using AutoMapper;

namespace HRMWebApp.KPI.Core.ABC
{
    public class ABC_ClassRatingDetailApiController : ApiController
    {
        public ABC_ClassRatingDetailApiController()
        {
            Mapper.Initialize(cfg =>
                cfg.CreateMap<ABC_ClassRating, ABC_ClassRatingDTO>()
                    .ForMember(dest => dest.StaffType, opt => opt.Ignore()));
        }

        [Authorize]
        [Route("")]
        public ABC_ClassRatingDTO GetClassRatingDetail(Guid evaluationId, Guid staffId, Guid supervisorId, Guid departmentId, byte isAdminRating)
        {
            try
            {
                ABC_ClassRatingDTO result = new ABC_ClassRatingDTO();
                SessionManager.DoWork(session =>
                {
                    var eb = session.Query<ABC_ClassEvaluationBoard>().SingleOrDefault(q => q.Id == evaluationId);
                    var classRating = session.Query<ABC_ClassRating>().SingleOrDefault(q => q.Staff.Id == staffId && q.ABC_ClassEvaluationBoard.Id == evaluationId);
                    if (classRating != null)
                    {
                        result = classRating.Map<ABC_ClassRatingDTO>();
                        result.StaffName = classRating.Staff?.StaffProfile?.Name;
                        result.WebGroupId = classRating.Staff?.StaffInfo?.WebUser?.WebGroupId ?? Guid.Empty;
                        result.StaffPosition = classRating.Staff?.StaffInfo?.Position?.Name ?? classRating.Staff?.Title?.Name ?? "";
                    }
                    else
                    {
                        Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();

                        result.Id = Guid.Empty;
                        result.StaffId = staffId;
                        result.ABC_ClassEvaluationBoardId = evaluationId;

                        result.DepartmentId = departmentId;

                        result.StaffName = staff?.StaffProfile?.Name;
                        result.WebGroupId = staff?.StaffInfo?.WebUser?.WebGroupId ?? Guid.Empty;
                        result.StaffPosition = staff?.StaffInfo?.Position?.Name ?? staff?.Title?.Name ?? "";
                        result.DepartmentName = staff?.Department?.Name;
                    }

                    result.IsValid = true;
                    result.Month = eb.Month;
                    result.Year = eb.Year;
                    result.SupervisorType = isAdminRating;
                    if (supervisorId != Guid.Empty || isAdminRating > 0)
                    {
                        result.IsSupervisor = true;
                    }
                });
                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_ClassRatingDetailApi/GetClassRatingDetail", ex);
                throw;
            }
        }

        [Authorize]
        [Route("")]
        public ABC_ClassRatingDTO PutClassRatingDetail(ABC_ClassRatingDTO objDTO)
        {
            try
            {
                ABC_ClassRatingDTO result = new ABC_ClassRatingDTO();
                SessionManager.DoWork(session =>
                {
                    var classRating = session.Query<ABC_ClassRating>().SingleOrDefault(q => q.Id == objDTO.Id);
                    if (classRating == null)
                    {
                        classRating = new ABC_ClassRating();
                        classRating.Id = Guid.NewGuid();
                        classRating.Staff = new Staff() { Id = objDTO.StaffId };
                        classRating.ABC_ClassEvaluationBoard = new ABC_ClassEvaluationBoard() { Id = objDTO.ABC_ClassEvaluationBoardId };
                        classRating.Department = new Department() { Id = objDTO.DepartmentId };
                    }
                    if (objDTO.SupervisorType == (int)SupervisorTypes.BGH)
                    {
                        classRating.IsRatedThird = true;
                        classRating.DateRatedThird = DateTime.Now;
                        classRating.ClassificationThird = objDTO.ClassificationThird;
                        classRating.NoteThird = objDTO.NoteThird;
                    }
                    else if (objDTO.SupervisorType == (int)SupervisorTypes.TruongDonVi)
                    {
                        classRating.IsRatedSecond = true;
                        classRating.DateRatedSecond = DateTime.Now;
                        classRating.ClassificationSecond = objDTO.ClassificationSecond;
                        classRating.NoteSecond = objDTO.NoteSecond;
                    }
                    else if (!objDTO.IsSupervisor)
                    {
                        classRating.IsRated = true;
                        classRating.DateRated = DateTime.Now;
                        classRating.Classification = objDTO.Classification;
                    }
                    session.SaveOrUpdate(classRating);
                });
                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_ClassRatingDetailApi/PutClassRatingDetail", ex);
                throw;
            }
        }

        [Authorize]
        [Route("")]
        public int PutListClassRatingDetail(List<ABC_ClassRatingDTO> listClassRatingDTO)
        {
            int result = 0;

            try
            {
                foreach (var objDTO in listClassRatingDTO)
                {
                    SessionManager.DoWork(session =>
                    {
                        var classRating = session.Query<ABC_ClassRating>().SingleOrDefault(q => q.Id == objDTO.Id);
                        if (classRating == null)
                        {
                            classRating = new ABC_ClassRating();
                            classRating.Id = Guid.NewGuid();
                            classRating.Staff = new Staff() { Id = objDTO.StaffId };
                            classRating.ABC_ClassEvaluationBoard = new ABC_ClassEvaluationBoard() { Id = objDTO.ABC_ClassEvaluationBoardId };
                            classRating.Department = new Department() { Id = objDTO.DepartmentId };
                        }
                        if (objDTO.SupervisorType == (int)SupervisorTypes.BGH)
                        {
                            classRating.IsRatedThird = true;
                            classRating.DateRatedThird = DateTime.Now;
                            classRating.ClassificationThird = objDTO.ClassificationThird;
                            classRating.NoteThird = objDTO.NoteThird;
                        }
                        else if (objDTO.SupervisorType == (int)SupervisorTypes.TruongDonVi)
                        {
                            classRating.IsRatedSecond = true;
                            classRating.DateRatedSecond = DateTime.Now;
                            classRating.ClassificationSecond = objDTO.ClassificationSecond;
                            classRating.NoteSecond = objDTO.NoteSecond;
                        }
                        else if (!objDTO.IsSupervisor)
                        {
                            classRating.IsRated = true;
                            classRating.DateRated = DateTime.Now;
                            classRating.Classification = objDTO.Classification;
                        }
                        session.SaveOrUpdate(classRating);
                    });
                }
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                Helper.ErrorLog("ABC_ClassRatingDetailApi/PutListClassRatingDetail", ex);
                throw;
            }
            return result;
        }
    }
}
