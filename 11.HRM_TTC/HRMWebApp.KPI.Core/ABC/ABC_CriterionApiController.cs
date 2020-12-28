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
using HRMWebApp.KPI.Core.DTO.ABC;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_CriterionApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_CriterionDTO> GetListByRatingType(Guid ratingType)
        {
            List<ABC_CriterionDTO> result = new List<ABC_CriterionDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_Criterion>().Where(q => q.RatingTypes.Any(qq => qq.Id == ratingType)).OrderBy(q => q.OrderNumber).Map<ABC_CriterionDTO>().ToList();
                //var criterionRatingType = session.Query<ABC_Criterion_RatingType>()
                //                                     .Where(c => c.ABC_RatingType.Id == ratingType)
                //                                     .OrderBy(c => c.ABC_Criterion.OrderNumber).ToList() ;

                //foreach (var item in criterionRatingType)
                //{
                //    result.Add(item.ABC_Criterion.Map<ABC_CriterionDTO>());
                //}    
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_CriterionDTO> GetListByRatingTypeHierarchy(Guid ratingType)
        {
            List<ABC_CriterionDTO> result = new List<ABC_CriterionDTO>();
            SessionManager.DoWork(session =>
            {
                List<Guid> criterionIds = session.Query<ABC_Criterion>().Where(q => q.RatingTypes.Any(qq => qq.Id == ratingType)).OrderBy(q => q.OrderNumber).Select(q => q.Id).ToList();
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
        public IEnumerable<ABC_RatingLevelDTO> GetListRatingLevel()
        {
            List<ABC_RatingLevelDTO> result = new List<ABC_RatingLevelDTO>();
            SessionManager.DoWork(session => {
                result = session.Query<ABC_RatingLevel>().Map<ABC_RatingLevelDTO>().ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public ABC_RatingLevelDTO GetRatingLevel(Guid id, Guid criterionId)
        { 
            ABC_RatingLevelDTO ratingLevelDTO = new ABC_RatingLevelDTO();
            SessionManager.DoWork(session => {
                var obj = session.Query<ABC_Criterion_RatingLevel>().Where(q => (q.ABC_Criterion.Id == criterionId) && (q.ABC_RatingLevel.Id == id)).SingleOrDefault();
                ratingLevelDTO.Id = id;
                if (obj != null)
                {
                    ratingLevelDTO.Name = obj.ABC_RatingLevel.Name;
                    ratingLevelDTO.Description = obj.Description;
                    ratingLevelDTO.CriterionId = criterionId;
                } else
                {
                    var temp = session.Query<ABC_RatingLevel>().Where(q => q.Id == id).SingleOrDefault();
                    ratingLevelDTO.Name = temp.Name;
                }
            });
            return ratingLevelDTO;
        }

        [Authorize]
        [Route("")]
        public ABC_RatingLevelDTO GetTempRatingLevelById(Guid id)
        {
            ABC_RatingLevelDTO ratingLevelDTO = new ABC_RatingLevelDTO();
            SessionManager.DoWork(session => {
                var criTemp = PutTempCriterion();
                var obj = session.Query<ABC_Criterion_RatingLevel>().Where(q => q.ABC_RatingLevel.Id == id && q.ABC_Criterion.Id == criTemp.Id && q.IsTemp).SingleOrDefault();
                if (obj == null)
                {
                    var temp = new ABC_Criterion_RatingLevel();
                    temp.ABC_Criterion = new ABC_Criterion();
                    temp.ABC_Criterion.Id = criTemp.Id;
                    temp.ABC_RatingLevel = new ABC_RatingLevel();
                    temp.ABC_RatingLevel.Id = id;
                    temp.IsTemp = true;

                    obj = new ABC_Criterion_RatingLevel();
                    obj.ABC_Criterion = new ABC_Criterion();
                    obj.ABC_Criterion.Id = temp.ABC_Criterion.Id;
                    obj.ABC_RatingLevel = new ABC_RatingLevel();
                    obj.ABC_RatingLevel.Id = temp.ABC_RatingLevel.Id;
                    obj.ABC_RatingLevel.Name = temp.ABC_RatingLevel.Name;
                    obj.Description = temp.Description;
                    obj.IsTemp = true;

                    ratingLevelDTO.Id = id;
                    ratingLevelDTO.CriterionId = criTemp.Id;
                    ratingLevelDTO.Name = obj.ABC_RatingLevel.Name;
                    ratingLevelDTO.Description = obj.Description;

                    session.Save(temp);
                } else
                {
                    ratingLevelDTO.Id = id;
                    ratingLevelDTO.CriterionId = criTemp.Id;
                    ratingLevelDTO.Name = obj.ABC_RatingLevel.Name;
                    ratingLevelDTO.Description = obj.Description;
                }
            });
            return ratingLevelDTO;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_RatingTypeDTO> GetRatingTypesByCriterionId(Guid id)
        {
            List<ABC_RatingTypeDTO> listRatingTypeDTO = new List<ABC_RatingTypeDTO>();
            SessionManager.DoWork(session => {
                var ratingTypes = session.Query<ABC_RatingType>().Where(q => q.ABC_Criterions.Any(qq => qq.Id == id)).OrderBy(q => q.Type).ToList();
                if (ratingTypes.Any())
                {
                    foreach (var item in ratingTypes)
                    {
                        ABC_RatingTypeDTO obj = new ABC_RatingTypeDTO();
                        obj.Id = item.Id;
                        obj.Name = item.Name;
                        obj.Type = item.Type;
                        listRatingTypeDTO.Add(obj);
                    }
                }
            });
            return listRatingTypeDTO;
        }

        [Authorize]
        [Route("")]
        public ABC_RatingLevelDTO PutRatingLevel(ABC_RatingLevelDTO ratingLevelDTO)
        {
            try
            {
                SessionManager.DoWork(session => {
                    var obj = session.Query<ABC_Criterion_RatingLevel>()
                                     .Where(q => q.ABC_Criterion.Id == ratingLevelDTO.CriterionId
                                                 && q.ABC_RatingLevel.Id == ratingLevelDTO.Id).SingleOrDefault();
                    if (obj == null)
                    {
                        ratingLevelDTO.Id = Guid.NewGuid();
                        obj.ABC_Criterion.Id = ratingLevelDTO.CriterionId;
                        obj.ABC_RatingLevel.Id = ratingLevelDTO.Id;
                    }
                    obj.Description = ratingLevelDTO.Description;
                    session.SaveOrUpdate(obj);
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("/Api/ABC_CriterionApi/PutRatingLevel", ex);
                throw;
            }
            return ratingLevelDTO;
        }

        [Authorize]
        [Route("")]
        public ABC_CriterionDTO PutCriterion(ABC_CriterionDTO objDTO)
        {
            try
            {
                SessionManager.DoWork(session => {
                    var criterion = session.Query<ABC_Criterion>().Where(c => c.Id == objDTO.Id).SingleOrDefault();
                    criterion.Name = objDTO.Name;
                    criterion.CriterionDetail = objDTO.CriterionDetail;
                    criterion.Methods = objDTO.Methods;
                    criterion.Percents = objDTO.Percents;
                    criterion.OrderNumber = objDTO.OrderNumber;
                    criterion.IsNotVisibleInEvaluationBoardType = objDTO.IsNotVisibleInEvaluationBoardType;
                    if (objDTO.RatingTypeIds.Any())
                    {
                        criterion.RatingTypes = new List<ABC_RatingType>();
                        foreach (var item in objDTO.RatingTypeIds)
                        {
                            var ratingType = session.Query<ABC_RatingType>().Where(r => r.Id == item).SingleOrDefault();
                            if (ratingType != null && !criterion.RatingTypes.Contains(ratingType))
                            {
                                criterion.RatingTypes.Add(ratingType);
                            }
                        }
                    }
                    session.SaveOrUpdate(criterion);
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("/Api/ABC_CriterionApi/PutCriterion", ex);
                throw;
            }
            return objDTO;
        }

        [Authorize]
        [Route("")]
        public ABC_CriterionDTO PutNewCriterion(ABC_CriterionDTO criDTO)
        {
            try
            {
                SessionManager.DoWork(session => {
                    ABC_Criterion criterion = new ABC_Criterion();
                    criterion.Id = Guid.NewGuid();
                    criterion.RatingLevels = new List<ABC_RatingLevel>();
                    var listRatingLevel = session.Query<ABC_RatingLevel>().ToList();
                    foreach (var item in listRatingLevel)
                    {
                        criterion.RatingLevels.Add(item);
                    }
                    criterion.Name = criDTO.Name;
                    criterion.CriterionDetail = criDTO.CriterionDetail;
                    criterion.Methods = criDTO.Methods;
                    criterion.Percents = criDTO.Percents;
                    criterion.OrderNumber = criDTO.OrderNumber;
                    criterion.IsNotVisibleInEvaluationBoardType = criDTO.IsNotVisibleInEvaluationBoardType;
                    criterion.IsTemp = false;
                    if (criDTO.RatingTypeIds.Any())
                    {
                        criterion.RatingTypes = new List<ABC_RatingType>();
                        foreach (var item in criDTO.RatingTypeIds)
                        {
                            var ratingType = session.Query<ABC_RatingType>().Where(r => r.Id == item).SingleOrDefault();
                            if (ratingType != null && !criterion.RatingTypes.Contains(ratingType))
                            {
                                criterion.RatingTypes.Add(ratingType);
                            }
                        }
                    }
                    criDTO.Id = criterion.Id;
                    session.SaveOrUpdate(criterion);
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("/Api/ABC_CriterionApi/PutCriterion", ex);
                throw;
            }
            return criDTO;
        }

        [Authorize]
        [Route("")]
        public int PutRatingLevelOfNewCriterion(ABC_CriterionDTO criDTO)
        {
            int result = 0;
            try
            {
                SessionManager.DoWork(session => {
                    var listRatingLevel = session.Query<ABC_RatingLevel>().ToList();
                    foreach (var item in listRatingLevel)
                    {
                        var obj = session.Query<ABC_Criterion_RatingLevel>().Where(q => q.ABC_Criterion.Id == criDTO.Id && q.ABC_RatingLevel.Id == item.Id).SingleOrDefault();
                        var tempObj = session.Query<ABC_Criterion_RatingLevel>()
                             .Where(q => q.ABC_RatingLevel.Id == item.Id && q.IsTemp).SingleOrDefault();
                        obj.Description = tempObj.Description;
                        obj.IsTemp = false;
                        session.SaveOrUpdate(obj);
                        result = 1;
                    }
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("/Api/ABC_CriterionApi/PutRatingLevel(Guid newCriterionId)", ex);
                throw;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public ABC_Criterion GetCriterionById(Guid id)
        {
            ABC_Criterion result = new ABC_Criterion();
            SessionManager.DoWork(session =>
            {
                var criterion = session.Query<ABC_Criterion>().Where(c => c.Id == id).SingleOrDefault();
                if (criterion != null)
                {
                    result.Id = criterion.Id;
                    result.Name = criterion.Name;
                    result.CriterionDetail = criterion.CriterionDetail;
                    result.Methods = criterion.Methods;
                    result.Percents = criterion.Percents;
                    result.OrderNumber = criterion.OrderNumber;
                    result.IsNotVisibleInEvaluationBoardType = criterion.IsNotVisibleInEvaluationBoardType;
                    result.RatingTypes = new List<ABC_RatingType>();
                    foreach (var item in criterion.RatingTypes)
                    {
                        ABC_RatingType ratingType = new ABC_RatingType();
                        ratingType.Id = item.Id;
                        ratingType.Name = item.Name;
                        ratingType.Type = item.Type;
                        result.RatingTypes.Add(ratingType);
                    }
                    result.RatingLevels = new List<ABC_RatingLevel>();
                    foreach (var item in criterion.RatingLevels)
                    {
                        ABC_RatingLevel ratingLevel = new ABC_RatingLevel();
                        ratingLevel.Id = item.Id;
                        ratingLevel.Name = item.Name;
                        result.RatingLevels.Add(ratingLevel);
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int DeleteCriterion(ABC_Criterion obj)
        {
            int success = 0;
            SessionManager.DoWork(session =>
            {
                try
                {
                    session.Delete(obj);
                    success = 1;
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
        public ABC_RatingLevelDTO PutTempRatingLevel(ABC_RatingLevelDTO tempRatingLevelDTO)
        {
            try
            {
                ABC_CriterionDTO tempCriterionDTO = PutTempCriterion();
                SessionManager.DoWork(session => {
                    var obj = session.Query<ABC_Criterion_RatingLevel>()
                                     .Where(q => q.ABC_Criterion.Id == tempCriterionDTO.Id
                                                 && q.ABC_RatingLevel.Id == tempRatingLevelDTO.Id
                                                 && q.IsTemp).SingleOrDefault();
                    if (obj == null)
                    {
                        tempRatingLevelDTO.CriterionId = tempCriterionDTO.Id;
                        obj = new ABC_Criterion_RatingLevel();
                        obj.ABC_Criterion = new ABC_Criterion();
                        obj.ABC_Criterion.Id = tempCriterionDTO.Id;
                        obj.ABC_RatingLevel = new ABC_RatingLevel();
                        obj.ABC_RatingLevel.Id = tempRatingLevelDTO.Id;
                        obj.IsTemp = true;
                    }
                    tempRatingLevelDTO.Name = obj.ABC_RatingLevel.Name;
                    obj.Description = tempRatingLevelDTO.Description;
                    session.SaveOrUpdate(obj);
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("/Api/ABC_CriterionApi/PutTempRatingLevel", ex);
                throw;
            }
            return tempRatingLevelDTO;
        }

        [Authorize]
        [Route("")]
        public int GetTempRatingLevel()
        {
            int result = 0;
            SessionManager.DoWork(session => {
                var objs = session.Query<ABC_Criterion_RatingLevel>().Where(q => q.IsTemp).ToList();
                foreach (var item in objs)
                {
                    if (item != null)
                    {
                        item.Description = null;
                        result = 1;
                    }
                }
            });
            return result;
        }

        private ABC_CriterionDTO PutTempCriterion()
        {
            ABC_CriterionDTO tempCriterionDTO = new ABC_CriterionDTO();
            try
            {
                SessionManager.DoWork(session => {
                    var tempCriterion = session.Query<ABC_Criterion>().Where(c => c.IsTemp).SingleOrDefault();
                    if (tempCriterion == null)
                    {
                        tempCriterion = new ABC_Criterion();
                        tempCriterion.Id = Guid.NewGuid();
                        tempCriterion.IsTemp = true;
                    }
                    tempCriterionDTO.Id = tempCriterion.Id;
                    tempCriterionDTO.IsTemp = tempCriterion.IsTemp;
                    session.SaveOrUpdate(tempCriterion);
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("/Api/ABC_CriterionApi/PutTempRatingLevel", ex);
                throw;
            }
            return tempCriterionDTO;
        }
    }
}
