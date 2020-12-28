using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class WebUserApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public WebUserDTO GetWebUserByUserId(Guid UserId)
        {
            WebUserDTO result = new WebUserDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<WebUser>().Where(e => e.Id == UserId).Map<WebUserDTO>().FirstOrDefault();
            });
            return result;
        }
    }
}
