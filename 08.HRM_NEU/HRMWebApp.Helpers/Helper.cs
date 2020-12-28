using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HRMWebApp.Helpers
{
    public static class Helper
    {
        public static bool IsNumber(string pText)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(pText);
        }
        public static ContentResult ToJSON(this object obj)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                Error = (sender, args) =>
                { args.ErrorContext.Handled = true; }
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(obj, jsonSerializerSettings), ContentType = "application/json", ContentEncoding = Encoding.UTF8 };
        }

        public static ContentResult JsonSucess()
        {
            return new ContentResult { Content = JsonConvert.SerializeObject(new { success = "success", status_code = 200 }), ContentType = "application/json", ContentEncoding = Encoding.UTF8 };
        }

        public static void AddRange<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            IEnumerable<T> itemReverse = items.Reverse();
            foreach (var item in itemReverse)
            {
                stack.Push(item);
            }
        }

        public static T Map<T>(this object source)
        {
            Mapper.CreateMap(source.GetType(), typeof(T));
            T des = (T)Mapper.Map(source, source.GetType(), typeof(T));
            return des;
        }

        public static List<T> Map<T>(this IEnumerable<object> source)
        {
            Type sourceType = source.GetType().GetGenericArguments()[0];
            Mapper.CreateMap(sourceType, typeof(T));
            List<T> des = (List<T>)Mapper.Map(source, source.GetType(), typeof(List<T>));
            return des;
        }

        public static int RemoveAll<T>(this IList<T> list, Predicate<T> match)
        {
            int count = 0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (match(list[i]))
                {
                    ++count;
                    list.RemoveAt(i);
                }
            }
            return count;
        }


        public static string getMd5Hash(string input)
        {
            if (input == null)
                input = string.Empty;
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.ASCII.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("X2").ToLower());
            }
            return sBuilder.ToString();
        }

        public static void ErrorLog(string chuoilog, Exception ex)
        {
            try
            {
                //var st = new System.Diagnostics.StackTrace(ex, true);
                //int line = st.GetFrame(st.FrameCount - 1).GetFileLineNumber(); //can't get error line on live website
                //var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message != null ? ex.Message : "";
                chuoilog = DateTime.Now + ", " + chuoilog + ", Message: " + ex?.ToString();
                StreamWriter f = null;
                try
                {
                    string path = HttpContext.Current.Server.MapPath("~/log.txt");
                    if (File.Exists(path))              //kiem tra xem file co ton tai ko
                    {
                        f = File.AppendText(path); //neu co thi ghi noi vao cuoi file
                    }
                    else
                    {
                        f = File.CreateText(path); //  neu file ko ton tai thi tao ra file moi
                    }
                    f.WriteLine(chuoilog);
                    f.WriteLine();
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
