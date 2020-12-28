using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HRMWebApp.ChamCong.Core.WebAPI
{
    public class ApiHelper
    {
        //public static string APIURL = "http://ttcurm.psctelecom.com.vn/";
        public static string APIURL = "http://urm.ttcedu.vn/";
        public static async Task<T> Post<T>(string endpoint, object data)
        {
            using (var client = new HttpClient())
            {
                var httpContent = new StringContent(JsonConvert.SerializeObject(data));
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpContent.Headers.Add("Token", User._currentUser.Token ?? "");

                var response = client.PostAsync(endpoint, httpContent).Result;
                string content = await response.Content.ReadAsStringAsync();
                return await Task.Run(() => JsonConvert.DeserializeObject<T>(content));
            }
        }

        public static Task<T> Post_NotAsync<T>(string endpoint, object data)
        {
            using (var client = new HttpClient())
            {
                var httpContent = new StringContent(JsonConvert.SerializeObject(data));
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpContent.Headers.Add("Token", User._currentUser.Token ?? "");

                var response = client.PostAsync(endpoint, httpContent).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                return Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(content));
            }
        }

        public static T Get<T>(string endpoint)
        {
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(endpoint);
                request.Headers.Add("Token", User._currentUser.Token ?? "");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                return DeserializeFromStream<T>(response.GetResponseStream());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static T DeserializeFromStream<T>(Stream s)
        {
            using (StreamReader reader = new StreamReader(s))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                JsonSerializer ser = new JsonSerializer();
                return ser.Deserialize<T>(jsonReader);
            }
        }
    }
}
