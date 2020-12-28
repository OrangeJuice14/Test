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
using System;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class TaskKPIApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<TaskKPIDTO> GetList()
        {
            List<TaskKPIDTO> result = new List<TaskKPIDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<TaskKPI>().ToList().Map<TaskKPIDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public TaskKPIDTO GetClass(Guid id)
        {
            TaskKPIDTO result = new TaskKPIDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<TaskKPI>().SingleOrDefault(a => a.Id == id).Map<TaskKPIDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public TaskKPI Put(TaskKPI obj)
        {
            if (obj.Id == Guid.Empty)
                obj.Id = Guid.NewGuid();
            SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            return obj;
        }

        [Authorize]
        [Route("")]
        public TaskKPI Delete(TaskKPI obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
        }
    }
}
