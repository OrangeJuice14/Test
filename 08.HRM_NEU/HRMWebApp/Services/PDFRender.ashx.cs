using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace HRMWebApp.Services
{
    /// <summary>
    /// Summary description for PDFRender
    /// </summary>
    public class PDFRender : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string ftpPath = context.Request.Params["path"] == null
                                ? null
                                : context.Request.Params["path"].ToString();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = WebRequest.Create(ftpPath);
                request.Credentials = new NetworkCredential("erp", "chc5v5y3qgk140q9905895wa8u2bbp");
                var response = request.GetResponse();
                var ftpStream = response.GetResponseStream();
                byte[] imageBytes = ReadFully(ftpStream);
                response.Close();
                ftpStream.Flush();
                ftpStream.Close();

                context.Response.Buffer = true;
                context.Response.Charset = "";
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.ContentType = "application/pdf";
                context.Response.AddHeader("content-disposition", "inline;filename=file.pdf");
                context.Response.BinaryWrite(imageBytes);
            }
            catch (Exception ex)
            {
                Helpers.Helper.ErrorLog("PDFRender.ashx?path=" + ftpPath, ex);
                throw ex;
            }
        }

        public static byte[] ReadFully(Stream input)
        {
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