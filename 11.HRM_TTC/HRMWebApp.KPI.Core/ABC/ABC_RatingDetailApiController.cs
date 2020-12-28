using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.Core.Controllers;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Globalization;
using Newtonsoft.Json;
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;
using HRMWebApp.KPI.Core.Helpers;
using System.Diagnostics;
using HRMWeb_Business.Predefined;
using HRMWebApp.KPI.Core.DTO.ABC;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_RatingDetailApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public ABC_RatingDTO GetRatingDetail(Guid evaluationId, Guid staffId, Guid supervisorId, Guid departmentId, byte isAdminRating)
        {
            ABC_RatingDTO result = new ABC_RatingDTO(staffId);
            SessionManager.DoWork(session =>
            {
                ABC_EvaluationBoardApiController ebController = new ABC_EvaluationBoardApiController();
                ABC_EvaluationBoard eb = session.Query<ABC_EvaluationBoard>().Where(e => e.Id == evaluationId).SingleOrDefault();
                result.Quater = eb.Quater;
                result.Year = eb.Year;
                result.IsValid = true;

                //Nếu admin=1 là BGH, 2 là admin
                result.IsAdmin = isAdminRating;
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);

                if (staffId == Guid.Empty)
                {
                    staffId = new Guid(applicationUser.Id);
                }

                if (supervisorId != Guid.Empty || isAdminRating > 0 || new Guid(applicationUser.WebGroupId) == WebGroupConst.TruongPhongUyQuyenID)
                {
                    result.IsSupervisor = true;
                }
                Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();

                //0: Không xác định
                //1: GV
                //2: NV
                int staffType = 0;
                //staffType = ebController.GetStaffType(staff);
                staffType = ebController.GetStaffTypeForEvaluationBoard(staff, evaluationId, session);
                result.StaffType = staffType;
                if (staffType != 0)
                {
                    result.StaffId = staff.Id;
                    if (staff.StaffInfo?.WebUsers != null)
                    {
                        try
                        {
                            result.WebGroupId = staff.StaffInfo.WebUsers.First(q => q.WebGroupId == WebGroupConst.TruongPhongID).WebGroupId;
                        }
                        catch
                        {
                            result.WebGroupId = staff.StaffInfo.WebUsers.OrderBy(q => q.WebGroupId).First().WebGroupId;
                        }
                    }
                    else result.WebGroupId = Guid.Empty;

                    ABC_Rating rating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluationId && r.Staff.Id == staffId).SingleOrDefault();

                    if (rating == null)
                    {
                        rating = new ABC_Rating();
                        rating.Id = Guid.NewGuid();
                        rating.IsRated = false;
                        rating.IsSupervisorRated = false;
                        rating.IsRatingLocked = false;
                        rating.Staff = new Staff() { Id = staffId };

                        if (departmentId != Guid.Empty)
                        {
                            rating.Department = new Department() { Id = departmentId };
                        }
                        else
                        {
                            rating.Department = new Department() { Id = staff.Department.Id };
                        }

                        rating.ABC_EvaluationBoard = new ABC_EvaluationBoard() { Id = evaluationId };

                        ABC_LockedRatingDepartment lockedDept = ebController.GetLockedRatingDepartment(rating.ABC_EvaluationBoard.Id, rating.Department.Id);
                        if (lockedDept != null)
                            rating.IsRatingLocked = lockedDept.Status;
                        session.Save(rating);

                        if (staffType != 0)
                            result = GetCriterions(result, session, staffType, rating);


                        //if (staffType == 1) // giảng viên
                        //{
                        //    var criterions = session.Query<ABC_Criterion>().Where(c => c.RatingType == 1).OrderBy(c => c.OrderNumber);

                        //    foreach (ABC_Criterion cri in criterions)
                        //    {
                        //        ABC_RatingDetail r = ParseNewRatingDetail(cri, rating);
                        //        session.Save(r);
                        //    }
                        //}

                        //if (staffType == 2) // nhân viên
                        //{
                        //    var criterions = session.Query<ABC_Criterion>().Where(c => c.RatingType == 2).OrderBy(c => c.OrderNumber);

                        //    foreach (ABC_Criterion cri in criterions)
                        //    {
                        //        ABC_RatingDetail r = ParseNewRatingDetail(cri, rating);
                        //        session.Save(r);
                        //    }
                        //}
                    }
                    else
                    {
                        ABC_LockedRatingDepartment lockedDept = ebController.GetLockedRatingDepartment(rating.ABC_EvaluationBoard.Id, rating.Department.Id);
                        if (lockedDept != null)
                            rating.IsRatingLocked = lockedDept.Status;

                        if (staffType != 0)
                            result = GetCriterions2(result, session, staffType, rating);

                        foreach (ABC_RatingDetailDTO rdDTO in result.ABC_RatingDetailDTOs)
                        {
                            var ratingDetail = session.Query<ABC_RatingDetail>().Where(r => (r.ABC_Rating.Id == rating.Id) && (r.ABC_Criterion.Id == rdDTO.ABC_CriterionDTO.Id)).SingleOrDefault();
                            rdDTO.StaffRecord = ratingDetail.StaffRecord;
                            rdDTO.SupervisorRecord = ratingDetail.SupervisorRecord;
                        }

                        result.StaffNote = rating.StaffNote;
                        result.SupervisorNote = rating.SupervisorNote;

                        //var ratingDetails = session.Query<ABC_RatingDetail>().Where(r => r.ABC_Rating.Id == rating.Id).OrderBy(c => c.ABC_Criterion.OrderNumber);
                        //foreach (ABC_RatingDetail ratingDetail in ratingDetails)
                        //{
                        //    //if (result.IsSupervisor == false && rating.IsRated == false)
                        //    //{
                        //    //    ParseRatingDetail(ratingDetail, rating.Staff.Id, rating.Department.Id, evaluationId, session);
                        //    //    session.SaveOrUpdate(ratingDetail);
                        //    //}

                        //    //ABC_RatingDetailDTO rDetailDTO = ParseRatingDetailDTO(ratingDetail);
                        //    //result.ABC_RatingDetailDTOs.Add(rDetailDTO);
                        //}
                    }

                    result.ABC_RatingLevelDTOs = new List<ABC_RatingLevelDTO>();
                    var listRatingLevel = session.Query<ABC_RatingLevel>().OrderBy(q => q.OrderNumber).ToList();
                    foreach (var item in listRatingLevel)
                    {
                        ABC_RatingLevelDTO rlDTO = new ABC_RatingLevelDTO();
                        rlDTO.Id = item.Id;
                        rlDTO.Name = item.Name;
                        rlDTO.Color = item.Color;
                        result.ABC_RatingLevelDTOs.Add(rlDTO);
                    }
                    result.Id = rating.Id;
                    result.IsRated = rating.IsRated;
                    result.IsSupervisorRated = rating.IsSupervisorRated;
                    result.IsRatingLocked = rating.IsRatingLocked;
                    
                }
                else
                    result.IsValid = false;
            });
            return result;
        }

        public ABC_RatingDTO GetCriterions(ABC_RatingDTO result, NHibernate.ISession session, int staffType, ABC_Rating rating)
        {
            if (staffType == 1)
            {
                // giảng viên
                var criterionRatingType = session.Query<ABC_Criterion_RatingType>()
                                                     .Where(c => c.ABC_RatingType.Type == 1)
                                                     .OrderBy(c => c.ABC_Criterion.OrderNumber).ToList();
                var criterions1 = new List<ABC_Criterion>();
                foreach (var item in criterionRatingType)
                {
                    criterions1.Add(item.ABC_Criterion);
                }
                foreach (ABC_Criterion cri in criterions1)
                {
                    ABC_RatingDetail r = ParseNewRatingDetail(cri, rating);
                    session.Save(r);
                    ABC_RatingDetailDTO rd = ParseRatingDetailDTO(cri, null, session);
                    rd.Id = r.Id;
                    result.ABC_RatingDetailDTOs.Add(rd);
                }
            }

            if (staffType == 2)
            {
                // nhân viên
                var criterionRatingType = session.Query<ABC_Criterion_RatingType>()
                                                     .Where(c => c.ABC_RatingType.Type == 2)
                                                     .OrderBy(c => c.ABC_Criterion.OrderNumber).ToList();
                var criterions2 = new List<ABC_Criterion>();
                foreach (var item in criterionRatingType)
                {
                    criterions2.Add(item.ABC_Criterion);
                }
                foreach (ABC_Criterion cri in criterions2)
                {
                    ABC_RatingDetail r = ParseNewRatingDetail(cri, rating);
                    session.Save(r);
                    ABC_RatingDetailDTO rd = ParseRatingDetailDTO(cri, null, session);
                    rd.Id = r.Id;
                    result.ABC_RatingDetailDTOs.Add(rd);
                }
            }
            return result;
        }

        public ABC_RatingDTO GetCriterions2(ABC_RatingDTO result, NHibernate.ISession session, int staffType, ABC_Rating rating)
        {
            if (staffType == 1)
            {
                // giảng viên
                var details = session.Query<ABC_RatingDetail>().Where(q => q.ABC_Rating.Id == rating.Id).OrderBy(q => q.ABC_Criterion.OrderNumber);
                foreach (var detail in details)
                {
                    ABC_RatingDetailDTO rd = ParseRatingDetailDTO(detail.ABC_Criterion, detail, session);
                    result.ABC_RatingDetailDTOs.Add(rd);
                }
            }

            if (staffType == 2)
            {
                // nhân viên
                var details = session.Query<ABC_RatingDetail>().Where(q => q.ABC_Rating.Id == rating.Id).OrderBy(q => q.ABC_Criterion.OrderNumber);
                foreach (var detail in details)
                {
                    ABC_RatingDetailDTO rd = ParseRatingDetailDTO(detail.ABC_Criterion, detail, session);
                    result.ABC_RatingDetailDTOs.Add(rd);
                }
            }

            return result;
        }

        public ABC_RatingDetailDTO ParseRatingDetailDTO(ABC_Criterion cri, ABC_RatingDetail detail, NHibernate.ISession session)
        {
            var rdDTO = new ABC_RatingDetailDTO();
            rdDTO.Id = detail?.Id ?? Guid.NewGuid();
            rdDTO.ABC_CriterionDTO.Id = cri.Id;
            rdDTO.ABC_CriterionDTO.Name = cri.Name;
            rdDTO.ABC_CriterionDTO.CriterionDetail = cri.CriterionDetail;
            rdDTO.ABC_CriterionDTO.Methods = cri.Methods;
            rdDTO.ABC_CriterionDTO.Percents = cri.Percents;
            rdDTO.ABC_CriterionDTO.OrderNumber = cri.OrderNumber;
            rdDTO.ABC_CriterionDTO.IsNotVisibleInEvaluationBoardType = cri.IsNotVisibleInEvaluationBoardType;
            rdDTO.RatingLevelDTOs = new List<ABC_RatingLevelDTO>();
            foreach (var item in cri.RatingLevels)
            {
                var obj = session.Query<ABC_Criterion_RatingLevel>()
                    .Where(q => q.ABC_Criterion.Id == cri.Id && q.ABC_RatingLevel.Id == item.Id).SingleOrDefault();
                var ratingLevelDTO = new ABC_RatingLevelDTO();
                ratingLevelDTO.Id = obj.ABC_RatingLevel.Id;
                ratingLevelDTO.Name = obj.ABC_RatingLevel.Name;
                ratingLevelDTO.Description = obj.Description;
                ratingLevelDTO.CriterionId = obj.ABC_Criterion.Id;
                rdDTO.RatingLevelDTOs.Add(ratingLevelDTO);
            }
            return rdDTO;
        }

        public ABC_RatingDetail ParseNewRatingDetail(ABC_Criterion cri, ABC_Rating rating)
        {
            ABC_RatingDetail rDetail = new ABC_RatingDetail();
            rDetail.Id = Guid.NewGuid();
            rDetail.ABC_Rating = new ABC_Rating() { Id = rating.Id };
            rDetail.StaffRecord = Guid.Empty;
            rDetail.SupervisorRecord = Guid.Empty;
            rDetail.ABC_Criterion = cri;
            return rDetail;
        }

        public string ConvertNumberToAlphabet(int num)
        {
            string result = "";
            if (num > 0 && num < 27)
                result = Convert.ToChar(num + 64).ToString().ToLower();
            return result;
        }

        public ABC_RatingDetailDTO ParseRatingDetailDTO(ABC_RatingDetail ratingDetail)
        {
            ABC_RatingDetailDTO rDetailDTO = new ABC_RatingDetailDTO();
            rDetailDTO.Id = ratingDetail.Id;
            rDetailDTO.StaffRecord = ratingDetail.StaffRecord;
            rDetailDTO.SupervisorRecord = ratingDetail.SupervisorRecord;
            return rDetailDTO;
        }

        public ABC_RatingDetailDTO ParseNewRatingDetailDTO(ABC_RatingDetail rDetail, ABC_Criterion cri)
        {
            ABC_RatingDetailDTO rDetailDTO = new ABC_RatingDetailDTO();
            rDetailDTO.Id = rDetail.Id;
            rDetailDTO.StaffRecord = rDetail.StaffRecord;
            rDetailDTO.SupervisorRecord = rDetail.SupervisorRecord;
            return rDetailDTO;
        }

        public static void ParseRatingDetail(ABC_RatingDetail ratingDetail, Guid staffId, Guid departmentId, Guid evaluationId, NHibernate.ISession session)
        {
            var listSamePlanDetail = session.Query<ABC_RatingDetail>().Where(r =>
            r.ABC_Criterion.Id == ratingDetail.ABC_Criterion.Id
            //Ðánh giá của staff
            && r.ABC_Rating.Staff.Id == staffId
            //Ðánh giá của đơn vị staff
            && r.ABC_Rating.Department.Id == departmentId
            && r.ABC_Rating.IsRated == true
            && r.ABC_Rating.IsSupervisorRated == true
            );
        }

        [Authorize]
        [Route("")]
        public ABC_RatingDTO Put(ABC_RatingDTO obj)
        {
            SessionManager.DoWork(session =>
            {
                obj = PutTemp(obj, session);
                SaveRatingResult(obj, session);
            });
            return obj;
        }

        [Authorize]
        [Route("")]
        public void SaveRatingResult(ABC_RatingDTO obj, NHibernate.ISession session)
        {
            ABC_Rating ratingResult = session.Query<ABC_Rating>().SingleOrDefault(r => r.Id == obj.Id);
            ratingResult.StaffNote = obj.StaffNote;
            ratingResult.SupervisorNote = obj.SupervisorNote;
            session.Update(ratingResult);
        }

        [Authorize]
        [Route("")]
        public ABC_RatingDTO PutTemp(ABC_RatingDTO obj, NHibernate.ISession session)
        {
            decimal sumcheck = 0;
            decimal numberoftype2 = 0;
            //bool isFirstCheck = false;
            obj.IsEligible = false;

            foreach (ABC_RatingDetailDTO rdDTO in obj.ABC_RatingDetailDTOs)
            {
                ABC_RatingDetail r = session.Query<ABC_RatingDetail>().Where(rate => rate.Id == rdDTO.Id).SingleOrDefault();
                r.StaffRecord = rdDTO.StaffRecord;
                r.SupervisorRecord = rdDTO.SupervisorRecord;
                session.Update(r);
            }

            if (sumcheck == numberoftype2)
                obj.IsEligible = true;

            return obj;
        }

        [Authorize]
        [Route("")]
        public ABC_RatingDTO PutTempStaff(ABC_RatingDTO obj, NHibernate.ISession session)
        {
            decimal sumcheck = 0;
            decimal numberoftype2 = 0;
            //bool isFirstCheck = false;
            obj.IsEligible = false;

            foreach (ABC_RatingDetailDTO rdDTO in obj.ABC_RatingDetailDTOs)
            {
                ABC_RatingDetail rd = session.Query<ABC_RatingDetail>().Where(rate => rate.Id == rdDTO.Id).SingleOrDefault();
                rd.StaffRecord = rdDTO.StaffRecord;

                if (obj.StaffType == 1)
                    rd.SupervisorRecord = rd.StaffRecord;
                else
                    rd.SupervisorRecord = rdDTO.SupervisorRecord;
                session.Update(rd);
            }

            if (sumcheck == numberoftype2)
                obj.IsEligible = true;

            return obj;
        }

        [Authorize]
        [Route("")]
        public void SaveLockRatingResult(ABC_RatingDTO obj, NHibernate.ISession session)
        {
            ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
            ABC_Rating ratingResult = session.Query<ABC_Rating>().SingleOrDefault(r => r.Id == obj.Id);
            ratingResult.DateRated = DateTime.Now;
            if (obj.IsSupervisor == true)
            {
                ratingResult.DateSupervisorRated = DateTime.Now;
                ratingResult.IsSupervisorRated = true;
                if (new Guid(applicationUser.WebGroupId) == WebGroupConst.TruongPhongUyQuyenID)
                {
                    Guid departmentId = applicationUser.DepartmentId != null ? new Guid(applicationUser.DepartmentId) : Guid.Empty;
                    Staff staff = session.Query<Staff>().Where(s =>
                    s.Department.Id == departmentId
                    && s.StaffInfo.Position != null
                    && s.StaffInfo.Position.LaQuanLy
                    ).FirstOrDefault();

                    if (staff != null)
                        ratingResult.StaffRating = new Staff() { Id = staff.Id };
                }
                else
                {
                    Guid staffId = new Guid(applicationUser.Id);
                    Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();
                    if (staff != null)
                        ratingResult.StaffRating = new Staff() { Id = staff.Id };
                }
                //if (ratingResult.TotalSupervisorRecord < 70)
                //    ratingResult.Classification = "D";
                //else if (ratingResult.TotalSupervisorRecord < 100)
                //    ratingResult.Classification = "C";
                ////N?u trên 100
                //else
                //{
                //    if (obj.IsEligible == true)
                //        ratingResult.Classification = "A";
                //    else
                //        ratingResult.Classification = "B";
                //}
            }

            else
                ratingResult.IsRated = true;
            session.Update(ratingResult);
        }

        [Authorize]
        [Route("")]
        public ABC_RatingDTO PutLock(ABC_RatingDTO obj)
        {
            SessionManager.DoWork(session =>
            {
                if (obj.IsSupervisor == true)
                {
                    obj = PutTemp(obj, session);
                }
                else
                {
                    obj = PutTempStaff(obj, session);
                }
                SaveLockRatingResult(obj, session);
            });
            return obj;
        }

        [Authorize]
        [Route("")]
        public ABC_RatingDTO GetRating(Guid Id)
        {
            ABC_RatingDTO result = new ABC_RatingDTO();
            SessionManager.DoWork(session =>
            {
                var temp = session.Query<ABC_Rating>().Where(r => r.Id == Id).SingleOrDefault();
                result.StaffNote = temp.StaffNote;
                result.SupervisorNote = temp.SupervisorNote;
                result.Id = temp.Id;
            });

            return result;
        }

        [Authorize]
        [Route("")]
        public int PutRating(ABC_RatingDTO obj)
        {
            int result = 0;
            SessionManager.DoWork(session =>
            {
                var temp = session.Query<ABC_Rating>().Where(r => r.Id == obj.Id).SingleOrDefault();
                session.Update(temp);
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public Guid GetCreateNewABCRating(Guid staffId, Guid evaluationId)
        {
            Guid result = Guid.Empty;
            SessionManager.DoWork(session =>
            {
                ABC_EvaluationBoardApiController ebController = new ABC_EvaluationBoardApiController();
                var staff = session.Query<Staff>().SingleOrDefault(q => q.Id == staffId);
                //int staffType = ebController.GetStaffType(staff);
                int staffType = ebController.GetStaffTypeForEvaluationBoard(staff, evaluationId, session);
                if (staff != null && staffType != 0)
                {
                    var rating = new ABC_Rating();
                    rating.Id = Guid.NewGuid();
                    result = rating.Id;
                    rating.IsRated = false;
                    rating.IsSupervisorRated = false;
                    rating.IsRatingLocked = false;
                    rating.Staff = new Staff() { Id = staffId };
                    rating.Department = new Department() { Id = staff.Department.Id };
                    rating.ABC_EvaluationBoard = new ABC_EvaluationBoard() { Id = evaluationId };
                    ABC_LockedRatingDepartment lockedDept = ebController.GetLockedRatingDepartment(rating.ABC_EvaluationBoard.Id, rating.Department.Id);
                    //Nếu đơn vị đã bị khóa dánh giá thì khóa luôn ko cho thêm mới
                    if (lockedDept != null)
                        rating.IsRatingLocked = lockedDept.Status;
                    session.Save(rating);

                    var ratingDTO = new ABC_RatingDTO();
                    //Lấy nhóm tiêu chí
                    ratingDTO = GetCriterions(ratingDTO, session, staffType, rating);
                    //tạo ABC_RatingDetail

                    //Lấy tiêu chí dánh giá
                    var criterions = session.Query<ABC_Criterion>().OrderBy(c => c.OrderNumber);
                    foreach (ABC_Criterion cri in criterions)
                    {
                        ABC_RatingDetail rDetail = ParseNewRatingDetail(cri, rating);
                        session.Save(rDetail);
                    }

                }
            });
            return result;
        }
    }
}
