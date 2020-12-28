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

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_RatingDetailApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public ABC_RatingDTO GetRatingDetail(Guid evaluationId, Guid staffId, Guid supervisorId, Guid departmentId, byte isAdminRating)
        {
            //CriterionType
            //0 : Tiêu chí đánh giá chung
            //1 : Điểm phạt riêng cho NV
            //2 : Tiêu chí đánh giá riêng cho NV
            //3 : Tiêu chí đánh giá riêng cho GV
            //4 : Điểm phạt riêng cho GV
            //5 : Điểm thưởng chung và quy định hoàn thành nhiệm vụ chung
            try
            {
                ABC_RatingDTO result = new ABC_RatingDTO();
                SessionManager.DoWork(session =>
                {
                    ABC_EvaluationBoardApiController ebController = new ABC_EvaluationBoardApiController();
                    ABC_EvaluationBoard eb = session.Query<ABC_EvaluationBoard>().Where(e => e.Id == evaluationId).SingleOrDefault();
                    result.Month = eb.Month;
                    result.Year = eb.Year;
                    result.IsValid = true;
                    result.EvaluationBoardType = eb.EvaluationBoardType;
                    //Nếu admin=1 là BGH, 2 là admin
                    result.IsAdmin = isAdminRating;
                    ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                    //Nếu staffId trống thì là người nhập kế hoạch, lấy user hiện tại
                    if (staffId == Guid.Empty)
                    {
                        staffId = new Guid(applicationUser.Id);
                    }
                    //Nếu là quản lý, ủy quyền, BGH, admin
                    if (supervisorId != Guid.Empty || isAdminRating > 0 || applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000005")
                    {
                        result.IsSupervisor = true;
                    }
                    Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();
                    //Loại nhân sự 
                    //0: Không xác định
                    //1: GV
                    //2: NV
                    int staffType = 0;
                    staffType = ebController.GetStaffTypeForEvaluationBoard(staff, evaluationId, session);
                    var ratingType = ebController.GetRatingTypeForEvaluationBoard(staff, session);
                    result.StaffType = staffType;

                    if (ratingType.Id != Guid.Empty)
                    {
                        result.StaffName = staff.StaffProfile.Name;
                        result.StaffId = staff.Id;
                        result.WebGroupId = staff.StaffInfo?.WebUser != null ? staff.StaffInfo.WebUser.WebGroupId : Guid.Empty;
                        if (staff.StaffInfo?.Position != null)
                            result.StaffPosition = staff.StaffInfo.Position.Name;
                        else if (staff.Title != null)
                            result.StaffPosition = staff.Title.Name;
                        else
                            result.StaffPosition = "";
                        result.DepartmentName = staff.Department?.Name;

                        //Kiểm tra đã có bảng đánh giá chưa
                        ABC_Rating rating = session.Query<ABC_Rating>().Where(r => r.ABC_EvaluationBoard.Id == evaluationId && r.Staff.Id == staffId).SingleOrDefault();

                        //Xóa và tạo lại bảng đánh giá mới khi chưa khóa và:
                        if (rating != null && !rating.IsRated && !rating.IsSupervisorRated
                            && rating.Classification == null && rating.ClassificationSecond == null && rating.ClassificationThird == null)
                        {
                            //Trường hợp chưa lưu điểm
                            if (rating.TotalStaffRecord == 0 && rating.TotalSupervisorRecord == 0)
                            {
                                session.Delete(rating);
                                rating = null;
                            }
                            else
                            {
                                //Trường hợp tạo tiêu chí sau khi load bảng đánh giá: số dòng detail khác nhau
                                ABC_RatingDTO ratingCheck = new ABC_RatingDTO();
                                ratingCheck = GetCriterions(ratingCheck, session, staff);
                                int numberOfCriterionDetails = 0; //số dòng detail trong bộ tiêu chí
                                foreach (var group in ratingCheck.ABC_RatingGroupDTOs)
                                {
                                    numberOfCriterionDetails += session.Query<ABC_CriterionDetail>().Where(c => c.ABC_Criterion.Id == group.Id).Count();
                                }
                                //số dòng detail trong bảng đánh giá hiện tại
                                int numberOfRatingDetails = session.Query<ABC_RatingDetail>().Where(q => q.ABC_Rating.Id == rating.Id).Count();
                                if (numberOfRatingDetails != numberOfCriterionDetails)
                                {
                                    session.Delete(rating);
                                    rating = null;
                                }
                            }
                        }

                        //Nếu chưa có thì tạo mới
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
                            //Nếu đơn vị đã bị khóa đánh giá thì khóa luôn kế hoạch thêm mới
                            if (lockedDept != null)
                                rating.IsRatingLocked = lockedDept.Status;
                            session.Save(rating);

                            //Lấy nhóm tiêu chí
                            if (ratingType.Id != Guid.Empty)
                                result = GetCriterions(result, session, staff);
                            foreach (ABC_RatingGroupDTO group in result.ABC_RatingGroupDTOs)
                            {
                                //Lấy tiêu chí đánh giá
                                var criterionDetails = session.Query<ABC_CriterionDetail>().Where(c => c.ABC_Criterion.Id == group.Id).OrderBy(c => c.OrderNumber);
                                foreach (ABC_CriterionDetail criDetail in criterionDetails)
                                {
                                    ABC_RatingDetail rDetail = ParseNewRatingDetail(group, criDetail, rating);
                                    session.Save(rDetail);

                                    ABC_RatingDetailDTO rDetailDTO = ParseNewRatingDetailDTO(rDetail, criDetail);
                                    group.ABC_RatingDetailDTOs.Add(rDetailDTO);
                                }
                            }
                        }
                        else
                        {
                            ABC_LockedRatingDepartment lockedDept = ebController.GetLockedRatingDepartment(rating.ABC_EvaluationBoard.Id, rating.Department.Id);
                            //Nếu đơn vị đã bị khóa đánh giá thì khóa luôn kế hoạch thêm mới
                            if (lockedDept != null)
                                rating.IsRatingLocked = lockedDept.Status;
                            //Lấy nhóm tiêu chí 
                            if (ratingType.Id != Guid.Empty)
                                result = GetCriterions(result, session, staff);

                            foreach (ABC_RatingGroupDTO group in result.ABC_RatingGroupDTOs)
                            {
                                //Lấy tiêu chí đánh giá
                                var ratingDetails = session.Query<ABC_RatingDetail>().Where(c => c.ABC_Criterion.Id == group.Id && c.ABC_Rating.Id == rating.Id).OrderBy(c => c.ABC_CriterionDetail.OrderNumber);
                                foreach (ABC_RatingDetail ratingDetail in ratingDetails)
                                {
                                    ABC_RatingDetailDTO rDetailDTO = ParseRatingDetailDTO(ratingDetail);
                                    group.ABC_RatingDetailDTOs.Add(rDetailDTO);
                                }
                            }
                        }
                        result.Id = rating.Id;
                        result.IsRated = rating.IsRated;
                        result.IsSupervisorRated = rating.IsSupervisorRated;
                        result.IsRatingLocked = rating.IsRatingLocked;
                        result.ABC_TitleId = rating.ABC_Title?.Id;
                        result.ABC_TitleName = rating.ABC_Title?.Name;
                        result.ABC_TitleSecondId = rating.ABC_TitleSecond?.Id;
                        result.ABC_TitleSecondName = rating.ABC_TitleSecond?.Name;
                        result.NoteSecond = rating.NoteSecond;
                        result.NoteThird = rating.NoteThird;
                    }
                    else
                    {
                        result.IsValid = false;
                    }

                    // có listRatingType
                    // có listCrirerion
                });
                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_RatingDetailApi/GetRatingDetail", ex);
                throw;
            }
        }

        public ABC_RatingDTO GetCriterions(ABC_RatingDTO result, NHibernate.ISession session, Staff staff)
        {
            var ratingType = session.Query<ThongTinDanhGia>().Where(q => q.StaffInfo.Id == staff.Id).SingleOrDefault().ABC_RatingType;
            var criterions = session.Query<ABC_Criterion>().Where(q => q.ABC_RatingType.Id == ratingType.Id).OrderBy(c => c.OrderNumber);
            if (criterions.Any())
            {
                foreach (ABC_Criterion cri in criterions)
                {
                    ABC_RatingGroupDTO rGroup = ParseRatingGroup(cri);
                    result.ABC_RatingGroupDTOs.Add(rGroup);
                }
            }

            // phân nhóm tiêu chí theo đối tượng đánh giá
            result.ABC_RatingTypeDTOs = new List<ABC_RatingTypeDTO>();

            var criterionIncluded = criterions.Where(q => q.CopyFromRatingType != null && q.IsGroupByRatingTypeOnEvaluationBoard).ToList();
            var ratingTypeIncludeds = criterionIncluded.Select(q => q.CopyFromRatingType).GroupBy(e => e.Id).Select(group => group.First());
            foreach (var ratingTypeIncluded in ratingTypeIncludeds)
            {
                ABC_RatingTypeDTO rtIncludedDTO = ratingTypeIncluded.Map<ABC_RatingTypeDTO>();
                
                rtIncludedDTO.CriterionIds = new List<Guid>();
                rtIncludedDTO.CriterionIds = criterionIncluded.Where(q=> q.CopyFromRatingType == ratingTypeIncluded).Select(q => q.Id).ToList();
                result.ABC_RatingTypeDTOs.Add(rtIncludedDTO);
            }
            ABC_RatingTypeDTO rtDTO = ratingType.Map<ABC_RatingTypeDTO>();
            rtDTO.CriterionIds = session.Query<ABC_Criterion>().Where(q => q.ABC_RatingType.Id == rtDTO.Id && q.CopyFromRatingType == null).Select(q => q.Id).ToList();
            result.ABC_RatingTypeDTOs.Add(rtDTO);

            return result;
        }

        public string ConvertNumberToAlphabet(int num)
        {
            string result = "";
            if (num > 0 && num < 27)
                result = Convert.ToChar(num + 64).ToString().ToLower();
            return result;
        }

        public ABC_RatingGroupDTO ParseRatingGroup(ABC_Criterion cri)
        {
            ABC_RatingGroupDTO rGroup = new ABC_RatingGroupDTO();
            rGroup.Id = cri.Id;
            rGroup.Name = cri.Name;
            rGroup.OrderNumber = cri.OrderNumber;
            rGroup.ABC_RatingGroupType = cri.CriterionType;
            rGroup.IsNotVisibleInEvaluationBoardType = cri.IsNotVisibleInEvaluationBoardType;
            return rGroup;
        }

        public ABC_RatingDetailDTO ParseNewRatingDetailDTO(ABC_RatingDetail rDetail, ABC_CriterionDetail criDetail)
        {
            ABC_RatingDetailDTO rDetailDTO = new ABC_RatingDetailDTO();
            rDetailDTO.Id = rDetail.Id;
            rDetailDTO.Name = criDetail.Name;
            rDetailDTO.MaxRecord = criDetail.MaxRecord;
            rDetailDTO.StaffRecord = rDetail.StaffRecord;
            rDetailDTO.SupervisorRecord = rDetail.SupervisorRecord;
            rDetailDTO.SupervisorNote = rDetail.SupervisorNote;
            rDetailDTO.OrderNumber = ConvertNumberToAlphabet(criDetail.OrderNumber);
            rDetailDTO.ABC_CriterionDetailType = criDetail.ABC_CriterionDetailType != null ? criDetail.ABC_CriterionDetailType.Id : 0;
            return rDetailDTO;
        }

        public ABC_RatingDetailDTO ParseRatingDetailDTO(ABC_RatingDetail ratingDetail)
        {
            ABC_RatingDetailDTO rDetailDTO = new ABC_RatingDetailDTO();
            rDetailDTO.Id = ratingDetail.Id;
            rDetailDTO.Name = ratingDetail.ABC_CriterionDetail.Name;
            rDetailDTO.StaffRecord = ratingDetail.StaffRecord;
            rDetailDTO.SupervisorRecord = ratingDetail.SupervisorRecord;
            rDetailDTO.SupervisorNote = ratingDetail.SupervisorNote;
            rDetailDTO.MaxRecord = ratingDetail.ABC_CriterionDetail.MaxRecord;
            rDetailDTO.OrderNumber = ConvertNumberToAlphabet(ratingDetail.ABC_CriterionDetail.OrderNumber);
            rDetailDTO.ABC_CriterionDetailType = ratingDetail.ABC_CriterionDetail.ABC_CriterionDetailType != null ? ratingDetail.ABC_CriterionDetail.ABC_CriterionDetailType.Id : 0;
            return rDetailDTO;
        }

        public ABC_RatingDetail ParseNewRatingDetail(ABC_RatingGroupDTO group, ABC_CriterionDetail criDetail, ABC_Rating rating)
        {
            ABC_RatingDetail rDetail = new ABC_RatingDetail();
            rDetail.Id = Guid.NewGuid();
            rDetail.ABC_Criterion = new ABC_Criterion() { Id = group.Id };
            rDetail.ABC_CriterionDetail = new ABC_CriterionDetail() { Id = criDetail.Id };
            rDetail.ABC_Rating = new ABC_Rating() { Id = rating.Id };
            rDetail.AdminRecord = 0;
            //Mặc định lấy điểm cá nhân đánh giá là điểm max
            rDetail.StaffRecord = criDetail.MaxRecord;
            rDetail.SupervisorRecord = 0;
            if (criDetail.ABC_CriterionDetailType.Id == 2)
            {
                rDetail.StaffRecord = 1;
                rDetail.SupervisorRecord = 1;
            }
            return rDetail;
        }

        public ABC_RatingDetail ParseNewRatingDetailSixMonth(ABC_RatingGroupDTO group, ABC_CriterionDetail criDetail, ABC_Rating rating, Guid staffId, Guid departmentId, Guid evaluationId, bool? isSupervisor, NHibernate.ISession session)
        {
            ABC_RatingDetail rDetail = new ABC_RatingDetail();
            rDetail.Id = Guid.NewGuid();
            rDetail.ABC_Criterion = new ABC_Criterion() { Id = group.Id };
            rDetail.ABC_CriterionDetail = new ABC_CriterionDetail() { Id = criDetail.Id };
            rDetail.ABC_Rating = new ABC_Rating() { Id = rating.Id };
            rDetail.AdminRecord = 0;
            rDetail.StaffRecord = 0;
            rDetail.SupervisorRecord = 0;
            if (criDetail.ABC_CriterionDetailType.Id == 2)
            {
                rDetail.StaffRecord = 1;
                rDetail.SupervisorRecord = 1;
            }
            //Tính điểm trung bình cho từng tiêu chí
            //Chỗ này nên viết stored

            //Lấy tất cả tiêu chí cùng loại trong 6 tháng
            var listSamePlanDetail = session.Query<ABC_RatingDetail>().Where(r =>
            //Cùng loại tiêu chí
            r.ABC_CriterionDetail.Id == criDetail.Id
            //Đánh giá tháng là con của đánh giá 6 tháng truyền vào
            && r.ABC_Rating.ABC_EvaluationBoard.ABC_ParentEvaluationBoard.Id == evaluationId
            //Đánh giá của staff
            && r.ABC_Rating.Staff.Id == staffId
            //Đánh giá của đơn vị staff
            //sửa ngày 7/1/2019: tính trung bình các bảng đánh giá con mà không cần phân biệt đơn vị
            //&& r.ABC_Rating.Department.Id == departmentId

            //Bảng đánh giá phải được khóa mới tính
            && (isSupervisor == false ? r.ABC_Rating.IsRated : isSupervisor == true ?
            r.ABC_Rating.IsSupervisorRated : r.ABC_Rating.IsRated && r.ABC_Rating.IsSupervisorRated)
            );
            if (listSamePlanDetail.Count() > 0)
            {
                rDetail.StaffRecord = Math.Round(listSamePlanDetail.Average(sr => sr.StaffRecord), 1, MidpointRounding.AwayFromZero);
                rDetail.SupervisorRecord = Math.Round(listSamePlanDetail.Average(sr => sr.SupervisorRecord), 1, MidpointRounding.AwayFromZero);
            }
            return rDetail;
        }

        public ABC_RatingDetail ParseNewRatingDetailYear(ABC_RatingGroupDTO group, ABC_CriterionDetail criDetail, ABC_Rating rating, Guid staffId, Guid departmentId, Guid evaluationId, bool? isSupervisor, NHibernate.ISession session)
        {
            ABC_RatingDetail rDetail = new ABC_RatingDetail();
            rDetail.Id = Guid.NewGuid();
            rDetail.ABC_Criterion = new ABC_Criterion() { Id = group.Id };
            rDetail.ABC_CriterionDetail = new ABC_CriterionDetail() { Id = criDetail.Id };
            rDetail.ABC_Rating = new ABC_Rating() { Id = rating.Id };
            rDetail.AdminRecord = 0;
            rDetail.StaffRecord = 0;
            rDetail.SupervisorRecord = 0;

            //Tính điểm trung bình cho từng tiêu chí
            //Chỗ này nên viết stored

            //Lấy tất cả tiêu chí cùng loại trong 6 tháng
            var listSamePlanDetail = session.Query<ABC_RatingDetail>().Where(r =>
            //Cùng loại tiêu chí
            r.ABC_CriterionDetail.Id == criDetail.Id
            //Đánh giá 6 tháng là con của đánh giá năm truyền vào
            && r.ABC_Rating.ABC_EvaluationBoard.ABC_ParentEvaluationBoard.Id == evaluationId
            //Đánh giá của staff
            && r.ABC_Rating.Staff.Id == staffId
            //Đánh giá của đơn vị staff
            //sửa ngày 7/1/2019: tính trung bình các bảng đánh giá con mà không cần phân biệt đơn vị
            //&& r.ABC_Rating.Department.Id == departmentId

            //Bảng đánh giá phải được khóa mới tính
            && (isSupervisor == false ? r.ABC_Rating.IsRated : isSupervisor == true ?
            r.ABC_Rating.IsSupervisorRated : r.ABC_Rating.IsRated && r.ABC_Rating.IsSupervisorRated)
            );

            if (listSamePlanDetail.Count() > 0)
            {
                //Lấy các tiêu chí đánh giá theo điểm trung bình
                var listPlanDetail1 = listSamePlanDetail.Where(pl => pl.ABC_Criterion.CriterionType != 5);
                if (listPlanDetail1.Count() > 0)
                {
                    rDetail.StaffRecord = Math.Round(listPlanDetail1.Average(sr => sr.StaffRecord), 1, MidpointRounding.AwayFromZero);
                    rDetail.SupervisorRecord = Math.Round(listPlanDetail1.Average(sr => sr.SupervisorRecord), 1, MidpointRounding.AwayFromZero);
                }
                //Lấy các tiêu chí đánh giá cộng dồn tổ hợp
                var listPlanDetail2 = listSamePlanDetail.Where(pl => pl.ABC_Criterion.CriterionType == 5 && pl.ABC_CriterionDetail.ABC_CriterionDetailType.Id == 0);
                if (listPlanDetail2.Count() > 0)
                {
                    rDetail.StaffRecord = listPlanDetail2.Sum(sr => sr.StaffRecord);
                    rDetail.SupervisorRecord = listPlanDetail2.Sum(sr => sr.SupervisorRecord);
                }
                //Lấy các tiêu chí đánh giá check chọn
                var listPlanDetail3 = listSamePlanDetail.Where(pl => pl.ABC_Criterion.CriterionType == 5 && pl.ABC_CriterionDetail.ABC_CriterionDetailType.Id == 2);
                if (listPlanDetail3.Count() > 0)
                {
                    //Nếu tổng điểm các check chọn lớn hơn hoặc bằng 1 thì gán 1, ngược lại gán 0
                    //rDetail.StaffRecord = listPlanDetail3.Sum(sr => sr.StaffRecord) >= 1 ? 1 : 0;
                    //rDetail.SupervisorRecord = listPlanDetail3.Sum(sr => sr.SupervisorRecord) >= 1 ? 1 : 0;

                    //Sửa lại ngày 23/11/2016: mặc định year là có check
                    rDetail.StaffRecord = 1;
                    rDetail.SupervisorRecord = 1;
                }
            }
            return rDetail;
        }

        public static void ParseRatingDetail(ABC_RatingDetail ratingDetail, Guid staffId, Guid departmentId, Guid evaluationId, bool? isSupervisor, NHibernate.ISession session)
        {
            //Lấy tất cả tiêu chí cùng loại trong 6 tháng
            var listSamePlanDetail = session.Query<ABC_RatingDetail>().Where(r =>
            //Cùng loại tiêu chí
            r.ABC_CriterionDetail.Id == ratingDetail.ABC_CriterionDetail.Id
            //Đánh giá tháng là con của đánh giá 6 tháng truyền vào
            && r.ABC_Rating.ABC_EvaluationBoard.ABC_ParentEvaluationBoard.Id == evaluationId
            //Đánh giá của staff
            && r.ABC_Rating.Staff.Id == staffId
            //Đánh giá của đơn vị staff
            //sửa ngày 7/1/2019: tính trung bình các bảng đánh giá con mà không cần phân biệt đơn vị
            //&& r.ABC_Rating.Department.Id == departmentId

            //Bảng đánh giá phải được khóa mới tính
            && (isSupervisor == false ? r.ABC_Rating.IsRated : isSupervisor == true ?
            r.ABC_Rating.IsSupervisorRated : r.ABC_Rating.IsRated && r.ABC_Rating.IsSupervisorRated)
            );

            if (listSamePlanDetail.Count() > 0)
            {
                //Lấy các tiêu chí đánh giá theo điểm trung bình
                var listPlanDetail1 = listSamePlanDetail.Where(pl => pl.ABC_Criterion.CriterionType != 5);
                if (listPlanDetail1.Count() > 0)
                {
                    ratingDetail.StaffRecord = Math.Round(listPlanDetail1.Average(sr => sr.StaffRecord), 1, MidpointRounding.AwayFromZero);
                    ratingDetail.SupervisorRecord = Math.Round(listPlanDetail1.Average(sr => sr.SupervisorRecord), 1, MidpointRounding.AwayFromZero);
                }
                //Lấy các tiêu chí đánh giá cộng dồn tổ hợp
                var listPlanDetail2 = listSamePlanDetail.Where(pl => pl.ABC_Criterion.CriterionType == 5 && pl.ABC_CriterionDetail.ABC_CriterionDetailType.Id == 0);
                if (listPlanDetail2.Count() > 0)
                {
                    ratingDetail.StaffRecord = listPlanDetail2.Sum(sr => sr.StaffRecord);
                    ratingDetail.SupervisorRecord = listPlanDetail2.Sum(sr => sr.SupervisorRecord);
                }
                //Lấy các tiêu chí đánh giá check chọn
                var listPlanDetail3 = listSamePlanDetail.Where(pl => pl.ABC_Criterion.CriterionType == 5 && pl.ABC_CriterionDetail.ABC_CriterionDetailType.Id == 2);
                if (listPlanDetail3.Count() > 0)
                {
                    //Nếu tổng điểm các check chọn lớn hơn hoặc bằng 1 thì gán 1, ngược lại gán 0
                    //ratingDetail.StaffRecord = listPlanDetail3.Sum(sr => sr.StaffRecord) >= 1 ? 1 : 0;
                    //ratingDetail.SupervisorRecord = listPlanDetail3.Sum(sr => sr.SupervisorRecord) >= 1 ? 1 : 0;

                    //Sửa lại ngày 23/11/2016: mặc định year là có check
                    ratingDetail.StaffRecord = 1;
                    ratingDetail.SupervisorRecord = 1;
                }
            }
        }

        public static void ParseRatingDetail_Supervisor(ABC_RatingDetail ratingDetail, Guid staffId, Guid departmentId, Guid evaluationId, bool? isSupervisor, NHibernate.ISession session)
        {
            //Lấy tất cả tiêu chí cùng loại trong 6 tháng
            var listSamePlanDetail = session.Query<ABC_RatingDetail>().Where(r =>
            //Cùng loại tiêu chí
            r.ABC_CriterionDetail.Id == ratingDetail.ABC_CriterionDetail.Id
            //Đánh giá tháng là con của đánh giá 6 tháng truyền vào
            && r.ABC_Rating.ABC_EvaluationBoard.ABC_ParentEvaluationBoard.Id == evaluationId
            //Đánh giá của staff
            && r.ABC_Rating.Staff.Id == staffId
            //Đánh giá của đơn vị staff
            //sửa ngày 7/1/2019: tính trung bình các bảng đánh giá con mà không cần phân biệt đơn vị
            //&& r.ABC_Rating.Department.Id == departmentId

            //Bảng đánh giá phải được khóa mới tính
            && (isSupervisor == false ? r.ABC_Rating.IsRated : isSupervisor == true ?
            r.ABC_Rating.IsSupervisorRated : r.ABC_Rating.IsRated && r.ABC_Rating.IsSupervisorRated)
            );

            if (listSamePlanDetail.Count() > 0)
            {
                //Lấy các tiêu chí đánh giá theo điểm trung bình
                var listPlanDetail1 = listSamePlanDetail.Where(pl => pl.ABC_Criterion.CriterionType != 5);
                if (listPlanDetail1.Count() > 0)
                {
                    ratingDetail.SupervisorRecord = Math.Round(listPlanDetail1.Average(sr => sr.SupervisorRecord), 1, MidpointRounding.AwayFromZero);
                }
                //Lấy các tiêu chí đánh giá cộng dồn tổ hợp
                var listPlanDetail2 = listSamePlanDetail.Where(pl => pl.ABC_Criterion.CriterionType == 5 && pl.ABC_CriterionDetail.ABC_CriterionDetailType.Id == 0);
                if (listPlanDetail2.Count() > 0)
                {
                    ratingDetail.SupervisorRecord = listPlanDetail2.Sum(sr => sr.SupervisorRecord);
                }
                //Lấy các tiêu chí đánh giá check chọn
                var listPlanDetail3 = listSamePlanDetail.Where(pl => pl.ABC_Criterion.CriterionType == 5 && pl.ABC_CriterionDetail.ABC_CriterionDetailType.Id == 2);
                if (listPlanDetail3.Count() > 0)
                {
                    //Nếu tổng điểm các check chọn lớn hơn hoặc bằng 1 thì gán 1, ngược lại gán 0
                    //ratingDetail.StaffRecord = listPlanDetail3.Sum(sr => sr.StaffRecord) >= 1 ? 1 : 0;
                    //ratingDetail.SupervisorRecord = listPlanDetail3.Sum(sr => sr.SupervisorRecord) >= 1 ? 1 : 0;

                    //Sửa lại ngày 23/11/2016: mặc định year là có check
                    ratingDetail.SupervisorRecord = 1;
                }
            }
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
        public ABC_RatingDTO PutTemp(ABC_RatingDTO obj, NHibernate.ISession session)
        {
            decimal sumcheck = 0;
            decimal numberoftype2 = 0;
            //bool isFirstCheck = false;
            obj.IsEligible = false;
            foreach (ABC_RatingGroupDTO group in obj.ABC_RatingGroupDTOs)
            {
                foreach (ABC_RatingDetailDTO rd in group.ABC_RatingDetailDTOs)
                {
                    ABC_RatingDetail r = session.Query<ABC_RatingDetail>().Where(rate => rate.Id == rd.Id).SingleOrDefault();
                    r.StaffRecord = rd.StaffRecord;
                    r.SupervisorRecord = rd.SupervisorRecord;
                    r.SupervisorNote = rd.SupervisorNote;
                    session.Update(r);
                }
            }
            foreach (ABC_RatingGroupDTO group in obj.ABC_RatingGroupSpecialDTOs)
            {
                foreach (ABC_RatingDetailDTO rd in group.ABC_RatingDetailDTOs)
                {
                    ABC_RatingDetail r = session.Query<ABC_RatingDetail>().Where(rate => rate.Id == rd.Id).SingleOrDefault();
                    r.StaffRecord = rd.StaffRecord;
                    r.SupervisorRecord = rd.SupervisorRecord;
                    r.SupervisorNote = rd.SupervisorNote;
                    session.Update(r);
                }
            }
            foreach (ABC_RatingGroupDTO group in obj.ABC_RatingGroupPropertyDTOs)
            {
                foreach (ABC_RatingDetailDTO rd in group.ABC_RatingDetailDTOs)
                {
                    ABC_RatingDetail r = session.Query<ABC_RatingDetail>().Where(rate => rate.Id == rd.Id).SingleOrDefault();
                    r.StaffRecord = rd.StaffRecord;
                    r.SupervisorRecord = rd.SupervisorRecord;
                    r.SupervisorNote = rd.SupervisorNote;

                    //Nếu là loại checkbox
                    if (rd.ABC_CriterionDetailType == 2)
                    {
                        numberoftype2++;
                        //if (r.ABC_CriterionDetail.OrderNumber == 1 && rd.SupervisorRecord == 1)
                        //    isFirstCheck = true;
                        sumcheck += rd.SupervisorRecord;
                    }
                    session.Update(r);
                }
            }
            //Nếu check tất cả thì đủ điều kiện loại A
            if (sumcheck == numberoftype2)
                obj.IsEligible = true;

            //Nếu có ít nhất 2 dấu check và có check đầu tiên thì đủ điều kiện loại A
            //if (sumcheck >= 2 && isFirstCheck == true)
            //    obj.IsEligible = true;
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
            foreach (ABC_RatingGroupDTO group in obj.ABC_RatingGroupDTOs)
            {
                foreach (ABC_RatingDetailDTO rd in group.ABC_RatingDetailDTOs)
                {
                    ABC_RatingDetail r = session.Query<ABC_RatingDetail>().Where(rate => rate.Id == rd.Id).SingleOrDefault();
                    r.StaffRecord = rd.StaffRecord;
                    //Nếu là cá nhân khóa sẽ mặc định đẩy điểm cá nhân đánh giá qua trưởng đơn vị
                    if (!r.ABC_Rating.IsSupervisorRated) r.SupervisorRecord = r.StaffRecord;
                    session.Update(r);
                }
            }
            foreach (ABC_RatingGroupDTO group in obj.ABC_RatingGroupSpecialDTOs)
            {
                foreach (ABC_RatingDetailDTO rd in group.ABC_RatingDetailDTOs)
                {
                    ABC_RatingDetail r = session.Query<ABC_RatingDetail>().Where(rate => rate.Id == rd.Id).SingleOrDefault();
                    r.StaffRecord = rd.StaffRecord;
                    //Nếu là cá nhân khóa sẽ mặc định đẩy điểm cá nhân đánh giá qua trưởng đơn vị
                    if (!r.ABC_Rating.IsSupervisorRated) r.SupervisorRecord = r.StaffRecord;
                    session.Update(r);
                }
            }
            foreach (ABC_RatingGroupDTO group in obj.ABC_RatingGroupPropertyDTOs)
            {
                foreach (ABC_RatingDetailDTO rd in group.ABC_RatingDetailDTOs)
                {
                    ABC_RatingDetail r = session.Query<ABC_RatingDetail>().Where(rate => rate.Id == rd.Id).SingleOrDefault();
                    r.StaffRecord = rd.StaffRecord;
                    r.SupervisorRecord = rd.SupervisorRecord;
                    r.SupervisorNote = r.SupervisorNote;

                    //Nếu là loại checkbox
                    if (rd.ABC_CriterionDetailType == 2)
                    {
                        numberoftype2++;
                        //if (r.ABC_CriterionDetail.OrderNumber == 1 && rd.SupervisorRecord == 1)
                        //    isFirstCheck = true;
                        sumcheck += rd.SupervisorRecord;
                    }
                    session.Update(r);
                }
            }
            //Nếu check tất cả thì đủ điều kiện loại A
            if (sumcheck == numberoftype2)
                obj.IsEligible = true;

            //Nếu có ít nhất 2 dấu check và có check đầu tiên thì đủ điều kiện loại A
            //if (sumcheck >= 2 && isFirstCheck == true)
            //    obj.IsEligible = true;
            return obj;
        }

        [Authorize]
        [Route("")]
        public void SaveRatingResult(ABC_RatingDTO obj, NHibernate.ISession session)
        {
            ABC_Rating ratingResult = session.Query<ABC_Rating>().SingleOrDefault(r => r.Id == obj.Id);
            ratingResult.TotalStaffRecord = obj.TotalStaffRecord;
            ratingResult.TotalSupervisorRecord = obj.TotalSupervisorRecord;
            if (obj.ABC_TitleId.HasValue)
                ratingResult.ABC_Title = new ABC_Title() { Id = obj.ABC_TitleId.Value };
            if (obj.ABC_TitleSecondId.HasValue)
                ratingResult.ABC_TitleSecond = new ABC_Title() { Id = obj.ABC_TitleSecondId.Value };
            ratingResult.NoteSecond = string.IsNullOrEmpty(obj.NoteSecond) ? null : obj.NoteSecond;
            ratingResult.NoteThird = string.IsNullOrEmpty(obj.NoteThird) ? null : obj.NoteThird;
            session.Update(ratingResult);
        }

        [Authorize]
        [Route("")]
        public void SaveLockRatingResult(ABC_RatingDTO obj, NHibernate.ISession session)
        {
            ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
            ABC_Rating ratingResult = session.Query<ABC_Rating>().SingleOrDefault(r => r.Id == obj.Id);
            ratingResult.TotalStaffRecord = obj.TotalStaffRecord;
            ratingResult.TotalSupervisorRecord = obj.TotalSupervisorRecord;
            if (obj.ABC_TitleId.HasValue)
                ratingResult.ABC_Title = new ABC_Title() { Id = obj.ABC_TitleId.Value };
            if (obj.ABC_TitleSecondId.HasValue)
                ratingResult.ABC_TitleSecond = new ABC_Title() { Id = obj.ABC_TitleSecondId.Value };
            ratingResult.NoteSecond = string.IsNullOrEmpty(obj.NoteSecond) ? null : obj.NoteSecond;
            ratingResult.NoteThird = string.IsNullOrEmpty(obj.NoteThird) ? null : obj.NoteThird;
            ratingResult.DateRated = DateTime.Now;
            if (obj.IsSupervisor == true)
            {
                ratingResult.DateSupervisorRated = DateTime.Now;
                ratingResult.IsSupervisorRated = true;
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

                    //Nếu là admin ủy quyền thì gán người đánh giá là trưởng đơn vị đó
                    if (staff != null)
                        ratingResult.StaffRating = new Staff() { Id = staff.Id };
                }
                else
                {
                    Guid staffId = new Guid(applicationUser.Id);
                    Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();
                    //Gán người đánh giá là người hiện tại
                    if (staff != null)
                        ratingResult.StaffRating = new Staff() { Id = staff.Id };
                }
                //if (ratingResult.TotalSupervisorRecord < 70)
                //    ratingResult.Classification = "D";
                //else if (ratingResult.TotalSupervisorRecord < 100)
                //    ratingResult.Classification = "C";
                ////Nếu trên 100
                //else
                //{
                //    //Đủ điều kiện
                //    if (obj.IsEligible == true)
                //        ratingResult.Classification = "A";
                //    else
                //        ratingResult.Classification = "B";
                //}

                ABC_ClassificationSet classificationSet = session.Query<ABC_ClassificationSet>().Where(q =>
                                                            q.ThoiGianApDung <= ratingResult.ABC_EvaluationBoard.ToDate).OrderByDescending(q => q.ThoiGianApDung).FirstOrDefault();
                if (classificationSet != null)
                {
                    var rank = session.Query<ABC_Classifications>().Where(q => q.ABC_ClassificationSet.Id == classificationSet.Id
                        && ratingResult.TotalSupervisorRecord < (q.MaxRecord ?? 1000) && ratingResult.TotalSupervisorRecord >= (q.MinRecord ?? -1000));
                    if (rank.Count() == 1)
                    {
                        ratingResult.Classification = rank.First().Rank;
                    }
                    else if (rank.Count() > 1)
                    {
                        if (obj.IsEligible == true)
                        {
                            ratingResult.Classification = rank.FirstOrDefault(q => q.IsEligible == true).Rank;
                        }
                        else
                        {
                            ratingResult.Classification = rank.FirstOrDefault(q => q.IsEligible == false).Rank;
                        }
                    }
                }


                //15/06/2017: Nếu có ít nhất 1 tháng(nhân viên) / 6 tháng(giảng viên) trưởng đơn vị không đánh giá thì bị hạ xếp loại 1 bậc
                ABC_EvaluationBoard eb = ratingResult.ABC_EvaluationBoard;
                //Đánh giá 6 tháng của GV ko cần xét
                if (eb.EvaluationBoardType == 1 || (eb.EvaluationBoardType == 2 && obj.StaffType == 2))
                {
                    string s = GetCheckChildAllRatingLocked(ratingResult, obj.StaffType, session);
                    if (s != "")
                    {
                        if (obj.StaffType == 2) //nhân viên
                        {
                            ratingResult.NoteThird = "Không đánh giá tháng " + s;
                            switch (ratingResult.Classification)
                            {
                                case "A":
                                    {
                                        ratingResult.ClassificationThird = "B";
                                    }
                                    break;
                                case "B":
                                    {
                                        ratingResult.ClassificationThird = "C";
                                    }
                                    break;
                                case "C":
                                    {
                                        ratingResult.ClassificationThird = "D";
                                    }
                                    break;
                            }
                        }
                        else if (obj.StaffType == 1) //giảng viên không hạ bậc
                        {
                            ratingResult.NoteThird = "Không đánh giá " + s;
                        }
                    }
                }
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
                //Nếu là cá nhân khóa sẽ mặc định đẩy điểm cá nhân đánh giá qua trưởng đơn vị
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
                result.Id = temp.Id;
                result.Classification = temp.Classification;
                result.ClassificationSecond = temp.ClassificationSecond;
                result.ClassificationThird = temp.ClassificationThird;
                result.NoteSecond = temp.NoteSecond;
                result.NoteThird = temp.NoteThird;
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
                temp.ClassificationSecond = obj.ClassificationSecond;
                temp.ClassificationThird = obj.ClassificationThird;
                temp.NoteSecond = obj.NoteSecond;
                temp.NoteThird = obj.NoteThird;
                session.Update(temp);
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public Guid GetCreateNewABCRating(Guid staffId, Guid evaluationId)
        {
            try
            {
                Guid result = Guid.Empty;
                SessionManager.DoWork(session =>
                {
                    ABC_EvaluationBoardApiController ebController = new ABC_EvaluationBoardApiController();
                    var staff = session.Query<Staff>().SingleOrDefault(q => q.Id == staffId);
                    var evaluationBoardType = session.Query<ABC_EvaluationBoard>().SingleOrDefault(q => q.Id == evaluationId).EvaluationBoardType;
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
                        //Nếu đơn vị đã bị khóa đánh giá thì khóa luôn kế hoạch thêm mới
                        if (lockedDept != null)
                            rating.IsRatingLocked = lockedDept.Status;
                        session.Save(rating);

                        var ratingDTO = new ABC_RatingDTO();
                        //Lấy nhóm tiêu chí
                            ratingDTO = GetCriterions(ratingDTO, session, staff);
                        //tạo ABC_RatingDetail
                        foreach (ABC_RatingGroupDTO group in ratingDTO.ABC_RatingGroupDTOs)
                        {
                            //Lấy tiêu chí đánh giá
                            var criterionDetails = session.Query<ABC_CriterionDetail>().Where(c => c.ABC_Criterion.Id == group.Id).OrderBy(c => c.OrderNumber);
                            foreach (ABC_CriterionDetail criDetail in criterionDetails)
                            {
                                ABC_RatingDetail rDetail = ParseNewRatingDetail(group, criDetail, rating);
                                session.Save(rDetail);
                            }
                        }
                        foreach (ABC_RatingGroupDTO group in ratingDTO.ABC_RatingGroupPropertyDTOs)
                        {
                            //Lấy tiêu chí đánh giá
                            var criterionDetails = session.Query<ABC_CriterionDetail>().Where(c => c.ABC_Criterion.Id == group.Id).OrderBy(c => c.OrderNumber);
                            foreach (ABC_CriterionDetail criDetail in criterionDetails)
                            {
                                ABC_RatingDetail rDetail = ParseNewRatingDetail(group, criDetail, rating);
                                session.Save(rDetail);
                            }
                        }
                        foreach (ABC_RatingGroupDTO group in ratingDTO.ABC_RatingGroupSpecialDTOs)
                        {
                            //Lấy tiêu chí đánh giá
                            var criterionDetails = session.Query<ABC_CriterionDetail>().Where(c => c.ABC_Criterion.Id == group.Id).OrderBy(c => c.OrderNumber);
                            foreach (ABC_CriterionDetail criDetail in criterionDetails)
                            {
                                ABC_RatingDetail rDetail = ParseNewRatingDetail(group, criDetail, rating);
                                session.Save(rDetail);
                            }
                        }
                    }
                });
                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_RatingDetailApi/GetCreateNewABCRating", ex);
                throw;
            }
        }

        public string GetCheckChildAllRatingLocked(ABC_Rating ratingResult, int staffType, NHibernate.ISession session)
        {
            string result = "";
            List<string> listString = new List<string>();
            ABC_EvaluationBoard eb = ratingResult.ABC_EvaluationBoard;
            if (eb.EvaluationBoardType == 1 && staffType == 1) //cả năm giảng viên
            {
                var listRating = session.Query<ABC_Rating>().Where(r =>
                    //Tất cả rating là con 
                    r.ABC_EvaluationBoard.ABC_ParentEvaluationBoard.Id == eb.Id
                    && r.Staff.Id == ratingResult.Staff.Id
                    //&& r.Department.Id == ratingResult.Department.Id //không xét bộ phận vì nhân viên có thể đổi bộ phận
                    );
                var list_TatCaThangCon = session.Query<ABC_EvaluationBoard>().Where(q => q.ABC_ParentEvaluationBoard.Id == eb.Id);
                var list_ThangConDaKhoa = listRating.Where(q => q.IsSupervisorRated == true).Select(q => q.ABC_EvaluationBoard);
                var list_ThangConChuaKhoa = list_TatCaThangCon.Where(q => !list_ThangConDaKhoa.Any(qq => qq.Id == q.Id));

                listString = list_ThangConChuaKhoa.OrderBy(q => q.Month).Select(q => q.Name).ToList();
            }
            else if (eb.EvaluationBoardType == 1 && staffType == 2) //cả năm nhân viên
            {
                var listRating = session.Query<ABC_Rating>().Where(r =>
                    //Tất cả rating là con 
                    r.ABC_EvaluationBoard.ABC_ParentEvaluationBoard.ABC_ParentEvaluationBoard.Id == eb.Id
                    && r.Staff.Id == ratingResult.Staff.Id
                    //&& r.Department.Id == ratingResult.Department.Id //không xét bộ phận vì nhân viên có thể đổi bộ phận
                    );
                var list_TatCaThangCon = session.Query<ABC_EvaluationBoard>().Where(q => q.ABC_ParentEvaluationBoard.ABC_ParentEvaluationBoard.Id == eb.Id);
                var list_ThangConDaKhoa = listRating.Where(q => q.IsSupervisorRated == true).Select(q => q.ABC_EvaluationBoard);
                var list_ThangConChuaKhoa = list_TatCaThangCon.Where(q => !list_ThangConDaKhoa.Any(qq => qq.Id == q.Id));

                listString = list_ThangConChuaKhoa.OrderBy(q => q.Month).Select(q => q.Month.ToString()).ToList();
            }
            else if (eb.EvaluationBoardType == 2 && staffType == 2) //6 tháng nhân viên
            {
                var listRating = session.Query<ABC_Rating>().Where(r =>
                    //Tất cả rating là con 
                    r.ABC_EvaluationBoard.ABC_ParentEvaluationBoard.Id == eb.Id
                    && r.Staff.Id == ratingResult.Staff.Id
                    //&& r.Department.Id == ratingResult.Department.Id //không xét bộ phận vì nhân viên có thể đổi bộ phận
                    );
                var list_TatCaThangCon = session.Query<ABC_EvaluationBoard>().Where(q => q.ABC_ParentEvaluationBoard.Id == eb.Id);
                var list_ThangConDaKhoa = listRating.Where(q => q.IsSupervisorRated == true).Select(q => q.ABC_EvaluationBoard);
                var list_ThangConChuaKhoa = list_TatCaThangCon.Where(q => !list_ThangConDaKhoa.Any(qq => qq.Id == q.Id));
                listString = list_ThangConChuaKhoa.OrderBy(q => q.Month).Select(q => q.Month.ToString()).ToList();
            }

            result = String.Join(", ", listString);
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_TitleDTO> GetListTitle()
        {
            List<ABC_TitleDTO> result = new List<ABC_TitleDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_Title>().OrderBy(q => q.OrderNumber).Map<ABC_TitleDTO>().ToList();
            });
            return result;
        }
    }
}
