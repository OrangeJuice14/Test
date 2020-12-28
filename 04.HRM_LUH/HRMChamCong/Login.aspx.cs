using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRMChamCong.Helper;
using HRMChamCong.Utility;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Configuration;
using HRMWeb_Service;
using HRMWeb_Service.Utils;
using System.Web.Security;
using System.Text;
using Newtonsoft.Json;

namespace HRMChamCong
{
    public partial class Login : System.Web.UI.Page
    {
        const string PublicKey = "HRMChamCong";
        const string SecretKey = "pscvietnam@hoasua";
        readonly string _token = EncryptUtil.MakeToken(PublicKey, SecretKey);
        readonly Service1 _service = new Service1();

        string googleClientID = ConfigurationManager.AppSettings["googleClientID"];
        string googleClientSecret = ConfigurationManager.AppSettings["googleClientSecret"];
        protected void Page_Load(object sender, EventArgs e)
        {
            DataBind();

            string error = Request.QueryString["error"];
            string code = Request.QueryString["code"];
            if (error == null && code != null)
            {
                string accessToken = ExchangeAuthorizationCode(code);
                if (accessToken != string.Empty)
                {
                    string email = GetEmail(accessToken);

                    var user = _service.WebUser_GetByEmail(PublicKey, _token, email);

                    if (user == null)
                    {
                        Response.Write("<script>alert('Email của bạn không tồn tại trong hệ thống! Vui lòng đăng xuất tài khoản email hiện tại rồi đăng nhập bằng tài khoản email khác.');</script>");
                        return;
                    }
                    var authTicket = new FormsAuthenticationTicket(user.UserName, true, 15);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
                    SessionHelper.Data(SessionKey.UserId, user.Oid);
                    SessionHelper.Data(SessionKey.ThongTinNhanVien, user.ThongTinNhanVien);
                    SessionHelper.Data(SessionKey.UserName, user.UserName);
                    Response.Redirect("/");
                }
            }
        }
        protected override void OnPreInit(EventArgs e)
        {
            QuickHelper.ChoseLoginLogo(this.logo);
            //QuickHelper.LoginEmail(this.loginEmail);
        }

        private string ExchangeAuthorizationCode(string code)
        {
            try
            {
                string uriGoogleCallBack = Request.Url.GetLeftPart(UriPartial.Authority) + "/Login.aspx";
                string redirectUrl = HttpUtility.UrlEncode(uriGoogleCallBack);
                var content = "code=" + code + "&client_id=" + googleClientID + "&client_secret=" + googleClientSecret + "&redirect_uri=" + redirectUrl + "&grant_type=authorization_code";
                var request = WebRequest.Create("https://accounts.google.com/o/oauth2/token");
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(content);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }
                var response = (HttpWebResponse)request.GetResponse();
                Stream responseDataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseDataStream);
                string responseData = reader.ReadToEnd();
                reader.Close();
                responseDataStream.Close();
                response.Close();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<Dictionary<string, string>>(responseData)["access_token"];
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        private string GetEmail(string accessToken)
        {
            try
            {
                var emailRequest = "https://openidconnect.googleapis.com/v1/userinfo?alt=json&access_token=" + accessToken;
                // Create a request for the URL.    
                var request = WebRequest.Create(emailRequest);
                // Get the response.    
                var response = (HttpWebResponse)request.GetResponse();
                // Get the stream containing content returned by the server.    
                var dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.    
                var reader = new StreamReader(dataStream);
                // Read the content.    
                var jsonString = reader.ReadToEnd();
                // Cleanup the streams and the response.    
                reader.Close();
                dataStream.Close();
                response.Close();
                var jsonObj = JObject.Parse(jsonString);
                return (string)jsonObj["email"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}