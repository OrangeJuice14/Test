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
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;
using HRMWebApp.KPI.Core.Helpers;


namespace HRMWebApp.KPI.Core.Controllers
{
    public class StudyYearApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<StudyYearDTO> GetList()
        {
            List<StudyYearDTO> result = new List<StudyYearDTO>();
            SessionManager.DoWork(session =>
            {
                List<StudyYear> objectResult = session.Query<StudyYear>().ToList();
                result = session.Query<StudyYear>().ToList().Map<StudyYearDTO>();
            });
            return result;
        }
    }
}
