using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using System.Web.Http;
using HRMWebApp.Helpers;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_CauHinhApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public List<ABC_ClassificationSetDTO> GetListClassificationSet()
        {
            try
            {
                List<ABC_ClassificationSetDTO> result = new List<ABC_ClassificationSetDTO>();
                SessionManager.DoWork(session =>
                {
                    result = session.Query<ABC_ClassificationSet>().OrderByDescending(q => q.ThoiGianApDung).ToList().Map<ABC_ClassificationSetDTO>();
                });
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [Route("")]
        public List<ABC_ClassificationsDTO> GetClassificationBySet(Guid id)
        {
            try
            {
                List<ABC_ClassificationsDTO> result = new List<ABC_ClassificationsDTO>();
                SessionManager.DoWork(session =>
                {
                    var list = session.Query<ABC_Classifications>().Where(q => q.ABC_ClassificationSet.Id == id).OrderBy(q => q.MinRecord).ThenByDescending(q => q.Rank);
                    foreach (var l in list)
                    {
                        ABC_ClassificationsDTO item = new ABC_ClassificationsDTO()
                        {
                            Id = l.Id,
                            MinRecord = l.MinRecord,
                            MaxRecord = l.MaxRecord,
                            Rank = l.Rank,
                            IsEligible = l.IsEligible,
                            ABC_ClassificationSetId = l.ABC_ClassificationSet?.Id ?? Guid.Empty
                        };
                        result.Add(item);
                    }
                });
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [Route("")]
        public int PutClassification(ABC_ClassificationsDTO obj)
        {
            try
            {
                int result = 0;
                SessionManager.DoWork(session =>
                {
                    Guid id = Guid.Empty;
                    if (obj.Id == Guid.Empty)
                    {
                        ABC_Classifications newItem = new ABC_Classifications();
                        newItem.Id = Guid.NewGuid();
                        newItem.MinRecord = obj.MinRecord;
                        newItem.MaxRecord = obj.MaxRecord;
                        newItem.Rank = obj.Rank;
                        newItem.IsEligible = obj.IsEligible;
                        newItem.ABC_ClassificationSet = new ABC_ClassificationSet() { Id = obj.ABC_ClassificationSetId };
                        session.Save(newItem);
                        id = newItem.Id;
                    }
                    else
                    {
                        var objDB = session.Query<ABC_Classifications>().SingleOrDefault(q => q.Id == obj.Id);
                        objDB.MinRecord = obj.MinRecord;
                        objDB.MaxRecord = obj.MaxRecord;
                        objDB.Rank = obj.Rank;
                        objDB.IsEligible = obj.IsEligible;
                        objDB.ABC_ClassificationSet = new ABC_ClassificationSet() { Id = obj.ABC_ClassificationSetId };
                        session.Update(objDB);
                        id = objDB.Id;
                    }
                    if (obj.IsEligible == true)
                    {
                        var listOther = session.Query<ABC_Classifications>().Where(q => q.ABC_ClassificationSet.Id == obj.ABC_ClassificationSetId && q.Id != id);
                        foreach (var l in listOther)
                        {
                            l.IsEligible = false;
                            session.Update(l);
                        }
                    }
                    result = 1;
                });
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [Route]
        public int DeleteClassification(ABC_ClassificationsDTO item)
        {
            try
            {
                SessionManager.DoWork(session =>
                {
                    var obj = session.Query<ABC_Classifications>().SingleOrDefault(q => q.Id == item.Id);
                    if (obj != null)
                        session.Delete(obj);
                });
                return 1;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [Route("")]
        public int PutClassificationSet(ABC_ClassificationSetDTO obj)
        {
            obj.ThoiGianApDung = obj.ThoiGianApDung?.ToLocalTime();
            try
            {
                int result = 0;
                SessionManager.DoWork(session =>
                {
                    if (obj.Id == Guid.Empty)
                    {
                        ABC_ClassificationSet newItem = new ABC_ClassificationSet();
                        newItem.Id = Guid.NewGuid();
                        newItem.Name = obj.Name;
                        newItem.ThoiGianApDung = obj.ThoiGianApDung;
                        session.Save(newItem);
                    }
                    else
                    {
                        var objDB = session.Query<ABC_ClassificationSet>().SingleOrDefault(q => q.Id == obj.Id);
                        objDB.Name = obj.Name;
                        objDB.ThoiGianApDung = obj.ThoiGianApDung;
                        session.Update(objDB);
                    }
                    result = 1;
                });
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [Route]
        public int DeleteClassificationSet(ABC_ClassificationSetDTO item)
        {
            try
            {
                SessionManager.DoWork(session =>
                {
                    var obj = session.Query<ABC_ClassificationSet>().SingleOrDefault(q => q.Id == item.Id);
                    if (obj != null)
                        session.Delete(obj);
                });
                return 1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
