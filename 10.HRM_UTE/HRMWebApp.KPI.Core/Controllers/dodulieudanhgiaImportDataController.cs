using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class dodulieudanhgiaImportDataController : Controller
    {

        public ActionResult SaveFileToData(IEnumerable<HttpPostedFileBase> files)
        {
            int result = 0;
            string ConnectionStringName = "PSCPortalConnectionString";
            // The Name of the Upload component is "files"
            SessionManager.DoWork(session =>
            {
                if (files != null)
                {
                    int year = DateTime.Now.Year;
                    if (files != null && files.Count() > 0)
                    {
                        HttpPostedFileBase file = files.FirstOrDefault();
                        if (file != null)
                        {
                            FileInfo finfo = new FileInfo(file.FileName);
                            if (finfo.Extension == ".xlsx" || finfo.Extension == ".xls")
                            {
                                string filePath = ConfigurationManager.AppSettings["FilesPath"] + "/" + year;
                                if (!Directory.Exists(filePath))
                                    Directory.CreateDirectory(Server.MapPath(filePath));

                                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                                var actualFileName = string.Format("{0}{1}", fileName, Path.GetExtension(file.FileName));
                                var physicalPath = Path.Combine(Server.MapPath(filePath), actualFileName);
                                //Save file xuống server
                                file.SaveAs(physicalPath);

                                Imports._connectionString = ConnectionStringName;
                                Imports._directFile = physicalPath;
                                result = Imports.ImportDataGiangDay(session);

                                foreach (var fil in files)
                                {
                                    FileGiangDay pf = new FileGiangDay();
                                    pf.Id = Guid.NewGuid();
                                    pf.Name = Path.GetFileNameWithoutExtension(file.FileName);
                                    pf.Path = filePath + "/" + actualFileName;
                                    pf.Extension = Path.GetExtension(file.FileName);
                                    pf.CreationTime = DateTime.Now;
                                    session.Save(pf);

                                    List<DoDuLieuDanhGia> dldg = session.Query<DoDuLieuDanhGia>().ToList();
                                    foreach(DoDuLieuDanhGia item in dldg)
                                    {
                                        item.Path = new FileGiangDay() { Id = pf.Id };
                                        session.SaveOrUpdate(item);
                                    }
                                }
                            }
                            
                        }
                    }

                }
            });

            // Return an empty string to signify success
            return Content(result.ToString());
        }
    }
}
