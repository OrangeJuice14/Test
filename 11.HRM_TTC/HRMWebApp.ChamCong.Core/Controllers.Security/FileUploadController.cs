using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.IO.Compression;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class FileUploadController : Controller
    {
        public ActionResult UploadSource(IEnumerable<HttpPostedFileBase> files, string path, bool backup = false)
        {
            string result = "";
            try
            {
                // The Name of the Upload component is "files"
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