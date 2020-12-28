using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using NHibernate.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.Helpers;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ScienceResearchDataApiController:ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ScienceResearchDataDTO> GetList()
        {
            var result = new List<ScienceResearchDataDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ScienceResearchData>().OrderBy(d =>d.StaffCode).Map<ScienceResearchDataDTO>().ToList();
              
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public ScienceResearchDataDTO GetObj(Guid id)
        {
            var result = new ScienceResearchDataDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ScienceResearchData>().SingleOrDefault(a => a.Id == id).Map<ScienceResearchDataDTO>();
                //if (sr != null)
                //{
                //    result.Id = sr.Id;
                //    result.Name = sr.CriterionDictionary.Name;
                //    result.PlanKPIDetailId = sr.PlanKPIDetail != null ? sr.PlanKPIDetail.Id : Guid.Empty;
                //    result.NumberOfResearch = sr.NumberOfResearch;

                //}
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public Guid Put(ScienceResearchData obj)
        {
            Guid result = Guid.Empty;
            try
            {
                SessionManager.DoWork(session =>
                {
                    session.SaveOrUpdate(obj);
                });
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public ScienceResearchData Delete(ScienceResearchData obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }
    }
}
