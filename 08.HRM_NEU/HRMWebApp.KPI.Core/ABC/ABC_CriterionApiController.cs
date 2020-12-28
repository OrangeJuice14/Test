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
    public class ABC_CriterionApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_CriterionDTO> GetList()
        {
            List<ABC_CriterionDTO> result = new List<ABC_CriterionDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_Criterion>().Map<ABC_CriterionDTO>().OrderBy(c=>c.OrderNumber).ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public ABC_Criterion PutCriterion (ABC_Criterion obj)
        {
            if (obj.Id == Guid.Empty)
                obj.Id = Guid.NewGuid();
            SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            return obj;
        }

        [Authorize]
        [Route("")]
        public ABC_Criterion GetCriterionById(Guid id)
        {
            ABC_Criterion result = new ABC_Criterion();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_Criterion>().Where(c=>c.Id==id).SingleOrDefault();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_CriterionDetailDTO> GetListCriterionDetailByCriterionId(Guid id)
        {
            List<ABC_CriterionDetailDTO> result = new List<ABC_CriterionDetailDTO>();
            SessionManager.DoWork(session =>
            {
                var list = session.Query<ABC_CriterionDetail>().Where(c => c.ABC_Criterion.Id==id).OrderBy(c => c.OrderNumber).ToList();
                foreach (ABC_CriterionDetail cd in list)
                {
                    ABC_CriterionDetailDTO cdd = new ABC_CriterionDetailDTO();
                    cdd.Id = cd.Id;
                    cdd.ABC_CriterionDetailTypeId = cd.ABC_CriterionDetailType != null ? cd.ABC_CriterionDetailType.Id : 0;
                    cdd.ABC_CriterionId = cd.ABC_Criterion != null ? cd.ABC_Criterion.Id : Guid.Empty;
                    cdd.MaxRecord = cd.MaxRecord;
                    cdd.Name = cd.Name;
                    result.Add(cdd);
                }
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public ABC_CriterionDetailDTO PutCriterionDetail(ABC_CriterionDetailDTO obj)
        {
            SessionManager.DoWork(session =>
            {
                if (obj.Id == Guid.Empty)
                {
                    ABC_CriterionDetail objsave = new ABC_CriterionDetail();
                    objsave.Id = Guid.NewGuid();
                    objsave.ABC_Criterion = new ABC_Criterion() { Id = obj.ABC_CriterionId };
                    objsave.ABC_CriterionDetailType = new ABC_CriterionDetailType() {Id=obj.ABC_CriterionDetailTypeId };
                    objsave.MaxRecord = obj.MaxRecord;
                    objsave.Name = obj.Name;
                    objsave.OrderNumber = obj.OrderNumber;
                    session.Save(objsave);
                }
                else
                {
                    ABC_CriterionDetail objsave = session.Query<ABC_CriterionDetail>().Where(c => c.Id == obj.Id).SingleOrDefault();
                    objsave.MaxRecord = obj.MaxRecord;
                    objsave.Name = obj.Name;
                    objsave.OrderNumber = obj.OrderNumber;
                    objsave.ABC_CriterionDetailType = new ABC_CriterionDetailType() { Id = obj.ABC_CriterionDetailTypeId };
                    session.Update(objsave);
                }
            });
            return obj;
        }

        [Authorize]
        [Route("")]
        public ABC_CriterionDetailDTO GetCriterionDetailById(Guid id)
        {
            ABC_CriterionDetailDTO result = new ABC_CriterionDetailDTO();
            SessionManager.DoWork(session =>
            {
                ABC_CriterionDetail temp = session.Query<ABC_CriterionDetail>().Where(c => c.Id == id).SingleOrDefault();
                result.Id = temp.Id;
                result.ABC_CriterionDetailTypeId = temp.ABC_CriterionDetailType != null ? temp.ABC_CriterionDetailType.Id : 0;
                result.ABC_CriterionId = temp.ABC_Criterion != null ? temp.ABC_Criterion.Id : Guid.Empty;
                result.MaxRecord = temp.MaxRecord;
                result.Name = temp.Name;
                result.OrderNumber = temp.OrderNumber;
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public bool GetCheckHasCriterionDetail(Guid id)
        {
            bool result = false;
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_CriterionDetail>().Any(c => c.ABC_Criterion.Id == id);
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public int DeleteCriterionDetail(ABC_CriterionDetail obj)
        {
            int success = 0;
            SessionManager.DoWork(session =>
            {
                //bool check = session.Query<PlanKPIDetail>().Any(p => p.CurrentKPI.ToString() == obj.Id.ToString());
                //if (check == false)
                //{
                try
                {
                    session.Delete(obj);
                    success = 1;
                }
                    catch (Exception e)
                {
                    success = 0;
                }
                //}
            });
            return success;
        }
        [Authorize]
        [Route("")]
        public int DeleteCriterion(ABC_Criterion obj)
        {
            int success = 0;
            SessionManager.DoWork(session =>
            {
            try
            {
                session.Delete(obj);
                success = 1;
                }
                catch (Exception e)
                {
                    success = 0;
                }
            });
            return success;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_CriterionDetailType> GetListCriterionDetailType()
        {
            List<ABC_CriterionDetailType> result = new List<ABC_CriterionDetailType>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_CriterionDetailType>().ToList();
            });
            return result;
        }
    }
}
