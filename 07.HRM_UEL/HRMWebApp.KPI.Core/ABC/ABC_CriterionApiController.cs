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

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_CriterionApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_CriterionDTO> GetList()
        {
            List<ABC_CriterionDTO> result = new List<ABC_CriterionDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_Criterion>().Map<ABC_CriterionDTO>().OrderBy(c=>c.OrderNumber).ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_CriterionDTO> GetListByRatingType(Guid ratingTypeId)
        {
            List<ABC_CriterionDTO> result = new List<ABC_CriterionDTO>();
            SessionManager.DoWork(session =>
            {
                var listCriterion = session.Query<ABC_Criterion>().Where(q => q.ABC_RatingType.Id == ratingTypeId).OrderBy(q => q.OrderNumber).ToList();
                foreach (var cri in listCriterion)
                {
                    var criDTO = new ABC_CriterionDTO();
                    criDTO = cri.Map<ABC_CriterionDTO>();
                    criDTO.ABC_RatingTypeDTO = cri.ABC_RatingType.Map<ABC_RatingTypeDTO>();
                    result.Add(criDTO);
                }
                result = result.OrderBy(q => q.OrderNumber).ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_CriterionDTO> GetListByRatingTypeHierarchy(Guid ratingTypeId)
        {
            List<ABC_CriterionDTO> result = new List<ABC_CriterionDTO>();
            SessionManager.DoWork(session =>
            {
                List<Guid> criterionIds = session.Query<ABC_Criterion>().Where(q => q.ABC_RatingType.Id == ratingTypeId).OrderBy(q => q.OrderNumber).Select(q => q.Id).ToList();
                var all = session.Query<ABC_Criterion>().Map<ABC_CriterionDTO>().OrderBy(c => c.OrderNumber).ToList();
                foreach (var cri in all)
                {
                    cri.Name = cri.OrderNumber + ". " + cri.Name;
                    if (criterionIds.Contains(cri.Id))
                        cri.@checked = true;
                    result.Add(cri);
                }
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public ABC_CriterionDTO PutCriterion(ABC_CriterionDTO obj)
        {
            SessionManager.DoWork(session =>
            {
                var objDB = session.Query<ABC_Criterion>().SingleOrDefault(q => q.Id == obj.Id);
                if (objDB == null)
                {
                    objDB = new ABC_Criterion();
                    objDB.Id = Guid.NewGuid();
                }
                objDB.Name = obj.Name;
                objDB.CriterionType = obj.CriterionType;
                objDB.OrderNumber = obj.OrderNumber;
                objDB.IsNotVisibleInEvaluationBoardType = obj.IsNotVisibleInEvaluationBoardType;
                objDB.ABC_RatingType = new ABC_RatingType();
                objDB.ABC_RatingType.Id = obj.ABC_RatingTypeDTO.Id;
                objDB.CopyFromCriterion = obj.CopyFromCriterion;
                objDB.IsGroupByRatingTypeOnEvaluationBoard = obj.IsGroupByRatingTypeOnEvaluationBoard;
                session.SaveOrUpdate(objDB);
                obj.CriterionCopyIds = new List<Guid>();
                if (obj.CopyToRatingTypes != null)
                {
                    foreach (var ratingTypeId in obj.CopyToRatingTypes)
                    {
                        ABC_Criterion copy = new ABC_Criterion();
                        copy.Id = Guid.NewGuid();
                        copy.ABC_RatingType = new ABC_RatingType();
                        copy.ABC_RatingType.Id = ratingTypeId;
                        copy.CopyFromCriterion = objDB.Id;
                        copy.Name = objDB.Name;
                        copy.CriterionType = objDB.CriterionType;
                        copy.OrderNumber = objDB.OrderNumber;
                        copy.IsNotVisibleInEvaluationBoardType = objDB.IsNotVisibleInEvaluationBoardType;
                        copy.IsGroupByRatingTypeOnEvaluationBoard = false;
                        obj.CriterionCopyIds.Add(copy.Id);
                        session.Save(copy);
                    }
                }
            });
            return obj;
        }

        [Authorize]
        [Route("")]
        public int PutCriterionsOfRatingTypes(ABC_RatingTypeDTO ratingTypeDTO)
        {
            int result = 0;
            SessionManager.DoWork(session => {
                var ratingType = session.Query<ABC_RatingType>().Where(q => q.Id == ratingTypeDTO.Id).SingleOrDefault();
                if (ratingTypeDTO.RatingTypeIncludedIds != null)
                {
                    foreach (var Id in ratingTypeDTO.RatingTypeIncludedIds)
                    {
                        var ratingTypeIncluded = session.Query<ABC_RatingType>().Where(q => q.Id == Id).SingleOrDefault();
                        var criterions = session.Query<ABC_Criterion>().Where(q => q.ABC_RatingType.Id == Id).ToList();
                        foreach (var cri in criterions)
                        {
                            ABC_Criterion newCri = new ABC_Criterion();
                            newCri.Id = Guid.NewGuid();
                            newCri.Name = cri.Name;
                            newCri.CriterionType = cri.CriterionType;
                            newCri.OrderNumber = cri.OrderNumber;
                            newCri.IsNotVisibleInEvaluationBoardType = cri.IsNotVisibleInEvaluationBoardType;
                            newCri.ABC_RatingType = new ABC_RatingType();
                            newCri.ABC_RatingType = ratingType;
                            newCri.CopyFromCriterion = cri.Id;
                            newCri.CopyFromRatingType = new ABC_RatingType();
                            newCri.CopyFromRatingType.Id = Id;
                            newCri.IsGroupByRatingTypeOnEvaluationBoard = ratingTypeDTO.IsGroupOnEvaluationBoard;
                            session.Save(newCri);
                        }
                    }
                }
                result = 1;
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public ABC_CriterionDTO GetCriterionById(Guid id)
        {
            ABC_CriterionDTO result = new ABC_CriterionDTO();
            SessionManager.DoWork(session =>
            {
                var cri = session.Query<ABC_Criterion>().Where(c => c.Id == id).SingleOrDefault();
                result = cri.Map<ABC_CriterionDTO>();
                result.ABC_RatingTypeDTO = new ABC_RatingTypeDTO();
                result.ABC_RatingTypeDTO = cri.ABC_RatingType.Map<ABC_RatingTypeDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_CriterionDetailDTO> GetListCriterionDetailByCriterionId(Guid id)
        {
            List<ABC_CriterionDetailDTO> result = new List<ABC_CriterionDetailDTO>();
            SessionManager.DoWork(session =>
            {
                var list = session.Query<ABC_CriterionDetail>().Where(c => c.ABC_Criterion.Id==id).OrderBy(c => c.OrderNumber).ToList();
                foreach (ABC_CriterionDetail cd in list)
                {
                    ABC_CriterionDetailDTO cdd = new ABC_CriterionDetailDTO();
                    cdd.Id = cd.Id;
                    cdd.ABC_CriterionDetailTypeId = cd.ABC_CriterionDetailType != null ? cd.ABC_CriterionDetailType.Id : 0;
                    cdd.ABC_CriterionId = cd.ABC_Criterion != null ? cd.ABC_Criterion.Id : Guid.Empty;
                    cdd.MaxRecord = cd.MaxRecord;
                    cdd.Name = cd.Name;
                    result.Add(cdd);
                }
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public ABC_CriterionDetailDTO PutCriterionDetail(ABC_CriterionDetailDTO obj)
        {
            SessionManager.DoWork(session =>
            {
                if (obj.Id == Guid.Empty)
                {
                    ABC_CriterionDetail objsave = new ABC_CriterionDetail();
                    objsave.Id = Guid.NewGuid();
                    objsave.ABC_Criterion = new ABC_Criterion() { Id = obj.ABC_CriterionId };
                    objsave.ABC_CriterionDetailType = new ABC_CriterionDetailType() {Id=obj.ABC_CriterionDetailTypeId };
                    objsave.MaxRecord = obj.MaxRecord;
                    objsave.Name = obj.Name;
                    objsave.OrderNumber = obj.OrderNumber;
                    session.Save(objsave);
                }
                else
                {
                    ABC_CriterionDetail objsave = session.Query<ABC_CriterionDetail>().Where(c => c.Id == obj.Id).SingleOrDefault();
                    objsave.MaxRecord = obj.MaxRecord;
                    objsave.Name = obj.Name;
                    objsave.OrderNumber = obj.OrderNumber;
                    objsave.ABC_CriterionDetailType = new ABC_CriterionDetailType() { Id = obj.ABC_CriterionDetailTypeId };
                    session.Update(objsave);
                }
            });
            return obj;
        }

        [Authorize]
        [Route("")]
        public int PutRatingDetailOfCriterions(ABC_RatingTypeDTO ratingTypeDTO)
        {
            int result = 0;
            SessionManager.DoWork(session =>
            {
                var criterions = session.Query<ABC_Criterion>().Where(q => q.ABC_RatingType.Id == ratingTypeDTO.Id).ToList();
                foreach (var criterion in criterions)
                {
                    var criterionDetails = session.Query<ABC_CriterionDetail>().Where(q => q.ABC_Criterion.Id == criterion.CopyFromCriterion).ToList();
                    foreach (var criDetail in criterionDetails)
                    {
                        ABC_CriterionDetail newCriDetail = new ABC_CriterionDetail();
                        newCriDetail.Id = Guid.NewGuid();
                        newCriDetail.Name = criDetail.Name;
                        newCriDetail.MaxRecord = criDetail.MaxRecord;
                        newCriDetail.OrderNumber = criDetail.OrderNumber;
                        newCriDetail.ABC_Criterion = new ABC_Criterion();
                        newCriDetail.ABC_Criterion = criterion;
                        newCriDetail.ABC_CriterionDetailType = new ABC_CriterionDetailType();
                        newCriDetail.ABC_CriterionDetailType = criDetail.ABC_CriterionDetailType;
                        session.Save(newCriDetail);
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int PutCriterionDetailsOfCriterionCopys(ABC_CriterionDTO criterionDTO)
        {
            int result = 0;
            SessionManager.DoWork(session =>
            {
                var criterionDetails = session.Query<ABC_CriterionDetail>().Where(q => q.ABC_Criterion.Id == criterionDTO.Id).ToList();
                foreach (var criterionDetail in criterionDetails)
                {
                    foreach (var criterionCopyId in criterionDTO.CriterionCopyIds)
                    {
                        ABC_CriterionDetail newCriDetail = new ABC_CriterionDetail();
                        newCriDetail.Id = Guid.NewGuid();
                        newCriDetail.ABC_Criterion = new ABC_Criterion();
                        newCriDetail.ABC_Criterion.Id = criterionCopyId;
                        newCriDetail.Name = criterionDetail.Name;
                        newCriDetail.MaxRecord = criterionDetail.MaxRecord;
                        newCriDetail.OrderNumber = criterionDetail.OrderNumber;
                        newCriDetail.ABC_CriterionDetailType = new ABC_CriterionDetailType();
                        newCriDetail.ABC_CriterionDetailType.Id = criterionDetail.ABC_CriterionDetailType.Id;
                        session.Save(newCriDetail);
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public ABC_CriterionDetailDTO GetCriterionDetailById(Guid id)
        {
            ABC_CriterionDetailDTO result = new ABC_CriterionDetailDTO();
            SessionManager.DoWork(session =>
            {
                ABC_CriterionDetail temp = session.Query<ABC_CriterionDetail>().Where(c => c.Id == id).SingleOrDefault();
                result.Id = temp.Id;
                result.ABC_CriterionDetailTypeId = temp.ABC_CriterionDetailType != null ? temp.ABC_CriterionDetailType.Id : 0;
                result.ABC_CriterionId = temp.ABC_Criterion != null ? temp.ABC_Criterion.Id : Guid.Empty;
                result.MaxRecord = temp.MaxRecord;
                result.Name = temp.Name;
                result.OrderNumber = temp.OrderNumber;
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public bool GetCheckHasCriterionDetail(Guid id)
        {
            bool result = false;
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_CriterionDetail>().Any(c => c.ABC_Criterion.Id == id);
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public int DeleteCriterionDetail(ABC_CriterionDetail obj)
        {
            int success = 0;
            SessionManager.DoWork(session =>
            {
                //bool check = session.Query<PlanKPIDetail>().Any(p => p.CurrentKPI.ToString() == obj.Id.ToString());
                //if (check == false)
                //{
                try
                {
                    session.Delete(obj);
                    success = 1;
                }
                    catch (Exception e)
                {
                    success = 0;
                }
                //}
            });
            return success;
        }
        [Authorize]
        [Route("")]
        public int DeleteCriterion(ABC_CriterionDTO obj)
        {
            int success = 0;
            SessionManager.DoWork(session =>
            {
                try
                {
                    var deleted = session.Query<ABC_Criterion>().SingleOrDefault(q => q.Id == obj.Id);
                    if (deleted != null)
                    {
                        session.Delete(deleted);
                        success = 1;
                    }   
                }
                catch (Exception e)
                {
                    success = 0;
                }
            });
            return success;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_CriterionDetailType> GetListCriterionDetailType()
        {
            List<ABC_CriterionDetailType> result = new List<ABC_CriterionDetailType>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_CriterionDetailType>().ToList();
            });
            return result;
        }
    }
}
