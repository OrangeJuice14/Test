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
        [Authorize]
        [Route("")]
        public IEnumerable<TargetGroupDTO> GetList()
        {
            List<TargetGroupDTO> result = new List<TargetGroupDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<TargetGroupDetail>().ToList().Map<TargetGroupDTO>();
            });
            return result;
        }
        //[Authorize]
        //[Route("")]
        //public IEnumerable<TargetGroupDTO> GetListTarget()
        //{
        //    List<TargetGroupDTO> result = new List<TargetGroupDTO>();
        //    SessionManager.DoWork(session =>
        //    {
        //        result = session.Query<TargetGroupDetail>().ToList().Map<TargetGroupDTO>();
        //    });
        //    return result;
        //}
        [Authorize]
        [Route("")]
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
                    result.StudyYearIds = new List<Guid>();
                    foreach (AgentObject ag in tgd.AgentObjects)
                    {
                        result.AgentObjectIds.Add(ag.Id);
                    }
                    foreach (StudyYear y in tgd.StudyYears)
                    {
                        result.StudyYearIds.Add(y.Id);
                    }

                    //result.TargetGroupDetailIds = new List<Guid>();
                    //foreach (TargetGroupDetail t in tgd.TagetGroupDetail)
                    //{
                    //    result.TargetGroupDetailIds.Add(t.Id);
                    //}                    
                    //result.TargetGroupId = result.TargetGroup.Id;
                }                              
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public List<TargetGroupDetailType> GetListTargetGroupDetailType()
        {
            List<TargetGroupDetailType> result = new List<TargetGroupDetailType>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<TargetGroupDetailType>().ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
        public TargetGroupDetailDTO GetTargetGroupDetailNameByCriterion(Guid id)
        {
            TargetGroupDetailDTO result = new TargetGroupDetailDTO();
            SessionManager.DoWork(session =>
            {
                ProfessorCriterion cri = session.Query<ProfessorCriterion>().SingleOrDefault(a => a.Id == id);
                if (cri != null)
                {
                    result.Id = cri.TargetGroupDetail.Id;
                    result.Name = cri.TargetGroupDetail.Name;
                    result.TargetGroupDetailTypeId = cri.TargetGroupDetail.TargetGroupDetailType.Id;
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
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
                }
                targetGroupDetail.OrderNumber = obj.OrderNumber;
                targetGroupDetail.Name = obj.Name;
                targetGroupDetail.Density = obj.Density;
                targetGroupDetail.TargetGroupDetailType = new TargetGroupDetailType() { Id = obj.TargetGroupDetailTypeId };
                targetGroupDetail.AgentObjects = new List<AgentObject>();
                targetGroupDetail.StudyYears = new List<StudyYear>();

             
                if (obj.AgentObjectIds != null)
                {
                    foreach (var id in obj.AgentObjectIds)
                    {
                        AgentObject_TargetGroup at = session.Query<AgentObject_TargetGroup>().Where(r => r.TargetGroupDetailId.Id == obj.Id && r.AgentObjectId.Id == id).SingleOrDefault(); 
                        if(at != null)
                        {
                            session.Update(at);
                        }
                        else
                        {
                            AgentObject ag = session.Query<AgentObject>().Where(r => r.Id == id).SingleOrDefault();
                            AgentObject_TargetGroup item = new AgentObject_TargetGroup();
                            {
                                item.Id = Guid.NewGuid();
                                item.AgentObjectId = new AgentObject() { Id = id };
                                item.TargetGroupDetailId = new TargetGroupDetail() { Id = obj.Id };
                                item.AgentObjectName = ag.Name;
                                item.TargetGroupDetailName = obj.Name;
                                item.TiTrong = 0;
                                session.Save(item);
                            };
                        }
                        //targetGroupDetail.AgentObjects.Add(new AgentObject() { Id = id });
                    }
                }
                if (obj.StudyYearIds != null)
                {
                    foreach (var id in obj.StudyYearIds)
                    {
                        targetGroupDetail.StudyYears.Add(new StudyYear () { Id = id });
                    }
                }

                //targetGroupDetail.TagetGroupDetail = new List<TargetGroupDetail>();
                //if (obj.TargetGroupDetailIds != null)
                //{
                //    foreach (var id in obj.TargetGroupDetailIds)
                //    {
                //        targetGroupDetail.TagetGroupDetail.Add(new TargetGroupDetail() { Id = id });
                //    }
                //}
                session.SaveOrUpdate(targetGroupDetail);
            });
            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            return targetGroupDetail;

        }

        [Authorize]
        [Route("")]
        public IEnumerable<TargetGroupDTO> GetListbyAgentObjectId(Guid agentObjectId)
        {
            List<TargetGroupDTO> result = new List<TargetGroupDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<TargetGroupDetail>().Where(a => a.AgentObjects.Any(ag=>ag.Id==agentObjectId)).Map<TargetGroupDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<TargetGroupDTO> GetListbyAgentObject()
        {
            List<TargetGroupDTO> result = new List<TargetGroupDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<TargetGroupDetail>().Map<TargetGroupDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<TargetGroupDTO> GetListbyGV()
        {
            List<TargetGroupDTO> result = new List<TargetGroupDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<TargetGroupDetail>().Where(r=>r.TargetGroupDetailType.Id == 0 || r.TargetGroupDetailType.Id == 4 || r.TargetGroupDetailType.Id == 5).Map<TargetGroupDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public TargetGroupDetail Delete(TargetGroupDetail obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
        }
    }
}
