using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HRMWebApp.KPI.Core.Security;
using HRMWebApp.Helpers;
using HRMWeb_Business.Model;
using HRMWeb_Service;
using Helper = HRMWebApp.Helpers.Helper;
using Microsoft.AspNet.Identity;
using NHibernate.Linq;
using Microsoft.Owin.Security;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.DB;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace HRMWebApp.KPI.Core.Security
{

    public class AuthenticationController : Controller
    {
        const string PublicKey = "HRMChamCong";
        const string SecretKey = "pscvietnam@hoasua";
        readonly string _token = EncryptUtil.MakeToken(PublicKey, SecretKey);
        readonly Service1 _service = new Service1();  //readonly Service1Client _service = new Service1Client();
        CustomUserManager PCustomUserManager = new CustomUserManager();
        string googleClientID = ConfigurationManager.AppSettings["googleClientID"];
        string googleClientSecret = ConfigurationManager.AppSettings["googleClientSecret"];

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return Helper.JsonSucess();
        }


        public async Task<int> Login(string userName, string password,bool isKPIs, string captchaString)
        {

            ApplicationUser model = new ApplicationUser() { UserName = userName, Password = password };
            CustomUserManager PCustomUserManager = new CustomUserManager();

            ApplicationUser user = new ApplicationUser();
            if (isKPIs)
            {
                string passwordmd5 = HRMWebApp.Helpers.Helper.getMd5Hash(password);
                SessionManager.DoWork(session =>
                {
                    KPI_WebUser userKPI = session.Query<KPI_WebUser>().SingleOrDefault(u => u.Name == userName && u.Password == passwordmd5);
                    if (userKPI != null)
                    {
                        user.Id = userKPI.Id.ToString();
                        user.UserName = userKPI.Name;
                        //user.HoVaTen = userKPI.Name;
                        user.IsKPIs = true;
                        SessionHelper.Data(SessionKey.IsKPIs, true);
                    }
                });
            }
            else
            {
                user = await PCustomUserManager.FindAsync(model.UserName, model.Password);
                SessionHelper.Data(SessionKey.IsKPIs, false);
            }
            if (user != null)
            {
                await SignInAsync(user, false);
                if (user.HoVaTen != null)
                    return 1;
                else
                    return 2;
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
                // If we got this far, something failed, redisplay form
                return 0;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);



            var identity = await PCustomUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            //var identity = await UserManager1.CreateAsync(user);//, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }


        public ActionResult GetUserSessionInfo()
        {
            ApplicationUser result = new ApplicationUser();
            string userName = User.Identity.Name;
            string currentUserId = User.Identity.GetUserId();
            
            if (currentUserId != null)
                result = AuthenticationHelper.GetUserById(new Guid(currentUserId), userName);


            //Dictionary<string, object> result = new Dictionary<string, object>();


            //result["UserId"] = Session[SessionKey.UserId.ToString()];
            //result["ThongTinNhanVien"] = Session[SessionKey.ThongTinNhanVien.ToString()];
            //result["UserName"] = Session[SessionKey.UserName.ToString()];
            //result["HoVaTen"] = Session[SessionKey.HoVaTen.ToString()];
            //result["WebGroupId"] = Session[SessionKey.WebGroupId.ToString()];
            return result.ToJSON();
        }

        private async Task RequestToGetInputReport()
        {
            // lots of code prior to this
            Response.Redirect("/");
        }
        public void Login_Gmail()
        {
            string uriGoogleCallBack = Request.Url.GetLeftPart(UriPartial.Authority) + "/Authentication/Login_GmailCallback";
            ActionResult action = new EmptyResult();
            string Url = "https://accounts.google.com/o/oauth2/auth?";
            StringBuilder UrlBuilder = new StringBuilder(Url);
            UrlBuilder.Append("client_id=" + googleClientID);
            UrlBuilder.Append("&redirect_uri=" + uriGoogleCallBack);
            UrlBuilder.Append("&response_type=code");
            UrlBuilder.Append("&scope=" + "https://www.googleapis.com/auth/userinfo.email");
            Response.Redirect(UrlBuilder.ToString(), false);
        }
        public async Task<ActionResult> Login_GmailCallback(string error, string code)
        {
            string html = "<h3>Email của bạn không tồn tại trong hệ thống! Vui lòng <a style='color: blue' href='https://www.google.com/accounts/Logout?continue=https://appengine.google.com/_ah/logout?continue=" + Request.Url.GetLeftPart(UriPartial.Authority) + "'>đăng xuất tài khoản email</a> hiện tại rồi đăng nhập bằng tài khoản email khác.</h3>";
            if (error == null && code != null)
            {
                string accessToken = ExchangeAuthorizationCode(code);
                if (accessToken != string.Empty)
                {
                    string email = GetEmail(accessToken);

                    var user = _service.WebUser_GetByEmail(PublicKey, _token, email);

                    if (user == null)
                    {
                        return Content(html, "text/html");
                    }
                    ApplicationUser applicationUser = new ApplicationUser();
                    applicationUser.UserName = user.UserName;
                    applicationUser.HoVaTen = user.HoVaTen;
                    applicationUser.Id = user.Oid.ToString();
                    await SignInAsync(applicationUser, false);
                    return RedirectToLocal("/");
                }
            }
            return Content(html, "text/html");
        }
        private string ExchangeAuthorizationCode(string code)
        {
            try
            {
                string uriGoogleCallBack = Request.Url.GetLeftPart(UriPartial.Authority) + "/Authentication/Login_GmailCallback";
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
                Helper.ErrorLog("GetEmail: access_token=" + accessToken, ex);
                throw ex;
            }
        }

    }
}
