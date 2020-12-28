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
    public class TargetGroupDetailApiController : ApiController
    {
        public IEnumerable<TargetGroupDTO> GetList()
        {
            List<TargetGroupDTO> result = new List<TargetGroupDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<TargetGroupDetail>().ToList().Map<TargetGroupDTO>();
            });
            return result;
        }
        public TargetGroupDetailDTO GetObj(Guid id)
        {
            TargetGroupDetailDTO result = new TargetGroupDetailDTO();
            SessionManager.DoWork(session =>
            {
                TargetGroupDetail tgd = session.Query<TargetGroupDetail>().SingleOrDefault(a => a.Id == id);
                if(tgd!=null)
                {
                    result = tgd.Map<TargetGroupDetailDTO>();
                    result.AgentObjectIds = new List<Guid>();
                    foreach(AgentObject ag in tgd.AgentObjects)
                    {
                        result.AgentObjectIds.Add(ag.Id);
                    }
                    //result.TargetGroupId = result.TargetGroup.Id;
                }                              
            });
            return result;
        }

        public int GetTargetGroupDetailTypeId(Guid id)
        {
            int result = 0;
            SessionManager.DoWork(session =>
            {
                TargetGroupDetail tgd = session.Query<TargetGroupDetail>().SingleOrDefault(a => a.Id == id);
                result = tgd.TargetGroupDetailType.Id;
            });
            return result;
        }

        public TargetGroupDetailDTO GetTargetGroupDetailNameByCriterion(Guid id)
        {
            TargetGroupDetailDTO result = new TargetGroupDetailDTO();
            SessionManager.DoWork(session =>
            {
                ProfessorCriterion cri = session.Query<ProfessorCriterion>().SingleOrDefault(a => a.Id == id);
                if (cri!=null)
                {
                    result.Id = cri.TargetGroupDetail.Id;
                    result.Name = cri.TargetGroupDetail.Name;
                    result.TargetGroupDetailTypeId = cri.TargetGroupDetail.TargetGroupDetailType.Id;
                }            
            });
            return result;
        }

        public IEnumerable<TargetGroupDetailDTO> GetListbyId(Guid classId)
        {
            List<TargetGroupDetailDTO> result = new List<TargetGroupDetailDTO>();
            SessionManager.DoWork(session =>
            {
                if (classId == Guid.Empty)
                {
                    List<TargetGroupDetail> tglist = new List<TargetGroupDetail>();
                    tglist = session.Query<TargetGroupDetail>().ToList();
                    foreach (TargetGroupDetail tg in tglist)
                    {
                        TargetGroupDetailDTO tgd = new TargetGroupDetailDTO();
                        tgd.Id = tg.Id;
                        tgd.Name = tg.Name;
                        tgd.Density = tg.Density;
                        tgd.OrderNumber = tg.OrderNumber;
                        tgd.TargetGroupDetailTypeId = tg.TargetGroupDetailType != null ? tg.TargetGroupDetailType.Id : 0;
                        result.Add(tgd);
                    }
                }
                else
                {
                    List<TargetGroupDetail> tglist = new List<TargetGroupDetail>();
                    tglist = session.Query<TargetGroupDetail>().Where(a => a.AgentObjects.Any(ag => ag.Id == classId)).ToList();
                    foreach (TargetGroupDetail tg in tglist)
                    {
                        TargetGroupDetailDTO tgd = new TargetGroupDetailDTO();
                        tgd.Id = tg.Id;
                        tgd.Name = tg.Name;
                        tgd.Density = tg.Density;
                        tgd.OrderNumber = tg.OrderNumber;
                        tgd.TargetGroupDetailTypeId = tg.TargetGroupDetailType != null ? tg.TargetGroupDetailType.Id : 0;
                        result.Add(tgd);
                    }
                }
            });
            return result;
        }

        public TargetGroupDetail Put(TargetGroupDetailDTO obj)
        {

            TargetGroupDetail targetGroupDetail = new TargetGroupDetail();
            //targetGroupDetail = obj.Map<TargetGroupDetail>();
            SessionManager.DoWork(session =>
            {
                if (obj.Id == Guid.Empty)
                {
                    targetGroupDetail.Id = Guid.NewGuid();
                }
                else
                {
                    targetGroupDetail = session.Query<TargetGroupDetail>().Where(t => t.Id == obj.Id).SingleOrDefault();
                    targetGroupDetail.Name = obj.Name;
                    targetGroupDetail.Density = obj.Density;
                    targetGroupDetail.AgentObjects = new List<AgentObject>();
                }

                foreach (var id in obj.AgentObjectIds)
                {
                    targetGroupDetail.AgentObjects.Add(new AgentObject() { Id = id });
                }
                session.SaveOrUpdate(targetGroupDetail);
            });
            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            return targetGroupDetail;

        }

        public IEnumerable<TargetGroupDTO> GetListbyAgentObjectId(Guid agentObjectId)
        {
            List<TargetGroupDTO> result = new List<TargetGroupDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<TargetGroupDetail>().Where(a => a.AgentObjects.Any(ag=>ag.Id==agentObjectId)).Map<TargetGroupDTO>();
            });
            return result;
        }

        public TargetGroupDetail Delete(TargetGroupDetail obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
        }
    }
}
