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
using System.Configuration;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ProfessorCriterionApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ProfessorCriterionDTO> GetList()
        {
            List<ProfessorCriterionDTO> result = new List<ProfessorCriterionDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ProfessorCriterion>().ToList().Map<ProfessorCriterionDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ProfessorCriterionDTO> GetListByTargetGroupDetailId(Guid targetId)
        {
            List<ProfessorCriterionDTO> result = new List<ProfessorCriterionDTO>();
            SessionManager.DoWork(session =>
            {
                List<ProfessorCriterion> cri = session.Query<ProfessorCriterion>().Where(p => p.TargetGroupDetail.Id == targetId).ToList();
                foreach (ProfessorCriterion c in cri)
                {
                    ProfessorCriterionDTO crd = new ProfessorCriterionDTO();
                    crd.Id = c.Id;
                    crd.Name = c.Name;
                    crd.NumberOfHour = c.NumberOfHour;
                    crd.Record = c.Record;
                    result.Add(crd);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<CriterionType> GetCriterionTypeList()
        {
            List<CriterionType> result = new List<CriterionType>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<CriterionType>().ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public ProfessorCriterionDTO GetListByClass(Guid id)
        {
            ProfessorCriterionDTO result = new ProfessorCriterionDTO();
            SessionManager.DoWork(session =>
            {
                ProfessorCriterion criterion = session.Query<ProfessorCriterion>().SingleOrDefault(a => a.Id == id);
                result = criterion.Map<ProfessorCriterionDTO>();
                result.TargetGroupDetail = new TargetGroupDetail() { Id = criterion.TargetGroupDetail.Id };
                result.CriterionType = new CriterionType() { Id = result.CriterionType != null ? result.CriterionType.Id : 0 };
            });
            return result;
        }



        [Authorize]
        [Route("")]
        public IEnumerable<ProfessorCriterionDTO> GetListbyId(Guid classId)
        {
            List<ProfessorCriterionDTO> result = new List<ProfessorCriterionDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ProfessorCriterion>().Where(a => a.TargetGroupDetail.Id == classId).Map<ProfessorCriterionDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ProfessorCriterionDTO> GetSearch(Guid targetGroupDetailId)
        {
            List<ProfessorCriterionDTO> result = new List<ProfessorCriterionDTO>();
            SessionManager.DoWork(session =>
            {
                if (targetGroupDetailId==Guid.Empty)
                {
                    result = session.Query<ProfessorCriterion>().OrderBy(a=>a.OrderNumber).Map<ProfessorCriterionDTO>();
                }
                else
                {
                    result = session.Query<ProfessorCriterion>().Where(a => a.TargetGroupDetail.Id == targetGroupDetailId).OrderBy(a => a.OrderNumber).Map<ProfessorCriterionDTO>();
                }                
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int Put(ProfessorCriterion obj)
        {
            if (obj.Id == Guid.Empty)
                obj.Id = Guid.NewGuid();
            int result = 1;
            try
            {
                SessionManager.DoWork(session =>
                {
                    TargetGroupDetail tg = session.Query<TargetGroupDetail>().Where(t => t.Id == obj.TargetGroupDetail.Id).SingleOrDefault();
                    if (tg.TargetGroupDetailType.Id == 4)
                    {
                        obj.CriterionType = new CriterionType();
                        obj.CriterionType.Id = 4;
                    }
                    session.SaveOrUpdate(obj);
                    result = 1;
                });
            }
           catch (Exception e)
            {
                result = 0;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public bool GetCheckHasDictionary(Guid id)
        {
            bool result = false;
            SessionManager.DoWork(session =>
            {
                List<CriterionDictionary> data = new List<CriterionDictionary>();
                data = session.Query<CriterionDictionary>().Where(c => c.ProfessorCriterion.Id == id).ToList();
                if (data.Count!=0)
                {
                    result = true;
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int Delete(ProfessorCriterion obj)
        {
            try
            {
                int result = 1;
                SessionManager.DoWork(session =>
                {
                    bool check = session.Query<PlanKPIDetail>().Any(p => p.FromProfessorCriterion.Id == obj.Id);
                    if (check==true)
                    {
                        result = 2;
                    }
                    else
                    {
                        session.Delete(obj);
                        result = 1;
                    }
                    
                });
                return result;
            }
            catch (Exception Ex)
            {
                return 0;
            }
        }

        //Criterion Dictionary
        [Authorize]
        [Route("")]
        public IEnumerable<CriterionDictionary> GetDictionnaryByCriterionId(Guid id)
        {
            List<CriterionDictionary> result = new List<CriterionDictionary>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<CriterionDictionary>().Where(c => c.ProfessorCriterion.Id == id).OrderByDescending(c=>c.Record).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<CriterionDictionary> GetDictionnaryByTargetGroupDetailId(Guid id)
        {
            List<CriterionDictionary> result = new List<CriterionDictionary>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<CriterionDictionary>().Where(c =>c.TargetGroupDetail!=null && c.TargetGroupDetail.Id == id).OrderByDescending(c => c.Record).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public CriterionDictionaryDTO GetDictionary(Guid id)
        {
            CriterionDictionaryDTO result = new CriterionDictionaryDTO();
            SessionManager.DoWork(session =>
            {
                CriterionDictionary crd = session.Query<CriterionDictionary>().SingleOrDefault(a => a.Id == id);
                result.Id = crd.Id;
                result.Name = crd.Name;
                result.NumberOfHour = crd.NumberOfHour;
                result.ManageCode = crd.ManageCode;
                result.LevelIndex = crd.LevelIndex;
                result.OrderNumber = crd.OrderNumber;
                result.Record = crd.Record;
                result.MaxRecord = crd.MaxRecord;
                result.CriterionId = crd.ProfessorCriterion.Id;
                string GiangDaySoTietChuan = ConfigurationManager.AppSettings["GiangDaySoTietChuan"];
                string ChatLuongGiangDay = ConfigurationManager.AppSettings["ChatLuongGiangDay"];
                if (result.CriterionId.ToString().ToUpper() == GiangDaySoTietChuan)
                {
                    result.Exception = 1;
                }
                if (result.CriterionId.ToString().ToUpper() == ChatLuongGiangDay)
                {
                    result.Exception = 2;
                }
                result.DataRecord = crd.DataRecord;
                result.DataMaxRecord = crd.DataMaxRecord;
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public CriterionDictionary PutDictionary(CriterionDictionary obj)
        {
            if (obj.Id == Guid.Empty)
                obj.Id = Guid.NewGuid();
            SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            return obj;
        }

        [Authorize]
        [Route("")]
        public CriterionDictionary DeleteDictionary(CriterionDictionary obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;           
        }   
      

    }
}
