using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMWeb_Service.Utils;
using System.IO;

namespace HRMWeb_Service
{
    public class Helper
    {
        public static bool TrustTest(string publicKey, string token)
        {
            try
            {
                String secretKey = "pscvietnam@hoasua";
                if (token != MakeToken(publicKey, secretKey))
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private static string MakeToken(string publicKey, string secretKey)
        {
            return EncryptUtil.MD5Mix(publicKey ?? "" + secretKey ?? "");
        }

        public static void ErrorLog(string chuoilog, Exception ex)
        {
            try
            {
                var st = new System.Diagnostics.StackTrace(ex, true);
                int line = st.GetFrame(st.FrameCount - 1).GetFileLineNumber();
                //var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message != null ? ex.Message : "";
                chuoilog = DateTime.Now + ", " + chuoilog + ", Message: " + ex.ToString() + ", Line: " + line;
                StreamWriter f = null;
                try
                {
                    string path = HttpContext.Current.Server.MapPath("~/log.txt");
                    if (File.Exists(path))              //kiem tra xem file co ton tai ko
                    {
                        f = File.AppendText(path); //neu ko thi ghi noi vao cuoi file
                        f.WriteLine(chuoilog);
                    }
                    else
                    {
                        f = File.CreateText(path); //  neu file ko ton tai thi tao ra file moi
                        f.WriteLine(chuoilog);
                    }
                }
                finally
                {
                    if (f != null)
                        f.Close();
                }
            }
            catch { }
        }
    }
}