using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMWebApp.KPI.DB.Entities;
using NHibernate.Linq;
using HRMWebApp.KPI.DB;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.IO.Compression;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class FileUploadController : Controller
    {
        // GET: FileUpload
        public ActionResult SavePlanDetailFile(IEnumerable<HttpPostedFileBase> files, Guid id, Guid planStaffId, Guid targetGroupDetailId)
        {
            // The Name of the Upload component is "files"
            SessionManager.DoWork(session =>
                    {
                        if (files != null)
                        {
                            int year = DateTime.Now.Year;
                            string filePath = ConfigurationManager.AppSettings["FilesPath"] + "/" + year;
                            if (!Directory.Exists(filePath))
                                Directory.CreateDirectory(Server.MapPath(filePath));

                            foreach (var file in files)
                            {
                                FileAttachment pf = new FileAttachment();
                                pf.Id = Guid.NewGuid();
                                pf.Name = Path.GetFileNameWithoutExtension(file.FileName);
                                pf.Extension = Path.GetExtension(file.FileName);
                                // Some browsers send file names with full path. This needs to be stripped.
                                var fileName = Path.GetFileName(file.FileName);
                                var actualFileName = string.Format("{0}{1}",pf.Id, Path.GetExtension(file.FileName));
                                var physicalPath = Path.Combine(Server.MapPath(filePath), actualFileName);
                                pf.Path = filePath + "/" + actualFileName;
                                //Save file xuống server
                                file.SaveAs(physicalPath);

                                if (id == Guid.Empty)
                                {
                                    //Thêm vào plandetail mới (parent)
                                    PlanKPIDetail pld = new PlanKPIDetail();
                                    pld.Id = Guid.NewGuid();
                                    pld.PlanStaff = new PlanStaff() { Id = planStaffId };
                                    pld.TargetGroupDetail = new TargetGroupDetail() { Id = targetGroupDetailId };
                                    pld.StartTime = DateTime.Now;
                                    pld.EndTime = DateTime.Now;
                                    pld.CreateTime = DateTime.Now;

                                    pf.CreationTime = DateTime.Now;
                                    pf.Name = pf.Name;
                                    pf.PlanKPIDetail = pld;
                                    pld.FileAttachments = new List<FileAttachment>();
                                    pld.FileAttachments.Add(pf);
                                    session.Save(pld);
                                    //obj.PlanKPIDetail = pld;
                                }
                                else
                                {
                                    pf.PlanKPIDetail = new PlanKPIDetail() { Id = id };
                                    pf.CreationTime = DateTime.Now;
                                    session.Save(pf);
                                }
                            }
                        }
                    });


            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult SaveResultDetailFile(IEnumerable<HttpPostedFileBase> files,  Guid resultDetailId)
        {
            // The Name of the Upload component is "files"
            SessionManager.DoWork(session =>
            {
                if (files != null)
                {
                    int year = DateTime.Now.Year;
                    string filePath = ConfigurationManager.AppSettings["FilesPath"] + "/" + year;
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(Server.MapPath(filePath));

                    foreach (var file in files)
                    {
                        FileAttachment pf = new FileAttachment();
                        pf.Id = Guid.NewGuid();
                        pf.Name = Path.GetFileNameWithoutExtension(file.FileName);
                        pf.Extension = Path.GetExtension(file.FileName);
                        // Some browsers send file names with full path. This needs to be stripped.
                        var fileName = Path.GetFileName(file.FileName);
                        var actualFileName = string.Format("{0}{1}", pf.Id, Path.GetExtension(file.FileName));
                        var physicalPath = Path.Combine(Server.MapPath(filePath), actualFileName);
                        pf.Path = filePath + "/" + actualFileName;
                        //Save file xuống server
                        file.SaveAs(physicalPath);

                        pf.Id = Guid.NewGuid();
                        pf.ResultDetail = new ResultDetail() { Id = resultDetailId };
                        pf.CreationTime = DateTime.Now;
                        session.Save(pf);
                        
                    }
                }
            });


            // Return an empty string to signify success
            return Content("");
        }

        public FileResult DownloadFile(Guid Id)
        {
            FileContentResult result = null;
            SessionManager.DoWork(session =>
            {
                FileAttachment fileSelected = session.Query<FileAttachment>().SingleOrDefault(f => f.Id == Id);
                var path = Server.MapPath(fileSelected.Path);

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = string.Format("{0}{1}", fileSelected.Name, fileSelected.Extension);
                result= File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);                         
            });

            return result;
        }

        public ActionResult UploadSource(IEnumerable<HttpPostedFileBase> files, string path, bool backup = false)
        {
            string result = "";
            try
            {
                // The Name of the Upload component is "files"
                SessionManager.DoWork(session =>
                {
                    if (files != null)
                    {
                        var now = DateTime.Now;
                        string filePath = "~/" + path;
                        if (!Directory.Exists(Server.MapPath(filePath)))
                            Directory.CreateDirectory(Server.MapPath(filePath));

                        foreach (var file in files)
                        {
                            // Some browsers send file names with full path. This needs to be stripped.
                            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                            var extension = Path.GetExtension(file.FileName);
                            var actualFileName = string.Format("{0}{1}", fileNameWithoutExtension, extension);
                            var physicalPath = Path.Combine(Server.MapPath(filePath), actualFileName);

                            //Remove readonly attribute if file exists.
                            if (System.IO.File.Exists(physicalPath))
                            {
                                try
                                {
                                    FileInfo fileInfo = new FileInfo(physicalPath);
                                    fileInfo.IsReadOnly = false;
                                }
                                catch { }
                            }
                            if (backup)
                            {
                                try
                                {
                                    ZipFile.CreateFromDirectory(Server.MapPath("~/bin"), Server.MapPath("~/") + "bin_" + now.Year + "" + now.Month.ToString().PadLeft(2, '0') + "" + now.Day.ToString().PadLeft(2, '0') + ".zip");
                                }
                                catch { }
                            }

                            file.SaveAs(physicalPath);

                            if (extension == ".zip")
                            {
                                using (ZipArchive archive = ZipFile.OpenRead(physicalPath))
                                {
                                    foreach (ZipArchiveEntry entry in archive.Entries)
                                    {
                                        string entryFullName = Path.Combine(Server.MapPath("~/"), entry.FullName);
                                        if (entry.Length > 0) //file
                                        {
                                            if (System.IO.File.Exists(entryFullName))
                                            {
                                                try
                                                {
                                                    FileInfo fileInfo = new FileInfo(entryFullName);
                                                    fileInfo.IsReadOnly = false;
                                                }
                                                catch { }
                                            }
                                            entry.ExtractToFile(entryFullName, true);
                                        }
                                        else //folder
                                        {
                                            if (!Directory.Exists(entryFullName))
                                                Directory.CreateDirectory(entryFullName);
                                        }
                                    }
                                }
                                if (System.IO.File.Exists(physicalPath))
                                {
                                    System.IO.File.Delete(physicalPath);
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            // Return an empty string to signify success
            return Content(result);
        }
    }
}