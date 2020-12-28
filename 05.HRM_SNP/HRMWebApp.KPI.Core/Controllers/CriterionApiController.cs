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
    public class CriterionApiController : ApiController
    {
        public IEnumerable<CriterionDTO> GetList()
        {
            List<CriterionDTO> result = new List<CriterionDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<Criterion>().Where(c=>c.PlanKPI==null).ToList().Map<CriterionDTO>();
            });
            return result;
        }
        

        public IEnumerable<CriterionType> GetCriterionTypeList()
        {
            List<CriterionType> result = new List<CriterionType>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<CriterionType>().ToList();
            });
            return result;
        }

        public CriterionDTO GetListByClass(Guid id)
        {
            CriterionDTO result = new CriterionDTO();
            SessionManager.DoWork(session =>
            {
                Criterion criterion = session.Query<Criterion>().SingleOrDefault(a => a.Id == id);
                result = criterion.Map<CriterionDTO>();
                result.TargetGroupDetail = new TargetGroupDetail() { Id = criterion.TargetGroupDetail.Id };
                result.CriterionType = new CriterionType() { Id = result.CriterionType != null ? result.CriterionType.Id : 0 };
            });
            return result;
        }

      

        public IEnumerable<CriterionDTO> GetListbyId(Guid classId)
        {
            List<CriterionDTO> result = new List<CriterionDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<Criterion>().Where(a => a.TargetGroupDetail.Id == classId).Map<CriterionDTO>();
            });
            return result;
        }
        public IEnumerable<CriterionDTO> GetSearch(Guid targetGroupDetailId)
        {
            List<CriterionDTO> result = new List<CriterionDTO>();
            SessionManager.DoWork(session =>
            {
                if (targetGroupDetailId==Guid.Empty)
                {
                    result = session.Query<Criterion>().OrderBy(a=>a.OrderNumber).Map<CriterionDTO>();
                }
                else
                {
                    result = session.Query<Criterion>().Where(a => a.TargetGroupDetail.Id == targetGroupDetailId).OrderBy(a => a.OrderNumber).Map<CriterionDTO>();
                }                
            });
            return result;
        }

        //public IEnumerable<CriterionDTO> GetSearch(Guid targetGroupDetailId, Guid departmentId)
        //{
        //    List<CriterionDTO> result = new List<CriterionDTO>();
        //    SessionManager.DoWork(session =>
        //    {
        //        if (targetGroupDetailId == Guid.Empty)
        //        {
        //            result = session.Query<Criterion>().Where(a => a.Department.Id == departmentId).OrderBy(a => a.OrderNumber).Map<CriterionDTO>();
        //        }
        //        else
        //        {
        //            result = session.Query<Criterion>().Where(a => a.TargetGroupDetail.Id == targetGroupDetailId && a.Department.Id == departmentId).OrderBy(a => a.OrderNumber).Map<CriterionDTO>();
        //        }

        //    });
        //    return result;
        //}

        public Criterion Put(Criterion obj)
        {
            if (obj.Id == Guid.Empty)
                obj.Id = Guid.NewGuid();
            SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            return obj;
        }

        //public bool GetCheckHasDictionary(Guid id)
        //{
        //    bool result = false;
        //    SessionManager.DoWork(session =>
        //    {
        //        List<CriterionDictionary> data = new List<CriterionDictionary>();
        //        data = session.Query<CriterionDictionary>().Where(c => c.Criterion.Id == id).ToList();
        //        if (data.Count!=0)
        //        {
        //            result = true;
        //        }
        //    });
        //    return result;
        //}

        public int Delete(Criterion obj)
        {
            try
            { 
                SessionManager.DoWork(session => session.Delete(obj));
                return 1;
            }
            catch (Exception Ex)
            {
                return 0;
            }
        }

        //Criterion Dictionary
        //public IEnumerable<CriterionDictionary> GetDictionnaryByCriterionId(Guid id)
        //{
        //    List<CriterionDictionary> result = new List<CriterionDictionary>();
        //    SessionManager.DoWork(session =>
        //    {
        //        result = session.Query<CriterionDictionary>().Where(c => c.Criterion.Id == id).OrderByDescending(c=>c.Record).ToList();
        //    });
        //    return result;
        //}

        public IEnumerable<CriterionDictionary> GetDictionnaryByTargetGroupDetailId(Guid id)
        {
            List<CriterionDictionary> result = new List<CriterionDictionary>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<CriterionDictionary>().Where(c =>c.TargetGroupDetail!=null && c.TargetGroupDetail.Id == id && c.LevelIndex==1).OrderByDescending(c => c.Record).ToList();
            });
            return result;
        }
        //public CriterionDictionaryDTO GetDictionary(Guid id)
        //{
        //    CriterionDictionaryDTO result = new CriterionDictionaryDTO();
        //    SessionManager.DoWork(session =>
        //    {
        //        CriterionDictionary crd = session.Query<CriterionDictionary>().SingleOrDefault(a => a.Id == id);
        //        result = crd.Map<CriterionDictionaryDTO>();
        //        result.CriterionId = crd.Criterion.Id;
                
        //    });
        //    return result;
        //}
        public CriterionDictionary PutDictionary(CriterionDictionary obj)
        {
            if (obj.Id == Guid.Empty)
                obj.Id = Guid.NewGuid();
            SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            return obj;
        }
        
        
        public CriterionDictionary DeleteDictionary(CriterionDictionary obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;           
        }
        /// <summary>
        /// Measure Unit
        /// </summary>
        /// <returns></returns>
        public int GetMaxId()
        {
            var result = new MeasureUnit();
            SessionManager.DoWork(session =>
            {
                result = session.Query<MeasureUnit>().OrderByDescending(s => s.Id).Take(1).SingleOrDefault();
            });
            return result.Id + 1;
        }
        public IEnumerable<MeasureUnitDTO> GetListMeasureUnit()
        {
            List<MeasureUnitDTO> result = new List<MeasureUnitDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<MeasureUnit>().ToList().Map<MeasureUnitDTO>();
            });
            return result;
        }
        public MeasureUnitDTO GetMeasureUnit(int id)
        {
            MeasureUnitDTO result = new MeasureUnitDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<MeasureUnit>().Where(m=>m.Id==id).SingleOrDefault().Map<MeasureUnitDTO>();
            });
            return result;
        }
        public int PutMeasureUnit(MeasureUnit obj)
        {
            if (obj.Id == 0)
                obj.Id = GetMaxId();
            SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            return 1;
        }
        public int DeleteMeasureUnit(MeasureUnit obj)
        {
            int success = 0;
            SessionManager.DoWork(session =>
                {
                    bool check=session.Query<PlanKPIDetail_KPI>().Any(p=>p.MeasureUnit.Id==obj.Id);
                    if (check==false)
                    {
                        session.Delete(obj);
                        success=1;
                    }
                });
            return success;
        }
        /// <summary>
        /// ManageCode
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ManageCode> GetListManageCode()
        {
            List<ManageCode> result = new List<ManageCode>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ManageCode>().ToList();
            });
            return result;
        }
        public ManageCode GetManageCode(string id)
        {
            ManageCode result = new ManageCode();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ManageCode>().Where(m => m.Id == id).SingleOrDefault();
            });
            return result;
        }
        public int PutNewManageCode(ManageCode obj)
        {
            int success = 0;
            SessionManager.DoWork(session =>
            {
                bool check = session.Query<ManageCode>().Any(p => p.Id == obj.Id);
                if (check == false)
                {
                    session.Save(obj);
                    success = 1;
                }
            });
            return success;
        }
        public int PutManageCode(ManageCode obj)
        {
            SessionManager.DoWork(session =>
            {
                    session.Update(obj);         
            });
            return 1;
        }
        public int DeleteManageCode(ManageCode obj)
        {
            int success = 0;
            SessionManager.DoWork(session =>
            {
                bool check = session.Query<PlanKPIDetail>().Any(p => p.ManageCode.Id == obj.Id);
                if (check == false)
                {
                    session.Delete(obj);
                    success = 1;
                }
            });
            return success;
        }

        /// <summary>
        /// TargetGroup 2,3 dictionary
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TargetGroupDetailDTO> GetListTargetGroupDetail()
        {
            List<TargetGroupDetailDTO> result = new List<TargetGroupDetailDTO>();
            SessionManager.DoWork(session =>
            {
                List<TargetGroupDetail> list = new List<TargetGroupDetail>();
                //Lấy nhóm 2 và 3 của phòng ban, nhân viên
                list = session.Query<TargetGroupDetail>().Where(t=>t.Id==new Guid("1FEC7670-B91E-4E24-AC68-45E270CECED3") || t.Id == new Guid("37A429BF-B53C-482E-AA33-9DDB17604558")).ToList();
                foreach (TargetGroupDetail t in list)
                {
                    TargetGroupDetailDTO td = new TargetGroupDetailDTO();
                    td.Id = t.Id;
                    td.Name = t.Name;
                    result.Add(td);
                }
            });
            return result;
        }
        public IEnumerable<CriterionDictionaryDTO> GetListDictionaryByTargetGroupDetailId(Guid id)
        {
            List<CriterionDictionaryDTO> result = new List<CriterionDictionaryDTO>();
            SessionManager.DoWork(session =>
            {
                List<CriterionDictionary> list = new List<CriterionDictionary>();
                //Lấy nhóm 2 và 3 của phòng ban, nhân viên
                list = session.Query<CriterionDictionary>().Where(c=>c.TargetGroupDetail.Id==id).OrderBy(c=>c.OrderNumber).ToList();
                foreach (CriterionDictionary c in list)
                {
                    CriterionDictionaryDTO cd = new CriterionDictionaryDTO();
                    cd.Id = c.Id;
                    cd.Name = c.Name;
                    cd.Record = c.Record;
                    cd.MaxRecord = c.MaxRecord;
                    result.Add(cd);
                }
            });
            return result;
        }
        public CriterionDictionary GetCriterionDictionary(Guid id)
        {
            CriterionDictionary result = new CriterionDictionary();
            SessionManager.DoWork(session =>
            {
                result = session.Query<CriterionDictionary>().Where(m => m.Id == id).SingleOrDefault();
            });
            return result;
        }
        public int PutCriterionDictionary(CriterionDictionary obj)
        {
            int result = 0;
            if (obj.Id == Guid.Empty)
                obj.Id = Guid.NewGuid();
            SessionManager.DoWork(session =>
            {
                session.SaveOrUpdate(obj);
                result = 1;
            });
            return result;
        }
        public int DeleteCriterionDictionary(CriterionDictionary obj)
        {
            int success = 0;
            SessionManager.DoWork(session =>
            {
                bool check = session.Query<PlanKPIDetail>().Any(p => p.CurrentKPI.ToString()==obj.Id.ToString());
                if (check == false)
                {
                    session.Delete(obj);
                    success = 1;
                }
            });
            return success;
        }

        //Configuration
        public IEnumerable<KPIConfiguration> GetListConfiguration()
        {
            List<KPIConfiguration> result = new List<KPIConfiguration>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<KPIConfiguration>().ToList();
            });
            return result;
        }
        public KPIConfiguration GetConfigurationById(int id)
        {
            KPIConfiguration result = new KPIConfiguration();
            SessionManager.DoWork(session =>
            {
                result = session.Query<KPIConfiguration>().SingleOrDefault(k=>k.Id==id);
            });
            return result;
        }
        public int PutConfiguration(KPIConfiguration obj)
        {
            int result = 0;
            SessionManager.DoWork(session =>
            {
                session.Update(obj);
                result = 1;
            });
            return result;
        }
    }
}
