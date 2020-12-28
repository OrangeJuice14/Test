using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using NHibernate.Linq;
using HRMWebApp.KPI.DB;
using HRMWebApp.Helpers;
using System.IO;
using System.Net.Http;
using System.Net;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class FileAttachmentApiController : ApiController
    {
        public IEnumerable<DepartmentDTO> GetList()
        {
            var result = new List<DepartmentDTO>();
            SessionManager.DoWork(session =>
            {
                Guid spkt = new Guid("E054A602-E077-444C-B843-E856D643CA7F");
                result = session.Query<Department>().Where(d => d.ParentDepartment.Id == spkt && d.GCRecord == null).OrderBy(d => d.Name).ToList().Map<DepartmentDTO>();
            });
            return result;
        }


        public IEnumerable<FileAttachmentDTO> GetListByPlanDetail(Guid Id)
        {
            var result = new List<FileAttachmentDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<FileAttachment>().Where(d => d.PlanKPIDetail.Id == Id).ToList().Map<FileAttachmentDTO>();
            });
            return result;
        }

        public IEnumerable<FileAttachmentDTO> GetListByResultDetail(Guid Id)
        {
            var result = new List<FileAttachmentDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<FileAttachment>().Where(d => d.ResultDetail.Id == Id).ToList().Map<FileAttachmentDTO>();
                result.ForEach(r => r.Name = r.Name + r.Extension);
            });
            return result;
        }


        public FileAttachmentDTO GetObj(Guid id)
        {
            var result = new FileAttachmentDTO();
            SessionManager.DoWork(session =>
            {
                FileAttachment planDetailFile = session.Query<FileAttachment>().SingleOrDefault(a => a.Id == id);
                if (planDetailFile != null)
                {
                    result = planDetailFile.Map<FileAttachmentDTO>();
                    //result.PlanKPIDetailId = planDetailFile.PlanKPIDetail.Id;
                }
            });
            return result;
        }

        public Guid Put(FileAttachment obj)
        {
            Guid result = Guid.Empty;
            try
            {
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
                            pld.StartTime = DateTime.Now;
                            pld.EndTime = DateTime.Now;
                            pld.CreateTime = DateTime.Now;

                            obj.CreationTime = DateTime.Now;
                            obj.Name = obj.Name;
                            obj.PlanKPIDetail = pld;
                            pld.FileAttachments = new List<FileAttachment>();
                            pld.FileAttachments.Add(obj);
                            session.Save(pld);
                            //obj.PlanKPIDetail = pld;
                        }
                        else
                        {
                            obj.CreationTime = DateTime.Now;                  
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

        public FileAttachment Delete(FileAttachment obj)
        {
            if (File.Exists(HttpContext.Current.Server.MapPath(obj.Path)))
                File.Delete(HttpContext.Current.Server.MapPath(obj.Path));
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }

       
    }
}
