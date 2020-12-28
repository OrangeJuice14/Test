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
    public class ABC_RatingTypeApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_RatingTypeDTO> GetList()
        {
            List<ABC_RatingTypeDTO> result = new List<ABC_RatingTypeDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_RatingType>().Map<ABC_RatingTypeDTO>().OrderBy(c => c.Type).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public ABC_RatingTypeDTO GetObj(Guid id)
        {
            ABC_RatingTypeDTO result = new ABC_RatingTypeDTO();
            SessionManager.DoWork(session =>
            {
                var ratingType = session.Query<ABC_RatingType>().SingleOrDefault(q => q.Id == id);
                result = ratingType.Map<ABC_RatingTypeDTO>();
                result.CriterionIds = new List<Guid>();
                foreach (var c in ratingType.ABC_Criterions)
                {
                    result.CriterionIds.Add(c.Id);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int PutCriterionManage(ABC_RatingTypeDTO obj)
        {
            try
            {
                int result = 0;
                SessionManager.DoWork(session =>
                {
                    var ratingType = session.Query<ABC_RatingType>().SingleOrDefault(q => q.Id == obj.Id);
                    ratingType.ABC_Criterions = new List<ABC_Criterion>();
                    foreach (var id in obj.CriterionIds)
                    {
                        ratingType.ABC_Criterions.Add(new ABC_Criterion() { Id = id });
                    }
                    session.Update(ratingType);
                    result = 1;
                });
                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_RatingTypeApiController/PutCriterionManage", ex);
                throw;
            }
        }
    }
}
