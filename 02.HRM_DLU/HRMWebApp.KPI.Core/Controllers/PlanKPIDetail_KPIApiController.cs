using System;
using System.Collections.Generic;
using System.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class PlanKPIDetail_KPIApiController : ApiController
    {
        


        public IEnumerable<PlanKPIDetail_KPIDTO> GetListByPlanDetail(Guid Id)
        {
            var result = new List<PlanKPIDetail_KPIDTO>();
            SessionManager.DoWork(session =>
            {
                List<PlanKPIDetail_KPI> list = session.Query<PlanKPIDetail_KPI>().Where(d => d.PlanKPIDetail.Id == Id).OrderBy(d => d.Name).ToList();
                foreach (PlanKPIDetail_KPI kpi in list)
                {
                    PlanKPIDetail_KPIDTO k = new PlanKPIDetail_KPIDTO();
                    k.Id = kpi.Id;
                    k.Name = kpi.Name;
                    k.PlanKPIDetailId = kpi.PlanKPIDetail != null ? kpi.PlanKPIDetail.Id : Guid.Empty;
                    k.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnitDTO() { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                    k.MeasureUnitId = kpi.MeasureUnit != null ? kpi.MeasureUnit.Id:0;
                    k.MeasureUnitName= kpi.MeasureUnit != null ? kpi.MeasureUnit.Name : "";
                    result.Add(k);
                }
            });
            return result;
        }

        public PlanKPIDetail_KPIDTO GetObj(Guid id)
        {
            var result = new PlanKPIDetail_KPIDTO();
            SessionManager.DoWork(session =>
            {
                PlanKPIDetail_KPI kpi = session.Query<PlanKPIDetail_KPI>().SingleOrDefault(a => a.Id == id);
                if(kpi != null)
                {
                    result.Id = kpi.Id;
                    result.Name = kpi.Name;
                    result.PlanKPIDetailId = kpi.PlanKPIDetail != null ? kpi.PlanKPIDetail.Id : Guid.Empty;
                    result.MeasureUnit = kpi.MeasureUnit != null ? new MeasureUnitDTO() { Id = kpi.MeasureUnit.Id, Name = kpi.MeasureUnit.Name } : null;
                    result.MeasureUnitId = kpi.MeasureUnit != null ? kpi.MeasureUnit.Id : 0;
                    result.MeasureUnitName = kpi.MeasureUnit != null ? kpi.MeasureUnit.Name : "";
                }                                  
            });
            return result;
        }

        public Guid Put(PlanKPIDetail_KPI obj)
        {
            Guid result = Guid.Empty;
            try { 
            SessionManager.DoWork(session =>
            {

                if (obj.Id == Guid.Empty)
                {
                    obj.Id = Guid.NewGuid();
                    if (obj.PlanKPIDetail.Id == Guid.Empty)
                    {
                        //Thêm vào plandetail mới (parent)
                        PlanKPIDetail pld = new PlanKPIDetail();
                        pld.Id = Guid.NewGuid();
                        pld.PlanStaff = new PlanStaff() { Id = obj.PlanKPIDetail.PlanStaff.Id };
                        pld.TargetGroupDetail = obj.PlanKPIDetail.TargetGroupDetail;
                        pld.CreateTime = DateTime.Now;
                        obj.PlanKPIDetail = pld;
                        pld.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPI>();
                        pld.PlanKPIDetail_KPIs.Add(obj);
                        session.Save(pld);
                        //obj.PlanKPIDetail = pld;
                    }
                    else
                    {                      
                        session.Save(obj);
                    }
                }
                else
                {
                    session.SaveOrUpdate(obj);
                }
                result = obj.PlanKPIDetail.Id;
            });
            }
            catch (Exception e)
            {
                result = Guid.Empty;
            }
            return result;
        }

        public PlanKPIDetail_KPI Delete(PlanKPIDetail_KPI obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }
    }
}
