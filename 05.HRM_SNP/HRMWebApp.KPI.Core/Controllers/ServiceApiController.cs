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
using HRMWebApp.KPI.Core.DTO.RatingKPIDTOs;


namespace HRMWebApp.KPI.Core.Controllers
{
    public class ServiceApiController : ApiController
    {
        /// <summary>
        /// Lấy số tiết chuẩn theo giảng viên
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="agentObjectId"></param>
        /// <param name="requestType">1: lấy để soạn kế hoạch. 2: lấy để đánh giá thông qua service bên ngoài</param>
        /// <returns></returns>
        public int GetNumberOfSection(Guid staffId,Guid agentObjectId,int requestType)
        {
            int result = 0;
            SessionManager.DoWork(session =>
            {
                Staff selectedStaff = session.Query<Staff>().SingleOrDefault(s => s.Id == staffId);
                switch(requestType)
                {
                    case 1:
                        {
                            result = session.Query<AgentObject>().SingleOrDefault(a => a.Id == agentObjectId).AgentObjectDetail.NumberOfSection;
                        }
                        break;
                }                          
            });
            return result;
        }
    }
}
