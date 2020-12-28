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
using NHibernate;
using HRMWebApp.KPI.Core.DTO.ABC;

namespace HRMWebApp.KPI.Core.ABC
{
    public class ABC_RatingTypeApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_RatingTypeDTO> GetList()
        {
            List<ABC_RatingTypeDTO> result = new List<ABC_RatingTypeDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_RatingType>().Map<ABC_RatingTypeDTO>().OrderBy(c => c.OrderNumber).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public ABC_RatingTypeDTO GetObj(Guid id)
        {
            ABC_RatingTypeDTO ratingTypeDTO = new ABC_RatingTypeDTO();
            SessionManager.DoWork(session =>
            {
                var ratingType = session.Query<ABC_RatingType>().SingleOrDefault(q => q.Id == id);
                ratingTypeDTO = ratingType.Map<ABC_RatingTypeDTO>();
                //ratingTypeDTO.RatingTypeIncludedIds = session.Query<ABC_Criterion>().Where(q => q.ABC_RatingType.Id == id && q.IncludedFromRatingType != null)
                //                                                                    .Select(q => q.IncludedFromRatingType.Id).Distinct().ToList();
            });
            return ratingTypeDTO;
        }

        [Authorize]
        [Route("")]
        public ABC_RatingTypeDTO GetStaffRatingType(Guid staffId)
        {
            ABC_RatingTypeDTO ratingTypeDTO = new ABC_RatingTypeDTO();
            SessionManager.DoWork(session => {
                var obj = session.Query<ThongTinDanhGia>().Where(q => q.StaffInfo.Id == staffId).SingleOrDefault();
                if (obj != null)
                {
                    ratingTypeDTO.Id = obj.ABC_RatingType.Id;
                    ratingTypeDTO.Name = obj.ABC_RatingType.Name;
                    ratingTypeDTO.OrderNumber = obj.ABC_RatingType.OrderNumber;
                }
            });
            return ratingTypeDTO;
        }

        [Authorize]
        [Route("")]
        public int PutStaffRatingType(ThongTinDanhGiaDTO obj)
        {
            int result = 0;
            try
            {
                SessionManager.DoWork(session => {
                    var thongTinDG = session.Query<ThongTinDanhGia>().Where(q => q.StaffInfo.Id == obj.StaffId).SingleOrDefault();
                    if (thongTinDG == null)
                    {
                        thongTinDG = new ThongTinDanhGia();
                        thongTinDG.Id = Guid.NewGuid();
                        thongTinDG.StaffInfo = new StaffInfo();
                        thongTinDG.StaffInfo.Id = obj.StaffId;
                    }
                    thongTinDG.ABC_RatingType = new ABC_RatingType();
                    thongTinDG.ABC_RatingType.Id = obj.RatingTypeId;
                    session.SaveOrUpdate(thongTinDG);
                    result = 1;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public ABC_RatingTypeDTO PutRatingType(ABC_RatingTypeDTO ratingTypeDTO)
        {
            try
            {
                SessionManager.DoWork(session => {
                    var ratingType = session.Query<ABC_RatingType>().Where(r => r.Id == ratingTypeDTO.Id).SingleOrDefault();
                    if (ratingType == null)
                    {
                        ratingType = new ABC_RatingType();
                        ratingType.Id = Guid.NewGuid();
                        ratingTypeDTO.Id = ratingType.Id;
                    }
                    ratingType.Name = ratingTypeDTO.Name;
                    ratingType.OrderNumber = ratingTypeDTO.OrderNumber;
                    session.SaveOrUpdate(ratingType);
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_RatingTypeApiController/PutRatingType", ex);
                throw ex;
            }
            return ratingTypeDTO;
        }

        //[Authorize]
        //[Route("")]
        //public int PutCriterionManage(ABC_RatingTypeDTO obj)
        //{
        //    int result = 0;
        //    try
        //    {
        //        SessionManager.DoWork(session =>
        //        {
        //            var ratingType = session.Query<ABC_RatingType>().SingleOrDefault(q => q.Id == obj.Id);
        //            ratingType.ABC_Criterions = new List<ABC_Criterion>();
        //            foreach (var id in obj.CriterionIds)
        //            {
        //                ratingType.ABC_Criterions.Add(new ABC_Criterion() { Id = id });
        //            }
        //            session.Update(ratingType);
        //            result = 1;
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Helper.ErrorLog("ABC_RatingTypeApiController/PutCriterionManage", ex);
        //        throw;
        //    }
        //    return result;
        //}

        [Authorize]
        [Route("")]
        public int DeleteRatingType(ABC_RatingType obj)
        {
            int success = 0;
            try
            {
                SessionManager.DoWork(session => {
                    session.Delete(obj);
                    success = 1;
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_RatingTypeApiController/DeleteRatingType", ex);
                throw ex;
            }
            return success;
        }
    }
}
