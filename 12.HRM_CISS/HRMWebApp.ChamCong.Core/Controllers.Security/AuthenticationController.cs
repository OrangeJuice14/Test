using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HRMWebApp.Helpers;
using HRMWeb_Business.Model;
using HRMWeb_Service;
using Helper = HRMWebApp.Helpers.Helper;
using Microsoft.AspNet.Identity;
using NHibernate.Linq;
using Microsoft.Owin.Security;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using HRMWeb_Business.Predefined;
using System.Web.Script.Serialization;
using HRMWebApp.ChamCong.Core.WebAPI;
using HRMWebApp.ChamCong.Core.WebAPI.Models;
using HRMWebApp.ChamCong.Core.DTO;

namespace HRMWebApp.ChamCong.Core.Controllers
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
            try
            {
                var cookie = Request.Cookies["User"];
                cookie.Values.Remove("User");
                Response.Cookies.Add(cookie);
            }
            catch (Exception ex) { }
            //
            AuthenticationManager.SignOut();
            return Helper.JsonSucess();
        }

        bool LoginDomain(string username, string password)
        {
            //
            string domainLogin = ConfigurationUtil.ReadAppSetting("DomainLogin");
            //
            if (CheckDomain(username, password, domainLogin))
            {
                //
                return true;
            }
            //
            return false;
        }
        bool CheckDomain(string username, string pass, string doamin)
        {
            bool result = false;
            //
            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, doamin))
                {
                    //
                    result = pc.ValidateCredentials(username, pass);
                }
            }
            catch (Exception ex)
            {

            }
            //
            return result;
        }

        public async Task<int> Login_New(string userName, string password, bool isKPIs, string captchaString)
        {
            //
            //
            Dictionary<string, object> postData = new Dictionary<string, object>();
            postData.Add("type", "password");
            postData.Add("username", userName);
            postData.Add("password", password);
            postData.Add("appid", "HETHONGERP");
            postData.Add("hours", 24);
            //
            var json = new JavaScriptSerializer().Serialize(postData);
            //
            var result = ApiHelper.Post_NotAsync<URMUser>(ApiHelper.APIURL + "api/User/Login", postData).Result;
            if (result.Success)
            {
                //
                ApplicationUser model = new ApplicationUser() { UserName = userName, Password = password };
                CustomUserManager PCustomUserManager = new CustomUserManager();

                ApplicationUser user = new ApplicationUser();
                //
                user = await PCustomUserManager.FindAsync(model.UserName, model.Password);
                SessionHelper.Data(SessionKey.IsKPIs, false);
                if (user != null)
                {
                    //
                    String idTaiKhoanCaNhan = WebGroupConst.TaiKhoanCaNhanID.ToString().ToUpper();
                    await SignInAsync(user, false);
                    // 
                    #region 1. Tạo cookie sử dụng UIS và HRM (Quan trọng)

                    HttpCookie cookie = new HttpCookie("User");
                    cookie["User"] = user.UserName;
                    cookie["Domain"] = "psctelecom.com.vn";
                    cookie.Expires = DateTime.Now.AddDays(1);
                    //
                    Response.Cookies.Add(cookie);
                    /*
                    Response.Cookies["User"].Value = user.UserName;
                    Response.Cookies["User"].Domain = "psctelecom.com.vn";*/
                    #endregion
                    //
                    #region 2. Trả dữ liệu cho client
                    if (user.WebGroupId.ToUpper().Equals(idTaiKhoanCaNhan))
                        return 1;
                    else
                        return 2;
                    #endregion
                    //

                }
                else
                {
                    ModelState.AddModelError("", "Kiểm tra tên và mật khẩu.");
                    //
                    return 0;
                }
            }
            else
            {
                ModelState.AddModelError("", "Kiểm tra tên và mật khẩu trên Phân quyền tổng.");
                //
                return 0;
            }
        }

        public async Task<int> Login(string userName, string password, bool isKPIs, string captchaString)
        {
            string maDonViSuDung = ConfigurationUtil.ReadAppSetting("MaDonViSuDung");
            //
            //Kiểm tra tài khoản đăng nhập có đúng với Ldap hay không
            if (maDonViSuDung.Equals(""))
            {
                if (!LoginDomain(userName, password))
                {
                    return 3;
                }
            }
            //
            ApplicationUser model = new ApplicationUser() { UserName = userName, Password = password };
            CustomUserManager PCustomUserManager = new CustomUserManager();

            ApplicationUser user = new ApplicationUser();
            //
            user = await PCustomUserManager.FindAsync(model.UserName, model.Password);
            SessionHelper.Data(SessionKey.IsKPIs, false);
            //
            if (user != null)
            {
                String idTaiKhoanCaNhan = WebGroupConst.TaiKhoanCaNhanID.ToString().ToUpper();
                await SignInAsync(user, false);

                #region 1. Tạo cookie sử dụng UIS và HRM (Quan trọng)
                HttpCookie cookie = new HttpCookie("User");
                cookie["User"] = user.UserName;
                cookie["Domain"] = "psctelecom.com.vn";
                cookie.Expires = DateTime.Now.AddDays(1);
                //
                Response.Cookies.Add(cookie);
               /*
                Response.Cookies["User"].Value = user.UserName;
                Response.Cookies["User"].Domain = "psctelecom.com.vn";*/
                #endregion
                //
                #region 2. Trả dữ liệu cho client
                if (user.WebGroupId.ToUpper().Equals(idTaiKhoanCaNhan))
                    return 1;
                else
                    return 2;
                #endregion

            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
                //
                return 0;
            }

        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //
            var identity = await PCustomUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            //
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
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
            string html = "<h3>Email của bạn không tồn tại trong hệ thống! Vui lòng <a style='color: blue' href='https://www.google.com/accounts/Logout?continue=https://appengine.google.com/_ah/logout?continue=" + Request.Url.GetLeftPart(UriPartial.Authority) + "'>đăng xuất tài khoản email</a> hiện tại rồi đăng nhập bằng tài khoản email được cung cấp.</h3>";
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
                    applicationUser.CongTyId = user.CongTyId.ToString();
                    applicationUser.WebGroupId = user.WebGroupID.ToString();
                    applicationUser.AgentObjectTypeId = user.AgentObjectTypeId.ToString();
                    applicationUser.DepartmentId = user.BoPhanId == null ? Guid.Empty.ToString() : user.BoPhanId.ToString();
                    //
                    await SignInAsync(applicationUser, false);

                    #region 1. Tạo cookie sử dụng UIS và HRM (Quan trọng)
                    HttpCookie cookie = new HttpCookie("User");
                    cookie["User"] = user.UserName;
                    cookie["Domain"] = "psctelecom.com.vn";
                    cookie.Expires = DateTime.Now.AddDays(1);
                    //
                    Response.Cookies.Add(cookie);
                    #endregion

                    //
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

        public ActionResult GetUserSessionInfo()
        {
           
            #region Kiểm tra thông tin tài khoản
            ApplicationUser result = new ApplicationUser();
            //
            string currentUserId = User.Identity.GetUserId();
            //
            if (!String.IsNullOrEmpty(currentUserId))
            {
                DTO_WebUser user = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(currentUserId));
                //
                if (user != null)
                {
                    result.UserId = user.Oid.ToString();
                    result.Id = user.ThongTinNhanVien == null ? Guid.Empty.ToString() : user.ThongTinNhanVien.ToString();
                    result.UserName = user.UserName;
                    result.HoVaTen = user.HoVaTen;
                    result.WebGroupId = user.WebGroupID.ToString();
                    result.AgentObjectTypeId = user.AgentObjectTypeId.ToString();
                    result.DepartmentId = user.BoPhanId == null ? Guid.Empty.ToString() : user.BoPhanId.ToString();
                    result.Email = user.Email;
                    result.CongTyId = user.CongTyId.ToString();

                    //Lưu ý Quan trọng: Lấy công ty hiện tại của user
                    WebServicesController._congTy = user.CongTyId.Value;
                    //
                }
            }
            #endregion

            //
            return result.ToJSON();
        }

        public ActionResult GetUserCookieInfo()
        {
            bool isExist = false;
            //
            try
            {
                var cookieId = Request.Cookies["User"];
                //
                if (cookieId != null && !string.IsNullOrEmpty(cookieId.Value))
                {
                    //
                    isExist = true;
                }
            }
            catch (Exception ex) { }
            //
            if (isExist)
            {
                return Helper.JsonExists();
            }
            else
            {
                return Helper.JsonErorr();
            }
        }
    }
}
