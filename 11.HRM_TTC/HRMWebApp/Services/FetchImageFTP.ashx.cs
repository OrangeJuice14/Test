using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace HRMWebApp.Services
{
    /// <summary>
    /// Summary description for FetchImageFTP
    /// </summary>
    public class FetchImageFTP : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string ftpPath = context.Request.Params["path"] == null
                            ? null
                            : context.Request.Params["path"].ToString();
            //var webClient = new WebClient();
            //webClient.Proxy = null;
            //webClient.Credentials = new NetworkCredential("erpftp", "ttcedu");
            //byte[] imageBytes = webClient.DownloadData(ftpPath);

            var request = (FtpWebRequest)WebRequest.Create(ftpPath);
            request.Credentials = new NetworkCredential("erpftp", "ttcedu");
            request.KeepAlive = true;
            request.ServicePoint.ConnectionLimit = 4;
            var response = request.GetResponse();
            var ftpStream = response.GetResponseStream();
            byte[] imageBytes = ReadFully(ftpStream);

            context.Response.Buffer = true;
            context.Response.Charset = "";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.ContentType = "image/jpg";
            context.Response.AddHeader("content-disposition", "inline;filename=Image.jpg");
            context.Response.BinaryWrite(imageBytes);
        }

        public static byte[] ReadFully(Stream input)
        {
            // cách 1
            //byte[] buffer = new byte[16 * 1024];
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    int read;
            //    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        ms.Write(buffer, 0, read);
            //    }
            //    return ms.ToArray();
            //}

            // cách 2
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}