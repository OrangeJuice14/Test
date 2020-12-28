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
    public class TargetGroupApiController:ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<TargetGroupDTO> GetList()
        {
            List<TargetGroupDTO> result = new List<TargetGroupDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<TargetGroup>().ToList().Map<TargetGroupDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public TargetGroupDTO GetObj(Guid id)
        {
            TargetGroupDTO result = new TargetGroupDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<TargetGroup>().SingleOrDefault(a => a.Id == id).Map<TargetGroupDTO>();
                result.AgentObjectId = result.AgentObject.Id;
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<TargetGroupDTO> GetListbyId(Guid classId)
        {
            List<TargetGroupDTO> result = new List<TargetGroupDTO>();                
            SessionManager.DoWork(session =>
            {
                if (classId == Guid.Empty )
                {
                    result = session.Query<TargetGroup>().ToList().Map<TargetGroupDTO>();
                }
                else
                {
                    result = session.Query<TargetGroup>().Where(a => a.AgentObject.Id == classId).Map<TargetGroupDTO>();
                }              
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public TargetGroup Put(TargetGroup obj)
        {
            if (obj.Id == Guid.Empty)
                obj.Id = Guid.NewGuid();
            SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            return obj;
        }

        [Authorize]
        [Route("")]
        public TargetGroup Delete(TargetGroup obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
        }
    }
}
